# Architecture Research

**Domain:** AI-assisted teacher lesson planning inside an existing lesson or
course editor
**Researched:** 2026-04-10
**Confidence:** MEDIUM

## Standard architecture

This domain usually works best as an embedded assistive layer inside the
existing teacher authoring flow, not as a separate planning product. Current
platform patterns from Google Classroom, Blackboard Learn Ultra, and Canvas
IgniteAI all converge on the same shape: context is taken from the current
course or lesson editor, AI generates a draft structure, the teacher reviews
and edits it, and only then is content committed into the main lesson data
model.

For OpenLearnSite, that means adding a planning workflow beside existing
teacher authoring pages and reusing the current AI provider plus custom skill
infrastructure. Do not let the model write directly into course records as its
first action. Generate into a draft buffer first, then let the teacher accept
all, accept part, or manually edit before save.

### System overview

```text
┌──────────────────────────────────────────────────────────────────────┐
│                   Teacher authoring surface (Web Forms)             │
├──────────────────────────────────────────────────────────────────────┤
│  lesson/course editor page                                          │
│  ┌─────────────────┐  ┌──────────────────┐  ┌────────────────────┐  │
│  │ Existing fields │  │ AI plan panel    │  │ Review + insert UI │  │
│  │ title/content   │  │ topic, options   │  │ section mapping     │  │
│  └────────┬────────┘  └────────┬─────────┘  └──────────┬─────────┘  │
│           │                    │                       │            │
├───────────┴────────────────────┴───────────────────────┴────────────┤
│                    Request / orchestration layer                     │
├──────────────────────────────────────────────────────────────────────┤
│  teacher/*.ashx handler or page method                              │
│  ┌────────────────────────────────────────────────────────────────┐  │
│  │ planner orchestration service                                 │  │
│  │ - collect editor context                                      │  │
│  │ - build prompt from skill template                            │  │
│  │ - call provider                                               │  │
│  │ - parse structured plan                                       │  │
│  │ - persist draft / telemetry                                   │  │
│  └────────────────────────────────────────────────────────────────┘  │
├──────────────────────────────────────────────────────────────────────┤
│                   Shared domain + infrastructure                    │
├──────────────────────────────────────────────────────────────────────┤
│  AI provider config  │ skill templates │ draft store │ lesson store │
│  validation helpers  │ plan schema     │ audit/log   │ rubric links │
└──────────────────────────────────────────────────────────────────────┘
```

### Component responsibilities

| Component | Responsibility | Typical implementation |
|-----------|----------------|------------------------|
| Editor host | Owns canonical lesson or course form state | Existing `teacher/*.aspx` page |
| AI plan panel | Collects prompt inputs and shows generation status | Server-rendered panel plus JS |
| Planning handler | Auth, request validation, streaming progress, result payload | `teacher/*.ashx` |
| Planning orchestration service | Builds context, selects skill, calls model, parses output | `App_Code/Bll/*` service |
| Skill template layer | Stores pedagogical prompt and output contract | Existing `AICustomSkill` pattern |
| Plan draft repository | Saves generated structured plan before publish into lesson | New DAL/model tables or JSON column |
| Merge mapper | Maps accepted plan sections into existing lesson/activity fields | BLL helper near lesson domain |
| Lesson persistence | Writes approved content into `Courses` / `Mission` / `ListMenu` model | Existing BLL + DAL |
| Rubric / assessment integration | Reuses generated assessment design in rubric flow | Existing gauge flow |
| Audit + observability | Tracks prompt, provider, fallback, acceptance result, errors | Local logs + DB rows |

## Recommended project structure

This project is brownfield, so the right structure is extension, not a new
subsystem.

```text
teacher/
├── courseedit.aspx                  # Existing editor host, add AI plan entry
├── missionadd.aspx                  # Existing activity authoring page
├── lessonplan_generate.ashx         # New planning generation endpoint
├── lessonplan_draft_save.ashx       # Optional explicit draft save endpoint
└── lessonplan_apply.ashx            # Apply accepted draft into lesson fields

App_Code/
├── Bll/
│   ├── AILessonPlanner.cs           # Orchestration service
│   ├── LessonPlanDrafts.cs          # Draft lifecycle service
│   └── LessonPlanMapper.cs          # Map structured output to mission/course
├── Dal/
│   ├── LessonPlanDrafts.cs          # Draft persistence
│   └── LessonPlanFeedback.cs        # Optional acceptance / rating records
├── Model/
│   ├── LessonPlanDraft.cs           # Draft entity
│   ├── LessonPlanSection.cs         # Structured plan contract
│   └── LessonPlanRequest.cs         # Request/options contract
└── Common/
    └── LessonPlanSchema.cs          # Output validation / normalization

js/
└── lessonplan-editor.js             # Panel UI, streaming, preview, apply
```

### Structure rationale

- **`teacher/`:** Keep the teacher workflow page-first. Planning is a teacher
  authoring capability, so its endpoints belong with other teacher endpoints.
- **`App_Code/Bll/`:** Put orchestration here so page code-behind stays thin and
  provider logic remains reusable across lesson, course, and rubric workflows.
- **`App_Code/Dal/` + `Model/`:** Add draft persistence as its own concept.
  Don't overload the final lesson tables with half-approved AI output.
- **`js/`:** Keep browser behavior isolated from markup. The current repo already
  uses page-specific JS for AI-enabled authoring.

## Architectural patterns

### Pattern 1: Draft-first generation

**What:** AI writes to a structured draft record, not directly to the canonical
lesson row.

**When to use:** Always for multi-section pedagogical plans.

**Trade-offs:** Slightly more persistence work, but much safer than immediate
overwrite and much easier to support partial acceptance.

**Example:**

```typescript
type LessonPlanDraft = {
  topic: string;
  goals: string[];
  activities: Array<{ title: string; minutes: number; method: string }>;
  resources: string[];
  assessment: string[];
  status: 'generated' | 'edited' | 'applied';
};
```

### Pattern 2: Structured-output contract over free-form prose

**What:** Ask the model for named sections with predictable fields, then render
those sections into the UI.

**When to use:** When teachers need to insert parts of a plan into different
editor fields or convert plan steps into multiple activities.

**Trade-offs:** More schema work up front, but much better downstream control,
validation, and reuse.

**Example:**

```typescript
type PlannerResponse = {
  lessonTitle: string;
  teachingGoals: string[];
  activityFlow: Array<{
    stepTitle: string;
    timeMinutes: number;
    teacherAction: string;
    studentAction: string;
    interactionMode: string;
  }>;
  resources: string[];
  assessmentDesign: string[];
};
```

### Pattern 3: Context sandwich orchestration

**What:** Build the model request from three layers: fixed pedagogy skill,
editor context, and teacher intent.

**When to use:** When the same planner must work inside multiple editors and
remain consistent across providers.

**Trade-offs:** More orchestration code, but better quality and less prompt
drift.

**Example:**

```typescript
const request = {
  systemSkill: 'lesson-planning-v1',
  editorContext: {
    gradeLevel,
    subject,
    existingCourseTitle,
    existingObjectives,
  },
  teacherInput: {
    topic,
    constraints,
    classDuration,
  },
};
```

## Data flow

The dominant pattern in embedded planning systems is:

1. Teacher starts in the normal editor.
2. Teacher opens the AI plan panel.
3. System reads local editor context.
4. Server combines that context with a planning skill template.
5. AI returns structured draft data.
6. Server validates and stores the draft.
7. UI renders a reviewable plan.
8. Teacher applies all or selected sections.
9. Existing lesson save flow persists final approved content.

### Request flow

```text
[Teacher opens lesson/course editor]
    ↓
[AI plan panel collects topic + options]
    ↓
[lessonplan_generate.ashx]
    ↓
[AILessonPlanner.BuildContext()]
    ↓
[AI provider + lesson-planning skill]
    ↓
[structured plan response]
    ↓
[schema validation + fallback normalization]
    ↓
[LessonPlanDrafts.Save()]
    ↓
[UI preview with accept/edit actions]
    ↓
[lessonplan_apply.ashx or normal form submit]
    ↓
[Mission/Courses/ListMenu existing persistence]
```

### Key data flows

1. **Editor context to planner:**
   Existing page fields, teacher role, grade, subject, and current course data
   flow into the orchestration service. This is what makes the planner embedded
   instead of generic.

2. **Planner draft to canonical lesson data:**
   Generated output first becomes a draft object. Only teacher-approved sections
   are mapped into final lesson/activity fields.

3. **Assessment handoff:**
   Assessment design from the plan can seed the existing rubric or gauge flow
   instead of duplicating rubric logic inside planning.

4. **Feedback loop:**
   Apply/reject/edit actions should be stored so later prompt and UX tuning is
   based on real teacher acceptance, not only generation counts.

## Suggested build order

Build in this order because each step reduces rework for the next one.

1. **Planning schema and draft model**
   - Define the structured response contract.
   - Add draft persistence.
   - This must come first because UI and prompt design both depend on it.

2. **Server orchestration service**
   - Add `AILessonPlanner` on top of the existing provider layer.
   - Reuse the custom skill pattern.
   - Include fallback behavior and validation from day one.

3. **Generation endpoint with progress reporting**
   - Mirror the existing `gauge_generate.ashx` pattern.
   - Stream progress and return draft identifiers.

4. **Embedded review panel in existing editor**
   - Add generate, preview, copy, and selective apply actions.
   - Keep the current editor as the source of truth.

5. **Apply mapper into lesson/activity records**
   - Convert structured steps into `Mission` content and, if needed,
     `ListMenu` items.
   - This is where most brownfield coupling risk sits, so do it after draft and
     UI behavior are stable.

6. **Rubric and assessment linkage**
   - Feed assessment suggestions into the existing gauge workflow.
   - Do this after the base planning loop works.

7. **Telemetry and quality feedback**
   - Capture fallback rate, apply rate, section edits, and failure causes.
   - Needed before expanding to broader course-planning automation.

## Scaling considerations

| Scale | Architecture adjustments |
|-------|--------------------------|
| 0-1k teachers | Web Forms monolith + DB draft storage is enough |
| 1k-10k teachers | Add queueing or async job option for long generations; tighten provider timeout and retry policy |
| 10k+ teachers | Separate AI orchestration from page request path, but keep teacher editor integration thin |

### Scaling priorities

1. **First bottleneck: provider latency**
   The UI will feel broken before the database breaks. Add progress streaming,
   timeout handling, and saved drafts before any microservice split.

2. **Second bottleneck: noisy low-quality outputs**
   Quality control becomes harder than infrastructure. Improve schema,
   validation, and teacher acceptance analytics before adding more automation.

## Anti-patterns

### Anti-pattern 1: Direct AI overwrite of lesson content

**What people do:** Send the prompt and immediately replace the editor body.

**Why it's wrong:** Teachers lose control, partial acceptance is impossible,
and errors become destructive.

**Do this instead:** Save a structured draft and apply only after teacher
review.

### Anti-pattern 2: Treat planning as one giant text blob

**What people do:** Generate a single markdown lesson plan and paste it into one
field.

**Why it's wrong:** The plan cannot map cleanly into goals, steps, resources,
and assessment. Later reuse becomes expensive.

**Do this instead:** Require named sections and validate them before render.

### Anti-pattern 3: Bypass the existing AI provider and skill layer

**What people do:** Add ad hoc provider calls inside page code-behind.

**Why it's wrong:** Configuration, routing, fallback behavior, and governance
fragment immediately.

**Do this instead:** Reuse the existing provider selection and custom skill
infrastructure.

## Integration points

### External services

| Service | Integration pattern | Notes |
|---------|---------------------|-------|
| OpenAI-compatible provider | Server-side chat completion call through existing provider config | Reuse current provider routing |
| Future standards / curriculum source | Optional retrieval context before generation | Keep out of v1 unless curriculum alignment is a hard requirement |

### Internal boundaries

| Boundary | Communication | Notes |
|----------|---------------|-------|
| `teacher/*.aspx` ↔ planning handler | AJAX or SSE-style progress updates | Match existing AI generation UX |
| Planning handler ↔ `App_Code/Bll/AILessonPlanner` | Direct method call | Keep page code thin |
| Planner ↔ `AIProvider` / `AICustomSkill` | Existing BLL + DAL | Reuse current governance points |
| Planner draft ↔ lesson persistence | Explicit apply step | Never implicit overwrite |
| Plan assessment ↔ gauge generation | Seed data handoff | Reuse rubric capability already present |

## Sources

- OpenLearnSite project context: `.planning/PROJECT.md` and
  `.planning/codebase/{ARCHITECTURE,INTEGRATIONS,STRUCTURE}.md`
  (HIGH confidence)
- Google for Education / Google Classroom official resources on Gemini in
  Classroom lesson-planning workflows, including Help Center references surfaced
  via search such as `support.google.com/classroom/answer/14732168` and
  `support.google.com/edu/classroom/answer/15039234` (MEDIUM confidence;
  official domain, but direct page fetch failed)
- Anthology / Blackboard official materials on AI Design Assistant and
  auto-generated modules in Blackboard Learn Ultra, surfaced via official-domain
  search and help references at `help.blackboard.com` and `anthology.com`
  (MEDIUM confidence)
- Instructure official materials on Canvas IgniteAI and IgniteAI Agent,
  surfaced via official-domain search and community references at
  `community.canvaslms.com` and `instructure.com` (MEDIUM confidence)

---

*Architecture research for: AI lesson planning inside existing teacher editor*
*Researched: 2026-04-10*
