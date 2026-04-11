---
status: in_progress
trigger: "/gsd-quick 把当前 webCourseware 从‘占位入口页’升级成‘更丰富、可帮助学生学习的网页课件’。要求：1) 仍沿用现有 webCourseware 发布链路、student/ware.aspx 入口和 Mback 方式；2) 用最小扩展增强 payload，使生成阶段能携带更丰富的课件内容，例如 lesson summary、学习目标、讲解卡片、关键词、互动小测或思考题；3) 更新 `teacher/aiprovider_api.ashx` 的 webCourseware payload 生成与序列化；4) 更新相关 helper 的解析/克隆/校验，确保保存、恢复、发布链路可用；5) 重写 `ai/courseware/preview.html`，根据 query 参数或 payload 映射展示更像真实教学课件的页面，至少包含：主题标题、知识点讲解、步骤/示例、互动练习或思考题、课堂小结；6) 维持 brownfield 兼容，不新增新的学生 runtime 页面；7) 补最小测试锁定。"
created: 2026-04-11T00:00:00Z
updated: 2026-04-11T00:00:00Z
---

## Current Focus

hypothesis: 通过扩展 `WebCoursewareBlockPayload` 并在发布时映射为短 query 参数，可在不改 brownfield runtime 入口的前提下让 `student/ware.aspx -> Mission.Mback -> ai/courseware/preview.html` 呈现更完整的网页课件
test: CommonLogicTests + TeacherRegressionTests 中的 webCourseware 解析、保存、源码回归断言
expecting: 生成/保存/恢复/发布链路继续沿用 `Mback`，但 richer payload 字段不丢失，preview 页面能展示真实教学课件结构
next_action: 实现 payload 扩展、preview 渲染和最小测试

## Constraints

- brownfield 兼容：仍使用 `student/ware.aspx` 与 `Mission.Mback`
- 不新增学生 runtime 页面
- `Mission.Mback` 列为旧库短字符串字段，发布时不能依赖长 JSON URL
