# Stack Research

**Domain:** AI-assisted teaching activity plan generation inside an existing
ASP.NET Web Forms teacher authoring system
**Researched:** April 10, 2026
**Confidence:** MEDIUM-HIGH

The right 2025 stack for this feature is not a new AI product stack. It is a
bounded, schema-driven generation layer added to the existing teacher editing
workflow. The current OpenLearnSite codebase already has the three foundations
that matter most: provider routing, scoped skills, and working teacher-facing
AI entry points.

The standard implementation pattern in 2025 is: collect structured teaching
context, call a strong general model with a strict output contract, validate
the returned JSON on the server, render a teacher-editable preview, and only
then insert approved content into the lesson editor. Do not persist free-form
LLM prose as the system contract.

## Recommended stack

### Core technologies

| Technology | Version | Purpose | Why recommended | Confidence |
|------------|---------|---------|-----------------|------------|
| ASP.NET Web Forms | Existing on .NET Framework 4.8 | Host the new lesson-plan skill inside the current teacher lesson/course editing flow | This project is explicitly brownfield. Reusing the existing teacher pages is faster, lower risk, and matches how Brisk-style workflow-native tools win adoption. | HIGH |
| Dedicated teaching-plan generator service in `App_Code/Bll` | New module on existing runtime | Encapsulate prompt assembly, provider call, JSON validation, fallback logic, and result shaping | This matches the stronger pattern already used by `AIGaugeGenerator` and `AIStudentExamGenerator`. It is far safer than stuffing more logic into the generic `chat` action. | HIGH |
| OpenAI-compatible provider interface with structured output support | Prefer current production models that support schema-constrained JSON | Generate classroom activity plans as typed JSON, not plain text | Provider-native structured output is the standard production pattern now because it reduces parsing failures and makes downstream UI insertion deterministic. Extend the current provider layer instead of adding a second AI stack. | MEDIUM |
| `Newtonsoft.Json` | 13.0.3 already present in repo tests and current code paths | Serialize request/response payloads in the legacy Web Forms layer | The codebase already uses `Newtonsoft.Json` heavily. Keep it for the website layer instead of doing a risky serializer migration during this feature. | HIGH |
| `NJsonSchema` | 11.5.2 | Define and validate the `TeachingActivityPlan` JSON contract server-side | In this brownfield app, `NJsonSchema` is the practical fit because it works well in legacy .NET / Newtonsoft-heavy code and gives you explicit contract validation without introducing a modern-sidecar rewrite. | MEDIUM |

### Supporting libraries

| Library | Version | Purpose | When to use | Confidence |
|---------|---------|---------|-------------|------------|
| `Polly` | 8.6.6 | Retries, timeout wrapping, and circuit-breaker behavior for AI calls | Use around outbound provider calls once you move this path off raw `HttpWebRequest` and into a small `HttpClient`-based provider client. | MEDIUM |
| `System.Text.Json` | 10.0.5 | Optional typed DTO serialization for any new helper library or sidecar | Use only in new isolated modules if you want stronger typed serialization. Do not migrate the whole Web Forms app to it for this milestone. | HIGH |
| `HtmlSanitizer` | 9.0.892 | Sanitize any generated HTML before inserting into rich-text editors | Use if the preview supports HTML insertion. If you render server-built HTML from validated JSON instead, sanitization pressure is much lower, but still valuable at the editor boundary. | MEDIUM |
| `OpenTelemetry` + OTLP exporter | 1.15.1 | Latency, failure, fallback, and provider tracing | Use if you already have a collector or APM backend. If not, add structured logs first and defer full telemetry rollout. | MEDIUM |

### Development tools

| Tool | Purpose | Notes |
|------|---------|-------|
| Local schema fixtures | Freeze expected activity-plan JSON shapes | Check sample outputs into tests so prompt changes do not silently break parsing. |
| Regression tests on `net48` | Protect brownfield integration points | Follow the repo’s existing `net48` test posture and add parser / fallback tests before UI-heavy work. |
| Prompt/version audit logging | Trace generated plans back to model + skill version | Log provider, model, skill name, schema version, latency, and whether fallback was used. |

## Recommended implementation approach

### 1. Add a dedicated teaching-plan generation path

Create a new bounded service, for example `AITeachingPlanGenerator`, instead of
expanding the current generic `aiprovider_api.ashx?action=chat` free-form path.

Why:

- the current `chat` action only takes `prompt` and returns plain text
- it has no schema contract
- it is fine for assistive prose generation, but weak for structured classroom
  planning
- the gauge and student-exam flows already prove the better pattern in this
  codebase: dedicated generator + scoped skill + parse/fallback

### 2. Keep prompt text configurable, but keep the output schema in code

Use the existing `AICustomSkill` table for the pedagogical prompt template, with
a new scope such as `lesson_plan` or `teaching_plan`.

Keep these in code, not in admin-editable prompt text:

- the JSON schema
- required fields
- validation rules
- fallback rules
- render-to-editor mapping

This split is important. Teachers or admins can tune pedagogy wording, but they
must not be able to accidentally break the app’s data contract.

### 3. Generate a strict `TeachingActivityPlan` object

Use a typed contract close to this shape:

```json
{
  "topic": "Fractions",
  "gradeBand": "Grade 4",
  "durationMinutes": 40,
  "teachingGoals": ["..."],
  "prerequisites": ["..."],
  "materials": ["..."],
  "activitySteps": [
    {
      "title": "Warm-up",
      "minutes": 5,
      "teacherActions": ["..."],
      "studentActions": ["..."],
      "interactionMode": "whole_class",
      "assessmentCheck": "..."
    }
  ],
  "differentiation": {
    "support": ["..."],
    "extension": ["..."]
  },
  "assessmentDesign": {
    "formative": ["..."],
    "exitTicket": "..."
  },
  "teacherNotes": ["..."]
}
```

This is the right contract because it maps directly to classroom use,
preview/edit UI, and later export or rubric-generation extensions.

### 4. Ground the request with lightweight teacher context, not RAG first

For v1, send:

- topic or knowledge point
- grade / subject if already known from page context
- desired lesson duration
- teaching objective if available
- preferred activity style if supplied

This matches the market pattern from teacher tools like MagicSchool, Eduaide,
and Brisk: workflow-native generation grounded in objective, standards, source
material, or teacher context.

Do **not** start with:

- a vector database
- curriculum-wide retrieval
- autonomous multi-step agents
- student-personalized orchestration

Those are phase-two concerns after usage proves that grounded v1 planning is
valuable.

### 5. Render preview from validated JSON, then insert

Do not insert raw LLM text directly into the editor as the source of truth.

Instead:

1. generate JSON
2. validate JSON against schema
3. show a teacher preview broken into sections
4. let the teacher accept all or selected sections
5. render accepted sections into the existing editor fields

This produces a much more stable authoring experience and sharply reduces
parsing, formatting, and trust problems.

### 6. Add fallback behavior from day one

Reuse the pattern already present in gauge and exam generation:

- provider missing -> return guided template
- schema validation failure -> return guided template
- partial model output -> salvage safe sections only
- timeout -> keep teacher input and offer retry

The fallback should not be “error only.” It should return a useful starter plan
shell so teachers can keep working.

## Installation

If you extract the AI planning logic into an SDK-style `net48` class library,
install packages there and ship the compiled DLLs into the website `Bin`
directory.

```bash
# Validation / contracts
dotnet add package NJsonSchema --version 11.5.2

# Resilience
dotnet add package Polly --version 8.6.6

# Optional serializer for isolated new modules
dotnet add package System.Text.Json --version 10.0.5

# Optional sanitization
dotnet add package HtmlSanitizer --version 9.0.892

# Optional observability
dotnet add package OpenTelemetry --version 1.15.1
dotnet add package OpenTelemetry.Exporter.OpenTelemetryProtocol --version 1.15.1
```

## Alternatives considered

| Recommended | Alternative | When to use alternative |
|-------------|-------------|-------------------------|
| Dedicated `AITeachingPlanGenerator` service | Keep extending the generic `chat` endpoint | Only for a disposable prototype. Not for a feature that inserts structured plans into production authoring pages. |
| OpenAI-compatible structured output path | Direct Anthropic or Gemini native integration | Use direct vendor integrations only if procurement or model quality requires it. For this codebase, widening the current provider abstraction is the smaller move. |
| `NJsonSchema` in legacy Web Forms context | `JsonSchema.Net` | Use `JsonSchema.Net` if you move this feature to a modern .NET service that is already `System.Text.Json`-first. |
| Stay inside existing editor workflow | Separate planning app or SPA rewrite | Only use a separate app if product strategy changes from “embedded assistant” to “standalone planning workspace.” That is out of scope for this milestone. |

## What not to use

| Avoid | Why | Use instead |
|-------|-----|-------------|
| Free-form plain text as the persisted plan contract | It is brittle to parse, hard to preview reliably, and difficult to reuse for later exports or analytics | Generate strict JSON, validate it, then render teacher-facing HTML/UI |
| `HttpWebRequest` as the long-term provider client | The current code works, but it is harder to make resilient, observable, and testable than a small `HttpClient` wrapper | Isolate AI calls behind a provider client that uses `HttpClient` plus policy handling |
| LangChain, Semantic Kernel, or Microsoft Agent Framework for v1 | This workflow is a bounded generation task, not a multi-agent system. These frameworks add surface area without solving the main brownfield integration problem | Keep orchestration simple inside one service class and a strict schema contract |
| Vector DB / RAG stack in the first release | It adds indexing, ingestion, permission, and freshness complexity before you know whether teachers need curriculum grounding beyond page context | Start with page context, teacher input, and optional attached source material |
| Full serializer migration from `Newtonsoft.Json` to `System.Text.Json` | It increases migration risk for little milestone value | Keep `Newtonsoft.Json` in the legacy app and isolate any new serializer usage to new modules only |

## Stack patterns by variant

**If you want the fastest safe v1:**

- Add `lesson_plan` scope to `AICustomSkill`
- Build `AITeachingPlanGenerator` in `App_Code/Bll`
- Add a dedicated handler or dedicated action for structured plan generation
- Validate JSON server-side
- Render a preview card beside the editor
- Insert accepted sections into the existing lesson/course editor

**If this feature becomes high-volume or vendor-complex:**

- Keep Web Forms as the teacher UI and system of record
- Move only AI orchestration to a small SDK-style `net8` or `net48` service
- Keep the same JSON contract so the Web Forms UI does not need to change
- Add OTLP tracing and richer model-routing logic there

## Version compatibility

| Package A | Compatible with | Notes |
|-----------|-----------------|-------|
| .NET Framework 4.8 | `System.Text.Json` 10.0.5 | Microsoft documents package support for .NET Framework 4.6.2+; use package-based deployment, not runtime assumptions. |
| .NET Framework 4.8 | `Polly` 8.6.6 | Current Polly line supports .NET Framework 4.6.2+, so it fits this brownfield app. |
| .NET Framework 4.8 | `NJsonSchema` 11.5.2 | Practical fit for legacy validation work; use for contract validation, not for broad app-model generation. |

## Sources

- Local code: `/home/wuxf/Develop/openlearnsite/.planning/PROJECT.md` —
  current milestone scope and constraints
- Local code: `/home/wuxf/Develop/openlearnsite/teacher/aiprovider_api.ashx`
  — existing provider routing, generic chat endpoint, and custom skill CRUD
- Local code:
  `/home/wuxf/Develop/openlearnsite/App_Code/Bll/AIGaugeGenerator.cs` —
  existing dedicated AI generator + fallback pattern
- Local code:
  `/home/wuxf/Develop/openlearnsite/App_Code/Bll/AIStudentExamGenerator.cs`
  — existing dedicated AI workflow pattern
- Local code:
  `/home/wuxf/Develop/openlearnsite/App_Code/Model/AICustomSkill.cs` —
  existing scoped-skill model
- Official docs: https://docs.anthropic.com/en/docs/build-with-claude/tool-use
  — strict tool/schema pattern for structured outputs (**HIGH**)
- Official docs:
  https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/migrate-from-newtonsoft
  — current Microsoft guidance on serializer tradeoffs and support (**HIGH**)
- Official docs:
  https://learn.microsoft.com/en-us/dotnet/core/diagnostics/observability-with-otel
  — OpenTelemetry guidance for .NET (**HIGH**)
- Official docs via URL verification from search:
  https://platform.openai.com/docs/guides/structured-outputs — provider-
  native schema-constrained output pattern (**MEDIUM**; direct fetch blocked,
  recommendation verified indirectly)
- Product evidence: https://www.magicschool.ai/ — workflow-native teacher AI,
  privacy and integration emphasis (**MEDIUM**)
- Product evidence: https://www.eduaide.ai/ — objective/standards/source-
  grounded lesson builder pattern (**MEDIUM**)
- Product evidence: https://www.briskteaching.com/ — in-workflow teacher
  authoring integration pattern (**MEDIUM**)

---
*Stack research for: teacher lesson-planning skills inside OpenLearnSite*
*Researched: April 10, 2026*
