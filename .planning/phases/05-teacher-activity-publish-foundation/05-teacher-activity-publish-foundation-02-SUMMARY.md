---
phase: 05-teacher-activity-publish-foundation
plan: 02
subsystem: api
tags: [activity-plan, publish-core, transaction, mission, listmenu]
requires:
  - phase: 05-01
    provides: publish contracts and draft linkage persistence
provides:
  - Transactional AI activity publish core across course, mission, menu, and draft tables
  - Server-side lesson append and mission content builders
  - Re-publish reuse of linked mission/menu records
affects: [phase-05-plan-03, student-showmission, activity-publish]
tech-stack:
  added: []
  patterns: [single publish transaction, append-vs-full content split]
key-files:
  created:
    - App_Code/Common/AIActivityPlanPublishContentBuilder.cs
    - App_Code/Dal/AIActivityPlanPublisher.cs
    - App_Code/Bll/AIActivityPlanPublisher.cs
    - Tests/CommonLogicTests/ActivityPlanPublishCoreTests.cs
  modified: []
key-decisions:
  - "Build lesson append HTML and full mission HTML separately from the same draft so teacher and student outputs stay synchronized without sharing identical rendering rules."
  - "Route all course, mission, menu, and draft-link updates through one DAL transaction to prevent publish-state drift."
patterns-established:
  - "Publish transaction pattern: one DAL entry owns all linked publish writes."
  - "Content rendering pattern: selected sections append into course content while full instructions populate mission content."
requirements-completed: [TAP-01, TAP-02, TAP-03]
duration: 5min
completed: 2026-04-11
---

# Phase 5 Plan 2: Synchronized publish core summary

**Transactional AI publish core that appends selected lesson sections while updating one linked mission/menu activity with full student-facing content.**

## Performance

- **Duration:** 5 min
- **Started:** 2026-04-11T03:44:00Z
- **Completed:** 2026-04-11T03:49:00Z
- **Tasks:** 2
- **Files modified:** 4

## Accomplishments

- Added `AIActivityPlanPublishContentBuilder` to generate append-only lesson HTML and full mission HTML with the required Chinese labels.
- Implemented `AIActivityPlanPublisher` DAL and BLL so one publish action updates `Courses`, `Mission`, `ListMenu`, and draft linkage together.
- Added source-level tests that pin transaction boundaries, linked record reuse, publish flags, and the single BLL entry point.

## Task Commits

Each task was committed atomically:

1. **Task 1: Build server-side append and mission-content renderers** - `391ce0e` (feat)
2. **Task 2: Implement the synchronized publish core** - `391ce0e` (feat)

## Files Created/Modified

- `App_Code/Common/AIActivityPlanPublishContentBuilder.cs` - Rebuilds selected lesson sections and full mission content from a validated draft.
- `App_Code/Dal/AIActivityPlanPublisher.cs` - Owns the transactional publish flow and linked mission/menu reuse.
- `App_Code/Bll/AIActivityPlanPublisher.cs` - Exposes the brownfield `Publish(...)` entry point for the handler.
- `Tests/CommonLogicTests/ActivityPlanPublishCoreTests.cs` - Locks renderer and publish-core source behavior.

## Decisions Made

- Forced the student activity contract to stay on the existing `Mission` + `ListMenu` route by always setting `Mupload=true`, `Mcategory=0`, `Mfiletype="office"`, and `Ltype=1` in the publish core.
- Reused previously linked mission and menu ids only when they still belong to the same course, which prevents cross-course linkage drift.

## Deviations from Plan

### Auto-fixed Issues

**1. [Rule 2 - Missing Critical] Publish flow now refreshes draft snapshot data during link save**
- **Found during:** Task 2 (Implement the synchronized publish core)
- **Issue:** Initial draft-link persistence only updated linked ids and assumed a draft row already existed, which could leave publish linkage unsaved or stale after a server-owned publish.
- **Fix:** Updated the publish DAL to rebuild the saved draft record and upsert topic, draft JSON, course content snapshot, and linked ids inside the publish transaction.
- **Files modified:** `App_Code/Dal/AIActivityPlanPublisher.cs`
- **Verification:** `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj -f net8.0 --filter "FullyQualifiedName~ActivityPlanPublishContent|FullyQualifiedName~ActivityPlanPublish"`
- **Committed in:** `391ce0e` (part of task commit)

---

**Total deviations:** 1 auto-fixed (1 missing critical)
**Impact on plan:** The auto-fix kept publish linkage durable and transactional without expanding scope.

## Issues Encountered

- `dotnet test` without `-f net8.0` is not usable in this environment because the `net48` test host is missing.

## User Setup Required

None - no external service configuration required.

## Next Phase Readiness

- The teacher UI can now post one authenticated publish action to a stable server core.
- Phase 05-03 only needs to wire browser controls and the handler route onto this publish service.

## Self-Check: PASSED

- Verified summary file target exists.
- Verified commit `391ce0e` exists in git history.

---

*Phase: 05-teacher-activity-publish-foundation*
*Completed: 2026-04-11*
