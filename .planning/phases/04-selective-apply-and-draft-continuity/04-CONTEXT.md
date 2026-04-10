# Phase 4: Selective apply and draft continuity - Context

**Gathered:** 2026-04-10
**Status:** Ready for planning

<domain>
## Phase Boundary

Let teachers move approved activity-plan content from the existing right-side
preview into the lesson editor under explicit teacher control, and let them save
unfinished planning work so it can be reopened later inside the same course
editing workflow. This phase covers selective apply actions, confirmation before
editor content changes, draft persistence, and resume behavior. It does not add
separate draft management screens, multiple parallel drafts per course, or any
automatic overwrite of lesson content.

</domain>

<decisions>
## Implementation Decisions

### Apply target mapping
- **D-01:** Approved plan sections apply into the existing lesson body field
  `mcontent` as clearly labeled structured content blocks.
- **D-02:** Phase 4 appends selected plan blocks into `mcontent` rather than
  replacing the whole lesson body.
- **D-03:** If a similar applied section already appears in the lesson body,
  Phase 4 still inserts a new labeled block and does not try to auto-detect or
  overwrite prior content.

### Apply interaction model
- **D-04:** Top-level sections remain the unit of apply control, matching the
  existing preview/review model.
- **D-05:** Phase 4 supports both per-section apply from a section card and
  applying several selected sections together.
- **D-06:** Every apply action requires an explicit confirmation step that names
  which section or sections will be inserted before any lesson editor content is
  changed.

### Saved draft scope
- **D-07:** A saved draft includes both the teacher input context and the latest
  merged structured draft.
- **D-08:** The saved input context includes topic plus the optional guidance
  fields already used by the assistant, rather than only saving the generated
  draft JSON.
- **D-09:** Phase 4 supports one current saved activity-plan draft per course,
  not multiple named drafts or snapshots.

### Resume entry and lifecycle
- **D-10:** If a saved draft exists for the current course, the existing
  right-side assistant panel shows a clear resume banner/action instead of using
  a separate draft manager.
- **D-11:** Saved drafts are not auto-loaded immediately on page open; the
  teacher explicitly chooses to resume from the panel.
- **D-12:** If a course already has a saved draft and the teacher starts a new
  planning attempt that would replace it, Phase 4 asks whether to keep editing
  the saved draft or replace it.

### the agent's Discretion
- Exact storage implementation for the single saved draft per course, as long as
  it fits the brownfield architecture and supports explicit resume.
- Exact labeled-block formatting inserted into `mcontent`, as long as applied
  content remains clearly distinguishable and teacher-editable in the existing
  editor.
- Exact confirmation UI wording and control styling within the established
  course editor panel language.

</decisions>

<canonical_refs>
## Canonical References

**Downstream agents MUST read these before planning or implementing.**

### Product scope and prior locked decisions
- `.planning/PROJECT.md` — Product goal, teacher-in-the-loop requirement, and
  brownfield constraints.
- `.planning/REQUIREMENTS.md` — Phase 4 requirement coverage for EDIT-03 and
  EDIT-04.
- `.planning/ROADMAP.md` — Phase 4 goal, success criteria, and final v1 phase
  boundary.
- `.planning/STATE.md` — Current project focus and Phase 4 pending concerns.
- `.planning/phases/01-embedded-planning-entry/01-CONTEXT.md` — Locked entry,
  input, and context reuse choices that still shape the assistant surface.
- `.planning/phases/02-structured-plan-draft-generation/02-CONTEXT.md` — Locked
  structured draft shape and preview-only trust rules that Phase 4 must preserve
  while adding apply behavior.
- `.planning/phases/03-guided-review-and-section-regeneration/03-CONTEXT.md` —
  Locked section granularity, merged-draft behavior, and review-loop decisions
  that Phase 4 must build on.

### Existing course editor and planning implementation
- `teacher/courseedit.aspx` — Existing course editor page, right-side planning
  panel, main `mcontent` editor field, and save button host.
- `teacher/courseedit.aspx.cs` — Existing lesson save flow that persists `Ctitle`,
  metadata fields, and `mcontent` through `Btnedit_Click`.
- `js/courseedit.js` — Existing in-memory draft state, preview rendering,
  section-regeneration flow, and current integration point for apply/resume UI.
- `teacher/aiprovider_api.ashx` — Existing authenticated provider route already
  used for draft generation and section regeneration; likely extension point if
  persistence uses the same assistant workflow boundary.

</canonical_refs>

<code_context>
## Existing Code Insights

### Reusable Assets
- `js/courseedit.js`: Already keeps the latest merged draft in
  `lastActivityPlanDraftResponse`, which is the natural source for selective
  apply and save-draft actions.
- `teacher/courseedit.aspx`: Already hosts the right-side assistant panel,
  preview cards, copy button, and the main editable `mcontent` body field.
- `teacher/courseedit.aspx.cs`: Already persists course title, metadata, and
  `mcontent` through the existing save button, which Phase 4 must respect.

### Established Patterns
- The course editor still persists one main lesson content field, so Phase 4
  must fit apply behavior into a single-body editor model rather than assuming
  structured server-side lesson sections already exist.
- Earlier phases kept the planning preview non-destructive and teacher-reviewed,
  so apply behavior must stay explicit and confirmation-based.
- Browser-side integration remains page-scoped JavaScript working against
  Web Forms controls and DOM IDs, not a client-side state framework.

### Integration Points
- `js/courseedit.js` is the main client integration point for apply controls,
  confirmation flows, saved-draft banners, and resume actions.
- `mcontent` in `teacher/courseedit.aspx` is the actual lesson body target for
  inserted applied content blocks.
- `Btnedit_Click` in `teacher/courseedit.aspx.cs` remains the authoritative
  persistence path for lesson body changes.
- A new persistence path may require either a course-linked store in existing
  server code or a nearby brownfield-compatible handler/helper extension.

</code_context>

<specifics>
## Specific Ideas

- Treat applied content as clearly labeled teacher-editable blocks inside the
  existing lesson body instead of trying to hide or formalize it as invisible
  structured storage.
- Keep saved-draft continuity lightweight: one resumable draft per course,
  resumed from the existing assistant panel.

</specifics>

<deferred>
## Deferred Ideas

- Multiple named drafts per course
- Separate draft-management page or list view
- Automatic detection/replacement of earlier applied plan blocks
- Automatic apply on generation or regeneration

</deferred>

---

*Phase: 04-selective-apply-and-draft-continuity*
*Context gathered: 2026-04-10*
