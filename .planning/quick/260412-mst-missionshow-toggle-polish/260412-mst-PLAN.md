---
quick_id: 260412-mst
title: 美化 teacher/missionshow.aspx 中 Markdown 渲染和 Reveal 演示文稿卡片右侧的开关按钮
status: ready
created: 2026-04-12
type: quick
files_expected:
  - teacher/missionshow.aspx
  - App_Themes/Teacher/missionshow.css
must_haves:
  - 保持 `teacher/missionshow.aspx` 现有 toggle 结构与交互脚本兼容。
  - 对 `App_Themes/Teacher/missionshow.css` 做最小视觉增强，让开关更精致、层次更清晰，并延续紫蓝色卡片风格。
  - on/off 状态继续明确可辨识，且补充焦点可见性。
---

# Quick Task 260412-mst Plan

## Task 1

检查 `teacher/missionshow.aspx` 中 Markdown 与 Reveal toggle 的 DOM 结构、状态类名和可复用属性，确认不需要调整标记即可完成样式升级。

## Task 2

更新 `App_Themes/Teacher/missionshow.css` 中 toggle 卡片与开关按钮样式：
- 提升卡片与状态条层次；
- 优化 switch 轨道、边框、阴影和 knob 质感；
- 强化 `is-on` / 默认关闭态和 `:focus-visible` 表现。

## Task 3

执行最小校验，确认样式文件 diff 无格式问题，并记录 quick summary。
