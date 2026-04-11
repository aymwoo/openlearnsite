---
phase: 05-teacher-activity-publish-foundation
plan: 03
subsystem: ui
tags: [activity-plan, teacher-ui, handler, publish-toggle, webforms]
requires:
  - phase: 05-02
    provides: synchronized publish core and content builders
provides:
  - Explicit unpublished-by-default AI publish controls in the course editor
  - Browser publish flow that updates the editor from committed server content
  - Authenticated handler route delegating to the publish BLL
affects: [phase-06, teacher-courseedit, activity-publish]
tech-stack:
  added: []
  patterns: [server-owned editor write-back, authenticated publish delegation]
key-files:
  created: []
  modified:
    - teacher/courseedit.aspx
    - js/courseedit.js
    - teacher/aiprovider_api.ashx
    - Tests/TeacherRegressionTests/TeacherRegressionTests.cs
key-decisions:
  - "Keep AI publish separate from legacy publish defaults by adding a dedicated unchecked toggle in the assistant panel."
  - "Replace editor content only from `updatedCourseContent` returned by the publish handler so the browser reflects committed server state."
patterns-established:
  - "UI publish pattern: require a generated draft plus selected sections before publish."
  - "Handler delegation pattern: parse browser payload, reauthorize course access, and pass the request to `AIActivityPlanPublisher` instead of duplicating persistence logic."
requirements-completed: [TAP-02, TAP-03]
duration: 4min
completed: 2026-04-11
---

# Phase 5 Plan 3: Teacher publish flow summary

**Dedicated AI publish controls plus an authenticated `activityPlanPublish` handler that writes back committed lesson content from the server publish core.**

## Performance

- **Duration:** 4 min
- **Started:** 2026-04-11T03:49:00Z
- **Completed:** 2026-04-11T03:53:01Z
- **Tasks:** 2
- **Files modified:** 4

## Accomplishments

- Added a separate AI publish toggle and publish button in `teacher/courseedit.aspx`, with hidden-by-default student visibility.
- Added `publishActivityPlan()` in `js/courseedit.js` to require a draft, require selected sections, post `action=activityPlanPublish`, and update the editor with committed server content.
- Extended `teacher/aiprovider_api.ashx` with an authenticated publish action that parses selected sections, validates course access, calls `AIActivityPlanPublisher`, and returns publish result fields.

## Task Commits

Each task was committed atomically:

1. **Task 1: Add dedicated AI publish controls to the course editor** - `0a54177` (feat)
2. **Task 2: Extend the authenticated handler with one publish action** - `0a54177` (feat)

## Files Created/Modified

- `teacher/courseedit.aspx` - Adds explicit AI publish controls that start unpublished by default.
- `js/courseedit.js` - Posts the AI publish request and writes back `updatedCourseContent` from the committed server response.
- `teacher/aiprovider_api.ashx` - Adds `activityPlanPublish` with authorization, draft parsing, section parsing, and publish-core delegation.
- `Tests/TeacherRegressionTests/TeacherRegressionTests.cs` - Pins the UI and handler hooks for AI publish behavior.

## Decisions Made

- Kept the legacy course publish checkbox untouched and introduced a separate AI publish control so the new flow does not inherit old mission defaults.
- Returned committed publish data from the handler and used it to replace the editor body, avoiding any second browser-side save path.

## Deviations from Plan

None - plan executed exactly as written.

## Issues Encountered

- `TeacherRegressionTests` also target `net48`, so verification used `-f net8.0` in this Linux environment where `testhost.net48.exe` is unavailable.

## User Setup Required

None - no external service configuration required.

## Next Phase Readiness

- Teacher-side AI publish is complete and can now feed the student activity experience in Phase 6.
- The publish route already returns mission/menu ids and committed lesson content needed by downstream student-entry work.

## Self-Check: PASSED

- Verified summary file target exists.
- Verified commit `0a54177` exists in git history.

---

*Phase: 05-teacher-activity-publish-foundation*
*Completed: 2026-04-11*
