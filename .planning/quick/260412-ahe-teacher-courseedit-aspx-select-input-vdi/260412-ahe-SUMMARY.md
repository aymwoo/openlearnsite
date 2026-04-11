---
quick_id: 260412-ahe
title: 美化 teacher/courseedit.aspx 中的活动计划助手区域：移动到内容编辑器右侧；美化学案名称、学案分类、授课年级区块并调整 select/input 尺寸；切换到 vditor 时自动进入分屏预览状态
status: completed
completed: 2026-04-11
files_changed:
  - teacher/courseedit.aspx
  - App_Themes/Teacher/courseedit.css
  - js/courseedit.js
verification:
  - git diff --check -- teacher/courseedit.aspx App_Themes/Teacher/courseedit.css js/courseedit.js
  - node -e "const fs=require('fs'); new Function(fs.readFileSync('js/courseedit.js','utf8'));"
---

# Quick Task 260412-ahe Summary

## Delivered

- 将 `teacher/courseedit.aspx` 顶部设置区补充为更明确的紧凑布局类，便于后续继续横向扩展更多设置项。
- 在 `App_Themes/Teacher/courseedit.css` 中把顶部设置区改为更紧凑的响应式网格，统一 `input/select` 高度与宽度，并让“活动计划助手”在桌面端稳定停靠到编辑器右侧、在窄屏下回落为单列。
- 在 `js/courseedit.js` 中保持 Vditor `preview.mode = 'both'`，确保切换到 `vditor` 时自动进入分屏预览，同时不影响 markdown 同步保存。

## Verification

- `git diff --check -- teacher/courseedit.aspx App_Themes/Teacher/courseedit.css js/courseedit.js`
- `node -e "const fs=require('fs'); new Function(fs.readFileSync('js/courseedit.js','utf8'));"`

## Notes

- 本次未做浏览器实机截图验证；布局正确性主要基于现有 DOM 结构、主题样式和脚本最小改动校验。
- 本次 quick 任务采用代码提交与文档提交分离的方式，满足原子提交边界。
