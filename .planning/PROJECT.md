# OpenLearnSite teaching skills enhancement

## What This Is

This project extends the existing OpenLearnSite teacher workflow with stronger
teaching-oriented AI skills. It focuses on the teacher-facing lesson and course
editing experience, so a teacher can enter a topic or knowledge point and get
an executable classroom activity plan that is easier to use in real teaching.

## Core Value

Teachers can turn a teaching topic into a concrete, teachable activity plan
without having to manually break the lesson into steps.

## Current Milestone: v1.2 AI 整课编排与活动组合

**Goal:** 让老师基于现有教学内容先生成一个可预览的整课草案，由 AI
自动组合合适的课堂活动，而不再只是产出纯文本活动说明。

**Target features:**
- AI 能根据现有课堂内容与教学意图，生成完整课堂或学案草案，而不是单段活动文本
- AI 优先复用现有活动类型来自动插入可执行环节，首批重点是测验、资源学习、网页课件
- 当现有活动类型不适合时，系统支持一种更适合 AI 生成的引导式探究活动

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
- ✓ Teachers can start activity-plan generation from `teacher/courseedit.aspx`
  with only a topic required, optional structured guidance, and existing lesson
  content reused through the current AI provider route — Validated in Phase 1:
  Embedded planning entry
- ✓ Generated activity plans include teaching goals, step-by-step activity
  flow, time allocation, interaction methods, resource suggestions, and
  assessment design — Validated in Phase 2: Structured plan draft generation
- ✓ The first version works inside the existing teacher lesson or course
  editing page instead of a separate planning product — Validated through
  Phases 1-4 in v1.0 milestone
- ✓ Teachers can preview a generated activity plan, regenerate individual
  sections, and keep the rest of the draft stable during review — Validated in
  Phase 3: Guided review and section regeneration
- ✓ Teachers can selectively append approved sections into existing lesson
  content and save or resume one course draft later — Validated in Phase 4:
  Selective apply and draft continuity

### Active

- [ ] Teachers can generate reusable, classroom-ready teaching activity
  segments from AI inside the existing planning workflow
- [ ] Teachers can preview an AI-composed full-lesson draft that combines
  multiple classroom activity blocks before applying it
- [ ] AI can choose and compose existing teacher activity types, starting with
  quizzes, resource-study blocks, and web courseware
- [ ] Teachers can use an AI-friendly guided inquiry activity block when
  existing activity types are not a good fit

### Validated in v1.1 milestone

- ✓ Teachers can add generated activity output into lesson content and also
  publish it as a student-enterable classroom activity — Validated in Phase 5:
  Teacher activity publish foundation
- ✓ Students can open published AI-generated activities, follow guided steps,
  submit results, and have participation or completion recorded — Validated in
  Phases 6-7: Student activity entry, guidance, submission, and completion
  tracking

### Validated in v1.2 milestone

- ✓ Teachers can generate and preview an AI-composed full-lesson draft as
  ordered activity blocks inside `teacher/courseedit.aspx` — Validated in Phase 8:
  Full-lesson draft orchestration
- ✓ Teachers can remove or regenerate one generated lesson block and save or
  resume the full-lesson draft without silently writing into lesson body content
  — Validated in Phase 8: Full-lesson draft orchestration

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
classroom activity design. The brownfield target remains the existing teacher
lesson or course editing surface plus the existing student activity entry flow,
not a new standalone assistant.

v1.0 ships that lesson-planning loop end to end inside `teacher/courseedit.aspx`.
Teachers can enter a topic, generate a structured activity-plan draft, regenerate
individual sections, save one draft per course, resume it later, and append
approved sections into the editor without auto-saving or auto-publishing.

v1.1 shifts the next validated step from planning-only output to executable
classroom activities. The milestone focus is turning AI-generated teaching
segments into publishable learning activities that fit both the teacher authoring
surface and the student classroom experience, including activity entry,
submission, and completion tracking.

Focused verification passed through the net8.0 slices of
`Tests/CommonLogicTests/CommonLogicTests.csproj` and
`Tests/TeacherRegressionTests/TeacherRegressionTests.csproj`. Remaining
follow-up is manual browser verification of saved-draft resume and append-only
apply behavior with an authenticated teacher session and a live provider.

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
| Start from the teacher lesson or course editing page instead of a separate assistant | Teachers already prepare content in this surface, so the smallest useful change is to improve the existing flow | Validated in Phase 1 |
| Focus v1 on activity-plan generation | The main pain is converting a topic into executable activities, not broad AI coverage everywhere | Validated in Phase 1 |
| Require only a topic or knowledge point as the minimum prompt | Lower input friction increases the chance teachers actually use the tool during preparation | Validated in Phase 1 |
| Generate full pedagogical structure, not just prose content | The missing value is organization of goals, steps, interactions, resources, and assessment | Validated in Phase 2 |
| Keep the generated plan preview-only until the teacher explicitly chooses what to keep | Teacher trust depends on review before lesson content changes | Validated in Phases 2-4 |
| Keep review improvements section-scoped instead of regenerating the whole draft every time | Teachers need to preserve strong sections while refining weak ones | Validated in Phase 3 |
| Store one current saved draft per course outside lesson body content | Draft continuity should not bypass the existing lesson save path or mix transient plan state into published content | Validated in Phase 4 |
| Apply approved plan sections by appending labeled blocks into the editor | Append-only writeback avoids silent overwrite and fits the brownfield editor workflow | Validated in Phase 4 |
| Publish approved AI activities through one brownfield transaction that links course, mission, and menu ids | Student entry must stay aligned with teacher-visible content and the existing classroom model | Validated in Phase 5 |
| Keep student AI activities on the existing `showmission.aspx` and upload handlers instead of introducing AI-specific routes | The smallest safe change is to reuse the established student flow and tighten it with guards/tests | Validated in Phases 6-7 |
| Record AI activity completion through existing `MenuWorks` state with duplicate-safe writes | Completion visibility must stay on the current classroom tracking model instead of fragmenting into an AI-only status store | Validated in Phase 7 |

## Current State

v1.1 milestone shipped on 2026-04-11. Teachers can already generate AI
classroom activity segments in the existing editor, publish them into the
brownfield student mission flow, and let students open, submit, and complete
those activities with existing classroom tracking surfaces.

The next milestone shifts from single activity generation to lesson
orchestration. The new goal is to let AI build a previewable full-lesson draft
that can combine multiple activity types, reuse existing teacher activity pages
where possible, and introduce a guided inquiry block when current activity
types are not enough.

Phase 8 shipped on 2026-04-11. The teacher course editor can now request a
full-lesson draft, render ordered block cards from a server-owned DTO, and
refine one block at a time through remove/regenerate/save/resume flows while
keeping the lesson body unchanged until explicit later actions.

## Milestone History

<details>
<summary>Shipped milestone context</summary>

## Current State

v1.0 milestone shipped on 2026-04-11. Phases 1-4 are complete and the planning
surface now supports embedded topic-first generation, structured preview,
section-level regeneration, saved-draft continuity, and selective append-only
apply inside `teacher/courseedit.aspx`.

v1.1 shipped on 2026-04-11. Phases 5-7 are complete and the AI planning flow now
extends through teacher publish, student entry, guided mission rendering,
submission, and completion tracking inside the existing brownfield application.

</details>

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
*Last updated: 2026-04-11 after Phase 8 completion*
