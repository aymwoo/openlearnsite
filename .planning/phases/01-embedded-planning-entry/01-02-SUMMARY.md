---
phase: 01-embedded-planning-entry
plan: 02
subsystem: ui
tags: [webforms, javascript, teacher-ui, regression-tests]
requires:
  - phase: 01-01
    provides: activity-plan backend contract and provider routing
provides:
  - embedded activity-plan assistant panel in the course editor
  - structured activityPlan request assembly from course edit UI
  - safe text-only rendering for AI plan output in the browser
affects: [teacher-course-editor, phase-2-generation-entry]
tech-stack:
  added: []
  patterns: [right-side assistant panel, page-scoped XHR helper, text-only result rendering]
key-files:
  created: []
  modified:
    - teacher/courseedit.aspx
    - js/courseedit.js
    - Tests/TeacherRegressionTests/TeacherRegressionTests.cs
key-decisions:
  - "Keep the assistant inside the existing editor stage with a right-side panel so save and editor-switch workflows remain primary."
  - "Use textContent for generated plan display so provider output never becomes raw HTML in the result panel."
patterns-established:
  - "Course editor assistant pattern: structured side-panel controls plus progress state beside the existing editor surface."
  - "Regression lock pattern: assert UI IDs, request wiring, and safe rendering tokens directly from source files."
requirements-completed: [INPUT-01, INPUT-02, INPUT-03, FLOW-01]
duration: 5 min
completed: 2026-04-10
---

# Phase 1 Plan 2: Embedded course-editor assistant Summary

**Right-side course-editor activity-plan assistant with structured request wiring and safe text rendering**

## Performance

- **Duration:** 5 min
- **Started:** 2026-04-10T11:47:00Z
- **Completed:** 2026-04-10T11:52:00Z
- **Tasks:** 2
- **Files modified:** 3

## Accomplishments
- Added a dedicated `活动计划助手` panel beside the existing course editor with topic-first and optional structured fields.
- Wired the panel to post `action=activityPlan` requests including current editor content as support background.
- Locked the UI contract with CourseEdit regression checks for markup, request assembly, and text-only result rendering.

## Task Commits

Each task was committed atomically:

1. **Task 1: Add the dedicated right-side planning panel to the course editor** - `a476aae` (feat)
2. **Task 2: Wire the panel to the backend contract and lock it with regression tests** - `db634c0` (feat)

## Files Created/Modified
- `teacher/courseedit.aspx` - Hosts the right-side assistant panel and passes grade/editor IDs to page script config.
- `js/courseedit.js` - Collects structured activity-plan inputs, posts to `aiprovider_api.ashx`, and renders results with `textContent`.
- `Tests/TeacherRegressionTests/TeacherRegressionTests.cs` - Covers panel markup, request payload hooks, repo root detection, and safe rendering tokens.

## Decisions Made
- Default the optional grade field from the current page grade selector while still letting the teacher override it in the panel.
- Preserve existing `switchEditor` and `syncContent()` behavior and only layer the assistant alongside them.

## Deviations from Plan

### Auto-fixed Issues

**1. [Rule 3 - Blocking] Fixed regression-test repo root discovery**
- **Found during:** Task 1 (Add the dedicated right-side planning panel to the course editor)
- **Issue:** `TeacherRegressionTests` looked for `learnsite-wz.sln`, which does not exist in this repository, so the focused CourseEdit test could not locate the repo root.
- **Fix:** Updated the helper to accept `openlearnsite.sln` as a valid repository root marker.
- **Files modified:** Tests/TeacherRegressionTests/TeacherRegressionTests.cs
- **Verification:** `dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj --filter "FullyQualifiedName~CourseEdit"`
- **Committed in:** `a476aae`

---

**Total deviations:** 1 auto-fixed (1 blocking)
**Impact on plan:** The fix was required to make the requested regression coverage executable in this repo and did not expand UI scope.

## Issues Encountered
- The focused regression command still aborts for the `net48` target on Linux because `testhost.net48.exe` is unavailable. The relevant `net8.0` CourseEdit tests passed and validated the new UI wiring.

## User Setup Required

None - no external service configuration required.

## Next Phase Readiness
- Teachers now have an embedded entry point that sends structured planning input and existing lesson context through the completed backend path.
- Phase 2 can build on this entry point to produce fuller structured activity-plan drafts and richer teacher review flows.

## Self-Check: PASSED

---
*Phase: 01-embedded-planning-entry*
*Completed: 2026-04-10*
