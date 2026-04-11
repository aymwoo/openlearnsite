# Phase 8: Full-lesson draft orchestration - Context

**Gathered:** 2026-04-11
**Status:** Ready for execution

<domain>
## Phase boundary

Phase 8 extends the shipped `teacher/courseedit.aspx` planning assistant from a
single section-based activity draft into a previewable full-lesson draft made of
ordered typed blocks. The phase stays inside the existing teacher course editor,
reuses `teacher/aiprovider_api.ashx`, and preserves the current preview-first,
teacher-controlled operating model. It does not yet publish multiple activity
entities, change student runtime behavior, or introduce drag-and-drop lesson
editing.

</domain>

<decisions>
## Implementation decisions

### Draft contract
- **D-01:** Phase 8 should add a new full-lesson JSON contract parallel to the
  existing `ActivityPlanDraft` section schema instead of mutating the old shape
  in place.
- **D-02:** Each lesson block must expose stable `blockKey`, ordered `sort`,
  `blockType`, `teachingPurpose`, `lessonPosition`, and `minutes` so preview,
  save/load, remove, and regenerate all operate on the same server-owned shape.

### Handler boundary
- **D-03:** New `fullLesson*` actions should be added in
  `teacher/aiprovider_api.ashx` parallel to `activityPlan*` actions, preserving
  the shipped single-activity flow and its regression locks.
- **D-04:** Save/load/delete/regenerate actions must continue to be teacher-
  authorized through the existing course authorization pattern and fail closed on
  malformed draft payloads.

### Course editor UX
- **D-05:** `teacher/courseedit.aspx` and `js/courseedit.js` should evolve from
  section cards to ordered block cards for the full-lesson mode, while keeping
  the existing assistant shell, saved-draft banner, and explicit apply/publish
  controls.
- **D-06:** Block preview should be server-driven. The browser may hold draft
  JSON state, but it should only render the DTO returned by the handler instead
  of inventing a different client-only schema.
- **D-07:** Block-level remove and regenerate must preserve the rest of the
  current draft and must not automatically overwrite `mcontent`.

### Scope and safety
- **D-08:** Phase 8 ends at previewable orchestration. Multi-entity publish
  fan-out, block link records, guided inquiry runtime, and student flow changes
  remain deferred to later phases.
- **D-09:** New preview fields should default to plain text and follow explicit
  encode/sanitize rules because `courseedit.aspx` already uses
  `ValidateRequest="false"`.

### The agent's discretion
- Prefer minimal extension of the current course editor panel rather than a new
  teacher page or a separate assistant route.
- Keep browser state in JSON and reuse the existing request/retry/resume patterns
  already present in `js/courseedit.js`.
- When choosing between enriching the old section schema versus adding a new
  full-lesson helper, prefer the new helper to avoid brittle mixed contracts.

</decisions>

<canonical_refs>
## Canonical references

**Downstream agents MUST read these before planning or implementing.**

### Phase scope and validation
- `.planning/ROADMAP.md` — Phase 8 goal, requirement mapping, and success
  criteria.
- `.planning/PROJECT.md` — v1.2 milestone intent and brownfield constraints.
- `.planning/REQUIREMENTS.md` — `ORCH-01`, `ORCH-02`, and `ORCH-03`.
- `.planning/STATE.md` — current milestone position after v1.1 shipment.
- `.planning/phases/08-full-lesson-draft-orchestration/08-RESEARCH.md` — code-
  level research and recommended split.
- `.planning/phases/08-full-lesson-draft-orchestration/08-VALIDATION.md` — per-
  task verification map and manual UAT expectations.

### Existing orchestration surface
- `teacher/courseedit.aspx` — current AI assistant panel, saved-draft banner,
  preview area, apply actions, and publish toggle shell.
- `teacher/courseedit.aspx.cs` — course-editor page state and existing lesson
  save flow.
- `js/courseedit.js` — current browser draft state, section preview, save/load,
  regenerate, apply, and publish behavior.
- `teacher/aiprovider_api.ashx` — existing authenticated AI handler boundary.

### Existing draft and publish contracts
- `App_Code/Common/AIActivityPlanDraftHelper.cs` — current single-activity draft
  parser and validator.
- `App_Code/Common/AIActivityPlanSavedDraftHelper.cs` — saved-draft envelope
  serialization and parsing.
- `App_Code/Model/CourseActivityPlanDraft.cs` — one saved draft per course.
- `App_Code/Dal/CourseActivityPlanDraft.cs` — current draft persistence.
- `App_Code/Dal/AIActivityPlanPublisher.cs` — shipped single-activity publish
  core that must remain unchanged in this phase.

### Existing regression coverage
- `Tests/CommonLogicTests/CommonLogicTests.csproj` — unit slice for new helper
  and parser validation.
- `Tests/TeacherRegressionTests/TeacherRegressionTests.csproj` — source-lock
  slice for `courseedit.aspx`, `courseedit.js`, and `aiprovider_api.ashx`.

</canonical_refs>

<code_context>
## Existing code insights

### Reusable assets
- `teacher/courseedit.aspx` already has the visual shell for AI draft preview,
  saved draft resume, apply controls, and the current publish control row.
- `js/courseedit.js` already centralizes request flow, local draft state,
  section-level regenerate, save/load/delete, and append-only apply behavior.
- `teacher/aiprovider_api.ashx` already owns the authenticated route for all
  course-editor AI actions and returns JSON DTOs to the browser.
- `CourseActivityPlanDraft` persistence already supports one current draft per
  teacher/course pair without mixing transient assistant state into lesson body
  content.

### Established patterns
- The browser posts to `.ashx` actions and renders server-returned JSON; it does
  not rely on dynamic server controls for the assistant preview.
- Existing draft continuity is explicit: teachers save, resume, or delete one
  draft. There is no auto-save and no auto-apply.
- Existing source tests lock important strings and wiring in
  `TeacherRegressionTests`, so Phase 8 should extend via parallel hooks instead
  of rewriting the old activity-plan flow.

### Integration points
- New full-lesson generation/save/load/remove/regenerate endpoints should live in
  `teacher/aiprovider_api.ashx` beside the current `activityPlan*` actions.
- Preview rendering changes belong to `teacher/courseedit.aspx` and
  `js/courseedit.js`, not `courseedit.aspx.cs`.
- Helper and validation logic should live under `App_Code/Common` and be covered
  by the existing common logic test project.

</code_context>

<specifics>
## Specific ideas

- Plan 01 should focus on the full-lesson contract, helper/parser, and parallel
  `fullLesson*` handler foundation, while explicitly keeping the old
  `activityPlan*` contract intact.
- Plan 02 should focus on block-card preview UX, saved-draft resume messaging,
  and block-level remove/regenerate actions in the current course editor panel.
- A safe Phase 8 preview can display `unknown` block types, as long as publish
  remains out of scope for this phase.

</specifics>

<deferred>
## Deferred ideas

- Multi-type publish fan-out to `Mission`, `Exam`, `TxtForm`, and `Ware`.
- Block-level linkage persistence and republish diff/ownership policy.
- Guided inquiry runtime and combined publish action.
- Student-side composed runtime and per-block completion visualization.
- Drag-and-drop reordering or a separate visual lesson builder.

</deferred>

---

*Phase: 08-full-lesson-draft-orchestration*
*Context gathered: 2026-04-11*
