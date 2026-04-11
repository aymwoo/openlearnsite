# Architecture Research

**Domain:** Extending a brownfield Web Forms LMS from AI planning to executable
classroom activities
**Researched:** 2026-04-11
**Confidence:** HIGH

## Standard architecture

The recommended architecture is to keep one continuous flow across existing
teacher authoring, lesson activity persistence, student navigation, and
submission tracking. New milestone logic should attach to those boundaries
instead of adding a separate subsystem.

### System overview

```text
┌─────────────────────────────────────────────────────────────┐
│                   Teacher authoring surface                 │
├─────────────────────────────────────────────────────────────┤
│  `teacher/courseedit.aspx`                                 │
│  AI generation panel + review + publish action             │
└───────────────┬─────────────────────────────────────────────┘
                │
┌───────────────▼─────────────────────────────────────────────┐
│                 Existing server-side workflow               │
├─────────────────────────────────────────────────────────────┤
│  AI provider route / prompt builder                        │
│  BLL orchestration for course, mission, and menu changes   │
│  DAL persistence with migration-backed schema updates       │
└───────────────┬─────────────────────────────────────────────┘
                │
┌───────────────▼─────────────────────────────────────────────┐
│                   Existing classroom stores                 │
├─────────────────────────────────────────────────────────────┤
│  `Courses`  `Mission`  `ListMenu`  `Works`  `MenuWorks`    │
└───────────────┬─────────────────────────────────────────────┘
                │
┌───────────────▼─────────────────────────────────────────────┐
│                   Student classroom surface                 │
├─────────────────────────────────────────────────────────────┤
│  `student/Scm.master` menu entry                           │
│  existing student activity page (`showmission` / `showtask`│
│  or closest compatible path) + submission handling         │
└─────────────────────────────────────────────────────────────┘
```

### Component responsibilities

| Component | Responsibility | Typical implementation |
|-----------|----------------|------------------------|
| Teacher course editor | Capture topic, show AI result, let teacher confirm add and publish | Extend existing `teacher/courseedit.aspx` and related JS |
| Activity publish orchestrator | Convert approved AI output into mission, menu, and lesson content updates atomically | Add focused server-side orchestration in BLL or handler layer |
| Student activity surface | Render published activity guidance, accept submission, show status | Reuse existing student activity pages and menu conventions |

## Recommended project structure

```text
teacher/
├── courseedit.aspx(.cs)     # teacher-side AI activity generation and publish

student/
├── Scm.master(.cs)          # student activity menu entry and status context
├── showcourse.aspx(.cs)     # lesson content landing page
└── showmission.aspx(.cs)    # likely activity entry reuse path

App_Code/
├── Bll/                     # publish orchestration and reuse wrappers
├── Dal/                     # mission/menu/works persistence
├── Model/                   # data contracts for lesson activity entities
└── Utility/                 # migration utilities for schema evolution
```

### Structure rationale

- **Teacher pages:** keep teacher-side generation and confirmation inside the
  current course editor workflow.
- **Student pages:** keep entry through the existing course menu and student
  activity pages so classroom navigation stays familiar.
- **BLL and DAL:** place multi-record publish logic on the server to protect
  auth, sequence, and transaction boundaries.

## Architectural patterns

### Pattern 1: Server-side publish orchestration

**What:** One authenticated server action creates or updates lesson content,
activity content, and menu visibility together.
**When to use:** Whenever the teacher confirms publish of AI-generated content.
**Trade-offs:** Slightly more server logic, but much lower data drift risk.

### Pattern 2: Existing-identity reuse

**What:** Treat published AI activities as first-class lesson activities using
the same IDs and menu records as hand-authored work.
**When to use:** Default v1.1 path.
**Trade-offs:** Constrained by legacy schema, but far safer than parallel models.

### Pattern 3: Structured-content fallback

**What:** Prefer structured activity data when available, but degrade to safe
rendered mission content if the student page only understands rich text.
**When to use:** When current student pages cannot yet render richer step data.
**Trade-offs:** Faster delivery, but may limit interactive richness in early
phases.

## Data flow

### Request flow

```text
[Teacher confirms AI activity]
    ↓
[courseedit handler / page action]
    ↓
[BLL publish orchestrator]
    ↓
[Course + Mission + ListMenu persistence]
    ↓
[Student menu renders entry]
    ↓
[Student opens activity and submits result]
    ↓
[Works / MenuWorks / status update]
```

### Key data flows

1. **Teacher generation to publish:** AI output becomes teacher-reviewed lesson
   content plus a publishable classroom activity record.
2. **Student entry to completion:** A visible lesson menu entry opens the
   activity page, then submission and completion update existing classroom
   records.

## Anti-patterns

### Anti-pattern 1: Split-brain publish state

**What people do:** Save activity content in one table, but forget to update the
student-visible menu or publish flag in sync.
**Why it's wrong:** Teachers think the activity is published, but students
cannot enter it or see stale content.
**Do this instead:** Publish through one server-side path that owns mission,
menu, and visibility updates together.

### Anti-pattern 2: Browser-owned classroom publishing

**What people do:** Let client JS decide publish records or map AI output into
student activity fields directly.
**Why it's wrong:** Weak auth and brittle data contracts.
**Do this instead:** Keep AI-to-activity mapping on the authenticated server.

## Integration points

### External services

| Service | Integration pattern | Notes |
|---------|---------------------|-------|
| Existing AI provider route | Server-side request from teacher authoring flow | Keep prompt construction and provider selection server-owned |

### Internal boundaries

| Boundary | Communication | Notes |
|----------|---------------|-------|
| `courseedit.aspx` ↔ AI route | existing authenticated request | Reuse v1.0 generation contract where possible |
| Teacher publish flow ↔ `Mission` / `ListMenu` | direct BLL and DAL calls | Must make new versus modified records explicit |
| Student menu ↔ activity page | querystring lesson activity identity | Existing menu flow already expects this pattern |
| Student submit ↔ `Works` / status tables | existing submission handlers | Reuse before designing new completion storage |

## Sources

- Repository code: `teacher/courseedit.aspx.cs`, `student/showcourse.aspx.cs`,
  `student/Scm.master.cs`, `App_Code/Bll/ListMenu.cs`,
  `App_Code/Dal/ListMenu.cs`
- Microsoft Learn: ASP.NET Page Life Cycle Overview

---
*Architecture research for: brownfield AI classroom activity delivery*
*Researched: 2026-04-11*
