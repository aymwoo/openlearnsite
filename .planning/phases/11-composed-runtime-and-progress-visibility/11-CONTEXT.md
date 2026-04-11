# Phase 11: Composed runtime and progress visibility - Context

**Gathered:** 2026-04-11
**Status:** Ready for planning

<domain>
## Phase boundary

Phase 11 is the first student-side phase after mixed full-lesson publish. Phase 10
already creates or updates the brownfield `Mission`, `Exam`, and `ListMenu`
records for each composed block and persists stable `blockKey -> ids` publish
links on the saved full-lesson draft. Phase 11 must make those published blocks
feel like one coherent composed lesson inside the existing student and teacher
surfaces, without introducing a new standalone runtime shell or AI-only
completion store.

The work stays inside the current brownfield classroom navigation and progress
models: student entry continues to flow through `student/Scm.master.cs` and the
existing activity pages, while completion visibility continues to rely on
`MenuWorks`, `Works`, and current course summary surfaces. This phase may add the
minimum server-owned metadata needed to relate published blocks back to the
composed lesson draft, but it should not redesign the whole course player or
replace legacy progress semantics.

</domain>

<decisions>
## Implementation decisions

### Runtime entry and composed navigation
- **D-01:** Student entry must continue to use the existing `ListMenu`-driven
  classroom navigation in `student/Scm.master.cs`; Phase 11 should not add a new
  AI-specific lesson player page.
- **D-02:** The composed lesson runtime should reuse the existing type-specific
  student pages selected by `ListMenu.Ltype`: guided/upload mission (`1`),
  resource-study reading (`6`), web courseware (`38`), and exam (`39`).
- **D-03:** Phase 11 should surface composed-block context from server-owned
  publish metadata so students can tell where a block sits in the full lesson,
  rather than guessing order from independent menu items only.

### Completion and progress visibility
- **D-04:** Per-block completion should stay backed by existing completion
  evidence: `MenuWorks` for menu completion, existing `Works` submission state
  for mission-backed uploads, and the current exam completion path for quizzes.
- **D-05:** Student-facing completion cues should distinguish progress across the
  mixed published blocks in one composed lesson without adding a second AI-only
  status table.
- **D-06:** Teacher-facing progress visibility should reuse the current course and
  activity status surfaces where possible, but add the smallest safe summary that
  relates composed blocks back to the mixed full-lesson publish result.

### Publish metadata boundary
- **D-07:** The saved full-lesson draft's existing `publishLinks` structure is the
  canonical place to relate `blockKey` to published brownfield ids; prefer
  extending that server-owned payload with block runtime/progress metadata over
  inventing a broader schema or title-matching heuristic.
- **D-08:** Any runtime/progress helper introduced in this phase must fail closed
  when publish links, block payloads, or brownfield ids are incomplete, rather
  than reporting misleading completion states.

### Scope and safety
- **D-09:** Phase 11 is about composed runtime continuity and visibility after
  publish. It should not reopen teacher-side draft generation, drag-and-drop
  editing, or general progress-dashboard redesign.
- **D-10:** The phase should prefer minimal metadata overlays and helper methods
  around current pages over broad changes to `Mission`, `Exam`, `ListMenu`, or
  the course editor publish contract.

### The agent's discretion
- Prefer adding a reusable helper that resolves composed publish links and block
  runtime summaries from the saved full-lesson draft instead of scattering JSON
  parsing across student and teacher pages.
- Prefer showing block order/type/title/progress in the current menu or summary
  surfaces rather than introducing a second composed-lesson navigation widget.
- Keep completion logic grounded in current brownfield evidence sources even if
  the phase adds a cleaner composed summary view on top.

</decisions>

<canonical_refs>
## Canonical references

**Downstream agents MUST read these before planning or implementing.**

### Phase scope and requirements
- `.planning/ROADMAP.md` — Phase 11 goal, requirement mapping, and success
  criteria.
- `.planning/PROJECT.md` — v1.2 milestone intent and student-flow constraints.
- `.planning/REQUIREMENTS.md` — `RUN-01`, `RUN-02`, `RUN-03`, and `RUN-04`.
- `.planning/STATE.md` — current milestone position after Phase 10 completion.
- `.planning/phases/10-guided-inquiry-fallback-and-combined-publish/10-CONTEXT.md`
  — mixed publish decisions and explicit Phase 11 deferrals.
- `.planning/phases/10-guided-inquiry-fallback-and-combined-publish/10-DISCUSSION-LOG.md`
  — why Phase 10 stopped at publish and deferred composed runtime/progress.
- `.planning/phases/10-guided-inquiry-fallback-and-combined-publish/10-guided-inquiry-fallback-and-combined-publish-02-SUMMARY.md`
  — shipped mixed publish behavior and `publishLinks` readiness.

### Publish metadata and mixed publish output
- `App_Code/Common/AIActivityPlanSavedDraftHelper.cs` — saved full-lesson draft
  envelope and `publishLinks` payload.
- `App_Code/Dal/AIActivityPlanPublisher.cs` — mixed publish fan-out and block
  result ids.
- `App_Code/Model/AIActivityPlanPublishResult.cs` — server return shape for
  published full-lesson blocks.

### Student runtime and completion surfaces
- `student/Scm.master.cs` — current `ListMenu`-driven student navigation and
  finish-state icon logic.
- `student/showmission.aspx.cs` — guided mission runtime and upload/completion
  guidance.
- `student/description.aspx.cs` — reading/runtime page for `Ltype = 6`.
- `student/ware.aspx.cs` — web courseware runtime page for `Ltype = 38`.
- `webform/preview.aspx.cs` — exam runtime page for `Ltype = 39`.
- `student/myinfo.aspx.cs` — student completed-course aggregation and summary
  surface.

### Teacher visibility surfaces
- `teacher/courseshow.aspx.cs` — teacher course menu display and per-activity
  navigation after mixed publish.
- `teacher/missionadd.aspx.cs`, `teacher/wareadd.aspx.cs`,
  `exam/examadd.aspx.cs` — brownfield semantics that Phase 11 must continue to
  respect.

### Completion evidence sources
- `App_Code/Bll/MenuWorks.cs` and `App_Code/Dal/MenuWorks.cs` — duplicate-safe
  completion state for menu items and existing course-level completion queries.
- `App_Code/Bll/Works.cs` — upload-backed completion writes for mission-style
  activities.

### Regression coverage
- `Tests/CommonLogicTests/CommonLogicTests.csproj` — helper and mapping tests.
- `Tests/TeacherRegressionTests/TeacherRegressionTests.csproj` — source-lock
  tests for student/teacher runtime and completion visibility code paths.

</canonical_refs>

<code_context>
## Existing code insights

### Reusable student runtime assets
- `student/Scm.master.cs` already routes menu items by `Ltype` and shows a finish
  icon when current completion evidence says a block is done.
- `student/showmission.aspx.cs` already renders AI-guided mission content and
  keeps explicit upload/completion restrictions, so guided inquiry can continue
  to rely on this route.
- Existing student pages for reading, ware, and exam are already the correct
  brownfield runtime targets for `resource-study`, `webCourseware`, and `quiz`.

### Reusable completion evidence
- Phase 7 already established `MenuWorks` as the duplicate-safe completion store
  for menu-backed activity completion.
- Upload mission handlers already call `Works.EnsureMenuWorksCompletion(...)`, so
  mission-backed composed blocks inherit existing completion writes.
- `student/myinfo.aspx.cs` and `student/Scm.master.cs` already aggregate or show
  completion based on legacy sources, not AI-specific state.

### Integration implications
- Phase 10 mixed publish now produces the exact `ListMenuId`, `MissionId`,
  `ExamId`, and `PaperId` references Phase 11 needs to build a composed runtime
  summary without title matching.
- Current student finish-state logic counts completions by raw `ListMenu` order
  and count. Phase 11 likely needs block-aware metadata so completion visibility
  is tied to the composed lesson block set rather than inferred only by menu
  position.
- Teacher and student progress views do not yet distinguish which menu items came
  from one composed full-lesson publish, so Phase 11 needs a minimal correlation
  helper for that relationship.

</code_context>

<specifics>
## Specific ideas

- Plan 01 should add a server-owned helper that resolves composed publish blocks,
  their brownfield ids, runtime targets, and per-block completion state from the
  saved full-lesson draft plus current legacy evidence sources.
- Plan 01 should also update regression/common tests to pin fail-closed handling
  when publish links are incomplete or drift from the draft.
- Plan 02 should wire the composed runtime/progress summaries into the current
  student and teacher surfaces, likely centered on `student/Scm.master.cs`,
  `student/myinfo.aspx.cs`, and `teacher/courseshow.aspx.cs`, while keeping
  existing runtime pages per `Ltype`.
- If a page-level runtime hint is needed, it should be a minimal composed block
  summary or breadcrumb, not a new dedicated player shell.

</specifics>

<deferred>
## Deferred ideas

- Reordering composed blocks after publish.
- A brand-new cross-activity runtime shell for students.
- Broad analytics dashboards or teacher-wide reporting redesign.
- Support for more legacy activity types beyond the current mixed publish set.
- Replacing current `MenuWorks` / `Works` evidence models with a new AI-specific
  completion store.

</deferred>

---

*Phase: 11-composed-runtime-and-progress-visibility*
*Context gathered: 2026-04-11*
