---
phase: 05-teacher-activity-publish-foundation
plan: 01
subsystem: database
tags: [activity-plan, publish, draft-linkage, webforms]
requires: []
provides:
  - Typed AI activity publish request and result contracts
  - Draft-level mission and menu linkage persistence
  - Migration coverage for linked publish identifiers
affects: [phase-05-plan-02, phase-05-plan-03, activity-publish]
tech-stack:
  added: []
  patterns: [explicit publish contracts, draft-linked mission identity]
key-files:
  created:
    - App_Code/Model/AIActivityPlanPublishRequest.cs
    - App_Code/Model/AIActivityPlanPublishResult.cs
    - Tests/CommonLogicTests/ActivityPlanDraftLinkageTests.cs
  modified:
    - App_Code/Model/CourseActivityPlanDraft.cs
    - App_Code/Common/AIActivityPlanSavedDraftHelper.cs
    - App_Code/Dal/CourseActivityPlanDraft.cs
    - App_Code/Utility/UpdateGrade.cs
    - Tests/CommonLogicTests/CommonLogicTests.cs
key-decisions:
  - "Use explicit request and result models for AI publish instead of anonymous handler payloads."
  - "Persist LinkedMissionId and LinkedListMenuId on the saved draft row so re-publish can target one stable activity identity per course."
patterns-established:
  - "Publish contract pattern: handler and BLL exchange strongly typed publish models."
  - "Draft linkage pattern: saved AI drafts carry downstream mission and menu identity for later publish reuse."
requirements-completed: [TAP-01, TAP-02]
duration: 4min
completed: 2026-04-11
---

# Phase 5 Plan 1: Publish contracts and draft linkage summary

**Typed publish contracts plus draft-linked mission/menu identifiers for stable AI activity re-publish targeting.**

## Performance

- **Duration:** 4 min
- **Started:** 2026-04-11T03:39:51Z
- **Completed:** 2026-04-11T03:44:00Z
- **Tasks:** 2
- **Files modified:** 8

## Accomplishments

- Added concrete `AIActivityPlanPublishRequest` and `AIActivityPlanPublishResult` models for the server-owned publish flow.
- Extended saved draft storage to preserve `LinkedMissionId` and `LinkedListMenuId` across later publish calls.
- Added migration and source-level tests to lock the linkage contract before publish-core wiring.

## Task Commits

Each task was committed atomically:

1. **Task 1: Define publish contracts for the server-owned flow** - `b57a58f` (feat)
2. **Task 2: Persist linked mission and menu ids on the existing draft record** - `8300549` (feat)

## Files Created/Modified

- `App_Code/Model/AIActivityPlanPublishRequest.cs` - Defines the typed publish input contract.
- `App_Code/Model/AIActivityPlanPublishResult.cs` - Defines committed publish result fields.
- `App_Code/Model/CourseActivityPlanDraft.cs` - Stores linked mission and menu ids with each saved draft.
- `App_Code/Common/AIActivityPlanSavedDraftHelper.cs` - Round-trips draft linkage through build and parse helpers.
- `App_Code/Dal/CourseActivityPlanDraft.cs` - Reads and writes linked ids in draft persistence.
- `App_Code/Utility/UpdateGrade.cs` - Adds upgrade coverage for linked draft columns.
- `Tests/CommonLogicTests/CommonLogicTests.cs` - Pins publish contract source expectations.
- `Tests/CommonLogicTests/ActivityPlanDraftLinkageTests.cs` - Locks migration, helper, and DAL linkage coverage.

## Decisions Made

- Used explicit publish request and result models so later handler code validates a known server contract.
- Kept linkage on the existing draft row instead of introducing a separate linkage table, which preserves the one-draft-per-course brownfield pattern.

## Deviations from Plan

None - plan executed exactly as written.

## Issues Encountered

- `dotnet test` without `-f net8.0` fails in this environment because the `net48` test host is unavailable, so targeted verification used `-f net8.0`.

## User Setup Required

None - no external service configuration required.

## Next Phase Readiness

- The publish core can now reuse stable draft-linked mission and menu identity.
- Plan 05-02 can build a synchronized transactional publish flow on top of these contracts.

## Self-Check: PASSED

- Verified summary file target exists.
- Verified commits `b57a58f` and `8300549` exist in git history.

---

*Phase: 05-teacher-activity-publish-foundation*
*Completed: 2026-04-11*
