# Roadmap: OpenLearnSite teaching skills enhancement

## Overview

This roadmap records the shipped v1.0-v1.1 milestones and the planned delivery
path for milestone v1.2. Milestone v1.2 starts at Phase 8 and focuses on
turning the existing teacher AI flow from single activity output into a
previewable full-lesson composition that reuses brownfield activity types and
still runs through the current teacher and student surfaces.

## Milestones

- ✅ **v1.0 milestone** — Phases 1-4 (shipped 2026-04-11)
- ✅ **v1.1 教学环节生成与课堂活动投放** — Phases 5-7 (shipped 2026-04-11)
- 🚧 **v1.2 AI 整课编排与活动组合** — Phases 8-11 (planned)

## Phases

<details>
<summary>✅ v1.0 milestone (Phases 1-4) — SHIPPED 2026-04-11</summary>

- [x] **Phase 1: Embedded planning entry** - Start AI lesson planning from the existing course editor with lightweight teacher input.
- [x] **Phase 2: Structured plan draft generation** - Generate a structured preview-first activity plan draft instead of plain prose.
- [x] **Phase 3: Guided review and section regeneration** - Let teachers refine one section without losing the rest of the draft.
- [x] **Phase 4: Selective apply and draft continuity** - Save, resume, and append approved draft sections into lesson content.

</details>

<details>
<summary>✅ v1.1 教学环节生成与课堂活动投放 (Phases 5-7) — SHIPPED 2026-04-11</summary>

- [x] **Phase 5: Teacher activity publish foundation** - Publish approved AI activity output into brownfield course and mission records.
- [x] **Phase 6: Student activity entry and guided experience** - Open published AI activities in the existing student flow with guided runtime content.
- [x] **Phase 7: Submission and completion tracking** - Submit student work and record duplicate-safe completion through existing classroom tracking.

Archive: `.planning/milestones/v1.1-ROADMAP.md`

</details>

### 🚧 v1.2 AI 整课编排与活动组合

- [ ] **Phase 8: Full-lesson draft orchestration** - Teachers generate and review an ordered full-lesson draft made of typed activity blocks.
- [ ] **Phase 9: Existing activity block composition** - AI selects reusable quiz, resource-study, and web courseware blocks for the lesson flow.
- [ ] **Phase 10: Guided inquiry fallback and combined publish** - Mixed drafts can include guided inquiry and publish as one confirmed classroom set.
- [ ] **Phase 11: Composed runtime and progress visibility** - Published lesson blocks run in student flow with per-block completion visibility.

## Phase Details

### Phase 8: Full-lesson draft orchestration
**Goal**: Teachers can generate and refine a previewable full-lesson draft made of ordered activity blocks inside the existing course editor
**Depends on**: Phase 7
**Requirements**: ORCH-01, ORCH-02, ORCH-03
**Success Criteria** (what must be TRUE):
  1. Teacher can generate a structured full-lesson draft that appears as multiple ordered activity blocks instead of one plain-text activity.
  2. Teacher can preview each generated block with its activity type, teaching purpose, and lesson position before deciding what to keep.
  3. Teacher can remove or regenerate one block without discarding the rest of the full-lesson draft.
**Plans**: TBD
**UI hint**: yes

### Phase 9: Existing activity block composition
**Goal**: Teachers can receive AI-selected reusable legacy activity blocks that fit different moments of the lesson flow
**Depends on**: Phase 8
**Requirements**: COMP-01, COMP-02, COMP-03
**Success Criteria** (what must be TRUE):
  1. Teacher can get a composed quiz block when the lesson needs a check-for-understanding or practice step.
  2. Teacher can get a composed resource-study block when the lesson needs guided reading or material study.
  3. Teacher can get a composed web courseware block when the lesson needs webpage-based presentation or interaction.
  4. Teacher can see AI choose among these existing activity types instead of forcing every step into the same generic format.
**Plans**: TBD
**UI hint**: yes

### Phase 10: Guided inquiry fallback and combined publish
**Goal**: Teachers can fill unsupported lesson moments with guided inquiry blocks and publish one confirmed composed draft through the normal workflow
**Depends on**: Phase 9
**Requirements**: INQ-01, INQ-02, COMP-04
**Success Criteria** (what must be TRUE):
  1. Teacher can receive a guided inquiry block when existing quiz, resource-study, or web courseware types are not a good fit for the lesson goal.
  2. Teacher can preview the guided inquiry block inside the same full-lesson draft as the other generated blocks.
  3. Teacher can confirm one composed full-lesson draft and have the system create or update the matching classroom activity entries through one combined publish action.
  4. Teacher can publish a mixed draft that includes both reused existing activity types and guided inquiry without leaving the existing teacher workflow.
**Plans**: TBD
**UI hint**: yes

### Phase 11: Composed runtime and progress visibility
**Goal**: Students and teachers can execute and track composed lesson blocks through the existing classroom flow
**Depends on**: Phase 10
**Requirements**: RUN-01, RUN-02, RUN-03, RUN-04
**Success Criteria** (what must be TRUE):
  1. Student can enter each published block in the composed lesson through the existing classroom navigation flow.
  2. Student can view each published block in a runtime that matches its selected activity type.
  3. Student can complete or submit work for each published block using that block's expected activity flow.
  4. Teacher and student completion views can distinguish progress across the composed lesson blocks after publication.
**Plans**: TBD
**UI hint**: yes

## Progress

| Phase | Milestone | Plans Complete | Status | Completed |
|-------|-----------|----------------|--------|-----------|
| 1. Embedded planning entry | v1.0 | 2/2 | Complete | 2026-04-10 |
| 2. Structured plan draft generation | v1.0 | 2/2 | Complete | 2026-04-10 |
| 3. Guided review and section regeneration | v1.0 | 2/2 | Complete | 2026-04-10 |
| 4. Selective apply and draft continuity | v1.0 | 2/2 | Complete | 2026-04-10 |
| 5. Teacher activity publish foundation | v1.1 | 3/3 | Complete | 2026-04-11 |
| 6. Student activity entry and guided experience | v1.1 | 2/2 | Complete | 2026-04-11 |
| 7. Submission and completion tracking | v1.1 | 2/2 | Complete | 2026-04-11 |
| 8. Full-lesson draft orchestration | v1.2 | 0/TBD | Not started | - |
| 9. Existing activity block composition | v1.2 | 0/TBD | Not started | - |
| 10. Guided inquiry fallback and combined publish | v1.2 | 0/TBD | Not started | - |
| 11. Composed runtime and progress visibility | v1.2 | 0/TBD | Not started | - |
