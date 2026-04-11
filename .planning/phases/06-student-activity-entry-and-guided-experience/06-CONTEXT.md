# Phase 6: Student activity entry and guided experience - Context

**Gathered:** 2026-04-11
**Status:** Ready for planning

<domain>
## Phase boundary

Phase 6 turns the Phase 5 teacher publish foundation into a usable student
entry flow. The phase covers the published menu entry, authenticated student
open flow, and a guided mission page that renders the published AI activity
goal, instructions, and steps clearly inside the existing `student/showmission`
experience. It does not change the underlying submission handler contract or
completion-record persistence from Phase 7.

</domain>

<decisions>
## Implementation decisions

### Student entry contract
- **D-01:** Phase 6 must keep the existing `ListMenu.Ltype=1` ->
  `student/showmission.aspx?lid=` route introduced in Phase 5.
- **D-02:** Student entry visibility continues to depend on the synchronized
  `Mission.Mpublish` and `ListMenu.Lshow` values created by the publish flow.

### Student mission rendering
- **D-03:** The published AI activity remains stored in `Mission.Mcontent` and
  must render through the existing mission page rather than a new student page.
- **D-04:** Phase 6 should add the smallest rendering help needed so the page
  surfaces the goal, instructions, and step sequence clearly even when the
  mission content originated from AI publish instead of a hand-authored mission.

### Guided learner experience
- **D-05:** Student guidance must come from published server content only. The
  student page cannot depend on teacher draft state, local storage, or the
  teacher-side editor.
- **D-06:** Guidance enhancements should preserve the existing upload sidebar
  and activity shell so Phase 7 can reuse the same page for submission.

### Safety and scope
- **D-07:** If the mission or menu record is missing, hidden, or invalid, the
  student page should fail closed with the existing not-found behavior instead
  of exposing unpublished AI output.
- **D-08:** Phase 6 should not introduce a new standalone student SPA, custom
  route type, or AI-specific menu model.

### The agent's discretion
- Choose the smallest mission-content markers or helper methods that make AI
  guidance readable without breaking older mission HTML.
- Keep any new UI additions inside `student/showmission.aspx` and its existing
  CSS/JS hooks unless a pure helper under `App_Code/Common/` is materially
  cleaner.
- Use source-based regression coverage where possible, then reserve manual UAT
  for the authenticated browser path and final readability checks.

</decisions>

<canonical_refs>
## Canonical references

**Downstream agents MUST read these before planning or implementing.**

### Phase scope and milestone requirements
- `.planning/ROADMAP.md` — Phase 6 goal, requirement mapping, and success
  criteria.
- `.planning/PROJECT.md` — v1.1 milestone intent, brownfield constraints, and
  preserved teacher-planning decisions.
- `.planning/REQUIREMENTS.md` — `SAE-01`, `SAE-02`, and `SAE-03` requirement
  text plus out-of-scope guardrails.
- `.planning/STATE.md` — current milestone state after Phase 5 and the
  milestone-gap audit.
- `.planning/v1.1-v1.1-MILESTONE-AUDIT.md` — explicit Phase 6 gap list and
  broken teacher-publish -> student-open flow.

### Existing publish foundation
- `App_Code/Bll/AIActivityPlanPublisher.cs` — teacher publish entry point that
  creates the linked mission/menu identity.
- `App_Code/Dal/AIActivityPlanPublisher.cs` — current publish persistence and
  mission-content write path.
- `App_Code/Common/AIActivityPlanPublishContentBuilder.cs` — current full
  mission-content builder for AI activity output.
- `teacher/aiprovider_api.ashx` — authenticated teacher publish route.

### Existing student entry and display flow
- `student/Scm.master.cs` — current menu rendering, `Ltype` route mapping, and
  finished/locked menu visuals.
- `student/showmission.aspx` — current student mission shell, content area, and
  upload panel.
- `student/showmission.aspx.cs` — current mission load, content render, and
  upload-gated behavior.
- `js/showmission.js` — current student mission client behavior.
- `App_Themes/Student/showmission.css` — current mission page styling.

### Existing completion and submission context to preserve
- `student/uploadwork.aspx.cs` — current upload submission path that Phase 7
  will reuse.
- `App_Code/Bll/MenuWorks.cs` — completion-record accessors used by the student
  menu flow.
- `App_Code/Dal/MenuWorks.cs` — underlying `MenuWorks` persistence and query
  contract.

### Existing validation assets
- `Tests/CommonLogicTests/CommonLogicTests.cs` — source and logic tests for
  brownfield helpers.
- `Tests/TeacherRegressionTests/TeacherRegressionTests.cs` — source regression
  tests used for page, handler, and route wiring.

</canonical_refs>

<code_context>
## Existing code insights

### Reusable assets
- `student/Scm.master.cs` already reads visible `ListMenu` rows and sends
  `Ltype=1` entries to `student/showmission.aspx?lid=`.
- `student/showmission.aspx.cs` already loads the linked mission from `lid`,
  decodes `Mission.Mcontent`, and exposes the upload panel when
  `Mission.Mupload=true`.
- `student/showmission.aspx` already contains a modernized content shell with a
  main content card and a sidebar, which is enough to host clearer AI guidance
  without introducing a new page.
- `js/content-show-markdown.js` and `js/showmission.js` already provide client
  hooks for content rendering, so Phase 6 can piggyback on the existing mission
  page lifecycle.

### Established patterns
- Student navigation is still server-rendered Web Forms plus light JavaScript,
  not a client-routed app.
- Teacher publish owns mission creation and visibility; the student side should
  consume published data, not recalculate it.
- Existing student mission behavior is tolerant of rich HTML content, hidden
  fields, and page-local rendering toggles.
- Student completion icons in the left menu depend on existing work and
  `MenuWorks` records rather than a separate AI completion table.

### Integration points
- Phase 6 entry starts with the `ListMenu` row created in Phase 5.
- Readability and guidance work belongs primarily in
  `student/showmission.aspx`, `student/showmission.aspx.cs`, and possibly a
  focused helper under `App_Code/Common/`.
- Regression coverage can stay in `TeacherRegressionTests` for route/page source
  assertions and `CommonLogicTests` for any new helper behavior.

</code_context>

<specifics>
## Specific ideas

- Treat the published AI mission as a special case of the existing mission page,
  not a brand-new activity product.
- Preserve the current upload sidebar placement so the later submission phase
  does not need to re-lay out the whole page.
- Prefer robust server-rendered section wrappers or parsing helpers over browser
  heuristics that only work for one exact HTML string shape.

</specifics>

<deferred>
## Deferred ideas

- Teacher-facing submission review changes belong to Phase 7.
- Rich adaptive learner guidance, differentiated paths, or multiple student
  presentation modes remain out of scope for v1.1.

</deferred>

---

*Phase: 06-student-activity-entry-and-guided-experience*
*Context gathered: 2026-04-11*
