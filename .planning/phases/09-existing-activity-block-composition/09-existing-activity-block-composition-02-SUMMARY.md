---
phase: 09-existing-activity-block-composition
plan: 02
subsystem: ui
tags: [aspnet-webforms, javascript, full-lesson, existing-activity-preview]
requires:
  - phase: 09-existing-activity-block-composition
    provides: typed full-lesson payloads for quiz, resource-study, and webCourseware
provides:
  - type-aware preview cards for quiz, resource-study, and webCourseware blocks in course editor
  - saved-draft resume rendering that preserves typed existing-activity summaries
  - preview-only regression coverage for block-level refine and no-auto-publish behavior
affects: [phase-10-publish-flow, teacher-course-editor-preview, composed-draft-review]
tech-stack:
  added: []
  patterns: [type-aware block preview from server DTO only, block-level refine without editor body writes]
key-files:
  created: [.planning/phases/09-existing-activity-block-composition/09-existing-activity-block-composition-02-SUMMARY.md]
  modified:
    - teacher/courseedit.aspx
    - js/courseedit.js
    - Tests/TeacherRegressionTests/TeacherRegressionTests.cs
    - .planning/STATE.md
    - .planning/ROADMAP.md
    - .planning/REQUIREMENTS.md
key-decisions:
  - "类型化摘要继续直接消费服务端 `quiz`、`resourceStudy`、`webCourseware` payload，不在浏览器侧拼装第二套 block 语义。"
  - "整课草案的 remove、regenerate、save、resume 继续以 `blockKey` 和 `fullLesson*` DTO 为边界，不触碰 `mcontent` 或 publish 路径。"
patterns-established:
  - "Pattern 1: 支持的 brownfield block 在课程编辑页中必须展示可识别的已有活动摘要，而不只是通用 block metadata。"
  - "Pattern 2: typed preview 的 UI 丰富化不改变 preview-first 边界，整课草案仍不自动写入正文也不自动发布。"
requirements-completed: [COMP-01, COMP-02, COMP-03]
duration: 18 min
completed: 2026-04-11
---

# Phase 09 Plan 02: Existing activity block composition Summary

**课程编辑页现在能把 `quiz`、`resource-study`、`webCourseware` 作为可识别的已有活动卡片预览出来，同时继续保持整课草案仅预览、不自动发布。**

## Performance

- **Duration:** 18 min
- **Tasks:** 2
- **Files modified:** 6

## Accomplishments

- 为 `quiz`、`resource-study`、`webCourseware` 增加类型化卡片样式和摘要区域，让老师能在整课草案里直接看出 AI 选了什么已有活动。
- 让保存、恢复、移除、重生成继续围绕已有 `fullLesson*` DTO 和 `blockKey` 工作，未引入新的客户端状态结构。
- 用回归测试固定类型化预览字符串、摘要 helper 和 preview-only 边界。

## Files Created/Modified

- `teacher/courseedit.aspx` - 增加已有活动摘要卡片所需的最小样式钩子。
- `js/courseedit.js` - 渲染 quiz/resource-study/webCourseware 类型化摘要、类型标签和彩色卡片，并把摘要接入复制文本。
- `Tests/TeacherRegressionTests/TeacherRegressionTests.cs` - 固定类型化预览 helper、摘要文案和 preview-only 行为。
- `.planning/STATE.md` - 记录 Phase 09 已执行完成并切换到下一阶段准备态。
- `.planning/ROADMAP.md` - 把 Phase 09 标记为 complete。
- `.planning/REQUIREMENTS.md` - 把 `COMP-01`、`COMP-02`、`COMP-03` 标记为完成。

## Verification

- `dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj --filter CourseEdit`
  - net8.0: **Passed (10/10)**
  - net48: 环境缺少 `testhost.net48.exe`，测试宿主中止

## Decisions Made

- 既有活动摘要只读取服务端已验证的 typed payload 字段，避免前端根据通用字段自行猜测 quiz/resource-study/webCourseware 内容。
- 类型化预览继续作为整课草案预览的一部分存在，不把摘要应用到 `mcontent`，也不新增 publish 入口。

## Deviations from Plan

None - plan executed exactly as written.

## Issues Encountered

- 本地 `dotnet test` 的 net48 切片仍因环境缺少 `testhost.net48.exe` 无法执行；本次继续以 net8.0 可运行切片完成自动验证。

## User Setup Required

None - no external service configuration required.

## Next Phase Readiness

- Phase 09 已具备老师可识别的已有活动组合预览，下一步可以进入 Phase 10 的 guided inquiry fallback 与 combined publish。
- 后续 publish 实现可继续复用本阶段 UI 中已固定的 typed summary 字段，避免老师在预览和发布结果之间感知漂移。

## Execution Notes

- 本次没有创建 git commit。
- 本次没有改动 `activityPlanPublish`、`Mission`、`Exam`、`ListMenu` 的实际落库路径。

## Self-Check: PASSED

- Summary 文件已生成：`.planning/phases/09-existing-activity-block-composition/09-existing-activity-block-composition-02-SUMMARY.md`
