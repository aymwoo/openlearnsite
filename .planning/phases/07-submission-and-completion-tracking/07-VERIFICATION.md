---
phase: 07-submission-and-completion-tracking
verified: 2026-04-11T10:20:42Z
status: human_needed
score: 5/6 must-haves verified
overrides_applied: 0
human_verification:
  - test: "学生在 AI 活动页上传一个合法作品文件"
    expected: "页面提示提交成功、刷新后出现已上传作品链接，且不会报活动参数错误或活动不存在"
    why_human: "需要真实登录态、浏览器文件选择、服务器文件写入与数据库联动，当前验证未运行认证上传流程"
  - test: "上传成功后刷新学生与教师完成态相关页面"
    expected: "学生侧当前活动/课程完成状态不再表现为未触达，教师侧课程或作品查看面能区分已提交与未提交"
    why_human: "源码显示完成记录会写入 MenuWorks/Works，但菜单完成图标还受 WorkPass 条件影响，需真实提交后确认最终 UI 呈现"
---

# Phase 7: Submission and completion tracking Verification Report

**Phase Goal:** Let students submit activity results and record reliable completion state.
**Verified:** 2026-04-11T10:20:42Z
**Status:** human_needed
**Re-verification:** No — initial verification

## Goal Achievement

### Observable Truths

| # | Truth | Status | Evidence |
|---|-------|--------|----------|
| 1 | 学生可以从已发布 AI 活动流程提交结果。 | ? UNCERTAIN | `student/showmission.aspx` 保留上传面板；`js/showmission.js` 将 `uploadButton` 绑定到 `uploadworkm.aspx?lid=`；`student/uploadworkm.aspx.cs` 校验 `lid`、解析 `ListMenu.Lxid`、落库 `Works`；相关 9 个 `TeacherRegressionTests` / 3 个 `CommonLogicTests` 通过，但未实际执行认证浏览器上传。 |
| 2 | 成功提交会关联到正确的 lesson activity identity。 | ✓ VERIFIED | `student/uploadwork.aspx.cs` 与 `student/uploadworkm.aspx.cs` 都从 `Request.QueryString["lid"]` 读取活动，先查 `ListMenu.GetModel(Wlid)`，再用 `lmodel.Lxid.Value` 解析 `Mission`，提交记录写入对应 `Wmid`。 |
| 3 | 成功提交后会通过既有课堂完成模型记录完成状态。 | ✓ VERIFIED | 两个上传处理器首次提交后都调用 `Works.EnsureMenuWorksCompletion(Wsid, Wlid, LoginTime, Wdate)`；`Works.EnsureMenuWorksCompletion` 构造 `MenuWorks` 记录并委托 `MenuWorks.EnsureCompletion(...)`。 |
| 4 | 教师侧/状态面能够区分已完成与未触达活动。 | ✓ VERIFIED | `teacher/start.aspx.cs` 的 `Showkc()` 通过 `Works.ShowDoneWorkCids(...)`、`MenuWorks.readCids(...)` 聚合已完成课程；`teacher/workshow.aspx.cs` / `teacher/workcheck.aspx.cs` 分开读取已提交作品与 `ShowTodayNotWorks(...)` 未提交名单。 |
| 5 | 任务页会明确告知当前是否可提交、可重提或已锁定。 | ✓ VERIFIED | `student/showmission.aspx.cs` 明确拆出 `ShowReadyToSubmitState()`、`ShowResubmitState()`、`ShowLockedSubmissionState()`、`ShowIpBlockedState()`、`ShowPreviousWorkRequiredState()`，并直接控制 `Panelswfupload.Visible` 与提示文案。 |
| 6 | 完成写入不会为同一学生/菜单活动产生重复 `MenuWorks` 记录。 | ✓ VERIFIED | `App_Code/Dal/MenuWorks.cs` 的 `Add(...)` 使用 `insert ... where not exists (...)`，并带 `UPDLOCK, HOLDLOCK`；若插入返回 0，仍用 `Exists(model.Ksid.Value, model.Klid.Value)` 视为已存在成功。 |

**Score:** 5/6 truths verified

### Required Artifacts

| Artifact | Expected | Status | Details |
|----------|----------|--------|---------|
| `student/showmission.aspx.cs` | AI 活动提交状态与活动页服务端状态控制 | ✓ VERIFIED | 412 行，包含任务加载、指引渲染、提交/重提/锁定/IP 限制/前序作品限制分支。 |
| `student/showmission.aspx` | 学生上传壳与活动指引 UI | ✓ VERIFIED | 包含 `PanelActivityGuide`、`Panelworks`、`Panelswfupload`、`HiddenMissionRaw`。 |
| `js/showmission.js` | 前端上传按钮与 `lid` 提交流程 | ✓ VERIFIED | `uploadButton` 实际绑定到 `uploadworkm.aspx?lid=`，成功后 reload。 |
| `student/uploadwork.aspx.cs` | 主上传处理器保留 `lid -> ListMenu -> Mission -> Works` 合同 | ✓ VERIFIED | 259 行，包含参数守卫、文件类型检查、提交/重提、首次完成写入。 |
| `student/uploadworkm.aspx.cs` | 活动页实际使用的上传处理器合同 | ✓ VERIFIED | 253 行，含 `lid` 守卫、`Works` 写入、`EnsureMenuWorksCompletion(...)`、JSON 结果返回。 |
| `App_Code/Bll/Works.cs` | 共享 completion helper | ✓ VERIFIED | `EnsureMenuWorksCompletion(...)` 将提交耗时转换为 `MenuWorks` 模型并交给 `MenuWorks.EnsureCompletion(...)`。 |
| `App_Code/Bll/MenuWorks.cs` | Completion BLL 访问层 | ✓ VERIFIED | 提供 `EnsureCompletion(...)`、`GetMyLidCount(...)`、`readCids(...)`。 |
| `App_Code/Dal/MenuWorks.cs` | Duplicate-safe completion persistence | ✓ VERIFIED | `Add(...)` 含空值保护、去重插入和存在性兜底。 |
| `Tests/TeacherRegressionTests/TeacherRegressionTests.cs` | 页面/路由/完成态源码回归 | ✓ VERIFIED | 包含 `UploadWork_*`、`ShowMission_*`、`ActivityPlanCompletion_*` 断言。 |
| `Tests/CommonLogicTests/CommonLogicTests.cs` | Completion helper 逻辑回归 | ✓ VERIFIED | 包含 `ActivityPlanCompletion_MenuWorksSource_*` 与上传处理器共享 helper 断言。 |

### Key Link Verification

| From | To | Via | Status | Details |
|------|----|-----|--------|---------|
| `student/showmission.aspx` + `js/showmission.js` | `student/uploadworkm.aspx.cs` | `uploadworkm.aspx?lid=` | ✓ WIRED | 页面输出 `LabelLid`，脚本用它构造上传地址，提交按钮直接走 `uploadworkm.aspx?lid=`。 |
| `student/uploadwork.aspx.cs` | `ListMenu` / `Mission` / `Works` | `lid -> Lxid -> Mid` | ✓ WIRED | 先查 `ListMenu.GetModel(Wlid)`，再查 `Mission.GetModel(lmodel.Lxid.Value)`，之后 `GetModelByStu` / `AddWorkUp` / `UpdateWorkUp`。 |
| `student/uploadworkm.aspx.cs` | `ListMenu` / `Mission` / `Works` | `lid -> Lxid -> Mid` | ✓ WIRED | 与主处理器保持同一身份解析链路。 |
| `student/uploadwork.aspx.cs` | `App_Code/Bll/Works.cs` | `EnsureMenuWorksCompletion(...)` | ✓ WIRED | 首次提交成功后统一调用共享 helper，而不是直接 `kbll.Add(...)`。 |
| `student/uploadworkm.aspx.cs` | `App_Code/Bll/Works.cs` | `EnsureMenuWorksCompletion(...)` | ✓ WIRED | 备用上传模式也走共享 helper。 |
| `App_Code/Bll/Works.cs` | `App_Code/Bll/MenuWorks.cs` | `EnsureCompletion(...)` | ✓ WIRED | `EnsureMenuWorksCompletion(...)` 内部 new `MenuWorks()` 并调用 `EnsureCompletion(kmodel)`。 |
| `student/Scm.master.cs` | `App_Code/Bll/MenuWorks.cs` | `GetMyLidCount(...)` | ⚠️ PARTIAL | 菜单确实读取 `GetMyLidCount(...)` 参与完成进度/解锁，但完成图标还额外依赖 `Works.WorkPass(...)`，源码无法单独证明“上传成功后当前项必然显示完成图标”。 |
| `student/myinfo.aspx.cs` | `Works` + `MenuWorks` | `ShowStuDoneWorkCids(...)` + `readCids(...)` | ✓ WIRED | 已完成课程汇总使用既有 `Works/MenuWorks` 聚合。 |
| `teacher/start.aspx.cs` | `Works` + `MenuWorks` | `ShowDoneWorkCids(...)` + `readCids(...)` | ✓ WIRED | 教师开始页据此分离已学课程与未学课程。 |

### Data-Flow Trace (Level 4)

| Artifact | Data Variable | Source | Produces Real Data | Status |
|----------|---------------|--------|--------------------|--------|
| `student/showmission.aspx.cs` | `guide` / `Labelmsg` / `Panelswfupload.Visible` | `ListMenu.GetModel` + `Mission.GetModel` + `Works.WorkDone/IpWorkDoneSnum/ExistsMyFirstWork` | Yes | ✓ FLOWING |
| `student/uploadworkm.aspx.cs` | `Wmid` / `wmodel` / completion side effect | `Request.QueryString["lid"]` -> `ListMenu` -> `Mission` -> `Works` / `MenuWorks` | Yes | ✓ FLOWING |
| `student/uploadwork.aspx.cs` | `Wmid` / `wmodel` / completion side effect | `Request.QueryString["lid"]` -> `ListMenu` -> `Mission` -> `Works` / `MenuWorks` | Yes | ✓ FLOWING |
| `App_Code/Bll/Works.cs` | `kmodel` | `loginTime`, `submitTime`, `Wsid`, `Wlid` -> `MenuWorks.EnsureCompletion` | Yes | ✓ FLOWING |
| `student/myinfo.aspx.cs` | `LabelCids.Text` | `Works.ShowStuDoneWorkCids(...)` + `MenuWorks.readCids(...)` | Yes | ✓ FLOWING |
| `student/Scm.master.cs` | `lcount` / `ma.ImageUrl` | `ListMenu.GetShowedMenu(...)` + `MenuWorks.GetMyLidCount(...)` + `Works.WorkPass(...)` | Yes, but icon still conditional on `WorkPass` | ⚠️ FLOWING (conditional finish icon) |

### Behavioral Spot-Checks

| Behavior | Command | Result | Status |
|----------|---------|--------|--------|
| 提交/任务页/完成态源码回归 | `dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj --filter "FullyQualifiedName~UploadWork|FullyQualifiedName~ShowMission|FullyQualifiedName~ActivityPlanCompletion" --framework net8.0` | Passed 9/9 | ✓ PASS |
| Completion helper 与共享写入规则回归 | `dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj --filter "FullyQualifiedName~MenuWorks|FullyQualifiedName~ActivityPlanCompletion" --framework net8.0` | Passed 3/3 | ✓ PASS |

### Requirements Coverage

| Requirement | Source Plan | Description | Status | Evidence |
|-------------|-------------|-------------|--------|----------|
| `SCT-01` | `07-01-PLAN.md`, `07-02-PLAN.md` | Student can submit a result for a published AI-generated activity through the activity page. | ? NEEDS HUMAN | 代码与测试表明 `showmission.aspx` -> `showmission.js` -> `uploadworkm.aspx.cs` 已连通，但未执行真实认证上传。 |
| `SCT-02` | `07-02-PLAN.md` | Student can have completion status recorded after a successful activity submission. | ? NEEDS HUMAN | `EnsureMenuWorksCompletion(...)` 与 `MenuWorks` 去重写入已存在，但真实上传后的页面完成态需要人工确认。 |

**Orphaned requirements:** none

### Anti-Patterns Found

| File | Line | Pattern | Severity | Impact |
|------|------|---------|----------|--------|
| `student/Scm.master.cs` | 313-315 | 当前菜单完成图标同时依赖 `MenuWorks` 与 `Works.WorkPass(...)` | ⚠️ Warning | 自动化源码检查能证明“完成记录已写入”，但不能单独证明“上传后当前菜单项一定显示完成图标”。 |
| `Tests/TeacherRegressionTests/TeacherRegressionTests.cs` | 932-939 | 测试只断言菜单源码依赖项，不验证上传后 UI 结果 | ℹ️ Info | 说明需要保留人工 UAT，避免把源码依赖误当作最终行为证明。 |
| `07-01-PLAN.md` key link vs actual code | n/a | 计划把 `showmission.aspx.cs -> uploadwork.aspx.cs` 作为关键链路，但真实活动页前端走 `uploadworkm.aspx` | ℹ️ Info | 不影响提交目标达成，但说明实际生效的是备用上传处理器路径。 |

### Human Verification Required

### 1. AI 活动页真实上传

**Test:** 教师先发布一个启用上传的 AI 活动；学生登录后打开该活动页，上传一个符合 `Mfiletype` 规则的文件。  
**Expected:** 页面提示“作品已经提交成功！”或等价成功反馈，刷新后出现已上传作品链接，且不会报“活动参数错误/此活动不存在/老师已经评价了”。  
**Why human:** 需要真实登录态、浏览器上传控件、服务器文件保存与数据库写入；当前验证未启动应用也未做端到端认证上传。

### 2. 完成态可见性联调

**Test:** 在上一步上传成功后，刷新学生左侧菜单、`student/myinfo.aspx`，并在教师侧打开 `teacher/start.aspx` / `teacher/workshow.aspx`。  
**Expected:** 至少一个学生侧完成面与一个教师侧状态面能把该活动/课程从未触达区分为已提交/已完成；若左侧菜单图标变化不明显，也应在 `myinfo` 或教师页面中明确可见。  
**Why human:** 真实效果取决于数据库中 `Works`、`MenuWorks`、`WorkPass` 的联动结果；源码显示存在条件分支，但未做运行时验证。

### Gaps Summary

没有发现阻断 Phase 7 目标的明确代码缺口：提交路径、活动身份解析、`MenuWorks` 完成写入、去重保护，以及教师/学生汇总面所需的数据链路都已在代码中存在并通过定向测试。

当前未给出 `passed`，原因不是代码缺失，而是**仍需人工确认真实上传与完成态展示**：该阶段的关键结果依赖认证浏览器流程、文件系统写入和数据库副作用，且学生菜单完成图标还额外受 `WorkPass` 条件影响。自动检查已通过，最终阶段结论取决于上述两项人工验收。

---

_Verified: 2026-04-11T10:20:42Z_  
_Verifier: the agent (gsd-verifier)_
