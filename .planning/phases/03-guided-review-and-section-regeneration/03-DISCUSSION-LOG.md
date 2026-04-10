# Phase 3: Guided review and section regeneration - Discussion Log

> **Audit trail only.** Do not use as input to planning, research, or execution
> agents.
> Decisions are captured in CONTEXT.md — this log preserves the alternatives
> considered.

**Date:** 2026-04-10
**Phase:** 03-guided-review-and-section-regeneration
**Areas discussed:** Section granularity, Merge behavior, Review controls,
Loading and failure UX

---

## Section granularity

| Option | Description | Selected |
|--------|-------------|----------|
| Top-level sections only | Allow regeneration only for `teachingGoals`, `activitySteps`, `resources`, `assessment`, and `teacherReminder`. | ✓ |
| Individual steps too | Allow regeneration for individual activity steps as well. | |
| Full draft only | Only support regenerating the whole draft again. | |

**User's choice:** Top-level sections only
**Notes:** Keeps the contract stable and matches the roadmap's section-level
review goal.

---

## Merge behavior

| Option | Description | Selected |
|--------|-------------|----------|
| Merged full draft | Server validates the regenerated section, merges it into the current draft, and returns the complete updated draft. | ✓ |
| Section only | Server returns only the regenerated section and the browser merges locally. | |
| Both section and full draft | Return both isolated and merged representations. | |

**User's choice:** Merged full draft
**Notes:** Keeps client logic simpler and safer.

---

## Review controls

| Option | Description | Selected |
|--------|-------------|----------|
| Regenerate only | Add section-level regenerate actions only. | ✓ |
| Regenerate plus lock | Add regenerate buttons and explicit lock/keep controls. | |
| Accept/reject per section | Add a heavier explicit review workflow. | |

**User's choice:** Regenerate only
**Notes:** Strong sections are preserved by leaving them untouched.

---

## Loading and failure UX

| Option | Description | Selected |
|--------|-------------|----------|
| Section-local state | Show loading and failure only on the targeted section card, keep all other sections visible, and preserve the last valid full draft on failure. | ✓ |
| Whole-panel loading | Freeze the full panel during section regeneration. | |
| Mixed approach | Keep the panel visible, but also show a top-level loading state. | |

**User's choice:** Section-local state
**Notes:** Keeps review understandable without disturbing accepted sections.

---

## the agent's Discretion

- Exact route/helper naming for section regeneration
- Exact visual style for section-local loading/failure affordances
- Exact placement of merge and validation helper logic

## Deferred Ideas

- Individual activity-step regeneration
- Explicit keep/lock/accept/reject controls
- Apply-to-editor and persistence workflows
