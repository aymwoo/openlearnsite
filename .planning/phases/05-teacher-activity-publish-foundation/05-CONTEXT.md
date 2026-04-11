# Phase 5: Teacher activity publish foundation - Context

**Gathered:** 2026-04-11
**Status:** Ready for planning

<domain>
## Phase boundary

Phase 5 turns approved AI activity-plan output inside `teacher/courseedit.aspx`
into one real, course-linked student activity record through an authenticated,
server-owned publish flow. The phase covers explicit publish control, linked
lesson-content update, and keeping teacher and student visibility in sync. It
does not add the new student guidance UI or submission/completion behavior from
Phases 6-7.

</domain>

<decisions>
## Implementation decisions

### Publish record lifecycle
- **D-01:** Re-publishing for the same course updates one linked AI activity for
  that course instead of creating multiple parallel AI activity records.
- **D-02:** The publish flow must own and persist the linkage between the course
  and its AI-backed mission/menu entry so later publishes resolve to the same
  record.

### Publish visibility control
- **D-03:** The publish toggle defaults to unpublished. Teachers must
  explicitly enable student visibility during the confirm flow.
- **D-04:** Publish state must stay synchronized across `Mission.Mpublish` and
  `ListMenu.Lshow` through one server-owned action so teacher and student views
  cannot drift.

### Student activity foundation
- **D-05:** Phase 5 publishes into the existing `student/showmission.aspx`
  route using `Ltype=1` and `Mupload=true` as the foundation contract.
- **D-06:** Phase 5 must reuse the existing `Mission` + `ListMenu` activity
  model instead of introducing a new AI-specific student entry type.

### Confirm action payload
- **D-07:** One teacher confirm action must both append the selected plan
  sections into course content and create or update the linked student activity
  entry.
- **D-08:** Course content keeps the existing append-only behavior from Phase 4.
  Only teacher-selected sections are written into `Courses.Ccontent`.
- **D-09:** The linked student activity receives the full generated activity
  instructions, not only the subset appended to course content.

### Existing defaults to override
- **D-10:** The new AI publish path must not inherit the current
  `teacher/missionadd.aspx` default of `CheckPublish.Checked=True`; the AI
  publish experience starts unpublished by default even though the legacy manual
  add page currently defaults to published.

### The agent's discretion
- Use the smallest internal linkage shape that reliably lets publish resolve to
  the same mission/menu pair on later updates.
- Choose the least invasive teacher UI that fits `teacher/courseedit.aspx`
  patterns, as long as explicit unpublished-by-default control remains clear.
- Decide the exact server endpoint contract and payload fields, provided the
  flow stays authenticated, course-authorized, and atomic from the teacher's
  perspective.

</decisions>

<canonical_refs>
## Canonical references

**Downstream agents MUST read these before planning or implementing.**

### Phase scope and milestone requirements
- `.planning/ROADMAP.md` — Phase 5 goal, requirements mapping, and success
  criteria.
- `.planning/PROJECT.md` — v1.1 milestone intent, brownfield constraints, and
  prior v1.0 decisions that Phase 5 must preserve.
- `.planning/REQUIREMENTS.md` — `TAP-01`, `TAP-02`, and `TAP-03` requirements
  plus out-of-scope guardrails.
- `.planning/STATE.md` — current milestone and session position.

### Existing teacher authoring flow
- `teacher/courseedit.aspx` — current embedded AI plan assistant UI and
  append-only apply controls.
- `teacher/courseedit.aspx.cs` — current course save path and course-level
  publish checkbox behavior.
- `js/courseedit.js` — current draft generation, save/load draft, section apply,
  and pre-generate replace-versus-resume logic.
- `teacher/aiprovider_api.ashx` — existing authenticated AI activity-plan
  generation and saved-draft endpoints, plus course authorization helper.

### Existing mission and menu model
- `teacher/missionadd.aspx` — legacy manual mission-create form defaults,
  including published-by-default and upload-enabled options.
- `teacher/missionadd.aspx.cs` — existing mission creation and `ListMenu`
  insertion flow.
- `teacher/missionedit.aspx.cs` — existing synchronized update path for
  `Mission` and `ListMenu` publish/title/type values.
- `teacher/courseshow.aspx.cs` — teacher course page entry point for adding and
  managing mission/menu items.
- `App_Code/Model/Mission.cs` — persisted mission fields including `Mpublish`,
  `Mupload`, `Mcontent`, and `Mcategory`.
- `App_Code/Model/ListMenu.cs` — persisted menu linkage fields including
  `Ltype`, `Lxid`, and `Lshow`.
- `App_Code/Bll/Mission.cs` — mission retrieval/update helpers used by teacher
  and student flows.
- `App_Code/Bll/ListMenu.cs` — menu update and visibility helpers.
- `App_Code/Dal/Mission.cs` — actual mission insert/update persistence.
- `App_Code/Dal/ListMenu.cs` — menu insert/update persistence and the current
  `GetMenu` / `GetShowedMenu` visibility contract.

### Existing draft continuity support
- `App_Code/Bll/CourseActivityPlanDraft.cs` — saved draft business wrapper.
- `App_Code/Dal/CourseActivityPlanDraft.cs` — current one-record-per-course
  upsert behavior for AI plan drafts.
- `App_Code/Common/AIActivityPlanSavedDraftHelper.cs` — draft snapshot shape and
  validation rules.

### Existing student activity routes
- `student/Scm.master.cs` — `Ltype` to student route mapping; `Ltype=1` routes
  to `student/showmission.aspx` and `Ltype=6` routes to
  `student/description.aspx`.
- `student/showmission.aspx` — current activity page shell with upload and work
  interaction affordances.
- `student/showmission.aspx.cs` — current mission display and upload-gated
  behavior.
- `student/description.aspx` — current read-only activity shell.
- `student/description.aspx.cs` — current read/acknowledge tracking path.

</canonical_refs>

<code_context>
## Existing code insights

### Reusable assets
- `js/courseedit.js`: already owns AI draft generation, section regeneration,
  saved-draft status, load, save, and append-only section apply behavior.
- `teacher/aiprovider_api.ashx`: already provides an authenticated teacher-side
  AJAX endpoint with `TryGetAuthorizedCourse(...)` that fits a publish action.
- `teacher/missionadd.aspx.cs`: already knows how to create a `Mission` row plus
  matching `ListMenu` row for a course.
- `teacher/missionedit.aspx.cs`: already shows the synchronization rule for
  updating mission publish state and the linked menu row together.
- `App_Code/Dal/CourseActivityPlanDraft.cs`: already enforces one saved draft
  per course, which matches the chosen one-linked-AI-activity-per-course model.

### Established patterns
- Teacher browser actions post to role-local `.ashx` handlers or normal Web
  Forms postbacks rather than introducing a new API layer.
- Authorization is cookie-based and course ownership checks happen server-side.
- Mission visibility uses dual fields: `Mission.Mpublish` and `ListMenu.Lshow`.
  Existing code keeps them synchronized on edit/toggle.
- Student navigation is driven by `ListMenu.Ltype`, so reusing an existing type
  is lower risk than creating a new route in Phase 5.
- Append-only course content updates are already implemented client-side in the
  course editor and should remain the lesson-content write pattern.

### Integration points
- Teacher-side integration starts in `teacher/courseedit.aspx` and
  `js/courseedit.js`, where the publish confirmation UI and payload assembly
  belong.
- Server-side integration should extend `teacher/aiprovider_api.ashx` so publish
  can share existing course authorization and AI draft context handling.
- Persistence integrates with `Mission` and `ListMenu` update/create paths.
- Student visibility integrates automatically through `student/Scm.master.cs`
  once the linked `ListMenu` row is shown with `Ltype=1`.

</code_context>

<specifics>
## Specific ideas

- The teacher-facing publish action is conceptually distinct from generic
  `missionadd.aspx`: it is an AI-plan confirm flow embedded inside
  `teacher/courseedit.aspx`, with unpublished-by-default behavior.
- The student-facing activity should be treated as the executable full version
  of the AI output, while course content remains a teacher-owned summary made of
  selected append-only sections.
- Re-publish must feel like “update this course's linked AI activity,” not “add
  another activity to the menu.”

</specifics>

<deferred>
## Deferred ideas

None — discussion stayed within phase scope.

</deferred>

---

*Phase: 05-teacher-activity-publish-foundation*
*Context gathered: 2026-04-11*
