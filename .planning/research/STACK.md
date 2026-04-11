# Technology Stack

**Project:** OpenLearnSite teaching skills enhancement — v1.2 AI 整课编排与活动组合
**Researched:** 2026-04-11
**Scope:** 仅研究新增能力所需的 stack 变化：AI 整课编排、复用现有活动类型、以及可能的引导式探究块
**Overall confidence:** HIGH

## Executive Recommendation

这一里程碑**不需要引入新前端框架、新独立 AI 服务、也不需要新学生端运行壳**。当前单体已经具备最关键的基础：

- 教师侧入口已在 `teacher/courseedit.aspx` + `teacher/aiprovider_api.ashx`
- AI provider 路由和自定义 skill 已存在
- 学生侧导航已由 `ListMenu.Ltype` 驱动，且已覆盖 `Mission`、`TxtForm`、`Exam`、`Ware`
- 发布/可见性/完成态已经绑定在 `Mission` / `ListMenu` / `Works` / `MenuWorks`

因此，推荐的 stack 方向是：**在现有 ASP.NET Web Forms + SQL Server + C# BLL/DAL 上，新增一个“整课编排组合层”**，而不是再造一个活动系统。

最重要的变化不是技术栈迁移，而是**服务层与数据契约升级**：

1. 从“单个活动计划 JSON”升级为“整课草案 + block 列表 JSON”
2. 从“固定发布成 `Mission + ListMenu(Ltype=1)`”升级为“按 block 类型发布到既有活动实体”
3. 从“单个 linked mission/listmenu”升级为“多活动链接映射”

## Recommended Stack Changes

### Core Framework

| Technology | Version | Purpose | Why |
|------------|---------|---------|-----|
| ASP.NET Web Forms | .NET Framework 4.8 | 教师整课草案预览、编辑、发布入口 | 已经承载 `courseedit.aspx`、`missionadd.aspx`、`wareadd.aspx`、`txtformadd.aspx` 等核心流程，继续复用风险最低 |
| C# BLL/DAL in `App_Code` | current repo pattern | 新增编排服务、发布适配器、JSON 校验 | 当前业务就是通过 BLL/DAL 组织，新增 orchestration layer 最符合 brownfield 结构 |
| SQL Server | 2022 target | 持久化整课草案、block 映射、发布链接 | 现有 `CourseActivityPlanDraft`、`Mission`、`ListMenu`、`Exam`、`TxtForm` 已在 SQL Server 上，且 SQL Server 2016+ 已支持 JSON 文本处理 |

### Data / Contract Layer

| Technology | Version | Purpose | Why |
|------------|---------|---------|-----|
| `Newtonsoft.Json` | 13.0.3 | 解析整课编排 JSON、block 配置、链接映射 | 仓库已在 AI 流程中使用；对 .NET Framework 4.8 兼容，无需新 JSON 库 |
| SQL `nvarchar(max)` JSON storage | SQL Server 2022 | 存储 full-lesson draft、block list、published artifact links | 比加多张高度专用的新表更适合 v1.2；当前代码也更偏向 C# 侧解析而非数据库内强建模 |
| SQL Server JSON functions (`ISJSON`, `JSON_VALUE`, `OPENJSON`) | SQL Server 2016+ / 2022 target | 数据校验、排查、必要时做后台查询 | 官方支持成熟；但应作为辅助能力，不应把核心编排逻辑下沉到复杂 SQL |

### Infrastructure / Integration

| Technology | Version | Purpose | Why |
|------------|---------|---------|-----|
| Existing AI provider route (`teacher/aiprovider_api.ashx`) | current repo implementation | 继续承接整课编排生成与重生成 | 现有默认 provider、鉴权、OpenAI-compatible 请求已跑通，不应旁路 |
| Existing `AICustomSkill` system | current repo implementation | 新增“整课编排 skill scope”与 prompt 模板 | 当前活动计划、量规、考试评估都走 skill；整课编排应沿用同一治理方式 |
| Existing DB migration pipeline (`DbMigration`, `UpdateGrade`) | current repo implementation | 增加草案/链接字段或新表 | 现有仓库已经规范化做数据库演进，不能用散落 SQL 脚本替代 |

## Concrete Additions Needed

### 1. Add a new full-lesson orchestration contract

**Must add:** 一个新的服务器端 JSON 契约，独立于当前 `ActivityPlanDraft`。

当前 `ActivityPlanDraft` 只支持：
- `teachingGoals`
- `activitySteps`
- `resources`
- `assessment`
- `teacherReminder`

这对“整课编排”不够，因为新目标是**输出多个可执行 block**，且 block 可能映射到不同实体：
- quiz → `Exam` + `ListMenu(Ltype=39)`
- resource-study → `Mission`(无上传) + `ListMenu(Ltype=6)`
- web courseware → `Mission(Mcategory=38/Mback)` + `ListMenu(Ltype=38)`
- guided inquiry → 优先 `TxtForm` + `ListMenu(Ltype=4)`

**Recommended C# additions:**

| Addition | Purpose | Why it fits |
|----------|---------|-------------|
| `FullLessonDraft` model | 顶层整课草案 | 和当前 `ActivityPlanDraft` 分离，避免把单活动模型硬扩展成难维护的巨型结构 |
| `LessonBlockDraft` model | 每个课堂活动块的通用模型 | 让 AI 先输出“block intent + block type + config”，再由发布层映射到既有实体 |
| `LessonBlockType` enum / constants | `quiz` / `resourceStudy` / `webCourseware` / `guidedInquiry` / `narrativeOnly` | 比散落字符串更稳，便于前后端和 DAL 对齐 |
| `FullLessonDraftValidator` | 服务端强校验 | 当前 `AIActivityPlanDraftHelper` 已有校验模式，直接沿用这个思想即可 |

**Recommended JSON shape (directional, not final):**

```json
{
  "lessonTitle": "...",
  "summary": "...",
  "blocks": [
    {
      "blockType": "resourceStudy",
      "title": "...",
      "minutes": "8分钟",
      "learningObjective": "...",
      "teacherNotes": "...",
      "studentInstructions": "...",
      "publishTarget": "existingActivity",
      "config": {
        "preferredLtype": 6
      }
    }
  ]
}
```

**Why:** 先有统一 block contract，后面才能安全复用不同活动类型。

---

### 2. Replace the current single-target publisher with a composition publisher

**Must change:** 当前 `App_Code/Dal/AIActivityPlanPublisher.cs` 只会落成：
- `Mission`
- `ListMenu`
- `Ltype = 1`

这只能发布“普通任务活动”，**无法编排多类型整课**。

**Recommended additions:**

| Addition | Purpose | Integration point |
|----------|---------|-------------------|
| `AIFullLessonPublisher` | 整课发布总入口 | 新增于 `App_Code/Bll` / `App_Code/Dal` |
| `ILessonBlockPublisher`-style adapter split (无需复杂 DI) | 每种 block 类型单独发布 | 适合老系统，直接用 switch/strategy 即可 |
| `QuizBlockPublisher` | 发布到 `Exam` + `ListMenu(Ltype=39)` | 复用 `exam/examadd.aspx.cs` 的数据落点 |
| `ResourceStudyBlockPublisher` | 发布到 `Mission(Mcategory=1, Mupload=false)` + `ListMenu(Ltype=6)` | 复用 `teacher/missionadd.aspx.cs` 的“描述页”模式 |
| `WebCoursewareBlockPublisher` | 发布到 `Mission(Mcategory=38, Mback=entryHtml)` + `ListMenu(Ltype=38)` | 复用 `teacher/wareadd.aspx.cs` 的网页课件模式 |
| `GuidedInquiryBlockPublisher` | 优先发布到 `TxtForm` + `ListMenu(Ltype=4)` | 复用 `teacher/txtformadd.aspx.cs` 和 `student/txtform.aspx.cs` |

**Why this fits the project:**

- 学生端菜单 `student/Scm.master.cs` 已经按 `Ltype` 路由
- 教师端已有各类活动的创建落点
- 只要 AI 发布层能正确落到这些实体，学生端无需新壳

---

### 3. Extend draft persistence from single-link to multi-link

**Must change:** 当前 `CourseActivityPlanDraft` 只有：
- `LinkedMissionId`
- `LinkedListMenuId`

这只适合“一个草案对应一个发布活动”。整课编排会产生**多个活动链接**，而且类型不同。

**Recommended change:**

| Option | Recommendation | Why |
|--------|----------------|-----|
| Extend `CourseActivityPlanDraft` with `LinkedArtifactsJson` | **Recommended** | brownfield 最小改动，允许一个课程草案记录多个 block→artifact 映射 |
| Create a normalized `CourseLessonDraftArtifact` child table | Defer unless v1.2 quickly grows | 结构更正规，但对当前里程碑偏重 |

**Recommended stored shape:**

```json
[
  {
    "blockId": "block-1",
    "blockType": "quiz",
    "entityType": "Exam",
    "entityId": 123,
    "listMenuId": 456,
    "ltype": 39
  }
]
```

**Why:**
- 能支持“整课预览 → 选择性发布 → 再次编辑 → 增量更新”
- 不必把 `CourseActivityPlanDraft` 拆成大量关系表
- 仍然保持“一课一份当前草案”的已验证模式

---

### 4. Add one new AI skill scope, not one new AI subsystem

**Must add:** 新的 skill scope / prompt 模板，用于“整课编排”。

**Recommended additions:**

| Addition | Purpose | Why |
|----------|---------|-----|
| New custom skill scope such as `lesson_orchestration` | 区分于现有 activity-plan skill | 单活动计划与整课组合不是同一个 prompt 任务 |
| Block-type guidance in prompt | 让模型优先选择已有块类型 | 符合 milestone：优先复用 quiz / resource-study / web courseware |
| Fallback rule for guided inquiry | 当现有类型不合适时才输出 `guidedInquiry` | 避免 AI 滥用新块，保持 brownfield 优先 |

**Important:**

不要把这个需求做成独立 Python orchestration service。当前仓库的 AI demo Python 区域与主业务链路无关，不适合作为 v1.2 主路径。

---

### 5. Keep teacher preview in existing JS/Web Forms, with richer block rendering only

**Should add:** `js/courseedit.js` 级别的预览增强，用来渲染 lesson blocks、block type 标签、block-specific actions。

**Do not add:** React/Vue/SPA preview shell。

**Why:**
- 当前 AI 预览已经在 `courseedit.aspx` / `courseedit.js` 成立
- 整课草案本质上只是预览结构更复杂，不值得引入前端重构
- Web Forms 页面已经能承载异步请求 + DOM 渲染

## Recommended Reuse Mapping

| AI block type | Reuse existing entity | Existing route | Recommended status |
|---------------|-----------------------|----------------|--------------------|
| Quiz | `Exam` + `ListMenu(Ltype=39)` | `webform/preview.aspx?lid=` / exam pages | **Use directly** |
| Resource study | `Mission` with `Mupload=false`, `Mcategory=1` + `ListMenu(Ltype=6)` | `student/description.aspx?lid=` | **Use directly** |
| Web courseware | `Mission(Mcategory=38, Mback=html entry)` + `ListMenu(Ltype=38)` | `student/ware.aspx?lid=` | **Use directly** |
| Guided inquiry | `TxtForm` + `ListMenu(Ltype=4)` | `student/txtform.aspx?lid=` | **Use as first implementation** |
| Generic upload task | `Mission` with `Mupload=true` + `ListMenu(Ltype=1)` | `student/showmission.aspx?lid=` | **Keep as fallback only** |

### Opinionated recommendation for guided inquiry

**Use `TxtForm` first.**

理由：
- `TxtForm` 已经是独立活动类型
- 学生端已有填写/提交/查看结果路径
- 比新增一个“AI inquiry engine”更 brownfield-compatible

只有在以下条件同时出现时，才考虑再扩活动实体：
- `TxtForm` 不能表达引导步骤
- 需要特定完成判定而无法复用现有回填
- 教师端必须配置远超当前表单能力的结构化参数

在 v1.2 之前，不建议先造新活动引擎。

## Minimal Database Changes

### Recommended

| Change | Type | Why |
|--------|------|-----|
| Extend `CourseActivityPlanDraft` for full-lesson JSON | Schema change | 现有草案表是最自然的整课预览持久化位置 |
| Add `LinkedArtifactsJson` (or equivalent) | Schema change | 支持一个草案链接多个活动成果 |
| Optionally add `DraftType` / `SchemaVersion` | Schema change | 让旧单活动草案与新整课草案能共存/升级 |

### Not recommended yet

| Avoid | Why avoid |
|------|-----------|
| New event-sourcing tables for every AI decision | 过重，当前没有审计/回放刚需 |
| Dedicated orchestration database | 会把单体内已有课程/活动关系打散 |
| Separate student runtime state store | 已有 `Works` / `MenuWorks` 足够支撑首版 |

## Installation

```bash
# No new baseline packages recommended for v1.2
# Keep existing stack:
# - ASP.NET Web Forms / .NET Framework 4.8
# - Newtonsoft.Json 13.0.3
# - SQL Server 2022 target
```

如果一定要新增代码依赖，优先级应是：

1. **不新增包，先用现有 `Newtonsoft.Json`**
2. 再考虑小型辅助库
3. 最后才考虑新服务或新前端框架

## Alternatives Considered

| Category | Recommended | Alternative | Why Not |
|----------|-------------|-------------|---------|
| Lesson composition | C# orchestration layer in existing monolith | New microservice for lesson orchestration | 会复制 auth、provider routing、course ownership、publish transaction |
| Draft persistence | Extend `CourseActivityPlanDraft` | New normalized multi-table lesson engine | 对 v1.2 太重，先用 JSON sidecar 更稳 |
| Guided inquiry | Reuse `TxtForm` | New inquiry-specific subsystem | 现有学生端和教师端已有可用路径，不值得重造 |
| Preview UI | Extend `courseedit.js` | New SPA/React page | Brownfield 不兼容，迁移成本高 |
| AI output control | Prompt + server validation; optional provider-native structure when available | Depend entirely on provider-specific structured output | 当前 provider 路由是 OpenAI-compatible but generic；不能把核心成功率绑定到单家能力 |

## What Explicitly NOT to Add

| Do Not Add | Why | Use Instead |
|------------|-----|-------------|
| React / Vue / SPA rewrite for teacher preview | 只为 richer preview 而重构 UI，收益远低于成本 | 继续在 `courseedit.aspx` + `js/courseedit.js` 做 block 预览 |
| New Python orchestration service | 与当前主业务链路脱节，增加部署与监控复杂度 | 继续用 C# BLL + existing provider route |
| Direct browser-to-LLM calls | 绕过 provider governance、泄露密钥、难审计 | 继续走 `teacher/aiprovider_api.ashx` |
| New student-side execution shell | 现有 `Ltype` 路由已覆盖目标类型 | 发布到现有 `Mission` / `TxtForm` / `Exam` / `Ware` |
| Separate progress/completion framework | 现有 `MenuWorks` / `Works` 已是系统事实来源 | 在现有完成模型上补映射 |
| Heavy workflow engine / BPM | 当前需求是 lesson draft composition，不是跨系统编排 | 用简单 C# strategy/adapter 即可 |

## Integration Points in Current Codebase

| Area | Current file(s) | Needed change |
|------|------------------|---------------|
| Teacher AI entry | `teacher/courseedit.aspx`, `js/courseedit.js`, `teacher/aiprovider_api.ashx` | 新增 full-lesson generate / regenerate / preview block actions |
| Prompt building | `App_Code/Common/AIActivityPlanPromptBuilder.cs` | 新增整课编排 prompt builder，不要继续复用单活动字段结构 |
| Draft parsing/validation | `App_Code/Common/AIActivityPlanDraftHelper.cs` | 新增 `FullLessonDraft` helper/validator |
| Draft persistence | `App_Code/Model/CourseActivityPlanDraft.cs` + related DAL/BLL | 支持多 block、多链接 JSON |
| Publish transaction | `App_Code/Dal/AIActivityPlanPublisher.cs` | 改为多 block adapter publisher |
| Quiz publishing | `exam/examadd.aspx.cs` pattern | 提炼可复用的 Exam 创建/挂接逻辑 |
| Resource-study publishing | `teacher/missionadd.aspx.cs` pattern | 复用 `Mission` 无上传模式与 `Ltype=6` |
| Web courseware publishing | `teacher/wareadd.aspx.cs` pattern | 复用 `Mcategory=38` / `Ltype=38` |
| Guided inquiry publishing | `teacher/txtformadd.aspx.cs`, `student/txtform.aspx.cs` | 以 `TxtForm` 作为第一版 inquiry block |
| Student navigation | `student/Scm.master.cs` | 基本无需新 stack，只需确保 publisher 写入正确 `Ltype` |

## Recommendation Summary for Roadmap Use

### Add now

1. `FullLessonDraft` / `LessonBlockDraft` JSON contract
2. Full-lesson skill scope and prompt builder
3. Composition publisher with block-type adapters
4. `CourseActivityPlanDraft` multi-link persistence extension
5. `courseedit.js` block preview rendering

### Reuse as-is

1. AI provider routing
2. ASP.NET Web Forms pages
3. SQL Server + migration pipeline
4. `Mission` / `TxtForm` / `Exam` / `Ware` entities
5. Student menu routing by `ListMenu.Ltype`

### Explicitly defer / reject

1. SPA rewrite
2. Microservice split
3. New student runtime
4. New progress-tracking subsystem
5. Provider-specific-only orchestration stack

## Sources

### High confidence
- Repository code:
  - `teacher/courseedit.aspx`, `js/courseedit.js`, `teacher/aiprovider_api.ashx`
  - `App_Code/Bll/AIActivityPlanDraftGenerator.cs`
  - `App_Code/Common/AIActivityPlanPromptBuilder.cs`
  - `App_Code/Common/AIActivityPlanDraftHelper.cs`
  - `App_Code/Dal/AIActivityPlanPublisher.cs`
  - `App_Code/Model/CourseActivityPlanDraft.cs`
  - `teacher/missionadd.aspx.cs`
  - `teacher/wareadd.aspx.cs`
  - `teacher/txtformadd.aspx.cs`
  - `exam/examadd.aspx.cs`
  - `student/Scm.master.cs`
  - `student/showmission.aspx.cs`
  - `student/txtform.aspx.cs`

### Official docs
- Microsoft Learn — SQL Server JSON support (`ISJSON`, `JSON_VALUE`, `OPENJSON`), updated 2025-11-18:  
  https://learn.microsoft.com/en-us/sql/relational-databases/json/json-data-sql-server?view=sql-server-ver17
- Microsoft Learn — ASP.NET Web Forms overview, updated 2026-02-21:  
  https://learn.microsoft.com/en-us/aspnet/web-forms/what-is-web-forms

### Research notes / uncertainty
- Context7 查询不可用（API key 配置缺失），因此本次库级验证主要依赖仓库代码和官方文档。
- Provider-native structured output（如 schema-constrained JSON）能力本次未用官方文档成功验证；因此**不建议把 v1.2 成功路径绑定到该能力**。当前建议仍以“prompt + 服务端 JSON 校验”为主，相关判断为 **MEDIUM confidence**。
