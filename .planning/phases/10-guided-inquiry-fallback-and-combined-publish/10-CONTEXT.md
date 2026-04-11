# Phase 10: Guided inquiry fallback and combined publish - Context

**Gathered:** 2026-04-11
**Status:** Ready for execution

<domain>
## Phase boundary

Phase 10 extends the Phase 8-9 full-lesson draft so a composed lesson can do two
new things inside the existing teacher workflow: use a guided inquiry block when
the supported existing activity types are not a good fit, and publish one mixed
full-lesson draft through a single explicit teacher action. The phase remains in
`teacher/courseedit.aspx`, keeps `teacher/aiprovider_api.ashx` as the server
boundary, and reuses the brownfield `Mission`, `Exam`, and `ListMenu` routes
instead of introducing a new publishing surface.

This phase is the first one allowed to create or update real classroom activity
rows from the composed full-lesson draft. It does not yet change student-side
composed navigation, block-to-block runtime sequencing, or per-block progress
visualization across a published mixed lesson; those remain Phase 11 work.

</domain>

<decisions>
## Implementation decisions

### Guided inquiry fallback contract
- **D-01:** Add a distinct `guidedInquiry` full-lesson block type as the fallback
  when `quiz`, `resource-study`, and `webCourseware` are not a good pedagogical
  fit for the lesson moment.
- **D-02:** `guidedInquiry` should publish through the existing guided mission
  path already exercised by `activityPlanPublish`, so the generated block can
  reuse the AI mission guidance rendering in `student/showmission.aspx` and
  `AIActivityPlanMissionViewHelper` instead of inventing a new student route.
- **D-03:** The guided inquiry payload should stay minimal and server-owned:
  enough structured fields to preview the inquiry goal, prompt/instructions,
  ordered steps, and submission expectation, without copying every legacy
  `Mission` column into the draft schema.

### Combined publish boundary
- **D-04:** Add a new `fullLessonPublish` action in `teacher/aiprovider_api.ashx`
  parallel to `activityPlanPublish`, keeping the same teacher/course
  authorization boundary and one explicit confirmation step from the current
  course editor.
- **D-05:** Mixed publish must create or update the matching brownfield rows for
  each supported block type inside one transaction: `guidedInquiry` and
  `resource-study` via mission-backed entries, `webCourseware` via ware-style
  mission entries, and `quiz` via the exam path plus matching `ListMenu` rows.
- **D-06:** Republish must be idempotent at the block level. The system should
  track publish links by `blockKey` in a server-owned structure so a teacher can
  confirm an updated draft without blindly inserting duplicate `Mission`,
  `Exam`, or `ListMenu` records.
- **D-07:** The combined publish result should preserve block order when it fans
  out into existing classroom entries so the published lesson still reflects the
  composed draft sequence teachers reviewed.

### Course editor UX
- **D-08:** `teacher/courseedit.aspx` and `js/courseedit.js` remain the entry,
  preview, save/resume, and confirm-publish surface; Phase 10 should reuse the
  current publish row instead of adding a separate teacher page.
- **D-09:** Teachers must be able to preview `guidedInquiry` blocks in the same
  mixed full-lesson draft as existing types before publishing.
- **D-10:** Generate/save/resume/remove/regenerate remain preview-first. Only the
  explicit combined publish action may create or update brownfield activity rows
  or append publish-oriented lesson content.

### Scope and safety
- **D-11:** Phase 10 should fail closed on unsupported or malformed publishable
  block payloads rather than partially publishing a mixed lesson.
- **D-12:** Student-side composed execution, per-block completion visibility, and
  teacher progress dashboards remain deferred to Phase 11 even if Phase 10
  publishes multiple entries.

### The agent's discretion
- Prefer reusing the existing `activityPlanPublish` guided mission content shape
  for `guidedInquiry` instead of defining a second, incompatible inquiry runtime.
- Prefer the smallest server-owned publish-link persistence that can safely map
  `blockKey` to created brownfield ids before adding broader schema or migration
  surface.
- Keep browser publish requests centered on the current server-returned
  full-lesson DTO; do not invent a client-only publish model for mixed drafts.

</decisions>

<canonical_refs>
## Canonical references

**Downstream agents MUST read these before planning or implementing.**

### Phase scope and requirements
- `.planning/ROADMAP.md` — Phase 10 goal, requirement mapping, and success
  criteria.
- `.planning/PROJECT.md` — v1.2 milestone intent and brownfield constraints.
- `.planning/REQUIREMENTS.md` — `COMP-04`, `INQ-01`, and `INQ-02`.
- `.planning/STATE.md` — current milestone position after Phase 9 completion.
- `.planning/phases/09-existing-activity-block-composition/09-CONTEXT.md` —
  typed block payload decisions that Phase 10 extends.
- `.planning/phases/09-existing-activity-block-composition/09-DISCUSSION-LOG.md`
  — why Phase 9 stopped at preview-only and deferred publish.

### Full-lesson orchestration and publish surface
- `teacher/courseedit.aspx` — current full-lesson preview shell and publish row.
- `js/courseedit.js` — current full-lesson preview, save/resume, remove,
  regenerate, and still-single-activity publish wiring.
- `teacher/aiprovider_api.ashx` — authenticated route boundary for current
  `fullLesson*` and `activityPlanPublish` actions.
- `App_Code/Common/AIActivityPlanDraftHelper.cs` — current full-lesson block
  contract and typed payload validation.
- `App_Code/Common/AIActivityPlanSavedDraftHelper.cs` — saved-draft envelope and
  the current place to preserve server-owned publish metadata if needed.

### Brownfield publish and runtime contracts
- `App_Code/Common/AIActivityPlanPublishContentBuilder.cs` — current guided
  mission content builder used by the single-activity publish path.
- `App_Code/Dal/AIActivityPlanPublisher.cs` — current single-activity publish
  transaction that Phase 10 can extend or parallelize.
- `App_Code/Bll/AIActivityPlanPublisher.cs` — publish boundary from handler to
  DAL.
- `teacher/missionadd.aspx.cs` — mission-backed activity creation semantics.
- `teacher/wareadd.aspx.cs` — ware creation semantics backed by `Mission` plus
  `ListMenu.Ltype = 38`.
- `exam/examadd.aspx.cs` — quiz/exam creation semantics and course entry shape.
- `teacher/courseshow.aspx.cs` — teacher routing for `Ltype = 1`, `6`, `38`, and
  `39`.
- `student/Scm.master.cs` — student routing for mission, reading, ware, and exam
  entries.
- `student/showmission.aspx.cs` — existing guided mission display and submission
  shell reused by AI-generated inquiry activities.

### Regression coverage
- `Tests/CommonLogicTests/CommonLogicTests.csproj` — helper, content, and
  publish-contract coverage.
- `Tests/TeacherRegressionTests/TeacherRegressionTests.csproj` — source-lock
  coverage for course editor, handler wiring, and publish routing.

</canonical_refs>

<code_context>
## Existing code insights

### Reusable assets
- Phase 9 already gives the full-lesson draft a stable block envelope plus typed
  payloads for `quiz`, `resource-study`, and `webCourseware`.
- `teacher/courseedit.aspx` already shows a publish toggle/button row, but
  `js/courseedit.js` still wires that button to `activityPlanPublish`, not to a
  mixed full-lesson publish path.
- `teacher/aiprovider_api.ashx` already owns authenticated `fullLesson*` routes,
  but there is currently no `fullLessonPublish` action.

### Brownfield publish evidence
- The current `AIActivityPlanPublisher` publishes exactly one mission-backed
  activity plus one `ListMenu` row and stores one pair of linked ids on the
  saved draft record.
- `student/showmission.aspx.cs` already knows how to parse AI activity-guide
  HTML and expose goal/instructions/steps inside the student mission shell. That
  makes it a natural runtime target for the new `guidedInquiry` fallback block.
- Existing teacher and student navigation already distinguish reading (`Ltype=6`),
  ware (`38`), exam (`39`), and guided/upload mission (`1`) entries.

### Integration implications
- Combined publish cannot safely rely on title matching or append-only insertion
  because Phase 10 must support create-or-update semantics for a mixed lesson.
- The current one-pair `LinkedMissionId` / `LinkedListMenuId` model is
  insufficient for a multi-block lesson, so Phase 10 needs a server-owned
  publish-link mapping keyed by `blockKey`.
- Guided inquiry should land first as a typed block and preview, then flow into
  the mixed publish path using the same reviewed full-lesson DTO teachers see in
  the editor.

</code_context>

<specifics>
## Specific ideas

- Plan 01 should add the `guidedInquiry` payload contract, full-lesson fallback
  generation/regenerate support, saved-draft persistence, and recognizable
  preview rendering inside the existing course editor.
- Plan 02 should add the combined `fullLessonPublish` server action, mixed
  brownfield publish fan-out, block-level publish-link persistence, and teacher
  publish UI wiring/tests.
- A safe minimal guided inquiry payload likely needs a mission title, inquiry
  prompt/instructions, ordered steps, and submission expectations that can be
  rendered in preview and transformed into the existing mission-guide HTML.

</specifics>

<deferred>
## Deferred ideas

- Student-side composed lesson navigation that keeps the full lesson inside one
  runtime shell instead of separate legacy entry pages.
- Per-block completion and progress visibility across a mixed composed lesson.
- Support for more brownfield activity types beyond `quiz`, `resource-study`,
  `webCourseware`, and `guidedInquiry`.
- Rich publish diffing, partial publish of only some blocks, or drag-and-drop
  reorder after publish.

</deferred>

---

*Phase: 10-guided-inquiry-fallback-and-combined-publish*
*Context gathered: 2026-04-11*
