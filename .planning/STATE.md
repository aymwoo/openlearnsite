---
gsd_state_version: 1.0
milestone: v1.2
milestone_name: AI 整课编排与活动组合
status: requirements
stopped_at: Defining milestone v1.2 requirements
last_updated: "2026-04-11T10:50:07.609Z"
last_activity: 2026-04-11
progress:
  total_phases: 3
  completed_phases: 3
  total_plans: 7
  completed_plans: 7
  percent: 100
---

# Project State

## Project Reference

See: .planning/PROJECT.md (updated 2026-04-11)

**Core value:** Teachers can turn a teaching topic into a concrete, teachable
activity plan without having to manually break the lesson into steps.
**Current focus:** Defining milestone v1.2

## Current Position

Milestone: v1.2 (AI 整课编排与活动组合)
Phase: Not started (defining requirements)
Plan: -
Status: Defining requirements
Last activity: 2026-04-11

Progress: [░░░░░░░░░░] 0%

## Performance Metrics

**Velocity:**

- Total plans completed: 13
- Average duration: -
- Total execution time: 0.0 hours

**By Phase:**

| Phase | Plans | Total | Avg/Plan |
|-------|-------|-------|----------|
| 01 | 2 | - | - |
| 02 | 2 | - | - |
| 03 | 2 | - | - |
| 04 | 2 | - | - |
| 07 | 2 | - | - |

**Recent Trend:**

- Last 5 plans: none
- Trend: Stable

| Phase 04 P01 | 38m | 2 tasks | 8 files |
| Phase 04 P02 | 26m | 2 tasks | 3 files |
| Phase 05 P01 | 4min | 2 tasks | 8 files |
| Phase 05 P02 | 5min | 2 tasks | 4 files |
| Phase 05 P03 | 4min | 2 tasks | 4 files |
| Phase 06 P01 | 6min | 2 tasks | 7 files |
| Phase 06 P02 | 12min | 2 tasks | 5 files |

## Accumulated Context

### Decisions

Decisions are logged in PROJECT.md Key Decisions table.
Recent decisions affecting current work:

- [Roadmap]: Keep v1 inside the existing teacher lesson or course editing page.
- [Roadmap]: Focus v1 on structured activity-plan generation from lightweight
  teacher input.

- [Roadmap]: Keep teachers in control through preview, regeneration, selective
  apply, and saved drafts.

- [Phase 01]: Build activity-plan requests on the server and reuse the default
  AI provider route instead of exposing raw prompts in the browser.

- [Phase 01]: Keep the course editor as the primary surface and embed the
  activity-plan assistant as a right-side panel.

- [Phase 04]: Keep saved activity-plan continuity in a dedicated per-course draft table and expose it through the existing authenticated AI handler instead of mixing draft state into lesson content.
- [Phase 04]: Require explicit draft-status checks and teacher-owned course authorization before any save, load, or delete draft action succeeds.
- [Phase 04]: Apply approved plan sections by appending labeled blocks into the existing editor content instead of calling a second lesson-save path.
- [Phase 04]: Recheck saved-draft status on generate so replace-versus-continue prompts do not rely only on stale browser state.
- [Milestone v1.0]: Archive detailed v1.0 roadmap and requirements into `.planning/milestones/` and keep the live roadmap ready for next-milestone planning.
- [Phase 05]: Use explicit publish request/result models and draft-linked mission/menu ids to keep one stable AI activity identity per course.
- [Phase 05]: Route course, mission, menu, and draft-link writes through one publish transaction and write the editor back from committed server content.
- [Phase 06]: Keep Ltype=1 hard-routed to student/showmission.aspx?lid= for published AI activities.
- [Phase 06]: Render student guidance from published Mission.Mcontent on the server with raw-content fallback when AI markers are absent.
- [Phase 06]: Prepend the published mission topic to learner instructions so students always get a stable first cue on showmission.aspx.
- [Phase 06]: Document authenticated browser UAT in the summary because the repository has no dedicated student UI regression suite.

### Pending Todos

- None.

### Blockers/Concerns

None.

## Session Continuity

Last session: 2026-04-11T10:50:07Z
Stopped at: Defining milestone v1.2 requirements
Resume file: .planning/PROJECT.md
