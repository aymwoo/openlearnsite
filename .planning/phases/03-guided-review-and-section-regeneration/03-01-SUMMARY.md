---
phase: 03-guided-review-and-section-regeneration
plan: 01
subsystem: api
tags: [ai-provider, webforms, structured-json, xunit, section-regeneration]
requires:
  - phase: 02-01
    provides: structured draft generation and strict draft validation
  - phase: 02-02
    provides: structured preview response contract
provides:
  - server-side section regeneration on the existing authenticated AI route
  - strict allowlist validation for supported section targets
  - safe merged-draft return after validated section retries
affects: [teacher-course-editor, phase-3-review-loop]
tech-stack:
  added: []
  patterns: [fail-closed section parsing, server-owned merge path, scoped-skill retry generation]
key-files:
  created: []
  modified:
    - App_Code/Common/AIActivityPlanDraftHelper.cs
    - App_Code/Common/AIActivityPlanPromptBuilder.cs
    - App_Code/Bll/AIActivityPlanDraftGenerator.cs
    - teacher/aiprovider_api.ashx
    - Tests/CommonLogicTests/CommonLogicTests.cs
key-decisions:
  - "Keep section regeneration on the existing authenticated provider handler instead of introducing a second API surface."
  - "Require the provider to return only one requested top-level section, then let the server validate and merge it into the full draft."
patterns-established:
  - "Section regeneration contract: allowlisted target, prompt-scoped single-key response, shared helper validation, and merged full-draft response."
  - "Current-draft trust boundary pattern: parse browser-sent draft JSON through the same shared helper before any provider call or merge."
requirements-completed: [EDIT-02]
duration: implemented 2026-04-10
completed: 2026-04-10
---

# Phase 3 Plan 1: Backend section regeneration summary

This plan adds the backend contract for guided section review inside the course
editor. Teachers can now retry one supported top-level section at a time while
the server keeps ownership of validation, parsing, and full-draft merging.

## Accomplishments

- Added a fixed allowlist for `teachingGoals`, `activitySteps`, `resources`,
  `assessment`, and `teacherReminder`.
- Added a dedicated section-regeneration prompt contract that includes the
  current valid draft as context and tells the provider to return only one
  top-level section key.
- Extended the draft generator to support section-level regeneration through the
  existing scoped skill and provider path.
- Added `activityPlanRegenerateSection` to `teacher/aiprovider_api.ashx` and
  kept the response shape aligned with the existing structured draft payload.
- Added focused xUnit coverage for allowlist validation, prompt scoping, valid
  merge behavior, and fail-closed rejection of bad section fragments.

## Files modified

- `App_Code/Common/AIActivityPlanDraftHelper.cs` - Adds section-target
  allowlist checks, section-only parsing, and safe merge helpers.
- `App_Code/Common/AIActivityPlanPromptBuilder.cs` - Adds the dedicated
  section-regeneration request model and single-key prompt builder.
- `App_Code/Bll/AIActivityPlanDraftGenerator.cs` - Adds server-side section
  regeneration orchestration and merged full-draft results.
- `teacher/aiprovider_api.ashx` - Adds the authenticated
  `activityPlanRegenerateSection` branch and shared draft validation on inbound
  browser JSON.
- `Tests/CommonLogicTests/CommonLogicTests.cs` - Adds section-regeneration unit
  coverage.

## Validation

The focused backend validation command passed:

`dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj --framework net8.0 --filter "FullyQualifiedName~ActivityPlan"`

## Notes

The Linux test run still emits many pre-existing nullable and legacy API
warnings from unrelated project files, but the focused ActivityPlan test slice
passed with zero failures.

---

*Phase: 03-guided-review-and-section-regeneration*
*Completed: 2026-04-10*
