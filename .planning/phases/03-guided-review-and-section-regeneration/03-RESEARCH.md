# Phase 3 Research: Guided review and section regeneration

**Researched:** 2026-04-10
**Phase:** 03-guided-review-and-section-regeneration
**Confidence:** High

## Summary

Phase 3 should extend the existing Phase 2 structured preview in
`teacher/courseedit.aspx` instead of introducing a new review page, storage
model, or approval system. The safest brownfield path is a narrow section-level
regeneration flow on the existing authenticated `teacher/aiprovider_api.ashx`
route: the browser sends the current structured draft plus one allowed top-level
section target, the server regenerates only that target, validates it against
the fixed contract, merges it back into the current draft, and returns the full
merged draft to the existing preview renderer.

This fits the locked decisions in `03-CONTEXT.md` because it keeps regeneration
limited to top-level sections, preserves all non-targeted sections by default,
keeps the preview non-destructive, and localizes loading and failure states to
the section being retried.

## What exists already

### Stable draft contract from Phase 2

- `App_Code/Common/AIActivityPlanDraftHelper.cs` already defines the fixed draft
  shape and rejects incomplete drafts.
- `App_Code/Bll/AIActivityPlanDraftGenerator.cs` already handles provider
  selection, scoped skill loading, prompt composition, provider calls, and
  structured draft acceptance.
- `App_Code/Common/AIActivityPlanPromptBuilder.cs` already enforces the fixed
  JSON schema and the preview-only teacher-control boundary.

### Existing review surface

- `teacher/courseedit.aspx` already hosts the full right-side preview panel used
  for teacher review.
- `js/courseedit.js` already stores the last successful structured response in
  `lastActivityPlanDraftResponse` and renders the five top-level sections as
  separate cards.
- Phase 2 human UAT confirms the preview is already working and remains
  non-destructive at runtime.

### Existing tests and patterns

- `Tests/CommonLogicTests/CommonLogicTests.cs` already covers prompt-builder and
  draft-helper behavior for the activity-plan contract.
- `Tests/TeacherRegressionTests/TeacherRegressionTests.cs` already locks the
  course editor panel markup, request wiring, and safe rendering hooks.
- The repo already favors authenticated `.ashx` handlers plus page-scoped JS for
  teacher workflows, so Phase 3 should stay on that boundary.

## Locked-decision implementation implications

### Regeneration target model

- Per D-01 and D-02, regeneration must be limited to these exact top-level
  section targets only: `teachingGoals`, `activitySteps`, `resources`,
  `assessment`, and `teacherReminder`.
- Do not add per-step regenerate buttons, whole-draft retry as the main Phase 3
  affordance, or any extra top-level sections.

### Server-owned merge flow

- Per D-03 and D-04, the server must own regeneration and merge.
- The browser should send the current full draft plus the requested section key.
- The provider prompt should ask for only the targeted section payload, not a
  brand-new full draft.
- The server should validate only the returned target section, merge it into the
  previously valid draft, and then return the merged full draft.

### Review control boundary

- Per D-05 and D-06, Phase 3 adds section-level regenerate actions only.
- Do not add explicit accept/reject/lock state, approval toggles, or saved
  section decisions.
- “Keep this section” is implicit: the teacher simply does not regenerate it.

### Local state and failure boundary

- Per D-07, D-08, and D-09, section retries need local busy and error states.
- The rest of the preview must remain visible while one section is being retried.
- On failure, the UI must keep showing the last valid full draft already on
  screen instead of clearing or partially replacing it.

## Recommended implementation shape

1. Add a dedicated section-regeneration request contract on the existing
   `activityPlan` infrastructure.
   - Add a request model that carries `topic`, optional structured inputs,
     `existingCourseContent`, `sectionTarget`, and the current draft JSON.
   - Reuse the existing scoped activity-plan skill and OpenAI-compatible provider
     path.

2. Extend the prompt-builder with a section-regeneration prompt.
   - Include the teacher topic and optional guidance again so the retry stays
     aligned with the original intent.
   - Include the current validated draft as context.
   - Instruct the provider to return a JSON object containing only the targeted
     section key.

3. Extend the draft helper with section-level parsing and merge helpers.
   - Add an allowlist for the five supported section keys.
   - Parse section-regeneration responses into a safe, validated fragment.
   - Reject responses that include the wrong key, empty content, or invalid step
     structure for `activitySteps`.
   - Merge only the validated targeted section into the current draft.

4. Extend `AIActivityPlanDraftGenerator` with a dedicated section-regeneration
   method.
   - Keep provider resolution, scoped skill loading, timeout behavior, and
     error-message shaping aligned with the existing full-draft generation path.
   - Return the merged full draft so the UI does not need to implement client
     merge logic.

5. Add a new handler action on `teacher/aiprovider_api.ashx`.
   - Keep the existing teacher-cookie auth guard.
   - Validate the allowed `sectionTarget` before any provider call.
   - Parse the client draft through the shared helper instead of trusting raw
     browser JSON.
   - Return `success:false` when the current draft is invalid, the target is not
     allowed, or the regenerated section fails validation.

6. Add section-local regenerate controls in `js/courseedit.js`.
   - Render one regenerate action per top-level section card.
   - For the steps card, use one card-level action for the whole
     `activitySteps` section, not per step.
   - Track busy/error state by section key so other cards remain unchanged.

7. Keep the render model non-destructive.
   - Do not clear the full preview on section regeneration failure.
   - Re-render only after a successful merged full draft comes back.
   - Preserve copy/export behavior against the latest valid merged draft.

## Why the merge must stay on the server

- It directly satisfies D-03 and D-04.
- It prevents the browser from becoming a second source of truth for the draft
  contract.
- It lets `AIActivityPlanDraftHelper` stay the only validation gate for both
  initial generation and later section retries.
- It avoids client-side partial-state bugs where one bad section could corrupt a
  previously valid draft.

## Brownfield risks and mitigations

| Risk | Why it matters | Mitigation |
|------|----------------|------------|
| Arbitrary section keys from the browser | Would let the client ask for unsupported or unsafe merge targets | Add a shared allowlist for exactly the five locked section keys and reject anything else before provider calls |
| Provider returns full draft or extra keys during section retry | Could overwrite preserved sections and violate D-03/D-04 | Parse only the targeted section fragment and reject responses that don't match the requested key |
| Retry failure clears the preview | Violates D-09 and loses teacher trust | Keep `lastActivityPlanDraftResponse` unchanged until a successful merged result returns |
| Global loading mask blocks the whole panel | Violates D-07/D-08 | Maintain per-section loading/error state in JS keyed by the section name |
| Client-side merge drifts from server validation rules | Creates inconsistent draft behavior between full generation and retries | Keep all merge and validation logic in `AIActivityPlanDraftHelper` on the server |

## Testing anchors

- `Tests/CommonLogicTests/CommonLogicTests.cs` should add focused coverage for:
  - allowed section-target validation,
  - section-regeneration prompt requirements,
  - parsing valid targeted section fragments,
  - rejecting wrong-key or incomplete section fragments, and
  - merging a validated section back into a current draft.
- `Tests/TeacherRegressionTests/TeacherRegressionTests.cs` should add source
  locks for:
  - section-level regenerate button markup/hooks,
  - the new `activityPlanRegenerateSection` request action,
  - local section-state hooks in `js/courseedit.js`, and
  - preview-preserving failure behavior hooks.

## Validation Architecture

The fastest reliable validation loop for this phase is:

1. `Tests/CommonLogicTests/CommonLogicTests.cs` for prompt-builder, helper,
   allowlist, section-parse, and merge logic.
2. `Tests/TeacherRegressionTests/TeacherRegressionTests.cs` for course-editor
   markup, section-regeneration action wiring, and safe local-state hooks.
3. Human verification after execution for one live browser retry flow with a
   configured provider, because runtime section regeneration still depends on an
   authenticated teacher session and live provider output.

Recommended quick commands:

- `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj --framework net8.0 --filter "FullyQualifiedName~ActivityPlan"`
- `dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj --framework net8.0 --filter "FullyQualifiedName~CourseEdit"`

## Planning implications

- Split Phase 3 into backend contract first, then course-editor UI wiring.
- Keep backend file ownership in the first plan and browser markup/JS ownership
  in the second plan.
- Do not plan apply-to-editor work, saved drafts, or explicit section approval
  state here; all are explicitly deferred.

## Recommendation

Plan Phase 3 as two execute plans:

1. Backend section-regeneration contract, validation, merge flow, and handler
   route extension.
2. Course-editor section-level regenerate controls, local retry state, and
   regression coverage.
