---
phase: 06
slug: student-activity-entry-and-guided-experience
status: partial
nyquist_compliant: false
wave_0_complete: true
created: 2026-04-11
---

# Phase 06 - Validation strategy

This document reconstructs the Phase 6 validation contract from the completed
plan, summary, and UAT artifacts. It records the current automated coverage,
the added Nyquist gap test, and the remaining environment prerequisites that
still block full compliance in this workspace.

---

## Test infrastructure

Phase 6 verification uses the existing xUnit regression suites plus Playwright
for the missing authenticated browser path.

| Property | Value |
|----------|-------|
| **Framework** | xUnit (`CommonLogicTests`, `TeacherRegressionTests`) + Playwright |
| **Config file** | `Tests/CommonLogicTests/CommonLogicTests.csproj`, `Tests/TeacherRegressionTests/TeacherRegressionTests.csproj`, `playwright.config.ts` |
| **Quick run command** | `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj --framework net8.0 --filter "FullyQualifiedName~ActivityPlanMissionView|FullyQualifiedName~ShowMission" && dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj --framework net8.0 --filter "FullyQualifiedName~ActivityPlan|FullyQualifiedName~ShowMission|FullyQualifiedName~StudentEntry" && npx playwright test Tests/e2e/activity-plan-student-flow.spec.js` |
| **Full suite command** | `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj --framework net8.0 && dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj --framework net8.0 && npm run test:e2e` |
| **Estimated runtime** | ~90 seconds |

---

## Sampling rate

Phase 6 now has a concrete command chain for task and wave-level feedback.

- **After every task commit:** Run `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj --framework net8.0 --filter "FullyQualifiedName~ActivityPlanMissionView|FullyQualifiedName~ShowMission" && dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj --framework net8.0 --filter "FullyQualifiedName~ActivityPlan|FullyQualifiedName~ShowMission|FullyQualifiedName~StudentEntry"`
- **After every plan wave:** Run `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj --framework net8.0 && dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj --framework net8.0 && npx playwright test Tests/e2e/activity-plan-student-flow.spec.js`
- **Before `/gsd-verify-work`:** The targeted xUnit suites must be green, and the Playwright flow must be green in an environment with seeded teacher/student/course data.
- **Max feedback latency:** 90 seconds

---

## Per-task verification map

The table below maps each completed Phase 6 task to the requirement it covers,
the observable secure behavior, and the command currently used to verify it.

| Task ID | Plan | Wave | Requirement | Threat Ref | Secure Behavior | Test Type | Automated Command | File Exists | Status |
|---------|------|------|-------------|------------|-----------------|-----------|-------------------|-------------|--------|
| 06-01-01 | 01 | 1 | SAE-01 | T-06-01 | Published AI activities keep `Ltype=1` and stay on `showmission.aspx?lid=` instead of introducing a new student route. | regression | `dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj --framework net8.0 --filter "FullyQualifiedName~ActivityPlan|FullyQualifiedName~StudentEntry"` | ✅ | ✅ green |
| 06-01-02 | 01 | 1 | SAE-02 | T-06-02 / T-06-03 | The student mission page renders learner guidance from published mission content and fails closed when mission data is absent. | regression | `dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj --framework net8.0 --filter "FullyQualifiedName~ShowMission|FullyQualifiedName~ActivityPlanStudent"` | ✅ | ✅ green |
| 06-02-01 | 02 | 2 | SAE-02 | T-06-04 / T-06-05 | The helper extracts deterministic goal, instructions, and steps from `Mission.Mcontent` only and returns `null` when the AI marker shape is missing. | integration | `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj --framework net8.0 --filter "FullyQualifiedName~ActivityPlanMissionView|FullyQualifiedName~ShowMission"` | ✅ | ✅ green |
| 06-02-02 | 02 | 2 | SAE-03 | T-06-06 | An authenticated teacher can publish to students, and an authenticated student can open the resulting `showmission.aspx` route and see the guidance shell plus upload panel. | e2e | `npx playwright test Tests/e2e/activity-plan-student-flow.spec.js` | ✅ | ⚠️ env-gated |

*Status: ⬜ pending · ✅ green · ❌ red · ⚠️ env-gated*

---

## Validation audit 2026-04-11

This audit reflects the current repository state after reconstructing missing
validation artifacts and adding the missing browser-level test.

| Metric | Count |
|--------|-------|
| Gaps found | 1 |
| Gaps resolved in code | 1 |
| Green in current workspace | 3 |
| Environment-gated | 1 |

---

## Wave 0 requirements

Existing test infrastructure covers all Phase 6 requirements. The only missing
piece was the dedicated browser-flow test file, which now exists at
`Tests/e2e/activity-plan-student-flow.spec.js`.

---

## Manual-only verifications

Full Nyquist compliance still depends on runtime data that this workspace has
not configured yet.

| Behavior | Requirement | Why Manual | Test Instructions |
|----------|-------------|------------|-------------------|
| Seed a teacher-owned course plus an enrolled student for the Playwright publish-to-student flow | SAE-03 | `activity-plan-student-flow.spec.js` is implemented, but it self-skips until `PLAYWRIGHT_TEACHER_USERNAME`, `PLAYWRIGHT_TEACHER_PASSWORD`, `PLAYWRIGHT_STUDENT_SNUM`, `PLAYWRIGHT_STUDENT_PASSWORD`, and `PLAYWRIGHT_ACTIVITY_PLAN_COURSE_ID` are provided. | Start the app, export the required environment variables, run `npx playwright test Tests/e2e/activity-plan-student-flow.spec.js`, and confirm the test passes end-to-end. |

---

## Validation sign-off

This sign-off records what is complete now and what still blocks `nyquist_compliant: true`.

- [x] All tasks have `<automated>` verify coverage or an added automated test file.
- [x] Sampling continuity has no three-task blind spot.
- [x] The missing Phase 6 automated reference now has a concrete test file and command.
- [x] No watch-mode flags are used.
- [x] Feedback latency stays under 90 seconds for the targeted commands.
- [ ] `nyquist_compliant: true` can be set in frontmatter in the current workspace.

**Approval:** pending - provide Phase 6 Playwright env data and rerun the browser test.
