# Requirements: OpenLearnSite teaching skills enhancement

**Defined:** 2026-04-10
**Core Value:** Teachers can turn a teaching topic into a concrete, teachable
activity plan without having to manually break the lesson into steps.

## v1 Requirements

Requirements for the first release of teaching-oriented skills inside the
existing teacher lesson or course editing workflow.

### Input context

- [ ] **INPUT-01**: Teacher can start generation by entering a lesson topic or
  knowledge point
- [ ] **INPUT-02**: Teacher can optionally provide grade, subject, or lesson
  duration to improve plan quality
- [ ] **INPUT-03**: Teacher can optionally provide teaching goals or key focus to
  steer plan generation
- [ ] **INPUT-04**: System can reuse current lesson or course content as prompt
  context when generating a plan

### Plan generation

- [ ] **PLAN-01**: Teacher can generate a structured activity plan with teaching
  goals, step flow, and time allocation
- [ ] **PLAN-02**: Generated activity plan includes interaction methods such as
  questioning, discussion, grouping, presentation, or hands-on practice
- [ ] **PLAN-03**: Generated activity plan includes resource suggestions such as
  materials, tools, examples, or practice ideas
- [ ] **PLAN-04**: Generated activity plan includes assessment design such as
  observation points, checks for understanding, or completion criteria

### Review and editing

- [ ] **EDIT-01**: Teacher can preview the generated activity plan before it is
  applied to existing lesson content
- [ ] **EDIT-02**: Teacher can regenerate individual sections of the plan without
  discarding the entire draft
- [ ] **EDIT-03**: Teacher can selectively write chosen plan sections back into
  the existing lesson or activity editor
- [ ] **EDIT-04**: System can save generated plan drafts so the teacher can
  continue editing later

### Workflow and trust

- [ ] **FLOW-01**: The activity-plan assistant works inside the existing teacher
  lesson or course editing page
- [ ] **FLOW-02**: Generated plans remain teacher-reviewed drafts and are never
  auto-published or auto-applied without teacher confirmation
- [ ] **FLOW-03**: System uses the existing AI provider and custom skill
  infrastructure for lesson-plan generation instead of introducing a separate AI
  stack

## v2 Requirements

Deferred to later releases after the structured planning loop is validated.

### Advanced planning

- **ADV-01**: Teacher can generate differentiated variants of the same activity
  plan for different learner levels
- **ADV-02**: Activity plan generation can align deeply with curriculum or
  standards frameworks beyond lightweight alignment cues

## Out of Scope

Explicitly excluded from this milestone to prevent scope creep.

| Feature | Reason |
|---------|--------|
| Automatic classroom publishing | Teacher review and judgment must remain in control |
| Standalone planning application | The project goal is to enhance the existing teacher editing workflow |
| Full unit or term generation from a single prompt | Too broad for the first validated activity-planning release |

## Traceability

Which phases cover which requirements. Updated during roadmap creation.

| Requirement | Phase | Status |
|-------------|-------|--------|
| INPUT-01 | Phase 1 | Pending |
| INPUT-02 | Phase 1 | Pending |
| INPUT-03 | Phase 1 | Pending |
| INPUT-04 | Phase 1 | Pending |
| PLAN-01 | Phase 2 | Pending |
| PLAN-02 | Phase 2 | Pending |
| PLAN-03 | Phase 2 | Pending |
| PLAN-04 | Phase 2 | Pending |
| EDIT-01 | Phase 3 | Pending |
| EDIT-02 | Phase 3 | Pending |
| EDIT-03 | Phase 4 | Pending |
| EDIT-04 | Phase 4 | Pending |
| FLOW-01 | Phase 1 | Pending |
| FLOW-02 | Phase 2 | Pending |
| FLOW-03 | Phase 1 | Pending |

**Coverage:**
- v1 requirements: 15 total
- Mapped to phases: 15
- Unmapped: 0 ✓

---
*Requirements defined: 2026-04-10*
*Last updated: 2026-04-10 after roadmap creation*
