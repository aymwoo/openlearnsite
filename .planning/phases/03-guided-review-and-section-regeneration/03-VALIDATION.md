---
phase: 03
slug: guided-review-and-section-regeneration
status: draft
nyquist_compliant: true
wave_0_complete: true
created: 2026-04-10
---

# Phase 03 — Validation Strategy

> Per-phase validation contract for feedback sampling during execution.

---

## Test Infrastructure

| Property | Value |
|----------|-------|
| **Framework** | xUnit |
| **Config file** | `Tests/CommonLogicTests/CommonLogicTests.csproj`, `Tests/TeacherRegressionTests/TeacherRegressionTests.csproj` |
| **Quick run command** | `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj --framework net8.0 --filter "FullyQualifiedName~ActivityPlan"` |
| **Full suite command** | `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj --framework net8.0 --filter "FullyQualifiedName~ActivityPlan" && dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj --framework net8.0 --filter "FullyQualifiedName~CourseEdit"` |
| **Estimated runtime** | ~25 seconds |

---

## Sampling Rate

- **After every task commit:** Run `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj --framework net8.0 --filter "FullyQualifiedName~ActivityPlan"`
- **After every plan wave:** Run `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj --framework net8.0 --filter "FullyQualifiedName~ActivityPlan" && dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj --framework net8.0 --filter "FullyQualifiedName~CourseEdit"`
- **Before `/gsd-verify-work`:** Full suite must be green
- **Max feedback latency:** 30 seconds

---

## Per-Task Verification Map

| Task ID | Plan | Wave | Requirement | Threat Ref | Secure Behavior | Test Type | Automated Command | File Exists | Status |
|---------|------|------|-------------|------------|-----------------|-----------|-------------------|-------------|--------|
| 03-01-01 | 01 | 1 | EDIT-02 | T-03-01 / T-03-02 | Only the requested top-level section key is accepted and mergeable | unit | `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj --framework net8.0 --filter "FullyQualifiedName~ActivityPlan"` | ✅ | ⬜ pending |
| 03-01-02 | 01 | 1 | EDIT-02 | T-03-03 / T-03-04 | Handler rejects invalid current drafts and returns only validated merged drafts | unit | `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj --framework net8.0 --filter "FullyQualifiedName~ActivityPlan"` | ✅ | ⬜ pending |
| 03-02-01 | 02 | 2 | EDIT-01 | T-03-05 | Preview cards expose section-level regenerate controls without mutating editor content | regression | `dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj --framework net8.0 --filter "FullyQualifiedName~CourseEdit"` | ✅ | ⬜ pending |
| 03-02-02 | 02 | 2 | EDIT-02 | T-03-06 / T-03-07 | Only the targeted section shows busy/error state; failed retries keep the last valid full draft visible | regression | `dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj --framework net8.0 --filter "FullyQualifiedName~CourseEdit"` | ✅ | ⬜ pending |

*Status: ⬜ pending · ✅ green · ❌ red · ⚠️ flaky*

---

## Wave 0 Requirements

Existing infrastructure covers all phase requirements.

---

## Manual-Only Verifications

| Behavior | Requirement | Why Manual | Test Instructions |
|----------|-------------|------------|-------------------|
| Regenerate one section from the live teacher course editor | EDIT-01, EDIT-02 | Requires authenticated browser interaction and a configured live AI provider | Open `teacher/courseedit.aspx`, generate a draft, click one section regenerate control, confirm only that card shows local busy state and the returned preview stays preview-only |
| Confirm failed section retry preserves the previous draft on screen | EDIT-02 | Needs runtime browser observation and an induced provider or network failure | Trigger a section retry with the provider unavailable or by interrupting the request, then confirm the previous full preview remains visible and only the target section shows an error |

---

## Validation Sign-Off

- [x] All tasks have `<automated>` verify or Wave 0 dependencies
- [x] Sampling continuity: no 3 consecutive tasks without automated verify
- [x] Wave 0 covers all MISSING references
- [x] No watch-mode flags
- [x] Feedback latency < 30s
- [x] `nyquist_compliant: true` set in frontmatter

**Approval:** approved 2026-04-10
