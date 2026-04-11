---
phase: 07-submission-and-completion-tracking
reviewed: 2026-04-11T10:13:49Z
depth: standard
files_reviewed: 9
files_reviewed_list:
  - student/showmission.aspx.cs
  - student/uploadwork.aspx.cs
  - student/uploadworkm.aspx.cs
  - App_Code/Bll/MenuWorks.cs
  - App_Code/Dal/MenuWorks.cs
  - App_Code/Bll/Works.cs
  - Tests/TeacherRegressionTests/TeacherRegressionTests.cs
  - Tests/CommonLogicTests/CommonLogicTests.cs
  - Tests/CommonLogicTests/CommonLogicTests.csproj
findings:
  critical: 0
  warning: 4
  info: 0
  total: 4
status: issues_found
---

# Phase 07: Code Review Report

**Reviewed:** 2026-04-11T10:13:49Z
**Depth:** standard
**Files Reviewed:** 9
**Status:** issues_found

## Summary

Reviewed the phase 07 submission/completion changes plus their regression coverage. The phase keeps the brownfield upload flow and completion wiring, but there are still correctness gaps in the touched upload handlers and one regression in the new/updated tests.

## Warnings

### WR-01: `uploadworkm` size guard is bypassed and can null-deref

**File:** `student/uploadworkm.aspx.cs:87-90`
**Issue:** The condition uses `||` instead of `&&`:

```csharp
if (work_upload.InputStream != null || work_upload.InputStream.Length < maxSize)
```

When `InputStream` is non-null, oversized files are still accepted; when it is null, the right side can throw. This defeats the intended upload size restriction.

**Fix:**
```csharp
if (work_upload.InputStream != null && work_upload.ContentLength < maxSize)
{
    ...
}
else
{
    showError("选择的文件大小超过限制!(最大为100MB)");
}
```

### WR-02: Primary upload handler still does not enforce mission file type

**File:** `student/uploadwork.aspx.cs:53-57`
**Issue:** The handler resolves `Mission.Mfiletype` but never validates the uploaded extension against it before saving. Phase 07 explicitly relies on preserving existing upload/file-type rules, but this path still accepts arbitrary extensions.

**Fix:** Reuse the same extension check already present in `uploadworkm.aspx.cs` (after fixing its guard), or extract a shared validator so both handlers reject mismatched file types before `AddWorkUp(...)` / `SaveAs(...)`.

### WR-03: Duplicate-safe completion write is not atomic

**File:** `App_Code/Bll/MenuWorks.cs:76-89`, `App_Code/Dal/MenuWorks.cs:126-156`
**Issue:** The new duplicate-prevention logic is still check-then-insert without a unique DB constraint or transactional insert. Two concurrent submissions can both pass `GetModelme(...)` / `Exists(...)` and insert duplicate `MenuWorks` rows, so the “one record per student activity” guarantee is not actually enforced under race conditions.

**Fix:** Add a unique constraint/index on `(Klid, Ksid)` and make insert idempotent by handling duplicate-key failures, or replace the separate existence check with a single transactional conditional insert.

### WR-04: Regression tests still assert the pre-refactor completion code

**File:** `Tests/TeacherRegressionTests/TeacherRegressionTests.cs:847-848`, `Tests/TeacherRegressionTests/TeacherRegressionTests.cs:866-867`
**Issue:** The tests still expect `kmodel.Klid = Int32.Parse(Wlid);` and `kbll.Add(kmodel);`, but the implementation now uses `ws.EnsureMenuWorksCompletion(...)`. That makes the updated regression suite stale and likely failing instead of protecting the new behavior.

**Fix:**
```csharp
Assert.Contains("ws.EnsureMenuWorksCompletion", uploadWork, StringComparison.Ordinal);
Assert.DoesNotContain("kbll.Add(kmodel);", uploadWork, StringComparison.Ordinal);
```

Apply the same update to the `uploadworkm.aspx.cs` assertions.

---

_Reviewed: 2026-04-11T10:13:49Z_
_Reviewer: the agent (gsd-code-reviewer)_
_Depth: standard_
