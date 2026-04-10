# Phase 1: Embedded planning entry - Context

**Gathered:** 2026-04-10
**Status:** Ready for planning

<domain>
## Phase Boundary

Embed an activity-plan starting point into the existing teacher editing workflow
so teachers can begin planning from the lesson editor with only a topic or
knowledge point required. This phase covers entry placement, lightweight input
capture, reuse of existing lesson context, and reuse of the current AI provider
path. It does not yet cover full plan generation behavior, draft review, or
applying generated sections back into lesson content.

</domain>

<decisions>
## Implementation Decisions

### Entry placement
- **D-01:** Phase 1 starts in `teacher/courseedit.aspx` only. It does not add a
  first-pass planning entry to `teacher/missionadd.aspx`.
- **D-02:** The activity-plan entry uses a right-side panel layout so the main
  course editor remains primary while the planning assistant stays visible.

### Planning input shape
- **D-03:** The only required teacher input is a topic or knowledge point.
- **D-04:** Optional guidance is collected through structured fields that expand
  beyond the topic-first entry, rather than through a generic freeform prompt.
- **D-05:** The explicit optional fields for Phase 1 are grade, duration, and
  teaching goals.

### Context reuse rules
- **D-06:** When existing course content is already present in the editor,
  Phase 1 includes that content automatically in the planning request.
- **D-07:** If the typed topic and existing editor content conflict, the typed
  topic or knowledge point is treated as the teacher's current intent and takes
  precedence, while existing content is supporting background.

### Assistant framing
- **D-08:** The entry is presented as a dedicated activity-plan assistant, not
  as a generic AI writing helper.
- **D-09:** Teachers interact through structured fields only in Phase 1. The
  system composes the request instead of exposing an editable final prompt.

### the agent's Discretion
- Exact field labels, panel copy, and visual styling within the established
  `courseedit.aspx` design language.
- Whether optional fields are shown by default or behind a compact
  expand/collapse control, as long as topic remains the only required field.
- The exact request-shaping logic used to combine topic, optional inputs, and
  current editor content before the provider call.

</decisions>

<canonical_refs>
## Canonical References

**Downstream agents MUST read these before planning or implementing.**

### Product scope and phase contract
- `.planning/PROJECT.md` — Product goal, v1 scope, brownfield constraints, and
  teacher-in-the-loop requirement.
- `.planning/REQUIREMENTS.md` — Phase 1 requirement coverage for INPUT-01,
  INPUT-02, INPUT-03, INPUT-04, FLOW-01, and FLOW-03.
- `.planning/ROADMAP.md` — Phase 1 goal, success criteria, and boundary versus
  later generation/review/apply phases.
- `.planning/STATE.md` — Current project focus and unresolved concerns that may
  influence planning details.

### Existing teacher editor and AI entry points
- `teacher/courseedit.aspx` — Primary Phase 1 host surface for the embedded
  planning entry.
- `teacher/courseedit.aspx.cs` — Existing lesson metadata and editor-content
  sources available for automatic planning context reuse.
- `teacher/missionadd.aspx` — Existing embedded AI side-panel pattern used as a
  reusable UI reference.
- `teacher/missionadd.aspx.cs` — Existing editor payload handling pattern for
  teacher content authoring.
- `js/missionadd.js` — Current assistant-side request, progress, and insertion
  behavior that shows how the brownfield UI currently talks to AI.
- `teacher/aiprovider_api.ashx` — Existing provider routing and chat endpoint
  path that Phase 1 must reuse instead of introducing a new AI stack.

</canonical_refs>

<code_context>
## Existing Code Insights

### Reusable Assets
- `teacher/courseedit.aspx`: Existing course editing page with grade, term,
  lesson-period, publish, title, and content fields already present.
- `teacher/courseedit.aspx.cs`: Server-side access to `Texttitle`, grade,
  course metadata, and decoded `mcontent`, which can supply planning context.
- `teacher/missionadd.aspx`: A proven right-side assistant panel layout inside
  a teacher editing page.
- `js/missionadd.js`: Existing client-side patterns for button state, progress
  UI, request lifecycle, and content insertion.
- `teacher/aiprovider_api.ashx`: Existing authenticated handler that resolves
  the default AI provider and sends a chat-style request.

### Established Patterns
- Teacher editing flows live in `teacher/*.aspx` with paired code-behind files,
  and preserve existing postback/save behavior rather than replacing the page
  architecture.
- Browser-side helpers use page-scoped global JavaScript rather than modular
  frontend frameworks.
- AI functionality is already embedded into teacher pages as an assistant panel
  that posts to `aiprovider_api.ashx`, so Phase 1 should extend that pattern
  instead of inventing a separate service boundary.
- Existing editor pages treat rich content fields as primary authoring surfaces,
  so the planning panel should augment the editor rather than replace it.

### Integration Points
- `teacher/courseedit.aspx` is the Phase 1 UI integration point.
- `teacher/courseedit.aspx.cs` is the server-side source for current lesson
  fields and existing content to reuse as prompt context.
- `teacher/aiprovider_api.ashx` is the AI request path that should remain the
  provider integration boundary in Phase 1.
- `js/courseedit.js` is the likely page script extension point for new panel
  behavior on the course editor surface.

</code_context>

<specifics>
## Specific Ideas

- Reuse the successful right-side AI assistant mental model already visible in
  `teacher/missionadd.aspx`, but narrow the framing to activity-plan start
  rather than generic content generation.
- Keep the planning interaction lightweight: topic first, optional fields second,
  and no editable raw prompt in Phase 1.

</specifics>

<deferred>
## Deferred Ideas

None — discussion stayed within phase scope.

</deferred>

---

*Phase: 01-embedded-planning-entry*
*Context gathered: 2026-04-10*
