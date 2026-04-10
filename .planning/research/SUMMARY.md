# Project Research Summary

**Project:** OpenLearnSite teaching skills enhancement
**Domain:** AI-assisted teacher lesson-planning inside an existing ASP.NET Web
Forms authoring system
**Researched:** April 10, 2026
**Confidence:** MEDIUM

## Executive Summary

This is not a greenfield AI copilot product. It is a brownfield teaching-skill
upgrade inside OpenLearnSite's existing teacher editing workflow. Across the
research, the strongest pattern is consistent: experts embed lesson planning in
the page teachers already use, collect a small amount of structured context,
generate a typed lesson-plan draft, validate it on the server, and require the
teacher to review and selectively apply it before saving. The winning product
shape is an in-workflow drafting assistant, not a generic chatbot and not a
standalone planning app.

The recommended approach is to add a dedicated lesson-planning service on top of
the current provider and custom-skill infrastructure, keep prompt pedagogy
configurable, keep the output contract in code, store AI output as a structured
draft, and map approved sections back into existing lesson fields. Build the
MVP around one narrow job: topic or knowledge point in, editable classroom
activity plan out. That MVP must include section-level review and regeneration,
basic alignment cues, fallback behavior, and teacher-in-the-loop controls.

The main risks are not model access. They are weak instructional alignment,
privacy leakage, unsafe or low-trust suggestions, and workflow mismatch inside
legacy pages. Mitigate them early with Phase 0 governance, Phase 1 schema and
validation, draft-first architecture, explicit teacher review, bounded context
inputs, and telemetry that measures edits, regeneration, and acceptance quality
instead of raw generation volume.

## Key Findings

### Recommended stack

The stack research points to a conservative architecture choice: stay in the
existing ASP.NET Web Forms monolith and extend the current AI provider layer
instead of introducing a new AI framework or separate app. The right technical
move is a bounded `AILessonPlanner`-style service that assembles context, calls
an OpenAI-compatible provider using structured outputs, validates the returned
JSON, and renders a teacher-editable preview. Keep `Newtonsoft.Json` in the
legacy app, add schema validation with `NJsonSchema`, and add resilience and
observability only where they reduce concrete delivery risk.

**Core technologies:**
- **ASP.NET Web Forms (.NET Framework 4.8):** host the feature in the current
  teacher editing flow — lowest-risk brownfield path.
- **Dedicated planner service in `App_Code/Bll`:** orchestrate prompt assembly,
  provider calls, validation, fallback, and shaping — matches existing gauge
  and exam patterns.
- **OpenAI-compatible structured-output provider path:** return typed JSON, not
  prose — makes preview and insertion deterministic.
- **`Newtonsoft.Json` 13.0.3:** serialize legacy web-layer payloads — already
  standard in the repo.
- **`NJsonSchema` 11.5.2:** validate the `TeachingActivityPlan` contract on the
  server — practical fit for a Newtonsoft-heavy .NET Framework app.

Critical version guidance is narrow but important: keep the Web Forms runtime on
`.NET Framework 4.8`, use `NJsonSchema` `11.5.2` for contract validation, and
only introduce `Polly` `8.6.6`, `HtmlSanitizer` `9.0.892`, or OpenTelemetry
`1.15.1` where the integration actually needs resilience, sanitization, or
traceability.

### Expected features

Feature research is clear that the MVP must feel like a teaching tool, not a
text generator. Teachers now expect a structured plan with goals, activity flow,
timing, resources, and assessment, plus lightweight context controls and the
ability to keep good sections while regenerating weak ones. Competitive upside
comes later from differentiated variants, artifact generation, and curriculum
grounding, but those only matter after the base structured-plan loop is trusted.

**Must have (table stakes):**
- **Structured activity plan generation:** turn a topic into goals, steps,
  timing, interaction mode, resources, and assessment.
- **In-workflow integration:** run inside the existing lesson or course editor.
- **Teacher context controls:** support topic plus optional grade, subject,
  duration, objective, and class context.
- **Section-level edit and regenerate:** let teachers keep strong sections and
  repair weak ones.
- **Human review before save:** require review, edit, and selective insert.

**Should have (competitive):**
- **Basic standards or alignment notes:** enough to show the plan is not generic.
- **Differentiated variants:** support, on-level, and extension paths built from
  the structured plan.
- **Artifact generation from approved plans:** exit tickets, rubrics,
  worksheets, or slides.
- **Reusable templates and remixing:** discussion, experiment, review game, and
  similar proven activity shapes.

**Defer (v2+):**
- **Curriculum-grounded generation from district materials:** valuable, but it
  needs ingestion, governance, and retrieval design first.
- **Data-informed suggestions from student performance:** high value, but only
  after data quality and permissions are reliable.
- **Cross-lesson or unit sequencing:** defer until single-lesson quality is
  proven.

### Architecture approach

Architecture research strongly supports a draft-first embedded workflow. The
teacher editor remains the source of truth. A planning panel collects topic and
options, a dedicated handler calls a planner service, the response is validated
against a structured schema, the plan is saved as a draft, and the teacher then
accepts all or selected sections into existing lesson fields. Reuse the current
AI provider and scoped-skill infrastructure; do not bypass it with ad hoc page
logic.

**Major components:**
1. **Editor host and AI plan panel** — collect context, show generation status,
   preview structured sections, and drive selective apply.
2. **Planning handler and orchestration service** — validate requests, build the
   context sandwich, call the provider, normalize fallback output, and persist
   drafts.
3. **Draft repository and apply mapper** — store structured plans before publish
   and map approved sections into `Mission`, `Courses`, or related lesson data.
4. **Skill template and schema layer** — keep pedagogical prompts configurable
   while keeping the data contract fixed in code.
5. **Audit and feedback loop** — track provider, prompt version, fallback,
   acceptance, edits, and failures for later quality tuning.

Key patterns to preserve are draft-first generation, structured-output contracts
instead of prose blobs, and context-sandwich orchestration that combines fixed
pedagogy, editor context, and teacher intent.

### Critical pitfalls

The pitfall research makes the roadmap constraints unusually clear: success will
fail faster on pedagogy, trust, privacy, and workflow than on infrastructure.

1. **Generating plans before locking instructional intent** — require a minimal
   planning frame, show assumptions, and validate timing/objective fit.
2. **Treating AI as a substitute for pedagogical judgment** — keep teacher-owned
   editing, selective insert, and review prompts prominent.
3. **Weak curriculum and assessment alignment** — require objective, evidence of
   learning, and assessment sections in the schema.
4. **Leaking student or sensitive classroom data into prompts** — add provider
   controls, redaction, bounded inputs, and privacy review before pilot.
5. **Bolting the feature onto workflow without adoption design** — bind output to
   native lesson fields and avoid copy-paste or dual sources of truth.

## Implications for Roadmap

Based on the combined research, the roadmap should be phase-gated around trust,
structure, and integration, in that order.

### Phase 0: Governance and MVP guardrails
**Rationale:** Privacy, provider approval, and scope control must exist before
teacher pilot usage. The research is explicit that existing provider routing is
not enough on its own.
**Delivers:** Provider allowlist review, prompt redaction rules, logging policy,
success metrics, and a hard MVP boundary around single-lesson planning.
**Addresses:** Human review, in-workflow use, narrow v1 scope.
**Avoids:** Sensitive-data leakage, autopublish creep, and misleading success
metrics.

### Phase 1: Structured planning foundation
**Rationale:** Every later feature depends on a stable contract, draft model,
and planner service. This is the architectural dependency root.
**Delivers:** `TeachingActivityPlan` schema, draft persistence,
`AILessonPlanner`, dedicated generation endpoint, validation, fallback, and
lightweight teacher context capture.
**Addresses:** Structured plan generation, teacher context controls, basic
alignment scaffolding.
**Uses:** Web Forms host, `Newtonsoft.Json`, `NJsonSchema`, existing provider and
skill infrastructure.
**Avoids:** Free-form prose persistence, weak instructional intent, unsafe output
shape, and generic chat misuse.

### Phase 2: Embedded review and selective apply UX
**Rationale:** The product only becomes useful when the draft can be reviewed,
edited, regenerated by section, and inserted into native lesson fields without
copy-paste.
**Delivers:** AI plan panel in existing editors, preview UI, section-level
regenerate, selective apply, and mapper logic into lesson/activity records.
**Addresses:** In-workflow integration, section-level edit/regenerate, human
review before save.
**Implements:** Editor host, review panel, apply handler, and merge mapper.
**Avoids:** Workflow mismatch, dual sources of truth, and destructive overwrite.

### Phase 3: Trust, assessment linkage, and adoption telemetry
**Rationale:** After the core loop works, the next leverage comes from proving
quality and connecting planning output to adjacent teacher workflows.
**Delivers:** Assessment/rubric handoff, prompt/version audit logging, feedback
signals, apply/edit/regenerate analytics, and quality review queues.
**Addresses:** Resource and assessment suggestions, measurable classroom fit, and
continuous tuning.
**Uses:** Existing gauge flow, audit metadata, and optional observability.
**Avoids:** Flying blind on quality, hallucinated assessments, and roadmap drift
toward breadth before reliability.

### Phase 4: Post-validation enhancements
**Rationale:** Differentiation, artifact generation, and curriculum grounding are
valuable only after the structured-planning core is trusted and measurable.
**Delivers:** Differentiated variants, artifact bundle generation, reusable
activity templates, and later curriculum-grounded planning.
**Addresses:** Competitive differentiation and stronger teacher time savings.
**Avoids:** Premature complexity, data-governance debt, and overbuilding before
adoption proof.

### Phase ordering rationale

- Start with governance because privacy and scope failures can block the pilot
  before quality tuning matters.
- Build schema, draft storage, and orchestration before UI work because the UI
  depends on a stable contract and fallback behavior.
- Add selective apply before advanced generation because teacher trust depends on
  preserving edits and keeping the editor as source of truth.
- Delay differentiation, artifact generation, and curriculum grounding until the
  narrow planning job shows strong apply and reuse signals.

### Research flags

Phases likely needing deeper research during planning:
- **Phase 0:** Privacy, retention, and approved-provider policy need local legal
  and institutional validation.
- **Phase 3:** Assessment linkage may need targeted research on how the current
  gauge workflow can consume lesson-plan output cleanly.
- **Phase 4:** Curriculum grounding and data-informed suggestions need separate
  retrieval, governance, and permissions research before implementation.

Phases with standard patterns (skip research-phase):
- **Phase 1:** Structured schema, draft-first generation, server validation, and
  bounded provider orchestration are already well-supported by research.
- **Phase 2:** Embedded preview, selective apply, and teacher-in-the-loop review
  follow established patterns and the repo's current AI workflow direction.

## Confidence Assessment

| Area | Confidence | Notes |
|------|------------|-------|
| Stack | HIGH | Strongly grounded in local code patterns plus official .NET and provider guidance. |
| Features | MEDIUM | Clear market pattern, but mostly based on product pages rather than deep implementation docs. |
| Architecture | MEDIUM | Pattern is consistent across LMS examples and local code, but some source pages were verified indirectly. |
| Pitfalls | HIGH | Supported by first-party project constraints plus official education and privacy guidance. |

**Overall confidence:** MEDIUM

### Gaps to address

- **Local data governance rules:** Confirm what prompt metadata, raw prompts, and
  outputs can be retained before pilot rollout.
- **Editor-field mapping details:** Validate how plan sections should map into
  `Mission`, `Courses`, and `ListMenu` without creating awkward teacher edits.
- **Section-level regeneration UX:** Decide whether regeneration is server-only,
  diff-aware, or draft-versioned before implementation planning.
- **Standards alignment depth for v1:** Choose whether alignment is a note,
  tagged field, or validated constraint in the first release.
- **Assessment handoff shape:** Define the minimal contract between lesson-plan
  output and existing rubric/gauge generation.

## Sources

### Primary (HIGH confidence)
- OpenLearnSite local project context: `.planning/PROJECT.md`.
- OpenLearnSite local implementation references:
  `teacher/aiprovider_api.ashx`, `App_Code/Bll/AIGaugeGenerator.cs`,
  `App_Code/Bll/AIStudentExamGenerator.cs`, and
  `App_Code/Model/AICustomSkill.cs`.
- Microsoft documentation on serializer migration and .NET observability.
- California Department of Education AI guidance.
- UNESCO guidance for generative AI in education and research.

### Secondary (MEDIUM confidence)
- OpenAI structured outputs guidance and Anthropic structured tool/schema
  patterns.
- Official product and platform material from MagicSchool, Brisk, SchoolAI,
  Google Classroom, Blackboard Learn Ultra, and Canvas IgniteAI.
- U.S. Department of Education AI and student privacy guidance.

### Tertiary (LOW confidence)
- Search-result-assisted verification where official pages were discoverable but
  direct fetches were incomplete or blocked.
- Competitor positioning inferences drawn from public marketing pages rather
  than technical implementation detail.

---
*Research completed: April 10, 2026*
*Ready for roadmap: yes*
