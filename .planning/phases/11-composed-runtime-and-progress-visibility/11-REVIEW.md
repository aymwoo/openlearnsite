---
phase: 11-composed-runtime-and-progress-visibility
reviewed: 2026-04-11T23:50:57Z
depth: standard
files_reviewed: 3
files_reviewed_list:
  - teacher/courseedit.aspx
  - App_Themes/Teacher/courseedit.css
  - js/courseedit.js
findings:
  critical: 0
  warning: 2
  info: 0
  total: 2
status: issues_found
---

# Phase 11: Code Review Report

**Reviewed:** 2026-04-11T23:50:57Z
**Depth:** standard
**Files Reviewed:** 3
**Status:** issues_found

## Summary

本次评审覆盖了 `teacher/courseedit.aspx`、`App_Themes/Teacher/courseedit.css` 与 `js/courseedit.js`，重点核对了三个宣称效果：活动计划助手是否移到编辑器右侧、元数据区是否明显更紧凑、切换到 Vditor 时是否自动进入分栏预览。

结论：右侧双栏布局本身已实现，但另外两个目标没有真正落地。元数据区虽然改成了网格排布，但“compact”相关类在样式中没有任何差异化实现；Vditor 仍以 `ir` 模式初始化，只是修改了 `preview.mode` 配置值，无法保证进入真正的左右分栏预览，因此与用户反馈一致，当前实现未真正达到宣称 UI 效果。

## Warnings

### WR-01: Vditor 仍以 IR 模式初始化，无法保证进入真正的 split preview

**File:** `js/courseedit.js:151-164, 193-203`
**Issue:** 代码在切换到 Vditor 时宣称会自动进入分栏预览，但实际初始化仍是 `mode: 'ir'`，随后只是在配置对象上把 `preview.mode` 改成 `'both'`。`ir` 模式本身不是标准的左右分栏编辑模式，因此这段逻辑并不能确保界面出现“编辑区 + 预览区”分栏，结果就是用户切换到 Vditor 后看不到预期效果。
**Fix:** 如果需求是“切换到 Vditor 自动进入分栏预览”，应直接用支持分栏预览的模式初始化，而不是在 `ir` 模式上改配置值。例如：
```javascript
vditorObj = new Vditor('vditor-container', {
  height: 400,
  width: '100%',
  mode: 'sv',
  preview: { mode: 'both' },
  cache: { enable: false },
  after: () => {
    vditorReady = true;
    vditorObj.setValue(contentToSet || '');
    rememberVditorState();
  }
});
```
如果产品决定继续保留 `ir` 模式，就不应再宣称“自动进入 split preview mode”，而应同步修改文案与验收标准。

### WR-02: “compact” 元数据布局只改了类名，未实现真正的紧凑样式

**File:** `teacher/courseedit.aspx:430-459`, `App_Themes/Teacher/courseedit.css:103-116`
**Issue:** 页面把多个元数据字段标成了 `course-edit-field-compact`、`course-edit-field-class`、`course-edit-field-grade` 等类，明显意图让信息区更紧凑；但样式表只定义了统一的 `.course-edit-field` 盒模型，没有任何对应的 compact/分类/年级差异化规则。结果这些字段仍沿用与普通字段相同的 padding、标签间距和控件高度，视觉密度并没有明显下降，和“metadata area more compact”的目标不一致。
**Fix:** 为 compact 字段补充专门样式，至少压缩内边距、标签间距和控件高度。例如：
```css
.course-edit-field-compact {
  padding: 0.55rem 0.7rem;
}

.course-edit-field-compact .course-edit-label {
  margin-bottom: 0.25rem;
  font-size: 11px;
}

.course-edit-field-compact .course-edit-input,
.course-edit-field-compact .course-edit-select,
.course-edit-field-compact .course-edit-static {
  height: 34px;
  min-height: 34px;
}
```
如果还希望标题字段更突出、分类/年级更窄，也应为 `course-edit-field-title`、`course-edit-field-class`、`course-edit-field-grade` 增加明确规则，而不是只在标记里保留无效类名。

---

_Reviewed: 2026-04-11T23:50:57Z_
_Reviewer: the agent (gsd-code-reviewer)_
_Depth: standard_
