# Project Retrospective

*A living document updated after each milestone. Lessons feed forward into
future planning.*

## Milestone: v1.0 — milestone

**Shipped:** 2026-04-11
**Phases:** 4 | **Plans:** 8 | **Sessions:** 1

### What Was Built
- Embedded a topic-first activity-plan assistant directly into
  `teacher/courseedit.aspx`.
- Added structured draft generation, read-only preview rendering, and
  section-level regeneration on the existing authenticated AI handler path.
- Added per-course saved-draft continuity plus append-only selective apply back
  into the existing lesson editor.

### What Worked
- Reusing the existing provider route and scoped-skill infrastructure kept the
  feature inside the brownfield architecture instead of creating a parallel AI
  stack.
- Focused `net8.0` test slices for `ActivityPlan` and `CourseEdit` gave fast,
  repeatable verification while the legacy `net48` runtime remained unavailable
  on Linux.
- Keeping teacher-facing changes inside one right-side panel preserved the
  existing course editor workflow and reduced UI sprawl.

### What Was Inefficient
- Several planning documents were backfilled after implementation, which made
  milestone completion depend on extra documentation cleanup.
- Milestone archival automation did not complete all documented follow-up steps,
  so `PROJECT.md`, `ROADMAP.md`, `STATE.md`, and retrospective updates still
  needed manual finish work.
- Manual browser UAT remains open because authenticated Web Forms interaction
  with a live provider is not covered by the automated source-lock tests.

### Patterns Established
- Server-composed structured activity-plan prompts with strict fail-closed draft
  parsing.
- Preview-only teacher review flows that preserve existing lesson content until
  an explicit append action.
- One-current-draft-per-course continuity with teacher-owned course checks and
  saved-draft status gates.

### Key Lessons
1. In this codebase, the safest way to add AI features is to reuse the existing
   authenticated handler and constrain output with typed server-side validation.
2. Brownfield teacher UX changes land more cleanly when they extend the
   existing editor surface instead of introducing a new workflow boundary.

### Cost Observations
- Model mix: unknown
- Sessions: 1
- Notable: focused regression locks kept the milestone cheap to verify, but the
  remaining human-only checks still need explicit scheduling.

---

## Cross-Milestone Trends

### Process Evolution

| Milestone | Sessions | Phases | Key Change |
|-----------|----------|--------|------------|
| v1.0 | 1 | 4 | Established the brownfield pattern for shipping scoped AI planning features through existing teacher editor surfaces |

### Cumulative Quality

| Milestone | Tests | Coverage | Zero-Dep Additions |
|-----------|-------|----------|-------------------|
| v1.0 | Focused xUnit and source-lock regression slices | Targeted feature coverage | 0 |

### Top Lessons (Verified Across Milestones)

1. Reuse existing authenticated product surfaces before inventing parallel AI
   routes or standalone planning tools.
2. Keep generated teaching content preview-only until the teacher explicitly
   chooses what to apply.
