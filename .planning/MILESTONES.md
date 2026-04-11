# Project Milestones: OpenLearnSite teaching skills enhancement

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
