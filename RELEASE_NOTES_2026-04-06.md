# 版本发布说明 - 2026-04-06

## 概要

- 新增 AI 量规生成与学生测验评估流程，支持实时进度反馈和教师端详情查看。
- 升级测验、预览和问卷体验，新增问卷设置中心，并提升提交流程稳定性。
- 改进学生活动页、`iframe` 活动处理、像素画编辑器，以及数据库升级中心。

## 重点更新

### AI 量规与测验评估

- 在 `teacher/gauge.aspx` 中新增 AI 量规生成，并支持基于 SSE 的实时进度展示。
- 在 `teacher/gaugeitem.aspx` 中新增重新生成和追加生成操作。
- 新增学生测验 AI 评估能力，可保存摘要、建议和学习日志。
- 在 `teacher/start.aspx` 中新增教师端 AI 评估详情查看。

### 测验与问卷体验

- 新增 `teacher/surveysettings.aspx`，用于集中配置问卷设置。
- 重做 `student/myexam.aspx`、`webform/exam.aspx`、`webform/preview.aspx`
  和 `lessons/presurvey.aspx`，统一作答、预览和打印体验。
- 提升问卷与测验的提交、评分和读取流程稳定性。

### 活动页与编辑器

- 新增 `CustomActivityCatalog` 与 `IframeUrlHelper`，提升活动分类和
  `iframe` 页面处理能力。
- 升级像素画创建、编辑和预览流程。
- 统一多类学生活动页与 AI 工具页的工具栏和交互体验。

### 升级中心

- 在 `upgrade.aspx` 中新增数据库升级预检查、迁移可视化和风险提示。
- 旧升级链执行后，自动补跑待执行迁移。
- 优化长迁移列表和高风险场景下的升级页交互。

## 修复项

- 优化 AI 量规重生成安全性，改为先生成后替换。
- 将教师端测验答题记录从原始 JSON 转为更易读的结果展示。
- 修复教师页缺失编辑器 CSS 的问题。
- 将多项前端静态依赖切换为本地资源，减少 CDN 依赖。
- 在数据库尚未创建 `AIStudentExamAssessment` 表时增加兼容处理。
- 修复问卷映射、测验提交、活动目录、课程展示和升级流程中的边界问题。

## 重点变更区域

- AI 功能：`teacher/gauge*`、`teacher/start.aspx`、`student/myexam.aspx`
- 问卷与试卷：`teacher/surveysettings.aspx`、`lessons/presurvey.aspx`、
  `webform/exam.aspx`、`webform/preview.aspx`
- 活动页与编辑器：`student/iframe.aspx`、`teacher/pixel*.aspx`
- 后端与迁移：`App_Code/Common/*`、`App_Code/Dal/Survey.cs`、
  `App_Code/Bll/AIStudentExamAssessment.cs`、
  `App_Code/Utility/DbMigration.cs`
