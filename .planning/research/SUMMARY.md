# Project Research Summary

**Project:** OpenLearnSite teaching skills enhancement
**Domain:** Brownfield AI-generated classroom activity authoring and delivery
**Researched:** 2026-04-11
**Confidence:** HIGH

## Executive summary

The next milestone fits the existing platform architecture. The current repo
already has the key building blocks for classroom delivery: teacher-side course
editing, AI generation routed through the server, lesson activity identity via
`Mission`, student navigation via `ListMenu`, and student submission and status
primitives via `Works`, `MenuWorks`, and related status hooks. The recommended
approach is to extend those existing paths rather than introduce a parallel
activity subsystem.

Research indicates that the milestone should be treated as an AI-to-execution
bridge, not as a broad learning-platform redesign. The essential scope is: the
teacher confirms AI-generated activity output, the system creates or updates a
real classroom activity and publish state, the student can enter it from the
existing class menu, and the submission path records completion in a way the
current platform can understand.

The biggest risk is split-brain state between teacher-side generated content and
student-side published activity records. The roadmap should therefore begin with
one server-owned publish orchestration phase before expanding the student
experience and tracking rules.

## Key findings

### Recommended stack

No new major stack additions are recommended for v1.1. Reuse ASP.NET Web Forms,
SQL Server, the existing AI provider route, and the current activity data model
as far as possible.

**Core technologies:**
- ASP.NET Web Forms: teacher and student page flow, auth, and postback model
- SQL Server: lesson activity, publish, submission, and completion persistence
- Existing AI provider route: server-side structured generation from teacher
  input

### Expected features

**Must have (table stakes):**
- Teacher can convert AI output into a real lesson activity
- Teacher controls publish state before students see it
- Student can enter the published activity from the existing class menu
- Student can view activity guidance, submit a result, and have completion
  recorded

**Should have (competitive):**
- One generation flow fills both lesson content and student activity entry
- Step-by-step student guidance derived from structured AI output

**Defer (v2+):**
- Autonomous publish
- Broad collaboration or live orchestration features
- Full analytics redesign

### Architecture approach

The milestone should keep one continuous flow: `teacher/courseedit.aspx` and the
existing AI route produce approved output, a server-side publish orchestrator
maps that output into `Mission` and `ListMenu` records plus lesson content, and
the student enters via the existing menu and submission path.

**Major components:**
1. Teacher generation and publish surface — captures and confirms AI activity
2. Publish orchestrator — creates or updates lesson content, mission state, and
   menu visibility together
3. Student activity and submission surface — renders guidance and records the
   classroom result

### Critical pitfalls

1. **Planning output not publishable** — ensure teacher confirmation creates a
   real classroom activity identity
2. **Teacher and student publish-state drift** — update mission, menu, and
   content in one server-side path
3. **Structured guidance lost on student page** — define the minimum student
   activity render contract early
4. **Completion semantics unclear** — decide what qualifies as completion before
   wiring submission

## Implications for roadmap

### Phase 1: Teacher publish foundation
**Rationale:** Publish orchestration is the dependency for all student-facing
work.
**Delivers:** AI-to-activity mapping, mission or menu record creation or update,
and explicit teacher publish control.
**Addresses:** Teacher authoring and classroom publish bridge.
**Avoids:** Planning-only output and publish-state drift.

### Phase 2: Student activity experience
**Rationale:** Once real activity identity exists, the student needs a clear
entry and guided activity page.
**Delivers:** Student menu entry, activity rendering, and visible step guidance.
**Uses:** Existing menu and student activity pages where possible.
**Implements:** Structured student-facing activity contract.

### Phase 3: Submission and completion tracking
**Rationale:** The user-defined minimum scope includes submission and recorded
completion.
**Delivers:** Student submission path and reliable completion semantics.
**Uses:** Existing `Works`, `MenuWorks`, and classroom tracking hooks.

### Phase ordering rationale

- Publish identity must exist before student entry can be stable.
- Student rendering must be defined before submission rules can be validated.
- Completion tracking must reuse existing classroom records instead of becoming
  a parallel system.

### Research flags

Phases likely needing deeper research during planning:
- **Phase 1:** exact mapping from AI-generated structure into existing mission
  schema or a linked side table
- **Phase 3:** precise rule for completion if current submission models differ
  by activity type

Phases with standard patterns:
- **Phase 2:** menu entry, status messaging, and step rendering follow common
  Web Forms and accessibility patterns

## Confidence assessment

| Area | Confidence | Notes |
|------|------------|-------|
| Stack | HIGH | Existing repo already provides required baseline |
| Features | HIGH | User-defined minimum scope is clear |
| Architecture | HIGH | Strong existing integration points were found in code |
| Pitfalls | HIGH | Main risks come from brownfield state drift, not unknown tech |

**Overall confidence:** HIGH

### Gaps to address

- Exact activity data contract for the student page: decide whether existing
  mission content is sufficient or needs structured side data.
- Exact completion rule: decide whether submission alone marks completion or
  whether current status hooks require extra updates.

## Sources

### Primary (high confidence)
- Repository code: teacher course editor, student course page, student menu,
  lesson menu DAL and BLL
- Microsoft Learn: ASP.NET Page Life Cycle Overview
- W3C WCAG 2.1 Understanding 4.1.3 Status Messages

### Secondary (medium confidence)
- W3C multi-page forms tutorial for step and progress guidance patterns

---
*Research completed: 2026-04-11*
*Ready for roadmap: yes*
