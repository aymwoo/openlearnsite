# Phase 9 Discussion Log

**Date:** 2026-04-11
**Mode:** discuss

## Inputs reviewed

- `.planning/ROADMAP.md`
- `.planning/REQUIREMENTS.md`
- `.planning/STATE.md`
- `.planning/PROJECT.md`
- `.planning/phases/08-full-lesson-draft-orchestration/08-CONTEXT.md`
- `teacher/courseedit.aspx`
- `js/courseedit.js`
- `teacher/aiprovider_api.ashx`
- `teacher/missionadd.aspx.cs`
- `teacher/missionshow.aspx.cs`
- `teacher/wareadd.aspx.cs`
- `teacher/wareshow.aspx.cs`
- `exam/examadd.aspx.cs`
- `teacher/courseshow.aspx.cs`
- `student/Scm.master.cs`

## Gray areas reviewed

1. Which brownfield route should represent `resource-study` in Phase 9?
2. Which existing assessment path should represent `quiz` in Phase 9?
3. How far should Phase 9 go: preview payloads only, publish-ready payloads, or
   real entity creation?

## Decisions captured

1. `resource-study` will reuse the mission reading path, modeled after
   `Mission + ListMenu.Ltype = 6`.
2. `quiz` will reuse the classroom exam path, modeled after
   `Exam + ListMenu.Ltype = 39`.
3. `webCourseware` remains the ware path, modeled after mission-backed ware
   records with `ListMenu.Ltype = 38`.
4. Phase 9 stops at typed preview plus minimal legacy payloads; it will not
   create real rows or introduce combined publish.

## Planning implications

1. The next phase plan should define minimal payload schemas for the three block
   types and add validation/tests first.
2. UI work should stay inside the existing course editor assistant and render
   server-owned type-aware summaries.
3. Phase 10 remains the first phase allowed to turn these payloads into real
   publish fan-out behavior.
