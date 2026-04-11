# Domain Pitfalls

**Domain:** 在 brownfield ASP.NET Web Forms 教学系统中增加 AI 整课编排、既有活动复用与引导式探究活动块  
**Researched:** 2026-04-11  
**Overall confidence:** HIGH

## Future Phase Labels Used Below

为便于后续 roadmap 消费，本文统一用以下未来阶段名标注每个坑应在哪一阶段优先处理：

1. **Phase 1 — Orchestration contract**：定义整课草案、活动块、预览与落库的结构化契约
2. **Phase 2 — Reuse adapters & publish transaction**：把 AI 选中的既有活动类型安全映射到现有 `Mission` / `ListMenu` / 其他类型记录
3. **Phase 3 — Guided inquiry block runtime**：补齐引导式探究块的教师编辑、学生展示、提交与完成语义
4. **Phase 4 — Teacher review & apply UX**：稳定预览、局部重生、差异确认、手工修订保护
5. **Phase 5 — Completion, grading & regression hardening**：完成态、评分态、回归测试、运营可见性

## Critical Pitfalls

这些坑一旦踩中，通常会导致返工、数据漂移，或让老师对“AI 整课编排”失去信任。

### Pitfall 1: 把“整课编排”做成富文本结果，而不是结构化活动编排合同

**What goes wrong:** AI 生成了一段“看起来像完整课堂”的内容，但系统无法稳定预览、局部重生成、选择性复用、按块发布或映射到既有活动类型。

**Why it happens:** 团队沿用 v1.0/v1.1 的“计划文本 + append”思路，继续把整课草案当作 HTML/文本处理，而不是当作“lesson draft + activity block list + per-block config”的 typed contract。

**Consequences:**
- 老师看到的是完整草案，系统实际只能落一段说明文字
- 局部重生成会破坏已确认块
- 后续“复用测验 / 资源学习 / 网页课件 / 引导式探究”无法做成稳定映射

**Warning signs:**
- API 返回仍只有 `teachingGoals / activitySteps / resources / assessment / teacherReminder`，没有 block-level 类型、顺序、配置、来源信息
- 预览可以看，但“应用 / 发布 / 复用已有活动”时只能统一走一条 HTML 拼接逻辑
- 一个活动块无法单独识别“这是 quiz / resource-study / ware / guided-inquiry”

**Prevention:**
- 在 Phase 1 先定义结构化 orchestration schema：`draft -> blocks[] -> blockType -> blockConfig -> publishMapping`
- 明确区分：`lesson preview content`、`teacher-editable block metadata`、`student runtime payload`
- 局部重生成必须以 block 为边界，而不是以整段 HTML 为边界

**Detection:**
- 新需求一来就需要改 prompt 和字符串解析，而不是扩展 block model
- 前端出现越来越多 `if text contains ...` 的判断

**Future phase to address:** **Phase 1 — Orchestration contract**

---

### Pitfall 2: “复用既有活动”只做了类型名选择，没有做 legacy type adapter

**What goes wrong:** AI 说“这里插入一个测验 / 资源学习 / 网页课件”，但真正发布时无法生成可运行记录，或者落库后学生入口错误。

**Why it happens:** 现有系统里的活动类型并不是统一模型。仓库里已经体现出强耦合差异：
- 普通任务由 `teacher/missionadd.aspx.cs` 写 `Mission` + `ListMenu(Ltype=1 或 6)`
- 网页课件由 `teacher/wareedit.aspx.cs` 写 `Mission` 的特定字段 + `ListMenu(Ltype=38)`
- 课堂测验由 `exam/examadd.aspx.cs` 写考试记录 + `ListMenu(Ltype=39)`
- 学生入口由 `student/Scm.master.cs` 中的 `Ltype` switch 决定

**Consequences:**
- 老师在预览里看见“已复用”，实际发布出来的是坏链接或错误页面
- 复用逻辑最后退化成“全部转成 mission 文本块”
- 同一个 block 在教师侧和学生侧不是同一个东西

**Warning signs:**
- 所有复用块最后都被强行发布成 `Ltype=1`
- block model 里只有 `typeName`，没有 `adapterKey`、`requiredFields`、`studentRoute`、`publishStrategy`
- “网页课件”和“测验”被当成只是不同文案模板，而不是不同 persistence contract

**Prevention:**
- 为每个首批支持类型定义 adapter：`quiz`、`resource-study/mission`、`web-courseware`、`guided-inquiry`
- 每个 adapter 必须声明：创建哪张表、要写哪些字段、最终 `Ltype` 是什么、学生从哪条 route 打开
- AI 只负责推荐 block type；真正发布由 server-side adapter 决定

**Detection:**
- 发布代码里只有一个“通用 insert”，没有类型分流
- teacher preview 可用，但无法用 contract test 证明学生 route 正确

**Future phase to address:** **Phase 2 — Reuse adapters & publish transaction**

---

### Pitfall 3: 整课草案与已发布活动身份漂移，导致“再次发布”静默改老内容

**What goes wrong:** 老师以为自己在发布当前草案，系统却悄悄更新了之前已链接的 `Mission` / `ListMenu`；或者老师手工改过旧活动后，AI 再次发布把人工修改覆盖掉。

**Why it happens:** 当前代码已经有 draft linkage 机制：`CourseActivityPlanDraft` 存 `LinkedMissionId`、`LinkedListMenuId`，`AIActivityPlanPublisher` 优先复用这些 ID。这个机制对“单活动稳定身份”有价值，但到了“整课多块编排”后，如果没有 block-level linkage、人工修改检测、版本比对，就会把“稳定 identity”误用成“静默覆盖”。

**Consequences:**
- 老师对“重新生成 / 再发布”失去信任
- 历史活动被覆盖，学生入口内容突然变化
- 既有活动复用与“引用已有活动”混在一起，边界不清

**Warning signs:**
- 再发布没有 diff 提示，也没有“将更新 X 个已存在活动块”的确认
- 只有 lesson-level `LinkedMissionId/LinkedListMenuId`，没有 per-block linkage
- 老师手动编辑旧活动后，系统无法检测“AI 草案基线已过期”

**Prevention:**
- 为编排块增加 block-level linkage / version fingerprint
- 发布前比较：`draft snapshot`、`current course content`、`current linked record state`
- 明确区分三种动作：`create new`、`update linked`、`reference existing`
- 对已手工改动的 linked activity fail closed，要求老师确认

**Detection:**
- 测试只验证“可再次发布”，没验证“人工修改后再次发布的行为”
- 老师反馈“我没动这个活动，为什么学生看到的内容变了”

**Future phase to address:** **Phase 2 — Reuse adapters & publish transaction**，并在 **Phase 4 — Teacher review & apply UX** 做确认与冲突提示

---

### Pitfall 4: Teacher preview 与 student runtime 不是同一套渲染合同

**What goes wrong:** 老师在 `teacher/courseedit.aspx` 预览到的整课块很完整，但真正保存后学生看到的是另一种结构、顺序丢失，或关键说明不显示。

**Why it happens:** 预览往往先在浏览器里直接渲染临时 JSON；而学生页最终走的是 legacy route。例如当前学生端：
- `student/Scm.master.cs` 通过 `Ltype` 决定页面
- `student/showmission.aspx.cs` 通过 `Mission.Mcontent` 和 `BuildActivityGuideView(...)` 做展示

如果 preview builder、publish builder、student renderer 三者不是同一 contract，就会出现“老师看到一套，学生执行另一套”。

**Consequences:**
- 老师确认预览失去意义
- 课堂上发现步骤或提交要求缺失
- 问题很难定位，因为预览和运行时分别正确，但不是同一个输出

**Warning signs:**
- 前端预览用一套字段名，后端发布/学生渲染用另一套字段名
- 同一个 block 在 preview 中有时长、目标、材料，但学生页只剩正文
- 新块型先做 teacher-side preview，再“之后再适配学生页”

**Prevention:**
- 统一使用 server-owned render contract：`draft block -> preview DTO -> persisted payload -> student render DTO`
- 预览尽量复用 server-side builder，而不是浏览器自拼 HTML
- 每种 block 增加“preview equals published render”回归测试

**Detection:**
- 一旦改 student renderer，需要同步改多个前端模板
- 课程预览截图与学生页截图经常对不上

**Future phase to address:** **Phase 1 — Orchestration contract** 和 **Phase 3 — Guided inquiry block runtime**

---

### Pitfall 5: 引导式探究块只有“新类型”之名，没有可执行的学生合同

**What goes wrong:** 团队新增一个“guided inquiry”块，但它只是新的文案容器，没有明确的学生目标、步骤、提交产物、教师提示、完成判定，最终既不适合课堂执行，也不适合平台追踪。

**Why it happens:** 现有系统的活动类型大多是 route-driven 的；如果新块只是为了“让 AI 更容易生成”，却没有定义最小运行合同，后面每一层都要猜它是什么。

**Consequences:**
- 老师能生成，但不知道如何发布和使用
- 学生能打开，但不知道要提交什么
- 完成、评分、统计无法落到现有模型

**Warning signs:**
- guided inquiry 只有 `title + content`，没有 `steps[] / deliverable / submissionMode / completionRule`
- 老师需要在发布前大量手工补字段
- 学生侧没有明确“下一步”“提交要求”“完成提示”

**Prevention:**
- 为 guided inquiry 明确定义 required fields：`goal`、`context`、`steps[]`、`teacherHints`、`studentOutput`、`submissionMode`、`completionRule`
- 先设计 student runtime，再反推 teacher authoring schema
- 如果现有 route 不能承载，优先在 block schema 上收缩范围，不要先做一个过宽的新类型

**Detection:**
- 需求评审时经常出现“这个块到底算不算完成”“学生交什么”
- QA 很难写明确验收用例

**Future phase to address:** **Phase 3 — Guided inquiry block runtime**

---

### Pitfall 6: 混合活动块没有统一完成语义，老师看到“已发布”但看不到“已完成”

**What goes wrong:** 整课中混有 quiz、mission、ware、guided inquiry，但每种块的完成判定不一致；老师最终只能看到学生点开过什么，却看不到谁真正完成了整课关键环节。

**Why it happens:** 现有系统的完成态分散在不同 legacy 模型里。当前 v1.1 已明确沿用 `Works` + `MenuWorks` 体系；但整课编排后，如果没有 block-level completion policy，就会把“进入页面”“提交文件”“通过测验”“读完资源”混成一个模糊状态。

**Consequences:**
- 老师不敢在课堂上依赖 AI 编排
- 后续评价、统计、复盘都失真
- 引导式探究块最容易成为“永远没有完成态”的黑洞

**Warning signs:**
- 发布 contract 没有 `completionRule`
- 一个 block 既可以无提交完成，也可以有提交完成，但系统没有明确优先级
- teacher dashboard 上只能看到 work artifact，看不到 block completion

**Prevention:**
- 为每类块明确定义 v1 completion semantics：`view-complete`、`submit-complete`、`pass-complete` 三选一或有限组合
- 映射回现有 `MenuWorks` / `Works` / exam result 模型，不新增 AI-only 完成表
- 把整课完成定义成“关键块完成集合”，不要默认所有块等权

**Detection:**
- 同一门课里不同块的完成图标逻辑分散在多个页面硬编码
- PM/QA 无法回答“学生做完什么算这节课完成”

**Future phase to address:** **Phase 5 — Completion, grading & regression hardening**

---

### Pitfall 7: 在 Web Forms 中用晚绑定 dynamic controls 做块编辑，导致 ViewState / postback 行为失真

**What goes wrong:** 老师在逐块编辑、折叠展开、局部修订时，控件值丢失、事件不触发、验证器错位，甚至出现 postback 后块顺序错乱。

**Why it happens:** Web Forms 的 page lifecycle 和 ViewState 对动态控件创建时机非常敏感。Microsoft 官方文档明确指出：动态控件如果不是在正确阶段重建，ViewState 和事件会不同步；插入到现有控件中间时问题更大。

**Consequences:**
- 老师以为保存了块设置，postback 后值消失
- 复杂块编辑 UI 越做越不稳定
- 最后不得不推倒重写为“全部前端状态 + 单提交”

**Warning signs:**
- 新块编辑器依赖 server-side dynamic controls，且在 `Page_Load` 里临时插入
- postback 后 checkbox / dropdown / validator 状态经常丢失
- 块顺序调整后，另一个块的数据跑位

**Prevention:**
- 块编辑优先走 client-side JSON state，不要把 block editor 建成重度 dynamic server control 树
- 如果必须用 dynamic controls，必须在 `PreInit`/`Init` 重建并保持稳定顺序
- 对 block list 禁止“插入到现有控件中间再依赖旧 ViewState”这类实现

**Detection:**
- 需要大量 `!IsPostBack` 特判和 hidden field 修补才能“勉强可用”
- QA 只能在一次直线流程中通过，回退/再编辑就出错

**Future phase to address:** **Phase 4 — Teacher review & apply UX**

---

### Pitfall 8: AI 内容与 legacy HTML 边界处理不清，造成渲染污染与 XSS 回归

**What goes wrong:** 系统为了支持 rich lesson preview / inquiry block，自由接收并回显 HTML，结果老师预览可见、学生页也直接渲染，最终把样式污染、危险脚本或异常标签带进课程页面。

**Why it happens:** 仓库里已经存在敏感边界：
- `teacher/courseedit.aspx` 设置了 `ValidateRequest="false"`
- `teacher/missionadd.aspx.cs` 会对人工输入做 `HtmlEncode`
- `AIActivityPlanPublishContentBuilder` 对字段做 `HtmlEncode`
- `student/showmission.aspx.cs` 又会 `HtmlDecode` 后写入 `InnerHtml`

这说明一旦 guided inquiry 或复用块开始接受更自由的 HTML，稍有边界不清就会把“安全编码字段”与“可信富文本字段”混起来。

**Consequences:**
- 学生页被异常 HTML 破坏
- 样式/脚本污染跨课程扩散
- 安全回归不是抽象风险，而是现有渲染链条上的真实缺口

**Warning signs:**
- 新 block 直接保存 AI 返回的原始 HTML
- 代码里越来越多 `HtmlDecode(...); InnerHtml = ...;`
- 没有字段级别白名单：哪些字段只能纯文本，哪些字段允许受限 HTML

**Prevention:**
- 对 orchestration schema 做字段级安全策略：默认纯文本，仅少数字段允许受限 HTML
- preview 与 publish 共用同一 sanitize/encode pipeline
- guided inquiry 如需富文本，必须先定 whitelist，而不是先开放原始 HTML

**Detection:**
- 预览与学生页对同一字段一个 encode、一个 raw render
- 只要 prompt 改一改，输出标签结构就能破坏页面布局

**Future phase to address:** **Phase 1 — Orchestration contract**，并在 **Phase 3 — Guided inquiry block runtime** 落实渲染白名单

## Moderate Pitfalls

### Pitfall 9: 老师操作语义不清，预览、应用、发布混成一个动作
**What goes wrong:** 老师不清楚“写回学案正文”和“对学生可见发布”是不是同一步。  
**Prevention:** 明确拆开 `preview`、`apply to lesson`、`publish to students`、`update linked activities` 四个动作，并给出 side-effect 文案。  
**Warning signs:** 一个按钮同时改正文、改 `Mission`、改 `ListMenu`。  
**Future phase to address:** **Phase 4 — Teacher review & apply UX**

### Pitfall 10: “复用已有活动”被实现成直接改已有实例，而不是“引用/克隆”
**What goes wrong:** AI 为当前课程改动了一个旧活动，结果影响之前课程或老师原来手工设计的活动。  
**Prevention:** 明确 `reference existing`、`clone existing`、`update linked draft-owned activity` 三种模式，默认不要直接改共享资产。  
**Warning signs:** 复用时只传已有 `Mid/Lid`，没有 ownership / provenance / clone policy。  
**Future phase to address:** **Phase 2 — Reuse adapters & publish transaction**

### Pitfall 11: 没有做课时与顺序校验，AI 组合出“不能上”的课堂
**What goes wrong:** 一个 40 分钟课堂被组合出 65 分钟活动，或先要求学生提交、后给资源。  
**Prevention:** 在 orchestration validator 中校验总时长、块顺序、前置依赖、同目标重复。  
**Warning signs:** 预览页只展示，不做任何结构校验。  
**Future phase to address:** **Phase 1 — Orchestration contract**

### Pitfall 12: 回归测试只覆盖 happy path，没覆盖 legacy route contract
**What goes wrong:** 新块发布后看似成功，但学生入口、提交、完成图标或菜单显示在某个 `Ltype` 分支上悄悄坏掉。  
**Prevention:** 为 publish contract、`Ltype` route、student open/submit/complete 建立 contract regression tests。  
**Warning signs:** 只有 handler 单测，没有 teacher→student 端到端验证。  
**Future phase to address:** **Phase 5 — Completion, grading & regression hardening**

## Minor Pitfalls

### Pitfall 13: 单课程单草案模型在多标签页或协作场景下让老师困惑
**What goes wrong:** 老师在两个标签页工作，后保存覆盖先保存；或误以为能同时保留多个方案。  
**Prevention:** 明确“当前仅保留一个进行中的整课草案”，并显示最后更新时间与替换提示。  
**Future phase to address:** **Phase 4 — Teacher review & apply UX**

### Pitfall 14: AI 无法可靠映射 legacy type 时仍强行发布
**What goes wrong:** 系统勉强把不适合的块塞进某个现有类型，老师与学生两边都变得别扭。  
**Prevention:** 无法可靠映射时 fail closed，退回 guided inquiry 或要求老师手选类型。  
**Future phase to address:** **Phase 2 — Reuse adapters & publish transaction**

### Pitfall 15: 块级元数据只存在 HTML 中，后续搜索、统计、排序都做不了
**What goes wrong:** 系统以后想统计“本学期用了多少探究块/测验块”，却只能解析 HTML。  
**Prevention:** 关键 block metadata 单独持久化，不要只藏在 `Mission.Mcontent`。  
**Future phase to address:** **Phase 1 — Orchestration contract**

## Phase-Specific Warnings

| Phase Topic | Likely Pitfall | Mitigation |
|-------------|---------------|------------|
| 整课草案 schema 设计 | 用纯文本/HTML 承载 block 结构 | 先定义 typed block contract，再定义 prompt 与 render |
| 既有活动复用 | 只有“选择类型”，没有 per-type adapter | 为 quiz / mission / ware / guided inquiry 建 publish adapters |
| 复用已有实例 | 直接修改旧活动实例 | 明确 reference / clone / update 三种模式 |
| 再发布 | 复用 draft linkage 时静默覆盖人工修改 | 加 block-level linkage、版本比对、diff 确认 |
| Teacher preview | 前端预览与学生渲染不是同一合同 | 让 preview/publish/student render 共用 server-owned builder |
| Guided inquiry 块 | 没有提交物与完成规则 | 先定义 student runtime contract，再扩 authoring UI |
| Web Forms 编辑 UI | dynamic controls 晚创建导致 ViewState 错乱 | 优先 client-side state；必须 dynamic 时在 PreInit/Init 重建 |
| 发布落库 | `Mission` / `ListMenu` / 正文 / draft links 分步写 | 单 transaction 写入并做 contract tests |
| 完成态 | 不同块的完成规则无法统一呈现 | 统一映射到 legacy completion semantics |
| 安全边界 | 直接渲染 AI HTML | 默认纯文本、少量字段白名单 HTML、统一 sanitize pipeline |

## Recommended Order for Risk Reduction

先处理顺序建议如下：

1. **先定结构化编排合同，再做 UI 与 prompt 扩展**  
   否则后面每加一种块都要返工。

2. **先做既有类型 adapter 与 publish transaction，再做“AI 自动选择块”**  
   否则 preview 很快会领先于可执行能力。

3. **先定 guided inquiry 的 student runtime contract，再开放老师可编辑细节**  
   否则只是新增一个“难以运行的类型名”。

4. **最后补 teacher review、冲突确认、回归测试加固**  
   这些不该省，但前提是底层 identity 与 runtime contract 已经稳定。

## Sources

### Repository evidence
- `App_Code/Common/AIActivityPlanDraftHelper.cs` — 当前草案结构仍是 section-based，不是 block orchestration
- `App_Code/Dal/AIActivityPlanPublisher.cs` — 当前发布已具备 draft linkage，但仍是单 mission/menu 导向
- `App_Code/Common/AIActivityPlanPublishContentBuilder.cs` — 当前 publish builder 主要输出 lesson/mission HTML
- `App_Code/Model/CourseActivityPlanDraft.cs` — 已有 `LinkedMissionId` / `LinkedListMenuId`，提示 identity 复用风险
- `student/Scm.master.cs` — 学生入口严格依赖 `Ltype` 路由
- `student/showmission.aspx.cs` — 当前 mission 渲染与活动引导展示 contract
- `teacher/missionadd.aspx.cs` — 普通任务类型的 legacy 创建合同
- `teacher/wareedit.aspx.cs` — 网页课件类型的 legacy 创建合同
- `exam/examadd.aspx.cs` — 课堂测验类型的 legacy 创建合同
- `js/courseedit.js` — 当前 teacher preview / publish / regenerate 行为边界

### Official documentation
- Microsoft Learn: ASP.NET Page Life Cycle Overview  
  https://learn.microsoft.com/en-us/previous-versions/aspnet/ms178472(v=vs.100)
- Microsoft Learn: Dynamic Web Server Controls and View State  
  https://learn.microsoft.com/en-us/previous-versions/aspnet/hbdfdyh7(v=vs.100)
- Microsoft Learn: Request Validation - Preventing Script Attacks  
  https://learn.microsoft.com/en-us/aspnet/whitepapers/request-validation

## Confidence Notes

| Area | Confidence | Notes |
|------|------------|-------|
| Legacy route / publish pitfalls | HIGH | 直接由仓库中的 `Ltype`、publish、student route 代码支持 |
| Draft identity / republish pitfalls | HIGH | 仓库已存在 linked draft records 与单活动 publish path |
| Guided inquiry runtime pitfalls | HIGH | 由当前缺失的新块合同与现有 legacy runtime 边界推导，证据充分 |
| Web Forms lifecycle pitfalls | HIGH | 有 Microsoft 官方文档支撑，且与 brownfield Web Forms 场景直接相关 |
| HTML safety pitfalls | HIGH | 现有 `ValidateRequest=false`、encode/decode/render 链条已证明是实际边界 |
