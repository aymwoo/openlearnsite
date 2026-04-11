---
phase: 10-guided-inquiry-fallback-and-combined-publish
plan: 02
subsystem: publish-core
tags: [aspnet-webforms, csharp, javascript, guided-inquiry, full-lesson, publish]
requires:
  - phase: 10-guided-inquiry-fallback-and-combined-publish
    provides: typed full-lesson payloads and guided inquiry preview support
provides:
  - explicit `fullLessonPublish` handler and course-editor publish wiring
  - transaction-safe mixed full-lesson publish fan-out into brownfield activity records
  - server-owned block-keyed publish-link persistence for idempotent republish
affects: [phase-10-complete, teacher-course-editor-publish, mixed-brownfield-publish]
tech-stack:
  added: []
  patterns: [combined publish fan-out, blockKey keyed republish links, fail-closed mixed publish]
key-files:
  created: [.planning/phases/10-guided-inquiry-fallback-and-combined-publish/10-guided-inquiry-fallback-and-combined-publish-02-SUMMARY.md]
  modified:
    - App_Code/Common/AIActivityPlanPublishContentBuilder.cs
    - App_Code/Common/AIActivityPlanSavedDraftHelper.cs
    - App_Code/Dal/AIActivityPlanPublisher.cs
    - App_Code/Bll/AIActivityPlanPublisher.cs
    - App_Code/Model/AIActivityPlanPublishRequest.cs
    - App_Code/Model/AIActivityPlanPublishResult.cs
    - teacher/aiprovider_api.ashx
    - js/courseedit.js
    - Tests/CommonLogicTests/CommonLogicTests.cs
    - Tests/TeacherRegressionTests/TeacherRegressionTests.cs
    - .planning/STATE.md
    - .planning/ROADMAP.md
    - .planning/REQUIREMENTS.md
key-decisions:
  - "不新增 publish links 专用表或大范围 schema，直接把 `blockKey -> ids` 放进 full-lesson draft 的 `DraftJson`。"
  - "mixed full-lesson publish 复用既有 Mission/ListMenu/Exam/ExamPaper 路径，并保持 fail-closed。"
  - "前端只通过显式确认触发 `fullLessonPublish`，保留旧 `activityPlanPublish` 给单 activity 流程。"
patterns-established:
  - "Pattern 1: mixed draft publish 必须按 block 顺序扇出，并在同一事务内完成。"
  - "Pattern 2: republish 只依赖服务端持久化的 `blockKey` publish links，不用标题匹配。"
requirements-completed: [COMP-04, INQ-02]
completed: 2026-04-11
---

# Phase 10 Plan 02: Guided inquiry fallback and combined publish Summary

**教师现在可以在现有 `teacher/courseedit.aspx` 工作流里，通过一次明确确认，将混合整课草案发布为可执行的课堂活动集合；系统会按 block 顺序创建或更新对应 brownfield 实体，并基于 `blockKey` 幂等重发。**

## Accomplishments

- 为 mixed full-lesson draft 增加显式 `PublishFullLesson(...)` 路径，统一在事务内发布 `guidedInquiry`、`quiz`、`resource-study`、`webCourseware` 四类 block。
- 为 saved draft 增加服务端拥有的 `publishLinks` 持久化结构，把 `blockKey` 与已发布实体 id 绑定，支持 republish 走 create-or-update 而不是重复插入。
- 扩展 `AIActivityPlanPublishContentBuilder`，让整课发布可以把汇总内容写回课程正文，并用 marker 支持后续替换式 republish。
- 在 `teacher/aiprovider_api.ashx` 中新增 `case "fullLessonPublish":`，并保留原有 `activityPlanPublish` 单 activity 路径不变。
- 在 `js/courseedit.js` 中把整课草案发布按钮接到 `action=fullLessonPublish`，沿用现有 publish toggle 和显式确认交互。
- 增加 common logic 与 source-lock regression tests，覆盖 publish-link round-trip、整课摘要构建、handler wiring 和前端确认发布路径。

## Files Created/Modified

- `App_Code/Common/AIActivityPlanSavedDraftHelper.cs` - 增加 `publishLinks` DTO、序列化和解析支持。
- `App_Code/Common/AIActivityPlanPublishContentBuilder.cs` - 增加 full-lesson publish marker、课程正文摘要构建和 guided inquiry mission 内容构建。
- `App_Code/Dal/AIActivityPlanPublisher.cs` - 加入 mixed full-lesson publish 核心、block fan-out、create-or-update 逻辑和 draft link 回写。
- `App_Code/Bll/AIActivityPlanPublisher.cs` - 暴露 `PublishFullLesson(...)`。
- `App_Code/Model/AIActivityPlanPublishRequest.cs` - 扩展 full-lesson draft publish 所需请求字段。
- `App_Code/Model/AIActivityPlanPublishResult.cs` - 增加 mixed publish block 结果结构。
- `teacher/aiprovider_api.ashx` - 新增 `fullLessonPublish` handler 路由与授权边界。
- `js/courseedit.js` - 为 full-lesson draft 增加显式确认发布和成功回写课程内容逻辑。
- `Tests/CommonLogicTests/CommonLogicTests.cs` - 增加 full-lesson publish 相关 contract 和 builder 测试。
- `Tests/TeacherRegressionTests/TeacherRegressionTests.cs` - 锁定新 handler case、前端 publish wiring 和 mixed publish 相关源代码证据。
- `.planning/STATE.md` - 记录 Phase 10 已完成并切换到 Phase 11 待执行。
- `.planning/ROADMAP.md` - 标记 `10-02-PLAN.md` 与 Phase 10 完成。
- `.planning/REQUIREMENTS.md` - 标记 `COMP-04` 和 `INQ-02` 已完成。

## Verification

- `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj --filter FullLesson --framework net8.0`
  - net8.0: **Passed (11/11)**
- `dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj --filter CourseEdit --framework net8.0`
  - net8.0: **Passed (10/10)**

## Decisions Made

- mixed full-lesson publish 仍复用现有棕地实体模型，不为 v1 额外扩展专门的 composed publish schema。
- republish 幂等性落在 saved draft 的服务端链接映射上，避免 fragile 的标题匹配或前端自持状态。
- 整课发布继续要求教师显式确认；save/load/regenerate/remove 仍保持 preview-first 行为。

## Issues Encountered

- 本地自动化验证仍以 `net8.0` 切片为主；`net48` 宿主环境问题未在本次修复范围内处理。
- 未执行带真实数据库写入的手工浏览器 UAT，因此 brownfield fan-out 的最终联调仍建议在教师会话中再走一次。

## Next Phase Readiness

- Phase 11 可以直接基于当前 mixed publish 产出的 `Mission`、`Exam`、`ListMenu` 记录，接入 composed runtime 与 completion visibility。
- 当前 `publishLinks` 已提供 block-level 稳定映射，后续 student runtime 可按发布结果追踪每个 block 的入口与完成状态。

## Execution Notes

- 本次没有创建 git commit。
- 本次保留了原有单 activity `activityPlanPublish` 路径。

## Self-Check: PASSED

- Summary 文件已生成：`.planning/phases/10-guided-inquiry-fallback-and-combined-publish/10-guided-inquiry-fallback-and-combined-publish-02-SUMMARY.md`
