# Phase 8 Research — Full-lesson draft orchestration

**Phase:** 8  
**Goal:** Teachers can generate and refine a previewable full-lesson draft made of ordered activity blocks inside the existing course editor  
**Researched:** 2026-04-11  
**Confidence:** HIGH

## Executive Summary

Phase 8 should extend the existing `teacher/courseedit.aspx` orchestration surface instead of introducing a new teacher page. The current shipped flow already supports single activity-plan generation, section-level regeneration, saved draft resume, append-only apply, and publish delegation through `teacher/aiprovider_api.ashx`, `js/courseedit.js`, and `CourseActivityPlanDraft`. Phase 8 should preserve that operating model while upgrading the draft contract from section-based prose to an ordered list of typed lesson blocks.

The safest implementation path for this phase is to stop at **previewable full-lesson draft orchestration**: define a typed full-lesson draft contract, expose generation/load/save/block-regeneration/remove endpoints parallel to the current `activityPlan*` actions, and upgrade the course editor preview from section cards to ordered block cards. Do **not** couple Phase 8 to multi-entity publish fan-out yet; publishing adapters for quiz / ware / inquiry belong to later phases. Phase 8 only needs enough persistence and UI state to satisfy ORCH-01/02/03 without reducing teacher control.

## Existing Baseline To Reuse

### Teacher entry and preview shell

- `teacher/courseedit.aspx` already hosts the AI panel, saved-draft banner, preview area, apply buttons, and publish toggle.
- `js/courseedit.js` already manages client-side draft state, draft resume/delete prompts, section-level regenerate, append-only apply, and progress messaging.
- `teacher/courseedit.aspx.cs` keeps the page focused on course editor state and persists lesson content separately via `Btnedit_Click`.

### Server AI boundary

- `teacher/aiprovider_api.ashx` already owns teacher auth, request validation, `activityPlan`, `activityPlanRegenerateSection`, `activityPlanSaveDraft`, `activityPlanLoadDraft`, `activityPlanDraftStatus`, and `activityPlanPublish`.
- The current pattern is: parse request -> validate contract -> delegate to BLL -> serialize DTO back to the editor.

### Draft persistence contract

- `App_Code/Model/CourseActivityPlanDraft.cs` stores one current draft per course and teacher.
- `App_Code/Dal/CourseActivityPlanDraft.cs` upserts draft payload by `Cid`/`Hid`.
- `App_Code/Common/AIActivityPlanSavedDraftHelper.cs` serializes/deserializes the saved JSON envelope.

### Current limitation that Phase 8 must solve

- `App_Code/Common/AIActivityPlanDraftHelper.cs` models a single activity plan as sections (`teachingGoals`, `activitySteps`, `resources`, `assessment`, `teacherReminder`).
- `js/courseedit.js` preview logic is built around section cards and `activityPlanSectionStates`.
- `AIActivityPlanPublisher` and its tests are intentionally pinned to a single `Mission + ListMenu(Ltype=1)` publish path, which is too narrow for full-lesson orchestration.

## Recommended Technical Direction For Phase 8

### 1. Introduce a full-lesson draft contract

Add a new contract parallel to the existing section draft instead of mutating the old one in place.

Recommended logical shape:

```json
{
  "schemaVersion": "v1.2-full-lesson",
  "topic": "分数的初步认识",
  "lessonSummary": "...",
  "totalMinutes": "40分钟",
  "blocks": [
    {
      "blockKey": "opening-1",
      "sort": 1,
      "blockType": "mission|ware|quiz|inquiry|unknown",
      "title": "情境导入",
      "minutes": "5",
      "teachingPurpose": "激活先验知识",
      "lessonPosition": "导入",
      "teacherAction": "...",
      "studentAction": "...",
      "materials": ["..."],
      "assessmentFocus": "...",
      "status": "draft"
    }
  ]
}
```

Rules:

- `blocks[]` must be ordered and each block needs a stable `blockKey`.
- Each block must expose `blockType`, `teachingPurpose`, and `lessonPosition` because ORCH-02 requires these in preview.
- `unknown` should not reach publish, but it can exist in preview if later phases map it to guided inquiry fallback.

### 2. Add parallel `fullLesson*` handler actions

Keep the single-activity `activityPlan*` flow intact and add a separate set of actions in `teacher/aiprovider_api.ashx`:

- `fullLessonGenerate`
- `fullLessonRegenerateBlock`
- `fullLessonDraftStatus`
- `fullLessonSaveDraft`
- `fullLessonLoadDraft`
- `fullLessonDeleteDraft`

This avoids breaking shipped v1.0/v1.1 regression locks while giving Phase 8 its own typed contract.

### 3. Upgrade preview to ordered block cards

`teacher/courseedit.aspx` and `js/courseedit.js` should evolve from section preview to block preview:

- render one card per lesson block
- show `blockType`, `teachingPurpose`, `lessonPosition`, `minutes`
- allow block-level remove
- allow block-level regenerate
- preserve the rest of the draft when one block changes

The preview should remain **preview-first** and **teacher-controlled**. No automatic writeback to `mcontent`, no automatic student publish.

### 4. Keep one current draft per course

Phase 8 can continue using one current saved draft per course, but `DraftJson` must store the new full-lesson payload. Existing `LinkedMissionId` / `LinkedListMenuId` fields are insufficient for future multi-block publish, but they do not block Phase 8 if publish fan-out is deferred.

## Scope Boundaries For Phase 8

### In scope

- ORCH-01: generate multiple ordered blocks instead of one plain-text activity
- ORCH-02: preview block type, teaching purpose, and lesson position
- ORCH-03: remove or regenerate one block while preserving the rest of the draft
- save/load/delete full-lesson draft state in the current course editor

### Out of scope for this phase

- multi-type publish fan-out to `Mission` / `Exam` / `TxtForm` / `Ware`
- block link child table
- combined publish transaction
- student runtime changes
- drag-and-drop reorder UI
- AI rationale/explanation text beyond required preview metadata

## Risks And Mitigations

### Risk 1: Reusing section-based schema for full-lesson blocks

If the implementation keeps encoding blocks inside `activitySteps` or other prose fields, block-level remove/regenerate will be brittle.

**Mitigation:** create dedicated full-lesson DTO/helper classes and separate `fullLesson*` actions.

### Risk 2: Preview and persistence use different shapes

If the browser invents its own block shape while the server stores another shape, resume and regenerate will drift.

**Mitigation:** the server should own the full-lesson JSON contract; the browser only renders the returned DTO.

### Risk 3: Web Forms postback pollution

Trying to model lesson blocks as dynamic server controls will be fragile.

**Mitigation:** keep draft orchestration state in client-side JSON and POST it to `aiprovider_api.ashx` endpoints, following the current `courseedit.js` pattern.

### Risk 4: Security drift from `ValidateRequest="false"`

The page already disables request validation, so rich preview content can become risky.

**Mitigation:** default new block fields to plain text and use explicit server-side encode/sanitize rules before rendering.

## Concrete Code Signals Supporting This Direction

- `teacher/courseedit.aspx` already exposes a right-side AI panel and saved draft banner.
- `js/courseedit.js` already has request/retry/progress/resume patterns that can be cloned for full-lesson actions.
- `teacher/aiprovider_api.ashx` already centralizes teacher-authorized AI endpoints and returns JSON DTOs.
- `App_Code/Common/AIActivityPlanSavedDraftHelper.cs` proves the current system already supports draft envelopes separate from lesson content.
- `Tests/TeacherRegressionTests/TeacherRegressionTests.cs` locks in append-only apply and draft resume behavior, so Phase 8 should extend via parallel hooks instead of rewriting the old flow.

## Validation Architecture

Phase 8 should validate at three levels:

1. **Common logic tests** for the full-lesson draft parser/validator/merge helper.
2. **Teacher regression source locks** for `courseedit.aspx`, `courseedit.js`, and `aiprovider_api.ashx` to ensure the new block preview and block actions remain wired.
3. **Focused publish guard** to confirm Phase 8 does not silently mutate the shipped single-activity publish contract.

Recommended fast commands:

- `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj --filter FullLesson`
- `dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj --filter CourseEdit`

## Recommended Planning Split

Phase 8 planning should likely separate into:

1. **Contract + handler foundation** — full-lesson models/helpers/save-load endpoints/tests
2. **Course editor block preview UX** — block cards, remove/regenerate flows, resume banner integration/tests

That keeps each plan within the context budget while fully covering ORCH-01/02/03.

## Sources

- `teacher/courseedit.aspx`
- `teacher/courseedit.aspx.cs`
- `js/courseedit.js`
- `teacher/aiprovider_api.ashx`
- `App_Code/Common/AIActivityPlanDraftHelper.cs`
- `App_Code/Common/AIActivityPlanSavedDraftHelper.cs`
- `App_Code/Model/CourseActivityPlanDraft.cs`
- `App_Code/Dal/CourseActivityPlanDraft.cs`
- `App_Code/Dal/AIActivityPlanPublisher.cs`
- `Tests/TeacherRegressionTests/TeacherRegressionTests.cs`
- `Tests/CommonLogicTests/ActivityPlanPublishCoreTests.cs`
- `.planning/research/SUMMARY.md`
- `.planning/research/ARCHITECTURE.md`
- `.planning/research/PITFALLS.md`

---

*Research completed: 2026-04-11*  
*Ready for planning: yes*
