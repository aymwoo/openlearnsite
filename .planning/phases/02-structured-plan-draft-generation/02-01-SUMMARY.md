---
phase: 02-structured-plan-draft-generation
plan: 01
subsystem: api
tags: [ai-provider, webforms, structured-json, xunit]
requires:
  - phase: 01-01
    provides: topic-first request shaping and scoped activity-plan skill bootstrap
provides:
  - structured activity-plan generation orchestration on the existing handler route
  - safe parsing and normalization of provider JSON into a fixed draft contract
  - strict validation that rejects incomplete structured drafts
affects: [teacher-course-editor, phase-2-preview-ui]
tech-stack:
  added: []
  patterns: [system-plus-user prompting, scoped custom skill runtime loading, fail-closed JSON normalization]
key-files:
  created:
    - App_Code/Bll/AIActivityPlanDraftGenerator.cs
    - App_Code/Common/AIActivityPlanDraftHelper.cs
  modified:
    - App_Code/Common/AIActivityPlanPromptBuilder.cs
    - teacher/aiprovider_api.ashx
    - Tests/CommonLogicTests/CommonLogicTests.cs
key-decisions:
  - "Use the stored scoped activity-plan custom skill as the runtime system prompt, with hardcoded schema guardrails still enforced in code."
  - "Reject malformed drafts that cannot be normalized into the required section and step contract instead of falling back to plain text."
patterns-established:
  - "Structured draft generator pattern: request shaping, provider call, parser gate, and typed handler response."
  - "Safe normalization pattern: tolerate code fences and alternate field names, then enforce strict completeness checks."
requirements-completed: [PLAN-01, PLAN-02, PLAN-03, PLAN-04, FLOW-02]
duration: implemented before 2026-04-10
completed: 2026-04-10
---

# Phase 2 Plan 1: Backend structured generation contract Summary

**Structured activity-plan generation on the existing provider route with strict
JSON parsing and validation**

## Overview

This summary backfills the already-implemented backend work for Phase 2. The
repo no longer treats `activityPlan` as a free-text AI response. It now sends a
fixed schema prompt, loads the scoped activity-plan skill as the `system`
message, parses provider output into a typed draft model, and rejects any draft
that is still incomplete after safe normalization.

## Accomplishments

- Added `AIActivityPlanDraftGenerator` to orchestrate provider selection,
  skill loading, prompt building, provider calls, and structured result return.
- Added `AIActivityPlanDraftHelper` to parse fenced JSON, unwrap nested draft
  objects, normalize alternate field names, and validate the full contract.
- Updated `teacher/aiprovider_api.ashx` so `activityPlan` returns structured
  draft payloads with provider and skill metadata instead of raw content text.
- Extended `AIActivityPlanPromptBuilder` tests and parser tests in
  `Tests/CommonLogicTests/CommonLogicTests.cs`.

## Implemented contract

The backend now enforces this top-level structured draft shape:

- `teachingGoals`
- `activitySteps`
- `resources`
- `assessment`
- `teacherReminder`

Each activity step must include:

- `title`
- `minutes`
- `teacherAction`
- `studentAction`
- `interactionMethod`
- `resourceSuggestion`
- `assessmentCheck`

The handler returns `success:false` when:

- `topic` is empty
- no default provider is configured
- provider output cannot be parsed into the structured draft shape
- any required top-level section is empty
- any returned step remains incomplete after normalization

## Files created or modified

- `App_Code/Bll/AIActivityPlanDraftGenerator.cs` - New generator for scoped
  skill loading, provider calling, and structured result handling.
- `App_Code/Common/AIActivityPlanDraftHelper.cs` - New parser/normalizer and
  validation gate for activity-plan draft JSON.
- `App_Code/Common/AIActivityPlanPromptBuilder.cs` - Prompt builder now
  declares the fixed Phase 2 JSON schema and required step fields.
- `teacher/aiprovider_api.ashx` - `ActivityPlan` now calls the structured
  generator and serializes the typed `draft` response payload.
- `Tests/CommonLogicTests/CommonLogicTests.cs` - Adds ActivityPlan prompt,
  parser, normalization, and fail-closed validation coverage.

## Validation and normalization notes

The implementation matches the Phase 2 context decisions closely.

- It accepts safe recovery from Markdown code fences and nested wrapper objects.
- It maps common alternate field names like `goals`, `steps`,
  `resourceSuggestions`, `assessmentDesign`, and `teacherTip`.
- It converts plain integer minute values such as `5` into `5分钟`.
- It still fails closed when any step or required section remains incomplete.

## Tests locking the behavior

Focused xUnit coverage lives in `Tests/CommonLogicTests/CommonLogicTests.cs`.

- `ActivityPlanPromptBuilder_DeclaresStructuredDraftSchema`
- `ActivityPlanDraftHelper_ParseDraft_ParsesStructuredJsonCodeFence`
- `ActivityPlanDraftHelper_ParseDraft_NormalizesCommonAlternateFieldNames`
- `ActivityPlanDraftHelper_ParseDraft_FailsWhenAnyReturnedStepRemainsInvalid`

## Phase readiness

This backend work gives the course editor a stable structured contract that the
browser can render as read-only draft cards. It also closes the main trust gap
for Phase 2 by ensuring the UI never receives partial drafts that look valid
but are missing required teaching structure.

---

*Phase: 02-structured-plan-draft-generation*
*Backfilled from implementation: 2026-04-10*
