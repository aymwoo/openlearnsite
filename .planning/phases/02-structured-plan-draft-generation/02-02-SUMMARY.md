---
phase: 02-structured-plan-draft-generation
plan: 02
subsystem: ui
tags: [webforms, javascript, teacher-ui, regression-tests]
requires:
  - phase: 02-01
    provides: structured backend draft response for activityPlan
provides:
  - structured preview rendering in the existing course editor side panel
  - preview metadata, progress states, and copy/export for structured drafts
  - safe DOM rendering that keeps generated output out of lesson content
affects: [teacher-course-editor, phase-3-review-foundation]
tech-stack:
  added: []
  patterns: [right-side read-only preview cards, safe DOM construction, structured copy export]
key-files:
  created: []
  modified:
    - teacher/courseedit.aspx
    - js/courseedit.js
    - Tests/TeacherRegressionTests/TeacherRegressionTests.cs
key-decisions:
  - "Render separate read-only cards in the existing right-side assistant panel instead of one continuous AI text block."
  - "Keep the preview explicitly non-destructive: generated drafts are copyable but never auto-applied to lesson content."
patterns-established:
  - "Structured preview renderer pattern: list cards plus expanded step cards built from server draft JSON."
  - "Regression lock pattern: assert panel IDs, request wiring, and safe render hooks from source files."
requirements-completed: [PLAN-01, PLAN-02, PLAN-03, PLAN-04, FLOW-02]
duration: implemented before 2026-04-10
completed: 2026-04-10
---

# Phase 2 Plan 2: Course-editor structured preview Summary

**Structured right-side activity-plan preview cards with safe rendering and
copy support**

## Overview

This summary backfills the already-implemented browser work for Phase 2. The
course editor now renders the `activityPlan` response as a structured preview in
the existing sidebar panel instead of showing one free-text AI result.

The rendered output remains preview-only. Teachers can review and copy the
draft, but the page does not write generated content into the lesson editor or
save it automatically.

## Accomplishments

- Kept the Phase 1 right-side assistant panel in `teacher/courseedit.aspx` as
  the only preview host.
- Updated `js/courseedit.js` to render `res.data.draft` into section cards and
  expanded activity-step blocks.
- Added preview metadata showing the provider and skill name used for the
  current draft.
- Added structured copy/export through `buildActivityPlanCopyText()`.
- Locked the panel and safe rendering hooks in
  `Tests/TeacherRegressionTests/TeacherRegressionTests.cs`.

## Actual UI behavior

The panel still gathers the same topic-first request fields, then shows:

- progress text and percentage updates during generation
- a placeholder while waiting for the structured draft
- a preview metadata line after success
- separate cards for `教学目标`, `活动步骤`, `教学资源`, `评价设计`, and
  `教师提醒`

Each activity step is expanded by default and shows:

- title and minutes
- `教师活动`
- `学生活动`
- `互动方式`
- `资源建议`
- `评价检查`

## Files created or modified

- `teacher/courseedit.aspx` - Retains the assistant panel markup, result area,
  copy button, and progress container used by the structured preview.
- `js/courseedit.js` - Renders structured cards, tracks the last successful
  response for copy, and manages placeholder, progress, and failure states.
- `Tests/TeacherRegressionTests/TeacherRegressionTests.cs` - Asserts panel
  markup, `action=activityPlan` wiring, and safe rendering hooks.

## Safety and workflow notes

The implementation preserves the teacher-in-the-loop boundary from the phase
context.

- Successful rendering uses `document.createElement` and `textContent`.
- The browser clears invalid results instead of showing partially parsed raw
  provider text.
- The save button still runs the existing `syncContent()` path only for editor
  content, not for generated plan output.
- Copy is supported, but auto-apply and persistence are deferred to later
  phases.

## Tests locking the behavior

Focused regression coverage lives in
`Tests/TeacherRegressionTests/TeacherRegressionTests.cs`.

- `CourseEdit_ShouldContainActivityPlanAssistantPanelMarkup`
- `CourseEdit_ShouldKeepPlanningRequestAndSafeRenderingHooks`

## Phase readiness

This UI work completes the visible Phase 2 teacher review loop. The course
editor now has a stable read-only structured preview surface that later phases
can extend for section regeneration, selective write-back, and draft
persistence without changing the basic panel contract.

---

*Phase: 02-structured-plan-draft-generation*
*Backfilled from implementation: 2026-04-10*
