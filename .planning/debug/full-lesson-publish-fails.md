---
status: diagnosed
trigger: "/gsd-debug 整课草案发布失败。目标：定位 Phase 10 引入的 fullLessonPublish 在 teacher/courseedit.aspx 工作流中的失败原因。请先检查前端 js/courseedit.js 中失败提示来源，再检查 teacher/aiprovider_api.ashx 和 App_Code/Dal/AIActivityPlanPublisher.cs 的 fullLessonPublish 路径，找出最可能导致真实教师会话下发布失败的根因。只做调查，不改代码。最后返回：1) 最可能根因 2) 相关文件与代码位置 3) 建议修复方向。"
created: 2026-04-11T00:00:00Z
updated: 2026-04-11T00:12:00Z
---

## Current Focus

hypothesis: 根因已定位为 full lesson draft 的“可生成/可校验”块类型集合与“可发布”块类型集合不一致：生成路径会产出 mission 块，但发布路径拒绝 mission 块。
test: 用代码链路交叉验证：前端 fullLessonPublish 请求 → aiprovider_api.ashx FullLessonPublish → DAL PublishFullLesson/IsSupportedPublishBlock，并对照 BuildFullLessonDraftFromActivityPlan 与 AIActivityPlanDraftHelper 校验规则。
expecting: mission 块会在真实教师草案中频繁出现，并在 PublishFullLesson 的循环中导致返回 null，前端最终只看到通用失败提示。
next_action: 返回诊断结果与修复方向

## Symptoms

expected: 教师在 teacher/courseedit.aspx 触发整课草案发布后，应成功走通 Phase 10 引入的 fullLessonPublish 工作流并完成发布。
actual: 整课草案发布失败，前端出现失败提示。
errors: 用户报告为“整课草案发布失败”，具体前端提示文本待从 js/courseedit.js 确认。
reproduction: 在 teacher/courseedit.aspx 的真实教师工作流中触发 fullLessonPublish。
started: Phase 10 引入 fullLessonPublish 后。

## Eliminated

## Evidence

- timestamp: 2026-04-11T00:03:00Z
  checked: .planning/debug/knowledge-base.md
  found: 知识库文件不存在。
  implication: 没有可直接复用的已知故障模式，需要从代码路径重新调查。

- timestamp: 2026-04-11T00:04:00Z
  checked: js/courseedit.js、teacher/aiprovider_api.ashx、App_Code/Dal/AIActivityPlanPublisher.cs（首轮读取）
  found: 前端与服务端都存在 fullLesson* 路径；aiprovider_api.ashx 的 FullLessonPublish 若发布结果为空或 publishedBlocks 为空，会统一返回“整课草案发布失败。”。
  implication: 用户看到的失败提示很可能是服务端笼统兜底消息，需进一步确认是哪一类条件导致 PublishFullLesson 返回 null。

- timestamp: 2026-04-11T00:08:00Z
  checked: js/courseedit.js:1124-1179
  found: 前端发布整课草案时固定 POST `action=fullLessonPublish&cid=...&topic=...&publishToStudents=...&currentDraft=...`；失败提示直接显示服务端 `res.msg`，否则退化为“整课草案发布失败”。
  implication: 前端本身没有额外业务校验；真实失败点应在服务端 fullLessonPublish 路径。

- timestamp: 2026-04-11T00:10:00Z
  checked: teacher/aiprovider_api.ashx:1098-1132 与 App_Code/Dal/AIActivityPlanPublisher.cs:83-179,918-947
  found: FullLessonPublish 调用 `PublishFullLesson(request)`；若返回 null 或 `PublishedBlocks.Count == 0` 就统一失败。DAL 在循环发布每个 block 时先走 `IsSupportedPublishBlock`，仅支持 `guidedinquiry`、`resource-study`、`webcourseware`、`quiz`，其他类型直接 rollback 并 return null。
  implication: 只要整课草案中出现任一非上述类型的 block，整次发布事务就会失败。

- timestamp: 2026-04-11T00:11:00Z
  checked: teacher/aiprovider_api.ashx:1282-1718 与 App_Code/Common/AIActivityPlanDraftHelper.cs:247-535
  found: `BuildFullLessonDraftFromActivityPlan` 会稳定生成 `BlockType = "mission"` 的块：教学目标块固定是 mission，普通 activity step 在不是 quiz/guidedInquiry 时也会是 mission；但 `IsValidFullLessonDraft`/`IsValidFullLessonBlock` 又允许这类块通过校验，因为未知 blockType 会直接返回 true。
  implication: 整课草案“生成/保存/预览”阶段允许 mission 块存在，但“发布”阶段拒绝 mission 块，形成前后端工作流内在不一致。这最符合真实教师会话下发布失败：教师正常生成的草案往往天然包含 mission 块。

- timestamp: 2026-04-11T00:12:00Z
  checked: Tests/CommonLogicTests/CommonLogicTests.cs:1409-1479
  found: 测试里的 `BuildValidFullLessonDraft()` 也把 `BlockType = "mission"` 视为合法 full lesson block。
  implication: 当前代码与测试都默认 mission 块在 full lesson draft 中合法，但发布器没有对应支持，说明这是设计/实现断裂而非偶发数据问题。

## Resolution

root_cause: Phase 10 的 fullLesson 生成与发布契约不一致。`BuildFullLessonDraftFromActivityPlan` 会生成 `mission` 类型 block（教学目标块固定如此，普通步骤多数也会落到 mission），`AIActivityPlanDraftHelper.IsValidFullLessonDraft` 也允许它们通过；但 `AIActivityPlanPublisher.PublishFullLesson` 只接受 `guidedinquiry/resource-study/webcourseware/quiz`，遇到 `mission` 就回滚并返回 null，最终前端只看到“整课草案发布失败”。
fix: 统一 full lesson draft 的块类型契约：要么让生成路径不再产出 mission 块并全部映射到可发布类型，要么让发布器补齐 mission 块的发布实现（Mission + ListMenu type 1）并与校验逻辑同步。
verification: 仅做代码调查，未改动代码；根因基于前端提示来源、handler fullLessonPublish 路径、DAL 发布条件、draft 生成/校验规则交叉比对确认。
files_changed: []
