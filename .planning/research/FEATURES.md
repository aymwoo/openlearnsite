# Feature Research

**Domain:** Teacher-facing AI lesson-planning assistants for K-12 classroom
activity design inside an existing teaching platform
**Researched:** April 10, 2026
**Confidence:** MEDIUM

## Feature Landscape

This landscape focuses on features that help a teacher turn a topic or
knowledge point into a usable classroom activity plan without leaving an
existing lesson-authoring workflow. Across MagicSchool, Brisk, and SchoolAI,
the common pattern is not "generic chat," but a structured assistant that
produces editable lesson components, fits into the teacher's existing tools,
and keeps the teacher in control.

### Table stakes (users expect these)

These are the features a teacher now reasonably expects from an AI lesson
planning assistant. Missing them makes the product feel like a generic text
generator instead of a real teaching tool.

| Feature | Why Expected | Complexity | Notes |
|---------|--------------|------------|-------|
| Topic or knowledge point to structured activity plan | Leading tools all start from a short teacher prompt and return a usable lesson draft, not blank-chat output | MEDIUM | Must accept minimal input, then generate a predictable schema rather than free-form prose |
| Structured pedagogical sections | Teachers expect goals, step flow, timing, interaction method, resources, and assessment because that is the real missing work | MEDIUM | This is the core v1 value for OpenLearnSite and should map directly into editable form sections |
| Teacher context controls | Grade, subject, lesson length, and class context are standard inputs for getting usable output | LOW | Keep optional defaults so the flow stays lightweight, but expose them when teachers need precision |
| Standards or curriculum alignment | MagicSchool and Brisk both position alignment as normal, not advanced | MEDIUM | v1 can support standards tags or alignment notes before full curriculum grounding exists |
| Section-level edit and regenerate | Teachers expect to keep what works and only rewrite weak parts | MEDIUM | Regenerate goals, activities, or assessment independently; do not force full-plan regeneration every time |
| Resource and assessment suggestions | Competitors pair planning with prompts for materials, checks for understanding, and exit tickets | MEDIUM | In OpenLearnSite, this can first be suggestions embedded in the plan, not full artifact generation |
| In-workflow integration | Brisk and SchoolAI win by working where teachers already plan; context-switching is now a product weakness | MEDIUM | The assistant must live in the existing lesson or course editing page, matching PROJECT.md |
| Human review before use | Education products consistently frame AI output as a draft for teacher judgment | LOW | Keep explicit review, edit, and save steps; never imply auto-approved pedagogy |

### Differentiators (competitive advantage)

These features are not required for the first useful release, but they are how
the product becomes meaningfully better than a generic lesson-plan generator.

| Feature | Value Proposition | Complexity | Notes |
|---------|-------------------|------------|-------|
| Curriculum-grounded generation from school materials | Brisk's Curriculum Intelligence shows the market moving from generic AI to district-aligned AI | HIGH | Best differentiator after v1; use existing course content, uploaded exemplars, or school-approved materials as grounding data |
| One-click differentiated variants | Teachers value support, on-level, extension, ELL, and scaffolded versions without rewriting the whole plan | HIGH | Build on structured output first; easiest path is variant generation per activity step |
| Generate a classroom-ready bundle | A plan becomes more useful when it can also draft an exit ticket, worksheet, quiz, rubric, or slides | HIGH | Strong v1.x move because OpenLearnSite already has AI support in adjacent teaching tasks |
| Reuse existing lesson or course context as hidden prompt context | Brownfield advantage: the platform already knows course metadata and nearby content | MEDIUM | Use title, topic, prior activity fields, and existing course text to reduce teacher prompting burden |
| Reusable activity templates and remixing | SchoolAI's Spaces library shows teachers like adapting proven structures, not always starting from zero | MEDIUM | Start with internal templates such as discussion, experiment, group task, review game, and exit ticket flow |
| Data-informed plan suggestions | Plans get better when they respond to prior student weaknesses, rubric history, or assessment gaps | HIGH | Valuable later-phase feature once integration with existing student performance data is reliable |

### Anti-features (commonly requested, often problematic)

These features sound impressive, but they are likely to create low trust,
implementation drag, or scope creep for this milestone.

| Feature | Why Requested | Why Problematic | Alternative |
|---------|---------------|-----------------|-------------|
| One-click autonomous publishing or classroom execution | It sounds like maximum time savings | It breaks the project's human-review constraint and raises trust, quality, and accountability risks | Keep teacher review, approval, and manual save/publish decisions |
| Giant prose-only lesson dump | It feels fast to generate a lot of text at once | Teachers still have to re-structure it manually, so it does not solve the workflow pain | Generate fixed sections with labels, timings, and editable chunks |
| Separate standalone planning app | It appears cleaner than integrating into legacy pages | It adds app-switching and fights the validated teacher workflow in PROJECT.md | Embed the assistant into the existing lesson or course editor |
| Fully personalized student-level plans by default | It sounds advanced and student-centered | It adds privacy, data, and classroom-management complexity too early | Start with class-level plans plus optional differentiated variants |
| Full unit or semester generation from one sentence in v1 | It looks impressive in demos | Output quality drops, review burden explodes, and scope jumps far beyond the current milestone | Start with single-lesson or single-activity planning and expand later |

## Feature Dependencies

```text
[Teacher context capture]
    └──requires──> [Structured activity plan generation]
                           ├──requires──> [Section-level edit/regenerate]
                           ├──enhances──> [Standards/curriculum alignment]
                           ├──enhances──> [Resource and assessment suggestions]
                           └──requires──> [In-workflow integration]

[Differentiated variants]
    └──requires──> [Structured activity plan generation]

[Classroom-ready bundle generation]
    └──requires──> [Structured activity plan generation]

[Curriculum-grounded generation]
    └──requires──> [Approved curriculum or lesson-source grounding]

[Data-informed plan suggestions]
    └──requires──> [Reliable assessment/performance data access]

[Standalone planning app] ──conflicts──> [In-workflow integration]
[Autonomous publishing] ──conflicts──> [Human review before use]
```

### Dependency notes

- **Teacher context capture requires structured activity plan generation:**
  context fields only matter if the output schema can reliably use them.
- **Structured activity plan generation requires section-level edit and
  regenerate:** without targeted edits, teachers must keep rerolling the whole
  lesson and trust drops quickly.
- **Structured activity plan generation enhances standards alignment:** a fixed
  schema makes it easier to attach standards to goals and activities.
- **Structured activity plan generation enhances resource and assessment
  suggestions:** once the system knows each step, it can recommend matching
  materials and checks for understanding.
- **Differentiated variants require structured activity plan generation:** it is
  much easier to adapt a known activity structure than to differentiate raw
  prose.
- **Classroom-ready bundle generation requires structured activity plan
  generation:** the lesson plan should become the source for quizzes, rubrics,
  and materials.
- **Curriculum-grounded generation requires approved curriculum or lesson-source
  grounding:** otherwise the product stays generic and cannot claim strong local
  alignment.
- **Data-informed plan suggestions require reliable assessment or performance
  data access:** weak data links will produce low-trust recommendations.
- **Standalone planning app conflicts with in-workflow integration:** it solves
  the wrong problem for this brownfield project.
- **Autonomous publishing conflicts with human review before use:** teacher
  oversight is a product requirement, not an optional safety layer.

## MVP Definition

The MVP should solve one narrow job well: help a teacher enter a topic and get
an editable classroom activity plan inside the page they already use.

### Launch with (v1)

Minimum viable product means the smallest feature set that proves teachers will
trust and reuse the assistant during real preparation.

- [ ] Topic or knowledge point input with optional grade, subject, and duration
      fields — keeps prompting lightweight but useful
- [ ] Structured activity plan output — goals, step-by-step flow, time,
      interaction mode, resource suggestions, and assessment design
- [ ] Section-level edit, regenerate, and save into the existing lesson editor
      — keeps the teacher in control and reduces rewrite work
- [ ] Basic standards or alignment note support — enough to show the plan is not
      generic
- [ ] Embedded experience inside the existing teacher lesson or course page —
      matches the validated workflow

### Add after validation (v1.x)

These features make the assistant substantially more useful once teachers are
already adopting the core plan generator.

- [ ] Differentiated variants by readiness level or support need — add when
      teachers repeatedly edit plans for mixed-ability classrooms
- [ ] Artifact generation from the plan — add when teachers want one-click exit
      tickets, rubrics, slides, or worksheets from approved plans
- [ ] Reusable activity templates and remixing — add when recurring lesson
      patterns appear across subjects or teacher groups

### Future consideration (v2+)

These features are strong long-term bets, but they rely on more platform and
data maturity than this milestone needs.

- [ ] Curriculum-grounded generation from district or school materials — defer
      until content ingestion and governance are ready
- [ ] Data-informed recommendations from student performance — defer until data
      quality and permissions are trustworthy
- [ ] Cross-lesson or unit sequencing assistance — defer until single-lesson
      planning quality is proven

## Feature prioritization matrix

This matrix recommends what to build first based on user value and cost inside
the current OpenLearnSite constraints.

| Feature | User Value | Implementation Cost | Priority |
|---------|------------|---------------------|----------|
| Structured activity plan generation | HIGH | MEDIUM | P1 |
| In-workflow integration in the lesson editor | HIGH | MEDIUM | P1 |
| Section-level edit and regenerate | HIGH | MEDIUM | P1 |
| Teacher context controls | MEDIUM | LOW | P1 |
| Basic standards or alignment notes | MEDIUM | MEDIUM | P2 |
| Differentiated variants | HIGH | HIGH | P2 |
| Artifact bundle generation | HIGH | HIGH | P2 |
| Reusable templates and remixing | MEDIUM | MEDIUM | P2 |
| Curriculum-grounded generation | HIGH | HIGH | P3 |
| Data-informed plan suggestions | HIGH | HIGH | P3 |

**Priority key:**
- P1: Must have for launch
- P2: Should have after core validation
- P3: Future consideration

## Competitor feature analysis

The market signal is clear: teachers want structured output, low-friction
workflow fit, and local relevance more than they want a flashy chatbot.

| Feature | MagicSchool | Brisk | Our Approach |
|---------|-------------|-------|--------------|
| Structured lesson output | Lesson Plan Generator emphasizes customization and standards alignment | Lesson Plan Generator explicitly promises objectives, activities, timing, and assessments | Use a fixed classroom-activity schema mapped to OpenLearnSite fields |
| Workflow integration | Broad platform with integrations, but planning is still product-centered | Strongest workflow story: works inside Docs, Slides, curriculum pages, and browser tabs | Win by embedding directly in existing teacher lesson/course editing pages |
| Curriculum grounding | General customization; district alignment is part of the broader platform story | Curriculum Intelligence makes district curriculum the grounding layer | Start with standards/alignment notes, then add grounded generation later |
| Reuse and remix | Many tools, but less centered on remixing classroom spaces | Builds from trusted source materials and bundles outputs | Add reusable activity templates once v1 planning quality is stable |
| Student-safe operational framing | Strong privacy and teacher review messaging | Strong privacy and transparent drafting messaging | Keep explicit teacher approval and avoid autopublish behavior |

## Sources

This research is based primarily on official product pages and internal project
context, which supports a MEDIUM confidence rating.

- **Internal context (HIGH):**
  `/home/wuxf/Develop/openlearnsite/.planning/PROJECT.md`
- **MagicSchool official pages (MEDIUM):**
  `https://www.magicschool.ai/`
  `https://www.magicschool.ai/tools/lesson-plan`
  `https://www.magicschool.ai/integrations`
- **Brisk official pages (MEDIUM):**
  `https://www.briskteaching.com/`
  `https://www.briskteaching.com/ai-tools/lesson-plan-generator`
  `https://www.briskteaching.com/create-instructional-materials`
  `https://www.briskteaching.com/curriculum-intelligence`
- **SchoolAI official pages (MEDIUM):**
  `https://schoolai.com/`
  `https://schoolai.com/products/spaces`
  `https://schoolai.com/products/browser-extension`
- **Cross-check search results (LOW):** Google search for 2026 K-12 AI lesson
  planning assistant features and product positioning

---
*Feature research for: Teacher-facing AI lesson-planning assistants in an
existing K-12 teaching platform*
*Researched: April 10, 2026*
