---
quick_id: 260412-ahe
title: 美化 teacher/courseedit.aspx 中的活动计划助手区域：移动到内容编辑器右侧；美化学案名称、学案分类、授课年级区块并调整 select/input 尺寸；切换到 vditor 时自动进入分屏预览状态
status: ready
created: 2026-04-11
type: quick
files_expected:
  - teacher/courseedit.aspx
  - App_Themes/Teacher/courseedit.css
  - js/courseedit.js
must_haves:
  - 活动计划助手在桌面端稳定显示于内容编辑器右侧，移动端自然折叠为单列。
  - 学案名称、学案分类、授课年级等顶部设置项更紧凑，input/select 高度和宽度更统一，并保留未来横向扩展空间。
  - 切换到 vditor 时自动进入分屏预览，而不影响保存链路。
  - quick 工作流产物、STATE.md 更新与原子提交完整。
---

# Quick Task 260412-ahe Plan

## Task 1

调整 `teacher/courseedit.aspx` 顶部设置区与编辑器/助手区结构标记，给关键字段补上更明确的布局类名，确保视觉改造只在当前页面生效。

## Task 2

更新 `App_Themes/Teacher/courseedit.css`：
- 将顶部设置区改为更紧凑的可扩展网格；
- 统一 `input` / `select` 高度、宽度和间距；
- 明确编辑器与“活动计划助手”双栏布局，并补充桌面/移动端响应式表现。

## Task 3

更新 `js/courseedit.js` 中的 Vditor 初始化与切换逻辑，确保切到 `vditor` 时自动保持分屏预览状态，并验证保存仍同步 markdown 内容。
