# Phase 1 Research: Embedded planning entry

**Researched:** 2026-04-10
**Phase:** 01-embedded-planning-entry
**Confidence:** Medium

## Summary

Phase 1 should extend `teacher/courseedit.aspx` instead of creating a new page or
AI stack. The closest proven UI pattern is the right-side assistant panel in
`teacher/missionadd.aspx`, while the existing provider boundary remains
`teacher/aiprovider_api.ashx`. The safest brownfield move is to add a dedicated
activity-plan entry panel on the course editor, collect structured fields there,
compose the final prompt on the server, and route the request through the
existing provider + custom-skill infrastructure.

## What exists already

### Editor host

- `teacher/courseedit.aspx` already contains the lesson title, grade selector,
  period selector, and main content editor.
- `teacher/courseedit.aspx.cs` already loads decoded course content into
  `mcontent.Value`, which can become automatic planning background context.
- `js/courseedit.js` already manages editor switching and page-scoped browser
  behavior, so it is the correct extension point for a course-editor assistant.

### Reusable AI pattern

- `teacher/missionadd.aspx` already ships a right-side AI assistant panel with
  local progress UI, result area, and insert actions.
- `js/missionadd.js` already shows the brownfield pattern for button states,
  progress feedback, and XHR calls into `aiprovider_api.ashx`.
- `teacher/aiprovider_api.ashx` already enforces teacher-cookie auth and routes
  generation through the configured default AI provider.

### Existing skill infrastructure

- `App_Code/Model/AICustomSkill.cs` defines scoped custom skills with
  `SkillName`, `PromptContent`, `SkillScope`, and `IsActive`.
- `App_Code/Bll/AIGaugeGenerator.cs` demonstrates the current pattern for
  ensuring a default scoped skill exists before an AI workflow runs.
- `App_Code/Utility/UpdateGrade.cs` already seeds `AICustomSkill` rows for other
  scoped AI workflows, confirming that activity-planning can reuse the same
  skill-table model.

## Recommended implementation shape

1. Add a dedicated activity-plan assistant panel to `teacher/courseedit.aspx`
   per D-01, D-02, D-08, and D-09.
2. Keep topic / knowledge point as the only required field, with optional grade,
   duration, and teaching-goals controls per D-03, D-04, and D-05.
3. Read existing course editor content client-side and send it automatically when
   present per D-06.
4. Compose the final prompt on the server, not in editable UI text, so the
   system can enforce topic-first precedence per D-07 and structured-fields-only
   interaction per D-09.
5. Extend `teacher/aiprovider_api.ashx` with a dedicated activity-plan action or
   equivalent routed branch that:
   - validates the required topic field,
   - ensures a scoped planning skill exists,
   - combines topic, optional inputs, and existing content into a server-built
     prompt, and
   - calls the existing default provider without introducing a new endpoint or
     provider client.

## Why not a separate planning endpoint stack

- Phase 1 only needs embedded entry and brownfield reuse, not a new subsystem.
- `aiprovider_api.ashx` already centralizes auth and provider routing.
- Creating a separate AI service now would violate FLOW-03 and create avoidable
  Phase 2 migration work.

## Brownfield risks and mitigations

| Risk | Why it matters | Mitigation |
|------|----------------|------------|
| Generic chat UX leaks back in | Violates D-08/D-09 and weakens the product framing | Use dedicated labels, structured fields, and a server-composed prompt |
| Existing course HTML is injected unsafely into the result area | AI results and editor content cross trust boundaries | Render response text safely with `textContent`/`innerText`, not `innerHTML` |
| Large existing lesson content produces oversized prompts | Existing `mcontent` can be long and noisy | Trim and bound supporting context server-side before calling the provider |
| Course editor regressions break save/editor switching | `courseedit.aspx` already has regression-sensitive editor hooks | Preserve current editor config and add assertions in `TeacherRegressionTests` |

## Testing anchors

- `Tests/TeacherRegressionTests/TeacherRegressionTests.cs` already locks editor
  hooks for `courseedit.aspx` and `missionadd.aspx`; Phase 1 should add markup /
  hook assertions there.
- `Tests/CommonLogicTests/CommonLogicTests.cs` already hosts pure helper and data
  mapping tests; prompt-building and scoped-skill helper logic should land here.
- `Tests/CommonLogicTests/CommonLogicTests.csproj` already links `App_Code`
  helper/model files directly, so any new planning helper must be linked there
  for fast isolated verification.

## Validation Architecture

The fastest reliable validation loop for this phase is:

1. Pure helper tests in `Tests/CommonLogicTests/CommonLogicTests.cs` for prompt
   composition, topic-priority rules, optional-field inclusion, and skill-scope
   defaults.
2. Regression tests in `Tests/TeacherRegressionTests/TeacherRegressionTests.cs`
   for page markup, assistant-panel identifiers, and preserved editor hooks.
3. Manual browser verification can stay optional for execution, but plans should
   be executable from automated coverage first.

Recommended quick commands:

- `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj --filter "FullyQualifiedName~ActivityPlan"`
- `dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj --filter "FullyQualifiedName~CourseEdit"`

## Planning implications

- Split work into server contract first, then UI wiring, because the UI needs a
  stable request shape.
- Keep file ownership clean: backend changes in the first plan, course editor UI
  and regression coverage in the second plan.
- Do not plan subject-field work in Phase 1; subject was not selected in
  CONTEXT.md.

## Recommendation

Plan Phase 1 as two execute plans:

1. Server-side activity-plan request shaping and provider reuse.
2. Course-editor embedded assistant panel and regression coverage.
