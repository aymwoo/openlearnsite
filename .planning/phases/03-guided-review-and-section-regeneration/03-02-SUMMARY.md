---
phase: 03-guided-review-and-section-regeneration
plan: 02
subsystem: ui
tags: [webforms, javascript, teacher-ui, regression-tests, section-regeneration]
requires:
  - phase: 02-02
    provides: structured preview cards in the course editor
  - phase: 03-01
    provides: merged section-regeneration backend route
provides:
  - section-level regenerate controls in the preview panel
  - local loading and error state per top-level section
  - preview-preserving failure handling for section retries
affects: [teacher-course-editor, phase-3-review-loop]
tech-stack:
  added: []
  patterns: [section-card actions, local retry state, stable preview rerender]
key-files:
  created: []
  modified:
    - teacher/courseedit.aspx
    - js/courseedit.js
    - Tests/TeacherRegressionTests/TeacherRegressionTests.cs
key-decisions:
  - "Keep one regenerate action per top-level preview section and avoid any accept/reject or per-step retry controls in phase 3."
  - "Preserve the last valid preview on all failure paths and localize retry feedback to the targeted section card."
patterns-established:
  - "Section-local retry pattern: request context reuse, keyed section state, and merged full-draft replacement only after success."
  - "Preview safety pattern: rerender from `lastActivityPlanDraftResponse` and never write regenerated content into lesson editor fields."
requirements-completed: [EDIT-01, EDIT-02]
duration: implemented 2026-04-10
completed: 2026-04-10
---

# Phase 3 Plan 2: Guided review UI summary

This plan extends the structured preview in `teacher/courseedit.aspx` into a
guided review loop. Each top-level section now has its own regenerate action,
and retry failures leave the last valid full preview in place.

## Accomplishments

- Added one regenerate action for `teachingGoals`, `activitySteps`,
  `resources`, `assessment`, and `teacherReminder`.
- Added local loading and error messaging for each targeted section card while
  keeping the rest of the preview visible.
- Wired section retries to `action=activityPlanRegenerateSection` and posted the
  current full draft JSON plus the requested section key.
- Updated the preview rerender path so `lastActivityPlanDraftResponse` is
  replaced only after a successful merged response.
- Added regression coverage for section-regeneration hooks and failure-safe
  preview behavior.

## Files modified

- `teacher/courseedit.aspx` - Adds the small section action and status styles
  used inside the existing assistant panel.
- `js/courseedit.js` - Adds section-local retry state, section action rendering,
  and preview-preserving success and failure handling.
- `Tests/TeacherRegressionTests/TeacherRegressionTests.cs` - Adds source-level
  assertions for section retry wiring and protected preview behavior.

## Validation

The focused UI regression command passed:

`dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj --framework net8.0 --filter "FullyQualifiedName~CourseEdit"`

## Notes

This phase still keeps the preview read-only. No section-regeneration path
applies content into `mcontent`, editor instances, or saved lesson data.

---

*Phase: 03-guided-review-and-section-regeneration*
*Completed: 2026-04-10*
