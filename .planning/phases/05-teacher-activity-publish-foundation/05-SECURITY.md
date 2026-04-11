---
phase: 05
slug: teacher-activity-publish-foundation
status: verified
threats_open: 0
asvs_level: 1
created: 2026-04-11
---

# Phase 05 - Security

This audit records the Phase 5 trust boundaries, the planned STRIDE threats,
and the verification evidence used to confirm the completed mitigations.

---

## Trust boundaries

This phase introduces server-owned AI activity publishing, so the main security
concerns are untrusted browser input crossing into teacher-authorized handler
code and one publish operation mutating multiple linked records.

| Boundary | Description | Data Crossing |
|----------|-------------|---------------|
| teacher browser -> publish handler | Untrusted publish payload crosses from the browser into authenticated server code. | `cid`, `topic`, `publishToStudents`, `selectedSections`, `currentDraft` |
| publish handler -> draft persistence | Linked activity identity is stored and reused across later requests. | Draft JSON, course snapshot, `LinkedMissionId`, `LinkedListMenuId` |
| handler -> publish BLL | Teacher-authenticated requests become privileged publish operations. | `AIActivityPlanPublishRequest` |
| publish DAL -> SQL tables | One operation mutates `Courses`, `Mission`, `ListMenu`, and `CourseActivityPlanDraft`. | Course content, mission content, publish state, linked ids |
| teacher browser -> `teacher/aiprovider_api.ashx` | User-controlled publish settings and selected sections cross into privileged server code. | Publish toggle, selected section keys, draft payload |
| handler -> publish result -> browser editor | Server response becomes the teacher-visible committed lesson body. | `updatedCourseContent`, publish result metadata |

---

## Threat register

All threats for this phase are closed. No accepted-risk or transfer disposition
was required.

| Threat ID | Category | Component | Disposition | Mitigation | Status |
|-----------|----------|-----------|-------------|------------|--------|
| T-05-01 | T | `CourseActivityPlanDraft` linkage columns | mitigate | Persist linked ids only through server-owned draft helpers, DAL mapping, and linkage tests in `Tests/CommonLogicTests/ActivityPlanDraftLinkageTests.cs`. | closed |
| T-05-02 | I | Publish request contract | mitigate | Use explicit `AIActivityPlanPublishRequest` and `AIActivityPlanPublishResult` models instead of anonymous payloads. | closed |
| T-05-03 | R | Upgrade path | mitigate | Add migration coverage for `LinkedMissionId` and `LinkedListMenuId` in `App_Code/Utility/UpdateGrade.cs` and source-level tests. | closed |
| T-05-04 | T | Publish DAL multi-table write | mitigate | Wrap course, mission, menu, and draft-link writes in one SQL transaction in `App_Code/Dal/AIActivityPlanPublisher.cs`. | closed |
| T-05-05 | E | Mission/menu route contract | mitigate | Force `Mupload=true`, `Mcategory=0`, `Mfiletype="office"`, and `Ltype=1` in the publish core. | closed |
| T-05-06 | I | Student mission content | mitigate | Build full `Mission.Mcontent` from the validated draft through `AIActivityPlanPublishContentBuilder`. | closed |
| T-05-07 | S | `activityPlanPublish` route | mitigate | Reuse `TryGetAuthorizedCourse(...)` before publish work in `teacher/aiprovider_api.ashx`. | closed |
| T-05-08 | T | Browser publish toggle | mitigate | Use only the dedicated AI `publishToStudents` input and keep it separate from legacy publish defaults. | closed |
| T-05-09 | R | Editor write-back | mitigate | Replace editor content only with server-returned `updatedCourseContent` in `js/courseedit.js`. | closed |

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
| 2026-04-11 | 9 | 9 | 0 | OpenCode |

### Evidence

- Artifact review: `05-01-PLAN.md`, `05-02-PLAN.md`, `05-03-PLAN.md`, and all
  three Phase 5 summary files.
- Code review: `App_Code/Common/AIActivityPlanSavedDraftHelper.cs`,
  `App_Code/Dal/CourseActivityPlanDraft.cs`,
  `App_Code/Utility/UpdateGrade.cs`,
  `App_Code/Common/AIActivityPlanPublishContentBuilder.cs`,
  `App_Code/Dal/AIActivityPlanPublisher.cs`,
  `App_Code/Bll/AIActivityPlanPublisher.cs`,
  `teacher/aiprovider_api.ashx`, `teacher/courseedit.aspx`, and
  `js/courseedit.js`.
- Verification: `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj -f
  net8.0 --filter "FullyQualifiedName~ActivityPlanPublishRequest|FullyQualifiedName~ActivityPlanDraftMigration|FullyQualifiedName~ActivityPlanSavedDraft|FullyQualifiedName~ActivityPlanPublishContent|FullyQualifiedName~ActivityPlanPublish"`
  passed with 18 tests.
- Verification: `dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj -f
  net8.0 --filter "FullyQualifiedName~ActivityPlanPublish|FullyQualifiedName~CourseEdit_Should"`
  passed with 6 tests.
- Summary threat flags: no `## Threat Flags` sections were present in the Phase
  5 summary artifacts, so the audit used the plan threat registers and
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
