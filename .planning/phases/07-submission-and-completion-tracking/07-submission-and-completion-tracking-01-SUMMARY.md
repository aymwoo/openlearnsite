---
phase: 07-submission-and-completion-tracking
plan: 01
subsystem: ui
tags: [webforms, student-upload, mission-submission, regression-tests]

# Dependency graph
requires:
  - phase: 06-student-activity-entry-and-guided-experience
    provides: Student mission routing plus the AI mission guidance shell on `student/showmission.aspx`.
provides:
  - Locked published AI activities onto the existing `lid` -> `ListMenu.Lxid` -> `Mission` upload contract.
  - Added source regressions for upload routing and explicit mission submission states.
  - Clarified student-facing submit, resubmit, locked, IP-blocked, and prior-work-required messaging on `showmission.aspx`.
affects: [phase-07-completion-tracking, student-mission, ai-activity-submission]

# Tech tracking
tech-stack:
  added: []
  patterns: [legacy upload handler reuse, source-pinned mission submission state helpers]

key-files:
  created: [.planning/phases/07-submission-and-completion-tracking/07-submission-and-completion-tracking-01-SUMMARY.md]
  modified: [student/uploadwork.aspx.cs, student/uploadworkm.aspx.cs, student/showmission.aspx.cs, Tests/TeacherRegressionTests/TeacherRegressionTests.cs]

key-decisions:
  - "Keep AI activity submission on the existing uploadwork/uploadworkm handlers and validate the `lid`-resolved mission contract instead of adding a new endpoint."
  - "Make mission submission state explicit through small helper branches on `showmission.aspx.cs` so upload visibility continues to follow legacy IP, prior-work, and teacher-lock rules."

patterns-established:
  - "Upload handlers fail closed when `lid`, `ListMenu`, or linked `Mission` is missing."
  - "Student mission submission copy is expressed through small named state helpers instead of duplicated inline branches."

requirements-completed: [SCT-01]

# Metrics
duration: 4min
completed: 2026-04-11
---

# Phase 07 Plan 01: Submission and completion tracking Summary

**Published AI activities now submit through the legacy mission upload handlers with guarded `lid` resolution and clear mission-page states for ready, resubmit, locked, and blocked submission flows.**

## Performance

- **Duration:** 4 min
- **Started:** 2026-04-11T09:59:01Z
- **Completed:** 2026-04-11T10:02:32Z
- **Tasks:** 2
- **Files modified:** 4

## Accomplishments
- Added minimal guards in both student upload handlers so AI-published activities must resolve a valid `lid`, `ListMenu`, and linked `Mission` before submission continues.
- Pinned the AI mission submission contract with targeted source regressions covering both `uploadwork.aspx.cs` and `uploadworkm.aspx.cs`.
- Made mission-page submission state explicit with named branches for ready-to-submit, resubmit, teacher-locked, IP-blocked, and prior-work-required cases while preserving existing upload gating.

## Task Commits

Each task was committed atomically:

1. **Task 1: Lock the AI mission upload route to the existing submission handlers** - `1e03d69` (fix)
2. **Task 2: Make submission state explicit on the mission page (RED)** - `70030dc` (test)
3. **Task 2: Make submission state explicit on the mission page (GREEN)** - `a2eba25` (feat)

## Files Created/Modified
- `student/uploadwork.aspx.cs` - Adds fail-closed `lid` and mission resolution guards before reusing the normal upload flow.
- `student/uploadworkm.aspx.cs` - Keeps the alternate upload mode on the same guarded `lid`-to-mission contract.
- `student/showmission.aspx.cs` - Extracts explicit submission-state helpers while preserving legacy IP and prior-work rules.
- `Tests/TeacherRegressionTests/TeacherRegressionTests.cs` - Adds regressions for upload routing and mission-page submission state messaging.

## Decisions Made
- Reused the brownfield mission upload handlers exactly as planned and only tightened missing request/model validation required for correctness.
- Kept server-side upload visibility driven by the existing conditions instead of inventing an AI-specific submission state model.

## Deviations from Plan

### Auto-fixed Issues

**1. [Rule 2 - Missing Critical] Added fail-closed upload guards for invalid mission routing**
- **Found during:** Task 1 (Lock the AI mission upload route to the existing submission handlers)
- **Issue:** Both upload handlers assumed `lid`, `ListMenu`, and `Mission` always existed, which could break the AI publish contract and bypass correct submission routing on malformed requests.
- **Fix:** Added minimal validation for numeric `lid`, existing `ListMenu`, and linked `Mission` before continuing the legacy upload flow.
- **Files modified:** `student/uploadwork.aspx.cs`, `student/uploadworkm.aspx.cs`, `Tests/TeacherRegressionTests/TeacherRegressionTests.cs`
- **Verification:** `dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj --filter "FullyQualifiedName~UploadWork|FullyQualifiedName~ActivityPlanSubmission"` (net8.0 passed; net48 host unavailable in this environment)
- **Committed in:** `1e03d69`

---

**Total deviations:** 1 auto-fixed (1 missing critical)
**Impact on plan:** The guardrails were necessary to make the planned AI submission path fail closed without expanding scope.

## Issues Encountered
- The repository's multi-target regression run still aborts on `net48` in this environment because `testhost.net48.exe` is unavailable; the targeted `net8.0` runs passed and were used for execution verification.

## User Setup Required

None - no external service configuration required.

## Next Phase Readiness
- Phase 07 plan 02 can build on the now-pinned upload contract and explicit mission-page state helpers for completion visibility work.
- No new endpoint or schema surface was introduced, so completion tracking can continue through existing `Works` and `MenuWorks` patterns.

## Known Stubs

None.

## Deviations from Threat Model

None.

## Self-Check: PASSED

- Verified `.planning/phases/07-submission-and-completion-tracking/07-submission-and-completion-tracking-01-SUMMARY.md` exists.
- Verified `student/uploadwork.aspx.cs`, `student/uploadworkm.aspx.cs`, `student/showmission.aspx.cs`, and `Tests/TeacherRegressionTests/TeacherRegressionTests.cs` exist.
- Verified commits `1e03d69`, `70030dc`, and `a2eba25` exist in git history.
