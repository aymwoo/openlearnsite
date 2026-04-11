# Pitfalls Research

**Domain:** Adding AI-generated classroom activities to an existing Web Forms
teaching platform
**Researched:** 2026-04-11
**Confidence:** HIGH

## Critical pitfalls

### Pitfall 1: Planning output is generated but not publishable

**What goes wrong:** The teacher sees a strong AI draft, but turning it into a
real student activity still requires manual re-entry.

**Why it happens:** Teams stop at content generation and never connect the
output to the platform's actual activity model.

**How to avoid:** Make teacher confirmation able to create or update the real
lesson activity and menu records, not only append prose into course content.

**Warning signs:** Demo works only in the teacher panel; no activity ID or menu
entry exists after publish.

**Phase to address:** First implementation phase.

---

### Pitfall 2: Teacher and student publish states drift apart

**What goes wrong:** A teacher thinks an activity is live, but students cannot
enter it, or students see stale content.

**Why it happens:** `Mission`, `ListMenu`, and lesson content updates happen in
separate paths without one orchestration boundary.

**How to avoid:** Publish through one server-side action that updates content,
menu visibility, and any mission metadata together.

**Warning signs:** Activity appears in course text but not in `student/Scm`
menu, or toggling visibility affects only one surface.

**Phase to address:** First implementation phase.

---

### Pitfall 3: Student activity view loses the structured AI guidance

**What goes wrong:** The AI output contains steps, prompts, or deliverable
instructions, but the student page renders only a flattened fragment.

**Why it happens:** Teams persist a rich teacher-side contract but never define
the minimum student-side render contract.

**How to avoid:** Define the minimum student-visible activity shape early:
title, task goal, steps, submission expectation, and completion signal.

**Warning signs:** Student page shows only title and generic body text, or no
clear submission requirement.

**Phase to address:** Student experience phase.

---

### Pitfall 4: Submission records exist but completion semantics are unclear

**What goes wrong:** Students can submit something, but teacher dashboards or
menus cannot reliably show who has completed the activity.

**Why it happens:** Submission persistence and completion tracking are treated as
different systems without an agreed success rule.

**How to avoid:** Decide what counts as completion for v1.1 and map it to the
existing `Works`, `MenuWorks`, and status-reporting model before coding.

**Warning signs:** Teachers can view a submission file but not a clear complete
or incomplete state.

**Phase to address:** Submission and tracking phase.

---

### Pitfall 5: Async status feedback is visible but not accessible

**What goes wrong:** Generation, publish, or submission feedback appears only as
visual text changes, so screen-reader users miss important status.

**Why it happens:** Brownfield JS enhancements are added without explicit status
semantics.

**How to avoid:** Use programmatically determinable status regions for loading,
success, and error messages.

**Warning signs:** Users need to visually watch the panel to know whether
publish or submit succeeded.

**Phase to address:** Any phase that adds async UI states.

## Technical debt patterns

| Shortcut | Immediate benefit | Long-term cost | When acceptable |
|----------|-------------------|----------------|-----------------|
| Store all generated activity structure as one opaque blob in course content | Fastest teacher-side demo | Hard to publish, edit, or render consistently for students | Only if paired with a real mission identity in the same phase |
| Bypass BLL and write ad-hoc SQL from page code | Faster first implementation | More drift and harder auth or transaction control | Never |
| Add a parallel student activity page with copied logic | Faster to prototype new UI | Permanent duplication across auth, menu, and submissions | Never for v1.1 |

## Integration gotchas

| Integration | Common mistake | Correct approach |
|-------------|----------------|------------------|
| AI provider route | Let client-side code define final publish schema | Parse and validate structured output on the server |
| `Mission` and `ListMenu` | Create mission content but forget matching visible menu state | Update or create both in one publish operation |
| Student submission tracking | Save a work artifact without linking it to lesson activity completion | Align submission identity with existing lesson activity ID model |

## Performance traps

| Trap | Symptoms | Prevention | When it breaks |
|------|----------|------------|----------------|
| Re-running full AI generation during every teacher save | Slow editor and accidental provider churn | Separate draft generation from final save and publish | Breaks immediately in normal teacher use |
| Heavy postback-only status feedback | Users see stale or flickering progress | Use light async updates with accessible status messages | Breaks under repeated generation or submit actions |

## Security mistakes

| Mistake | Risk | Prevention |
|---------|------|------------|
| Trusting client-submitted publish mapping | Unauthorized activity creation or bad record linking | Re-resolve teacher ownership and course identity server-side |
| Rendering unguarded AI HTML directly to students | XSS and broken lesson surfaces | Reuse existing encoding and content guards before persistence |

## UX pitfalls

| Pitfall | User impact | Better approach |
|---------|-------------|-----------------|
| One button silently writes both lesson content and student publish state | Teachers lose confidence and cannot predict side effects | Make lesson-fill and classroom-publish outcomes explicit in confirmation copy |
| Student activity lacks step progress or status cues | Students do not know what to do next | Render clear steps, submit expectations, and completion feedback |

## Looks done but isn't checklist

- [ ] **Teacher publish:** Verify a real activity record and student menu entry
  are created, not only teacher-side content changes.
- [ ] **Student entry:** Verify students can open the activity from the class
  menu in an authenticated session.
- [ ] **Submission:** Verify a result can be submitted through the intended page,
  not only displayed.
- [ ] **Completion:** Verify teacher-facing status can distinguish submitted or
  completed from untouched.

## Pitfall-to-phase mapping

| Pitfall | Prevention phase | Verification |
|---------|------------------|--------------|
| Planning output not publishable | Phase 1 | Teacher confirm action creates usable activity identity |
| Publish-state drift | Phase 1 | Teacher and student surfaces show matching activity availability |
| Structured guidance lost | Phase 2 | Student page displays goal, steps, and submission expectations |
| Unclear completion semantics | Phase 3 | Submission produces a reliable completed state |
| Inaccessible status feedback | Any UI phase | Loading, success, and error states are announced accessibly |

## Sources

- Repository code and migration utilities
- Microsoft Learn: ASP.NET page lifecycle notes for Web Forms behavior
- W3C WCAG 2.1 status-message guidance

---
*Pitfalls research for: AI-generated classroom activities in a brownfield LMS*
*Researched: 2026-04-11*
