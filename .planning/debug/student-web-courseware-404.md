---
status: diagnosed
trigger: "/gsd-debug 学生端进入整课中的 web courseware block 时出现 404：`/ai/courseware/preview` 不存在。请定位这个 URL 的生成来源，检查现有 brownfield ware 路径应该是什么，判断是 payload 生成错了、publish 写错了，还是 runtime 路由假设错了。只调查，不改代码。最后返回：1) 最可能根因 2) 相关文件位置 3) 建议最小修复。"
created: 2026-04-11T00:00:00Z
updated: 2026-04-11T00:20:00Z
---

## Current Focus
<!-- OVERWRITE on each update - reflects NOW -->

hypothesis: 根因已确认：webCourseware payload 的 `Mback` 被生成为不存在的 `/ai/courseware/preview?...`，publish 和 runtime 都按 brownfield 约定原样透传/消费该值
test: 已完成代码链路核对，无需进一步测试
expecting: 诊断输出应说明 payload 源头、brownfield 正确路径语义，以及最小修复方向
next_action: 返回调查结论

## Symptoms
<!-- Written during gathering, then IMMUTABLE -->

expected: 学生端进入整课中的 web courseware block 时，应加载现有课件预览页面而不是 404
actual: 学生端访问 web courseware block 时请求 `/ai/courseware/preview`，该地址不存在并返回 404
errors: 404: `/ai/courseware/preview` not found
reproduction: 学生端进入整课，打开其中的 web courseware block
started: 未说明

## Eliminated
<!-- APPEND only - prevents re-investigating -->

## Evidence
<!-- APPEND only - facts discovered -->

- timestamp: 2026-04-11T00:05:00Z
  checked: 调试知识库
  found: `.planning/debug/knowledge-base.md` 不存在
  implication: 无可复用历史模式，需要从代码直接建模

- timestamp: 2026-04-11T00:05:00Z
  checked: 常见模式快速检查
  found: 当前症状最匹配 Environment/Config 中的 hardcoded path or URL，以及 Follow the Indirection 类型的路径构造不一致
  implication: 优先追踪 URL/路径是在哪里被硬编码或拼接出来的

- timestamp: 2026-04-11T00:09:00Z
  checked: 全局搜索 `/ai/courseware/preview`
  found: 仓库源码命中只在 `teacher/aiprovider_api.ashx:1730` 和对应测试快照中，那里将 webCourseware payload 的 `Mback` 直接设为 `/ai/courseware/preview?topic=...`
  implication: 404 URL 很可能不是 runtime 自行拼出来，而是服务端 draft/payload 直接写入的

- timestamp: 2026-04-11T00:09:00Z
  checked: webCourseware / ware 相关搜索
  found: 规划与代码注释一致指向 brownfield web courseware 语义为 `Mcategory=38`、`Mfiletype="ware"`、`Ltype=38`，并存在学生端页面 `student/ware.aspx`
  implication: 既有 brownfield 运行时看起来是 ware 页面，而不是 `/ai/courseware/preview`

- timestamp: 2026-04-11T00:12:00Z
  checked: typed payload 规范与运行时页面
  found: `AIActivityPlanDraftHelper.IsValidWebCoursewarePayload` 只要求 `Mcategory=38`、`Mfiletype="ware"`、`Mback` 非空、`Ltype=38`；`student/ware.aspx.cs` 会把 `Mission.Mback` 原样赋给 `WareUrl`
  implication: 系统没有 runtime 兜底把 ware 映射到别的地址，`Mback` 写什么学生端就加载什么

- timestamp: 2026-04-11T00:16:00Z
  checked: publish 与 brownfield 既有 ware 流程
  found: `App_Code/Dal/AIActivityPlanPublisher.cs` 的 `PublishWebCoursewareBlock(...)` 以 `category=38`、`fileType="ware"` 调用 `ResolveOrCreateMission(...)`，并把 `block.WebCourseware.Mback` 直接写入 `Mission.Mback`；学生端菜单 `student/Scm.master.cs` 对 `Ltype=38` 固定跳到 `~/student/ware.aspx?lid=...`；teacher 侧手工新增/编辑网页课件也都是直接把输入 URL 保存到 `mission.Mback`
  implication: brownfield 约定非常清晰：`student/ware.aspx` 是容器页，真正内容地址存在 `Mission.Mback`。当前问题更像生成/发布了错误的 `Mback` 值，而不是 runtime 路由假设错误

- timestamp: 2026-04-11T00:20:00Z
  checked: `/ai/courseware/preview` 路由实现
  found: 仓库中不存在 `ai/courseware/**` 目录，也没有 `preview.aspx`/`preview.ashx` 对应该路由；唯一相关 preview 页面是 quiz 用的 `webform/preview.aspx`
  implication: `/ai/courseware/preview` 在当前 brownfield 中并不是已存在但未接线的 runtime 路由，而是一个不存在的目标地址

## Resolution
<!-- OVERWRITE as understanding evolves -->

root_cause: `teacher/aiprovider_api.ashx` 在生成 `webCourseware` block payload 时，把 brownfield `Mission.Mback` 直接写成了不存在的 `/ai/courseware/preview?topic=...`。后续 `AIActivityPlanPublisher` 会原样写入 `Mission.Mback`，学生端 `student/ware.aspx` 又原样把该值用作 iframe `src`，因此进入整课时必然请求到不存在的 URL 并 404。
fix: 最小修复应落在 `BuildWebCoursewarePayload(...)` 的 `Mback` 生成逻辑：不要输出不存在的 `/ai/courseware/preview`，而应生成一个 brownfield 已存在、可被 `Mission.Mback` 直接消费的真实课件 URL（或在 publish 前把 AI draft 占位 URL 解析/替换成真实 ware URL）。
verification: 仅完成代码链路调查，未改代码
files_changed: []
