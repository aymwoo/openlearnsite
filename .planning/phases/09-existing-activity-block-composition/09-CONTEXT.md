# Phase 9: Existing activity block composition - Context

**Gathered:** 2026-04-11
**Status:** Ready for planning

<domain>
## Phase boundary

Phase 9 extends the shipped full-lesson draft from Phase 8 so the generated
blocks can carry minimal brownfield payloads for three existing activity types:
quiz, resource-study, and web courseware. The work stays inside
`teacher/courseedit.aspx`, keeps `teacher/aiprovider_api.ashx` as the server
boundary, and remains preview-first. It does not yet publish composed blocks,
create combined teacher actions, or change student runtime orchestration.

</domain>

<decisions>
## Implementation decisions

### Activity mapping
- **D-01:** `quiz` blocks should target the existing exam flow created through
  `exam/examadd.aspx.cs` and represented in course navigation by
  `ListMenu.Ltype = 39`.
- **D-02:** `resource-study` blocks should target the existing mission reading
  flow by creating mission-shaped data that would map to `Mission` plus
  `ListMenu.Ltype = 6`, not `TxtForm`.
- **D-03:** `webCourseware` blocks should target the existing ware flow stored
  in `Mission` with `Mcategory = 38`, `Mfiletype = "ware"`, `Mback` homepage,
  and `ListMenu.Ltype = 38`.

### Phase 9 contract depth
- **D-04:** Phase 9 should stop at typed preview payloads and minimal legacy
  contracts. It should not create real `Mission`, `Exam`, or `ListMenu` rows in
  this phase.
- **D-05:** The full-lesson block DTO should keep the Phase 8 stable block
  envelope and add a type-specific payload per block rather than branching into
  unrelated client-only shapes.
- **D-06:** Type-specific payloads should carry only the minimum preview and
  publish-preparation fields needed for planning, validation, and future Phase
  10 publish fan-out.

### UX boundary
- **D-07:** `teacher/courseedit.aspx` and `js/courseedit.js` should render a
  richer preview for each supported legacy type so teachers can tell what quiz,
  reading block, or ware block AI selected without opening a separate page.
- **D-08:** Block-level remove and regenerate must continue to operate on the
  server-owned full-lesson DTO and must not mutate `mcontent` or create hidden
  side effects in brownfield tables.

### Scope and safety
- **D-09:** Phase 9 should keep the current preview-only operating model from
  Phase 8 and leave combined publish, fallback guided inquiry blocks, and
  student-side composed execution to Phases 10-11.
- **D-10:** Payload validation should fail closed when required legacy fields
  for `quiz`, `resource-study`, or `webCourseware` are missing or malformed.

### The agent's discretion
- Prefer reusing existing brownfield field names where practical so Phase 10 can
  publish with shallow translation instead of inventing a second domain model.
- Prefer minimal payloads that are human-readable in preview and sufficient for
  later publish fan-out, instead of copying every legacy table column into the
  draft schema.
- Treat `resource-study` as a mission-reading block, because the current
  teacher/student routes already distinguish readable content through
  `ListMenu.Ltype = 6` and `missionshow.aspx` / `student/description.aspx`.

</decisions>

<canonical_refs>
## Canonical references

**Downstream agents MUST read these before planning or implementing.**

### Phase scope and requirements
- `.planning/ROADMAP.md` — Phase 9 goal, requirement mapping, and success
  criteria.
- `.planning/PROJECT.md` — v1.2 milestone intent and brownfield constraints.
- `.planning/REQUIREMENTS.md` — `COMP-01`, `COMP-02`, and `COMP-03`.
- `.planning/STATE.md` — current milestone position after Phase 8 completion.
- `.planning/phases/08-full-lesson-draft-orchestration/08-CONTEXT.md` — prior
  orchestration decisions that Phase 9 extends.

### Full-lesson orchestration surface
- `teacher/courseedit.aspx` — current assistant shell and full-lesson preview.
- `js/courseedit.js` — current block rendering, draft state, save/load/remove,
  and regenerate behavior.
- `teacher/aiprovider_api.ashx` — authenticated AI route boundary for full-lesson
  actions.
- `App_Code/Common/AIActivityPlanDraftHelper.cs` — current full-lesson helper
  and validation entry point.

### Brownfield activity contracts
- `teacher/missionadd.aspx.cs` — mission creation flow and the `ListMenu.Ltype`
  split between upload activity (`1`) and reading block (`6`).
- `teacher/missionshow.aspx.cs` — mission reading display, especially `Mcontent`,
  `Mupload`, and read-only semantics.
- `teacher/wareadd.aspx.cs` — ware creation flow using mission storage,
  `Mcategory = 38`, `Mfiletype = "ware"`, and `Mback` homepage.
- `teacher/wareshow.aspx.cs` — ware preview route based on `Mission.Mback`.
- `exam/examadd.aspx.cs` — classroom exam creation flow and `ListMenu.Ltype = 39`.
- `teacher/courseshow.aspx.cs` — teacher-side course navigation routing for
  `Ltype = 6`, `38`, and `39`.
- `student/Scm.master.cs` — student-side routing for reading, ware, and exam
  entries (`description.aspx`, `ware.aspx`, `webform/preview.aspx`).

### Regression coverage
- `Tests/CommonLogicTests/CommonLogicTests.csproj` — helper and validation tests.
- `Tests/TeacherRegressionTests/TeacherRegressionTests.csproj` — source-lock
  coverage for `courseedit.aspx`, `courseedit.js`, and `aiprovider_api.ashx`.

</canonical_refs>

<code_context>
## Existing code insights

### Reusable orchestration assets
- Phase 8 already established a stable full-lesson DTO with `blockKey`, `sort`,
  `blockType`, `teachingPurpose`, `lessonPosition`, and `minutes`.
- `teacher/aiprovider_api.ashx` already owns authenticated full-lesson actions,
  so Phase 9 should enrich the existing block contract instead of adding a new
  page or route family.
- `js/courseedit.js` already renders server-returned blocks and supports remove,
  regenerate, save, and resume against the current draft envelope.

### Brownfield activity evidence
- Reading blocks already exist through the mission stack. In
  `teacher/missionadd.aspx.cs`, non-upload activities are persisted as
  `Mission` rows with `Mcontent`, `Mupload = false`, and `ListMenu.Ltype = 6`.
- Teacher course navigation routes `Ltype = 6` to `missionshow.aspx`, and the
  student menu routes the same type to `student/description.aspx`.
- Web courseware already exists through `teacher/wareadd.aspx.cs`, which stores
  a mission-shaped record with `Mcategory = 38`, `Mfiletype = "ware"`,
  `Mupload = true`, and `Mback` as the homepage URL, then adds
  `ListMenu.Ltype = 38`.
- Teacher course navigation routes `Ltype = 38` to `wareshow.aspx`, and the
  student menu routes it to `student/ware.aspx`.
- Quiz blocks already exist through `exam/examadd.aspx.cs`, which creates an
  `Exam` row and, when launched from a course, adds `ListMenu.Ltype = 39`.
- Teacher course navigation routes `Ltype = 39` to `~/webform/exam.aspx`, and
  the student menu routes it to `~/webform/preview.aspx`.

### Integration implications
- `resource-study` is the one brownfield type that does not need a separate new
  entity family; it can reuse the existing mission-reading route with a smaller
  payload centered on title and HTML/markdown content.
- `webCourseware` and `quiz` need different payload shapes because they map to
  different downstream stores: ware reuses `Mission`, while quiz uses the `Exam`
  aggregate.
- Phase 9 should therefore add typed nested payloads under the existing block
  contract, not flatten every legacy field into one generic block schema.

</code_context>

<specifics>
## Specific ideas

- Plan 01 should inventory and codify the minimal payload schemas for `quiz`,
  `resource-study`, and `webCourseware`, then extend helper validation and tests
  around those shapes.
- Plan 02 should update `teacher/aiprovider_api.ashx`, `teacher/courseedit.aspx`,
  and `js/courseedit.js` so the assistant can render type-aware preview cards
  using server-returned payload summaries.
- A safe minimal `resource-study` payload likely needs a title plus content
  summary/body source; a safe minimal `webCourseware` payload likely needs a
  title plus homepage path/url; a safe minimal `quiz` payload likely needs a
  title plus assessment summary and enough structured fields to later create an
  `Exam`.

</specifics>

<deferred>
## Deferred ideas

- Combined publish fan-out into `Mission`, `Exam`, and `ListMenu` records.
- Guided inquiry fallback blocks and mixed composed publish.
- Student-side composed runtime sequencing and per-block progress visibility.
- AI explanations for why a type was chosen.
- Support for more brownfield activity types beyond the three Phase 9 targets.

</deferred>

---

*Phase: 09-existing-activity-block-composition*
*Context gathered: 2026-04-11*
