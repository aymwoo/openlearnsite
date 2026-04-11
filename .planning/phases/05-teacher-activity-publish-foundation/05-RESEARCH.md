# Phase 5 research — teacher activity publish foundation

**Date:** 2026-04-11
**Discovery level:** 0 (existing codebase patterns only)
**Question answered:** What must be true to plan a safe Phase 5 publish flow in this Web Forms codebase?

## Summary

Phase 5 should extend the existing authenticated `teacher/aiprovider_api.ashx`
handler instead of creating a new API surface. The safest publish design is a
single server-owned publish action that:

1. validates teacher ownership with `TryGetAuthorizedCourse(...)`,
2. appends only teacher-selected sections into `Courses.Ccontent`,
3. creates or updates exactly one linked `Mission` + `ListMenu` pair for the
   course,
4. keeps `Mission.Mpublish` and `ListMenu.Lshow` synchronized from the same
   request, and
5. persists the linked `Mid` + `Lid` on the existing
   `CourseActivityPlanDraft` row so later re-publishes update instead of fork.

## Relevant code facts

### Teacher authoring flow

- `teacher/courseedit.aspx` already hosts the AI panel, draft banner, section
  selection, and append-only apply actions.
- `js/courseedit.js` already tracks selected sections, saved-draft status, and
  append-only editor writes through `appendActivityPlanSectionsToEditor(...)`.
- `teacher/courseedit.aspx.cs` still uses the normal lesson save path for final
  course updates and currently binds the legacy course publish checkbox.

### Existing authenticated handler pattern

- `teacher/aiprovider_api.ashx` already routes AI plan actions through
  `ProcessRequest` and uses `TryGetAuthorizedCourse(...)` for teacher-owned
  course checks.
- The draft endpoints already prove this handler can safely mutate per-course AI
  state without exposing raw provider details to the browser.

### Existing mission/menu behavior

- `teacher/missionadd.aspx.cs` shows the creation recipe: create `Mission`, then
  create matching `ListMenu`, with `Ltype=1` when `Mupload=true` and `Ltype=6`
  otherwise.
- `teacher/missionedit.aspx.cs` shows the update recipe: update `Mission`, then
  update the linked `ListMenu` title, `Ltype`, and `Lshow` together.
- `student/Scm.master.cs` routes `Ltype=1` to `student/showmission.aspx`.
- `student/showmission.aspx.cs` renders `Mission.Mcontent` and unlocks the
  upload-oriented activity shell when `Mupload=true`.

### Existing one-record-per-course storage

- `CourseActivityPlanDraft` already has a unique index on `Cid`, which matches
  the locked decision that one course owns one linked AI activity record.
- The current draft table stores request context and draft JSON, but it does not
  yet store the linked mission/menu identity needed for stable re-publish.

## Recommended implementation

### 1. Persist linkage on `CourseActivityPlanDraft`

Add nullable linkage fields to the existing draft record:

- `LinkedMissionId`
- `LinkedListMenuId`

This is the smallest brownfield-compatible shape that satisfies D-01 and D-02
without introducing a new table.

### 2. Create a dedicated publish contract and core service

Add a publish request/result contract plus a dedicated publish orchestrator in
`App_Code/Bll/` backed by a DAL class that performs the cross-table write as one
transactional unit.

Recommended request fields:

- `Cid`
- `Hid`
- `Topic`
- `PublishToStudents`
- `SelectedSectionKeys`
- `Draft`

Recommended result fields:

- `MissionId`
- `ListMenuId`
- `PublishedToStudents`
- `MissionTitle`
- `UpdatedCourseContent`

### 3. Keep the publish action server-owned

The browser should send selected sections plus the full current draft to a new
`action=activityPlanPublish` route in `teacher/aiprovider_api.ashx`. The server
should then:

1. rebuild append-only lesson content from the selected sections,
2. rebuild full student-facing mission content from the full draft,
3. update or create the linked `Mission` + `ListMenu` pair,
4. synchronize `Mission.Mpublish` and `ListMenu.Lshow`, and
5. return the updated course content so the editor can reflect the committed
   state.

This directly satisfies D-04 and D-07.

### 4. Use the existing student activity contract

Phase 5 should not add a new route type. The publish core should force:

- `Mupload=true`
- `Ltype=1`
- `Mcategory=0`

That keeps the output inside `student/showmission.aspx` per D-05 and D-06.

### 5. Override the legacy published-by-default behavior in the AI panel

`teacher/missionadd.aspx` defaults `CheckPublish.Checked=True`, but Phase 5 must
not inherit that default. The AI publish control inside `teacher/courseedit.aspx`
should be a separate control with an unchecked default and explicit copy that the
activity remains hidden until enabled.

## Concrete brownfield choices

- **Handler location:** keep publish in `teacher/aiprovider_api.ashx`
- **Link persistence:** extend `CourseActivityPlanDraft`
- **Student route:** keep `Ltype=1` → `student/showmission.aspx`
- **Mission upload mode:** force `Mupload=true`
- **Mission file type:** use `office` as the default upload-compatible mission
  file type until Phase 7 introduces a more specific submission contract
- **Mission title format:** derive from topic with `AI活动-` prefix and bound to
  the existing `ListMenu.Ltitle` width limit

## Risks and mitigations

| Risk | Why it matters | Mitigation |
|------|----------------|------------|
| Mission/menu drift | Teacher and student visibility could diverge | Update `Mission.Mpublish` and `ListMenu.Lshow` in one publish core path only |
| Duplicate AI activities per course | Re-publish could spam the menu | Persist and reuse `LinkedMissionId` + `LinkedListMenuId` |
| Browser-only append logic | Course content and mission creation could succeed separately | Move append calculation and final write into the server publish action |
| Legacy publish default leaks in | New AI flow could auto-publish unexpectedly | Add a separate unchecked AI publish toggle in `teacher/courseedit.aspx` |
| Student shell receives partial data | Showmission page would render a crippled activity | Build full mission content from the complete draft, not selected sections |

## Testing strategy

- Extend `Tests/CommonLogicTests/CommonLogicTests.cs` for:
  - linkage-field migration coverage,
  - append-block builder coverage,
  - full mission-content builder coverage,
  - publish-core source assertions.
- Extend `Tests/TeacherRegressionTests/TeacherRegressionTests.cs` for:
  - AI publish control markup,
  - unchecked publish default,
  - new JS action wiring,
  - new `activityPlanPublish` handler case.

## Validation Architecture

Phase 5 can be validated with existing xUnit infrastructure only. Use narrow C#
logic tests for the publish core and source-based regression tests for the UI and
handler wiring. No new framework is required.

**Quick run:**

`dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj --filter "FullyQualifiedName~ActivityPlan"`

**Full run:**

`dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj && dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj --filter "FullyQualifiedName~ActivityPlan|FullyQualifiedName~CourseEdit_Should"`

## Planning implications

- Plan the linkage persistence before the UI wiring.
- Make the transactional publish core the hardest early task.
- Keep the existing manual “apply selected sections” behavior intact; add a new
  publish action rather than replacing Phase 4 behavior.
- Do not plan student guidance UI or submission logic here; those belong to
  Phases 6 and 7.
