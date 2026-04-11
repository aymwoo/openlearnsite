---
phase: 10-guided-inquiry-fallback-and-combined-publish
plan: 01
subsystem: api-ui
tags: [aspnet-webforms, csharp, javascript, guided-inquiry, full-lesson]
requires:
  - phase: 09-existing-activity-block-composition
    provides: typed full-lesson payloads and type-aware mixed-draft preview shell
provides:
  - server-owned `guidedInquiry` fallback payload under the existing full-lesson block envelope
  - fail-closed validation and saved-draft round-trip for inquiry fallback blocks
  - mixed full-lesson preview support for recognizable guided inquiry summaries in course editor
affects: [10-02-combined-publish, teacher-course-editor-preview, inquiry-runtime-reuse]
tech-stack:
  added: []
  patterns: [typed fallback payload under stable block envelope, preview-only inquiry rendering from server DTO]
key-files:
  created: [.planning/phases/10-guided-inquiry-fallback-and-combined-publish/10-guided-inquiry-fallback-and-combined-publish-01-SUMMARY.md]
  modified:
    - App_Code/Common/AIActivityPlanDraftHelper.cs
    - teacher/aiprovider_api.ashx
    - teacher/courseedit.aspx
    - js/courseedit.js
    - Tests/CommonLogicTests/CommonLogicTests.cs
    - Tests/TeacherRegressionTests/TeacherRegressionTests.cs
    - .planning/STATE.md
    - .planning/ROADMAP.md
    - .planning/REQUIREMENTS.md
key-decisions:
  - "`guidedInquiry` 保持最小 server-owned payload：探究目标、探究问题、回退原因、成果预期、顺序步骤。"
  - "Phase 10 Plan 01 只扩展 generate/save/load/regenerate/preview，不触碰 `Mission`、`Exam`、`ListMenu` 实际发布路径。"
patterns-established:
  - "Pattern 1: 当 blockType 为 `guidedInquiry` 时，payload 必须具备 inquiry goal/prompt 与顺序步骤，否则 full-lesson draft fail closed。"
  - "Pattern 2: inquiry fallback 的老师预览只消费服务端 DTO，不由浏览器自行猜测探究摘要。"
requirements-completed: [INQ-01, INQ-02]
duration: 24 min
completed: 2026-04-11
---

# Phase 10 Plan 01: Guided inquiry fallback and combined publish Summary

**整课草案现在可以在现有 mixed full-lesson DTO 中安全承载 `guidedInquiry` fallback block，并在 `teacher/courseedit.aspx` 中以可识别的引导探究摘要进行预览，仍保持 preview-only。**

## Performance

- **Duration:** 24 min
- **Tasks:** 2
- **Files modified:** 9

## Accomplishments

- 为 `FullLessonDraftBlock` 增加 `guidedInquiry` payload，并补齐 fail-closed parser/validator，拒绝缺少目标、问题或顺序步骤的部分 payload。
- 让 `fullLessonGenerate`、`fullLessonRegenerateBlock`、`fullLessonLoadDraft`、saved-draft round-trip 都能返回并保留 inquiry fallback block。
- 在课程编辑页增加引导探究卡片样式、fallback 原因说明、探究目标/问题/成果预期摘要与复制文本输出，继续保持“只预览、不发布”。

## Files Created/Modified

- `App_Code/Common/AIActivityPlanDraftHelper.cs` - 增加 `guidedInquiry` DTO、解析分支、顺序步骤校验和 blockType 路由。
- `teacher/aiprovider_api.ashx` - 在 full-lesson 生成/重生成/序列化路径中接入 `guidedInquiry`，并在 block replace 时保留 typed fallback 数据。
- `teacher/courseedit.aspx` - 增加 inquiry fallback 卡片所需的最小样式钩子。
- `js/courseedit.js` - 渲染引导探究类型标签、fallback 原因摘要、typed preview items 和复制文本。
- `Tests/CommonLogicTests/CommonLogicTests.cs` - 固定 `guidedInquiry` contract、reject partial payload、saved-draft round-trip 行为。
- `Tests/TeacherRegressionTests/TeacherRegressionTests.cs` - 锁定 handler wiring、preview helper 和 preview-only 边界。
- `.planning/STATE.md` - 记录 Plan 01 已完成并切换到 Plan 02 待执行状态。
- `.planning/ROADMAP.md` - 把 `10-01-PLAN.md` 标记为完成。
- `.planning/REQUIREMENTS.md` - 把 `INQ-01`、`INQ-02` 标记为完成。

## Verification

- `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj --filter FullLesson`
  - net8.0: **Passed (8/8)**
  - net48: 环境缺少 `testhost.net48.exe`，测试宿主中止
- `dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj --filter CourseEdit --framework net8.0`
  - net8.0: **Passed (10/10)**

## Decisions Made

- `guidedInquiry` preview 摘要继续只显示老师 review 所需信息，不提前引入 publish fan-out 或 student-side composed runtime 逻辑。
- inquiry fallback 生成先复用现有 `ActivityPlanDraftStep` 信息做最小安全映射，等 `10-02`/Phase 11 再接入真实 publish/runtime 语义。

## Issues Encountered

- 本地 `dotnet test` 的 net48 切片仍因环境缺少 `testhost.net48.exe` 无法执行；本次以 net8.0 可运行切片完成自动验证。

## Next Phase Readiness

- `10-02` 可以在当前稳定的 `guidedInquiry` block contract 之上增加 `fullLessonPublish`、block-keyed publish-link persistence 和 mixed publish fan-out。
- 当前 inquiry payload 已与 mission guide 所需“目标/说明/步骤”结构接近，后续可直接映射到 brownfield guided mission 内容构建器。

## Execution Notes

- 本次没有创建 git commit。
- 本次没有改动 `activityPlanPublish`、`Mission`、`Exam`、`ListMenu` 的实际落库路径。

## Self-Check: PASSED

- Summary 文件已生成：`.planning/phases/10-guided-inquiry-fallback-and-combined-publish/10-guided-inquiry-fallback-and-combined-publish-01-SUMMARY.md`
