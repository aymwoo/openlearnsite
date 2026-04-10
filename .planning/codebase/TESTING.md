# Testing Patterns

**Analysis Date:** 2026-04-10

## Test Framework

This repository uses three active test styles: xUnit for C# logic and regression
tests, Vitest for browser-side JavaScript logic, and Playwright for end-to-end
WebForms flows.

**Runner:**
- xUnit via `dotnet test` for C# projects in `Tests/EnDeCodeTests/`,
  `Tests/ImageCheckTests/`, `Tests/SharpZipTests/`, `Tests/CommonLogicTests/`,
  and `Tests/TeacherRegressionTests/`.
- Vitest `^3.0.0` with config in `vitest.config.ts`.
- Playwright `^1.50.0` with config in `playwright.config.ts`.

**Assertion Library:**
- xUnit `Assert.*` in C# tests, such as
  `Tests/CommonLogicTests/CommonLogicTests.cs`.
- Vitest `expect(...)` in `Tests/unit/exam.test.js`.
- Playwright `expect(...)` plus locator assertions in
  `Tests/e2e/*.spec.js`.

**Run Commands:**
```bash
./scripts/run_tests.sh              # Run scripted C#, Vitest, and Playwright suites
./scripts/run_tests.sh --only-unit  # Run scripted C# + Vitest suites
./scripts/run_tests.sh --no-e2e     # Skip Playwright
npm run test:unit                   # Run Vitest directly from `package.json`
npm run test:e2e                    # Run Playwright directly from `package.json`
dotnet test Tests/CommonLogicTests/CommonLogicTests.csproj
dotnet test Tests/TeacherRegressionTests/TeacherRegressionTests.csproj
```

## Test File Organization

Tests are centralized under the root `Tests/` directory, with separate folders by
test style and target runtime.

**Location:**
- JS unit tests live in `Tests/unit/`.
- Playwright E2E tests live in `Tests/e2e/`.
- C# xUnit projects live in dedicated directories under `Tests/`, such as
  `Tests/EnDeCodeTests/` and `Tests/CommonLogicTests/`.

**Naming:**
- Use `*.test.js` for Vitest unit specs, such as `Tests/unit/exam.test.js`.
- Use `*.spec.js` for Playwright specs, such as
  `Tests/e2e/student-login.spec.js`.
- Use `*Tests.cs` for xUnit classes, such as
  `Tests/ImageCheckTests/ImageCheckTests.cs` and
  `Tests/TeacherRegressionTests/TeacherRegressionTests.cs`.

**Structure:**
```text
Tests/
├── unit/
│   ├── exam.test.js
│   ├── exam-functions.js
│   └── setup.js
├── e2e/
│   ├── auth.helper.js
│   ├── navigation.spec.js
│   ├── student-login.spec.js
│   └── teacher-login.spec.js
├── EnDeCodeTests/
├── ImageCheckTests/
├── SharpZipTests/
├── CommonLogicTests/
└── TeacherRegressionTests/
```

## Test Structure

Each test type uses a clear, repeated structure. Match the style of the suite you
are extending.

**Suite Organization:**
```typescript
// `Tests/unit/exam.test.js`
describe('checkAnswer - 单选题', () => {
  const question = { type: 'single_choice', answer: 2, score: 5 };

  it('正确答案返回 true', () => {
    expect(checkAnswer(question, 2)).toBe(true);
  });
});

// `Tests/e2e/navigation.spec.js`
test.describe('学生注册页面 (/student/register.aspx)', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('/student/register.aspx');
    await page.waitForLoadState('domcontentloaded');
  });
});

// `Tests/ImageCheckTests/ImageCheckTests.cs`
[Fact]
public void CheckImageType_ValidPng_ReturnsTrue()
{
    string filePath = Path.Combine(_tempDir, "valid.png");
    File.WriteAllBytes(filePath, new byte[] { 0x89, 0x50, 0x4E, 0x47 });

    bool result = ImageCheck.CheckImageType(filePath);

    Assert.True(result);
}
```

**Patterns:**
- Use nested `describe` blocks and explicit scenario names in Vitest, as shown in
  `Tests/unit/exam.test.js`.
- Use `test.describe(...)` and `test.beforeEach(...)` in Playwright to group by
  page or route, as shown in `Tests/e2e/navigation.spec.js` and
  `Tests/e2e/teacher-login.spec.js`.
- Use one `[Fact]` or `[Theory]` per behavior in xUnit and keep method names in
  `Subject_Condition_ExpectedResult` form, as shown throughout
  `Tests/CommonLogicTests/CommonLogicTests.cs`.
- Use `// Arrange`, `// Act`, and `// Assert` comments in C# tests when the flow
  is stateful, as shown in `Tests/EnDeCodeTests/EnDeCodeTests.cs` and
  `Tests/SharpZipTests/SharpZipTests.cs`.

## Mocking

Mocking is minimal. Most tests favor pure functions, temporary filesystem state,
or real browser interactions over framework-heavy mocks.

**Framework:**
- No dedicated mocking library is detected in `package.json` or the C# test
  projects.
- Vitest tests use direct invocation instead of `vi.mock(...)`.
- Playwright tests use the real app UI and real navigation rather than request
  interception.

**Patterns:**
```typescript
// `Tests/unit/setup.js`
if (typeof localStorage === 'undefined') {
  const store = {};
  global.localStorage = {
    getItem: (key) => store[key] || null,
    setItem: (key, value) => { store[key] = String(value); },
    removeItem: (key) => { delete store[key]; },
    clear: () => { Object.keys(store).forEach(k => delete store[k]); },
  };
}

// `Tests/e2e/auth.helper.js`
export async function teacherLogin(page, hname, hpwd) {
  await page.goto('/teacher/index.aspx');
  await page.fill('[id$="Textname"]', hname);
  await page.fill('[id$="Textpwd"]', hpwd);
  await page.click('[id$="Btnlogin"]');
}
```

**What to Mock:**
- Mock only browser globals needed to execute pure front-end logic under jsdom,
  such as `localStorage` in `Tests/unit/setup.js`.
- Create temporary files and directories instead of mocking file IO in C# tests,
  as shown by `_tempDir` usage in `Tests/ImageCheckTests/ImageCheckTests.cs`,
  `Tests/SharpZipTests/SharpZipTests.cs`, and
  `Tests/CommonLogicTests/CommonLogicTests.cs`.

**What NOT to Mock:**
- Do not fake login cookies in E2E tests. `Tests/e2e/auth.helper.js` documents
  that authentication cookies are dynamic and encrypted, so tests log in through
  the real UI.
- Do not replace pure exam logic with spies. `Tests/unit/exam.test.js` imports
  extracted logic from `Tests/unit/exam-functions.js` and asserts real return
  values.
- Do not replace page source files in regression tests. `Tests/TeacherRegressionTests/`
  `TeacherRegressionTests.cs` reads the actual repository files and asserts on
  their contents.

## Fixtures and Factories

Fixtures are lightweight and local to each suite. The common pattern is to build
only the minimum data needed for the assertion.

**Test Data:**
```typescript
// `Tests/unit/exam.test.js`
const question = {
  type: 'multiple_choice',
  answer: [0, 2, 3],
  score: 10,
};

// `Tests/e2e/auth.helper.js`
export const TEST_ACCOUNTS = {
  admin: { username: 'admin', password: '12345' },
};
```

**Location:**
- Inline object fixtures are preferred in `Tests/unit/exam.test.js`.
- Inline `DataTable` fixtures are preferred in `Tests/CommonLogicTests/`
  `CommonLogicTests.cs` to exercise mapper code.
- Test account fixtures live in `Tests/e2e/auth.helper.js`.
- Temporary filesystem fixtures are created inside the test class constructor in
  `Tests/ImageCheckTests/ImageCheckTests.cs` and `Tests/SharpZipTests/`
  `SharpZipTests.cs`.

## Coverage

Coverage is collected for selected suites, but no repository-wide threshold is
enforced.

**Requirements:** None enforced.

**View Coverage:**
```bash
./scripts/run_tests.sh --only-unit
dotnet test Tests/EnDeCodeTests/EnDeCodeTests.csproj --collect:"XPlat Code Coverage"
npx vitest run --coverage
```

- `scripts/run_tests.sh` enables `XPlat Code Coverage` for the scripted C# test
  projects and stores results under `scripts/TestResults/`.
- `vitest.config.ts` uses V8 coverage with `reporter: ['text']` and includes
  `webform/**/*.js`, `code/**/*.js`, and `js/**/*.js`.
- Default scripted C# coverage currently covers only
  `Tests/EnDeCodeTests/EnDeCodeTests.csproj`,
  `Tests/ImageCheckTests/ImageCheckTests.csproj`, and
  `Tests/SharpZipTests/SharpZipTests.csproj`. Other xUnit projects exist but are
  not included in the default shell script.

## Test Types

The repository uses unit, regression, and browser-level integration tests.

**Unit Tests:**
- Use Vitest for front-end pure logic in `Tests/unit/exam.test.js`.
- Use xUnit for isolated C# utility and storage code in
  `Tests/EnDeCodeTests/EnDeCodeTests.cs`,
  `Tests/ImageCheckTests/ImageCheckTests.cs`, and
  `Tests/SharpZipTests/SharpZipTests.cs`.

**Integration Tests:**
- Use xUnit with linked production files and synthetic `DataTable` inputs to test
  mapping and helper integration in `Tests/CommonLogicTests/CommonLogicTests.cs`.
- Use repository-content regression tests in
  `Tests/TeacherRegressionTests/TeacherRegressionTests.cs` to lock down markup,
  copy, route values, and editor wiring across many `.aspx` and `.cs` files.

**E2E Tests:**
- Use Playwright in `Tests/e2e/`.
- Run in a single Chromium worker according to `playwright.config.ts`.
- Depend on a running site. `scripts/run_tests.sh` checks `BASE_URL` before
  running E2E. `playwright.config.ts` defaults to `http://localhost:8080`, while
  `scripts/run_tests.sh` defaults `BASE_URL` to `http://localhost:9080`, so pass
  `BASE_URL` explicitly when needed.

## Common Patterns

Several patterns repeat across suites. Reuse them instead of inventing a new
style.

**Async Testing:**
```typescript
// `Tests/e2e/teacher-login.spec.js`
test('管理员登录 - 跳转到管理后台', async ({ page }) => {
  await page.fill('[id$="Textname"]', username);
  await page.fill('[id$="Textpwd"]', password);
  await page.click('[id$="Btnlogin"]');
  await page.waitForURL(/manager\/index\.aspx/, { timeout: 15000 });
});
```

- Use `await page.goto(...)`, `await page.waitForLoadState(...)`, and
  `await page.waitForURL(...)` for WebForms flows in `Tests/e2e/*.spec.js`.
- Use async helpers for repeated login or navigation sequences in
  `Tests/e2e/auth.helper.js`.

**Error Testing:**
```csharp
// `Tests/EnDeCodeTests/EnDeCodeTests.cs`
[Fact]
public void Decrypt_InvalidEncryptedString_ThrowsException()
{
    Assert.ThrowsAny<Exception>(() => EnDeCode.Decrypt("NotHexadecimalString", "12"));
}
```

```typescript
// `Tests/unit/exam.test.js`
it('无效 JSON 返回 null', () => {
  expect(parseExamAiSseData('not json')).toBeNull();
  expect(parseExamAiSseData('{broken')).toBeNull();
});
```

- Use `Assert.ThrowsAny<Exception>(...)` in C# when the production API throws
  broad exceptions, as in `Tests/EnDeCodeTests/EnDeCodeTests.cs`.
- Use explicit fallback assertions in JS when the production API returns safe
  values instead of throwing, as in `Tests/unit/exam.test.js`.
- Use permissive assertions when server behavior varies by hosting environment,
  such as the `[200, 302, 404, 500]` status check in
  `Tests/e2e/navigation.spec.js` for error-page handling.

---

*Testing analysis: 2026-04-10*
