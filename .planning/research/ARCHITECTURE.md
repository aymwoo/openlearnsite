# Architecture Research: AI 整课编排与活动组合

**Project:** OpenLearnSite teaching skills enhancement  
**Focus:** 预览式整课编排、复用既有活动页、引导式探究块  
**Researched:** 2026-04-11  
**Confidence:** HIGH

## Executive Recommendation

整课编排应继续挂在 `teacher/courseedit.aspx`，保持“生成预览 → 教师审阅 → 明确发布/应用”这条已验证路径，不要引入新的教师入口页，也不要在生成时直接落库到学生可见活动。

发布层不要继续沿用当前“单个 draft 对应单个 Mission/ListMenu”的假设。v1.2 的本质变化是：**一个整课草案会拆成多个有序活动块**，而不同块要落到不同的 brownfield 实体和学生入口：

- **资源学习块** → 继续走 `Mission + ListMenu(Ltype=1) + student/showmission.aspx`
- **网页课件块** → 继续走 `Mission(Mcategory=38, Mfiletype=ware) + ListMenu(Ltype=38) + student/ware.aspx`
- **测验块** → 继续走 `Exam + ListMenu(Ltype=39) + webform/preview.aspx?lid=`
- **引导式探究块** → 复用 `TxtForm + ListMenu(Ltype=4) + student/txtform.aspx`

因此推荐新增一个**整课发布编排器**，在服务器端把一个 lesson draft fan-out 成多种 legacy 活动记录；不要把所有块硬塞进 `Mission`，也不要新增 AI 专用学生页。

---

## Brownfield Target Architecture

```text
teacher/courseedit.aspx
  └─ 整课编排预览 UI
      ├─ 生成整课 draft
      ├─ 按块重生成
      ├─ 选择保留/跳过块
      └─ 触发发布
            ↓
teacher/aiprovider_api.ashx
  └─ FullLesson actions
            ↓
App_Code/Common
  ├─ FullLessonPromptBuilder
  ├─ FullLessonDraftHelper
  └─ ActivityBlockCatalog
            ↓
App_Code/Bll/AIFullLessonPublisher
  └─ 按 blockType 调用 legacy adapters
            ↓
      ┌───────────────┬───────────────┬───────────────┬───────────────┐
      │资源学习块      │网页课件块      │测验块          │引导式探究块    │
      │Mission/Ltype=1│Mission/Ltype=38│Exam/Ltype=39  │TxtForm/Ltype=4│
      └───────────────┴───────────────┴───────────────┴───────────────┘
            ↓
student/Scm.master 现有导航分发
            ↓
showmission / ware / preview(myexam chain) / txtform
```

---

## Explicit Integration Points

### 1. Teacher orchestration entry

| Area | Current state | Recommended integration |
|------|---------------|-------------------------|
| `teacher/courseedit.aspx` | 已有 activity-plan 侧边预览、保存、恢复、发布按钮 | 扩成“整课块预览器”，但仍停留在当前页，不新建 AI 编排页 |
| `teacher/courseedit.aspx` JS | 当前只理解单个 draft 的 section 选择/发布 | 改为理解 `lesson blocks[]`：排序展示、块级 regenerate、块级 include/exclude、块级发布结果回显 |
| `teacher/aiprovider_api.ashx` | 已有 `activityPlan*` 系列 action | 新增平行的 `fullLesson*` action；不要把多块 lesson draft 塞进现有单活动 contract |

**Why:** `courseedit.aspx` 已经是已验证的教师心智模型；真正要扩的是 draft 结构，不是入口位置。

### 2. Draft persistence boundary

| Area | Current state | Required change |
|------|---------------|-----------------|
| `CourseActivityPlanDraft` | 一课一个 draft；当前只够保存单活动 JSON；只留 `LinkedMissionId`/`LinkedListMenuId` | 继续作为“当前课程草案父记录”，但必须支持整课 schema，并新增多块链接能力 |
| `AIActivityPlanSavedDraftHelper` | 保存 topic/grade/duration/simple draft | 为整课编排新增独立 helper，不要复用单活动 parser |

**Critical implication:** 当前 `LinkedMissionId` 和 `LinkedListMenuId` 只适合 v1.1 的“单个发布物”。v1.2 一旦有多个块，这两个字段就不再足够。

**Recommendation:**

- 保留 `CourseActivityPlanDraft` 作为父记录（因为“每课一个当前草案”仍然成立）
- 新增一个子链接表，例如 `CourseActivityPlanDraftBlockLink`

Suggested responsibility of child link table:

| Field | Purpose |
|------|---------|
| `Cid/Hid` or `DraftId` | 归属到当前课程草案 |
| `BlockKey` | 稳定标识某个 lesson block |
| `BlockType` | `mission` / `ware` / `quiz` / `inquiry` |
| `EntityId` | 指向 `Mission` / `Exam` / `TxtForm` 主键 |
| `ListMenuId` | 对应学生导航入口 |
| `Sort` | 保持整课顺序 |

不要再试图给父表继续加 `LinkedMissionId2/3/...` 这类字段。

### 3. Publish orchestration boundary

| Area | Current state | Recommended integration |
|------|---------------|-------------------------|
| `App_Code/Dal/AIActivityPlanPublisher.cs` | 单事务内处理 `Course + Mission + ListMenu`，默认假定 Ltype=1 | 保留给“单 Mission 发布”场景；v1.2 新增 `AIFullLessonPublisher` 统一编排多个 block adapter |
| `App_Code/Bll/AIActivityPlanPublisher.cs` | BLL 只是 DAL 包装 | 新增 `AIFullLessonPublisher` BLL；按 block 类型调用具体 publisher |
| `AIActivityPlanPublishContentBuilder` | 只会把单个 draft 转成课程追加内容和 Mission 内容 | 新增 `FullLessonPublishContentBuilder`，负责课程正文摘要与块级内容拼装 |

**Why:** v1.2 不是“更大的 Mission 文本”，而是“多个 legacy 活动的批量创建/更新”。

---

## Reuse Map: Block Type → Existing Brownfield Route

这是本次架构最关键的映射表。

| New lesson block | Reuse existing entity | Reuse menu type | Student route | Notes |
|------------------|----------------------|-----------------|---------------|-------|
| 资源学习块 | `Mission` | `Ltype=1` | `student/showmission.aspx?lid=` | 最适合承载 AI 生成的目标/步骤/资源/提交说明；现有 `AIActivityPlanMissionViewHelper` 已可解析结构化内容 |
| 网页课件块 | `Mission` with `Mcategory=38`, `Mfiletype="ware"`, `Mback=url` | `Ltype=38` | `student/ware.aspx?lid=` | 已有 `teacher/wareadd.aspx.cs` 和 `student/Scm.master.cs` 路由约定 |
| 测验块 | `Exam` | `Ltype=39` | `webform/preview.aspx?lid=`（学生菜单现有映射） | 不要伪装成 `Mission`；直接复用 exam 链路 |
| 引导式探究块 | `TxtForm` | `Ltype=4` | `student/txtform.aspx?lid=` | 这是最适合的 AI-friendly inquiry 容器，已支持文本填写与协作 |

### Strong recommendation on guided inquiry

**引导式探究块应复用 `TxtForm`，不要新建 AI Inquiry 专用学生页。**

理由：

1. `student/txtform.aspx` 已是现成“阅读说明 → 填写/协作 → 提交结果”容器
2. `teacher/txtformadd.aspx.cs` 已说明这类对象是单独实体 `TxtForm + Ltype=4`
3. 这比把 inquiry 塞进 `showmission.aspx` 更贴近“过程记录/探究作答”
4. 这比新建 AI 页面更符合 brownfield 最小改动原则

---

## New vs Modified Components

### New components

| Component | Type | Responsibility |
|-----------|------|----------------|
| `AIFullLessonPromptBuilder` | Common | 生成“整课块数组”提示词，要求模型返回 ordered blocks |
| `AIFullLessonDraftHelper` | Common | 解析/校验/merge 整课 JSON，支持块级 regenerate |
| `AIFullLessonBlockCatalog` | Common | 定义支持的 block types 及其 brownfield 映射，不要混入 `CustomActivityCatalog` |
| `AIFullLessonPublisher` | BLL/DAL | 统一发布整课，协调课程正文、多个实体、多个 ListMenu |
| Block publishers/adapters | BLL/DAL | `MissionBlockPublisher` / `WareBlockPublisher` / `QuizBlockPublisher` / `InquiryBlockPublisher` |
| `CourseActivityPlanDraftBlockLink` | Persistence | 记录一个整课草案发布出的多个 legacy 实体链接 |

### Modified components

| Component | Change |
|-----------|--------|
| `teacher/courseedit.aspx` | 从 section cards 升级为 lesson block cards；显示 block type、预计时长、目标页面、发布状态 |
| `teacher/aiprovider_api.ashx` | 新增 `fullLessonGenerate / fullLessonRegenerateBlock / fullLessonSaveDraft / fullLessonLoadDraft / fullLessonPublish` |
| `CourseActivityPlanDraft` 相关 BLL/DAL | 支持整课 draft payload；保留“每课一个当前草案”语义 |
| `teacher/courseshow.aspx.cs` | 发布后导航仍依赖既有 `Ltype`；只需确保新建块正确落入现有类型分支 |

### Components to keep unchanged unless forced

| Component | Why keep stable |
|-----------|-----------------|
| `student/showmission.aspx(.cs)` | 已经能把 Mission 内容解析为学习目标/步骤/说明，足够支撑资源学习块 |
| `student/txtform.aspx(.cs)` | 已经提供 inquiry 型填写入口 |
| `student/Scm.master.cs` | 已经通过 `Ltype` 分发到 `showmission / txtform / ware / preview` |
| `student/uploadwork*.aspx.cs` | 仅 mission 型块继续使用；不要为了整课重写上传通道 |

---

## Draft Shape Recommendation

整课 draft 应该是**有序 block 集合**，而不是当前单活动 draft 的简单放大版。

Suggested logical shape:

```typescript
type FullLessonDraft = {
  schemaVersion: "v1.2-full-lesson"
  topic: string
  lessonSummary: string
  totalMinutes: string
  blocks: Array<{
    blockKey: string
    sort: number
    blockType: "mission" | "ware" | "quiz" | "inquiry"
    title: string
    minutes: string
    rationale: string
    teacherPreview: object
    publishPayload: object
  }>
}
```

**Important:**

- `teacherPreview` 用于前端展示
- `publishPayload` 用于落到 legacy entity
- `blockType` 必须在生成时就定下来，发布阶段只做验证与转换，不做“再次智能猜测”

---

## Data Flow Implications

### A. Preview flow

```text
teacher/courseedit.aspx
  → fullLessonGenerate
  → AI returns ordered blocks
  → draft saved as preview-only lesson draft
  → teacher reviews per block
```

**Rule:** 预览阶段绝不创建 `Mission/Exam/TxtForm/ListMenu`。

### B. Publish flow

```text
teacher confirms selected blocks
  → AIFullLessonPublisher
      → for each block:
          mission block  -> Mission + ListMenu(Ltype=1)
          ware block     -> Mission + ListMenu(Ltype=38)
          quiz block     -> Exam    + ListMenu(Ltype=39)
          inquiry block  -> TxtForm + ListMenu(Ltype=4)
      → append lesson summary to Courses.Ccontent once
      → save block links for future resume/update
```

### C. Student execution flow

```text
ListMenu ordered by lesson draft sort
  → student/Scm.master route dispatch by Ltype
  → existing student page executes block
  → existing completion/submission paths remain page-specific
```

### D. Ordering implication

整课编排的顺序必须落到 `ListMenu.Lsort`，而不是只保存在 draft JSON 里。否则教师预览顺序与学生实际课堂顺序会分裂。

---

## Transaction and Consistency Rules

### Publish transaction scope

发布应至少保证以下内容在同一 server-owned orchestration 中完成：

1. 计算每个 block 的目标 legacy type
2. 创建/更新对应实体
3. 创建/更新对应 `ListMenu`
4. 同步 `Lsort`
5. 更新 draft link records
6. 最后一次性更新 `Courses.Ccontent`

### Recommended failure model

- **Fail closed**：任一块创建失败，则整个 publish 返回失败
- 不要出现“课程正文已追加，但学生入口只创建了一半”
- 不要让前端循环逐块调用独立 publish API

---

## Architecture Patterns to Follow

### Pattern 1: Type-directed publishing

生成阶段先确定 `blockType`，发布阶段按 `blockType` 走固定 adapter。

**Why:** 这样可以把 AI 不确定性限制在 draft 生成，而不是进入持久化路径。

### Pattern 2: Reuse by route contract, not by UI similarity

不是“看起来像活动说明就都发到 showmission”，而是按现有 route contract：

- `showmission` = Mission 学习活动
- `ware` = 网页课件
- `preview/myexam` = 测验
- `txtform` = 引导式探究/填写

### Pattern 3: Parent draft + child block links

父 draft 负责恢复；子 link 负责多块发布关联。

**Why:** 这是从 v1.1 单活动发布平滑过渡到 v1.2 多活动发布的最小可维护演进。

---

## Anti-Patterns to Avoid

### Anti-pattern 1: One giant Mission for the whole lesson

**What goes wrong:** 老师看到整课预览，但学生侧只有一个大块文本活动，无法真正复用测验/课件/探究页。  
**Instead:** 整课 draft 仅用于编排；发布时拆成多个 legacy blocks。

### Anti-pattern 2: Reusing current `AIActivityPlanPublisher` as-is

**What goes wrong:** 该发布器天然假设 `Mission + ListMenu(Ltype=1)`，会把 quiz/ware/inquiry 错落到错误实体。  
**Instead:** 新建整课 publisher，单 Mission publisher 仅作为其中一个 adapter。

### Anti-pattern 3: Adding a new AI-specific student activity page

**What goes wrong:** 学生入口、完成态、教师回看、导航图标都会分叉。  
**Instead:** 引导式探究直接复用 `TxtForm`；其它块复用现有 route。

### Anti-pattern 4: Continuing to rely on single `LinkedMissionId`

**What goes wrong:** 第二个块发布后，草案只记住最后一个实体，无法 resume/update。  
**Instead:** 引入 block link child table。

---

## Sensible Build Order

### Step 1. Define the orchestration contract

先做 `FullLessonDraft`、`blockType`、validation、save/load schema。  
**Reason:** 不先把 draft shape 定住，前端 preview 和发布器都会返工。

### Step 2. Upgrade `courseedit.aspx` to preview blocks

先让老师能看到 ordered blocks、块类型、块级 regenerate，但仍然不发布。  
**Reason:** 先验证编排质量，再接 persistence fan-out。

### Step 3. Implement mission block publishing

先接 `Mission/Ltype=1` 资源学习块。  
**Reason:** 这是当前 brownfield AI 活动链路最接近、风险最低的一类。

### Step 4. Add inquiry block via `TxtForm`

再接 `TxtForm/Ltype=4`。  
**Reason:** 这是新增价值最大的“AI-friendly fallback”，同时不需要新学生页。

### Step 5. Add web courseware block publishing

接 `Mission(Mcategory=38) + Ltype=38`。  
**Reason:** 已有明确 teacher/student route contract，但 publish payload 与普通 Mission 不同。

### Step 6. Add quiz block publishing

最后接 `Exam + Ltype=39`。  
**Reason:** 这条链路实体不同，不走 Mission，最适合放在 block adapter 体系稳定后接入。

### Step 7. Add multi-block link persistence and republish safety

保存 block links，支持 resume、二次发布、避免重复建块。  
**Reason:** 没有这个层，v1.2 只能“一次性生成一次性发布”，不可维护。

---

## Roadmap Implications

1. **先做 draft schema，再做 UI，再做 publisher**，不要反过来。  
2. **以 block adapters 为 phase 边界** 比以“页面改动/后端改动”分 phase 更清晰。  
3. **guided inquiry 应作为一个明确 phase**，因为它是唯一新增但仍可 brownfield 落地的 fallback activity type。  
4. **quiz integration 应后置**，因为它不是 Mission 系列，集成形态不同。

---

## Source Evidence

- `teacher/courseedit.aspx` / `teacher/courseedit.aspx.cs` — 当前整课助手挂载位置
- `teacher/aiprovider_api.ashx` — 当前 `activityPlan*` 生成、保存、发布入口
- `App_Code/Dal/AIActivityPlanPublisher.cs` — 当前单 Mission 发布器，只适合 v1.1
- `App_Code/Common/AIActivityPlanSavedDraftHelper.cs` — 当前单 draft 持久化模式
- `App_Code/Common/AIActivityPlanMissionViewHelper.cs` — `showmission` 结构化内容解析能力
- `student/showmission.aspx(.cs)` — 资源学习块当前最佳学生页
- `teacher/wareadd.aspx.cs` — 网页课件的 `Mission + Ltype=38` 创建契约
- `exam/examadd.aspx.cs` — 测验的 `Exam + Ltype=39` 创建契约
- `teacher/txtformadd.aspx.cs` / `student/txtform.aspx(.cs)` — 引导式探究块最合适的 brownfield 容器
- `student/Scm.master.cs` — `Ltype` 到学生页面的现有分发规则

---

*Opinionated conclusion:*  
**v1.2 应被实现为“courseedit 上的整课草案 + 服务器端多块发布编排器 + 复用 showmission/ware/quiz/txtform 四条既有学生路径”。** 这比新增 AI 专用活动体系更符合当前系统，也更容易分阶段落地。
