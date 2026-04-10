# Roadmap: OpenLearnSite teaching skills enhancement

## Overview

This roadmap delivers the v1 lesson-planning loop inside the existing teacher
editing workflow. The phases follow the natural path from entering planning
context, to generating a structured draft, to reviewing and regenerating
sections, to selectively applying approved content back into the editor.

## Phases

**Phase Numbering:**
- Integer phases (1, 2, 3): Planned milestone work
- Decimal phases (2.1, 2.2): Urgent insertions (marked with INSERTED)

Decimal phases appear between their surrounding integers in numeric order.

- [x] **Phase 1: Embedded planning entry** - Teachers start planning from the
  existing editor with lightweight context and existing provider routing.
- [x] **Phase 2: Structured plan draft generation** - Teachers receive a
  complete classroom activity-plan draft from their topic and context.
- [x] **Phase 3: Guided review and section regeneration** - Teachers inspect the
  draft safely and improve weak sections without losing strong ones.
- [ ] **Phase 4: Selective apply and draft continuity** - Teachers move approved
  sections into lesson content and resume saved drafts later.

## Phase Details

### Phase 1: Embedded planning entry
**Goal**: Teachers can start lesson planning from the existing lesson or course
editing page with the minimum context needed for useful generation.
**Depends on**: Nothing (first phase)
**Requirements**: INPUT-01, INPUT-02, INPUT-03, INPUT-04, FLOW-01, FLOW-03
**Success Criteria** (what must be TRUE):
  1. Teacher can open the activity-plan assistant inside the existing lesson or
     course editing page.
  2. Teacher can start generation by entering only a topic or knowledge point.
  3. Teacher can optionally add grade, subject, lesson duration, and teaching
     goals to guide the plan.
  4. When lesson or course content already exists in the editor, the planning
     request uses that context and follows the existing configured AI provider
     path without a separate setup flow.
**Plans**: 2 plans
Plans:
- [x] 01-01-PLAN.md — Add the server-side activity-plan request contract on the existing AI provider path.
- [x] 01-02-PLAN.md — Embed the course-editor activity-plan assistant UI and regression coverage.
**UI hint**: yes

### Phase 2: Structured plan draft generation
**Goal**: Teachers can generate a complete classroom activity-plan draft that is
pedagogically structured and still stays under teacher control.
**Depends on**: Phase 1
**Requirements**: PLAN-01, PLAN-02, PLAN-03, PLAN-04, FLOW-02
**Success Criteria** (what must be TRUE):
  1. Teacher can generate a draft activity plan with teaching goals, ordered
     activity steps, and time allocation.
  2. Generated steps include interaction methods such as questioning,
     discussion, grouping, presentation, or hands-on practice.
  3. Generated output includes resource suggestions such as materials, tools,
     examples, or practice ideas.
  4. Generated output includes assessment design such as checks for
     understanding, observation points, or completion criteria.
  5. The generated plan remains a teacher-reviewed draft and is never
     auto-applied or auto-published without teacher confirmation.
**Plans**: TBD

### Phase 3: Guided review and section regeneration
**Goal**: Teachers can review a generated plan before committing changes and
improve only the parts that need revision.
**Depends on**: Phase 2
**Requirements**: EDIT-01, EDIT-02
**Success Criteria** (what must be TRUE):
  1. Teacher can preview the generated activity plan before any existing lesson
     content is changed.
  2. Teacher can regenerate an individual section of the plan without
     discarding the rest of the draft.
  3. Teacher can keep accepted sections while replacing weaker sections in the
     same draft review flow.
**Plans**: 2 plans
Plans:
- [x] 03-01-PLAN.md — Add server-side section regeneration, validation, and merged draft return on the existing AI route.
- [x] 03-02-PLAN.md — Add section-level regenerate controls with local retry state in the course-editor preview.
**UI hint**: yes

### Phase 4: Selective apply and draft continuity
**Goal**: Teachers can carry approved plan content into the existing editor and
continue unfinished planning work later.
**Depends on**: Phase 3
**Requirements**: EDIT-03, EDIT-04
**Success Criteria** (what must be TRUE):
  1. Teacher can selectively write chosen plan sections back into the existing
     lesson or activity editor.
  2. Teacher controls when editor content is updated, so no lesson content is
     overwritten until the teacher confirms the apply action.
  3. Teacher can save a generated plan draft and reopen it later to continue
     reviewing, editing, or applying sections.
**Plans**: TBD
**UI hint**: yes

## Progress

**Execution Order:**
Phases execute in numeric order: 1 -> 2 -> 3 -> 4

| Phase | Plans Complete | Status | Completed |
|-------|----------------|--------|-----------|
| 1. Embedded planning entry | 2/2 | Complete | 2026-04-10 |
| 2. Structured plan draft generation | 2/2 | Complete | 2026-04-10 |
| 3. Guided review and section regeneration | 2/2 | Complete | 2026-04-10 |
| 4. Selective apply and draft continuity | 0/TBD | Not started | - |
