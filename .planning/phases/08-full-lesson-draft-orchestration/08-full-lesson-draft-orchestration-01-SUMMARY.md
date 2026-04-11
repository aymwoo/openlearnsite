---
phase: 08-full-lesson-draft-orchestration
plan: 01
subsystem: api
tags: [aspnet-webforms, csharp, full-lesson, draft-persistence]
requires:
  - phase: 07-submission-and-completion-tracking
    provides: single-activity activityPlan draft, publish, and regression baseline
provides:
  - typed full-lesson draft parser and validator with ordered block metadata
  - full-lesson saved-draft envelope compatible with the existing one-draft-per-course record
  - parallel authenticated fullLesson* handler actions without rewriting activityPlanPublish
affects: [08-02, full-lesson-preview, future-full-lesson-generation]
tech-stack:
  added: []
  patterns: [parallel fullLesson contract beside activityPlan contract, server-owned full-lesson draft envelope]
key-files:
  created: [.planning/phases/08-full-lesson-draft-orchestration/08-full-lesson-draft-orchestration-01-SUMMARY.md]
  modified:
    - App_Code/Common/AIActivityPlanDraftHelper.cs
    - App_Code/Common/AIActivityPlanSavedDraftHelper.cs
    - teacher/aiprovider_api.ashx
    - Tests/CommonLogicTests/CommonLogicTests.cs
    - Tests/TeacherRegressionTests/TeacherRegressionTests.cs
key-decisions:
  - "为整课草案新增独立 FullLessonDraft/FullLessonDraftBlock 合同，而不是复用 activitySteps 混装旧结构。"
  - "fullLesson* 路由与已发货 activityPlan* 并行扩展，并继续复用 TryGetAuthorizedCourse 做教师/课程授权。"
patterns-established:
  - "Pattern 1: 新整课草案统一要求 blockKey、sort、blockType、teachingPurpose、lessonPosition 五个核心元数据。"
  - "Pattern 2: 保存草案时用 DraftJson 内部 draftType 区分 activityPlan 与 fullLesson，而不改动现有单草案记录模型。"
requirements-completed: [ORCH-01, ORCH-03]
duration: 6 min
completed: 2026-04-11
---

# Phase 08 Plan 01: Full-lesson draft orchestration Summary

**整课草案现在具备服务端强校验的 block 合同、可往返保存的 fullLesson 草稿封装，以及与既有 activityPlan 并行的授权 handler 基础路由。**

## Performance

- **Duration:** 6 min
- **Started:** 2026-04-11T11:55:34Z
- **Completed:** 2026-04-11T12:02:04Z
- **Tasks:** 2
- **Files modified:** 5

## Accomplishments
- 为整课编排新增 `FullLessonDraft` / `FullLessonDraftBlock` 类型，并在服务端拒绝畸形或部分缺失 block。
- 为单课程单教师草稿记录增加 `fullLesson` JSON envelope 的序列化/反序列化能力，同时保留旧 `activityPlan` 合同。
- 在 `teacher/aiprovider_api.ashx` 中加入六个 `fullLesson*` 并行 action，并保持 `activityPlanPublish` 原路径不变。

## Task Commits

Each task was committed atomically:

1. **Task 1 (TDD RED): Add a typed full-lesson draft parser and validator** - `6f8e23d` (test)
2. **Task 1 (TDD GREEN): Add a typed full-lesson draft parser and validator** - `4d0957c` (feat)
3. **Task 2: Add authenticated `fullLesson*` draft actions parallel to the shipped activity-plan actions** - `dbe5f94` (feat)

## Files Created/Modified
- `App_Code/Common/AIActivityPlanDraftHelper.cs` - 新增整课 DTO、block 元数据校验、整课解析入口。
- `App_Code/Common/AIActivityPlanSavedDraftHelper.cs` - 新增 full-lesson 草稿封装、round-trip 解析与 `draftType` 区分。
- `teacher/aiprovider_api.ashx` - 新增 `fullLessonGenerate`、`fullLessonRegenerateBlock`、`fullLessonDraftStatus`、`fullLessonSaveDraft`、`fullLessonLoadDraft`、`fullLessonDeleteDraft`。
- `Tests/CommonLogicTests/CommonLogicTests.cs` - 固定整课解析拒绝条件、元数据要求与草稿往返行为。
- `Tests/TeacherRegressionTests/TeacherRegressionTests.cs` - 固定 full-lesson handler 命名、授权复用与 brownfield 路由安全。

## Decisions Made
- 用新的整课合同承载 block 顺序与元数据，避免把整课块塞回 `activitySteps` 造成后续 remove/regenerate 脆弱化。
- 复用现有 `CourseActivityPlanDraft` 单条记录和 `DraftJson` 字段，通过 envelope 内部 `draftType` 分流，避免本计划引入结构性持久化变更。

## Deviations from Plan

### Auto-fixed Issues

**1. [Rule 2 - Missing Critical] 为整课草案补上 blockKey 去重与顺序校验**
- **Found during:** Task 1
- **Issue:** 仅校验字段存在仍可能让重复 blockKey 或乱序 sort 混入草案，后续 block 级操作会失去稳定定位。
- **Fix:** 在 `IsValidFullLessonDraft` 中增加唯一键集合检查与 `sort == index + 1` 顺序约束。
- **Files modified:** `App_Code/Common/AIActivityPlanDraftHelper.cs`
- **Verification:** `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj --filter FullLesson`
- **Committed in:** `4d0957c`

---

**Total deviations:** 1 auto-fixed (1 missing critical)
**Impact on plan:** 该修正直接满足 T-8-01，对后续 block 级 regenerate/remove 属于必要正确性保障，无额外范围膨胀。

## Issues Encountered
- `dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj --filter CourseEdit` 在当前环境的 net48 切片会因缺少 `testhost.net48.exe` 中止，但 net8.0 切片 8 个相关测试全部通过；本次以已通过的 net8.0 回归结果完成代码验证，并保留该环境问题给后续统一测试环境修复。

## Known Stubs
- `teacher/aiprovider_api.ashx:768` - `fullLessonGenerate` 目前只完成授权入口与失败关闭响应，真正 AI 整课生成器待后续整课预览/编排层接入。
- `teacher/aiprovider_api.ashx:791` - `fullLessonRegenerateBlock` 已完成 blockKey 校验与授权边界，但实际 block 重生成仍返回显式未接线提示。

## User Setup Required

None - no external service configuration required.

## Next Phase Readiness
- Plan 02 可以直接复用当前 `FullLessonDraft` DTO 与 `fullLessonLoadDraft` 返回结构，在课程编辑页渲染 block card 预览。
- 若要真正打通 `fullLessonGenerate` / `fullLessonRegenerateBlock`，需要后续计划补齐整课 prompt/generator，而不是继续复用单活动生成器。

## Execution Notes
- 按用户要求，本次未更新 `.planning/STATE.md` 与 `.planning/ROADMAP.md`；共享追踪交由 orchestrator 在 wave 完成后统一处理。

## Self-Check: PASSED
- Summary 文件已生成：`.planning/phases/08-full-lesson-draft-orchestration/08-full-lesson-draft-orchestration-01-SUMMARY.md`
- 任务提交已存在：`6f8e23d`、`4d0957c`、`dbe5f94`
