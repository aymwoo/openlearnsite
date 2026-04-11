# Roadmap: OpenLearnSite teaching skills enhancement

## Overview

This roadmap records shipped milestone history and the proposed execution plan
for milestone v1.1. Phase numbering continues from the completed v1.0
milestone, so the next milestone starts at Phase 5.

## Milestones

- ✅ **v1.0 milestone** — Phases 1-4 (shipped 2026-04-11)
- 📋 **v1.1 教学环节生成与课堂活动投放** — Phases 5-7 (proposed)

## Phases

<details>
<summary>✅ v1.0 milestone (Phases 1-4) — SHIPPED 2026-04-11</summary>

- [x] Phase 1: Embedded planning entry (2/2 plans) — completed 2026-04-10
- [x] Phase 2: Structured plan draft generation (2/2 plans) — completed 2026-04-10
- [x] Phase 3: Guided review and section regeneration (2/2 plans) — completed 2026-04-10
- [x] Phase 4: Selective apply and draft continuity (2/2 plans) — completed 2026-04-10

</details>

### 📋 Next milestone

- [x] Phase 5: Teacher activity publish foundation (completed 2026-04-11)
- [ ] Phase 6: Student activity entry and guided experience
- [ ] Phase 7: Submission and completion tracking

## Proposed roadmap for v1.1

**3 phases** | **8 requirements mapped** | All covered ✓

| # | Phase | Goal | Requirements | Success criteria |
|---|-------|------|--------------|------------------|
| 5 | Teacher activity publish foundation | 3/3 | Complete   | 2026-04-11 |
| 6 | Student activity entry and guided experience | Let students enter the published activity and follow generated guidance from the class menu | SAE-01, SAE-02, SAE-03 | 4 |
| 7 | Submission and completion tracking | Let students submit activity results and record reliable completion state | SCT-01, SCT-02 | 4 |

### Phase details

**Phase 5: Teacher activity publish foundation**
Goal: Turn approved AI output into a real lesson activity with explicit publish
control and linked lesson-content update.
Requirements: TAP-01, TAP-02, TAP-03
Plans: 3 plans

- [x] 05-01-PLAN.md — define publish contracts and stable mission/menu linkage
- [x] 05-02-PLAN.md — build the synchronized publish core and content builders
- [x] 05-03-PLAN.md — wire teacher publish controls and authenticated handler flow

Success criteria:
1. A teacher can confirm AI-generated activity output and create or update a
   real course-linked lesson activity.
2. A teacher can explicitly control whether the generated activity is published
   before students can enter it.
3. The confirmed publish flow updates teacher-side lesson content and the
   student-enterable activity entry together.
4. Publish changes are applied through one authenticated server-owned flow so
   teacher and student visibility do not drift.

**Phase 6: Student activity entry and guided experience**
Goal: Let students enter the published activity and follow generated guidance
from the class menu.
Requirements: SAE-01, SAE-02, SAE-03
Success criteria:
1. A published AI-generated activity appears in the student's existing class
   menu flow.
2. A student can open the activity page from the menu entry in an authenticated
   session.
3. The activity page shows the published goal, instructions, and task steps.
4. The student experience presents step-by-step learner guidance clearly enough
   to complete the activity without relying on teacher-side draft context.

**Phase 7: Submission and completion tracking**
Goal: Let students submit activity results and record reliable completion state.
Requirements: SCT-01, SCT-02
Success criteria:
1. A student can submit a result from the published AI-generated activity flow.
2. A successful submission is linked to the correct lesson activity identity.
3. Completion status is recorded after successful submission using the existing
   classroom tracking model.
4. Teacher-facing review or status surfaces can distinguish completed activity
   submissions from untouched entries for the scoped flow.

## Progress

| Phase | Milestone | Plans Complete | Status | Completed |
|-------|-----------|----------------|--------|-----------|
| 1. Embedded planning entry | v1.0 | 2/2 | Complete | 2026-04-10 |
| 2. Structured plan draft generation | v1.0 | 2/2 | Complete | 2026-04-10 |
| 3. Guided review and section regeneration | v1.0 | 2/2 | Complete | 2026-04-10 |
| 4. Selective apply and draft continuity | v1.0 | 2/2 | Complete | 2026-04-10 |
| 5. Teacher activity publish foundation | v1.1 | 3/3 | Complete | 2026-04-11 |
| 6. Student activity entry and guided experience | v1.1 | 0/0 | Proposed | - |
| 7. Submission and completion tracking | v1.1 | 0/0 | Proposed | - |
