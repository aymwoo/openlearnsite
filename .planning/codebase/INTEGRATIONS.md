# External Integrations

**Analysis Date:** 2026-04-10

## APIs & External Services

This codebase primarily integrates with SQL Server and OpenAI-compatible AI
providers. Most other HTTP endpoints are internal `.ashx` handlers rather than
third-party services.

**AI providers:**
- OpenAI-compatible chat completion APIs - Teacher-facing provider management,
  test calls, chat, rubric generation, and exam assessment
  - SDK/Client: custom `HttpWebRequest` calls in `teacher/aiprovider_api.ashx`,
    `App_Code/Bll/AIGaugeGenerator.cs`, and
    `App_Code/Bll/AIStudentExamGenerator.cs`
  - Auth: `AIProvider.ApiKey` stored via `App_Code/Model/AIProvider.cs`
- Aliyun DashScope (seeded default option) - Seed data for AI provider setup
  - SDK/Client: OpenAI-compatible base URL seeded in
    `App_Code/Utility/UpdateGrade.cs`
  - Auth: `AIProvider.ApiKey`
- DeepSeek (seeded default option) - Seed data for AI provider setup and local
  activity branding under `deepseek/`
  - SDK/Client: OpenAI-compatible base URL seeded in
    `App_Code/Utility/UpdateGrade.cs`
  - Auth: `AIProvider.ApiKey`

**Remote content fetch:**
- Weather page source - Server-side weather text scraping in
  `App_Code/Common/Computer.cs`
  - SDK/Client: `WebRequest.Create`
  - Auth: no auth detected; URL read from `Weather` app setting

## Data Storage

The primary persistence layer is SQL Server. File assets, uploads, logs, and
backups stay on the local filesystem under the web root or mounted Docker
volumes.

**Databases:**
- Microsoft SQL Server
  - Connection: `SqlServer` connection string in `web.config` and
    `web.config.docker`
  - Client: `System.Data.SqlClient` via `App_Code/Utility/SqlHelper.cs`,
    `App_Code/Utility/DatabaseSetupHelper.cs`, and many `*.aspx.cs` files

**File Storage:**
- Local filesystem only
  - Homework storage configured from `Homework` in `web.config`
  - DB backups configured from `DbBackupPhysicalPath` in `web.config.docker`
    and used by `App_Code/Utility/DbBackup.cs`
  - Download and upload paths handled in `App_Code/Bll/Soft.cs`,
    `App_Code/Common/Fileupload.cs`, `webform/upimg.ashx`, and
    `student/upload*.ashx`

**Caching:**
- In-process ASP.NET session/application state and custom memory cache
  - Session-backed tokens in `api/ResourceProxy.ashx` and `api/LinkProxy.ashx`
  - Application state flags in `api/LabControlApi.ashx`
  - Model cache duration read through `App_Code/Common/ConfigHelper.cs`

## Authentication & Identity

Authentication is custom and cookie-based even though `web.config` declares a
Windows auth mode. App code consistently relies on its own student, teacher,
and manager cookies.

**Auth Provider:**
- Custom cookie/session auth
  - Implementation: login and auth checks in `App_Code/Common/CookieHelp.cs`,
    `index.aspx.cs`, `student/register.aspx.cs`, and handlers such as
    `teacher/aiprovider_api.ashx`

## Monitoring & Observability

Observability is local and file-based. No external monitoring SaaS integration
was detected.

**Error Tracking:**
- None detected

**Logs:**
- Local log files under `~/log/` managed by `App_Code/Common/Log.cs`

## CI/CD & Deployment

Deployment paths include Windows IIS and Linux containers. CI and image
publishing are implemented in GitHub Actions.

**Hosting:**
- Mono/XSP4 container deployment in `Dockerfile` and `docker-compose.yml`
- Windows IIS deployment documented in `README.md`
- GitHub Container Registry image publishing in
  `.github/workflows/build_and_push_docker.yml`

**CI Pipeline:**
- GitHub Actions workflows in `.github/workflows/`
  - Linux test path: `.github/workflows/run_test_for_linux.yml`
  - Docker test path: `.github/workflows/run_test_for_docker.yml`
  - Image publishing: `.github/workflows/build_and_push_docker.yml`

## Environment Configuration

Configuration is split between ASP.NET config files, Docker environment
variables, and CI runtime variables.

**Required env vars:**
- `DB_PASSWORD` - Rewrites the app DB password in `entrypoint.sh`
- `MONO_THREADS_PER_CPU` - Mono tuning in `Dockerfile` and `README.md`
- `BASE_URL` - Playwright test base URL in `playwright.config.ts`
- `MSSQL_SA_PASSWORD` - SQL Server container variable in `docker-compose.yml`
  and `.github/workflows/run_test_for_docker.yml`

**Secrets location:**
- Database connection settings are file-based in `web.config` and
  `web.config.docker`
- AI provider API keys are stored in the `AIProvider` table through
  `App_Code/Dal/AIProvider.cs` and administered via `teacher/aiprovider_api.ashx`
- GitHub package publishing uses repository-provided token injection in
  `.github/workflows/build_and_push_docker.yml`

## Webhooks & Callbacks

The repository exposes many internal AJAX handlers, but no third-party webhook
consumer was detected.

**Incoming:**
- None detected for external webhook providers
- Server-sent events stream for AI rubric generation in
  `teacher/gauge_generate.ashx`
- Internal AJAX/API handlers under `api/*.ashx`, `teacher/*.ashx`,
  `student/*.ashx`, and `webform/*.ashx`

**Outgoing:**
- POST requests to external AI providers at `BaseUrl + /chat/completions` from
  `teacher/aiprovider_api.ashx`, `App_Code/Bll/AIGaugeGenerator.cs`, and
  `App_Code/Bll/AIStudentExamGenerator.cs`
- Weather fetch via configured URL from `App_Code/Common/Computer.cs`

---

*Integration audit: 2026-04-10*
