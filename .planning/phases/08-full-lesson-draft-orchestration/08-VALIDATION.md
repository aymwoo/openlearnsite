---
phase: 8
slug: full-lesson-draft-orchestration
status: draft
nyquist_compliant: true
wave_0_complete: false
created: 2026-04-11
---

# Phase 8 — Validation Strategy

> Per-phase validation contract for feedback sampling during execution.

---

## Test Infrastructure

| Property | Value |
|----------|-------|
| **Framework** | xUnit (.NET test slices) |
| **Config file** | `Tests/CommonLogicTests/CommonLogicTests.csproj`, `Tests/TeacherRegressionTests/TeacherRegressionTests.csproj` |
| **Quick run command** | `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj --filter FullLesson` |
| **Full suite command** | `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj && dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj` |
| **Estimated runtime** | ~45 seconds |

---

## Sampling Rate

- **After every task commit:** Run `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj --filter FullLesson`
- **After every plan wave:** Run `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj && dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj`
- **Before `/gsd-verify-work`:** Full suite must be green
- **Max feedback latency:** 45 seconds

---

## Per-Task Verification Map

| Task ID | Plan | Wave | Requirement | Threat Ref | Secure Behavior | Test Type | Automated Command | File Exists | Status |
|---------|------|------|-------------|------------|-----------------|-----------|-------------------|-------------|--------|
| 8-01-01 | 01 | 1 | ORCH-01 | T-8-01 | Full-lesson draft parsing rejects malformed or partial block payloads | unit | `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj --filter FullLesson` | ✅ | ⬜ pending |
| 8-01-02 | 01 | 1 | ORCH-01, ORCH-03 | T-8-02 | Save/load/regenerate endpoints stay teacher-authorized and fail closed on invalid draft data | source-lock | `dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj --filter CourseEdit` | ✅ | ⬜ pending |
| 8-02-01 | 02 | 2 | ORCH-02 | T-8-03 | Course editor preview renders block type, teaching purpose, lesson position, and minutes from server DTO only | source-lock | `dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj --filter CourseEdit` | ✅ | ⬜ pending |
| 8-02-02 | 02 | 2 | ORCH-03 | T-8-04 | Block-level remove/regenerate preserves remaining draft state and does not overwrite lesson body automatically | source-lock | `dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj --filter CourseEdit` | ✅ | ⬜ pending |

*Status: ⬜ pending · ✅ green · ❌ red · ⚠️ flaky*

---

## Wave 0 Requirements

- [ ] `Tests/CommonLogicTests/*FullLesson*` — full-lesson helper/parser/regeneration coverage
- [ ] `Tests/TeacherRegressionTests/*CourseEdit*` additions — block preview/action markup and handler wiring checks

---

## Manual-Only Verifications

| Behavior | Requirement | Why Manual | Test Instructions |
|----------|-------------|------------|-------------------|
| Teacher reviews multi-block preview in `teacher/courseedit.aspx` | ORCH-01, ORCH-02 | Visual Web Forms rendering and interaction state are not fully covered by source-lock tests | Open a teacher course editor, generate a full-lesson draft, confirm multiple ordered cards render with type/purpose/position metadata |
| Teacher removes or regenerates one block without losing the rest | ORCH-03 | Needs end-to-end browser interaction with authenticated teacher session | Remove one block, regenerate another block, confirm untouched blocks remain visible and stable |

---

## Validation Sign-Off

- [x] All tasks have `<automated>` verify or Wave 0 dependencies
- [x] Sampling continuity: no 3 consecutive tasks without automated verify
- [x] Wave 0 covers all MISSING references
- [x] No watch-mode flags
- [x] Feedback latency < 45s
- [x] `nyquist_compliant: true` set in frontmatter

**Approval:** pending
