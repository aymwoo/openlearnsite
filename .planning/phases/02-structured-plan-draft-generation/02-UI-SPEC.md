# Phase 2 UI spec: course editor structured preview

**Observed:** 2026-04-10
**Status:** Backfilled from implemented code

## Overview

This spec records the actual Phase 2 right-side preview behavior already
implemented in `teacher/courseedit.aspx` and `js/courseedit.js`.

The activity-plan assistant remains embedded beside the existing editor. The
panel keeps generation preview-only: it renders a structured draft in read-only
 cards, supports copy, and does not write generated content back into the
 editor automatically.

## Host surface

The structured preview lives inside the existing sidebar panel:

- Page host: `teacher/courseedit.aspx`
- Panel root: `#courseedit-plan-panel`
- Result surface: `#activity-plan-result`
- Footer action: `copyActivityPlanResult()`

The panel contract still includes the Phase 1 topic-first entry fields:

- Required: `#activity-plan-topic`
- Optional collapsed section: `#activity-plan-fields`
- Optional fields: `#activity-plan-grade`, `#activity-plan-duration`,
  `#activity-plan-goals`
- Generate button: `#activity-plan-generate-btn`
- Progress region: `#activity-plan-progress-wrap`

## Request and loading behavior

The page JavaScript assembles the request in `generateActivityPlan()`.

- Endpoint: `teacher/aiprovider_api.ashx`
- Method: `POST`
- Content type: `application/x-www-form-urlencoded`
- Action: `action=activityPlan`
- Sent fields: `topic`, `grade`, `duration`, `teachingGoals`,
  `existingCourseContent`
- Timeout: 125 seconds in the browser XHR

The browser synchronizes current editor content before submission by calling
`syncContent()`, then reads the hidden editor field via
`window.__courseeditConfig.mcontentId`.

The grade field defaults from the page grade selector when the optional grade
input is empty.

## Progress contract

The progress region is always text-based and is updated through
`setActivityPlanProgress(percent, text, note)`.

Observed states:

- Initial: `0%`, `准备生成`
- Request sent: `10%`, `正在提交请求`
- Headers received: `45%`, `服务端处理中`
- Streaming/response body: `75%`, `正在整理结果`
- Success: `100%`, `生成完成`
- Failures: `生成失败`, `解析失败`, `请求失败`, `网络异常`, `请求超时`

The success note explicitly says the draft is preview-only and will not be
written back automatically.

## Success response contract

The UI expects `res.success === true` and renders `res.data`.

Expected response shape:

```json
{
  "success": true,
  "data": {
    "providerDisplayName": "string",
    "skillName": "string",
    "message": "string",
    "draft": {
      "teachingGoals": ["string"],
      "activitySteps": [
        {
          "sort": 1,
          "title": "string",
          "minutes": "5分钟",
          "teacherAction": "string",
          "studentAction": "string",
          "interactionMethod": "string",
          "resourceSuggestion": "string",
          "assessmentCheck": "string"
        }
      ],
      "resources": ["string"],
      "assessment": ["string"],
      "teacherReminder": "string"
    }
  }
}
```

## Rendering contract

The result surface is rendered only through DOM creation helpers in
`js/courseedit.js`.

- `renderActivityPlanDraft(responseData)` clears the old result and stores the
  latest payload in `lastActivityPlanDraftResponse`.
- `document.createElement(...)` builds all cards and fields.
- `textContent` is used for all teacher-visible values.
- No successful draft path injects provider output with `innerHTML`.

Rendered sections appear in this order:

1. Preview metadata line with provider and skill name
2. `教学目标` list card
3. `活动步骤` steps card
4. `教学资源` list card
5. `评价设计` list card
6. `教师提醒` single-value card

## Step card layout

Each activity step is expanded by default. There is no collapsed accordion
state in Phase 2.

For each `activitySteps[i]`, the UI renders:

- Header title: `N. {title}`
- Header minutes: `{minutes}`
- Field grid entries:
  - `教师活动`
  - `学生活动`
  - `互动方式`
  - `资源建议`
  - `评价检查`

The `sort` field is not rendered directly. Display order follows the array
order returned by the server.

## Placeholder and failure behavior

Before a response arrives, the result area shows:

`生成中，结构化草案完成后会显示在这里。`

On failure, parse error, timeout, or network error, the script clears the
preview and shows browser alerts. The result area is left empty rather than
showing raw provider text.

## Copy behavior

`copyActivityPlanResult()` copies a text export built from the last successful
structured response.

The copied text flattens the structured preview into these sections:

- `【教学目标】`
- `【活动步骤】`
- `【教学资源】`
- `【评价设计】`
- `【教师提醒】`

Copy requires a successful prior render. If no draft is available, the page
alerts `没有可复制的内容`.

## Phase boundary

This UI contract intentionally stays inside the Phase 2 boundary from
`02-CONTEXT.md`.

- The preview is read-only.
- The page does not auto-apply draft content into `mcontent`.
- The page does not persist draft state.
- The page does not support per-section regeneration.

---

*Phase: 02-structured-plan-draft-generation*
*Spec captured from implementation: 2026-04-10*
