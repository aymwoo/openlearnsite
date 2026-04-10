# Architecture

**Analysis Date:** 2026-04-10

## Pattern overview

This codebase uses an ASP.NET Web Forms monolith with page-first feature
composition. Runtime behavior centers on `.aspx` pages and `.ashx` handlers,
while reusable domain logic lives under `App_Code/` in BLL, DAL, Model,
Common, Store, and Utility layers.

**Overall:** Layered Web Forms monolith with page controllers and shared
domain services.

**Key characteristics:**
- Route handling is file-based. Entry pages such as `index.aspx`,
  `teacher/start.aspx`, `student/myinfo.aspx`, and `manager/setting.aspx`
  execute directly through paired code-behind files.
- Shared business logic is centralized in `App_Code/Bll/*.cs`, which wraps
  `App_Code/Dal/*.cs` table access and returns `App_Code/Model/*.cs` objects.
- UI composition is role-specific. Master pages in `teacher/Teach.master`,
  `student/Stud.master`, `student/Scm.master`, and `manager/Manage.master`
  provide layout, navigation, and shared client assets.

## Layers

The repository organizes runtime code into clear but loosely enforced layers.
When adding behavior, keep dependencies flowing from page or handler code into
BLL, then DAL, then database helpers.

**Presentation layer:**
- Purpose: Render HTML, bind server controls, respond to page lifecycle events,
  and coordinate user interactions.
- Location: Root `.aspx` pages plus feature folders such as `teacher/`,
  `student/`, `manager/`, `exam/`, `python/`, `lessons/`, and `profile/`.
- Contains: `.aspx`, `.master`, `.aspx.cs`, and some `.ashx` endpoints such as
  `teacher/Runpy.ashx` and `student/uploadpython.ashx`.
- Depends on: `LearnSite.BLL`, `LearnSite.Common`, cookies, session, and some
  direct `LearnSite.DBUtility` calls.
- Used by: Browser requests and AJAX calls.

**Business logic layer:**
- Purpose: Encapsulate use-case logic around courses, students, sign-in,
  scoring, resources, and course navigation.
- Location: `App_Code/Bll/`.
- Contains: Service-style wrappers such as `App_Code/Bll/Courses.cs`,
  `App_Code/Bll/Students.cs`, `App_Code/Bll/Signin.cs`,
  `App_Code/Bll/StudentScoreService.cs`, and `App_Code/Bll/CourseScheduleBLL.cs`.
- Depends on: `App_Code/Dal/*.cs`, `App_Code/Model/*.cs`, and shared helpers
  from `App_Code/Common/`.
- Used by: Page code-behind files and some handlers such as
  `api/ScoreProxy.ashx` and `api/ResourceProxy.ashx`.

**Data access layer:**
- Purpose: Translate application operations into SQL queries and result mapping.
- Location: `App_Code/Dal/`.
- Contains: Table-oriented repositories such as `App_Code/Dal/Courses.cs`,
  `App_Code/Dal/Students.cs`, `App_Code/Dal/Works.cs`, and
  `App_Code/Dal/CourseScheduleDAL.cs`.
- Depends on: `LearnSite.DBUtility.DbHelperSQL`, `LearnSite.DBUtility.SqlHelper`,
  `System.Data.SqlClient`, and `App_Code/Model/*.cs`.
- Used by: Matching BLL classes.

**Domain model layer:**
- Purpose: Carry table-shaped entities and cookie/session payloads between
  layers.
- Location: `App_Code/Model/`.
- Contains: POCO-like classes such as `App_Code/Model/Courses.cs`,
  `App_Code/Model/Students.cs`, `App_Code/Model/Room.cs`,
  `App_Code/Model/Cook.cs`, and `App_Code/Model/TeaCook.cs`.
- Depends on: Minimal framework types.
- Used by: BLL, DAL, and presentation code.

**Shared infrastructure layer:**
- Purpose: Provide cross-cutting helpers for cookies, XML config, file paths,
  preview rendering, uploads, logging, and database operations.
- Location: `App_Code/Common/`, `App_Code/Store/`, and `App_Code/Utility/`.
- Contains: Helpers such as `App_Code/Common/CookieHelp.cs`,
  `App_Code/Common/XmlHelp.cs`, `App_Code/Common/ViewPage.cs`,
  `App_Code/Common/WorkUpload.cs`, `App_Code/Store/CourseStore.cs`, and
  `App_Code/Utility/DbHelperSQL.cs`.
- Depends on: ASP.NET runtime, filesystem, XML config, and SQL Server.
- Used by: All other layers.

**Client asset layer:**
- Purpose: Deliver JavaScript, styles, editors, and browser-side tools used by
  pages and embedded activities.
- Location: `js/`, `code/`, `ai/`, `plugins/`, `kindeditor/`, and feature
  folders that ship standalone assets.
- Contains: Shared scripts such as `js/learnstatus.js`, `js/course.js`, and
  `js/MenuCookie.js`, plus embedded activity runtimes in `code/` and `ai/`.
- Depends on: Browser runtime and server pages that reference the assets.
- Used by: Master pages and activity pages.

## Data flow

The dominant flow is request-driven. New server behavior should follow these
existing paths instead of bypassing the shared layers.

**Page request flow:**

1. IIS or XSP routes a request to a file such as `student/showcourse.aspx` or
   `teacher/start.aspx`.
2. The paired code-behind file validates cookies or session state through
   helpers such as `App_Code/Common/CookieHelp.cs`.
3. The page calls BLL classes such as `App_Code/Bll/Courses.cs` or
   `App_Code/Bll/Signin.cs` to load or mutate application data.
4. BLL classes delegate SQL work to DAL classes such as
   `App_Code/Dal/Courses.cs`.
5. DAL classes execute SQL through `App_Code/Utility/DbHelperSQL.cs` and map
   results back into `App_Code/Model/*.cs` instances or `DataTable` objects.
6. The page binds the result to server controls or injects HTML into the page.

**Student lesson navigation flow:**

1. `index.aspx.cs` authenticates a student and redirects into
   `student/showcourse.aspx` or `student/myinfo.aspx`.
2. `student/showcourse.aspx.cs` loads lesson HTML from
   `App_Code/Bll/Courses.cs`.
3. `student/Scm.master.cs` resolves lesson steps through
   `App_Code/Bll/ListMenu.cs` and builds the lesson navigation menu.
4. Step pages such as `student/program.aspx`, `student/myexam.aspx`, and
   `student/topicdiscuss.aspx` handle each activity type.
5. Client reporting posts status through `js/learnstatus.js` using values
   emitted by `student/Scm.master.cs`.

**Teacher classroom control flow:**

1. `teacher/start.aspx.cs` verifies teacher cookies and initializes grade,
   class, room, and course selection.
2. It queries `App_Code/Bll/Room.cs`, `App_Code/Bll/Courses.cs`,
   `App_Code/Bll/Signin.cs`, and `App_Code/Bll/Students.cs` to determine live
   class state.
3. Starting class updates room state, sets session flags, and records teaching
   activity.
4. The page refreshes sign-in, unfinished work, and navigation links for
   downstream teacher pages such as `teacher/workshow.aspx` and
   `teacher/coursetotal.aspx`.

**Upload and resource access flow:**

1. Browser-side activity pages submit work to endpoints such as
   `student/uploadwork.aspx`, `student/uploadpython.ashx`, and
   `student/uploadmedia.ashx`.
2. Upload code uses `App_Code/Common/WorkUpload.cs` to resolve storage paths
   and `App_Code/Bll/Works.cs` plus `App_Code/Bll/MenuWorks.cs` to persist
   metadata.
3. Resource access uses `api/ScoreProxy.ashx` to enforce score-based access and
   `api/ResourceProxy.ashx` plus `api/ResourceAccess.ashx` to mint and consume
   short-lived session tokens.

**State management:**
- Authentication state lives in cookies modeled by `App_Code/Model/Cook.cs`,
  `App_Code/Model/TeaCook.cs`, and `App_Code/Model/MngCook.cs`.
- Short-lived workflow state uses `Session`, for example in
  `teacher/start.aspx.cs` and `student/Scm.master.cs`.
- Global online-user and feature-toggle state uses `Application`, for example
  via `App_Code/Common/App.cs` and checks in `student/Stud.master.cs`.

## Key abstractions

These abstractions shape most of the system. Reuse them instead of introducing
parallel concepts.

**Course:**
- Purpose: Represent a lesson shell with title, grade, term, content, publish
  state, and banner.
- Examples: `App_Code/Model/Courses.cs`, `App_Code/Bll/Courses.cs`,
  `App_Code/Dal/Courses.cs`.
- Pattern: BLL facade over DAL plus direct page binding.

**ListMenu lesson node:**
- Purpose: Represent a step inside a course, with `Ltype` deciding which page
  or activity runtime to open.
- Examples: `App_Code/Bll/ListMenu.cs`, `student/Scm.master.cs`,
  `teacher/start.aspx.cs`.
- Pattern: Menu-driven polymorphism based on numeric type codes.

**Room and live class state:**
- Purpose: Represent teacher-to-class assignment, in-class switches, login
  locking, and active course selection.
- Examples: `App_Code/Bll/Room.cs`, `App_Code/Model/Room.cs`,
  `teacher/start.aspx.cs`.
- Pattern: Persistent room record plus session flags for active teaching.

**User cookie models:**
- Purpose: Normalize student, teacher, and manager identity across requests.
- Examples: `App_Code/Model/Cook.cs`, `App_Code/Model/TeaCook.cs`,
  `App_Code/Common/CookieHelp.cs`.
- Pattern: Cookie-backed session model populated in page code.

**Work submission:**
- Purpose: Store uploaded student output and track completion for each lesson
  step.
- Examples: `App_Code/Bll/Works.cs`, `App_Code/Model/Works.cs`,
  `student/uploadwork.aspx.cs`, `App_Code/Common/ViewPage.cs`.
- Pattern: File-save plus metadata row plus preview strategy by `Wtype`.

**Schedule-backed smart selection:**
- Purpose: Auto-pick the current or next class for teacher workflows.
- Examples: `App_Code/Bll/Courses.cs`, `App_Code/Dal/Courses.cs`,
  `teacher/start.aspx.cs`.
- Pattern: SQL-backed schedule lookup joined to room ownership.

## Entry points

The application exposes multiple top-level entry points. New features should
attach to the closest existing role or activity area.

**Public login page:**
- Location: `index.aspx` and `index.aspx.cs`
- Triggers: Initial browser access.
- Responsibilities: Render site title, validate login, initialize sign-in, and
  redirect students to their current course or dashboard.

**Application bootstrap:**
- Location: `Global.asax`
- Triggers: Every request.
- Responsibilities: Enforce UTF-8 response encoding and default HTML content
  type.

**Teacher console:**
- Location: `teacher/start.aspx` and `teacher/start.aspx.cs`
- Triggers: Teacher navigation after authentication.
- Responsibilities: Select classroom context, start class, display live sign-in,
  and link to teacher management workflows.

**Student dashboard:**
- Location: `student/myinfo.aspx` and `student/myinfo.aspx.cs`
- Triggers: Student landing after login and logout return target.
- Responsibilities: Show profile, online classmates, current and completed
  courses, and student-level metrics.

**Student course host:**
- Location: `student/showcourse.aspx` and `student/showcourse.aspx.cs`
- Triggers: Student entry into a selected course.
- Responsibilities: Load course content and let `student/Scm.master` build the
  lesson-step navigation shell.

**Admin console:**
- Location: `manager/setting.aspx` and `manager/setting.aspx.cs`
- Triggers: Manager navigation.
- Responsibilities: Edit XML-backed system settings and trigger global actions
  such as course unpublish.

**API and AJAX handlers:**
- Location: `api/*.ashx`, feature handlers such as `student/*.ashx`,
  `teacher/*.ashx`, `exam/*.ashx`, and `python/*.ashx`
- Triggers: AJAX calls, uploads, and browser-side polling.
- Responsibilities: Return JSON, stream files, proxy resources, and handle
  background operations outside full page renders.

## Error handling

Error handling is pragmatic and localized rather than centralized. New code
should follow the safer patterns already present in the newer handlers and BLL
methods.

**Strategy:** Per-page and per-handler guard clauses with fallback redirects or
inline messages.

**Patterns:**
- Cookie and authorization checks happen early through
  `App_Code/Common/CookieHelp.cs`, then pages redirect when the user is not
  valid.
- Setup validation in `index.aspx.cs` redirects to `upgrade.aspx` when the
  database is missing or schema checks fail.
- Handlers such as `api/ResourceProxy.ashx` wrap `ProcessRequest` in `try/catch`
  and return JSON error payloads.
- Some legacy code catches exceptions and suppresses details, for example in
  `index.aspx.cs` and `api/ScoreProxy.ashx`.

## Cross-cutting concerns

Several concerns appear across the entire codebase. Keep new implementations in
these shared utilities instead of duplicating behavior inside pages.

**Logging:**
Logging helpers live in `App_Code/Common/Log.cs`, but many pages rely on inline
messages or silent catches instead of centralized structured logging.

**Validation:**
Input validation is mostly manual. Pages and handlers use helper checks such as
`LearnSite.Common.WordProcess.IsNum()` and `int.TryParse`, as seen in
`student/showcourse.aspx.cs`, `index.aspx.cs`, and `api/ResourceProxy.ashx`.

**Authentication:**
Role checks are cookie-based through `App_Code/Common/CookieHelp.cs`, with role
specific cookies consumed by student, teacher, and manager pages.

---

*Architecture analysis: 2026-04-10*
