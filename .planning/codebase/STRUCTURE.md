# Codebase Structure

**Analysis Date:** 2026-04-10

## Directory layout

This repository is a single deployable web application with role-based feature
folders at the root and shared server code in `App_Code/`. When adding files,
prefer the existing role or feature directory instead of creating a new top-
level area.

```text
openlearnsite/
├── App_Code/            # Shared server-side BLL, DAL, models, and helpers
├── App_Themes/          # Theme CSS and shared visual assets for Web Forms
├── api/                 # Cross-feature HTTP handlers for proxy-style APIs
├── manager/             # System administration pages and admin master page
├── teacher/             # Teacher console pages, handlers, and scripts
├── student/             # Student dashboard, lesson host pages, and uploads
├── exam/                # Exam management pages, question banks, and handlers
├── lessons/             # Pre-class lesson pages and lesson-specific master
├── python/              # Python activity pages and upload handlers
├── js/                  # Shared browser-side scripts and styles
├── code/                # Embedded coding runtimes and standalone activity UIs
├── ai/                  # Browser-side AI demos and activity assets
├── sql/                 # Database bootstrap scripts
├── Tests/               # C# test projects plus JS E2E and unit tests
├── scripts/             # Local startup, release, and test scripts
├── index.aspx           # Public login entry page
├── Global.asax          # Request lifecycle bootstrap
├── web.config           # ASP.NET application configuration
└── package.json         # JS test runner manifest
```

## Directory purposes

Each major directory has a stable role. New code should follow these placement
rules so later agents can discover it quickly.

**App_Code:**
- Purpose: Hold shared server-side code compiled automatically by ASP.NET.
- Contains: Business logic in `App_Code/Bll/`, repositories in
  `App_Code/Dal/`, entities in `App_Code/Model/`, helpers in
  `App_Code/Common/`, packaging helpers in `App_Code/Store/`, and DB utilities
  in `App_Code/Utility/`.
- Key files: `App_Code/Bll/Courses.cs`, `App_Code/Dal/Courses.cs`,
  `App_Code/Common/CookieHelp.cs`, `App_Code/Common/XmlHelp.cs`,
  `App_Code/Utility/DbHelperSQL.cs`.

**teacher:**
- Purpose: Host teacher-facing pages for classroom control, course authoring,
  grading, analytics, and configuration.
- Contains: Paired `.aspx` and `.aspx.cs` files, `Teach.master`, and feature
  handlers such as `teacher/Runpy.ashx` and `teacher/ware.ashx`.
- Key files: `teacher/start.aspx`, `teacher/start.aspx.cs`,
  `teacher/Teach.master`, `teacher/workshow.aspx`, `teacher/courseedit.aspx`.

**student:**
- Purpose: Host student-facing dashboard pages, course-step pages, uploads, and
  supporting handlers.
- Contains: Dashboard pages, course runtimes, `Stud.master`, `Scm.master`, and
  many upload or save handlers.
- Key files: `student/myinfo.aspx`, `student/showcourse.aspx`,
  `student/Stud.master`, `student/Scm.master`, `student/uploadwork.aspx.cs`.

**manager:**
- Purpose: Host administrator pages for global settings and school-year or data
  maintenance tasks.
- Contains: Settings, class setup, teacher management, import, backup, and DB
  upgrade pages.
- Key files: `manager/setting.aspx`, `manager/teacher.aspx`,
  `manager/upgrade.aspx`, `manager/Manage.master`.

**api:**
- Purpose: Provide application-wide handler endpoints used by AJAX or external
  access patterns.
- Contains: `.ashx` handlers.
- Key files: `api/ResourceProxy.ashx`, `api/ResourceAccess.ashx`,
  `api/ScoreProxy.ashx`, `api/LabLoginApi.ashx`.

**exam:**
- Purpose: Group exam workflows and exam-specific handlers outside the main
  student and teacher folders.
- Contains: Authoring, monitoring, result pages, and AJAX handlers.
- Key files: `exam/examlist.aspx`, `exam/examadd.aspx`,
  `exam/GetExamQuestions.ashx`, `exam/question/`.

**lessons:**
- Purpose: Hold pre-class or lesson-preparation flows with their own master
  page.
- Contains: `pre*.aspx` pages and `prescm.master`.
- Key files: `lessons/precourse.aspx`, `lessons/premission.aspx`,
  `lessons/presurvey.aspx`, `lessons/prescm.master`.

**python:**
- Purpose: Host Python-specific learning tools and code-upload flows.
- Contains: Practice, question, ranking, turtle, and match pages plus upload
  handlers.
- Key files: `python/index.aspx`, `python/code.aspx`, `python/manage.aspx`,
  `python/uploadcode.ashx`.

**js:**
- Purpose: Store shared front-end JavaScript and CSS referenced by many pages.
- Contains: Feature scripts, editor integrations, utilities, and vendored
  browser assets.
- Key files: `js/learnstatus.js`, `js/course.js`, `js/index.js`,
  `js/MenuCookie.js`, `js/css/tailwind.config.js`.

**code:**
- Purpose: Provide embedded activity runtimes and static HTML app shells used
  from lesson steps.
- Contains: Standalone HTML, JS runtimes, Python tools, and media.
- Key files: `code/blockpy.html`, `code/index.html`, `code/mqtt.html`,
  `code/skulpt.min.js`.

**ai:**
- Purpose: Bundle AI or ML browser experiences used by lesson activities.
- Contains: Standalone demos and activity subfolders.
- Key files: `ai/ai.html`, `ai/handnum/`, `ai/stylization/`, `ai/Tic_Tac_Toe/`.

**Tests:**
- Purpose: Hold automated tests for C#, Playwright, and Vitest.
- Contains: C# test projects plus JS tests in `Tests/e2e/` and `Tests/unit/`.
- Key files: `Tests/TeacherRegressionTests/TeacherRegressionTests.csproj`,
  `Tests/e2e/student-login.spec.js`, `Tests/unit/exam.test.js`.

**sql:**
- Purpose: Store database initialization scripts and upgrade notes.
- Contains: SQL bootstrap files and markdown notes.
- Key files: `sql/learnsite.sql`, `sql/learnsite_xg.sql`.

**scripts:**
- Purpose: Support local setup, startup, build, and test workflows.
- Contains: Shell scripts.
- Key files: `scripts/start_dev.sh`, `scripts/start_app.sh`,
  `scripts/run_tests.sh`.

## Key file locations

These locations answer “where is the thing that controls this area?” Use them
as anchors when extending an existing workflow.

**Entry points:**
- `index.aspx`: Public student login page.
- `index.aspx.cs`: Login flow, redirect logic, and upgrade checks.
- `Global.asax`: Request-wide encoding and content-type bootstrap.
- `teacher/start.aspx`: Teacher classroom control landing page.
- `student/myinfo.aspx`: Student home dashboard.
- `manager/setting.aspx`: Manager configuration page.

**Configuration:**
- `web.config`: ASP.NET runtime configuration and connection string location.
- `web.config.docker`: Container-oriented configuration override.
- `Global.asax`: Request defaults.
- `package.json`: JS test commands.
- `vitest.config.ts`: Vitest setup.
- `playwright.config.ts`: Playwright setup.

**Core logic:**
- `App_Code/Bll/`: Business operations.
- `App_Code/Dal/`: SQL access.
- `App_Code/Model/`: Entity and cookie/session models.
- `App_Code/Common/`: Shared helpers, uploads, previews, config, auth.
- `App_Code/Utility/`: Low-level DB helpers and migrations.

**Testing:**
- `Tests/`: Main automated test root.
- `Tests/e2e/`: Playwright specs.
- `Tests/unit/`: Vitest unit tests.
- `Tests/*/*.csproj`: C# regression and unit test projects.

## Naming conventions

Names are strongly shaped by ASP.NET Web Forms conventions and role folders.
New files should match those patterns so code-behind wiring keeps working.

**Files:**
- Web Forms pages use lowercase feature names with paired code-behind files:
  `teacher/start.aspx` + `teacher/start.aspx.cs`.
- Master pages use PascalCase-like names inside role folders:
  `teacher/Teach.master`, `manager/Manage.master`, `student/Stud.master`.
- Shared classes in `App_Code/` are singular or plural table/service names in
  PascalCase: `App_Code/Bll/Courses.cs`, `App_Code/Dal/Students.cs`.
- Handlers use descriptive PascalCase or mixed legacy names ending in `.ashx`:
  `api/ResourceProxy.ashx`, `student/FileManagerSql.ashx`.

**Directories:**
- Role-based top-level folders use lowercase names: `teacher/`, `student/`,
  `manager/`.
- Shared compile-time server code stays under `App_Code/` subfolders.
- Static asset directories use lowercase or lowercase-with-underscores:
  `js/`, `code/`, `ai/`, `lesson/`-style folders such as `lessons/`.

## Where to add new code

These placement rules are the most useful guidance for future implementation
work. Follow the nearest existing pattern rather than inventing a new subtree.

**New teacher feature:**
- Primary code: Add the page in `teacher/` as `feature.aspx` and
  `feature.aspx.cs`.
- Shared navigation/layout: Wire UI through `teacher/Teach.master` if the page
  belongs in the main console.
- Domain logic: Add or extend services in `App_Code/Bll/` and `App_Code/Dal/`.
- Tests: Add browser coverage in `Tests/e2e/` and C# coverage in an existing or
  new `Tests/*/*.csproj` project.

**New student lesson step or dashboard page:**
- Lesson step implementation: Add the page in `student/` and route to it from
  `student/Scm.master.cs` based on `Ltype` or existing step rules.
- Dashboard implementation: Add the page in `student/` and link it from
  `student/Stud.master`.
- Upload/background endpoint: Add an `.ashx` or upload page in `student/` when
  the endpoint is student-only.

**New shared business rule:**
- Primary logic: `App_Code/Bll/<Entity>.cs`.
- Persistence: `App_Code/Dal/<Entity>.cs`.
- Data contract: `App_Code/Model/<Entity>.cs`.
- Shared helper only: `App_Code/Common/` if the logic is cross-entity and not
  table-owned.

**New API or AJAX endpoint:**
- Cross-feature endpoint: `api/`.
- Role-specific endpoint: the owning role folder, for example `teacher/` or
  `student/`.
- Response shape: Match existing handler style and keep parsing plus auth checks
  inside the handler file.

**Utilities:**
- Shared helpers: `App_Code/Common/`.
- Database helper or migration support: `App_Code/Utility/`.
- Packaging or import/export support: `App_Code/Store/`.
- Browser-only utility: `js/`.

**Static activity assets:**
- Shared script or CSS: `js/`.
- Standalone coding runtime or HTML app: `code/`.
- AI activity asset: `ai/`.

## Special directories

Some directories are operational rather than feature-specific. Treat them with
care because they affect deployment, tests, or generated outputs.

**App_Code:**
- Purpose: Auto-compiled ASP.NET source tree.
- Generated: No.
- Committed: Yes.

**App_Themes:**
- Purpose: Theme assets used by master pages and legacy controls.
- Generated: No.
- Committed: Yes.

**Bin:**
- Purpose: Compiled assemblies for runtime deployment.
- Generated: Yes.
- Committed: Yes in the current repository layout.

**TestResults:**
- Purpose: Test output artifacts.
- Generated: Yes.
- Committed: Yes in the current repository layout.

**backupdb:**
- Purpose: Database backup storage.
- Generated: Yes.
- Committed: Yes in the current repository layout.

**artifacts:**
- Purpose: Build or release outputs.
- Generated: Yes.
- Committed: Yes in the current repository layout.

**node_modules:**
- Purpose: Installed JS dependencies for tests and tooling.
- Generated: Yes.
- Committed: Yes in the current repository layout.

**.planning/codebase:**
- Purpose: Generated repository analysis documents.
- Generated: Yes.
- Committed: Intended for planning workflows.

---

*Structure analysis: 2026-04-10*
