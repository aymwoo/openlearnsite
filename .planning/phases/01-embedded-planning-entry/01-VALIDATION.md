---
phase: 01
slug: embedded-planning-entry
status: draft
nyquist_compliant: false
wave_0_complete: true
created: 2026-04-10
---

# Phase 01 — Validation Strategy

> Per-phase validation contract for feedback sampling during execution.

---

## Test Infrastructure

| Property | Value |
|----------|-------|
| **Framework** | xUnit |
| **Config file** | `Tests/CommonLogicTests/CommonLogicTests.csproj`, `Tests/TeacherRegressionTests/TeacherRegressionTests.csproj` |
| **Quick run command** | `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj --filter "FullyQualifiedName~ActivityPlan"` |
| **Full suite command** | `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj && dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj` |
| **Estimated runtime** | ~45 seconds |

---

## Sampling Rate

- **After every task commit:** Run `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj --filter "FullyQualifiedName~ActivityPlan"` when the task changes prompt-building or handler logic, otherwise run the task-specific regression command.
- **After every plan wave:** Run `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj && dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj`
- **Before `/gsd-verify-work`:** Full suite must be green
- **Max feedback latency:** 45 seconds

---

## Per-Task Verification Map

| Task ID | Plan | Wave | Requirement | Threat Ref | Secure Behavior | Test Type | Automated Command | File Exists | Status |
|---------|------|------|-------------|------------|-----------------|-----------|-------------------|-------------|--------|
| 01-01-01 | 01 | 1 | INPUT-04, FLOW-03 | T-01-01 / T-01-02 | Server composes prompt from validated fields and existing content without exposing raw prompt editing | unit | `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj --filter "FullyQualifiedName~ActivityPlan"` | ✅ | ⬜ pending |
| 01-01-02 | 01 | 1 | INPUT-04, FLOW-03 | T-01-03 | Handler rejects empty topic and reuses authenticated provider path | unit | `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj --filter "FullyQualifiedName~ActivityPlan"` | ✅ | ⬜ pending |
| 01-02-01 | 02 | 2 | FLOW-01, INPUT-01, INPUT-02, INPUT-03 | T-01-04 | Course editor exposes dedicated assistant controls without breaking save/editor hooks | regression | `dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj --filter "FullyQualifiedName~CourseEdit"` | ✅ | ⬜ pending |
| 01-02-02 | 02 | 2 | INPUT-01, INPUT-02, INPUT-03, INPUT-04 | T-01-05 | Browser code posts structured fields and renders AI output as text, not raw HTML | regression | `dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj --filter "FullyQualifiedName~CourseEdit"` | ✅ | ⬜ pending |

*Status: ⬜ pending · ✅ green · ❌ red · ⚠️ flaky*

---

## Wave 0 Requirements

Existing infrastructure covers all phase requirements.

---

## Manual-Only Verifications

All phase behaviors have automated verification.

---

## Validation Sign-Off

- [x] All tasks have `<automated>` verify or Wave 0 dependencies
- [x] Sampling continuity: no 3 consecutive tasks without automated verify
- [x] Wave 0 covers all MISSING references
- [x] No watch-mode flags
- [x] Feedback latency < 60s
- [ ] `nyquist_compliant: true` set in frontmatter

**Approval:** pending
