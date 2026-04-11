---
status: complete
phase: 05-teacher-activity-publish-foundation
source:
  - .planning/phases/05-teacher-activity-publish-foundation/05-teacher-activity-publish-foundation-01-SUMMARY.md
  - .planning/phases/05-teacher-activity-publish-foundation/05-teacher-activity-publish-foundation-02-SUMMARY.md
  - .planning/phases/05-teacher-activity-publish-foundation/05-teacher-activity-publish-foundation-03-SUMMARY.md
started: 2026-04-11T00:00:00Z
updated: 2026-04-11T05:39:13Z
---

## Current Test
<!-- OVERWRITE each test - shows where we are -->

[testing complete]

## Tests

### 1. Dedicated AI publish controls default to unpublished
expected: In `teacher/courseedit.aspx`, the AI assistant area shows a separate AI publish toggle and publish button distinct from the legacy course publish checkbox. The AI publish toggle starts unchecked so the generated activity remains hidden from students unless the teacher explicitly enables it.
result: pass

### 2. Publish blocks when no draft exists
expected: If the teacher tries to publish before generating or loading an AI draft, the publish flow stops immediately with guidance that a draft is required, and no publish request is sent.
result: pass

### 3. Publish blocks when no sections are selected
expected: If an AI draft exists but no lesson sections are selected for publish, the publish flow stops with guidance to select sections before continuing, and the lesson content is not changed.
result: pass

### 4. Successful publish writes back committed lesson content
expected: After the teacher selects lesson sections and publishes, the browser sends one `activityPlanPublish` request and the editor body is replaced with the committed `updatedCourseContent` returned by the server instead of relying on a second browser-side save path.
result: pass

### 5. Re-publish updates the same linked activity
expected: Publishing the same course again after editing the AI draft updates the existing linked mission and menu activity for that course instead of creating duplicate student activities, while keeping teacher lesson append content and student mission content synchronized.
result: pass

## Summary

total: 5
passed: 5
issues: 0
pending: 0
skipped: 0

## Gaps

[none yet]
