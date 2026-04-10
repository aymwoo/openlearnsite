# Coding Conventions

**Analysis Date:** 2026-04-10

## Naming Patterns

This repository mixes ASP.NET WebForms, C#, and browser JavaScript. Follow the
existing convention of matching the surrounding subsystem instead of trying to
normalize the whole repository.

**Files:**
- Use lowercase WebForms page names for teacher, student, manager, and webform
  pages, with paired code-behind files such as `teacher/start.aspx` and
  `teacher/start.aspx.cs`.
- Use PascalCase class filenames under `App_Code/` when the file maps to a C#
  type, such as `App_Code/Common/CookieHelp.cs` and
  `App_Code/Bll/BllDataTableMappers.cs`.
- Use descriptive lowercase or kebab-style JavaScript filenames for browser
  helpers and test helpers, such as `teacher/editor-upload-helper.js`,
  `webform/exam.js`, and `Tests/e2e/auth.helper.js`.
- Use `*.test.js` for Vitest unit tests in `Tests/unit/` and `*.spec.js` for
  Playwright tests in `Tests/e2e/`.

**Functions:**
- Use PascalCase for C# methods and page helpers, such as `Page_Load`,
  `SetStuCookie`, `ClearTeacherCookies`, `MapQuizGradeList`, and `showLock` in
  `teacher/start.aspx.cs`.
- Keep ASP.NET event handlers in the `Control_Event` form, such as
  `Btnadd_Click` in `teacher/consoleadd.aspx`.
- Use camelCase for JavaScript functions, such as `updateScore`,
  `syncExamAiSetting`, `collectSubmissionData`, and `buildExamSseUrl` in
  `webform/exam.js`, `webform/preview.js`, and `Tests/unit/exam-functions.js`.

**Variables:**
- Use underscore-prefixed private fields in C# test classes, such as `_tempDir`,
  `_xmlFile`, `_zipFile`, and `_extractDir` in `Tests/CommonLogicTests/`
  `CommonLogicTests.cs`, `Tests/ImageCheckTests/ImageCheckTests.cs`, and
  `Tests/SharpZipTests/SharpZipTests.cs`.
- Use local variable names that mirror domain fields in C# code-behind, even
  when they are short or legacy, such as `Hid`, `Rgrade`, `Rclass`, and `Rhid`
  in `teacher/start.aspx.cs` and `App_Code/Common/CookieHelp.cs`.
- Use `const` and `let` in modern JavaScript test and config files, such as
  `vitest.config.ts`, `playwright.config.ts`, and `Tests/unit/exam.test.js`.
- Keep object keys aligned with persisted or transmitted field names in browser
  scripts, such as `enableAiAssessment`, `keywordThreshold`, and `questionType`
  in `webform/exam.js` and `webform/preview.js`.

**Types:**
- Use PascalCase for C# classes and models, such as `CookieHelp`,
  `BllDataTableMappers`, `TeacherRegressionTests`, and `CommonLogicTests`.
- Use namespace names that match the feature or test project, such as
  `LearnSite.Common`, `LearnSite.BLL`, `TeacherRegressionTests`, and
  `CommonLogicTests`.
- Use plain object literals instead of formal type declarations in browser
  JavaScript, such as `examData`, `matchingState`, and `studentInfo` in
  `webform/exam.js`.

## Code Style

Formatting is driven by existing file style, not by a repository-wide formatter.
Match the indentation, spacing, and brace style of the file you edit.

**Formatting:**
- Root formatter config is not detected. No root `.prettierrc`, `prettier`
  config, `eslint.config.*`, `.eslintrc*`, or `biome.json` is present.
- Use four-space indentation and Allman-style braces in C# files, as shown in
  `App_Code/Common/CookieHelp.cs`, `App_Code/Bll/BllDataTableMappers.cs`, and
  `teacher/start.aspx.cs`.
- Use semicolon-terminated statements in C# and JavaScript.
- Use two-space indentation in modern TypeScript and JS config and test files,
  such as `vitest.config.ts`, `playwright.config.ts`, and
  `Tests/unit/exam.test.js`.
- Use inline Chinese comments and section dividers when the surrounding file
  already uses them, such as `// 初始化函数` in `webform/exam.js` and
  `// Arrange` / `// Act` / `// Assert` in `Tests/EnDeCodeTests/`
  `EnDeCodeTests.cs`.

**Linting:**
- Repository-wide linting is not detected.
- One nested third-party area has its own ESLint config at
  `ai/stylization/.eslintrc.json`; do not treat that file as the project-wide
  standard.
- In practice, correctness is enforced more by tests than by lint rules. Follow
  the existing local style in the file you touch.

## Import Organization

Import order differs by language, but each area is internally consistent.

**Order:**
1. Framework or platform imports first, such as `System.*` and `Xunit` in
   `Tests/EnDeCodeTests/EnDeCodeTests.cs` or `defineConfig` in
   `vitest.config.ts`.
2. Third-party package imports next, such as `@playwright/test` in
   `Tests/e2e/navigation.spec.js`.
3. Local project imports last, such as `LearnSite.Common` in C# tests and
   `./auth.helper.js` or `./exam-functions.js` in JS tests.

**Path Aliases:**
- No JS or TS path aliases are detected in the root test setup.
- Use relative imports in test files, such as `./auth.helper.js` in
  `Tests/e2e/teacher-login.spec.js` and `./exam-functions.js` in
  `Tests/unit/exam.test.js`.
- Use namespace imports in C# instead of filesystem aliases, such as
  `using LearnSite.Common;` in `Tests/CommonLogicTests/CommonLogicTests.cs`.

## Error Handling

Error handling follows the technology boundary. Server-side code tends to catch
`Exception` broadly and continue safely. Browser helpers often return fallback
values instead of throwing.

**Patterns:**
- Use guard clauses for null, empty, and missing data before deeper processing,
  as shown by `if (stmodel != null)` in `App_Code/Common/CookieHelp.cs`,
  `if (rowsCount > 0)` in `App_Code/Bll/BllDataTableMappers.cs`, and
  `if (userAnswer === null || userAnswer === undefined) return false;` in
  `Tests/unit/exam-functions.js`.
- Use broad `try/catch (Exception ex)` when interacting with ASP.NET request,
  response, or cookie state, then degrade safely, as shown in
  `App_Code/Common/CookieHelp.cs`.
- Return `false`, `0`, or `null` for invalid input in utility code instead of
  surfacing rich error types, as shown in `App_Code/Common/ImageCheck.cs`
  through tests in `Tests/ImageCheckTests/ImageCheckTests.cs`, and in
  `parseExamAiSseData` at `webform/preview.js`.
- Throw only when callers already expect exception-driven validation, as shown by
  `App_Code/Common/Des.cs` and the matching assertions in
  `Tests/EnDeCodeTests/EnDeCodeTests.cs`.

## Logging

Logging is lightweight and local. Follow the existing mechanism already used in
the layer you change.

**Framework:** `System.Diagnostics.Trace` on the server and `console` in browser
scripts.

**Patterns:**
- Use `System.Diagnostics.Trace.WriteLine(...)` for server-side cleanup or cookie
  failure diagnostics, as shown in `App_Code/Common/CookieHelp.cs` and
  `App_Code/Model/MngCook.cs`.
- Use `console.log(...)` and `console.warn(...)` for browser-side runtime
  inspection, especially in exam and preview flows, as shown in
  `webform/exam.js` and `webform/preview.js`.
- Keep user-facing failure feedback separate from logs. Browser upload helpers in
  `teacher/editor-upload-helper.js` log minimally and use `alert(...)` for the
  visible failure path.

## Comments

Comments are used to explain intent, business rules, and legacy WebForms
constraints. Keep them short and close to the relevant code.

**When to Comment:**
- Add comments for WebForms-specific behavior that is not obvious from the code,
  such as master page ID mangling in `Tests/e2e/auth.helper.js` and delayed
  control disabling in `teacher/start.aspx.cs`.
- Add comments for data shape or scoring rules in exam flows, such as the
  question schema comments in `webform/exam.js` and keyword scoring comments in
  `Tests/unit/exam.test.js`.
- Use test comments to separate sections and document scenario intent, such as
  the large section banners in `Tests/unit/exam.test.js` and the
  `// Arrange` / `// Act` / `// Assert` comments in
  `Tests/SharpZipTests/SharpZipTests.cs`.

**JSDoc/TSDoc:**
- Use JSDoc-style block comments in JavaScript tests and helpers when the file is
  written as a reusable helper module, such as `Tests/e2e/auth.helper.js`,
  `Tests/unit/exam-functions.js`, `vitest.config.ts`, and
  `playwright.config.ts`.
- XML doc comments exist in older C# utility code, such as
  `App_Code/Common/CookieHelp.cs`, but they are not applied consistently across
  the repository.

## Function Design

Function shape follows the runtime model. WebForms code-behind favors long page
methods and helper clusters. Browser logic uses global function declarations and
mutable state objects.

**Size:**
- Accept long page/controller methods in WebForms files when they orchestrate UI
  state, such as `Page_Load` and `showLock` in `teacher/start.aspx.cs`.
- Extract pure logic only when it needs isolated tests, as shown by
  `Tests/unit/exam-functions.js`, which mirrors logic from `webform/exam.js` and
  `webform/preview.js`.

**Parameters:**
- Use strongly ordered primitive parameters in legacy C# helpers, such as
  `SetStuCookie(LearnSite.Model.Students stmodel, string LoginIp)` in
  `App_Code/Common/CookieHelp.cs`.
- Use object parameters in JavaScript when many related values travel together,
  as shown by `buildExamSseUrl(baseUrl, params)` in
  `Tests/unit/exam-functions.js`.

**Return Values:**
- Return booleans for validation and success checks, such as `SetPPTistSnum`,
  `SetStuCookie`, and `checkAnswer` in `App_Code/Common/CookieHelp.cs` and
  `Tests/unit/exam-functions.js`.
- Return numeric scores directly for scoring helpers, such as
  `calculateEarnedScore` in `Tests/unit/exam-functions.js`.
- Return structured objects only when multiple fields are needed downstream, such
  as `scoreShortAnswer(...)` in `Tests/unit/exam-functions.js` and
  `collectSubmissionData()` in `webform/preview.js`.

## Module Design

Modules are organized by platform. C# favors one class per file. Browser code is
often page-scoped and global unless a helper is explicitly extracted.

**Exports:**
- Use implicit class exposure in C# via one primary type per file, such as
  `App_Code/Common/CookieHelp.cs` and `App_Code/Bll/BllDataTableMappers.cs`.
- Use IIFE-based globals for browser helpers that must be callable from ASPX
  pages, such as `window.LearnSiteEditorUploadHelper` in
  `teacher/editor-upload-helper.js`.
- Use explicit ES module exports only in test-only extraction files, such as
  `Tests/unit/exam-functions.js`.

**Barrel Files:**
- Barrel files are not used.
- Reference concrete files directly, such as `./auth.helper.js` from
  `Tests/e2e/teacher-login.spec.js` and linked source files from
  `Tests/CommonLogicTests/CommonLogicTests.csproj`.

---

*Convention analysis: 2026-04-10*
