# Project Research Summary

**Project:** OpenLearnSite teaching skills enhancement — v1.2 AI 整课编排与活动组合
**Domain:** brownfield ASP.NET Web Forms 教师侧 AI 整课编排与活动组合
**Researched:** 2026-04-11
**Confidence:** HIGH

## Executive Summary

v1.2 本质上不是“把活动计划写得更长”，而是把现有教师侧 AI 助手升级成一个**结构化整课编排器**：教师输入主题或知识点后，系统返回一个可预览、可调整、可选择性应用的整课草案，草案由有类型、有顺序的活动块组成，再由服务器端把这些块发布到现有的 `Mission`、`TxtForm`、`Exam`、`Ware` 路径中。行业研究也支持这一方向：成熟教师工具普遍强调结构化 lesson sequence、可审阅 block、以及教师主导的最终应用，而不是一次性生成长篇文本。

基于仓库现状，最佳方案不是引入新前端框架、新微服务或新学生端运行壳，而是继续复用 `teacher/courseedit.aspx`、`teacher/aiprovider_api.ashx`、现有 AI provider/skill 体系以及 `ListMenu.Ltype` 驱动的学生入口。真正需要新增的是：`FullLessonDraft`/`LessonBlockDraft` 结构化合同、整课 prompt 与 validator、按 block type 分流的发布编排器，以及 block 级链接与重发安全机制。v1.2 的差异化价值不在“AI 会写教案”，而在“AI 能直接编排成 OpenLearnSite 里可执行的课堂活动组合”。

关键风险集中在四类：把整课继续做成富文本而非 typed contract；只有“类型名”没有 per-type adapter，导致发布后学生入口错误；再次发布时静默覆盖老师手改内容；以及 teacher preview 与 student runtime 不是同一合同。规避方式也很明确：先定结构化 block schema，再做 preview；发布阶段由 server-side adapter 决定 persistence contract；引入 block-level linkage / diff 确认；并让 preview、publish、student render 尽量复用同一服务端构建链路。

## Key Findings

### Recommended Stack

v1.2 不需要迁移技术栈。研究结论一致支持继续以 **ASP.NET Web Forms + C# BLL/DAL + SQL Server + 现有 AI provider route** 为主干，在单体内部增加“整课编排组合层”。这既符合 brownfield 约束，也最大化复用现有教师入口、学生 `Ltype` 路由和数据库迁移机制。

真正的“stack addition”主要是服务层和数据合同升级，而不是基础设施升级：新增整课 draft/block JSON contract，新增整课 prompt builder 与 validator，新增按块分流的 publisher/adapters，并补齐 block 级链接记录与事务一致性。`Newtonsoft.Json`、SQL Server JSON 能力和当前 migration pipeline 已足够支撑首版。

**Core technologies:**
- ASP.NET Web Forms (.NET Framework 4.8)：继续承载教师生成、预览、编辑、应用入口 —— 复用 `teacher/courseedit.aspx` 风险最低。
- C# BLL/DAL (`App_Code`)：新增 orchestration service、publish adapters、validator —— 最符合当前单体业务组织方式。
- SQL Server 2022：保存 full-lesson draft、block metadata、发布链接 —— 继续复用现有课程/活动数据面。
- `Newtonsoft.Json` 13.0.3：解析与校验整课 JSON —— 仓库已使用，兼容 .NET Framework 4.8。
- 现有 `teacher/aiprovider_api.ashx` + `AICustomSkill`：承接 `fullLesson*` 生成与重生成 —— 不新增独立 AI 子系统。

**Critical additions:**
- `FullLessonDraft` / `LessonBlockDraft` / `blockType` 合同
- `AIFullLessonPromptBuilder` 与 `AIFullLessonDraftHelper`
- `AIFullLessonPublisher` + per-type adapters
- block 级链接记录与重发安全机制
- `courseedit.js` 的 block 预览与块级操作增强

### Expected Features

v1.2 的 table stakes 很清楚：AI 必须返回**可预览的整课序列**，而不是一段长文本；每个块都要有可识别类型与基础元数据；教师必须能在块级别做审阅与控制；并且输出最终能进入现有教师/学生工作流。行业上“能生成教案”已经不再构成差异化，真正有价值的是“能把教案编排成老师立刻能用的活动组合”。

因此，本里程碑最值得做的 differentiator 是 **brownfield-fit orchestration**：优先复用现有活动库存（quiz、resource study、web courseware），在无法可靠映射时提供 guided inquiry fallback，并保持教师审核为中心。应明确避免把 milestone 拉向 drag-and-drop 大编辑器、学生侧 AI 聊天、标准对齐引擎或任意新活动 schema。

**Must have (table stakes):**
- 可预览的整课草案，且按有序 block 展示 opening / core activities / closure。
- AI 基于已知活动类型做组合，而不是只输出 prose。
- 每个 block 有基础元数据：目的、时长、教师动作、学生活动、选择理由。
- 教师具备块级控制：至少支持 remove、regenerate、edit、selective apply。
- 当现有类型不适配时，有 guided inquiry fallback，而不是强行塞进错误类型。
- 应用结果可进入现有 lesson/activity 流程，且任何学生可见变更都需要教师明确确认。

**Should have (competitive):**
- 混合模式 lesson draft：quiz + resource study + web courseware + inquiry。
- block-scoped regeneration，保留其余已确认块。
- lesson-level pacing summary 与时长过密提示。
- 基于现有课程上下文做 composition，减少“泛泛而谈”的 AI 输出。
- “为什么选择这个 block” 的解释文本，提升教师信任。

**Defer (v2+):**
- 深度 standards/curriculum mapping
- 可视化 drag-and-drop orchestration builder
- 自动学生发布
- 新的通用活动 schema 库
- 学生侧 AI copilot/chat
- 多教师协作与版本分支

### Architecture Approach

架构上最重要的结论是：**整课编排仍应挂在 `teacher/courseedit.aspx`，但发布边界必须从“单 draft → 单 Mission/ListMenu”升级为“单 draft → 多 block → 多 legacy entity”**。也就是说，teacher preview、draft persistence、publish orchestration、student runtime route 必须围绕同一个 block contract 工作，而不是分别维护三套格式。推荐的实现路径是：在 `teacher/aiprovider_api.ashx` 新增 `fullLesson*` actions，在 `App_Code/Common` 增加 full-lesson prompt/helper/catalog，在 `App_Code/Bll/Dal` 增加 `AIFullLessonPublisher` 和 block adapters，再由现有 `student/Scm.master.cs` 继续按 `Ltype` 分发到既有学生页。

对于持久化，最合理的 v1.2 目标是保留 `CourseActivityPlanDraft` 作为“每课一个当前草案”的父记录，同时引入**block-level link records** 来保存已发布块与 legacy 实体的对应关系。相比只在父表塞 `LinkedArtifactsJson`，子链接记录更有利于顺序、重发、安全更新和后续回归测试；而 draft JSON 仍可保留在父记录中。

**Major components:**
1. `teacher/courseedit.aspx` + `js/courseedit.js` —— 整课 block 预览、块级 regenerate/remove/reorder/apply 入口。
2. `teacher/aiprovider_api.ashx` + full-lesson prompt/helper —— 生成、保存、加载、重生成整课 draft 的 AI 与校验边界。
3. `AIFullLessonPublisher` + per-type adapters —— 按 `blockType` 发布到 `Mission` / `TxtForm` / `Exam` / `Ware`。
4. `CourseActivityPlanDraft` + block link records —— 保存当前草案、已发布块映射、顺序与更新关系。
5. `student/Scm.master.cs` + 现有学生页 —— 继续执行 `showmission` / `txtform` / `ware` / `preview` 四条既有运行路径。

### Critical Pitfalls

1. **把整课继续做成富文本结果** —— 必须先定义 typed `draft -> blocks[] -> blockType -> publishMapping` 合同，局部重生成以 block 为边界。
2. **只有类型名，没有 per-type adapter** —— quiz / mission / ware / inquiry 都必须声明目标表、必填字段、`Ltype` 与学生 route。
3. **再次发布静默覆盖老师人工修改** —— 必须有 block-level linkage、版本指纹、diff 提示，并区分 create / update linked / reference existing。
4. **teacher preview 与 student runtime 不是同一合同** —— preview、publish、student render 尽量共用服务端 builder，避免“老师看到一套，学生执行另一套”。
5. **guided inquiry 只是新名称，没有可执行运行合同** —— 需先定义 goal、steps、deliverable、submissionMode、completionRule，再决定作者界面。
6. **Web Forms 块编辑过度依赖 dynamic controls** —— 优先 client-side JSON state，避免 ViewState/postback 失真。
7. **AI HTML 与 legacy render 边界不清** —— 默认纯文本，少数字段白名单 HTML，并统一 sanitize/encode pipeline。

## Implications for Roadmap

Based on research, suggested phase structure:

### Phase 1: 整课编排合同与预览基础
**Rationale:** 所有后续工作都依赖稳定的 block contract；如果先做 publisher 或 UI 细节，后面每增加一种 block 都会返工。  
**Delivers:** `FullLessonDraft` / `LessonBlockDraft` / `blockType`、validator、`fullLessonGenerate/load/save` actions、block preview DTO、`courseedit.aspx` block cards。  
**Addresses:** 预览式整课草案、typed block、基础元数据、lesson pacing metadata。  
**Avoids:** Pitfall 1（富文本假合同）、Pitfall 4（preview/runtime 分裂）、Pitfall 11（无时长与顺序校验）、Pitfall 15（metadata 只藏 HTML）。

### Phase 2: 既有活动复用适配器与首批可执行块
**Rationale:** v1.2 的核心价值是“能真正落到现有活动体系”，所以必须尽早把 block type 接到真实 legacy route，而不是只停留在 preview。  
**Delivers:** `AIFullLessonPublisher` 骨架、resource-study / mission adapter、guided inquiry adapter（优先复用 `TxtForm`）、单事务发布框架、`ListMenu.Lsort` 写入。  
**Uses:** 现有 `Mission`、`TxtForm`、`ListMenu`、SQL migration pipeline。  
**Implements:** type-directed publishing、parent draft + child block links。  
**Addresses:** known activity composition、safe fallback block、selective apply into existing flow。  
**Avoids:** Pitfall 2（无 adapter）、Pitfall 5（inquiry 无运行合同）、Pitfall 14（强行错误映射）。

### Phase 3: 多类型发布扩展与重发安全
**Rationale:** 当 Mission/Inquiry 适配跑通后，再接入实体差异更大的 ware 和 quiz，风险更可控。  
**Delivers:** web courseware adapter、quiz adapter、block link records、republish diff/ownership policy、`create vs update vs reference` 规则。  
**Addresses:** mixed-mode draft、更多可执行块、复用现有活动库存的差异化价值。  
**Avoids:** Pitfall 3（静默覆盖）、Pitfall 10（直接改旧实例）、Pitfall 12（只测 happy path）。

### Phase 4: 教师审阅与应用 UX 稳定化
**Rationale:** 只有在合同和发布路径稳定后，块级交互和冲突提示才不会变成反复返工的 UI 泥潭。  
**Delivers:** block-level regenerate/remove/reorder/edit、include/exclude、apply/publish 动作拆分、冲突提示、最后更新时间与单草案提示。  
**Addresses:** teacher control、preserve draft before apply、block-scoped refinement。  
**Avoids:** Pitfall 7（Web Forms 动态控件问题）、Pitfall 9（预览/应用/发布语义混淆）、Pitfall 13（单草案模型困惑）。

### Phase 5: 完成态、评分态与回归加固
**Rationale:** v1.2 真正进入课堂使用前，必须把“已发布”与“已完成”分清，并补齐 teacher→student contract regression。  
**Delivers:** per-block completion semantics、关键块完成集合、teacher dashboard 可见性、publish/route/runtime regression tests。  
**Addresses:** lesson usefulness in real classroom execution。  
**Avoids:** Pitfall 6（完成语义不统一）、Pitfall 12（legacy route 回归缺失）。

### Phase Ordering Rationale

- 先定合同，再做 UI，再做多类型 publisher，这是四份研究都一致支持的依赖顺序。
- phase 边界应按 **block adapter / runtime contract** 来划，而不是按“前端一 phase、后端一 phase”来划。
- guided inquiry 应单独视为关键 phase capability，因为它既是 fallback block，也是最容易失控的新类型。
- quiz 集成应后置于 mission/inquiry/ware 之后，因为它不走 `Mission` 路径，持久化合同差异最大。

### Research Flags

Phases likely needing deeper research during planning:
- **Phase 2：** resource-study 的最终 `Ltype` / student route 仍有实现分歧（研究中出现 `Ltype=1` 与 `Ltype=6` 两种建议），需以代码验证后定案。
- **Phase 2：** guided inquiry 复用 `TxtForm` 的最小运行合同需要更细的字段与提交/完成语义验证。
- **Phase 3：** exam adapter 的创建、更新、重发边界与 teacher-owned ownership policy 需要补足代码级验证。
- **Phase 5：** mixed blocks completion/grading 需要更深入的现有 `Works` / `MenuWorks` / exam result 关联梳理。

Phases with standard patterns (skip research-phase):
- **Phase 1：** full-lesson contract、prompt builder、JSON validator、draft save/load 属于当前单体内的标准扩展。
- **Phase 4：** 在现有 `courseedit.aspx` 上做 preview-first、block-level teacher controls 属于已验证心智模型的延伸。

## Confidence Assessment

| Area | Confidence | Notes |
|------|------------|-------|
| Stack | HIGH | 结论主要来自仓库现有代码与官方文档；方向稳定，且明确“不引入新框架/新服务”。 |
| Features | MEDIUM | 多家教师 AI 产品在 workflow 上高度一致，但公开信息多为产品页，细节不如代码证据扎实。 |
| Architecture | HIGH | 关键结论直接由 `courseedit`、`aiprovider_api`、`AIActivityPlanPublisher`、`student/Scm.master.cs` 等现有实现支撑。 |
| Pitfalls | HIGH | 大多由现有单活动发布路径、`Ltype` 路由、Web Forms lifecycle、HTML 渲染边界直接推导。 |

**Overall confidence:** HIGH

### Gaps to Address

- **resource-study 的精确落点：** `Ltype=1` 还是 `Ltype=6` 需在 Phase 2 做代码与课堂语义验证，再冻结 adapter contract。
- **block link 持久化形式：** 研究中存在“父表 JSON sidecar”与“子链接表”两种建议；本摘要推荐子链接表优先，但若里程碑时间极紧，可先以父表 JSON 过渡，前提是不牺牲 block identity。
- **provider-native structured output：** 本次未形成可靠依赖，不应作为成功路径；继续采用 prompt + server validation。
- **guided inquiry completion semantics：** 需在规划时先定义最小提交物与完成规则，否则容易变成不可执行的新类型名。

## Sources

### Primary (HIGH confidence)
- Repository code — `teacher/courseedit.aspx`, `js/courseedit.js`, `teacher/aiprovider_api.ashx`, `App_Code/Common/AIActivityPlanPromptBuilder.cs`, `App_Code/Common/AIActivityPlanDraftHelper.cs`, `App_Code/Dal/AIActivityPlanPublisher.cs`, `App_Code/Model/CourseActivityPlanDraft.cs`, `teacher/missionadd.aspx.cs`, `teacher/wareadd.aspx.cs`, `teacher/txtformadd.aspx.cs`, `exam/examadd.aspx.cs`, `student/Scm.master.cs`, `student/showmission.aspx.cs`, `student/txtform.aspx.cs`
- Microsoft Learn — SQL Server JSON support: https://learn.microsoft.com/en-us/sql/relational-databases/json/json-data-sql-server?view=sql-server-ver17
- Microsoft Learn — ASP.NET Web Forms overview: https://learn.microsoft.com/en-us/aspnet/web-forms/what-is-web-forms
- Microsoft Learn — ASP.NET Page Life Cycle Overview: https://learn.microsoft.com/en-us/previous-versions/aspnet/ms178472(v=vs.100)
- Microsoft Learn — Dynamic Web Server Controls and View State: https://learn.microsoft.com/en-us/previous-versions/aspnet/hbdfdyh7(v=vs.100)
- Microsoft Learn — Request Validation / script attack prevention: https://learn.microsoft.com/en-us/aspnet/whitepapers/request-validation

### Secondary (MEDIUM confidence)
- MagicSchool Lesson Plan / Quiz / Worksheet / Presentation tools — 证明 teacher-side composition workflow 是市场常态
- Brisk Lesson Plan Generator — 证明结构化 objectives / activities / timing / assessment 是 table stakes
- Khanmigo teacher tools — 证明教师审核优先、structured planning 与 rubric/exit-ticket 结合是成熟路径

### Tertiary (LOW confidence)
- provider-native structured output 能力判断 —— 本次未通过可靠文档与环境验证，需实现中谨慎验证

---
*Research completed: 2026-04-11*
*Ready for roadmap: yes*
