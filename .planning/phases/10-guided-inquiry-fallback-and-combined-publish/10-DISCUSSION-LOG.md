# Phase 10 Discussion Log

**Date:** 2026-04-11
**Mode:** discuss

## Inputs reviewed

- `.planning/ROADMAP.md`
- `.planning/REQUIREMENTS.md`
- `.planning/STATE.md`
- `.planning/PROJECT.md`
- `.planning/phases/09-existing-activity-block-composition/09-CONTEXT.md`
- `.planning/phases/09-existing-activity-block-composition/09-DISCUSSION-LOG.md`
- `App_Code/Common/AIActivityPlanDraftHelper.cs`
- `App_Code/Common/AIActivityPlanPublishContentBuilder.cs`
- `App_Code/Dal/AIActivityPlanPublisher.cs`
- `teacher/aiprovider_api.ashx`
- `teacher/courseedit.aspx`
- `js/courseedit.js`
- `student/showmission.aspx.cs`
- `Tests/TeacherRegressionTests/TeacherRegressionTests.cs`

## Gray areas reviewed

1. `guidedInquiry` 应该复用哪条现有发布与学生运行路径？
2. mixed full-lesson publish 应该如何避免重复插入已有 `Mission`、`Exam`、   `ListMenu` 行？
3. Phase 10 的发布动作是否应该沿用当前课程编辑页的显式确认交互，而不是新增独立入口？

## Decisions captured

1. `guidedInquiry` 作为 fallback block type，发布时复用当前 AI activity
   mission 路径，而不是新建学生页面。
2. Phase 10 需要新增并行的 `fullLessonPublish` 服务端动作，不能继续把整课
   mixed draft 塞进单活动的 `activityPlanPublish` 合同里。
3. mixed publish 必须按 `blockKey` 保留服务端 publish link，支持 create or
   update，而不是依赖标题匹配或每次都盲目插入新记录。
4. 当前课程编辑页的 publish 行可以继续作为确认入口，但 generate/save/
   resume/remove/regenerate 仍保持 preview-first，不提前写入正文或发布表。

## Planning implications

1. Plan 01 先把 `guidedInquiry` 的 typed payload、fallback 生成和整课预览补齐。
2. Plan 02 再落 mixed publish 事务、publish link 持久化和 UI/handler 接线。
3. Phase 11 继续承担学生侧 composed runtime 与 progress visibility，而不是把
   Phase 10 扩成完整运行时重构。
