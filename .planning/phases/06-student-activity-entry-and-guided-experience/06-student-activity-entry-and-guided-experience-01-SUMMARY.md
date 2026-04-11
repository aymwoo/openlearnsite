---
phase: 06-student-activity-entry-and-guided-experience
plan: 01
subsystem: ui
tags: [webforms, student-mission, ai-activity, regression-tests]

# Dependency graph
requires:
  - phase: 05-teacher-activity-publish-foundation
    provides: Published AI mission and ListMenu rows for student entry.
provides:
  - Pinned the published AI activity menu route to the existing `showmission.aspx` flow.
  - Added a server-owned mission guidance helper that derives learner sections from published mission content.
  - Preserved the legacy upload shell while surfacing goal, instructions, and ordered steps on the student page.
affects: [phase-06-plan-02, student-guidance, phase-07-submission]

# Tech tracking
tech-stack:
  added: []
  patterns: [server-owned mission guidance shaping, source-regression route pinning]

key-files:
  created: [App_Code/Common/AIActivityPlanMissionViewHelper.cs, .planning/phases/06-student-activity-entry-and-guided-experience/06-student-activity-entry-and-guided-experience-01-SUMMARY.md]
  modified: [student/Scm.master.cs, student/showmission.aspx, student/showmission.aspx.cs, Tests/TeacherRegressionTests/TeacherRegressionTests.cs, Tests/CommonLogicTests/CommonLogicTests.cs, Tests/CommonLogicTests/CommonLogicTests.csproj]

key-decisions:
  - "Keep `Ltype=1` hard-routed to `student/showmission.aspx?lid=` instead of following upload-mode indirection."
  - "Render learner guidance from published `Mission.Mcontent` on the server and fall back to raw mission content when AI markers are absent."

patterns-established:
  - "Student AI activity guidance uses a focused helper under `App_Code/Common/` rather than teacher draft state or browser-owned parsing."
  - "Student route and mission page behavior are pinned with source assertions in TeacherRegressionTests plus helper logic tests in CommonLogicTests."

requirements-completed: [SAE-01, SAE-02]

# Metrics
duration: 6min
completed: 2026-04-11
---

# Phase 06 Plan 01: Student activity entry and guided experience Summary

**Student AI activities now open through the legacy mission page with server-rendered learner guidance and the upload sidebar intact.**

## Performance

- **Duration:** 6 min
- **Started:** 2026-04-11T06:33:45Z
- **Completed:** 2026-04-11T06:40:07Z
- **Tasks:** 2
- **Files modified:** 6

## Accomplishments
- Locked published AI activity menu entries onto `student/showmission.aspx?lid=` for `Ltype=1`.
- Added learner-facing goal, instructions, and ordered step rendering from published mission content only.
- Preserved `HiddenMissionRaw`, `Panelworks`, and the existing upload shell while adding regression and helper coverage.

## Task Commits

Each task was committed atomically:

1. **Task 1: Lock the published AI activity route into the student menu flow** - `64cea5a` (feat)
2. **Task 2: Shape the mission page into a readable guided activity experience** - `6173030` (feat)

## Files Created/Modified
- `App_Code/Common/AIActivityPlanMissionViewHelper.cs` - Parses published mission content into learner goal, instruction, and step sections.
- `student/Scm.master.cs` - Pins `Ltype=1` student menu entries to `showmission.aspx?lid=`.
- `student/showmission.aspx` - Adds learner guidance section containers above the raw mission content.
- `student/showmission.aspx.cs` - Integrates helper-based guidance shaping and fail-closed not-found handling.
- `Tests/CommonLogicTests/CommonLogicTests.cs` - Verifies helper extraction, fallback behavior, and published-content-only sourcing.
- `Tests/CommonLogicTests/CommonLogicTests.csproj` - Includes the new mission view helper and publish content builder in the test project.
- `Tests/TeacherRegressionTests/TeacherRegressionTests.cs` - Pins the publish route contract and guided mission shell source assertions.

## Decisions Made
- Kept the brownfield student contract on `showmission.aspx` instead of adding an AI-specific page or menu type.
- Used server-side mission-content shaping so the student page never depends on teacher draft JSON, local storage, or teacher routes.

## Deviations from Plan

### Auto-fixed Issues

**1. [Rule 3 - Blocking] Added missing CommonLogic test project includes for the new mission helper**
- **Found during:** Task 2 (Shape the mission page into a readable guided activity experience)
- **Issue:** Filtered `CommonLogicTests` could not resolve `AIActivityPlanPublishContentBuilder` and `AIActivityPlanMissionViewHelper` because those source files were not included in `CommonLogicTests.csproj`.
- **Fix:** Added both source files to the test project compile list before rerunning verification.
- **Files modified:** `Tests/CommonLogicTests/CommonLogicTests.csproj`
- **Verification:** `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj --framework net8.0 --filter "FullyQualifiedName~ActivityPlanMissionView|FullyQualifiedName~ShowMission"`
- **Committed in:** `6173030` (part of task commit)

---

**Total deviations:** 1 auto-fixed (1 blocking)
**Impact on plan:** The auto-fix was required to verify the planned helper behavior. No scope creep.

## Issues Encountered
- The default multi-target `dotnet test` invocation aborts on `net48` in this environment because `testhost.net48.exe` is unavailable. Verification completed on the runnable `net8.0` target instead.

## User Setup Required

None - no external service configuration required.

## Next Phase Readiness
- Phase 6 plan 02 can build on the new helper instead of duplicating mission parsing logic in the page.
- The student page now exposes readable guidance while preserving the upload surface needed for Phase 7 submission work.

## Deviations from Threat Model

None.

## Self-Check: PASSED

- Verified `App_Code/Common/AIActivityPlanMissionViewHelper.cs` exists.
- Verified commits `64cea5a` and `6173030` exist in git history.
