---
phase: 07-submission-and-completion-tracking
plan: 02
subsystem: testing
tags: [menuworks, works, student-upload, completion-tracking, webforms]
requires:
  - phase: 07-01
    provides: AI mission submissions locked to the legacy student upload route
provides:
  - duplicate-safe MenuWorks completion writes for AI activity uploads
  - regression coverage for student completion visibility on existing menu and summary surfaces
affects: [student-menu, student-summary, ai-activity-upload]
tech-stack:
  added: []
  patterns: [shared brownfield completion helper, source regression tests for legacy Web Forms wiring]
key-files:
  created: []
  modified:
    - App_Code/Bll/MenuWorks.cs
    - App_Code/Dal/MenuWorks.cs
    - App_Code/Bll/Works.cs
    - student/uploadwork.aspx.cs
    - student/uploadworkm.aspx.cs
    - Tests/CommonLogicTests/CommonLogicTests.cs
    - Tests/CommonLogicTests/CommonLogicTests.csproj
    - Tests/TeacherRegressionTests/TeacherRegressionTests.cs
key-decisions:
  - "Keep AI activity completion on MenuWorks and extract only the smallest shared helper in existing BLL classes."
  - "Pin completed-state visibility through source regressions on Scm.master.cs and myinfo.aspx.cs instead of introducing a new UI status model."
patterns-established:
  - "Upload handlers should call Works.EnsureMenuWorksCompletion so first submit and resubmit share one completion rule."
  - "AI activity completion visibility must stay backed by MenuWorks plus existing work-summary surfaces."
requirements-completed: [SCT-01, SCT-02]
duration: 4min
completed: 2026-04-11
---

# Phase 07 Plan 02: Completion Tracking Summary

**MenuWorks 去重完成记录与学生完成态可见性回归，确保 AI 活动上传继续走既有课堂完成模型。**

## Performance

- **Duration:** 4 min
- **Started:** 2026-04-11T10:05:21Z
- **Completed:** 2026-04-11T10:09:15Z
- **Tasks:** 2
- **Files modified:** 8

## Accomplishments
- 为 AI 活动上传补上共享的 `MenuWorks` 完成记录 helper，首次提交与重复提交都只保留一条 `Klid + Ksid` 完成记录。
- 在 `uploadwork.aspx.cs` 与 `uploadworkm.aspx.cs` 上统一复用 `Works.EnsureMenuWorksCompletion(...)`，未引入任何 AI 专用完成表。
- 为学生菜单完成图标与 `myinfo` 学习汇总补充回归测试，明确完成态仍依赖既有 `MenuWorks` / `Works` 模型。

## Task Commits

Each task was committed atomically:

1. **Task 1: Harden completion writes to one `MenuWorks` record per student activity** - `4d801a8` (test)
2. **Task 1: Harden completion writes to one `MenuWorks` record per student activity** - `b73f691` (feat)
3. **Task 2: Verify completed-state visibility across student flow surfaces** - `04b2e8b` (test)

_Note: Task 1 followed TDD with separate test and implementation commits._

## Files Created/Modified
- `App_Code/Bll/MenuWorks.cs` - 新增 `EnsureCompletion`，复用现有 `GetModelme` 防止重复完成记录。
- `App_Code/Dal/MenuWorks.cs` - 在 DAL 插入前增加空值与重复记录保护。
- `App_Code/Bll/Works.cs` - 提供 `EnsureMenuWorksCompletion` 共享 helper，集中生成 `MenuWorks` 完成数据。
- `student/uploadwork.aspx.cs` - 首次上传成功后改为调用共享完成 helper。
- `student/uploadworkm.aspx.cs` - 备用上传模式同样改为调用共享完成 helper。
- `Tests/CommonLogicTests/CommonLogicTests.cs` - 增加 `ActivityPlanCompletion` 源码守卫测试。
- `Tests/CommonLogicTests/CommonLogicTests.csproj` - 为 net48 测试目标补充 `System.Web` 引用，避免既有链接源码无法编译。
- `Tests/TeacherRegressionTests/TeacherRegressionTests.cs` - 增加学生菜单与学习汇总完成态可见性回归断言。

## Decisions Made
- 保持 AI 活动完成写入继续落在 `MenuWorks`，只抽取最小共享 helper，不新增 AI 专属完成存储。
- 完成态可见性通过既有 `Scm.master.cs` 与 `myinfo.aspx.cs` 断言验证，避免扩展新的 teacher/student 状态面板。

## Deviations from Plan

### Auto-fixed Issues

**1. [Rule 3 - Blocking] 为 CommonLogicTests 的 net48 目标补齐 `System.Web` 引用**
- **Found during:** Task 1（TDD RED）
- **Issue:** `AIActivityPlanMissionViewHelper` 通过链接源码进入 `CommonLogicTests` 后，net48 目标缺少 `System.Web` 引用，导致测试项目先于本任务断言失败。
- **Fix:** 在 `Tests/CommonLogicTests/CommonLogicTests.csproj` 中仅对 `net48` 增加 `System.Web` 引用，恢复既有测试编译前提。
- **Files modified:** `Tests/CommonLogicTests/CommonLogicTests.csproj`
- **Verification:** `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj --filter "FullyQualifiedName~MenuWorks|FullyQualifiedName~ActivityPlanCompletion" --framework net8.0`
- **Committed in:** `4d801a8`

---

**Total deviations:** 1 auto-fixed (1 blocking)
**Impact on plan:** 该修复仅恢复测试基础设施，不改变业务范围；其余实现按计划完成。

## Issues Encountered
- 本地 `dotnet test` 的 net48 运行器缺少 `testhost.net48.exe`，因此自动验证改用 `--framework net8.0` 执行同一组源码回归测试。

## Manual UAT

1. 教师端发布一个启用上传的 AI 活动，并确认学生侧入口可访问。
2. 学生端登录后打开该活动，上传一个合法作品文件并看到成功返回。
3. 刷新当前课程左侧菜单，确认该活动从未完成状态变为完成图标。
4. 返回 `student/myinfo.aspx`，确认该课程进入已完成/已学列表而不是保持未触达状态。
5. 在教师未评前再次上传同一活动，确认仍可重提，但数据库语义应保持单条 `MenuWorks` 完成记录。

## Next Phase Readiness
- AI 活动上传后的完成记录现在显式落在既有 `MenuWorks` 模型上，可继续做里程碑级人工验收。
- 若后续需要覆盖真实数据库层面的重复写入验证，可在具备 SQL Server 测试夹具时补充集成测试。

## Self-Check: PASSED

- FOUND: `.planning/phases/07-submission-and-completion-tracking/07-submission-and-completion-tracking-02-SUMMARY.md`
- FOUND: `4d801a8`
- FOUND: `b73f691`
- FOUND: `04b2e8b`
