# Technology Stack

**Analysis Date:** 2026-04-10

## Languages

This repository is a mixed Web Forms application with a C# server core and a
large browser-side JavaScript surface. Supporting test, automation, and AI demo
code adds TypeScript, Python, SQL, and container configuration.

**Primary:**
- C# / ASP.NET Web Forms - Main application code in `App_Code/**/*.cs`,
  page code-behind files such as `index.aspx.cs`, and handlers such as
  `teacher/aiprovider_api.ashx`
- JavaScript - Frontend behavior in `js/*.js`, `webform/*.js`,
  `teacher/editor-upload-helper.js`, and feature pages under `deepseek/`

**Secondary:**
- TypeScript - Test runner configuration in `vitest.config.ts` and
  `playwright.config.ts`
- SQL / T-SQL - Database bootstrap and migrations referenced from
  `sql/learnsite.sql`, `upgrade.aspx.cs`, and
  `App_Code/Utility/DatabaseSetupHelper.cs`
- Python - Auxiliary AI and plugin services in `ai/webcanvas/*.py`,
  `luckysheetbottle/web.py`, and `plugins/luckysheet/requirements.txt`

## Runtime

The production app runs as classic ASP.NET Web Forms. Linux deployment uses
Mono with XSP4, while Windows deployment uses IIS with .NET Framework.

**Environment:**
- .NET Framework 4.8 - Declared in `web.config` and `web.config.docker`
- Mono + XSP4 - Container runtime in `Dockerfile` and `entrypoint.sh`
- Node.js 18.x - CI test runtime in `.github/workflows/run_test_for_linux.yml`
- SQL Server 2022 - Container and CI database target in `docker-compose.yml`
  and `.github/workflows/run_test_for_docker.yml`

**Package Manager:**
- npm (version not pinned) - Root test toolchain in `package.json`
- Lockfile: present in `package-lock.json`
- yarn (version not pinned) - AI demo subproject in `ai/stylization/package.json`
  with lockfile `ai/stylization/yarn.lock`

## Frameworks

The stack is centered on Web Forms and handler-based HTTP endpoints, with a
modern JS test toolchain added alongside legacy and bundled frontend assets.

**Core:**
- ASP.NET Web Forms - Page and handler architecture in `*.aspx`, `*.aspx.cs`,
  `Global.asax`, and `App_Code/`
- Mono XSP4 - Linux-hosted ASP.NET server in `Dockerfile` and `entrypoint.sh`

**Testing:**
- Vitest 3.0.0 - Frontend unit tests from `package.json` and `vitest.config.ts`
- Playwright 1.50.0 - Browser E2E tests from `package.json` and
  `playwright.config.ts`
- xUnit 2.9.3 - .NET logic tests in `Tests/CommonLogicTests/CommonLogicTests.csproj`

**Build/Dev:**
- Docker - Container packaging in `Dockerfile` and `docker-compose.yml`
- GitHub Actions - CI/CD workflows in `.github/workflows/*.yml`
- Visual Studio solution / MSBuild - Solution orchestration in
  `openlearnsite.sln`
- Browserify/Budo - TFJS demo build tooling in `ai/stylization/package.json`

## Key Dependencies

The repository mixes NuGet-era binary dependencies checked into `Bin/`, npm
test tooling, and a few standalone Python requirements.

**Critical:**
- `Newtonsoft.Json` 13.0.3 - JSON serialization for AI provider calls and tests
  in `App_Code/Bll/AIStudentExamGenerator.cs`,
  `App_Code/Bll/AIGaugeGenerator.cs`, and
  `Tests/CommonLogicTests/CommonLogicTests.csproj`
- `System.Data.SqlClient` - SQL Server access across
  `App_Code/Utility/SqlHelper.cs`, `teacher/check.aspx.cs`, and
  `App_Code/Utility/DatabaseSetupHelper.cs`
- `@playwright/test` ^1.50.0 - E2E automation in `package.json`
- `vitest` ^3.0.0 - Unit test runner in `package.json`

**Infrastructure:**
- `log4net.dll` - Bundled logging library in `Bin/log4net.dll`
- `NPOI*.dll` - Office and spreadsheet handling in `Bin/NPOI.dll` and related
  assemblies
- `ICSharpCode.SharpZipLib.dll` - Archive support in
  `Bin/ICSharpCode.SharpZipLib.dll`
- `@tensorflow/tfjs` ~1.0.0 - Browser ML demo in `ai/stylization/package.json`
- `flask`, `flask-cors`, `tensorflow`, `tensorflowjs` - Python AI demo service
  stack in `ai/webcanvas/requirements.txt`

## Configuration

Configuration is file-based for the web app and env-based for containerized
deployment and CI.

**Environment:**
- Main app settings and connection strings live in `web.config`
- Docker-specific ASP.NET settings live in `web.config.docker`
- Container startup rewrites DB credentials from `DB_PASSWORD` in
  `entrypoint.sh`
- App behavior also reads settings such as `Homework`, `Download`, `Weather`,
  `ModelCache`, and `DbBackupPhysicalPath` from `web.config` and helper code in
  `App_Code/Common/ConfigHelper.cs` and `App_Code/Common/Computer.cs`
- `.env` files were not detected at repository root

**Build:**
- Container build config: `Dockerfile`, `docker-compose.yml`
- Test config: `vitest.config.ts`, `playwright.config.ts`
- Solution/build metadata: `openlearnsite.sln`,
  `Tests/CommonLogicTests/CommonLogicTests.csproj`, `Benchmark/Benchmark.csproj`

## Platform Requirements

The project supports both classic Windows hosting and Linux container hosting.

**Development:**
- SQL Server available to the app via the `SqlServer` connection string in
  `web.config`
- Mono `mono-complete` and `mono-xsp4` for Linux workflows in
  `.github/workflows/run_test_for_linux.yml`
- Node.js for Vitest and Playwright from `package.json`
- Optional .NET SDKs for auxiliary projects and tests, including net48, net8.0,
  and a benchmark target in `Tests/CommonLogicTests/CommonLogicTests.csproj`
  and `Benchmark/Benchmark.csproj`

**Production:**
- Linux container target: Mono + XSP4 serving `/app` on port 8080 in
  `Dockerfile`
- Windows target: IIS with .NET Framework 4.8 as documented in `README.md`

---

*Stack analysis: 2026-04-10*
