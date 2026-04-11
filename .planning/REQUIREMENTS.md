# Milestone v1.2 requirements

**Defined:** 2026-04-11
**Core Value:** Teachers can turn a teaching topic into a concrete, teachable
activity plan without having to manually break the lesson into steps.

## v1.2 requirements

### Full-lesson orchestration

- [x] **ORCH-01**: Teacher can generate a structured full-lesson draft composed
  of multiple ordered activity blocks instead of one plain-text activity.
- [x] **ORCH-02**: Teacher can preview each generated block with its activity
  type, teaching purpose, and position in the lesson flow before publishing.
- [x] **ORCH-03**: Teacher can remove or regenerate an individual generated block
  without discarding the rest of the full-lesson draft.

### Existing activity composition

- [ ] **COMP-01**: Teacher can have AI choose and compose an existing quiz
  activity block when the lesson design calls for a check-for-understanding or
  practice step.
- [ ] **COMP-02**: Teacher can have AI choose and compose an existing
  resource-study activity block when the lesson design calls for guided content
  reading or material study.
- [ ] **COMP-03**: Teacher can have AI choose and compose an existing web
  courseware activity block when the lesson design calls for webpage-based
  presentation or interaction.
- [ ] **COMP-04**: Teacher can confirm one composed full-lesson draft and have
  the system create or update the matching set of classroom activity entries as
  one combined publish action.

### Guided inquiry activity

- [ ] **INQ-01**: Teacher can have AI generate a guided inquiry activity block
  when existing activity types are not a good fit for the lesson goal.
- [ ] **INQ-02**: Teacher can preview the guided inquiry block as part of the
  same full-lesson draft and publish it through the normal teacher workflow.

### Student runtime and completion

- [ ] **RUN-01**: Student can enter each published block in the composed lesson
  through the existing classroom navigation flow.
- [ ] **RUN-02**: Student can view the generated content for each published block
  in a runtime that matches the selected activity type.
- [ ] **RUN-03**: Student can complete or submit work for each published block
  using the expected activity flow for that type.
- [ ] **RUN-04**: Teacher and student completion views can distinguish progress
  across the composed lesson blocks after publication.

## Future requirements

- **ORCH-04**: Teacher can reorder generated blocks with drag-and-drop before
  publishing.
- **ORCH-05**: Teacher can see an explanation of why AI chose each activity
  type for a block.
- **COMP-05**: Teacher can compose additional legacy activity types beyond quiz,
  resource-study, and web courseware.
- **INQ-03**: Guided inquiry activity can support differentiated learner paths
  inside the same published block.
- **RUN-05**: Composed lessons can express weighted or conditional completion
  rules across blocks.

## Out of scope

| Feature | Reason |
|---------|--------|
| Fully automatic publish without teacher confirmation | The teacher must still review and decide what becomes part of the real classroom flow |
| Replacing the existing Web Forms teacher and student surfaces with a new SPA orchestrator | This milestone must extend the brownfield workflow rather than fork it |
| Arbitrary visual drag-and-drop lesson builder as the main v1.2 experience | The immediate goal is AI composition and preview, not a full manual editor redesign |
| Supporting every existing teacher activity type in the first composed-lesson release | v1.2 focuses on the highest-value reusable types first: quiz, resource-study, web courseware, and guided inquiry fallback |

## Traceability

| Requirement | Phase | Status |
|-------------|-------|--------|
| ORCH-01 | Phase 8 | Complete |
| ORCH-02 | Phase 8 | Complete |
| ORCH-03 | Phase 8 | Complete |
| COMP-01 | Phase 9 | Pending |
| COMP-02 | Phase 9 | Pending |
| COMP-03 | Phase 9 | Pending |
| COMP-04 | Phase 10 | Pending |
| INQ-01 | Phase 10 | Pending |
| INQ-02 | Phase 10 | Pending |
| RUN-01 | Phase 11 | Pending |
| RUN-02 | Phase 11 | Pending |
| RUN-03 | Phase 11 | Pending |
| RUN-04 | Phase 11 | Pending |

**Coverage:**
- v1.2 requirements: 13 total
- Mapped to phases: 13
- Unmapped: 0 ✓

---
*Requirements defined: 2026-04-11*
*Last updated: 2026-04-11 after Phase 8 completion*
