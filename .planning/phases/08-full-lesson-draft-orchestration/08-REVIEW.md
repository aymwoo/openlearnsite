---
phase: 08-full-lesson-draft-orchestration
reviewed: 2026-04-11T12:30:00Z
depth: standard
files_reviewed: 7
files_reviewed_list:
  - App_Code/Common/AIActivityPlanDraftHelper.cs
  - App_Code/Common/AIActivityPlanSavedDraftHelper.cs
  - teacher/aiprovider_api.ashx
  - teacher/courseedit.aspx
  - js/courseedit.js
  - Tests/CommonLogicTests/CommonLogicTests.cs
  - Tests/TeacherRegressionTests/TeacherRegressionTests.cs
findings:
  critical: 0
  warning: 2
  info: 1
  total: 3
status: issues_found
---

# Phase 08: Code Review Report

**Reviewed:** 2026-04-11T12:30:00Z
**Depth:** standard
**Files Reviewed:** 7
**Status:** issues_found

## Summary

本次 review 覆盖了 phase 08 的整课草案服务端 DTO、保存/恢复路由、教师端预览交互与相关回归测试。整体实现延续了 fail-closed 和授权复用思路，前端也基本避免了直接 HTML 注入；但整课块级重生成存在明显正确性问题，且整课总时长汇总与实际 block 展示可能不一致，当前测试也没有覆盖这两类回归。

## Warnings

### WR-01: Block 重生成会把目标环节替换成“新草案的第一个环节”

**File:** `teacher/aiprovider_api.ashx:1254-1284`
**Issue:** `ReplaceFullLessonBlock(...)` 固定使用 `generatedDraft.Blocks.FirstOrDefault()` 作为 replacement，而不是按请求的 `blockKey` / block 类型去选对应环节。这样当教师重生成 `practice-1`、`resource-study-1` 等非首块时，返回内容可能被错误替换成“教学目标对齐”之类的首块内容，只是保留了旧的 `blockKey` 和部分旧元数据，导致整课草案语义错位。
**Fix:** 让服务端按目标 block 身份选 replacement，而不是取第一个 block；同时补一条覆盖“重生成非首块”的回归测试。

```csharp
LearnSite.Common.FullLessonDraftBlock currentBlock = currentDraft.Blocks
    .FirstOrDefault(block => string.Equals(block.BlockKey, blockKey, StringComparison.OrdinalIgnoreCase));

LearnSite.Common.FullLessonDraftBlock replacement = generatedDraft.Blocks
    .FirstOrDefault(block => string.Equals(block.BlockKey, blockKey, StringComparison.OrdinalIgnoreCase))
    ?? generatedDraft.Blocks.FirstOrDefault(block =>
        string.Equals(block.BlockType, currentBlock.BlockType, StringComparison.OrdinalIgnoreCase)
        && string.Equals(block.LessonPosition, currentBlock.LessonPosition, StringComparison.OrdinalIgnoreCase));

if (currentBlock == null || replacement == null)
{
    return null;
}
```

### WR-02: 未显式传入时长时，整课总时长会低估实际 block 总和

**File:** `teacher/aiprovider_api.ashx:1159-1223,1328-1355`
**Issue:** `BuildFullLessonDraftFromActivityPlan(...)` 会额外插入固定 5 分钟的“教学目标对齐”和“资源学习支持”块，但 `GetFullLessonTotalMinutes(...)` 只累加 `ActivitySteps` 的分钟数。结果是未填写 `duration` 时，顶部 `totalMinutes` 可能小于实际渲染出来的 block 总和，造成教师预览信息不一致。
**Fix:** 在整课 block 完成构建后，从最终 `fullLessonDraft.Blocks` 统一回算总时长，或把这些服务端补充 block 的分钟数纳入累计逻辑。

```csharp
private string GetFullLessonTotalMinutesFromBlocks(List<LearnSite.Common.FullLessonDraftBlock> blocks)
{
    int total = 0;
    foreach (var block in blocks)
    {
        string digits = new string((block.Minutes ?? string.Empty).Where(char.IsDigit).ToArray());
        if (int.TryParse(digits, out int minutes))
        {
            total += minutes;
        }
    }

    return total > 0 ? total.ToString() + "分钟" : "40分钟";
}
```

## Info

### IN-01: 当前测试没有锁定“非首块重生成保持块身份”的回归

**File:** `Tests/TeacherRegressionTests/TeacherRegressionTests.cs:828-843`
**Issue:** 现有测试验证了 `blockKey` 路由、按钮命名和 no-auto-apply 约束，但没有断言“请求重生成 `practice-1` 时，返回的仍然是 practice block，而不是首块内容”。这让 WR-01 这类错位问题可以在字符串回归测试全部通过的情况下漏出。
**Fix:** 新增一条 CommonLogic 或 handler 级测试，构造至少 3 个 block 的整课草案，断言 `ReplaceFullLessonBlock` / `fullLessonRegenerateBlock` 会替换目标块且保持其它块顺序与身份不变。

---

_Reviewed: 2026-04-11T12:30:00Z_
_Reviewer: the agent (gsd-code-reviewer)_
_Depth: standard_
