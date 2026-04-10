# Phase 2: Structured plan draft generation - Context

**Gathered:** 2026-04-10
**Status:** Ready for planning

<domain>
## Phase Boundary

Turn the existing activity-plan assistant from a free-text generator into a
teacher-reviewable structured draft generator inside `teacher/courseedit.aspx`.
This phase covers the server-side draft contract, structured AI output handling,
and structured preview rendering in the existing right-side panel. It does not
yet include per-section regeneration, write-back into lesson content, or saved
draft persistence.

</domain>

<decisions>
## Implementation Decisions

### Draft data shape
- **D-01:** Phase 2 uses a fixed top-level draft structure, not a flexible AI
  outline.
- **D-02:** The fixed top-level sections are teaching goals, ordered activity
  steps, resources, assessment, and a short teacher reminder.
- **D-03:** Each activity step must explicitly include title, minutes, teacher
  action, student action, and interaction method.
- **D-04:** Each activity step must also explicitly include a resource
  suggestion and an assessment check.

### Malformed output handling
- **D-05:** If the AI returns malformed or incomplete structured output, the
  server should normalize it when recovery is safe.
- **D-06:** After normalization, the draft is accepted only if all core
  sections are present and each activity step still contains the required step
  fields.
- **D-07:** If safe normalization still cannot satisfy the required draft
  contract, the request fails instead of falling back to plain text output.

### Draft preview layout
- **D-08:** The structured draft renders inside the existing right-side panel as
  separate read-only section cards, not one continuous document.
- **D-09:** Activity steps are expanded by default so teachers can review full
  step details without extra clicks.
- **D-10:** Phase 2 stays preview-only. No generated content is auto-applied,
  auto-published, or written back into the lesson editor in this phase.

### Skill and prompt control
- **D-11:** Phase 2 should follow the established gauge/exam pattern and load
  the stored scoped activity-plan custom skill as the `system` prompt.
- **D-12:** Teacher topic and structured inputs should be sent separately as the
  `user` message, rather than collapsing everything into a single prompt.
- **D-13:** Required draft schema rules stay enforced in code as hardcoded
  guardrails, even when the stored custom skill supplies pedagogical guidance.

### the agent's Discretion
- Exact property names for the structured draft model, as long as they map
  cleanly to the locked sections and step fields above.
- Exact visual treatment of the section cards within the established
  `courseedit.aspx` panel style, as long as the result remains read-only and
  scannable.
- Exact normalization heuristics for near-valid model output, as long as they do
  not silently accept drafts that miss required sections or fields.

</decisions>

<canonical_refs>
## Canonical References

**Downstream agents MUST read these before planning or implementing.**

### Product scope and phase contract
- `.planning/PROJECT.md` — Product goal, pedagogical structure goal, brownfield
  constraints, and teacher-in-the-loop requirement.
- `.planning/REQUIREMENTS.md` — Phase 2 requirement coverage for PLAN-01,
  PLAN-02, PLAN-03, PLAN-04, and FLOW-02.
- `.planning/ROADMAP.md` — Phase 2 goal, success criteria, and boundary versus
  later review/regenerate/apply/persistence phases.
- `.planning/STATE.md` — Current project focus plus unresolved downstream
  concerns that should stay out of Phase 2 scope.
- `.planning/phases/01-embedded-planning-entry/01-CONTEXT.md` — Locked Phase 1
  decisions that Phase 2 must carry forward.

### Existing AI generation patterns and contracts
- `App_Code/Bll/AIGaugeGenerator.cs` — Existing pattern for loading a scoped
  custom skill as `system` prompt, sending a separate `user` prompt, and parsing
  structured AI output.
- `App_Code/Bll/AIStudentExamGenerator.cs` — Existing pattern for AI generation
  with structured parsing, fallbacks, and saved custom skill usage.
- `App_Code/Common/AIActivityPlanPromptBuilder.cs` — Current activity-plan input
  shaping rules that must evolve from free-text prompting into a structured
  draft contract.
- `App_Code/Bll/AIActivityPlanSkillBootstrap.cs` — Existing scoped skill seed for
  activity-plan generation that Phase 2 should begin using at runtime.
- `teacher/aiprovider_api.ashx` — Current `activityPlan` route that must be
  upgraded from plain text chat output to a structured draft response.

### Existing course editor integration points
- `teacher/courseedit.aspx` — Existing right-side assistant panel host that
  continues to be the Phase 2 preview surface.
- `js/courseedit.js` — Existing request assembly, loading state, and safe text
  rendering path that will become the structured draft preview renderer.

</canonical_refs>

<code_context>
## Existing Code Insights

### Reusable Assets
- `teacher/courseedit.aspx`: Already contains the embedded right-side activity
  plan panel introduced in Phase 1, so Phase 2 should upgrade the result area
  rather than build a new entry surface.
- `js/courseedit.js`: Already gathers topic, optional guidance, and existing
  course content, and includes safe text-only result rendering plus progress UI.
- `App_Code/Bll/AIGaugeGenerator.cs`: Demonstrates the repo's preferred pattern
  for `system` + `user` prompting, provider calling, and structured response
  parsing.
- `App_Code/Bll/AIStudentExamGenerator.cs`: Demonstrates structured JSON parsing,
  fallback logic, and custom-skill-backed AI generation in the current codebase.
- `App_Code/Bll/AIActivityPlanSkillBootstrap.cs`: Already defines the scoped
  activity-plan skill seed and gives Phase 2 a concrete custom-skill anchor.

### Established Patterns
- Brownfield AI features in this repo work through ASP.NET handlers plus page
  JavaScript, not through a separate frontend app or API tier.
- Mature AI generation paths in this repo send separate `system` and `user`
  messages and then parse a machine-usable response shape on the server.
- Teacher-facing editing pages preserve human review and explicit user action,
  which reinforces the preview-only boundary for this phase.
- Browser-side rendering favors explicit DOM writes and safe text rendering over
  trusting AI-generated HTML.

### Integration Points
- `teacher/aiprovider_api.ashx` remains the AI provider integration boundary for
  `activityPlan` requests.
- A new structured activity-plan generator can sit alongside existing AI BLL
  generators and be called from `activityPlan` instead of the current plain text
  helper path.
- `js/courseedit.js` is the main Phase 2 client integration point for replacing
  plain text rendering with structured section-card rendering.
- `teacher/courseedit.aspx` remains the main Phase 2 UI host and should keep the
  existing panel-first editing workflow intact.

</code_context>

<specifics>
## Specific Ideas

- Treat Phase 2 as the point where the activity-plan assistant commits to a
  stable structured draft model that later phases can reuse.
- Keep the output teacher-reviewable and visible in the side panel, not mixed
  directly into lesson content yet.

</specifics>

<deferred>
## Deferred Ideas

- Per-section regeneration belongs to Phase 3.
- Selective write-back into lesson content belongs to Phase 4.
- Saved draft persistence belongs to Phase 4.

</deferred>

---

*Phase: 02-structured-plan-draft-generation*
*Context gathered: 2026-04-10*
