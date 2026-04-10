# OpenLearnSite teaching skills enhancement

## What This Is

This project extends the existing OpenLearnSite teacher workflow with stronger
teaching-oriented AI skills. It focuses on the teacher-facing lesson and course
editing experience, so a teacher can enter a topic or knowledge point and get
an executable classroom activity plan that is easier to use in real teaching.

## Core Value

Teachers can turn a teaching topic into a concrete, teachable activity plan
without having to manually break the lesson into steps.

## Requirements

### Validated

- ✓ Teachers can create and edit course and activity content in the existing
  Web Forms teacher workflow — existing
- ✓ Teachers can use AI-assisted content generation in the activity creation
  page `teacher/missionadd.aspx` — existing
- ✓ Teachers can configure AI providers and route generation through a default
  provider in `teacher/aiprovider.aspx` and `teacher/aiprovider_api.ashx` —
  existing
- ✓ Teachers can use scoped AI skills for rubric generation and student exam
  assessment in `teacher/gauge.aspx`, `teacher/gaugeitem.aspx`, and
  `student/myexam.aspx` — existing

### Active

- [ ] Teachers can enter a lesson topic or knowledge point in the teacher
  lesson or course editing flow and generate a structured classroom activity
  plan
- [ ] Generated activity plans include teaching goals, step-by-step activity
  flow, time allocation, interaction methods, resource suggestions, and
  assessment design
- [ ] The first version works inside the existing teacher lesson or course
  editing page instead of a separate planning product

### Out of Scope

- Full end-to-end teaching lifecycle redesign — the current effort focuses on
  lesson planning support first
- Fully autonomous lesson publishing or classroom execution orchestration —
  teachers still review and decide what to use
- Broad student-side learning workflow changes — this work starts with the
  teacher authoring surface

## Context

OpenLearnSite is an existing ASP.NET Web Forms monolith with role-based teacher,
student, and manager experiences. The current codebase already includes an AI
provider layer, custom skill tables, teacher-facing AI entry points, and
several AI-assisted workflows.

Recent shipped capabilities show that AI support is already present but scoped
to narrower teaching tasks. `teacher/missionadd.aspx` exposes an "AI 教学助手"
panel that helps generate activity description content, learning goals, sample
content, and practice materials. Separate AI flows already exist for rubric
generation in `teacher/gauge.aspx` and `teacher/gaugeitem.aspx`, and for
student exam assessment in `student/myexam.aspx`.

The current problem is not AI access itself. The gap is pedagogical structure.
Teachers can generate fragments, but they still struggle to break a teaching
topic into executable classroom activities that fit real lesson organization.

This project treats the existing AI and skills foundation as validated system
capability, then adds a more teaching-aware planning experience focused on
classroom activity design. The initial brownfield target is the existing teacher
lesson or course editing surface, not a new standalone assistant.

## Constraints

- **Tech stack**: Build on the existing ASP.NET Web Forms architecture and the
  current teacher pages — this must fit the established brownfield system
- **Integration**: Reuse the existing AI provider and skill infrastructure —
  the system already has provider routing and scoped AI skill concepts
- **Scope**: Deliver a narrow v1 around activity-plan generation first — the
  goal is to solve the highest-value planning gap before expanding editing,
  reuse, or publishing flows
- **User input**: The minimum teacher input for v1 is a topic or knowledge point
  — the experience must stay lightweight enough for real teacher preparation
- **Human review**: Generated teaching plans remain teacher-reviewed content —
  the system supports planning, not unsupervised instructional decisions

## Key Decisions

| Decision | Rationale | Outcome |
|----------|-----------|---------|
| Start from the teacher lesson or course editing page instead of a separate assistant | Teachers already prepare content in this surface, so the smallest useful change is to improve the existing flow | — Pending |
| Focus v1 on activity-plan generation | The main pain is converting a topic into executable activities, not broad AI coverage everywhere | — Pending |
| Require only a topic or knowledge point as the minimum prompt | Lower input friction increases the chance teachers actually use the tool during preparation | — Pending |
| Generate full pedagogical structure, not just prose content | The missing value is organization of goals, steps, interactions, resources, and assessment | — Pending |

## Evolution

This document evolves at phase transitions and milestone boundaries.

**After each phase transition** (via `/gsd-transition`):
1. Requirements invalidated? -> Move to Out of Scope with reason
2. Requirements validated? -> Move to Validated with phase reference
3. New requirements emerged? -> Add to Active
4. Decisions to log? -> Add to Key Decisions
5. "What This Is" still accurate? -> Update if drifted

**After each milestone** (via `/gsd-complete-milestone`):
1. Full review of all sections
2. Core Value check — still the right priority?
3. Audit Out of Scope — reasons still valid?
4. Update Context with current state

---
*Last updated: 2026-04-10 after initialization*
