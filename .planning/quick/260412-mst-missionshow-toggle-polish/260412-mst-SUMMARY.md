---
quick_id: 260412-mst
title: 美化 teacher/missionshow.aspx 中 Markdown 渲染和 Reveal 演示文稿卡片右侧的开关按钮
status: completed
completed: 2026-04-12
files_changed:
  - App_Themes/Teacher/missionshow.css
verification:
  - git diff --check -- App_Themes/Teacher/missionshow.css
---

# Quick Task 260412-mst Summary

## Delivered

- 保持 `teacher/missionshow.aspx` 现有 toggle 按钮结构与 `js/missionshow.js` 的 `is-on` 切换逻辑不变，仅在 `App_Themes/Teacher/missionshow.css` 上做最小视觉增强。
- 提升了 toggle 卡片阴影与状态条层次，让 Markdown 渲染和 Reveal 演示文稿两个紫蓝色卡片更贴近当前页面风格。
- 重绘了开关轨道、边框、阴影、`ON/OFF` 文案和 knob 质感，并补充 `:hover` 与 `:focus-visible`，使 on/off 状态更清楚。

## Verification

- `git diff --check -- App_Themes/Teacher/missionshow.css`

## Notes

- 本次未改动 `teacher/missionshow.aspx` 结构，因为当前 DOM 和状态类名已足够支撑最小样式升级。
