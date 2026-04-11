# Stack Research

**Domain:** Brownfield classroom activity generation and delivery in ASP.NET Web
Forms
**Researched:** 2026-04-11
**Confidence:** HIGH

## Recommended Stack

This milestone does not need a new frontend or service stack. The current
application already has the core primitives needed to deliver classroom
activities: teacher-side course editing, `Mission` records for activities,
`ListMenu` for student navigation and publish state, `Works` for submissions,
and `MenuWorks` and student status hooks for completion tracking.

The recommended approach is to extend those existing paths and add only small,
targeted data and handler changes where the current schema cannot represent
AI-generated activity structure. Avoid introducing a parallel student activity
system for v1.1.

### Core technologies

| Technology | Version | Purpose | Why recommended |
|------------|---------|---------|-----------------|
| ASP.NET Web Forms | .NET Framework 4.8 | Teacher and student page lifecycle, form postbacks, existing auth flow | The current teacher and student experiences already run on this stack, so reusing it avoids integration risk |
| SQL Server | 2022 target | Persist activity publish state, generated content, submissions, and completion | Existing `Mission`, `ListMenu`, `Works`, and migration utilities already use SQL Server |
| Existing AI provider route | current repo implementation | Generate structured teaching activity content from the teacher editor | v1.0 already validated provider routing and scoped skill use in the brownfield surface |

### Supporting libraries

| Library | Version | Purpose | When to use |
|---------|---------|---------|-------------|
| `Newtonsoft.Json` | 13.0.3 | Parse and validate structured AI activity payloads | Use for server-side JSON contracts between prompt builder and publish flow |
| Existing markdown and HTML guards | current repo implementation | Normalize and safely persist generated rich text into course or mission content | Use whenever AI output is copied into teacher-visible or student-visible content |
| Existing migration utilities | current repo implementation | Add any new persistence fields without ad-hoc SQL drift | Use if current mission or works schema is missing AI-activity metadata |

### Development tools

| Tool | Purpose | Notes |
|------|---------|-------|
| Existing C# regression tests | Validate BLL and page-level brownfield behavior | Extend focused tests around activity creation, publish state, and student entry |
| Existing JS tests | Validate teacher-side client workflow | Use for AI panel state, publish confirmations, and status rendering if logic stays in JS |

## Installation

No new packages are recommended for the milestone baseline. Prefer the current
repo stack unless a concrete schema or editor limitation forces a narrowly
scoped addition.

## Alternatives considered

| Recommended | Alternative | When to use alternative |
|-------------|-------------|-------------------------|
| Reuse `Mission` + `ListMenu` + `Works` | Create a brand-new activity subsystem | Only if existing mission records cannot represent the minimum student activity contract after careful code validation |
| Reuse existing student entry pages and menu | Build a new SPA student activity shell | Only for a future major redesign, not for this brownfield v1.1 |
| Keep AI generation server-routed | Call providers directly from browser | Only if the platform intentionally changes its provider security model, which is not in scope |

## What not to use

| Avoid | Why | Use instead |
|-------|-----|-------------|
| New standalone activity service for v1.1 | Duplicates auth, publish, and submission flows already solved in the monolith | Extend current BLL, DAL, pages, and handlers |
| Direct browser-to-provider AI calls | Exposes prompt and provider concerns in the client and bypasses established routing | Keep generation in the existing authenticated server route |
| New progress-tracking standard such as xAPI for this milestone | Adds infrastructure and integration cost beyond the validated need | Reuse current completion and submission records first |

## Stack patterns by variant

**If the AI-generated activity can map to existing `Mission` fields:**
- Reuse mission create and edit flows.
- Because the student navigation, publish toggle, and submission model already
  expect that shape.

**If the activity needs structured multi-step data beyond current mission
content:**
- Add narrowly scoped persistence fields or a side table linked to the mission
  record.
- Because the menu, publish, and works flows should still hang off the existing
  lesson activity identity.

## Version compatibility

| Package A | Compatible with | Notes |
|-----------|-----------------|-------|
| `Newtonsoft.Json` 13.0.3 | .NET Framework 4.8 | Already used in the repo for AI integration and tests |
| ASP.NET Web Forms | SQL Server persistence via `System.Data.SqlClient` | Existing monolith pattern, no stack change needed |

## Sources

- Repository code: `teacher/courseedit.aspx.cs`, `student/showcourse.aspx.cs`,
  `student/Scm.master.cs`, `App_Code/Bll/ListMenu.cs`,
  `App_Code/Dal/ListMenu.cs`
- Microsoft Learn: ASP.NET Page Life Cycle Overview — lifecycle and postback
  constraints verified for Web Forms integration
- W3C WCAG 2.1 Understanding 4.1.3 — status-message accessibility guidance for
  async generation and submission feedback

---
*Stack research for: brownfield classroom activity generation and delivery*
*Researched: 2026-04-11*
