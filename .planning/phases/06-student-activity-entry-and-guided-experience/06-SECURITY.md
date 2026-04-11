---
phase: 06
slug: student-activity-entry-and-guided-experience
status: verified
threats_open: 0
asvs_level: 1
created: 2026-04-11
---

# Phase 06 - Security

This audit records the Phase 6 trust boundaries, the planned STRIDE threats,
and the verification evidence used to confirm the completed mitigations.

---

## Trust boundaries

This phase keeps the student experience on the legacy mission route, so the
main security concerns are routing published teacher-owned activity data into
the existing student menu and rendering only published mission content as
learner guidance.

| Boundary | Description | Data Crossing |
|----------|-------------|---------------|
| teacher publish data -> student menu | Previously teacher-owned publish output becomes visible student navigation. | `Ltype`, `Lshow`, `lid`, menu title |
| `student/showmission.aspx.cs` -> student browser | Server-rendered mission content becomes learner guidance on the page. | `Mission.Mcontent`, learner guidance HTML |
| published mission HTML -> guidance helper | Existing stored content is interpreted into learner guidance sections. | Published mission HTML markers and section content |
| server guidance output -> student browser | Learner-facing instructions are presented as actionable classroom steps. | Goal, instructions, steps, upload-shell state |

---

## Threat register

All threats for this phase are closed. No accepted-risk or transfer disposition
was required.

| Threat ID | Category | Component | Disposition | Mitigation | Status |
|-----------|----------|-----------|-------------|------------|--------|
| T-06-01 | I | Student route contract | mitigate | Keep `Ltype=1` pinned to `~/student/showmission.aspx?lid=` in `student/Scm.master.cs` and lock it with `ActivityPlanStudentEntry_ShowMissionRoute_ShouldStayPinnedToLegacyMissionPage`. | closed |
| T-06-02 | T | Mission guidance rendering | mitigate | Build learner guidance from published `Mission.Mcontent` only in `student/showmission.aspx.cs` through `AIActivityPlanMissionViewHelper.BuildActivityGuideView(...)`. | closed |
| T-06-03 | D | Mission page fallback | mitigate | Fail closed in `ShowMissionNotFound()` and hide the guidance and upload/group panels when mission data is missing or invalid. | closed |
| T-06-04 | T | Mission guidance parser | mitigate | Parse only the published activity-plan marker format and return `null` when required sections are absent in `App_Code/Common/AIActivityPlanMissionViewHelper.cs`. | closed |
| T-06-05 | I | Student guidance source | mitigate | Keep the student flow server-owned from `Mission.Mcontent`; helper tests confirm it never reads teacher draft JSON, local storage, or teacher routes. | closed |
| T-06-06 | R | Phase 6 validation | mitigate | Pair automated helper and source assertions with explicit authenticated browser UAT documented in `06-student-activity-entry-and-guided-experience-02-SUMMARY.md`. | closed |

Status values in this register are audit outcomes, not plan intent.

---

## Accepted risks log

This audit found no residual items that required explicit risk acceptance.

No accepted risks.

---

## Security audit trail

This phase was audited from plan and summary artifacts because no prior
phase-level `SECURITY.md` existed.

| Audit Date | Threats Total | Closed | Open | Run By |
|------------|---------------|--------|------|--------|
| 2026-04-11 | 6 | 6 | 0 | OpenCode |

### Evidence

- Artifact review: `06-01-PLAN.md`, `06-02-PLAN.md`, and both Phase 6 summary
  files.
- Code review: `student/Scm.master.cs`, `student/showmission.aspx`,
  `student/showmission.aspx.cs`,
  `App_Code/Common/AIActivityPlanMissionViewHelper.cs`,
  `Tests/TeacherRegressionTests/TeacherRegressionTests.cs`, and
  `Tests/CommonLogicTests/CommonLogicTests.cs`.
- Verification: `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj -f
  net8.0 --filter "FullyQualifiedName~ActivityPlanMissionView|FullyQualifiedName~ShowMission"`
  passed with 4 tests.
- Verification: `dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj -f
  net8.0 --filter "FullyQualifiedName~ActivityPlan|FullyQualifiedName~ShowMission|FullyQualifiedName~StudentEntry"`
  passed with 6 tests.
- Manual verification path: the Phase 6 plan and summary require authenticated
  browser UAT for publish -> student menu -> `showmission.aspx` -> readable
  guidance -> upload panel retention.
- Summary threat flags: no `## Threat Flags` sections were present in the Phase
  6 summary artifacts, so the audit used the plan threat registers and
  implementation evidence directly.

---

## Sign-off

This phase satisfies the security gate because every planned threat has a
closed disposition and no accepted risks remain open.

- [x] All threats have a disposition (mitigate / accept / transfer)
- [x] Accepted risks documented in Accepted Risks Log
- [x] `threats_open: 0` confirmed
- [x] `status: verified` set in frontmatter

**Approval:** verified 2026-04-11
