# Phase 6 research — student activity entry and guided experience

**Date:** 2026-04-11
**Discovery level:** 0 (existing codebase patterns only)
**Question answered:** What is the smallest safe way to expose published AI
activities to students through the current class menu and mission page?

## Summary

Phase 6 should stay on the existing `ListMenu.Ltype=1` ->
`student/showmission.aspx` contract created in Phase 5. The student entry gap is
not a missing route; it is missing phase-scoped implementation and verification
that the publish flow produces a visible menu entry and a readable learner
experience on the mission page. The safest design is:

1. verify and lock the publish-to-menu route contract with regression tests,
2. add focused mission-content rendering help so AI activity sections display as
   clear learner guidance inside `student/showmission.aspx`, and
3. verify the authenticated browser path manually because no dedicated student
   regression test project exists.

## Relevant code facts

### Menu entry already exists in the brownfield flow

- `App_Code/Dal/AIActivityPlanPublisher.cs` already forces `Ltype=1`,
  `Mupload=true`, `Mpublish`, and `Lshow` together when teachers publish an AI
  activity.
- `student/Scm.master.cs` already maps `Ltype=1` to
  `~/student/showmission.aspx?lid=`.
- Student menu visibility already depends on `GetShowedMenu(...)`, so Phase 6
  should confirm the publish foundation feeds that contract instead of adding a
  parallel entry mechanism.

### Mission page already renders the needed payload

- `student/showmission.aspx.cs` already resolves the `ListMenu` row by `lid`,
  loads the linked `Mission`, decodes `Mission.Mcontent`, and writes it into the
  `Mcontent` server element.
- `student/showmission.aspx` already includes a modernized content card and a
  stable sidebar, so readable AI guidance can be layered into the current page.
- The page already preserves the raw mission HTML in `HiddenMissionRaw`, which
  gives flexibility for rendering logic without fetching teacher-side state.

### Submission surface must remain intact

- The same mission page already controls upload availability through
  `Mission.Mupload` and `Panelworks`, which Phase 7 will reuse.
- Replacing `showmission.aspx` with another page would create unnecessary risk
  for submission, existing upload widgets, and menu routing.

## Recommended implementation

### 1. Lock the publish-to-student route contract

Add source-based tests that pin the exact brownfield route:

- publish core sets `Ltype=1` and `Mupload=true`
- `student/Scm.master.cs` sends `Ltype=1` entries to `showmission.aspx`
- the menu still depends on shown entries only

This closes the audit gap around teacher publish -> student menu -> page open.

### 2. Add minimal AI mission guidance rendering on `showmission`

Use the smallest possible helper or server-side shaping to recognize the AI
publish mission format and surface it as explicit learner guidance sections.

Recommended rendered concepts:

- activity goal
- learner instructions
- ordered activity steps
- resources or assessment reminders when present

The implementation should preserve older mission HTML and fail closed to raw
content rendering when structured AI markers are absent.

### 3. Keep verification on existing test infrastructure

Because there is no dedicated student regression project, Phase 6 should use:

- `TeacherRegressionTests` for source assertions about route/page wiring
- `CommonLogicTests` for any new mission-guidance parsing or formatting helper
- manual browser UAT for authenticated teacher publish -> student menu ->
  showmission readability

## Concrete brownfield choices

- **Student route:** keep `student/showmission.aspx?lid=`
- **Menu contract:** keep `ListMenu.Ltype=1` and `ListMenu.Lshow`
- **Mission contract:** keep `Mission.Mcontent` as the published source of truth
- **Rendering approach:** prefer server-owned shaping or helper parsing over a
  separate AI-only student page
- **Verification stack:** reuse `CommonLogicTests` and
  `TeacherRegressionTests`, then add manual UAT for the authenticated path

## Risks and mitigations

| Risk | Why it matters | Mitigation |
|------|----------------|------------|
| Route drift | Students could miss the activity even though teachers published it | Pin `Ltype=1` and `showmission.aspx` routing with regression tests |
| AI mission content is hard to follow | Published activities might technically open but fail SAE-02 and SAE-03 | Add explicit goal and step rendering in the existing mission shell |
| Overfitting to one HTML blob shape | Older mission content could regress | Fail closed to raw mission content when AI-specific markers are absent |
| Breaking upload behavior | Phase 7 depends on the existing submission shell | Keep guidance changes additive inside `showmission.aspx` |

## Testing strategy

- Extend `Tests/TeacherRegressionTests/TeacherRegressionTests.cs` for:
  - `Ltype=1` -> `showmission.aspx` route assertions,
  - student mission page markup for guidance sections,
  - source assertions that the mission page still preserves upload affordances.
- Extend `Tests/CommonLogicTests/CommonLogicTests.cs` or a focused new test file
  for:
  - AI mission-content parse/format helper coverage,
  - fail-closed behavior when guidance markers are absent,
  - ordered step rendering coverage.
- Run manual browser UAT with one published AI activity and one student login to
  verify the menu entry, page open, and readable learner guidance.

## Validation architecture

Phase 6 requires both source-level and manual validation. Existing C# test
projects can pin the route and helper behavior, but the end-to-end student menu
and authenticated page open still need browser verification because there is no
student UI regression project in this repository.

**Quick run:**

`dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj --filter "FullyQualifiedName~ActivityPlan|FullyQualifiedName~ShowMission|FullyQualifiedName~Student"`

**Full run:**

`dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj && dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj`

## Planning implications

- Plan route/contract verification before mission-page UI refinement so the
  entry path is locked first.
- Keep guidance rendering in the current mission page to avoid cascading Phase 7
  rework.
- Reserve one plan for manual validation and completion of any remaining page
  shaping after the route contract is pinned.
