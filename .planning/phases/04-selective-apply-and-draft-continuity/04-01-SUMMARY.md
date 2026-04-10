---
phase: 04-selective-apply-and-draft-continuity
plan: 01
subsystem: api
tags: [webforms, sql-server, ai-provider, saved-drafts, xunit, migrations]
requires:
  - phase: 03-01
    provides: section-safe activity-plan draft validation on the existing AI route
  - phase: 03-02
    provides: course-editor preview flow that will consume saved draft status and resume data
provides:
  - one-current-draft-per-course persistence for activity-plan continuity
  - authenticated draft status, save, load, and delete actions on the existing AI handler
  - fail-closed revalidation of stored draft JSON before resume data is returned
affects: [teacher-course-editor, phase-4-ui-flow, db-upgrade-path]
tech-stack:
  added: []
  patterns: [per-course draft upsert, fail-closed saved-draft parsing, teacher-owned course authorization]
key-files:
  created:
    - App_Code/Model/CourseActivityPlanDraft.cs
    - App_Code/Common/AIActivityPlanSavedDraftHelper.cs
    - App_Code/Dal/CourseActivityPlanDraft.cs
    - App_Code/Bll/CourseActivityPlanDraft.cs
  modified:
    - App_Code/Utility/UpdateGrade.cs
    - App_Code/Utility/DbMigration.cs
    - teacher/aiprovider_api.ashx
    - Tests/CommonLogicTests/CommonLogicTests.cs
key-decisions:
  - "Keep saved activity-plan continuity in a dedicated per-course draft table and expose it through the existing authenticated AI handler instead of mixing draft state into lesson content."
  - "Require explicit draft-status checks and teacher-owned course authorization before any save, load, or delete draft action succeeds."
patterns-established:
  - "Saved-draft trust boundary pattern: reparse stored DraftJson through AIActivityPlanDraftHelper before any resume payload is returned."
  - "Course-owned continuity pattern: authorize by teacher cookie plus Courses.Chid before draft status, save, load, or delete actions run."
requirements-completed: [EDIT-04]
duration: 38m
completed: 2026-04-10
---

# Phase 4 Plan 1: Backend draft continuity summary

**Per-course saved activity-plan drafts with migration-backed persistence and authenticated resume endpoints on the existing AI route**

## Performance

- **Duration:** 38m
- **Started:** 2026-04-10T16:07:00Z
- **Completed:** 2026-04-10T16:22:35Z
- **Tasks:** 2
- **Files modified:** 8

## Accomplishments

- Added a dedicated `CourseActivityPlanDraft` model plus DAL/BLL support for one
  current saved draft per course.
- Added `AIActivityPlanSavedDraftHelper` so saved request context and structured
  draft JSON are serialized together and reparsed fail-closed on load.
- Added migration `1.9.1.3` and a dedicated `CourseActivityPlanDraft` table with
  a unique `Cid` index for one-row-per-course protection.
- Extended `teacher/aiprovider_api.ashx` with authenticated draft status, save,
  load, and delete actions that verify teacher ownership before success.
- Expanded focused xUnit coverage to lock the saved-draft contract, migration
  registration, and route wiring.

## Task Commits

Each task was committed atomically:

1. **Task 1: Add a one-draft-per-course persistence contract and fail-closed saved-draft helper** - `beabc90`, `c84a0d1` (test, feat)
2. **Task 2: Expose draft status, save, load, and replace-safe deletion on the existing AI route** - `97bed4d` (feat)

**Plan metadata:** pending final docs commit

_Note: Task 1 followed TDD with separate failing-test and implementation commits._

## Files Created/Modified

- `App_Code/Model/CourseActivityPlanDraft.cs` - Defines the typed per-course
  saved-draft record.
- `App_Code/Common/AIActivityPlanSavedDraftHelper.cs` - Builds saved records and
  reparses stored draft JSON through the shared draft validator.
- `App_Code/Dal/CourseActivityPlanDraft.cs` - Adds parameterized get, upsert,
  and delete operations for the current course draft.
- `App_Code/Bll/CourseActivityPlanDraft.cs` - Adds guard-clause business access
  for the saved-draft DAL.
- `App_Code/Utility/UpdateGrade.cs` - Adds schema creation and unique-index
  protection for the new draft table.
- `App_Code/Utility/DbMigration.cs` - Registers migration `1.9.1.3` in the
  upgrade pipeline.
- `teacher/aiprovider_api.ashx` - Adds authenticated draft status, save, load,
  delete, and course-ownership checks.
- `Tests/CommonLogicTests/CommonLogicTests.cs` - Adds saved-draft helper tests
  and source-lock coverage for the migration and route contract.

## Decisions Made

- Kept saved draft persistence separate from `Courses.Ccontent` so lesson-body
  changes still flow only through the normal editor save path.
- Returned only lightweight status metadata from `activityPlanDraftStatus` so
  the UI can show a resume affordance without auto-loading the saved draft.
- Reused the existing authenticated AI handler surface instead of introducing a
  second draft-specific endpoint.

## Deviations from Plan

None - plan executed exactly as written.

## Issues Encountered

- The focused backend test project does not compile the Web Forms handler or
  migration files directly, so Task 2 validation used targeted source-lock
  tests in `Tests/CommonLogicTests/CommonLogicTests.cs` in addition to the
  existing helper tests.

## User Setup Required

None - no external service configuration required.

## Next Phase Readiness

- The backend now exposes the saved-draft status/save/load/delete contract that
  Phase 4 plan 2 can call from the course editor panel.
- The database upgrade path now includes the saved-draft table, so UI work can
  assume one current course draft exists when persistence is enabled.
- Remaining phase work is front-end only: append-only selective apply, resume
  banner wiring, and replace-versus-continue UI prompts.

## Self-Check: PASSED
