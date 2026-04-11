# Phase 11 Discussion Log

**Date:** 2026-04-11
**Mode:** discuss

## Inputs reviewed

- `.planning/ROADMAP.md`
- `.planning/REQUIREMENTS.md`
- `.planning/STATE.md`
- `.planning/PROJECT.md`
- `.planning/phases/10-guided-inquiry-fallback-and-combined-publish/10-CONTEXT.md`
- `.planning/phases/10-guided-inquiry-fallback-and-combined-publish/10-DISCUSSION-LOG.md`
- `.planning/phases/10-guided-inquiry-fallback-and-combined-publish/10-guided-inquiry-fallback-and-combined-publish-02-SUMMARY.md`
- `App_Code/Common/AIActivityPlanSavedDraftHelper.cs`
- `App_Code/Dal/AIActivityPlanPublisher.cs`
- `App_Code/Model/AIActivityPlanPublishResult.cs`
- `student/Scm.master.cs`
- `student/showmission.aspx.cs`
- `student/myinfo.aspx.cs`
- `teacher/courseshow.aspx.cs`
- `App_Code/Bll/MenuWorks.cs`
- `App_Code/Bll/Works.cs`
- `Tests/TeacherRegressionTests/TeacherRegressionTests.cs`

## Gray areas reviewed

1. Phase 11 是否需要新增独立的 composed student runtime 页面，还是继续复用
   `ListMenu` + 现有活动页？
2. 混合整课发布后的 block-level progress 应该依赖什么作为可信完成证据？
3. teacher/student 侧如何识别“这些 brownfield 活动属于同一份 composed
   lesson publish”，而不是继续只看零散的 menu 项？

## Decisions captured

1. Phase 11 继续复用现有 `student/Scm.master.cs` 导航和各类型运行页，不新增
   AI 专属 student player。
2. block-level completion 继续落在既有 brownfield 证据上：`MenuWorks`、
   mission upload/`Works`、既有 exam 完成路径；不引入 AI-only completion 表。
3. `publishLinks` 已是稳定的 `blockKey -> brownfield ids` 关系源，Phase 11
   应优先围绕它构建 composed runtime/progress helper，而不是用标题匹配。
4. teacher/student 侧的“整课进度可见性”优先做最小关联与摘要增强，而不是大改
   现有课程页、统计页或做全新 dashboard。

## Planning implications

1. Plan 01 先建立 composed publish/runtime summary helper，把 block 元数据、
   运行目标、发布 ids 和完成状态做成可复用的服务端结构，并补 tests。
2. Plan 02 再把这个结构接入学生菜单/学习汇总和教师课程显示等现有页面，补齐
   per-block completion visibility。
3. 若某些 brownfield 类型完成证据不足，Phase 11 应选择 fail-closed 的“未完成/
   未知”表现，而不是猜测完成状态。
