---
phase: 01-embedded-planning-entry
plan: 01
subsystem: api
tags: [ai-provider, webforms, prompt-builder, xunit]
requires: []
provides:
  - topic-first activity-plan prompt composition on the server
  - scoped activity-plan custom skill bootstrap for course editing
  - dedicated activityPlan handler branch on the existing AI provider route
affects: [teacher-course-editor, phase-1-ui]
tech-stack:
  added: []
  patterns: [server-composed prompts, scoped custom skill bootstrap, bounded request shaping]
key-files:
  created:
    - App_Code/Common/AIActivityPlanPromptBuilder.cs
    - App_Code/Bll/AIActivityPlanSkillBootstrap.cs
  modified:
    - teacher/aiprovider_api.ashx
    - Tests/CommonLogicTests/CommonLogicTests.cs
    - Tests/CommonLogicTests/CommonLogicTests.csproj
key-decisions:
  - "Compose the final activity-plan prompt on the server so the browser never edits the provider-ready prompt directly."
  - "Reuse the default AI provider /chat/completions flow for activity-plan generation instead of adding a second provider client."
patterns-established:
  - "Prompt builder pattern: collect structured teacher inputs, bound lengths, and append existing editor content as supporting background."
  - "Scoped skill bootstrap pattern: ensure one active AICustomSkill exists for a feature-specific SkillScope before generation."
requirements-completed: [INPUT-04, FLOW-03]
duration: 5 min
completed: 2026-04-10
---

# Phase 1 Plan 1: Backend activity-plan contract Summary

**Server-side activity-plan prompt building with scoped skill bootstrap and default-provider request routing**

## Performance

- **Duration:** 5 min
- **Started:** 2026-04-10T11:41:59Z
- **Completed:** 2026-04-10T11:47:00Z
- **Tasks:** 2
- **Files modified:** 5

## Accomplishments
- Added a topic-first prompt builder that accepts structured planning fields and marks existing lesson content as support context.
- Added an activity-plan custom-skill bootstrap scoped to `activity_plan_courseedit`.
- Routed `activityPlan` requests through the existing authenticated AI provider handler and `/chat/completions` flow.

## Task Commits

Each task was committed atomically:

1. **Task 1: Add scoped activity-plan prompt contracts** - `49fadd6` (feat)
2. **Task 2: Route activity-plan requests through the existing AI provider handler** - `4f59622` (feat)

## Files Created/Modified
- `App_Code/Common/AIActivityPlanPromptBuilder.cs` - Builds bounded topic-first prompts from structured teacher inputs.
- `App_Code/Bll/AIActivityPlanSkillBootstrap.cs` - Ensures the default activity-plan custom skill exists for course edit scope.
- `teacher/aiprovider_api.ashx` - Adds the dedicated `activityPlan` branch and shared provider-call helper.
- `Tests/CommonLogicTests/CommonLogicTests.cs` - Covers prompt rules, scoped skill metadata, and bounded field handling.
- `Tests/CommonLogicTests/CommonLogicTests.csproj` - Links the new helper files into the xUnit project.

## Decisions Made
- Kept activity-plan requests on the existing handler so teacher auth, provider resolution, and provider compatibility stay unchanged.
- Added explicit input-length bounding in the prompt builder to mitigate oversized browser payloads before provider calls.

## Deviations from Plan

### Auto-fixed Issues

**1. [Rule 2 - Missing Critical] Added bounded field handling for teacher inputs**
- **Found during:** Task 2 (Route activity-plan requests through the existing AI provider handler)
- **Issue:** The initial prompt builder validated topic presence but did not bound optional field lengths or existing content size at the shared contract layer.
- **Fix:** Added `BoundText` plus max-length constants and used them in both prompt building and handler request shaping.
- **Files modified:** App_Code/Common/AIActivityPlanPromptBuilder.cs, teacher/aiprovider_api.ashx, Tests/CommonLogicTests/CommonLogicTests.cs
- **Verification:** `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj --filter "FullyQualifiedName~ActivityPlan"`
- **Committed in:** `4f59622`

---

**Total deviations:** 1 auto-fixed (1 missing critical)
**Impact on plan:** The extra bounds were required by the threat model and kept the plan inside intended backend scope.

## Issues Encountered
- `dotnet test` attempts to run `net48` tests in this Linux environment and aborts because `testhost.net48.exe` is missing. The `net8.0` target completed and passed the relevant ActivityPlan tests.

## User Setup Required

None - no external service configuration required.

## Next Phase Readiness
- The course editor can now post structured activity-plan requests to a dedicated backend action.
- Wave 2 can focus on embedding the assistant UI and safe browser-side rendering against the completed backend contract.

## Self-Check: PASSED

---
*Phase: 01-embedded-planning-entry*
*Completed: 2026-04-10*
