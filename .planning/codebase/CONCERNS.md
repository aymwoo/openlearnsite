# Codebase Concerns

**Analysis Date:** 2026-04-10

## Tech Debt

This codebase carries long-lived structural debt in the Web Forms layer, the DAL,
and several shared utility classes. New changes need to avoid copying these
patterns into more files.

**Raw SQL string concatenation across DAL and page code:**
- Issue: SQL is frequently built with string concatenation instead of parameters.
- Files: `App_Code/Dal/Works.cs`, `App_Code/Dal/StudentHonors.cs`,
  `App_Code/Bll/HonorService.cs`, `teacher/start.aspx.cs`,
  `teacher/honorboardmanage.aspx.cs`
- Impact: Input handling is hard to audit, security fixes are repetitive, and
  query logic is duplicated between page code and DAL classes.
- Fix approach: Move page-level queries into DAL/BLL methods and require
  parameterized `SqlCommand` or `DbHelperSQL` overloads for all user-derived
  values.

**Oversized god classes and pages:**
- Issue: Core behavior is concentrated in very large files.
- Files: `App_Code/Bll/Works.cs` (~3665 lines),
  `App_Code/Common/WordProcess.cs` (~3557 lines),
  `App_Code/Dal/Students.cs` (~3519 lines),
  `App_Code/Utility/UpdateGrade.cs` (~3409 lines),
  `teacher/start.aspx.cs` (~2012 lines)
- Impact: Small changes have high regression risk, navigation cost is high, and
  shared state leaks across unrelated features.
- Fix approach: Split by responsibility first: queries, scoring, login,
  honor-board logic, and upgrade routines.

**Authentication state spread across cookies, session, and application memory:**
- Issue: login state is tracked in cookies, `Session`, and
  `HttpContext.Current.Application` at the same time.
- Files: `App_Code/Common/CookieHelp.cs`, `App_Code/Common/App.cs`,
  `web.config`
- Impact: Behavior depends on process lifetime, session expiry, and cookie
  consistency, which makes auth bugs difficult to reproduce.
- Fix approach: Define a single source of truth for active login state and keep
  cookie contents minimal and server-validated.

**Year-specific business logic embedded in code:**
- Issue: honor-board behavior is pinned to hardcoded years.
- Files: `student/honorboard.aspx.cs`,
  `teacher/honorboardmanage.aspx.cs`, `App_Code/Bll/HonorService.cs`
- Impact: Features silently age out after each academic year and require code
  edits for rollover.
- Fix approach: Read current school year from configuration or student/session
  context instead of literals.

## Known Bugs

These issues are current code defects rather than style problems.

**Student login runs duplicated write/sign-in flow:**
- Symptoms: student login can write cookies and sign-in records twice during one
  request.
- Files: `index.aspx.cs`
- Trigger: successful login through `LoginCode()`.
- Workaround: None in code. Fix the stray block structure around lines
  `268-327` so only one successful-login path executes.

**Honor-board settings are locked to specific years:**
- Symptoms: honor-board queries and saves stop matching current-school-year
  data after year rollover.
- Files: `student/honorboard.aspx.cs`,
  `teacher/honorboardmanage.aspx.cs`, `App_Code/Bll/HonorService.cs`
- Trigger: running the system outside the hardcoded `2025` and `2026` values.
- Workaround: Manual code changes.

**Perfect-attendance honor logic is inverted:**
- Symptoms: students with zero sign-in records can receive the full-attendance
  award.
- Files: `App_Code/Bll/HonorService.cs`
- Trigger: `AwardHonorIfPerfectAttendance()` treats `COUNT(*) == 0` as success.
- Workaround: None in code.

**Honor evaluation uses placeholder randomness for real awards:**
- Symptoms: “进步之星” and “互助达人” are assigned randomly instead of by
  measurable behavior.
- Files: `App_Code/Bll/HonorService.cs`
- Trigger: `AwardHonorIfMostImproved()` and `AwardHonorIfHelperStar()` run.
- Workaround: None in code.

**Command execution is not actually cross-platform:**
- Symptoms: command execution can fail on Linux even though the helper claims
  Linux support.
- Files: `App_Code/Common/CmdUtil.cs`
- Trigger: `ExeCommand()` always starts `cmd.exe` at line `77`.
- Workaround: Avoid `ExeCommand()` on Linux until the process start logic uses
  the detected shell.

## Security Considerations

Several security-sensitive defaults remain unsafe for an internet-facing app.

**SQL injection exposure in user-influenced queries:**
- Risk: untrusted values can reach SQL text directly.
- Files: `App_Code/Dal/Works.cs`, `App_Code/Dal/StudentHonors.cs`,
  `App_Code/Bll/HonorService.cs`, `teacher/start.aspx.cs`,
  `teacher/honorboardmanage.aspx.cs`
- Current mitigation: Partial parameterization exists in newer code such as
  `teacher/check.aspx.cs` and `sub.aspx.cs`.
- Recommendations: Treat all concatenated SQL as migration candidates and block
  new non-parameterized queries.

**Weak cryptography and hardcoded keys:**
- Risk: password and URL-protection mechanisms are easier to brute-force,
  reuse, or reverse than modern server-side secrets.
- Files: `App_Code/Common/Des.cs`, `App_Code/Common/WordProcessCore.cs`,
  `App_Code/Common/CookieHelp.cs`, `App_Code/Common/LinkEncryption.cs`
- Current mitigation: `LinkEncryption` uses AES and session binding, and auth
  cookies are marked `HttpOnly` in most cases.
- Recommendations: Replace MD5- and DES-based flows, move encryption material
  out of source, and use per-environment secrets.

**Unsafe request and cookie configuration:**
- Risk: XSS and mixed-transport cookie exposure are easier to trigger.
- Files: `web.config`, `App_Code/Common/CookieHelp.cs`
- Current mitigation: `httpOnlyCookies="true"` is enabled globally, and most
  auth cookies set `HttpOnly = true`.
- Recommendations: Re-enable request validation where possible, stop running
  with `validateRequest="false"`, set `requireSSL="true"` behind HTTPS, and
  disable `customErrors mode="Off"` outside local debugging.

**Public JavaScript-readable student cookie:**
- Risk: student identifiers are exposed to client-side scripts.
- Files: `App_Code/Common/CookieHelp.cs`
- Current mitigation: `SetPPTistSnum()` only stores the student number.
- Recommendations: Avoid browser-readable cookies unless the client feature
  cannot work without them, and scope them to the smallest path and lifetime.

**Outdated vendored frontend libraries:**
- Risk: older browser libraries carry known CVEs and are difficult to patch
  because they are committed as static assets.
- Files: `wuziqi/js/jquery-1.8.2.min.js`,
  `wuziqi/js/jquery.mobile-1.2.0.min.js`, `ztype/jquery.min.js`
- Current mitigation: Not detected.
- Recommendations: Inventory vendored JS, upgrade or isolate unused bundles,
  and prefer package-managed versions.

## Performance Bottlenecks

The biggest runtime risks come from blocking sleeps and repeated per-row SQL.

**N+1 database reads on the teacher start page:**
- Problem: each student row runs extra queries for work score and check state.
- Files: `teacher/start.aspx.cs`
- Cause: `DLonline_ItemDataBound()` issues direct SQL at lines `660-685` and
  again around `1111-1118`.
- Improvement path: preload row state in one query keyed by `Wnum` and `Wcid`
  before binding the `DataList`.

**Request-thread blocking with `Thread.Sleep`:**
- Problem: web requests are intentionally paused instead of waiting on real
  async completion.
- Files: `teacher/workpackage.aspx.cs`, `index.aspx.cs`,
  `student/register.aspx.cs`, `App_Code/Common/CookieHelp.cs`, many
  `teacher/*.aspx.cs` pages
- Cause: `System.Threading.Thread.Sleep()` appears widely, including a
  `30000ms` pause in `teacher/workpackage.aspx.cs`.
- Improvement path: remove sleeps, poll actual completion state, or redirect
  immediately after durable writes.

**Honor-board page can trigger full-system evaluation on request:**
- Problem: first visit can execute broad honor recalculation from the UI.
- Files: `student/honorboard.aspx.cs`, `App_Code/Bll/HonorService.cs`
- Cause: `CheckAndEvaluateHonors()` calls `EvaluateAllHonors()` when
  `StudentHonors` is empty.
- Improvement path: move award calculation to an admin action, scheduled job,
  or explicit migration step.

## Fragile Areas

These areas are easy to break because behavior is implicit or spread across too
many layers.

**Login and online-user tracking:**
- Files: `index.aspx.cs`, `App_Code/Common/CookieHelp.cs`,
  `App_Code/Common/App.cs`
- Why fragile: login flow mixes cookies, sign-in persistence, application-wide
  online-user dictionaries, and single-login kick lists.
- Safe modification: change one concern at a time and verify both single-login
  and normal-login behavior.
- Test coverage: No focused tests cover `index.aspx.cs` or
  `App_Code/Common/CookieHelp.cs`.

**Upgrade and initialization path:**
- Files: `upgrade.aspx.cs`, `App_Code/Utility/UpdateGrade.cs`
- Why fragile: upgrade order is imperative, long, and highly stateful.
- Safe modification: add new migrations through the migration mechanism before
  touching the legacy `Oldupdate()` chain.
- Test coverage: No automated test coverage was found for upgrade sequencing.

**Honor-board module:**
- Files: `student/honorboard.aspx.cs`,
  `teacher/honorboardmanage.aspx.cs`, `App_Code/Bll/HonorService.cs`,
  `App_Code/Dal/StudentHonors.cs`
- Why fragile: logic mixes presentation, direct SQL, hardcoded years, and
  placeholder evaluation rules.
- Safe modification: correct rule semantics and parameterize queries before
  adding new honor types.
- Test coverage: No dedicated tests were found for honor-board queries or award
  logic.

## Scaling Limits

Current runtime state management is designed for a single process and single
database node.

**Session and online-user tracking are single-node only:**
- Current capacity: one IIS/Mono worker process with in-memory session and
  `Application` state.
- Limit: `sessionState mode="InProc"` in `web.config` and
  `HttpContext.Current.Application` usage in `App_Code/Common/App.cs` do not
  share state across instances.
- Scaling path: move session and online-user state to shared infrastructure
  before adding horizontal scaling.

**Teacher dashboard query cost grows with class size:**
- Current capacity: acceptable for small classes because most reads happen in
  page lifecycle code.
- Limit: `teacher/start.aspx.cs` adds per-row database work, and
  `App_Code/Dal/Works.cs` plus `App_Code/Dal/Students.cs` contain large,
  multi-purpose query surfaces.
- Scaling path: batch teacher dashboard reads and isolate summary queries into
  dedicated read models.

## Dependencies at Risk

This repository vendors multiple browser libraries directly instead of updating
them through a package workflow.

**Legacy jQuery and jQuery Mobile bundles:**
- Risk: old client libraries are difficult to audit and upgrade incrementally.
- Impact: browser-side security issues or compatibility bugs require manual
  asset replacement.
- Migration plan: replace committed bundles in `wuziqi/` and `ztype/` with
  maintained versions or remove the features that still depend on them.

## Missing Critical Features

Several important production features are only partially implemented or absent.

**Deterministic honor evaluation rules:**
- Problem: multiple honor types do not use real domain metrics.
- Blocks: reliable student-facing honor-board results and teacher trust in the
  award system.

**Centralized secure data-access layer:**
- Problem: page code still issues raw SQL directly instead of going through a
  single parameterized access pattern.
- Blocks: systematic SQL-injection hardening and consistent query review.

## Test Coverage Gaps

The existing test suite covers helper logic and file-content regression checks,
but it does not protect the highest-risk production paths.

**Authentication and login flow:**
- What's not tested: student login branching, single-login kick behavior, and
  cookie/session interactions.
- Files: `index.aspx.cs`, `App_Code/Common/CookieHelp.cs`,
  `App_Code/Common/App.cs`
- Risk: login regressions can silently affect sign-in records and active-user
  tracking.
- Priority: High

**Honor-board and award logic:**
- What's not tested: award calculation semantics, year rollover, and ranking
  queries.
- Files: `App_Code/Bll/HonorService.cs`,
  `App_Code/Dal/StudentHonors.cs`, `student/honorboard.aspx.cs`
- Risk: incorrect honors can be published without detection.
- Priority: High

**Teacher dashboard and check workflow:**
- What's not tested: `DataList` query behavior, export flow, edit flow, and
  check-record filtering.
- Files: `teacher/start.aspx.cs`, `teacher/check.aspx.cs`, `sub.aspx.cs`
- Risk: performance and data-integrity regressions reach classrooms directly.
- Priority: Medium

**Upgrade and initialization path:**
- What's not tested: migration ordering, empty-database bootstrap, and legacy
  upgrade sequencing.
- Files: `upgrade.aspx.cs`, `App_Code/Utility/UpdateGrade.cs`
- Risk: deployment-time failures are discovered only in live environments.
- Priority: High

---

*Concerns audit: 2026-04-10*
