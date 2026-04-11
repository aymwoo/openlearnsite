---
phase: 5
slug: teacher-activity-publish-foundation
status: validated
nyquist_compliant: true
wave_0_complete: true
created: 2026-04-11
---

# Phase 5 — Validation Strategy

> Per-phase validation contract for feedback sampling during execution.

---

## Test Infrastructure

| Property | Value |
|----------|-------|
| **Framework** | xUnit |
| **Config file** | existing `Tests/CommonLogicTests/CommonLogicTests.csproj` and `Tests/TeacherRegressionTests/TeacherRegressionTests.csproj` |
| **Quick run command** | `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj -f net8.0 --filter "FullyQualifiedName~ActivityPlan"` |
| **Full suite command** | `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj -f net8.0 --filter "FullyQualifiedName~ActivityPlanPublishRequest|FullyQualifiedName~ActivityPlanDraftMigration|FullyQualifiedName~ActivityPlanSavedDraft|FullyQualifiedName~ActivityPlanPublishContent|FullyQualifiedName~ActivityPlanPublish" && dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj -f net8.0 --filter "FullyQualifiedName~ActivityPlanPublish|FullyQualifiedName~CourseEdit_Should"` |
| **Estimated runtime** | ~2 seconds in the current Linux `net8.0` environment |

---

## Sampling Rate

- **After every task commit:** Run `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj -f net8.0 --filter "FullyQualifiedName~ActivityPlan"`
- **After every plan wave:** Run `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj -f net8.0 --filter "FullyQualifiedName~ActivityPlanPublishRequest|FullyQualifiedName~ActivityPlanDraftMigration|FullyQualifiedName~ActivityPlanSavedDraft|FullyQualifiedName~ActivityPlanPublishContent|FullyQualifiedName~ActivityPlanPublish" && dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj -f net8.0 --filter "FullyQualifiedName~ActivityPlanPublish|FullyQualifiedName~CourseEdit_Should"`
- **Before `/gsd-verify-work`:** Full suite must be green
- **Max feedback latency:** 2 seconds

---

## Per-Task Verification Map

| Task ID | Plan | Wave | Requirement | Threat Ref | Secure Behavior | Test Type | Automated Command | File Exists | Status |
|---------|------|------|-------------|------------|-----------------|-----------|-------------------|-------------|--------|
| 05-01-01 | 01 | 1 | TAP-01 | T-05-01 | Publish request/result contracts reject missing core identifiers through typed server entry | unit | `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj -f net8.0 --filter "FullyQualifiedName~ActivityPlanPublishRequest"` | ✅ | ✅ green |
| 05-01-02 | 01 | 1 | TAP-01 | T-05-02 | Linked mission/menu ids persist on the draft row for one-course reuse | unit | `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj -f net8.0 --filter "FullyQualifiedName~ActivityPlanDraftMigration|FullyQualifiedName~ActivityPlanSavedDraft"` | ✅ | ✅ green |
| 05-02-01 | 02 | 2 | TAP-03 | T-05-03 | Selected sections append into lesson content while full draft remains available for mission content | unit | `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj -f net8.0 --filter "FullyQualifiedName~ActivityPlanPublishContent"` | ✅ | ✅ green |
| 05-02-02 | 02 | 2 | TAP-01, TAP-02, TAP-03 | T-05-04 | Publish core keeps course, mission, and menu updates in one server-owned path | unit | `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj -f net8.0 --filter "FullyQualifiedName~ActivityPlanPublish"` | ✅ | ✅ green |
| 05-03-01 | 03 | 3 | TAP-02 | T-05-05 | AI publish toggle is unchecked by default and separate from legacy mission defaults | regression | `dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj -f net8.0 --filter "FullyQualifiedName~ActivityPlanPublish|FullyQualifiedName~CourseEdit_Should"` | ✅ | ✅ green |
| 05-03-02 | 03 | 3 | TAP-03 | T-05-06 | Authenticated handler exposes one publish action that returns committed course content | regression | `dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj -f net8.0 --filter "FullyQualifiedName~ActivityPlanPublish|FullyQualifiedName~CourseEdit_Should"` | ✅ | ✅ green |

*Status: ⬜ pending · ✅ green · ❌ red · ⚠️ flaky*

---

## Wave 0 Requirements

Existing infrastructure covers all phase requirements.

---

## Manual-Only Verifications

| Behavior | Requirement | Why Manual | Test Instructions |
|----------|-------------|------------|-------------------|
| Teacher reviews the new AI publish control copy and flow clarity | TAP-02, TAP-03 | Existing automated coverage is source-based, not browser-interaction based | Open `teacher/courseedit.aspx`, generate or resume a draft, select sections, confirm publish once with toggle off and once with toggle on, and verify the success messaging matches the committed state |

---

## Validation Sign-Off

- [x] All tasks have `<automated>` verify or Wave 0 dependencies
- [x] Sampling continuity: no 3 consecutive tasks without automated verify
- [x] Wave 0 covers all MISSING references
- [x] No watch-mode flags
- [x] Feedback latency < 40s
- [x] `nyquist_compliant: true` set in frontmatter

**Approval:** validated on 2026-04-11 via targeted `net8.0` CommonLogic and TeacherRegression test runs.

---

## Validation Audit 2026-04-11

| Metric | Count |
|--------|-------|
| Gaps found | 0 |
| Resolved | 0 |
| Escalated | 0 |
