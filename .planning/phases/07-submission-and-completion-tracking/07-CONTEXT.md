# Phase 7: Submission and completion tracking - Context

**Gathered:** 2026-04-11
**Status:** Ready for planning

<domain>
## Phase boundary

Phase 7 closes the milestone by letting students submit a result from the
published AI activity flow and by recording completion through the existing
classroom tracking model. The phase starts from the Phase 6 mission page and
must reuse the established upload and `MenuWorks` patterns instead of building a
new submission stack. It does not redesign teacher grading, peer review, or a
new analytics system.

</domain>

<decisions>
## Implementation decisions

### Submission contract
- **D-01:** Phase 7 must reuse the existing `student/uploadwork.aspx` and
  `student/uploadworkm.aspx` style submission flow for AI-published activities
  that use `Mupload=true`.
- **D-02:** Submission identity continues to come from the linked
  `ListMenu.Lxid` -> `Mission.Mid` contract published in Phase 5.

### Completion model
- **D-03:** Successful submission should continue to record completion through
  the existing `MenuWorks` model keyed by `Klid` and `Ksid`.
- **D-04:** Phase 7 should not introduce an AI-specific completion table if the
  current `MenuWorks` plus `Works` records can distinguish submitted versus
  untouched activities.

### Student and teacher-visible outcomes
- **D-05:** The student mission page should make submission state clear enough
  that students know whether they can submit, resubmit, or are blocked after
  grading.
- **D-06:** Existing completion indicators such as menu finish states and
  classroom tracking should reflect the AI activity through the same brownfield
  mechanisms used by other upload tasks.

### Safety and scope
- **D-07:** Phase 7 must preserve existing upload restrictions, file-type rules,
  and authorization assumptions instead of bypassing them for AI activities.
- **D-08:** Teacher-facing review or status changes should be minimal and only
  added if current surfaces cannot distinguish completed AI submissions.

### The agent's discretion
- Keep submission work centered in the existing upload handlers and shared BLL
  methods if one path already encapsulates the right add-or-update behavior.
- Prefer a small shared helper when current upload handlers duplicate the same
  completion-record write needed for AI activities.
- Use the existing source-based test projects for coverage and rely on manual
  browser validation for the actual upload path when necessary.

</decisions>

<canonical_refs>
## Canonical references

**Downstream agents MUST read these before planning or implementing.**

### Phase scope and milestone requirements
- `.planning/ROADMAP.md` — Phase 7 goal, requirement mapping, and success
  criteria.
- `.planning/PROJECT.md` — v1.1 milestone intent and brownfield constraints.
- `.planning/REQUIREMENTS.md` — `SCT-01` and `SCT-02` requirement text.
- `.planning/STATE.md` — current milestone position after gap planning.
- `.planning/v1.1-v1.1-MILESTONE-AUDIT.md` — explicit broken submission and
  completion flow from the milestone audit.

### Existing student submission flow
- `student/showmission.aspx` — student mission shell that exposes the upload UI.
- `student/showmission.aspx.cs` — upload availability and prior-submission state.
- `student/uploadwork.aspx.cs` — primary per-student upload submission handler.
- `student/uploadworkm.aspx.cs` — alternate upload-mode submission handler.
- `student/uploadgroup.aspx.cs` and `student/uploadgroupm.aspx.cs` — group-flow
  references for existing submission patterns.

### Existing completion and work-tracking model
- `App_Code/Bll/Works.cs` — work add/update helpers and shared submission logic.
- `App_Code/Dal/Works.cs` — underlying work persistence.
- `App_Code/Model/Works.cs` — persisted student work shape.
- `App_Code/Bll/MenuWorks.cs` — activity completion record accessors.
- `App_Code/Dal/MenuWorks.cs` — `MenuWorks` add/update/query behavior.
- `App_Code/Model/MenuWorks.cs` — completion-record shape keyed by `Ksid` and
  `Klid`.
- `App_Code/Dal/Signin.cs` — classroom work-count update used by uploads.

### Existing completion surfaces
- `student/Scm.master.cs` — finish and lock icon behavior in the student menu.
- `student/myinfo.aspx.cs` — student completion summary surface.
- `teacher/start.aspx.cs` — teacher lesson visibility and activity context used
  during live class review.

### Existing validation assets
- `Tests/CommonLogicTests/CommonLogicTests.cs` — logic coverage target for shared
  upload or completion helpers.
- `Tests/TeacherRegressionTests/TeacherRegressionTests.cs` — source regression
  target for page and handler wiring.

</canonical_refs>

<code_context>
## Existing code insights

### Reusable assets
- `student/uploadwork.aspx.cs` already handles the normal upload lifecycle for a
  mission: resolve `lid` -> `mid`, update or add a `Works` row, add a
  `MenuWorks` row for first submission, and update daily classroom work counts.
- `student/showmission.aspx.cs` already determines whether the current student
  can submit, resubmit, or is blocked after grading.
- `student/Scm.master.cs` already uses completion and work-pass information to
  show finish icons in the class menu.

### Established patterns
- Upload submission is still server-side file handling in an `.aspx.cs` page,
  not an API controller.
- Completion state is recorded as a side effect of successful submission through
  `MenuWorks`, not a separate event stream.
- Brownfield upload behavior distinguishes between first submit and resubmit,
  and preserves the "teacher already evaluated" block on later changes.

### Integration points
- Phase 7 starts on the existing `showmission.aspx` page created and refined in
  Phases 5-6.
- Submission logic lives in `student/uploadwork*.aspx.cs` and possibly shared
  `App_Code/Bll/Works.cs` helpers if duplication should be reduced.
- Completion verification touches `MenuWorks` plus the student menu finish state.

</code_context>

<specifics>
## Specific ideas

- Treat the AI activity as a normal upload-capable mission that happens to be AI
  generated, not a special submission product.
- Prefer proving that the current upload handler already satisfies the AI case,
  then only add the smallest missing glue or tests.
- If the completion write currently happens only on the first submission path,
  make that behavior explicit and testable rather than assuming it covers all AI
  states automatically.

</specifics>

<deferred>
## Deferred ideas

- Rich teacher dashboards for AI activity review remain out of scope.
- New analytics, scoring redesign, or AI-specific submission rubrics remain out
  of scope for v1.1.

</deferred>

---

*Phase: 07-submission-and-completion-tracking*
*Context gathered: 2026-04-11*
