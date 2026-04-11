# Project Milestones: OpenLearnSite teaching skills enhancement

## v1.1 教学环节生成与课堂活动投放 (Shipped: 2026-04-11)

**Delivered:** Teachers can now publish AI-generated classroom activities from
the existing course editor, and students can enter, submit, and complete those
activities through the legacy mission flow.

**Phases completed:** 3 phases, 7 plans, 14 tasks

**Key accomplishments:**

- Typed publish contracts plus draft-linked mission/menu identifiers for stable AI activity re-publish targeting.
- Transactional AI publish core that appends selected lesson sections while updating one linked mission/menu activity with full student-facing content.
- Dedicated AI publish controls plus an authenticated `activityPlanPublish` handler that writes back committed lesson content from the server publish core.
- Student AI activities now open through the legacy mission page with server-rendered learner guidance and the upload sidebar intact.
- Published AI mission guidance now renders with deterministic learner instructions, final page copy, and explicit browser UAT for the authenticated student flow.
- Published AI activities now submit through the legacy mission upload handlers with guarded `lid` resolution and clear mission-page states for ready, resubmit, locked, and blocked submission flows.
- MenuWorks 去重完成记录与学生完成态可见性回归，确保 AI 活动上传继续走既有课堂完成模型。

**Stats:**

- Timeline: 1 day (2026-04-11 -> 2026-04-11).
- Git range: `b57a58f` -> `6185f03`.
- Verification: Phase 06/07 targeted `net8.0` regression slices passed and
  authenticated human UAT was approved for submission and completion visibility.

**Archive note:**

- Milestone archival proceeded after user approval even though the pre-existing
  `.planning/v1.1-v1.1-MILESTONE-AUDIT.md` file was stale and no longer matched
  the completed Phase 06/07 state.

**What's next:** Define the next milestone with fresh requirements and roadmap
for the post-v1.1 teaching workflow improvements.

---

## v1.0 milestone (Shipped: 2026-04-11)

**Delivered:** The first end-to-end teacher activity-plan workflow shipped
inside `teacher/courseedit.aspx`, covering topic-first generation, structured
preview, guided section regeneration, saved-draft continuity, and append-only
editor apply.

**Phases completed:** 1-4 (8 plans total)

**Key accomplishments:**

- Server-side activity-plan prompt building with scoped skill bootstrap and
  default-provider request routing.

- Right-side course-editor activity-plan assistant with structured request
  wiring and safe text rendering.

- Structured activity-plan generation on the existing provider route with
  strict JSON parsing and validation.

- Structured right-side activity-plan preview cards with safe rendering and
  copy support.

- Per-course saved activity-plan drafts with migration-backed persistence and
  authenticated resume endpoints on the existing AI route.

- Append-only section apply, explicit confirmation, and in-panel save/resume
  draft continuity for the teacher course editor.

**Stats:**

- 4 phases, 8 plans, 8 tasks.
- Timeline: 1 day (2026-04-10 -> 2026-04-11).
- Git range: `49fadd6` -> `2293d08`.
- Verification: focused `net8.0` ActivityPlan and CourseEdit test slices
  passed; remaining follow-up is manual authenticated browser UAT for saved
  draft resume and append-only apply behavior.

**What's next:** Define the next milestone, then validate the remaining manual
browser checks or extend the teacher planning loop with the highest-value next
improvement.

---
