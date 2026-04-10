---
phase: 02-structured-plan-draft-generation
verified: 2026-04-10T13:41:02Z
status: human_needed
score: 11/11 must-haves verified
overrides_applied: 0
human_verification:
  - test: "Generate a structured draft from teacher/courseedit.aspx"
    expected: "The right-side panel shows metadata plus separate cards for 教学目标、活动步骤、教学资源、评价设计、教师提醒 using a live configured provider."
    why_human: "Requires authenticated browser interaction and a live AI provider response."
  - test: "Confirm preview-only behavior after successful generation"
    expected: "The generated draft is visible and copyable, but editor content is unchanged until the teacher manually edits or saves existing lesson content."
    why_human: "Requires runtime UI interaction across the editor, preview panel, and save flow."
---

# Phase 2: Structured plan draft generation Verification Report

**Phase Goal:** Structured plan draft generation inside the existing teacher
course editor, with fixed structured draft output, strict
validation/normalization, and read-only preview rendering.
**Verified:** 2026-04-10T13:41:02Z
**Status:** human_needed
**Re-verification:** No — initial verification

## Goal Achievement

Phase 2's code deliverables are present, substantive, and wired. Focused net8.0
tests for ActivityPlan and CourseEdit pass. Remaining verification is limited to
live browser and provider behavior.

### Observable Truths

| # | Truth | Status | Evidence |
| --- | --- | --- | --- |
| 1 | Teacher can generate a draft activity plan with teaching goals, ordered activity steps, and time allocation. | ✓ VERIFIED | `AIActivityPlanPromptBuilder` fixes the schema and required step fields (`App_Code/Common/AIActivityPlanPromptBuilder.cs:60-63`); the handler returns `teachingGoals` and `activitySteps` including `minutes` (`teacher/aiprovider_api.ashx:428-455`); the UI renders separate goal and step cards (`js/courseedit.js:297-327`). |
| 2 | Generated steps include interaction methods such as questioning, discussion, grouping, presentation, or hands-on practice. | ✓ VERIFIED | Prompt and parser both require `interactionMethod` (`App_Code/Common/AIActivityPlanPromptBuilder.cs:62-63`, `App_Code/Common/AIActivityPlanDraftHelper.cs:251-257`); the handler serializes it (`teacher/aiprovider_api.ashx:444-448`); the UI renders `互动方式` (`js/courseedit.js:277-281`). |
| 3 | Generated output includes resource suggestions such as materials, tools, examples, or practice ideas. | ✓ VERIFIED | Parser requires both top-level `resources` and per-step `resourceSuggestion` (`App_Code/Common/AIActivityPlanDraftHelper.cs:56-65`, `247-257`); the handler returns both (`teacher/aiprovider_api.ashx:447-452`); the UI renders `教学资源` and `资源建议` (`js/courseedit.js:277-281`, `317`). |
| 4 | Generated output includes assessment design such as checks for understanding, observation points, or completion criteria. | ✓ VERIFIED | Parser requires top-level `assessment` and per-step `assessmentCheck` (`App_Code/Common/AIActivityPlanDraftHelper.cs:57-65`, `255-257`); the handler returns them (`teacher/aiprovider_api.ashx:447-452`); the UI renders `评价设计` and `评价检查` (`js/courseedit.js:281`, `318`). |
| 5 | The generated plan remains a teacher-reviewed draft and is never auto-applied or auto-published without teacher confirmation. | ✓ VERIFIED | Success metadata states the output is preview-only (`js/courseedit.js:312`); progress text repeats no auto-writeback (`js/courseedit.js:475`); rendering code never assigns generated draft data into `mcontent`; save still uses existing `syncContent()` path on the editor (`teacher/courseedit.aspx:257-259`, `js/courseedit.js:178-191`). |
| 6 | Structured activity-plan generation stays on the existing authenticated provider route. | ✓ VERIFIED | `teacher/aiprovider_api.ashx` checks the teacher cookie before dispatch (`teacher/aiprovider_api.ashx:18-23`) and serves `activityPlan` through the existing handler route (`teacher/aiprovider_api.ashx:52-54`, `392-457`). |
| 7 | The scoped activity-plan custom skill is loaded as the system prompt at runtime. | ✓ VERIFIED | The generator calls `EnsureDefaultSkill()`, loads the scoped skill, and uses its prompt as the `system` message (`App_Code/Bll/AIActivityPlanDraftGenerator.cs:26`, `41-45`, `130-143`); the bootstrap scope is `activity_plan_courseedit` (`App_Code/Bll/AIActivityPlanSkillBootstrap.cs:9-11`, `42-50`). |
| 8 | The server accepts only complete structured drafts after parsing and normalization. | ✓ VERIFIED | `ParseDraft` normalizes wrappers and aliases, then returns `null` unless the full contract is valid (`App_Code/Common/AIActivityPlanDraftHelper.cs:40-66`); `IsValidDraft` rejects empty sections or invalid steps (`68-125`). |
| 9 | Invalid structured responses fail closed instead of falling back to plain text. | ✓ VERIFIED | The generator rejects invalid drafts and returns `Success = false` (`App_Code/Bll/AIActivityPlanDraftGenerator.cs:49-58`); the handler sends a failure payload without raw provider text (`teacher/aiprovider_api.ashx:415-425`). |
| 10 | The existing right-side course editor panel remains the only Phase 2 preview host, and the browser renders structured draft cards instead of one raw AI text block. | ✓ VERIFIED | `teacher/courseedit.aspx` keeps `#courseedit-plan-panel`, `#activity-plan-result`, and the copy button (`teacher/courseedit.aspx:197-249`); `renderActivityPlanDraft()` creates metadata plus section cards and step cards (`js/courseedit.js:297-327`). |
| 11 | The browser keeps safe DOM rendering and copy support for the structured draft. | ✓ VERIFIED | Rendering uses `document.createElement` and `textContent` throughout (`js/courseedit.js:228-327`); copy/export is built from structured response data through `buildActivityPlanCopyText()` (`js/courseedit.js:329-356`, `515-526`). |

**Score:** 11/11 truths verified

### Required Artifacts

| Artifact | Expected | Status | Details |
| --- | --- | --- | --- |
| `App_Code/Bll/AIActivityPlanDraftGenerator.cs` | Structured generation orchestration and provider call flow | ✓ VERIFIED | Exists, substantive, and called by `ActivityPlan()` in the handler. |
| `App_Code/Common/AIActivityPlanDraftHelper.cs` | Parsing, normalization, and strict validation | ✓ VERIFIED | Exists, substantive, and used as the server acceptance gate before success is returned. |
| `App_Code/Common/AIActivityPlanPromptBuilder.cs` | Fixed Phase 2 prompt/schema guardrails | ✓ VERIFIED | Exists, substantive, and called by the generator to build the `user` prompt. |
| `App_Code/Bll/AIActivityPlanSkillBootstrap.cs` | Scoped skill bootstrap/runtime seed | ✓ VERIFIED | Exists, substantive, and used by the generator for scoped system-prompt loading. |
| `teacher/aiprovider_api.ashx` | Structured `activityPlan` browser contract | ✓ VERIFIED | Exists, substantive, and routes browser requests into the typed draft generator. |
| `teacher/courseedit.aspx` | Existing preview host markup and IDs | ✓ VERIFIED | Exists, substantive, and keeps the right-side panel/result/copy controls. |
| `js/courseedit.js` | Structured draft rendering, progress, failure, and copy logic | ✓ VERIFIED | Exists, substantive, and wired from the page via script include and DOM IDs. |
| `Tests/CommonLogicTests/CommonLogicTests.cs` | Focused ActivityPlan test coverage | ✓ VERIFIED | Exists and contains prompt, parsing, normalization, and fail-closed tests. |
| `Tests/TeacherRegressionTests/TeacherRegressionTests.cs` | CourseEdit regression coverage | ✓ VERIFIED | Exists and locks panel markup, request wiring, and safe rendering hooks. |

### Key Link Verification

| From | To | Via | Status | Details |
| --- | --- | --- | --- | --- |
| `teacher/aiprovider_api.ashx` | `App_Code/Bll/AIActivityPlanDraftGenerator.cs` | `ActivityPlan(context)` generation call | ✓ WIRED | `ActivityPlan()` instantiates `AIActivityPlanDraftGenerator` and calls `Generate(request)` (`teacher/aiprovider_api.ashx:404-415`). |
| `App_Code/Bll/AIActivityPlanDraftGenerator.cs` | `App_Code/Bll/AIActivityPlanSkillBootstrap.cs` | `EnsureDefaultSkill()` and scoped skill lookup | ✓ WIRED | Generator calls bootstrap methods and uses bootstrap default skill metadata (`App_Code/Bll/AIActivityPlanDraftGenerator.cs:26`, `43`, `91-127`). |
| `App_Code/Bll/AIActivityPlanDraftGenerator.cs` | `App_Code/Common/AIActivityPlanPromptBuilder.cs` | topic-first structured user prompt building | ✓ WIRED | Generator builds the `user` prompt with `AIActivityPlanPromptBuilder.Build(request)` (`App_Code/Bll/AIActivityPlanDraftGenerator.cs:44`). |
| `App_Code/Bll/AIActivityPlanDraftGenerator.cs` | `App_Code/Common/AIActivityPlanDraftHelper.cs` | provider response parsing and validation | ✓ WIRED | Generator parses provider output and checks validity before success (`App_Code/Bll/AIActivityPlanDraftGenerator.cs:48-58`). |
| `teacher/courseedit.aspx` | `js/courseedit.js` | `activity-plan-*` IDs and `window.__courseeditConfig` | ✓ WIRED | The page defines the panel IDs/config and includes `../js/courseedit.js` (`teacher/courseedit.aspx:213-249`, `325-338`). |
| `js/courseedit.js` | `teacher/aiprovider_api.ashx` | `action=activityPlan` XHR payload | ✓ WIRED | Browser XHR posts to `aiprovider_api.ashx` with `action=activityPlan` and structured fields (`js/courseedit.js:452-455`, `507-512`). |

### Data-Flow Trace (Level 4)

| Artifact | Data variable | Source | Produces real data | Status |
| --- | --- | --- | --- | --- |
| `teacher/aiprovider_api.ashx` | `result.Draft` | `AIActivityPlanDraftGenerator.Generate(request)` | Yes — handler serializes the typed draft returned by the generator | ✓ FLOWING |
| `App_Code/Bll/AIActivityPlanDraftGenerator.cs` | `draft` | Provider `/chat/completions` response → `AIActivityPlanDraftHelper.ParseDraft(responseText)` | Yes — upstream source is external provider content, then normalized and validated before acceptance | ✓ FLOWING |
| `js/courseedit.js` | `responseData.draft` / `lastActivityPlanDraftResponse` | XHR success path from `aiprovider_api.ashx` | Yes — successful `res.data.draft` is stored, rendered, and used for copy export | ✓ FLOWING |

### Behavioral Spot-Checks

| Behavior | Command | Result | Status |
| --- | --- | --- | --- |
| Activity-plan prompt/parsing/validation contract | `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj --framework net8.0 --filter "FullyQualifiedName~ActivityPlan" --no-build` | 10 passed, 0 failed | ✓ PASS |
| Course editor structured preview hooks | `dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj --framework net8.0 --filter "FullyQualifiedName~CourseEdit"` | 2 passed, 0 failed | ✓ PASS |

Net48 testhost is unavailable on this Linux machine and was not treated as a
product failure.

### Requirements Coverage

| Requirement | Source plan | Description | Status | Evidence |
| --- | --- | --- | --- | --- |
| `PLAN-01` | `02-01`, `02-02` | Teacher can generate a structured activity plan with teaching goals, step flow, and time allocation | ✓ SATISFIED | Fixed schema, strict parsing, structured handler payload, and goal/step rendering (`AIActivityPlanPromptBuilder.cs`, `AIActivityPlanDraftHelper.cs`, `aiprovider_api.ashx`, `courseedit.js`). |
| `PLAN-02` | `02-01`, `02-02` | Generated activity plan includes interaction methods | ✓ SATISFIED | `interactionMethod` is required by prompt, validation, handler output, and UI rendering. |
| `PLAN-03` | `02-01`, `02-02` | Generated activity plan includes resource suggestions | ✓ SATISFIED | `resources` and `resourceSuggestion` are required, serialized, and rendered. |
| `PLAN-04` | `02-01`, `02-02` | Generated activity plan includes assessment design | ✓ SATISFIED | `assessment` and `assessmentCheck` are required, serialized, and rendered. |
| `FLOW-02` | `02-01`, `02-02` | Generated plans remain teacher-reviewed drafts and are never auto-applied without confirmation | ✓ SATISFIED | Preview-only copy, no draft-to-editor assignment, existing save flow unchanged, and preview-only messaging in UI. |

### Anti-Patterns Found

| File | Line | Pattern | Severity | Impact |
| --- | --- | --- | --- | --- |
| — | — | No blocker anti-patterns found in inspected Phase 2 files. Input `placeholder` attributes were present but were valid UI hints, not stubs. | ℹ️ Info | No impact on Phase 2 goal achievement. |

### Human Verification Required

### 1. Generate a live structured draft in the teacher editor

**Test:** Open `teacher/courseedit.aspx` as an authenticated teacher, enter a
topic, and click **生成活动计划** with a configured AI provider.

**Expected:** The right-side panel shows preview metadata plus separate cards
for `教学目标`, `活动步骤`, `教学资源`, `评价设计`, and `教师提醒`. The copy
button copies the structured text export.

**Why human:** This needs authenticated browser interaction and a live external
provider response.

### 2. Confirm preview-only behavior at runtime

**Test:** Before and after generation, compare the editor content, then save the
page through the normal save button.

**Expected:** The preview appears in the side panel only; generated content does
not auto-write into the editor or silently publish.

**Why human:** This requires observing runtime browser/editor behavior across
multiple UI elements.

### Gaps Summary

No code gaps were found against the Phase 2 goal, roadmap success criteria, or
requested requirement IDs. Remaining work is live human verification of browser
behavior and configured provider integration.

---

_Verified: 2026-04-10T13:41:02Z_
_Verifier: the agent (gsd-verifier)_
