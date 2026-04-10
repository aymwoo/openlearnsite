# Pitfalls Research

**Domain:** Teacher lesson-planning assistants for K-12 classroom activity
planning inside an existing teaching platform
**Researched:** April 10, 2026
**Confidence:** MEDIUM

## Critical Pitfalls

### Pitfall 1: Generating plans before locking instructional intent

**What goes wrong:**
The assistant produces polished activity plans from only a topic, but the plan
is weakly aligned to the real lesson goal, curriculum standard, lesson length,
or assessment target. Teachers get something that looks complete but does not
fit the class they are actually teaching.

**Why it happens:**
Teams optimize for low-friction generation and treat "topic in, plan out" as
the product. They under-specify the planning context and assume the model will
infer grade level, prerequisite knowledge, pacing, and standards correctly.

**How to avoid:**
- Require a lightweight but structured planning frame before generation: grade
  band, class duration, lesson objective, and optional standard.
- Make the output explicitly show what assumptions it used.
- Force the plan into editable fields for goals, steps, timing, interaction,
  resources, and assessment instead of one long paragraph.
- Add a teacher review checkpoint before saving into course/activity records.

**Warning signs:**
- Different teachers get similar generic plans for different classes.
- Time allocations do not add up to the class period.
- Activities are interesting but not assessable.
- Teachers copy only fragments because the full plan is unusable.

**Phase to address:**
Phase 1 - Planning schema and generation contract

---

### Pitfall 2: Treating AI as a substitute for pedagogical judgment

**What goes wrong:**
The system gradually pushes teachers toward accepting AI plans as finished
instructional decisions. Plans become generic, teacher voice disappears, and
the workflow weakens professional judgment instead of supporting it.

**Why it happens:**
Product teams measure success by generation speed and acceptance rate, not by
teacher editing quality or classroom usefulness. This encourages one-click
automation rather than co-planning.

**How to avoid:**
- Position the feature as draft generation, not auto-planning.
- Show prompts such as "review assumptions," "adapt for your class," and
  "check safety and inclusion" before final save.
- Track edit distance, regenerate rate, and manual overrides as healthy signals,
  not failure.
- Keep teacher-owned editing controls prominent in the existing workflow.

**Warning signs:**
- Teachers save plans with little or no review.
- Support tickets describe plans as "robotic" or "not my class."
- Product metrics reward fewer edits.
- Teachers stop adding local examples, constraints, or classroom norms.

**Phase to address:**
Phase 1 - Product guardrails and teacher-in-the-loop UX

---

### Pitfall 3: Weak curriculum and assessment alignment

**What goes wrong:**
The assistant suggests engaging classroom activities that are poorly aligned to
 standards, prerequisite knowledge, or how the teacher will assess learning.
The result is extra prep work, not time savings.

**Why it happens:**
General-purpose generation is easier than building alignment checks. Teams ship
activity ideas first and defer standards mapping, objective checks, and exit
criteria.

**How to avoid:**
- Ask for or infer curriculum context only from approved school data, not from
  freeform guesswork.
- Add a required output section: objective, evidence of learning, and assessment
  method.
- Validate each generated step against the declared objective and available
  lesson time.
- Add teacher controls to select "introduce," "practice," or "assess" mode so
  the plan structure changes appropriately.

**Warning signs:**
- Generated assessments do not measure the stated goal.
- Plans mix too many objectives into one lesson.
- Teachers frequently rewrite the assessment section from scratch.
- School leaders question standards coverage despite high usage.

**Phase to address:**
Phase 1 - Objective, standards, and assessment scaffolding

---

### Pitfall 4: Ignoring differentiation and classroom reality

**What goes wrong:**
Plans assume a homogeneous class and average pacing. They fail for multilingual
learners, students needing more scaffolding, mixed-ability groups, or classes
with limited materials and large headcounts.

**Why it happens:**
Teams optimize the base plan only. Differentiation is treated as a future
enhancement instead of a core teaching requirement.

**How to avoid:**
- Include optional constraints for class size, resource limits, learner support
  needs, and grouping style.
- Generate at least one scaffold and one extension by default.
- Add resource-light alternatives so plans still work when classrooms lack
  devices, printouts, or specialty materials.
- Pilot with real teachers across grade bands and school contexts before broad
  rollout.

**Warning signs:**
- Plans assume one device per student or unlimited materials.
- Teachers report that only "ideal classroom" scenarios work.
- The same plan is reused across grades with minor wording changes.
- Differentiation notes are superficial, such as "support struggling students."

**Phase to address:**
Phase 2 - Differentiation and real-classroom adaptation

---

### Pitfall 5: Leaking student or sensitive classroom data into prompts

**What goes wrong:**
Teachers paste student names, performance history, IEP-like details, behavior
notes, or unpublished curriculum content into prompts. That creates privacy,
contract, and trust risk.

**Why it happens:**
The easiest way to improve outputs is to add context, and teams do not put
input guardrails around existing AI entry points. In brownfield systems,
approved provider routing is often mistaken for full privacy compliance.

**How to avoid:**
- Default to data minimization: topic, objective, class traits, and approved
  non-PII metadata only.
- Add inline warnings and server-side filtering for names, IDs, health data,
  special education details, and other sensitive records.
- Use only district-approved providers under appropriate contractual control.
- Log prompt templates and metadata, not raw sensitive teacher inputs, unless
  governance explicitly permits and protects it.

**Warning signs:**
- Teachers paste class rosters or student work into planning prompts.
- Freeform prompt boxes encourage "tell us everything about your class."
- Provider terms or retention settings are unclear.
- Privacy review happens after implementation, not before.

**Phase to address:**
Phase 0 - Governance and provider/data controls before pilot

---

### Pitfall 6: Shipping hallucinated or unsafe activity suggestions

**What goes wrong:**
The assistant invents facts, suggests resources that do not exist, proposes
unsafe experiments, or recommends activities that violate school policy,
developmental appropriateness, or classroom management constraints.

**Why it happens:**
Teams trust fluent output too much and do not separate "creative suggestion"
from "instructionally safe and verifiable plan." Safety review is often limited
to moderation for toxic language, which is not enough for classroom planning.

**How to avoid:**
- Constrain output to a fixed planning schema rather than open prose.
- Add content checks for age appropriateness, time realism, materials safety,
  and unsupported factual claims.
- Prefer retrieval from approved local exemplars, curriculum references, or
  vetted templates when generating structured plans.
- Flag resource suggestions and facts as teacher-verified items when confidence
  is low.

**Warning signs:**
- Citations or resources cannot be verified.
- Science or physical activities omit safety notes.
- Time estimates are wildly unrealistic.
- Teachers report "sounds good, fails in class."

**Phase to address:**
Phase 1 - Safety validation and constrained generation

---

### Pitfall 7: Embedding bias, stereotypes, or culturally narrow examples

**What goes wrong:**
Generated examples, scenarios, group roles, or assessment prompts reinforce
stereotypes, privilege one cultural perspective, or miss the needs of diverse
learners and families.

**Why it happens:**
Foundation models reflect training-data bias, and teams rarely test planning
outputs for classroom inclusiveness. Bias review is often handled as a generic
AI concern rather than a lesson-design concern.

**How to avoid:**
- Test outputs across subjects, grade bands, and diverse learner contexts.
- Add review rubrics for representation, language accessibility, and cultural
  relevance.
- Provide teachers with easy "regenerate with more inclusive examples" controls.
- Keep a feedback loop so biased outputs become prompt, rule, or template fixes.

**Warning signs:**
- Repeated gendered or culturally narrow role examples.
- Examples assume one family structure or one language norm.
- Differentiation language frames students as deficits.
- Teachers manually sanitize examples before use.

**Phase to address:**
Phase 2 - Inclusiveness evaluation and output QA

---

### Pitfall 8: Bolting the feature onto workflow without adoption design

**What goes wrong:**
The assistant exists inside the page, but it does not match how teachers really
plan. It creates copy-paste work, duplicates existing activity fields, or
forces teachers to leave their normal edit sequence.

**Why it happens:**
In brownfield platforms, teams add an AI panel rather than redesigning the
teacher flow around the smallest high-value job to be done.

**How to avoid:**
- Start from the current teacher planning sequence on the existing page.
- Write generated content directly into existing editable fields with preview and
  selective insert controls.
- Support partial generation and regeneration for one section at a time.
- Pilot with observed teacher prep sessions, not only internal demos.

**Warning signs:**
- Teachers copy AI output manually into form fields.
- The assistant creates a second source of truth outside the saved lesson.
- Usage drops after first trial despite positive demo reactions.
- Teachers ask for section-level generation, but the product only supports full
  plan generation.

**Phase to address:**
Phase 1 - Brownfield workflow integration

---

### Pitfall 9: No feedback loop from classroom use back into quality controls

**What goes wrong:**
The team cannot tell which plans were useful, which failed in class, or which
parts teachers consistently rewrite. Quality stagnates because production data
is limited to "generate clicked."

**Why it happens:**
Roadmaps focus on initial generation, not the learning loop after use. Existing
platform analytics usually capture page activity, not pedagogical usefulness.

**How to avoid:**
- Capture structured teacher feedback after save or after classroom use.
- Track section-level edits, deletion rates, and regeneration causes.
- Build a review queue for low-rated or heavily edited outputs.
- Use feedback to improve prompt scaffolds, templates, and safety rules.

**Warning signs:**
- Success is reported only as request volume.
- The same teacher complaints recur across releases.
- There is no taxonomy for failure modes.
- Teams cannot explain why some subjects or grades underperform.

**Phase to address:**
Phase 2 - Instrumentation and quality improvement loop

---

### Pitfall 10: Expanding scope before the narrow planning job works

**What goes wrong:**
The product expands into full lesson authoring, auto-publishing, grading,
student-side coaching, or cross-workflow orchestration before activity planning
quality is reliable. The team accumulates broad AI debt on top of a weak core.

**Why it happens:**
AI features demo well, so roadmap pressure pushes breadth over teaching value.
Existing AI infrastructure makes adjacent expansion look cheaper than it is.

**How to avoid:**
- Keep v1 success criteria narrow: can a teacher turn a topic into a usable,
  editable classroom activity plan inside the current workflow?
- Gate expansion on proven plan quality, adoption, and privacy/safety controls.
- Defer student-facing or autonomous actions until teacher-reviewed planning is
  stable.

**Warning signs:**
- Roadmap adds publishing or grading before plan quality metrics stabilize.
- Teams discuss "AI copilot everywhere" without evidence from planning usage.
- QA burden expands faster than teacher value.
- Pilot teachers ask for reliability fixes while roadmap adds surface area.

**Phase to address:**
Phase 0/1 - Scope control during roadmap definition and MVP delivery

## Technical Debt Patterns

Shortcuts that seem reasonable but create long-term problems.

| Shortcut | Immediate Benefit | Long-term Cost | When Acceptable |
|----------|-------------------|----------------|-----------------|
| Store the whole AI plan as one blob field | Fastest MVP | Hard to edit, diff, validate, or reuse sections | Only for a throwaway prototype, not production |
| Reuse a generic freeform prompt box | Low engineering effort | Encourages privacy leaks and low-quality prompts | Never for teacher-facing production |
| Add AI output outside existing lesson fields | Minimal integration work | Creates dual sources of truth and copy-paste workflow | Only in internal evaluation |
| Skip section-level regeneration | Simpler UI | Teachers must rerun and re-edit the whole plan | Acceptable only in very early pilot |
| Measure only generation count | Easy dashboard | Hides poor classroom usefulness | Never as the primary success metric |

## Integration Gotchas

Common mistakes when connecting to external services.

| Integration | Common Mistake | Correct Approach |
|-------------|----------------|------------------|
| Existing AI provider layer | Assuming existing provider routing automatically solves privacy and pedagogy | Add planning-specific prompt controls, redaction, and output validation on top of provider routing |
| Existing lesson/course edit pages | Injecting a side panel that does not write into native fields | Bind generation to the existing teacher authoring model and editable form fields |
| Skill/template storage | Hard-coding prompts in page logic | Version prompts/templates server-side so failures can be traced and improved |
| Logging/analytics | Logging raw prompts and outputs indiscriminately | Log structured metadata and sanitized failure signals with privacy review |
| School curriculum context | Letting teachers rely on generic public-model knowledge | Use approved local context, explicit teacher selections, or vetted templates |

## Performance Traps

Patterns that work at small scale but fail as usage grows.

| Trap | Symptoms | Prevention | When It Breaks |
|------|----------|------------|----------------|
| Full-plan regeneration for every small edit | Slow teacher workflow, high token cost | Support section-level generation and save drafts | Breaks during pilot if teachers iterate heavily |
| Synchronous generation on page save | Teachers wait or lose work on timeout | Separate draft save from generation and use recoverable async status | Breaks once multiple teachers generate during peak prep times |
| No prompt/output caching for common templates | Duplicate cost and latency | Cache safe reusable scaffolds, not personalized raw prompts | Breaks as adoption broadens across grade teams |
| Large unbounded prompt context | Rising cost, slower responses, unstable outputs | Enforce bounded structured inputs | Breaks when teachers paste long background text |

## Security Mistakes

Domain-specific security issues beyond general web security.

| Mistake | Risk | Prevention |
|---------|------|------------|
| Sending student-identifiable details to non-approved models | FERPA/privacy exposure and trust damage | Use provider allowlists, prompt redaction, and input warnings/blocking |
| Retaining raw prompts longer than needed | Sensitive classroom context becomes discoverable later | Minimize retention and separate audit metadata from sensitive content |
| Letting teachers upload student work for planning without policy controls | Sensitive records may be repurposed outside intended use | Restrict uploads in v1 unless contracts and review controls are in place |
| Using vendor default terms without district review | School loses control over data use and retention | Require reviewed contracts and clear data handling terms before rollout |

## UX Pitfalls

Common user experience mistakes in this domain.

| Pitfall | User Impact | Better Approach |
|---------|-------------|-----------------|
| One big "Generate lesson" button with no context controls | Output feels generic and unreliable | Ask for a few critical teaching constraints before generation |
| Showing only final prose | Teachers cannot edit or trust plan structure | Show goals, steps, timing, resources, and assessment in separate fields |
| Hiding assumptions | Teachers do not know what to fix | Surface grade, duration, and learner assumptions explicitly |
| Forcing full replacement on regenerate | Teachers lose good edits | Allow regenerate-by-section and preserve manual edits |
| Treating teacher edits as failure | Product steers toward over-automation | Treat editing as core collaborative behavior |

## "Looks Done But Isn't" Checklist

Things that appear complete but are missing critical pieces.

- [ ] **Activity-plan generation:** Often missing explicit lesson objective and
  evidence of learning — verify every plan maps steps to an assessable outcome.
- [ ] **Classroom activity flow:** Often missing realistic timing and transition
  steps — verify the plan fits an actual class period.
- [ ] **Differentiation:** Often missing concrete scaffolds and extensions —
  verify support and enrichment are actionable, not generic advice.
- [ ] **Teacher workflow integration:** Often missing selective insert/edit flow
  — verify teachers do not need copy-paste to use the output.
- [ ] **Privacy controls:** Often missing prompt redaction and provider
  constraints — verify teachers cannot easily paste sensitive student data.
- [ ] **Quality measurement:** Often missing post-generation feedback signals —
  verify the team can learn from edited and rejected plans.

## Recovery Strategies

When pitfalls occur despite prevention, how to recover.

| Pitfall | Recovery Cost | Recovery Steps |
|---------|---------------|----------------|
| Weak instructional alignment | MEDIUM | Add required objective/assessment fields, retrain prompts, re-run pilot with teacher rubric review |
| Privacy leakage through prompts | HIGH | Stop affected workflow, audit logs, purge where possible, notify stakeholders per policy, tighten input controls before re-enable |
| Unsafe or hallucinated activity suggestions | HIGH | Disable affected template/skill, add rule checks, review recent outputs, require teacher re-verification |
| Low teacher adoption due to workflow mismatch | MEDIUM | Observe real planning sessions, redesign insertion flow, add section-level generation, repilot |
| Biased examples in output | MEDIUM | Add targeted regression tests, update prompts/templates, review flagged subjects/grades, monitor teacher reports |

## Pitfall-to-Phase Mapping

How roadmap phases should address these pitfalls.

| Pitfall | Prevention Phase | Verification |
|---------|------------------|--------------|
| Generating plans before locking instructional intent | Phase 1 | Plans always show objective, duration, and assumptions before save |
| Treating AI as a substitute for pedagogical judgment | Phase 1 | Teachers can review, edit, and selectively insert every section |
| Weak curriculum and assessment alignment | Phase 1 | Teacher rubric review shows plans map activities to objectives and assessment |
| Ignoring differentiation and classroom reality | Phase 2 | Pilot teachers across contexts report plans are adaptable with minimal rewrite |
| Leaking student or sensitive classroom data into prompts | Phase 0 | Privacy review, provider approval, and prompt redaction controls are in place before pilot |
| Shipping hallucinated or unsafe activity suggestions | Phase 1 | Safety/validity checks catch unverifiable or unsafe outputs before teacher use |
| Embedding bias or stereotypes | Phase 2 | Diverse output review set passes inclusive-content checks |
| Bolting the feature onto workflow without adoption design | Phase 1 | Teachers can generate and save plans without copy-paste or duplicate editing |
| No feedback loop from classroom use | Phase 2 | Dashboard includes edit, regenerate, reject, and teacher-rating signals |
| Expanding scope before the narrow planning job works | Phase 0/1 | MVP exit criteria are met before roadmap expands to adjacent AI workflows |

## Sources

- California Department of Education, "Guidance for the Safe and Effective Use
  of Artificial Intelligence in California Public Schools" and AI overview page
  (official, 2025 guidance page accessed April 10, 2026) - HIGH confidence
  https://www.cde.ca.gov/ci/pl/aiincalifornia.asp
- UNESCO, "Guidance for generative AI in education and research" (official,
  published September 7, 2023; updated January 16, 2026) - HIGH confidence
  https://www.unesco.org/en/articles/guidance-generative-ai-education-and-research
- U.S. Department of Education, Office of Educational Technology, "Artificial
  Intelligence and the Future of Teaching and Learning: Insights and
  Recommendations" (official report identified via ed.gov/tech.ed.gov search
  results) - MEDIUM confidence
  https://www2.ed.gov/documents/ai-report/ai-report.pdf
- U.S. Department of Education privacy guidance on online educational services
  and model terms of service, used as privacy/governance context for vendor and
  prompt-handling pitfalls (official resource identified via ed.gov search
  results) - MEDIUM confidence
  https://studentprivacy.ed.gov/
- Project context from OpenLearnSite `.planning/PROJECT.md` (first-party
  product context) - HIGH confidence

---
*Pitfalls research for: Teacher lesson-planning assistants in an existing K-12 teaching platform*
*Researched: April 10, 2026*
