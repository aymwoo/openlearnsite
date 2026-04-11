---
phase: 06-student-activity-entry-and-guided-experience
plan: 02
subsystem: ui
tags: [webforms, student-mission, ai-guidance, manual-uat]

# Dependency graph
requires:
  - phase: 06-student-activity-entry-and-guided-experience
    provides: Student mission routing plus the initial AI mission guidance helper.
provides:
  - Hardened the mission guidance helper with stable topic-first instruction ordering.
  - Finalized the learner guidance shell and validation hooks on `student/showmission.aspx`.
  - Added explicit manual UAT steps for authenticated publish-to-student verification.
affects: [phase-07-submission, student-validation, ai-mission-guidance]

# Tech tracking
tech-stack:
  added: []
  patterns: [topic-first learner guidance ordering, browser-UAT documented in summary]

key-files:
  created: [.planning/phases/06-student-activity-entry-and-guided-experience/06-student-activity-entry-and-guided-experience-02-SUMMARY.md]
  modified: [App_Code/Common/AIActivityPlanMissionViewHelper.cs, student/showmission.aspx, student/showmission.aspx.cs, Tests/CommonLogicTests/CommonLogicTests.cs, Tests/TeacherRegressionTests/TeacherRegressionTests.cs]

key-decisions:
  - "Prepend the published mission topic to learner instructions so the student page has a stable first cue before resources and assessment reminders."
  - "Document browser UAT in the summary because the repository lacks a dedicated authenticated student UI regression suite."

patterns-established:
  - "Mission guidance instructions render in deterministic order: topic, resources, assessment reminders, then learner prompt."
  - "Phase summaries carry manual UAT steps when browser-only validation is required to close a phase."

requirements-completed: [SAE-02, SAE-03]

# Metrics
duration: 12min
completed: 2026-04-11
---

# Phase 06 Plan 02: Student activity entry and guided experience Summary

**Published AI mission guidance now renders with deterministic learner instructions, final page copy, and explicit browser UAT for the authenticated student flow.**

## Performance

- **Duration:** 12 min
- **Started:** 2026-04-11T06:33:45Z
- **Completed:** 2026-04-11T06:45:35Z
- **Tasks:** 2
- **Files modified:** 5

## Accomplishments
- Hardened the mission guidance helper so learner instructions always lead with the published topic and keep a stable order.
- Finalized the student mission guidance shell with a clear learning notice while preserving `Panelworks` and upload behavior.
- Completed automated source and helper verification, then documented browser UAT for the publish -> menu -> mission page path.

## Task Commits

Each task was committed atomically:

1. **Task 1: Add a helper for AI mission guidance extraction and fallback** - `4b6d92f` (feat)
2. **Task 2: Finish Phase 6 validation with browser-focused UAT support** - `a786463` (feat)

## Files Created/Modified
- `App_Code/Common/AIActivityPlanMissionViewHelper.cs` - Adds topic-first instruction shaping and deterministic learner guidance ordering.
- `student/showmission.aspx` - Completes the learner guidance shell with a dedicated learning notice block.
- `student/showmission.aspx.cs` - Writes final notice copy alongside helper-driven guidance sections and preserves fail-closed behavior.
- `Tests/CommonLogicTests/CommonLogicTests.cs` - Verifies stable instruction ordering and published-content-only helper behavior.
- `Tests/TeacherRegressionTests/TeacherRegressionTests.cs` - Pins the completed student mission shell and guidance integration source assertions.

## Decisions Made
- Surfaced the published activity topic as the first learner instruction so students can orient before reading resources or checks.
- Kept manual UAT in the summary artifact rather than inventing a new test surface for authenticated student browser behavior.

## Deviations from Plan

None - plan executed exactly as written.

## Issues Encountered
- The default multi-target `dotnet test` path still cannot run `net48` in this environment because `testhost.net48.exe` is unavailable, so verification remained on the passing `net8.0` target.

## User Setup Required

None - no external service configuration required.

## Manual UAT

1. Log in as a teacher and publish one AI activity with student visibility enabled.
2. Log in as a student enrolled in the same course and open the course menu.
3. Confirm the published activity appears in the existing class menu and opens `student/showmission.aspx?lid=...`.
4. Verify the page shows the learning notice, activity topic, learner goal, activity instructions, and ordered task steps clearly.
5. Confirm the right-side upload panel and submission controls remain visible for upload-enabled missions.

## Next Phase Readiness
- Phase 7 can reuse the stabilized mission page and upload panel for AI activity submission work.
- Phase-level verification can now evaluate both automated helper/source coverage and the documented manual browser flow.

## Deviations from Threat Model

None.

## Self-Check: PASSED

- Verified `App_Code/Common/AIActivityPlanMissionViewHelper.cs` and `student/showmission.aspx` exist.
- Verified commits `4b6d92f` and `a786463` exist in git history.
