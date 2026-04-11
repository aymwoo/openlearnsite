---
phase: 11-composed-runtime-and-progress-visibility
plan: 01
subsystem: composed-runtime-core
tags: [aspnet-webforms, csharp, full-lesson, runtime, progress, fail-closed]
requires:
  - phase: 10-guided-inquiry-fallback-and-combined-publish
    provides: block-keyed publish links and mixed brownfield publish output
provides:
  - fail-closed composed runtime/progress helper over saved full-lesson publish links
  - ordered block summaries with explicit runtime route typing and completion state contract
  - source-lock coverage for current student and teacher boundary surfaces
affects: [phase-11-in-progress, student-runtime-foundation, teacher-progress-boundary]
tech-stack:
  added: []
  patterns: [server-owned composed runtime contract, fail-closed publish-link resolution, legacy-evidence-backed completion]
key-files:
  created:
    - App_Code/Common/AIActivityPlanComposedRuntimeHelper.cs
    - .planning/phases/11-composed-runtime-and-progress-visibility/11-composed-runtime-and-progress-visibility-01-SUMMARY.md
  modified:
    - App_Code/Common/AIActivityPlanSavedDraftHelper.cs
    - App_Code/Common/AIActivityPlanDraftHelper.cs
    - App_Code/Model/AIActivityPlanPublishResult.cs
    - App_Code/Dal/AIActivityPlanPublisher.cs
    - Tests/CommonLogicTests/CommonLogicTests.cs
    - Tests/CommonLogicTests/CommonLogicTests.csproj
    - Tests/TeacherRegressionTests/TeacherRegressionTests.cs
    - .planning/ROADMAP.md
    - .planning/STATE.md
key-decisions:
  - "composed runtime helper 只消费服务端持久化的 `publishLinks` 和 full-lesson block payload，不做标题匹配。"
  - "publish metadata 缺失时返回 `unknown` / runtime-not-ready，publish link 漂移时直接拒绝整组结果。"
  - "Phase 11 继续绑定既有 `student/Scm.master.cs`、`student/myinfo.aspx.cs`、`teacher/courseshow.aspx.cs`，不新增 AI 专用学生 runtime 页。"
patterns-established:
  - "Pattern 1: composed runtime summary 必须按 full-lesson block 顺序输出，并保留 `blockKey`。"
  - "Pattern 2: runtime route 由 block type 和 brownfield ids 显式解析，不从标题或菜单顺序猜测。"
  - "Pattern 3: completion 只建立在 MenuWorks / mission work pass 等既有证据之上。"
requirements-completed: []
completed: 2026-04-11
---

# Phase 11 Plan 01: Composed runtime and progress visibility Summary

**系统现在已经拥有一个服务端的 composed runtime/progress contract：它可以把一份已发布的整课草案解析为按 block 顺序排列的 brownfield runtime 摘要，并在 publish link 缺失时 fail-closed，在 publish link 漂移时直接拒绝结果。**

## Accomplishments

- 新增 `AIActivityPlanComposedRuntimeHelper`，把 full-lesson draft + `publishLinks` 解析为 ordered block summaries。
- 每个 block summary 都保留 `blockKey`、`blockType`、标题、`ListMenuId`、`ListMenuType`、`RuntimeRouteType`、runtime URL，以及 `completed` / `incomplete` / `unknown` completion state。
- publish link 缺少 `ListMenuId`、`MissionId`、`ExamId` 或 `PaperId` 时，helper 会返回 runtime-not-ready 的 `unknown` 状态，而不是伪造完成情况。
- publish link 与 block payload / brownfield menu 发生漂移时，helper 会 fail-closed 返回 `null`，避免输出部分错误进度数据。
- 扩展 saved draft 与 draft helper 的最小公共方法，统一 `blockKey` publish link 查找和 block type normalization。
- 为 publish result model 增加最小运行时元数据字段 `ListMenuType` 和 `RuntimeRouteType`，与 mixed publish 输出保持对齐。
- 增加 common logic tests 和 source-lock regression tests，钉住当前学生/教师边界仍然是既有页面，不引入新的 AI runtime route。

## Files Created/Modified

- `App_Code/Common/AIActivityPlanComposedRuntimeHelper.cs` - 新的 composed runtime/progress helper 和 summary DTO。
- `App_Code/Common/AIActivityPlanSavedDraftHelper.cs` - 增加 `GetPublishLink(...)` 统一 publish link 解析入口。
- `App_Code/Common/AIActivityPlanDraftHelper.cs` - 暴露 full-lesson block type normalization / supported-publish 判断。
- `App_Code/Model/AIActivityPlanPublishResult.cs` - 为 published block result 增加最小 runtime metadata 字段。
- `App_Code/Dal/AIActivityPlanPublisher.cs` - mixed publish block result 回写 `ListMenuType` / `RuntimeRouteType`。
- `Tests/CommonLogicTests/CommonLogicTests.cs` - 增加 composed runtime 顺序、legacy evidence、fail-closed 测试。
- `Tests/CommonLogicTests/CommonLogicTests.csproj` - 引入新 helper 和 publish result model 源文件。
- `Tests/TeacherRegressionTests/TeacherRegressionTests.cs` - 增加 current-boundary source-lock coverage。
- `.planning/ROADMAP.md` - 标记 `11-01-PLAN.md` 已完成。
- `.planning/STATE.md` - 将当前 focus 切到 `11-02-PLAN.md`。

## Verification

- `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj --filter ComposedRuntime`
- `dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj --filter ComposedRuntime`

## Decisions Made

- 本计划只落 helper contract 和边界测试，不提前把 UI wiring 混入，避免把 Plan 02 的页面改动和 contract 设计耦合在一起。
- publish metadata 不完整时保留 block context，但 runtime 进入和完成状态一律 fail-closed；只有 drift / unsupported 才整组拒绝。
- 运行时入口继续复用 `showmission`、`description`、`ware`、`preview` 这四个既有 student runtime 页面。

## Issues Encountered

- 现有 full-lesson draft 测试数据里仍有未进入 mixed publish 范围的 block type，因此 composed runtime helper 测试额外引入了仅包含当前 publishable block set 的测试草案。
- 本次 helper 仍是纯 contract 层；真实页面展示和 breadcrumb/progress UI 需要在 `11-02` 再接上。

## Next Phase Readiness

- `11-02` 可以直接消费 composed block summaries，把 block order、runtime hint、completion visibility 接入 `student/Scm.master.cs`、`student/myinfo.aspx.cs`、`teacher/courseshow.aspx.cs`。
- mixed publish output 和 saved draft payload 现在已经具备稳定的 runtime/progress 解析边界，不需要再在页面里手写 JSON 猜关系。

## Execution Notes

- 本次没有创建 git commit。
- 本次没有新增 AI 专用学生 runtime 页面。

## Self-Check: PASSED

- Summary 文件已生成：`.planning/phases/11-composed-runtime-and-progress-visibility/11-composed-runtime-and-progress-visibility-01-SUMMARY.md`
