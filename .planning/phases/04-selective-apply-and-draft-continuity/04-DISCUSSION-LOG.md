# Phase 4: Selective apply and draft continuity - Discussion Log

> **Audit trail only.** Do not use as input to planning, research, or execution
> agents.
> Decisions are captured in CONTEXT.md — this log preserves the alternatives
> considered.

**Date:** 2026-04-10
**Phase:** 04-selective-apply-and-draft-continuity
**Areas discussed:** Apply target mapping, Apply interaction model, Saved draft
scope, Resume entry and lifecycle

---

## Apply target mapping

| Option | Description | Selected |
|--------|-------------|----------|
| Append structured blocks | Apply selected sections into `mcontent` as clearly labeled content blocks in the existing lesson body. | ✓ |
| Replace whole editor body | Rewrite the lesson body from the selected sections as a new full document. | |
| Separate hidden fields first | Store sections outside `mcontent` first and compose later. | |

**User's choice:** Append structured blocks
**Notes:** Best fit for the current single-body editor model.

| Option | Description | Selected |
|--------|-------------|----------|
| Insert new block only | Always insert a new clearly labeled plan block and never auto-overwrite old content. | ✓ |
| Replace matching block | Try to detect and replace a previous matching block automatically. | |
| Ask every time | Prompt to replace or insert when a likely block exists. | |

**User's choice:** Insert new block only
**Notes:** Avoids fragile overwrite logic and keeps teacher control explicit.

---

## Apply interaction model

| Option | Description | Selected |
|--------|-------------|----------|
| Per-section apply plus multi-select | Allow applying one section directly and also several selected sections together. | ✓ |
| Per-section only | Allow applying only one section at a time. | |
| Whole draft with exclusions | Main apply action for the whole draft with optional exclusions. | |

**User's choice:** Per-section apply plus multi-select
**Notes:** Keeps the section-card review model while avoiding all-or-nothing apply.

| Option | Description | Selected |
|--------|-------------|----------|
| Explicit confirm before write | Show a clear confirmation step before any lesson content changes. | ✓ |
| Apply immediately with undo hint | Write into the editor immediately, then suggest undo if needed. | |
| Preview diff first | Show a before/after diff before every apply. | |

**User's choice:** Explicit confirm before write
**Notes:** Protects the trust boundary before `mcontent` is changed.

---

## Saved draft scope

| Option | Description | Selected |
|--------|-------------|----------|
| Input plus current draft | Save teacher input context together with the latest merged structured draft. | ✓ |
| Current draft only | Save only the latest structured draft JSON. | |
| Whole panel state | Save the full assistant working state including transient UI state. | |

**User's choice:** Input plus current draft
**Notes:** Enough to resume meaningfully without over-persisting transient UI state.

| Option | Description | Selected |
|--------|-------------|----------|
| Single current draft | One saved activity-plan draft per course. | ✓ |
| Multiple named drafts | Several drafts per course with titles/timestamps. | |
| Single autosave plus manual snapshots | One autosave plus optional snapshots. | |

**User's choice:** Single current draft
**Notes:** Keeps v1 persistence simple.

---

## Resume entry and lifecycle

| Option | Description | Selected |
|--------|-------------|----------|
| Resume banner in panel | Show a resume message and action inside the existing right-side panel. | ✓ |
| Auto-load immediately | Always reopen the saved draft automatically. | |
| Separate draft list | Add a dedicated saved-drafts list or manager. | |

**User's choice:** Resume banner in panel
**Notes:** Keeps the workflow inside the existing assistant surface.

| Option | Description | Selected |
|--------|-------------|----------|
| Ask replace or keep editing | If a saved draft exists, ask whether to continue it or replace it. | ✓ |
| Always replace saved draft | New planning automatically becomes the current saved draft. | |
| Always keep saved draft separate | New planning never overwrites the saved draft in this phase. | |

**User's choice:** Ask replace or keep editing
**Notes:** Prevents silent data loss while keeping single-draft scope.

---

## the agent's Discretion

- Exact saved-draft storage implementation
- Exact labeled-block formatting inserted into `mcontent`
- Exact confirmation wording and styling

## Deferred Ideas

- Multiple named drafts per course
- Separate draft-management page or list view
- Automatic replacement of earlier applied blocks
- Automatic apply on generation or regeneration
