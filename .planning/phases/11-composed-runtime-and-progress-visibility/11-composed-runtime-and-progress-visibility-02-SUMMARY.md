---
phase: 11-composed-runtime-and-progress-visibility
plan: 02
subsystem: composed-runtime-wiring
tags: [aspnet-webforms, csharp, student-runtime, teacher-visibility, progress]
requires:
  - phase: 11-composed-runtime-and-progress-visibility
    provides: composed runtime helper and fail-closed publish-link contract
provides:
  - student menu block order and per-block progress cues on existing runtime routes
  - mission runtime context for composed lesson position and progress
  - student summary and teacher course visibility for composed block progress
affects: [milestone-v1.2-complete, student-runtime-visible, teacher-course-progress-visible]
tech-stack:
  added: []
  patterns: [existing-page wiring, server-owned progress summaries, no-new-runtime-route]
key-files:
  created:
    - .planning/phases/11-composed-runtime-and-progress-visibility/11-composed-runtime-and-progress-visibility-02-SUMMARY.md
  modified:
    - App_Code/Common/AIActivityPlanComposedRuntimeHelper.cs
    - App_Code/Bll/Works.cs
    - student/Scm.master
    - student/Scm.master.cs
    - student/showmission.aspx
    - student/showmission.aspx.cs
    - student/myinfo.aspx
    - student/myinfo.aspx.cs
    - teacher/courseshow.aspx
    - teacher/courseshow.aspx.cs
    - Tests/TeacherRegressionTests/TeacherRegressionTests.cs
    - .planning/ROADMAP.md
    - .planning/STATE.md
key-decisions:
  - "学生侧继续复用 `student/Scm.master` 菜单和既有 `showmission` / `description` / `ware` / `preview` runtime，不新增 AI 页面。"
  - "整课进度提示全部从 `AIActivityPlanComposedRuntimeHelper` 输出读取，不靠标题或顺序猜测。"
  - "教师侧完成态采用已有 brownfield 证据：`MenuWorks` 或 `Works` 中任意学生已完成记录。"
patterns-established:
  - "Pattern 1: 学生菜单项标题可增加 `第N环` 定位，但路由仍由原 `Ltype` 控制。"
  - "Pattern 2: mission runtime 只增加最小整课上下文和进度提示，不改提交流程。"
  - "Pattern 3: teacher/courseshow 继续作为课程可见性入口，不扩展新 dashboard。"
requirements-completed: [RUN-01, RUN-02, RUN-03, RUN-04]
completed: 2026-04-11
---

# Phase 11 Plan 02: Composed runtime and progress visibility Summary

**Phase 11 已完成：已发布的整课 block 现在可以在现有学生课堂流和教师课程页中，以最小增量方式显示 block 顺序、当前完成状态和整课上下文。**

## Accomplishments

- 在 `student/Scm.master` 中接入 composed runtime summaries，为现有菜单项补充 `第N环` 顺序提示、整课进度摘要和按 block 的 tooltip 状态。
- 保持所有 block 继续走原 brownfield runtime：`showmission`、`description`、`ware`、`preview`。
- 在 `student/showmission.aspx(.cs)` 增加整课上下文卡片，学生进入 mission-backed block 时可看到当前所处环节和整课 block 进度。
- 在 `student/myinfo.aspx(.cs)` 已学学案区域增加整课环节进度胶囊，并在侧边补充整课完成汇总。
- 在 `teacher/courseshow.aspx(.cs)` 增加整课活动发布进度摘要，并在既有菜单行中补充 `第N环` 与 block 状态提示。
- 在 `AIActivityPlanComposedRuntimeHelper` 中补充按课程读取 full-lesson 草案的入口，以及按 `ListMenuId` 查找 block summary 的辅助方法。
- 在 `Works` BLL 暴露 `GetRecordCount(...)` 以便教师页按现有证据判断“已有学生完成”。
- 扩展 `TeacherRegressionTests` source-lock coverage，钉住 student/teacher 边界页面上的 composed runtime wiring。

## Verification

- `dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj --framework net8.0 --filter ComposedRuntime`
- `dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj --framework net8.0 --filter CourseShow`

## Issues Encountered

- 当前环境缺少 `net48` test host，`TeacherRegressionTests` 的 `net48` 目标会在测试宿主启动阶段失败，因此本次自动验证以 `net8.0` 目标为准。
- 真实教师/学生登录态的浏览器 UAT 仍未在本次自动执行中覆盖。

## Next Phase Readiness

- v1.2 里程碑的 11 个 phase 已完成。
- 若后续继续扩展，可在此基础上增加更细的教师班级维度统计或浏览器端视觉优化，但不影响当前 brownfield 路由契约。

## Execution Notes

- 本次没有创建 git commit。
- 本次没有新增 AI 专用学生 runtime 页面。

## Self-Check: PASSED

- Summary 文件已生成：`.planning/phases/11-composed-runtime-and-progress-visibility/11-composed-runtime-and-progress-visibility-02-SUMMARY.md`
