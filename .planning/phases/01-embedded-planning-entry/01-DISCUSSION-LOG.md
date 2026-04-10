# Phase 1: Embedded planning entry - Discussion Log

> **Audit trail only.** Do not use as input to planning, research, or execution
> agents.
> Decisions are captured in CONTEXT.md — this log preserves the alternatives
> considered.

**Date:** 2026-04-10
**Phase:** 01-embedded-planning-entry
**Areas discussed:** Entry placement, Planning input shape, Context reuse rules,
Assistant framing

---

## Entry placement

| Option | Description | Selected |
|--------|-------------|----------|
| Course editor only | Start inside `teacher/courseedit.aspx` as the primary planning entry. It already has course title, grade, term, period, and existing lesson content. | ✓ |
| Mission editor only | Start inside `teacher/missionadd.aspx` using the existing AI side-panel pattern. | |
| Both editors now | Add the entry to both course and mission editing surfaces in Phase 1. | |

**User's choice:** Course editor only
**Notes:** Phase 1 stays focused on the lesson editor as the strongest context
source.

| Option | Description | Selected |
|--------|-------------|----------|
| Right-side panel | Mirror the successful `missionadd.aspx` AI assistant layout so the editor stays primary. | ✓ |
| Inline section above editor | Add a planning block before the main content editor. | |
| Separate modal/drawer | Open planning from a button into an overlay. | |

**User's choice:** Right-side panel
**Notes:** The embedded panel model fits the brownfield page best.

---

## Planning input shape

| Option | Description | Selected |
|--------|-------------|----------|
| Topic first, expand more | Topic or knowledge point is required; optional guidance fields expand as needed. | ✓ |
| All guidance fields visible | Always show topic, grade, subject, duration, and goals together. | |
| Single freeform prompt | Use one large prompt box like the current generic AI helper. | |

**User's choice:** Topic first, expand more
**Notes:** Keeps the flow aligned with the lightweight-input v1 requirement.

| Option | Description | Selected |
|--------|-------------|----------|
| Grade | Expose grade as a first-class optional field. | ✓ |
| Duration | Expose lesson duration as a first-class optional field. | ✓ |
| Teaching goals | Expose teaching goals or key focus as a first-class optional field. | ✓ |
| Subject | Expose subject as a first-class optional field. | |

**User's choice:** Grade, duration, and teaching goals
**Notes:** Subject was not selected as an explicit first-pass field.

---

## Context reuse rules

| Option | Description | Selected |
|--------|-------------|----------|
| Include automatically | Use current editor content as prompt context whenever it exists. | ✓ |
| Include with toggle | Include current content by default, but let teachers opt out per request. | |
| Manual only | Never include editor content unless the teacher explicitly adds it. | |

**User's choice:** Include automatically
**Notes:** Supports the lightweight workflow and INPUT-04 directly.

| Option | Description | Selected |
|--------|-------------|----------|
| Topic wins | Typed topic or knowledge point is the teacher's current intent, with editor content as background. | ✓ |
| Merge equally | Pass both with equal weight. | |
| Existing content wins | Prefer the current course body over the new topic. | |

**User's choice:** Topic wins
**Notes:** New topic input overrides mismatched existing content during request
composition.

---

## Assistant framing

| Option | Description | Selected |
|--------|-------------|----------|
| Dedicated plan assistant | Present the entry as a specific activity-plan assistant. | ✓ |
| Generic AI helper preset | Keep a generic AI helper feel with an activity-plan preset. | |
| Simple and advanced modes | Offer both dedicated and advanced freeform modes in Phase 1. | |

**User's choice:** Dedicated plan assistant
**Notes:** Keeps the feature narrowly aligned with the roadmap.

| Option | Description | Selected |
|--------|-------------|----------|
| Structured fields only | Teachers provide topic plus optional guidance fields; the system composes the request. | ✓ |
| Fields plus editable prompt | Show structured fields and the final prompt for teacher editing. | |
| Mostly freeform prompt | Keep the interaction centered on a writable prompt box. | |

**User's choice:** Structured fields only
**Notes:** No editable raw prompt in Phase 1.

---

## the agent's Discretion

- Exact wording, control density, and compact/expanded presentation of optional
  fields inside the right-side panel.
- Exact prompt-assembly logic, as long as it honors topic-first intent and
  automatic existing-content reuse.

## Deferred Ideas

None.
