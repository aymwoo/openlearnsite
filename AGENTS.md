<!-- GSD:project-start source:PROJECT.md -->
## Project

**OpenLearnSite teaching skills enhancement**

This project extends the existing OpenLearnSite teacher workflow with stronger
teaching-oriented AI skills. It focuses on the teacher-facing lesson and course
editing experience, so a teacher can enter a topic or knowledge point and get
an executable classroom activity plan that is easier to use in real teaching.

**Core Value:** Teachers can turn a teaching topic into a concrete, teachable activity plan
without having to manually break the lesson into steps.

### Constraints

- **Tech stack**: Build on the existing ASP.NET Web Forms architecture and the
  current teacher pages — this must fit the established brownfield system
- **Integration**: Reuse the existing AI provider and skill infrastructure —
  the system already has provider routing and scoped AI skill concepts
- **Scope**: Deliver a narrow v1 around activity-plan generation first — the
  goal is to solve the highest-value planning gap before expanding editing,
  reuse, or publishing flows
- **User input**: The minimum teacher input for v1 is a topic or knowledge point
  — the experience must stay lightweight enough for real teacher preparation
- **Human review**: Generated teaching plans remain teacher-reviewed content —
  the system supports planning, not unsupervised instructional decisions
<!-- GSD:project-end -->

<!-- GSD:stack-start source:codebase/STACK.md -->
## Technology Stack

## Languages
- C# / ASP.NET Web Forms - Main application code in `App_Code/**/*.cs`,
- JavaScript - Frontend behavior in `js/*.js`, `webform/*.js`,
- TypeScript - Test runner configuration in `vitest.config.ts` and
- SQL / T-SQL - Database bootstrap and migrations referenced from
- Python - Auxiliary AI and plugin services in `ai/webcanvas/*.py`,
## Runtime
- .NET Framework 4.8 - Declared in `web.config` and `web.config.docker`
- Mono + XSP4 - Container runtime in `Dockerfile` and `entrypoint.sh`
- Node.js 18.x - CI test runtime in `.github/workflows/run_test_for_linux.yml`
- SQL Server 2022 - Container and CI database target in `docker-compose.yml`
- npm (version not pinned) - Root test toolchain in `package.json`
- Lockfile: present in `package-lock.json`
- yarn (version not pinned) - AI demo subproject in `ai/stylization/package.json`
## Frameworks
- ASP.NET Web Forms - Page and handler architecture in `*.aspx`, `*.aspx.cs`,
- Mono XSP4 - Linux-hosted ASP.NET server in `Dockerfile` and `entrypoint.sh`
- Vitest 3.0.0 - Frontend unit tests from `package.json` and `vitest.config.ts`
- Playwright 1.50.0 - Browser E2E tests from `package.json` and
- xUnit 2.9.3 - .NET logic tests in `Tests/CommonLogicTests/CommonLogicTests.csproj`
- Docker - Container packaging in `Dockerfile` and `docker-compose.yml`
- GitHub Actions - CI/CD workflows in `.github/workflows/*.yml`
- Visual Studio solution / MSBuild - Solution orchestration in
- Browserify/Budo - TFJS demo build tooling in `ai/stylization/package.json`
## Key Dependencies
- `Newtonsoft.Json` 13.0.3 - JSON serialization for AI provider calls and tests
- `System.Data.SqlClient` - SQL Server access across
- `@playwright/test` ^1.50.0 - E2E automation in `package.json`
- `vitest` ^3.0.0 - Unit test runner in `package.json`
- `log4net.dll` - Bundled logging library in `Bin/log4net.dll`
- `NPOI*.dll` - Office and spreadsheet handling in `Bin/NPOI.dll` and related
- `ICSharpCode.SharpZipLib.dll` - Archive support in
- `@tensorflow/tfjs` ~1.0.0 - Browser ML demo in `ai/stylization/package.json`
- `flask`, `flask-cors`, `tensorflow`, `tensorflowjs` - Python AI demo service
## Configuration
- Main app settings and connection strings live in `web.config`
- Docker-specific ASP.NET settings live in `web.config.docker`
- Container startup rewrites DB credentials from `DB_PASSWORD` in
- App behavior also reads settings such as `Homework`, `Download`, `Weather`,
- `.env` files were not detected at repository root
- Container build config: `Dockerfile`, `docker-compose.yml`
- Test config: `vitest.config.ts`, `playwright.config.ts`
- Solution/build metadata: `openlearnsite.sln`,
## Platform Requirements
- SQL Server available to the app via the `SqlServer` connection string in
- Mono `mono-complete` and `mono-xsp4` for Linux workflows in
- Node.js for Vitest and Playwright from `package.json`
- Optional .NET SDKs for auxiliary projects and tests, including net48, net8.0,
- Linux container target: Mono + XSP4 serving `/app` on port 8080 in
- Windows target: IIS with .NET Framework 4.8 as documented in `README.md`
<!-- GSD:stack-end -->

<!-- GSD:conventions-start source:CONVENTIONS.md -->
## Conventions

## Naming Patterns
- Use lowercase WebForms page names for teacher, student, manager, and webform
- Use PascalCase class filenames under `App_Code/` when the file maps to a C#
- Use descriptive lowercase or kebab-style JavaScript filenames for browser
- Use `*.test.js` for Vitest unit tests in `Tests/unit/` and `*.spec.js` for
- Use PascalCase for C# methods and page helpers, such as `Page_Load`,
- Keep ASP.NET event handlers in the `Control_Event` form, such as
- Use camelCase for JavaScript functions, such as `updateScore`,
- Use underscore-prefixed private fields in C# test classes, such as `_tempDir`,
- Use local variable names that mirror domain fields in C# code-behind, even
- Use `const` and `let` in modern JavaScript test and config files, such as
- Keep object keys aligned with persisted or transmitted field names in browser
- Use PascalCase for C# classes and models, such as `CookieHelp`,
- Use namespace names that match the feature or test project, such as
- Use plain object literals instead of formal type declarations in browser
## Code Style
- Root formatter config is not detected. No root `.prettierrc`, `prettier`
- Use four-space indentation and Allman-style braces in C# files, as shown in
- Use semicolon-terminated statements in C# and JavaScript.
- Use two-space indentation in modern TypeScript and JS config and test files,
- Use inline Chinese comments and section dividers when the surrounding file
- Repository-wide linting is not detected.
- One nested third-party area has its own ESLint config at
- In practice, correctness is enforced more by tests than by lint rules. Follow
## Import Organization
- No JS or TS path aliases are detected in the root test setup.
- Use relative imports in test files, such as `./auth.helper.js` in
- Use namespace imports in C# instead of filesystem aliases, such as
## Error Handling
- Use guard clauses for null, empty, and missing data before deeper processing,
- Use broad `try/catch (Exception ex)` when interacting with ASP.NET request,
- Return `false`, `0`, or `null` for invalid input in utility code instead of
- Throw only when callers already expect exception-driven validation, as shown by
## Logging
- Use `System.Diagnostics.Trace.WriteLine(...)` for server-side cleanup or cookie
- Use `console.log(...)` and `console.warn(...)` for browser-side runtime
- Keep user-facing failure feedback separate from logs. Browser upload helpers in
## Comments
- Add comments for WebForms-specific behavior that is not obvious from the code,
- Add comments for data shape or scoring rules in exam flows, such as the
- Use test comments to separate sections and document scenario intent, such as
- Use JSDoc-style block comments in JavaScript tests and helpers when the file is
- XML doc comments exist in older C# utility code, such as
## Function Design
- Accept long page/controller methods in WebForms files when they orchestrate UI
- Extract pure logic only when it needs isolated tests, as shown by
- Use strongly ordered primitive parameters in legacy C# helpers, such as
- Use object parameters in JavaScript when many related values travel together,
- Return booleans for validation and success checks, such as `SetPPTistSnum`,
- Return numeric scores directly for scoring helpers, such as
- Return structured objects only when multiple fields are needed downstream, such
## Module Design
- Use implicit class exposure in C# via one primary type per file, such as
- Use IIFE-based globals for browser helpers that must be callable from ASPX
- Use explicit ES module exports only in test-only extraction files, such as
- Barrel files are not used.
- Reference concrete files directly, such as `./auth.helper.js` from
<!-- GSD:conventions-end -->

<!-- GSD:architecture-start source:ARCHITECTURE.md -->
## Architecture

## Pattern overview
- Route handling is file-based. Entry pages such as `index.aspx`,
- Shared business logic is centralized in `App_Code/Bll/*.cs`, which wraps
- UI composition is role-specific. Master pages in `teacher/Teach.master`,
## Layers
- Purpose: Render HTML, bind server controls, respond to page lifecycle events,
- Location: Root `.aspx` pages plus feature folders such as `teacher/`,
- Contains: `.aspx`, `.master`, `.aspx.cs`, and some `.ashx` endpoints such as
- Depends on: `LearnSite.BLL`, `LearnSite.Common`, cookies, session, and some
- Used by: Browser requests and AJAX calls.
- Purpose: Encapsulate use-case logic around courses, students, sign-in,
- Location: `App_Code/Bll/`.
- Contains: Service-style wrappers such as `App_Code/Bll/Courses.cs`,
- Depends on: `App_Code/Dal/*.cs`, `App_Code/Model/*.cs`, and shared helpers
- Used by: Page code-behind files and some handlers such as
- Purpose: Translate application operations into SQL queries and result mapping.
- Location: `App_Code/Dal/`.
- Contains: Table-oriented repositories such as `App_Code/Dal/Courses.cs`,
- Depends on: `LearnSite.DBUtility.DbHelperSQL`, `LearnSite.DBUtility.SqlHelper`,
- Used by: Matching BLL classes.
- Purpose: Carry table-shaped entities and cookie/session payloads between
- Location: `App_Code/Model/`.
- Contains: POCO-like classes such as `App_Code/Model/Courses.cs`,
- Depends on: Minimal framework types.
- Used by: BLL, DAL, and presentation code.
- Purpose: Provide cross-cutting helpers for cookies, XML config, file paths,
- Location: `App_Code/Common/`, `App_Code/Store/`, and `App_Code/Utility/`.
- Contains: Helpers such as `App_Code/Common/CookieHelp.cs`,
- Depends on: ASP.NET runtime, filesystem, XML config, and SQL Server.
- Used by: All other layers.
- Purpose: Deliver JavaScript, styles, editors, and browser-side tools used by
- Location: `js/`, `code/`, `ai/`, `plugins/`, `kindeditor/`, and feature
- Contains: Shared scripts such as `js/learnstatus.js`, `js/course.js`, and
- Depends on: Browser runtime and server pages that reference the assets.
- Used by: Master pages and activity pages.
## Data flow
- Authentication state lives in cookies modeled by `App_Code/Model/Cook.cs`,
- Short-lived workflow state uses `Session`, for example in
- Global online-user and feature-toggle state uses `Application`, for example
## Key abstractions
- Purpose: Represent a lesson shell with title, grade, term, content, publish
- Examples: `App_Code/Model/Courses.cs`, `App_Code/Bll/Courses.cs`,
- Pattern: BLL facade over DAL plus direct page binding.
- Purpose: Represent a step inside a course, with `Ltype` deciding which page
- Examples: `App_Code/Bll/ListMenu.cs`, `student/Scm.master.cs`,
- Pattern: Menu-driven polymorphism based on numeric type codes.
- Purpose: Represent teacher-to-class assignment, in-class switches, login
- Examples: `App_Code/Bll/Room.cs`, `App_Code/Model/Room.cs`,
- Pattern: Persistent room record plus session flags for active teaching.
- Purpose: Normalize student, teacher, and manager identity across requests.
- Examples: `App_Code/Model/Cook.cs`, `App_Code/Model/TeaCook.cs`,
- Pattern: Cookie-backed session model populated in page code.
- Purpose: Store uploaded student output and track completion for each lesson
- Examples: `App_Code/Bll/Works.cs`, `App_Code/Model/Works.cs`,
- Pattern: File-save plus metadata row plus preview strategy by `Wtype`.
- Purpose: Auto-pick the current or next class for teacher workflows.
- Examples: `App_Code/Bll/Courses.cs`, `App_Code/Dal/Courses.cs`,
- Pattern: SQL-backed schedule lookup joined to room ownership.
## Entry points
- Location: `index.aspx` and `index.aspx.cs`
- Triggers: Initial browser access.
- Responsibilities: Render site title, validate login, initialize sign-in, and
- Location: `Global.asax`
- Triggers: Every request.
- Responsibilities: Enforce UTF-8 response encoding and default HTML content
- Location: `teacher/start.aspx` and `teacher/start.aspx.cs`
- Triggers: Teacher navigation after authentication.
- Responsibilities: Select classroom context, start class, display live sign-in,
- Location: `student/myinfo.aspx` and `student/myinfo.aspx.cs`
- Triggers: Student landing after login and logout return target.
- Responsibilities: Show profile, online classmates, current and completed
- Location: `student/showcourse.aspx` and `student/showcourse.aspx.cs`
- Triggers: Student entry into a selected course.
- Responsibilities: Load course content and let `student/Scm.master` build the
- Location: `manager/setting.aspx` and `manager/setting.aspx.cs`
- Triggers: Manager navigation.
- Responsibilities: Edit XML-backed system settings and trigger global actions
- Location: `api/*.ashx`, feature handlers such as `student/*.ashx`,
- Triggers: AJAX calls, uploads, and browser-side polling.
- Responsibilities: Return JSON, stream files, proxy resources, and handle
## Error handling
- Cookie and authorization checks happen early through
- Setup validation in `index.aspx.cs` redirects to `upgrade.aspx` when the
- Handlers such as `api/ResourceProxy.ashx` wrap `ProcessRequest` in `try/catch`
- Some legacy code catches exceptions and suppresses details, for example in
## Cross-cutting concerns
<!-- GSD:architecture-end -->

<!-- GSD:skills-start source:skills/ -->
## Project Skills

No project skills found. Add skills to any of: `.claude/skills/`, `.agents/skills/`, `.cursor/skills/`, or `.github/skills/` with a `SKILL.md` index file.
<!-- GSD:skills-end -->

<!-- GSD:workflow-start source:GSD defaults -->
## GSD Workflow Enforcement

Before using Edit, Write, or other file-changing tools, start work through a GSD command so planning artifacts and execution context stay in sync.

Use these entry points:
- `/gsd-quick` for small fixes, doc updates, and ad-hoc tasks
- `/gsd-debug` for investigation and bug fixing
- `/gsd-execute-phase` for planned phase work

Do not make direct repo edits outside a GSD workflow unless the user explicitly asks to bypass it.
<!-- GSD:workflow-end -->



<!-- GSD:profile-start -->
## Developer Profile

> Profile not yet configured. Run `/gsd-profile-user` to generate your developer profile.
> This section is managed by `generate-claude-profile` -- do not edit manually.
<!-- GSD:profile-end -->
