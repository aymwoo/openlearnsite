---
phase: 04-selective-apply-and-draft-continuity
plan: 02
subsystem: ui
tags: [webforms, javascript, teacher-ui, saved-drafts, selective-apply, regression-tests]
requires:
  - phase: 03-02
    provides: structured preview rendering and section-card interaction patterns in the course editor
  - phase: 04-01
    provides: authenticated saved-draft status, save, load, and delete actions on the AI route
provides:
  - append-only selective apply controls for one or many top-level plan sections
  - explicit confirmation before activity-plan content is inserted into the lesson editor
  - saved-draft banner, save, resume, and replace-versus-continue flows inside the existing panel
affects: [teacher-course-editor, phase-4-verification, teacher-editing-workflow]
tech-stack:
  added: []
  patterns: [append-only editor apply, explicit resume banner, generate-time draft status recheck]
key-files:
  created: []
  modified:
    - teacher/courseedit.aspx
    - js/courseedit.js
    - Tests/TeacherRegressionTests/TeacherRegressionTests.cs
key-decisions:
  - "Apply approved plan sections by appending labeled blocks into the existing editor content instead of calling a second lesson-save path."
  - "Recheck saved-draft status on generate so replace-versus-continue prompts do not rely only on stale browser state."
patterns-established:
  - "Append-only apply pattern: confirm selected top-level sections, escape all dynamic draft text, and sync the active editor without auto-saving the lesson."
  - "Resume banner pattern: show saved-draft existence in-panel, resume only on explicit teacher action, and gate replacement behind a continue-or-replace prompt."
requirements-completed: [EDIT-03, EDIT-04]
duration: 26m
completed: 2026-04-10
---

# Phase 4 Plan 2: Selective apply and resume UI summary

**Append-only section apply, explicit confirmation, and in-panel save/resume draft continuity for the teacher course editor**

## Performance

- **Duration:** 26m
- **Started:** 2026-04-10T16:07:00Z
- **Completed:** 2026-04-10T16:32:08Z
- **Tasks:** 2
- **Files modified:** 3

## Accomplishments

- Added per-section apply controls plus a multi-select apply action inside the
  existing activity-plan preview panel.
- Added explicit confirmation that names the selected sections before any draft
  content is appended into `mcontent`.
- Kept apply behavior append-only and escaped dynamic draft text before building
  inserted lesson blocks.
- Added saved-draft banner, explicit resume, save-draft, and
  replace-versus-continue flows without auto-loading a saved draft on page open.
- Added focused regression coverage for the new panel markup and client-side
  apply/resume wiring.

## Task Commits

Each task was committed atomically:

1. **Task 1: Add explicit append-only apply controls for one or many top-level sections** - `8322187`, `a838ad5` (test, feat)
2. **Task 2: Add saved-draft resume, save, and replace-versus-continue UI flows inside the existing panel** - `00d4af5`, `4c7bd95` (test, feat)

**Plan metadata:** pending final docs commit

_Note: Both tasks used TDD-style source-lock regressions before implementation._

## Files Created/Modified

- `teacher/courseedit.aspx` - Adds the saved-draft banner plus save/apply
  action buttons inside the existing assistant panel.
- `js/courseedit.js` - Adds selection state, confirmation, escaped append-only
  editor insertion, saved-draft status checks, save/resume requests, and
  replace-versus-continue handling.
- `Tests/TeacherRegressionTests/TeacherRegressionTests.cs` - Adds source-lock
  coverage for apply controls, append-only editor updates, and saved-draft UI
  hooks.

## Decisions Made

- Kept apply entirely inside the existing page editor flow so teachers still use
  the normal `保存学案` button for final persistence.
- Used explicit `window.confirm(...)` prompts for both apply confirmation and
  replace-versus-continue decisions to keep the brownfield UI lightweight.
- Rechecked draft status before starting a new generation request so the replace
  path does not trust stale panel state.

## Deviations from Plan

### Auto-fixed Issues

**1. [Rule 2 - Missing Critical] Rechecked saved-draft status before replace flow**
- **Found during:** Task 2 (saved-draft resume, save, and replace-versus-continue UI flows)
- **Issue:** The initial UI implementation tracked saved-draft existence in local state only, which could become stale before a new generation attempt.
- **Fix:** Added a fresh `activityPlanDraftStatus` request inside the generate-time gate before showing the continue-or-replace prompt.
- **Files modified:** `js/courseedit.js`, `Tests/TeacherRegressionTests/TeacherRegressionTests.cs`
- **Verification:** `dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj --framework net8.0 --filter "FullyQualifiedName~CourseEdit"`
- **Committed in:** `00d4af5`, `4c7bd95`

---

**Total deviations:** 1 auto-fixed (1 missing critical)
**Impact on plan:** The extra status recheck tightened correctness at the saved-draft trust boundary without changing the intended UX scope.

## Issues Encountered

- The regression suite is source-lock based, so UI verification focused on
  markup and client hook contracts rather than browser-executed interaction
  tests.

## User Setup Required

None - no external service configuration required.

## Next Phase Readiness

- Phase 4 is now functionally complete: teachers can save and resume drafts and
  selectively append approved sections into the editor.
- Remaining follow-up is final verification and any manual browser checks from
  the validation plan.

## Self-Check: PASSED
