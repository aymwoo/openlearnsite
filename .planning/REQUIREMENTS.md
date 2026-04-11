# Milestone v1.1 requirements

## Overview

This document defines the scoped requirements for milestone v1.1, which extends
AI lesson planning into executable classroom activities. The milestone focuses
on turning approved AI-generated teaching activity output into teacher-owned
lesson activities that students can enter, follow, submit, and complete through
the existing platform.

## v1.1 requirements

### Teacher activity publishing

- [ ] **TAP-01**: Teacher can turn approved AI-generated teaching activity output
  into a real lesson activity linked to the current course.
- [ ] **TAP-02**: Teacher can choose whether the generated lesson activity is
  published to students before students can enter it.
- [ ] **TAP-03**: Teacher can use one confirmed action to both add generated
  activity output into lesson content and publish a matching student-enterable
  activity entry.

### Student activity experience

- [ ] **SAE-01**: Student can enter a published AI-generated activity from the
  current class menu.
- [ ] **SAE-02**: Student can view the published activity goal, instructions,
  and task steps on the activity page.
- [ ] **SAE-03**: Student can follow step-by-step learner guidance rendered from
  the generated activity content.

### Submission and completion tracking

- [ ] **SCT-01**: Student can submit a result for a published AI-generated
  activity through the activity page.
- [ ] **SCT-02**: Student can have completion status recorded after a successful
  activity submission.

## Future requirements

- [ ] **TAP-04**: Teacher can generate multiple candidate classroom activities
  for the same teaching topic before choosing one to publish.
- [ ] **SAE-04**: Student activity guidance can adapt to different learner
  levels for the same published activity.
- [ ] **CUR-01**: Teacher can align generated classroom activities to a deeper
  curriculum or standards framework.

## Out of scope

- Fully autonomous publish after AI generation.
  Reason: milestone v1.1 keeps teacher review and explicit publish control.
- A new standalone student activity product or SPA shell.
  Reason: v1.1 must reuse the existing teacher, menu, and student activity flow.
- Real-time collaborative orchestration, classroom monitoring, or broad live
  activity management.
  Reason: the current milestone only validates entry, guidance, submission, and
  completion.
- Full analytics redesign for AI-generated activity quality.
  Reason: activity publishing and completion must be proven first.

## Traceability

| Requirement | Phase |
|-------------|-------|
| TAP-01 | |
| TAP-02 | |
| TAP-03 | |
| SAE-01 | |
| SAE-02 | |
| SAE-03 | |
| SCT-01 | |
| SCT-02 | |
