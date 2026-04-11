---
phase: 09-existing-activity-block-composition
plan: 01
subsystem: api
tags: [aspnet-webforms, csharp, full-lesson, existing-activity-blocks]
requires:
  - phase: 08-full-lesson-draft-orchestration
    provides: full-lesson DTO, saved-draft envelope, preview-first fullLesson routes
provides:
  - typed payload contracts for quiz, resource-study, and webCourseware blocks
  - fail-closed validation for supported existing-activity payloads
  - preview-only fullLesson generate/load/save/regenerate responses that preserve typed payloads
affects: [09-02, future-compose-publish, teacher-course-editor-preview]
tech-stack:
  added: []
  patterns: [typed payloads under stable full-lesson block envelope, preview-only brownfield preparation]
key-files:
  created: [.planning/phases/09-existing-activity-block-composition/09-existing-activity-block-composition-01-SUMMARY.md]
  modified:
    - App_Code/Common/AIActivityPlanDraftHelper.cs
    - teacher/aiprovider_api.ashx
    - Tests/CommonLogicTests/CommonLogicTests.cs
    - Tests/TeacherRegressionTests/TeacherRegressionTests.cs
key-decisions:
  - "Phase 9 继续复用 Phase 8 的 block envelope，只在 block 下追加 typed payload，而不引入新的 client-only shape。"
  - "`resource-study` 锁定 mission reading 语义（`Ltype = 6`），`webCourseware` 锁定 ware 语义（`Mcategory = 38`, `Mfiletype = \"ware\"`, `Ltype = 38`），`quiz` 锁定 exam 语义（`Ltype = 39`）。"
patterns-established:
  - "Pattern 1: `quiz`/`resource-study`/`webCourseware` 的最小 brownfield 字段都必须存在，否则 full-lesson draft fail closed。"
  - "Pattern 2: typed payload 会随 full-lesson save/load/regenerate 一起 round-trip，但 Phase 9 仍不创建 `Mission`、`Exam`、`ListMenu` 行。"
requirements-completed: [COMP-01, COMP-02, COMP-03]
duration: 22 min
completed: 2026-04-11
---

# Phase 09 Plan 01: Existing activity block composition Summary

**整课草案现在能在服务端安全承载 `quiz`、`resource-study`、`webCourseware` 三类既有活动 payload，并保持 preview-only，不落库到 brownfield 发布表。**

## Performance

- **Duration:** 22 min
- **Tasks:** 2
- **Files modified:** 4

## Accomplishments

- 在 `FullLessonDraftBlock` 下新增三种 typed payload，并为每种类型补上独立解析与 fail-closed 校验。
- 让 `fullLessonGenerate` / `fullLessonRegenerateBlock` / `fullLessonLoadDraft` 返回 typed payload，且重生成时保留 block 级 typed 数据。
- 用测试固定 quiz/resource-study/webCourseware 的最小字段要求、saved-draft round-trip，以及 preview-only handler 边界。

## Files Created/Modified

- `App_Code/Common/AIActivityPlanDraftHelper.cs` - 增加 typed payload DTO、解析分支和类型校验。
- `teacher/aiprovider_api.ashx` - 生成 quiz/resource-study/webCourseware typed payload，序列化到 full-lesson 响应，并在 block replace 时保留 typed 内容。
- `Tests/CommonLogicTests/CommonLogicTests.cs` - 增加 typed payload contract、reject partial payload、saved-draft round-trip 覆盖。
- `Tests/TeacherRegressionTests/TeacherRegressionTests.cs` - 锁定 Phase 9 支持类型、typed 字段输出和 no-row-creation 边界。

## Verification

- `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj --filter FullLesson`
  - net8.0: **Passed (6/6)**
  - net48: 环境缺少 `testhost.net48.exe`，测试宿主中止
- `dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj --filter CourseEdit`
  - net8.0: **Passed (10/10)**
  - net48: 环境缺少 `testhost.net48.exe`，测试宿主中止

## Issues Encountered

- 本地 `dotnet test` 的 net48 切片仍因环境缺少 `testhost.net48.exe` 无法执行；本次以 net8.0 可运行切片完成自动验证。

## Next Phase Readiness

- `09-02` 可以直接消费现有 full-lesson DTO 中的 `quiz`、`resourceStudy`、`webCourseware` 字段，为课程编辑页渲染 type-aware preview。
- Phase 10 以后若做真实 publish fan-out，可直接复用本次锁定的 brownfield 字段名，减少转换层。

## Execution Notes

- 本次没有改动 `activityPlanPublish` 或任何实际 `Mission` / `Exam` / `ListMenu` 创建路径。
- 本次没有更新 `.planning/STATE.md` 与 `.planning/ROADMAP.md`。

## Self-Check: PASSED

- Summary 文件已生成：`.planning/phases/09-existing-activity-block-composition/09-existing-activity-block-composition-01-SUMMARY.md`
