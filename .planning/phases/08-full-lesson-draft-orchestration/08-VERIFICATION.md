---
phase: 08-full-lesson-draft-orchestration
verified: 2026-04-11T12:28:59Z
status: human_needed
score: 6/6 must-haves verified
overrides_applied: 0
human_verification:
  - test: "Authenticated teacher UAT: generate a full-lesson draft in teacher/courseedit.aspx"
    expected: "The right-side assistant renders multiple ordered block cards with type, teaching purpose, lesson position, and minutes."
    why_human: "Requires authenticated browser flow plus a live AI provider and visual confirmation of Web Forms rendering."
  - test: "Authenticated teacher UAT: remove one block, regenerate another, then resume saved draft"
    expected: "Untouched blocks stay stable, the edited block updates by blockKey scope, saved-draft banner still resumes the same full-lesson view, and mcontent stays unchanged until explicit teacher action."
    why_human: "Needs end-to-end browser interaction, authenticated persistence, and UI-state verification that source inspection cannot fully prove."
---

# Phase 8: Full-lesson draft orchestration Verification Report

**Phase Goal:** Teachers can generate and refine a previewable full-lesson draft made of ordered activity blocks inside the existing course editor
**Verified:** 2026-04-11T12:28:59Z
**Status:** human_needed
**Re-verification:** No — initial verification

## Goal Achievement

### Observable Truths

| # | Truth | Status | Evidence |
| --- | --- | --- | --- |
| 1 | Teacher can generate a structured full-lesson draft as multiple ordered activity blocks instead of one plain-text activity. | ✓ VERIFIED | `teacher/aiprovider_api.ashx:768-824` exposes `fullLessonGenerate`, builds `FullLessonDraft` via `BuildFullLessonDraftFromActivityPlan(...)`; `js/courseedit.js:1429-1510` posts `action=fullLessonGenerate`; `js/courseedit.js:576-690` renders ordered `draft.blocks`. |
| 2 | Teacher can preview each generated block with activity type, teaching purpose, lesson position, and minutes before deciding what to keep. | ✓ VERIFIED | `js/courseedit.js:665-680` renders `blockType`, `teachingPurpose`, `lessonPosition`, `minutes`; `teacher/courseedit.aspx:470-489` provides preview shell and preview-first note; `Tests/TeacherRegressionTests/TeacherRegressionTests.cs:809-825` locks those fields in source. |
| 3 | Teacher can remove or regenerate one block without discarding the rest of the full-lesson draft. | ✓ VERIFIED | `js/courseedit.js:530-570` removes only the selected `blockKey` and resequences remaining blocks; `js/courseedit.js:1226-1303` regenerates by `blockKey`; `teacher/aiprovider_api.ashx:1239-1329` replaces one block while copying untouched blocks forward. |
| 4 | Full-lesson draft parsing is block-based and rejects malformed or partial payloads. | ✓ VERIFIED | `App_Code/Common/AIActivityPlanDraftHelper.cs:126-156,165-201,337-356,532-565` requires `blockKey`, ordered `sort`, `blockType`, `teachingPurpose`, `lessonPosition`, etc.; `Tests/CommonLogicTests/CommonLogicTests.cs:967-1016` includes malformed-payload rejection and round-trip tests. |
| 5 | Save/load/delete/regenerate full-lesson actions remain teacher-authorized and parallel to shipped single-activity actions. | ✓ VERIFIED | `teacher/aiprovider_api.ashx:73-89` adds six `fullLesson*` cases; each route uses `TryGetAuthorizedCourse(...)` (`768-776`, `827-835`, `907-913`, `931-938`, `973-979`, `1028-1034`); persistence routes use `BuildFullLessonRecord`, `ParseFullLessonRecord`, and draft BLL/DAL. |
| 6 | The shipped single-activity publish path stays intact in this phase. | ✓ VERIFIED | `teacher/aiprovider_api.ashx:70-71,714-766` still keeps `activityPlanPublish`; `js/courseedit.js:847-907` still publishes only `activityPlanPublish` and explicitly blocks full-lesson publish; `Tests/TeacherRegressionTests/TeacherRegressionTests.cs:881-889` locks this path. |

**Score:** 6/6 truths verified

### Required Artifacts

| Artifact | Expected | Status | Details |
| --- | --- | --- | --- |
| `App_Code/Common/AIActivityPlanDraftHelper.cs` | Full-lesson parser and validator | ✓ VERIFIED | Exists, substantive, and used by handler save/load/regenerate paths; full-lesson DTOs at `38-85`, parse/validate path at `126-201`. |
| `App_Code/Common/AIActivityPlanSavedDraftHelper.cs` | Full-lesson saved-draft envelope logic | ✓ VERIFIED | Exists, substantive, and wired into save/load/delete flow; `BuildFullLessonRecord` / `ParseFullLessonRecord` at `87-115` and `175-230`. |
| `teacher/aiprovider_api.ashx` | Authenticated `fullLesson*` handler surface | ✓ VERIFIED | Exists, substantive, and wired to helper/persistence/generator code; switch cases at `73-89`, full-lesson handlers at `768-1056`. |
| `teacher/courseedit.aspx` | Block-card preview shell and saved-draft UX shell | ✓ VERIFIED | Exists, substantive, and wired via DOM ids plus script include; block-card styles `30-111`, preview/banner UI `434-489`, script wiring `567-580`. |
| `js/courseedit.js` | Browser rendering plus block-level remove/regenerate/save/load interactions | ✓ VERIFIED | Exists, substantive, and wired from page + API calls; render path `576-690`, save/load `939-1098`, regenerate `1226-1303`, generate `1429-1510`. |
| `Tests/CommonLogicTests/CommonLogicTests.cs` | Full-lesson parser validation coverage | ✓ VERIFIED | Contains executable `FullLesson` tests at `967-1016`; net8.0 slice passed 3/3. |
| `Tests/TeacherRegressionTests/TeacherRegressionTests.cs` | Source-lock coverage for handler/UI wiring | ✓ VERIFIED | Contains `CourseEdit` / `FullLesson` coverage at `692-900`; net8.0 slice passed 10/10. |

### Key Link Verification

| From | To | Via | Status | Details |
| --- | --- | --- | --- | --- |
| `teacher/aiprovider_api.ashx` | `App_Code/Common/AIActivityPlanDraftHelper.cs` | full-lesson request parsing and validation | ✓ WIRED | Handler calls `ParseFullLessonDraft(...)` and `IsValidFullLessonDraft(...)` in save/load/regenerate flow (`849-850`, `940`, `984`, helper `126-201`). |
| `teacher/aiprovider_api.ashx` | `App_Code/Dal/CourseActivityPlanDraft.cs` | authenticated save/load/delete of one current draft | ✓ WIRED | Handler uses BLL `GetCurrentByCourse / UpsertCurrent / DeleteCurrent` (`917`, `965`, `983`, `1038`, `1050`), and DAL persists by `Cid` + `Hid` (`15`, `40`). |
| `js/courseedit.js` | `teacher/aiprovider_api.ashx` | block-level regenerate and draft save/load actions | ✓ WIRED | Browser posts `fullLessonGenerate`, `fullLessonSaveDraft`, `fullLessonLoadDraft`, `fullLessonDeleteDraft`, `fullLessonDraftStatus`, `fullLessonRegenerateBlock` (`971`, `1031`, `1064`, `1097`, `1294`, `1504`). |
| `teacher/courseedit.aspx` | `js/courseedit.js` | full-lesson preview shell and action buttons | ✓ WIRED | Page defines activity-plan DOM ids and includes `../js/courseedit.js` (`434-489`, `568-580`). |

### Data-Flow Trace (Level 4)

| Artifact | Data Variable | Source | Produces Real Data | Status |
| --- | --- | --- | --- | --- |
| `teacher/aiprovider_api.ashx` | `fullLessonDraft` | `AIActivityPlanDraftGenerator.Generate(request)` → `BuildFullLessonDraftFromActivityPlan(...)` | Yes — generator returns parsed structured draft, then server maps it into ordered block DTOs (`793-808`, `1146-1236`) | ✓ FLOWING |
| `teacher/aiprovider_api.ashx` | `payload.FullLessonDraft` | `CourseActivityPlanDraft.GetCurrentByCourse(...)` → `AIActivityPlanSavedDraftHelper.ParseFullLessonRecord(...)` | Yes — reads persisted `DraftJson` and reparses validated full-lesson JSON (`917-918`, `983-1002`) | ✓ FLOWING |
| `js/courseedit.js` | `responseData.draft.blocks` | XHR responses from `fullLessonGenerate`, `fullLessonLoadDraft`, `fullLessonRegenerateBlock` | Yes — browser renders only server DTO fields and does not invent a client-only schema (`576-690`, `998-1002`, `1260-1264`, `1470-1472`) | ✓ FLOWING |

### Behavioral Spot-Checks

| Behavior | Command | Result | Status |
| --- | --- | --- | --- |
| Full-lesson parser/round-trip tests | `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj --filter FullLesson` | net8.0 slice passed `3/3`; net48 slice aborted because `testhost.net48.exe` is missing in this environment | ✓ PASS |
| Course editor wiring/regression tests | `dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj --filter CourseEdit` | net8.0 slice passed `10/10`; net48 slice aborted because `testhost.net48.exe` is missing in this environment | ✓ PASS |

### Requirements Coverage

| Requirement | Source Plan | Description | Status | Evidence |
| --- | --- | --- | --- | --- |
| ORCH-01 | `08-01-PLAN.md`, `08-02-PLAN.md` | Teacher can generate a structured full-lesson draft composed of multiple ordered activity blocks instead of one plain-text activity. | ✓ SATISFIED | Roadmap SC1 + handler `fullLessonGenerate` (`teacher/aiprovider_api.ashx:768-824`) + browser render path (`js/courseedit.js:576-690`, `1429-1510`). |
| ORCH-02 | `08-02-PLAN.md` | Teacher can preview each generated block with its activity type, teaching purpose, and position in the lesson flow before publishing. | ✓ SATISFIED | Block-card rendering outputs `blockType`, `teachingPurpose`, `lessonPosition`, `minutes` (`js/courseedit.js:665-680`); preview shell exists in `teacher/courseedit.aspx:470-489`. |
| ORCH-03 | `08-01-PLAN.md`, `08-02-PLAN.md` | Teacher can remove or regenerate an individual generated block without discarding the rest of the full-lesson draft. | ✓ SATISFIED | Remove is block-scoped client-side (`js/courseedit.js:530-570`); regenerate is block-scoped API + server merge (`js/courseedit.js:1226-1303`, `teacher/aiprovider_api.ashx:1239-1329`). |

### Anti-Patterns Found

| File | Line | Pattern | Severity | Impact |
| --- | --- | --- | --- | --- |
| `Tests/TeacherRegressionTests/TeacherRegressionTests.cs` | `861-877` | Source-lock assertions only (`Assert.Contains(...)`) for full-lesson handler behavior | ⚠️ Warning | Tests confirm route names/wiring, but do not execute a non-first-block regeneration scenario; semantic regressions could slip through despite passing source tests. |
| `Tests/CommonLogicTests/CommonLogicTests.cs` | `967-1016` | No helper-level test covering block replacement/merge semantics | ℹ️ Info | Full-lesson parsing is covered, but the `ReplaceFullLessonBlock(...)` path is only indirectly protected by source-lock tests, not behavior-level unit tests. |

### Human Verification Required

### 1. Authenticated full-lesson generation preview

**Test:** 以教师身份进入 `teacher/courseedit.aspx`，输入主题并生成整课草案。  
**Expected:** 右侧助手面板显示多个按顺序排列的 block card；每张卡片能看到活动类型、教学目的、课堂位置、预计时长；页面不会自动改写下方 `mcontent`。  
**Why human:** 需要真实登录态、AI provider、以及浏览器视觉确认。

### 2. Block-level refine + saved-draft resume

**Test:** 在已生成的整课草案中移除一个环节、重生成另一个环节、保存草案，再刷新后恢复草案。  
**Expected:** 未操作的环节保持稳定；被重生成的环节按 `blockKey` 范围更新；恢复后仍是同一整课 block 视图；`mcontent` 仍未被静默写入。  
**Why human:** 需要端到端交互、已保存草案的真实持久化，以及 UI 状态连贯性验证。

### Gaps Summary

未发现阻断 Phase 8 目标的代码级缺口。服务端已提供整课草案合同、授权路由与草案持久化，课程编辑页也已切换到 ordered block-card 预览并支持 block 级 remove/regenerate。当前剩余的是人工 UAT：需要在真实教师登录态与可用 AI provider 下确认视觉呈现、交互连贯性和 saved-draft 恢复体验。

---

_Verified: 2026-04-11T12:28:59Z_  
_Verifier: the agent (gsd-verifier)_
