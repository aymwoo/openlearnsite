# Phase 3: Guided review and section regeneration - Context

**Gathered:** 2026-04-10
**Status:** Ready for planning

<domain>
## Phase Boundary

Extend the existing structured activity-plan preview into a guided review loop
inside `teacher/courseedit.aspx`. This phase adds targeted section regeneration
for weak parts of the draft while preserving strong sections and keeping the
lesson editor untouched. It covers section-level regenerate actions, targeted
server regeneration, strict validation, safe merging back into the current
draft, and non-destructive preview updates. It does not include write-back into
lesson content, explicit per-section acceptance state, or saved draft
persistence.

</domain>

<decisions>
## Implementation Decisions

### Regeneration granularity
- **D-01:** Phase 3 supports regeneration only for top-level sections, not for
  individual steps or full-draft-only retry.
- **D-02:** The supported section targets are `teachingGoals`, `activitySteps`,
  `resources`, `assessment`, and `teacherReminder`.

### Merge behavior
- **D-03:** When a section is regenerated, the backend returns a fully merged
  draft instead of only the isolated regenerated section.
- **D-04:** The server owns the safe-merge flow: it receives the current draft,
  regenerates only the target section, validates that section against the fixed
  contract, merges it into the current draft, and returns the merged result.

### Review controls
- **D-05:** Phase 3 adds section-level regenerate actions only.
- **D-06:** Phase 3 does not add explicit lock, keep, accept, reject, or
  approval state per section. Teachers preserve strong sections simply by not
  regenerating them.

### Loading and failure behavior
- **D-07:** Regeneration loading and failure states are local to the targeted
  section card, not global to the whole panel.
- **D-08:** While one section is regenerating, the rest of the preview remains
  visible and unchanged.
- **D-09:** If regeneration fails, the UI keeps the last valid full draft on
  screen rather than replacing it with partial or invalid content.

### the agent's Discretion
- Exact endpoint, helper, and method naming for section-regeneration server code,
  as long as it remains on the existing authenticated provider route.
- Exact visual treatment of section-local loading and error states within the
  established course editor side-panel style.
- Whether merge/validation helpers extend the existing draft helper or live in a
  nearby dedicated helper, as long as the fixed structured draft contract stays
  centralized and enforced.

</decisions>

<canonical_refs>
## Canonical References

**Downstream agents MUST read these before planning or implementing.**

### Product scope and prior locked decisions
- `.planning/PROJECT.md` — Product goal, teacher-in-the-loop boundary, and
  brownfield constraints.
- `.planning/REQUIREMENTS.md` — Phase 3 requirement coverage for PLAN-05,
  FLOW-03, and FLOW-04.
- `.planning/ROADMAP.md` — Phase 3 goal, success criteria, and boundary versus
  Phase 4 apply/persistence behavior.
- `.planning/STATE.md` — Current project state and unresolved downstream
  concerns.
- `.planning/phases/01-embedded-planning-entry/01-CONTEXT.md` — Locked entry and
  input/context reuse decisions that still apply.
- `.planning/phases/02-structured-plan-draft-generation/02-CONTEXT.md` — Locked
  structured draft shape, validation, preview, and prompting decisions that
  Phase 3 must preserve.

### Existing structured draft implementation to extend
- `App_Code/Bll/AIActivityPlanDraftGenerator.cs` — Existing structured draft
  generation flow and runtime scoped-skill usage.
- `App_Code/Common/AIActivityPlanDraftHelper.cs` — Existing draft parsing,
  normalization, and validation contract that Phase 3 regeneration must reuse.
- `App_Code/Common/AIActivityPlanPromptBuilder.cs` — Existing schema guardrails
  and topic-first request shaping.
- `teacher/aiprovider_api.ashx` — Existing authenticated `activityPlan` route
  that Phase 3 should extend with a dedicated section-regeneration action.
- `teacher/courseedit.aspx` — Existing right-side preview panel host for all
  review actions.
- `js/courseedit.js` — Existing structured preview rendering and in-memory draft
  state that Phase 3 should extend rather than replace.

</canonical_refs>

<code_context>
## Existing Code Insights

### Reusable Assets
- `js/courseedit.js`: Already stores the latest structured response in
  `lastActivityPlanDraftResponse`, renders read-only cards, and keeps generation
  preview separate from lesson content.
- `teacher/courseedit.aspx`: Already hosts the right-side assistant panel,
  result area, progress area, and copy control introduced in earlier phases.
- `App_Code/Common/AIActivityPlanDraftHelper.cs`: Already knows the fixed draft
  schema and rejects incomplete drafts, making it the natural contract anchor
  for section-level regeneration validation.
- `App_Code/Bll/AIActivityPlanDraftGenerator.cs`: Already handles provider
  selection, custom skill loading, prompt composition, provider calling, and
  structured draft acceptance.

### Established Patterns
- AI features stay on the existing authenticated ASP.NET handler path rather than
  introducing a separate API or frontend app.
- The browser preview remains safe and non-destructive: it renders with DOM APIs
  and `textContent`, not raw HTML injection.
- Teacher review happens in the side panel while lesson-editor content remains
  under explicit teacher control.
- Structured draft behavior is already centralized enough that Phase 3 can be a
  narrow extension instead of a new subsystem.

### Integration Points
- `teacher/aiprovider_api.ashx` is the right integration point for a dedicated
  section-regeneration handler action.
- A server-side section regeneration flow should sit next to the existing draft
  generation logic and reuse the same skill/provider infrastructure.
- `js/courseedit.js` is the main client integration point for section-local
  action buttons, local loading/failure state, and merged-draft re-rendering.
- `teacher/courseedit.aspx` remains the only review surface in this phase.

</code_context>

<specifics>
## Specific Ideas

- Treat Phase 3 as a review-loop extension of the Phase 2 draft model, not as a
  new editor or approval system.
- Keep the browser simple by having the server return the fully merged draft
  after a successful section regeneration.
- Preserve user trust by never replacing the visible preview with invalid or
  partial content during targeted retries.

</specifics>

<deferred>
## Deferred Ideas

- Individual activity-step regeneration
- Explicit lock/keep/accept/reject state per section
- Applying regenerated content into lesson editor fields
- Saved draft persistence and resume-later behavior

</deferred>

---

*Phase: 03-guided-review-and-section-regeneration*
*Context gathered: 2026-04-10*
