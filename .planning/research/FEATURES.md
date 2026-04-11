# Feature Research

**Domain:** AI-generated classroom activity authoring and student delivery
**Researched:** 2026-04-11
**Confidence:** HIGH

## Feature landscape

The repo already supports teacher-authored activities and student entry through
course menus. For this milestone, the research focus is not generic LMS scope.
It is the minimum feature set required to turn AI-generated planning output into
publishable classroom work that students can actually enter and complete.

### Table stakes (users expect these)

| Feature | Why expected | Complexity | Notes |
|---------|--------------|------------|-------|
| Teacher can turn AI output into a real lesson activity | AI planning has little value if it cannot become executable classroom work | MEDIUM | Must reuse current course and lesson authoring flow |
| Teacher can control publish state before students see it | Teachers expect review before release | LOW | Existing `Mpublish` and `ListMenu.Lshow` patterns already fit this |
| Student can enter the published activity from the class menu | Classroom use requires a clear entry point | LOW | Existing `student/Scm.master` already renders activity menu entries |
| Student can view task instructions and steps | Students need the generated content, not just a title | MEDIUM | May reuse mission content or require structured rendering |
| Student can submit a result and have completion recorded | User confirmed this as minimum scope | MEDIUM | Likely reuse `Works` plus existing completion tracking hooks |

### Differentiators (competitive advantage)

| Feature | Value proposition | Complexity | Notes |
|---------|-------------------|------------|-------|
| AI generates classroom-ready activity segments, not only lesson prose | Extends v1.0 planning into executable learning flow | MEDIUM | Core milestone value |
| Teacher can publish both lesson text and student activity from one generation flow | Reduces duplicate work between planning and classroom launch | MEDIUM | Strong brownfield fit |
| Generated activity carries step-by-step learner guidance | Makes student execution clearer and more usable in class | MEDIUM | Can ride on structured AI output already proven in v1.0 |

### Anti-features (commonly requested, often problematic)

| Feature | Why requested | Why problematic | Alternative |
|---------|---------------|-----------------|-------------|
| Fully autonomous publish after generation | Feels fast and magical | Removes teacher review and increases bad-classroom-risk | Keep explicit teacher review and publish action |
| New parallel student activity product | Seems cleaner than legacy integration | Fragments auth, menus, and submission logic | Reuse `Mission`, `ListMenu`, and student entry pages |
| Rich collaboration or live orchestration in v1.1 | Attractive classroom vision | Too broad for the next validated step | Keep to entry, guided learning, submit, completion |

## Feature dependencies

```
[AI-generated activity authoring]
    └──requires──> [Teacher review and publish control]
                         └──requires──> [Mission and menu record creation]
                                              └──requires──> [Student activity entry]
                                                                   └──requires──> [Submission and completion tracking]

[Structured step rendering] ──enhances──> [Student activity entry]

[Auto-publish after generation] ──conflicts──> [Teacher review and publish control]
```

### Dependency notes

- **AI-generated activity authoring requires teacher review and publish
  control:** generated output must remain teacher-reviewed content.
- **Teacher review and publish control requires mission and menu record
  creation:** students need a real activity identity and visible menu entry.
- **Student activity entry requires submission and completion tracking:** the
  user-defined minimum scope includes student submission and recorded status.
- **Structured step rendering enhances student activity entry:** step data is
  what makes the activity meaningfully executable instead of plain text.

## MVP definition

### Launch with (v1.1)

- [ ] Teacher can generate a classroom-ready activity segment from AI output —
  essential milestone value
- [ ] Teacher can add the generated result into lesson content and publish it as
  a student-enterable activity — essential teacher workflow bridge
- [ ] Student can enter the activity, see the generated guidance, submit work,
  and have completion recorded — essential classroom validation loop

### Add after validation (v1.x)

- [ ] Multiple generated activity options per topic — add after basic publish
  flow is proven
- [ ] Smarter curriculum alignment for each generated activity — add after the
  execution loop is stable

### Future consideration (v2+)

- [ ] Real-time collaborative student activity orchestration — defer because it
  exceeds the current classroom-execution minimum
- [ ] Full analytics dashboards for AI-generated activity quality — defer until
  activity publishing is validated in real usage

## Feature prioritization matrix

| Feature | User value | Implementation cost | Priority |
|---------|------------|---------------------|----------|
| Teacher AI-to-activity conversion | HIGH | MEDIUM | P1 |
| Publish to student menu | HIGH | MEDIUM | P1 |
| Student entry and guided activity view | HIGH | MEDIUM | P1 |
| Student submission and completion record | HIGH | MEDIUM | P1 |
| Multiple activity variants | MEDIUM | MEDIUM | P2 |
| Deep curriculum mapping | MEDIUM | HIGH | P3 |

## Sources

- Repository code: teacher course editor, student course page, student menu,
  lesson menu DAL and BLL
- Existing repo activity model around `Mission`, `ListMenu`, `Works`, and
  `MenuWorks`
- W3C status and multi-step guidance for visible progress and status messages

---
*Feature research for: AI-generated classroom activity authoring and delivery*
*Researched: 2026-04-11*
