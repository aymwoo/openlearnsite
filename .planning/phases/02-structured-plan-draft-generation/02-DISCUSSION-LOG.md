# Phase 2: Structured plan draft generation - Discussion Log

> **Audit trail only.** Do not use as input to planning, research, or execution
> agents.
> Decisions are captured in CONTEXT.md — this log preserves the alternatives
> considered.

**Date:** 2026-04-10
**Phase:** 02-structured-plan-draft-generation
**Areas discussed:** Draft data shape, Malformed AI output, Draft preview
layout, Skill and prompt control

---

## Draft data shape

| Option | Description | Selected |
|--------|-------------|----------|
| Fixed sections | Always return the same named sections: teaching goals, ordered activity steps, resources, assessment, and a short teacher reminder. | ✓ |
| Steps only | Make the draft primarily a sequence of steps, with goals/resources/assessment embedded around each step. | |
| Flexible outline | Let the model choose the section structure as long as the required concepts appear somewhere. | |

**User's choice:** Fixed sections
**Notes:** Phase 2 locks a stable top-level draft shape for downstream phases.

| Option | Description | Selected |
|--------|-------------|----------|
| Title | Short title for each step. | ✓ |
| Minutes | Explicit time allocation for each step. | ✓ |
| Teacher action | What the teacher does in the step. | ✓ |
| Student action | What students do in the step. | ✓ |
| Interaction method | Explicit interaction style such as questioning or discussion. | ✓ |
| Resource suggestion | Resource/tool/material suggestion directly on the step. | ✓ |
| Assessment check | Check-for-understanding or observation point directly on the step. | ✓ |

**User's choice:** Title, minutes, teacher action, student action,
interaction method, resource suggestion, and assessment check
**Notes:** Step-level resource and assessment detail were explicitly included,
not left only to top-level sections.

---

## Malformed AI output

| Option | Description | Selected |
|--------|-------------|----------|
| Normalize if safe | Recover near-valid output into the fixed draft shape, but fail if required sections or fields are still missing. | ✓ |
| Fail closed always | Reject any malformed or partial response immediately. | |
| Show plain text fallback | Show raw AI text if structure fails. | |

**User's choice:** Normalize if safe
**Notes:** Recovery is allowed only when the draft can still meet the contract.

| Option | Description | Selected |
|--------|-------------|----------|
| All core sections required | Accept only if goals, steps, resources, and assessment are all present and each step has required fields. | ✓ |
| Steps required, others optional | Accept if ordered steps are good enough even when other sections are weak. | |
| Any usable structure | Accept any partially structured draft. | |

**User's choice:** All core sections required
**Notes:** Phase 2 keeps a strict acceptance bar after any normalization pass.

---

## Draft preview layout

| Option | Description | Selected |
|--------|-------------|----------|
| Section cards | Render separate read-only blocks for goals, steps, resources, and assessment. | ✓ |
| Single document | Render the whole draft as one continuous formatted document. | |
| Outline plus details | Show a compact outline first, then expand details. | |

**User's choice:** Section cards
**Notes:** This keeps the preview scannable in the existing right-side panel.

| Option | Description | Selected |
|--------|-------------|----------|
| Expanded steps by default | Show each step's full fields without extra clicks. | ✓ |
| Collapsed step summaries | Show only titles and minutes first. | |
| First step expanded | Open the first step only. | |

**User's choice:** Expanded steps by default
**Notes:** The Phase 2 preview should prioritize direct teacher review over
compactness.

---

## Skill and prompt control

| Option | Description | Selected |
|--------|-------------|----------|
| Stored skill as system prompt | Load the scoped custom skill as the `system` prompt and send teacher input separately as `user`. | ✓ |
| Hardcoded prompt only | Keep everything in server code and ignore the saved custom skill. | |
| Merge stored and hardcoded prompts | Use both stored skill text and hardcoded instruction blocks. | |

**User's choice:** Stored skill as system prompt
**Notes:** Phase 2 should align `activityPlan` with existing gauge/exam
generator patterns.

| Option | Description | Selected |
|--------|-------------|----------|
| Hardcoded schema guardrails | Keep section names and required field contract enforced in code while the skill provides pedagogical guidance. | ✓ |
| Skill text only | Trust the stored skill text to fully define output rules. | |
| Response parser only | Keep prompting loose and rely only on parser/normalizer logic. | |

**User's choice:** Hardcoded schema guardrails
**Notes:** Admin-editable skill text should not be able to break the required
runtime draft contract.

---

## the agent's Discretion

- Exact class/property names for the structured draft model.
- Exact section-card visual styling within the existing course editor panel.
- Exact safe normalization heuristics before the strict acceptance check.

## Deferred Ideas

- Per-section regeneration — Phase 3
- Selective write-back into lesson content — Phase 4
- Saved draft persistence — Phase 4
