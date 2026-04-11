---
phase: 08-full-lesson-draft-orchestration
plan: 02
subsystem: ui
tags: [aspnet-webforms, javascript, full-lesson, teacher-course-editor]
requires:
  - phase: 08-full-lesson-draft-orchestration
    provides: full-lesson draft DTO, saved-draft envelope, parallel fullLesson actions
provides:
  - ordered full-lesson block card preview in teacher course editor
  - block-level remove and regenerate interactions scoped by blockKey
  - saved full-lesson draft resume flow without auto-writing mcontent
affects: [phase-09-publish-orchestration, teacher-uat, full-lesson-review]
tech-stack:
  added: []
  patterns: [server-dto-driven full-lesson preview, block-scoped refinement without lesson-body writeback]
key-files:
  created: [.planning/phases/08-full-lesson-draft-orchestration/08-full-lesson-draft-orchestration-02-SUMMARY.md]
  modified:
    - teacher/courseedit.aspx
    - js/courseedit.js
    - teacher/aiprovider_api.ashx
    - Tests/TeacherRegressionTests/TeacherRegressionTests.cs
key-decisions:
  - "整课预览继续沿用 courseedit 右侧助手面板，但把 section card 改为按顺序渲染的 block card。"
  - "整课 remove/regenerate 只操作 fullLesson 草案与 saved draft，不触碰 mcontent，保持教师显式应用原则。"
patterns-established:
  - "Pattern 1: 课程编辑页的整课草案一律从服务端 DTO 渲染 blockType、teachingPurpose、lessonPosition、minutes。"
  - "Pattern 2: block 级 refine 请求统一以 blockKey 为作用域，并要求其余 block 在本地/服务端返回中保持稳定。"
requirements-completed: [ORCH-02, ORCH-03]
duration: 15 min
completed: 2026-04-11
---

# Phase 08 Plan 02: Full-lesson draft orchestration Summary

**教师现在可以在 `teacher/courseedit.aspx` 里查看按顺序排列的整课环节卡片，并按 `blockKey` 单独移除、重生成和恢复草案，同时保持学案正文不被静默改写。**

## Performance

- **Duration:** 15 min
- **Started:** 2026-04-11T12:04:19Z
- **Completed:** 2026-04-11T12:19:18Z
- **Tasks:** 2
- **Files modified:** 4

## Accomplishments
- 将课程编辑器预览从 section 视图切换为 ordered full-lesson block cards，并展示活动类型、教学目的、课堂位置、时长等核心元数据。
- 打通整课草案的保存、恢复、删除、复制以及 block 级 remove/regenerate 前端交互。
- 保持整课草案预览优先：整课交互不会自动写入 `mcontent`，也不会在本阶段直接触发 publish。

## Task Commits

Each task was committed atomically:

1. **Task 1: Replace section preview with ordered full-lesson block cards** - `fb9e706` (feat)
2. **Task 2: Add block-level remove and regenerate interactions without auto-apply** - `440f68f` (feat)

## Files Created/Modified
- `teacher/courseedit.aspx` - 增加整课 block 卡片样式、预览说明和整课草案恢复文案。
- `js/courseedit.js` - 切换到 full-lesson 生成/保存/恢复主路径，渲染 block card，并实现 `blockKey` 级 remove/regenerate。
- `teacher/aiprovider_api.ashx` - 为整课预览补齐生成与 block 替换响应，使前端整课交互可真正返回 DTO。
- `Tests/TeacherRegressionTests/TeacherRegressionTests.cs` - 固定整课预览元数据、block 级动作、saved-draft 路由和 no-auto-apply 合同。

## Decisions Made
- 用现有单活动生成器结果映射出 Phase 8 所需的整课 block DTO，先满足 UI 合同与逐块调整流程，再把真正多活动编排留给后续阶段深化。
- 整课草案的保存/恢复统一切换到 `fullLesson*` 路径，避免与已发货 `activityPlan*` 单活动流混用状态。

## Deviations from Plan

### Auto-fixed Issues

**1. [Rule 3 - Blocking] 为整课预览补齐 `fullLessonGenerate` / `fullLessonRegenerateBlock` 的可用返回**
- **Found during:** Task 1
- **Issue:** Plan 01 的 handler 仍是 fail-closed 占位返回，Task 1/2 的前端整课预览与 block 重生成无法完成真实交互。
- **Fix:** 在 `teacher/aiprovider_api.ashx` 中把现有 activity-plan 生成结果映射为 `FullLessonDraft`，并按 `blockKey` 返回替换后的整课草案 DTO。
- **Files modified:** `teacher/aiprovider_api.ashx`
- **Verification:** `dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj --filter CourseEdit`（net8.0 10/10 通过）
- **Committed in:** `fb9e706`

---

**Total deviations:** 1 auto-fixed (1 blocking)
**Impact on plan:** 该修正是完成整课 UI 合同与 block 级交互的必要前置，没有扩大到 publish 或 student 运行时范围。

## Issues Encountered
- `dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj --filter CourseEdit` 仍会在 net48 切片因缺少 `testhost.net48.exe` 中止；本次 net8.0 切片 10 个 `CourseEdit` 相关测试全部通过，因此以可运行切片完成自动验证，并保留 net48 环境问题给后续统一修复。
- 代码库当前工作树存在大量与本计划无关的 `.planning` 历史文件删除/未跟踪内容，本次提交仅精确 stage 本计划相关文件，避免污染任务提交。

## User Setup Required

None - no external service configuration required.

## Next Phase Readiness
- 已具备教师侧整课预览、保存恢复、逐块 refine 的基础，可在后续阶段继续接入更真实的多活动编排与 publish 扇出。
- 后续 phase 需要把当前“由单活动结果映射整课 DTO”的过渡实现替换为真正的 full-lesson generation contract。

## Execution Notes
- 按用户明确要求，本次未更新 `.planning/STATE.md` 与 `.planning/ROADMAP.md`；共享追踪由 orchestrator 在 wave 完成后统一处理。

## Self-Check: PASSED
- Summary 文件已生成：`.planning/phases/08-full-lesson-draft-orchestration/08-full-lesson-draft-orchestration-02-SUMMARY.md`
- 任务提交已存在：`fb9e706`、`440f68f`
