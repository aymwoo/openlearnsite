# Feature Landscape

**Domain:** AI-composed full-lesson drafts and activity composition for teachers  
**Researched:** 2026-04-11  
**Scope:** v1.2 milestone only — new teacher-facing full-lesson draft and activity-composition behavior on top of already-shipped planning, publish, and student mission flows  
**Overall confidence:** MEDIUM

## Executive Summary

Current teacher-facing AI lesson tools typically work as a **composition workflow**, not a single long-form text generator. The common pattern is: teacher provides a topic/objective and light constraints, AI returns a structured lesson with objectives, timing, activities, and assessment ideas, then the teacher refines or exports the useful parts. Official product pages from Brisk, MagicSchool, and Khanmigo consistently emphasize this structure-first workflow over freeform prose.

The important milestone insight is that the next valuable step is **not** “better generated text.” It is **block-based lesson orchestration**: AI should assemble a previewable lesson draft from concrete activity types the teacher can actually run. MagicSchool explicitly exposes separate generators for lesson plans, quizzes, worksheets, and presentations; Brisk emphasizes lesson plans with objectives, activities, timing, and assessments; Khanmigo emphasizes standards-aligned lesson planning, objectives, rubrics, and exit tickets. Together, this suggests the market baseline is a teacher reviewing a structured sequence plus attached teaching artifacts.

For this project, that means table stakes are: a previewable full-lesson draft, recognizable activity blocks, teacher control over block order/editability, and a safe fallback block when existing activity types do not fit. The differentiator is not “AI can generate a lesson” — that is already common. The differentiator is **brownfield-fit orchestration**: AI composes directly into existing OpenLearnSite activity types and keeps teacher review in the center.

## How These Systems Typically Work

### Common teacher workflow

1. **Teacher gives a lightweight starting prompt**  
   Usually topic/objective, sometimes grade, class length, standards, language, or special constraints.
2. **AI creates a structured lesson skeleton**  
   Usually objectives + activity sequence + pacing + assessment/check-for-understanding.
3. **AI proposes or generates concrete teaching artifacts**  
   Typical artifacts: quiz, worksheet, presentation, text/resource study, exit ticket.
4. **Teacher reviews and tweaks block by block**  
   In strong tools, teachers do not restart from scratch every time.
5. **Teacher exports, copies, or applies selected outputs**  
   The final action is teacher-controlled, not auto-run classroom orchestration.

### What that means for this milestone

For OpenLearnSite, the new milestone should behave like an **AI lesson composer** inside the existing teacher flow:

- input = existing lesson context + teaching intent
- output = ordered draft made of executable activity blocks
- review = teacher previews, replaces, reorders, removes, or regenerates blocks
- apply = teacher explicitly accepts blocks into the existing lesson/activity model

## Table Stakes

Features users will expect for this milestone. Missing them makes the feature feel incomplete even if generation quality is good.

| Feature | Why Expected | Complexity | Notes |
|---------|--------------|------------|-------|
| Previewable full-lesson draft with ordered blocks | Market tools already return structured lesson plans rather than one long paragraph | Medium | Draft should show lesson opening, core activities, closure/check-for-understanding, not just raw text |
| AI composition over known activity types | Teachers need to see what kind of classroom activity each segment becomes | High | Must map draft blocks to existing system types first: quiz, resource-study, web courseware |
| Per-block rationale and basic metadata | Teachers need to understand why a block is there before trusting it | Medium | Include purpose, estimated time, teacher action, student action |
| Teacher block-level review controls | Teachers expect refinement without losing the whole draft | Medium | Minimum: remove, regenerate, and edit one block at a time |
| Safe fallback block for “no existing type fits” | Real lessons often need discussion/inquiry tasks that are not quiz/resource/courseware | High | This is the milestone’s new activity type: guided inquiry block |
| Apply selected blocks into the existing lesson flow | Teachers need a bridge from draft to usable lesson artifacts | High | Must fit current Web Forms edit/publish surfaces rather than create a parallel authoring product |
| Stable teacher review before publish | Official tools emphasize support, not autonomous release | Low | Keep explicit teacher approval before any student-visible change |

## Differentiators

Features that make this milestone strong rather than merely equivalent to generic AI lesson-plan tools.

| Feature | Value Proposition | Complexity | Notes |
|---------|-------------------|------------|-------|
| Compose from existing OpenLearnSite activity inventory | Turns AI output into immediately executable classroom structure instead of copy-paste text | High | Strongest practical differentiator for this brownfield system |
| Mixed-mode lesson draft (quiz + resource study + web courseware + inquiry) | Feels like a real lesson sequence, not one repeated activity template | High | Important for teacher trust and classroom realism |
| Inquiry block designed for AI generation | Covers gaps where rigid legacy activity types are too narrow | High | Should be structured enough to preview and publish, but looser than quiz/resource formats |
| Block-scoped regeneration that preserves the rest of the lesson | Matches how teachers refine plans in practice | Medium | Extends the already-validated section-stable regeneration idea into orchestration |
| Lesson-level pacing summary | Helps teachers judge if the sequence fits one class period | Medium | Sum of block durations + warning when lesson is too dense |
| Composition guided by existing lesson content | Reduces generic output and avoids disconnected generated blocks | Medium | Should reuse the course context already available in the system |

## Anti-Features

Features to explicitly avoid in this milestone.

| Anti-Feature | Why Avoid | What to Do Instead |
|--------------|-----------|-------------------|
| Single giant prose draft with no block semantics | Hard to review, impossible to map cleanly to existing activity types, low trust | Generate typed lesson blocks with metadata and preview structure |
| Autonomous student publishing after generation | Violates current teacher-review requirement and creates classroom risk | Keep explicit teacher apply/publish actions |
| Letting AI pick any new arbitrary activity schema per run | Explodes implementation scope and breaks brownfield consistency | Restrict to known activity types plus one guided inquiry fallback |
| Full visual drag-and-drop lesson builder as milestone core | Attractive, but UI-heavy and not the actual validation question | Start with ordered preview + simple block actions |
| Student-facing free-chat AI inside every generated lesson | Khanmigo’s official stance shows student AI access is a separate governance problem | Keep milestone focused on teacher-authored, teacher-reviewed activities |
| Standards/curriculum coverage engine as a prerequisite | Useful, but not required to validate lesson orchestration in this brownfield milestone | Allow optional guidance; defer deep standards mapping |
| Multi-teacher collaboration/versioning for drafts | Broad workflow expansion beyond current goal | Keep one teacher-owned draft flow first |

## Feature Categories for Milestone Scoping

### 1. Lesson Draft Composition Core

This is the irreducible milestone value.

| Capability | Complexity | Why It Matters |
|------------|------------|----------------|
| Generate a full-lesson draft from existing lesson context and teacher intent | High | Core orchestration behavior |
| Produce ordered activity blocks, not free text sections | High | Enables downstream apply/publish behavior |
| Include block metadata: title, purpose, duration, teacher action, student action | Medium | Makes review practical |

### 2. Activity-Type Mapping

This is what turns “AI plan” into “AI-composed lesson.”

| Capability | Complexity | Why It Matters |
|------------|------------|----------------|
| Map suitable blocks to quiz | High | Reuses existing executable activity type |
| Map suitable blocks to resource-study | High | Reuses existing guided learning flow |
| Map suitable blocks to web courseware | High | Reuses presentation/content delivery flow |
| Detect when none of the above fits well | Medium | Prevents forced bad mapping |

### 3. Guided Inquiry Fallback

Necessary because typical lesson tools include activities that are not naturally quiz-like.

| Capability | Complexity | Why It Matters |
|------------|------------|----------------|
| New inquiry-style block with prompt, steps, evidence/output expectation | High | Covers open exploration and discussion-driven instruction |
| Preview structure for inquiry block | Medium | Keeps teacher trust high |
| Publish/apply path consistent with existing mission model | High | Avoids special-case product fragmentation |

### 4. Teacher Review and Refinement

Without this, the feature becomes a demo instead of a usable teacher tool.

| Capability | Complexity | Why It Matters |
|------------|------------|----------------|
| Regenerate one block | Medium | Teachers refine weak segments without losing good ones |
| Remove one block | Low | Needed when AI over-plans |
| Reorder blocks | Medium | Teachers often know the right sequence better than the model |
| Edit block details before apply | Medium | Important for classroom fit and trust |

### 5. Brownfield Apply Path

The milestone succeeds only if output enters the existing system cleanly.

| Capability | Complexity | Why It Matters |
|------------|------------|----------------|
| Apply selected blocks into current course editing model | High | Avoids copy-paste dead end |
| Preserve draft/preview state until teacher confirms | Medium | Matches validated preview-first model |
| Keep publish decision separate from composition | Low | Aligns with current product decisions |

## Feature Dependencies

```text
Full-lesson draft generation
  → requires typed lesson blocks
    → requires activity-type mapping
      → requires existing-type adapters (quiz/resource-study/web courseware)
      → requires fallback inquiry block

Typed lesson blocks
  → enable block-level preview
  → enable block-level regenerate/remove/reorder
  → enable selective apply into existing lesson flow

Teacher review controls
  → should exist before broad publish/apply rollout

Inquiry fallback
  → depends on a minimal shared block contract
  → should not depend on full new student-side workflow redesign

Lesson pacing summary
  → depends on per-block duration metadata
```

## MVP Recommendation

Prioritize:

1. **Previewable typed full-lesson draft**  
   Because this is the actual new milestone promise.
2. **Composition into existing types first: quiz, resource-study, web courseware**  
   Because reuse is the highest-value brownfield advantage.
3. **Guided inquiry fallback block**  
   Because without it the composer will force bad mappings or collapse back to prose.
4. **Block-level regenerate/remove/apply**  
   Because teacher trust depends on controllable review.
5. **Lesson pacing summary**  
   Because teachers judge lesson usefulness partly by time fit.

Defer:

- **Deep standards alignment workflows** — useful, but not required to validate composition behavior.
- **Full drag-and-drop visual builder** — expensive UI work before proving orchestration value.
- **Automatic student publishing** — directly conflicts with review-first milestone constraints.
- **General-purpose new activity schema library** — too broad for first composition milestone.

## Milestone-Specific Scoping Guidance

### Must-have for v1.2

- AI returns a **full lesson sequence**, not a standalone activity paragraph
- each sequence item has a **typed activity identity** or explicit inquiry fallback
- teacher can **preview before apply**
- teacher can **drop or regenerate** weak blocks
- output can enter the **existing teacher workflow** without creating a second product surface

### Good-to-have if implementation stays contained

- reorder blocks inside preview
- lesson-level duration warnings
- “why this block was chosen” explanation text

### Should not define milestone success

- perfect pedagogy scoring
- district-standard mapping engine
- student AI copilot/chat in generated lessons
- sophisticated visual orchestration canvas

## Sources

### HIGH confidence

- Project context and milestone scope: `/home/wuxf/Develop/learnsite-wz/.planning/PROJECT.md`
- MagicSchool Lesson Plan tool: https://www.magicschool.ai/tools/lesson-plan
- MagicSchool Multiple Choice Quiz tool: https://www.magicschool.ai/tools/multiple-choice-quiz-assessment
- MagicSchool Worksheet Generator: https://www.magicschool.ai/tools/worksheet-generator
- MagicSchool Presentation Generator: https://www.magicschool.ai/tools/presentation-generator
- Brisk Lesson Plan Generator: https://www.briskteaching.com/ai-tools/lesson-plan-generator
- Khanmigo teacher-facing overview: https://www.khanacademy.org/khanmigo and https://www.khanacademy.org/khanmigo/teacher-tools

### LOW confidence / limitations

- Google Search tool was unavailable due to authentication, so broader ecosystem discovery was constrained.
- Some vendor pages are marketing/product-description pages rather than detailed product docs; conclusions are strongest where multiple vendors converge on the same workflow pattern.
