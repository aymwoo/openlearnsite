# Phase 7 research — submission and completion tracking

**Date:** 2026-04-11
**Discovery level:** 0 (existing codebase patterns only)
**Question answered:** What is the safest way to complete AI activity submission
and completion tracking without introducing a new brownfield stack?

## Summary

Phase 7 can and should reuse the existing upload mission contract rather than
creating a parallel AI submission system. The current `student/uploadwork.aspx`
flow already does the core work needed by the milestone:

1. resolves the activity from `lid` to `mid`,
2. creates or updates the student's `Works` record,
3. records first completion in `MenuWorks`, and
4. updates classroom work counts through `Signin.UpdateQwork(...)`.

The gap is not that submission infrastructure is missing. The gap is that the
AI activity path has not yet been explicitly validated and, if needed, lightly
refined so the published AI mission page and completion surfaces clearly follow
the same contract.

## Relevant code facts

### Existing submission path already matches AI publish output

- Phase 5 publish already forces `Mupload=true` and `Ltype=1`, which is exactly
  the contract `student/showmission.aspx` and `student/uploadwork.aspx.cs`
  already expect.
- `student/uploadwork.aspx.cs` loads `ListMenu` by `lid`, resolves the linked
  mission id, creates or updates a `Works` row, and writes a `MenuWorks` row on
  first successful submission.
- The same handler updates `Signin.UpdateQwork(...)`, which means the current
  classroom participation accounting is already wired into upload success.

### Existing completion model is already menu-based

- `MenuWorks` stores `Klid`, `Ksid`, `Ktime`, `Kseconds`, and `Kcheck`, which is
  enough to indicate the student has completed the activity at least once.
- `student/Scm.master.cs` already uses completion-related queries and work-pass
  behavior to show finish icons in the activity menu.
- `student/myinfo.aspx.cs` already renders completion-like summaries from the
  current classroom models.

### Risks remain in explicit validation and edge cases

- The first-submit path writes `MenuWorks`, but resubmission behavior needs to
  remain consistent and not create duplicate completion rows.
- The mission page must clearly reflect whether submission is available,
  completed, or blocked after evaluation.
- Because upload is browser and filesystem dependent, final verification still
  needs manual UAT even if source tests are added.

## Recommended implementation

### 1. Pin the AI mission submission contract with tests

Add source tests that assert the AI publish flow continues to force the same
mission shape consumed by `showmission` and `uploadwork`:

- `Mupload=true`
- `Ltype=1`
- submission resolves by `lid` -> `ListMenu` -> `Mission`

### 2. Extract or tighten shared completion writes if needed

If the AI mission path exposes duplicated or fragile completion behavior,
extract the smallest shared helper for:

- creating the first `MenuWorks` record,
- preserving one record per `Klid` + `Ksid`, and
- updating the existing classroom work counters.

If the current handler already does this reliably, prefer tests and minimal
clarifying guards over a refactor.

### 3. Make student completion state explicit on the mission page

Use the existing `showmission.aspx.cs` state checks to ensure the page clearly
communicates:

- not yet submitted,
- submitted and can revise,
- submitted and locked because the teacher already evaluated it.

That closes the user-facing side of `SCT-01` even if the upload handler itself
does not change much.

## Concrete brownfield choices

- **Submission route:** keep `student/uploadwork.aspx` / `student/uploadworkm.aspx`
- **Completion model:** keep `MenuWorks`
- **Work artifact model:** keep `Works`
- **Teacher/classroom counters:** keep `Signin.UpdateQwork(...)`
- **Verification stack:** source tests plus manual authenticated upload UAT

## Risks and mitigations

| Risk | Why it matters | Mitigation |
|------|----------------|------------|
| Duplicate completion writes | Repeated submissions could spam `MenuWorks` | Reuse existing `GetModelme`/exists checks or extract one helper that enforces one completion row per student and menu item |
| AI path drifts from normal upload behavior | Students could open the activity but fail to submit reliably | Keep the AI mission on the exact existing `showmission` + `uploadwork` contract |
| Completion is recorded but not visible | Milestone passes technically but users cannot tell | Tighten mission-page state messages and verify menu finish behavior |
| Upload path is insufficiently tested | Filesystem and browser dependencies can hide regressions | Use source tests for contract coverage and manual UAT for real uploads |

## Testing strategy

- Extend `Tests/TeacherRegressionTests/TeacherRegressionTests.cs` for:
  - `showmission` upload shell presence,
  - `uploadwork.aspx.cs` contract assertions,
  - completion-related mission-page state messaging.
- Extend `Tests/CommonLogicTests/CommonLogicTests.cs` for any shared helper that
  centralizes AI mission completion writes.
- Run manual browser UAT that publishes an AI activity, opens it as a student,
  uploads a valid file, and confirms completion status changes.

## Validation architecture

Phase 7 is primarily brownfield reuse validation. Existing code already contains
the required tables and upload flow, so the main work is to make that contract
explicit, tighten any edge behavior, and verify it with a combination of source
tests and manual authenticated upload checks.

**Quick run:**

`dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj --filter "FullyQualifiedName~UploadWork|FullyQualifiedName~ShowMission|FullyQualifiedName~ActivityPlan"`

**Full run:**

`dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj && dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj`

## Planning implications

- Start by pinning the existing upload contract before adding any shared helper.
- Keep any completion changes small and centered on `MenuWorks`.
- Reserve final plan work for end-to-end verification across upload success and
  completion visibility, not just handler source changes.
