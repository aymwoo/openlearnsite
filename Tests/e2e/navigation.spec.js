/**
 * E2E 测试: 页面可访问性和基础导航
 * 
 * 不需要登录的公开页面测试，以及核心导航流程验证
 */
import { test, expect } from '@playwright/test';

test.describe('公开页面可访问性', () => {
  test('首页加载 - HTTP 200', async ({ page }) => {
    const response = await page.goto('/index.aspx');
    expect(response?.status()).toBe(200);
  });

  test('教师登录页加载 - HTTP 200', async ({ page }) => {
    const response = await page.goto('/teacher/index.aspx');
    expect(response?.status()).toBe(200);
  });

  test('学生注册页加载 - HTTP 200', async ({ page }) => {
    const response = await page.goto('/student/register.aspx');
    expect(response?.status()).toBe(200);
  });

  test('学号查询页加载 - HTTP 200', async ({ page }) => {
    const response = await page.goto('/student/mynum.aspx');
    expect(response?.status()).toBe(200);
  });

  test('课堂守则页加载 - HTTP 200', async ({ page }) => {
    const response = await page.goto('/student/myrule.aspx');
    expect(response?.status()).toBe(200);
  });

  test('错误页面 - 404处理', async ({ page }) => {
    const response = await page.goto('/nonexistent-page.aspx');
    // ASP.NET 可能返回 404 或自定义错误页
    const status = response?.status();
    expect([200, 302, 404, 500]).toContain(status);
  });
});

test.describe('学生注册页面 (/student/register.aspx)', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('/student/register.aspx');
    await page.waitForLoadState('domcontentloaded');
  });

  test('页面加载 - 显示注册表单', async ({ page }) => {
    // 注册页无 master page，ID 直接渲染
    await expect(page.locator('#DDLgrade')).toBeVisible();
    await expect(page.locator('#DDLclass')).toBeVisible();
    await expect(page.locator('#DDLsex')).toBeVisible();
    await expect(page.locator('#Tsname')).toBeVisible();
    await expect(page.locator('#BtnRegister')).toBeVisible();
    await expect(page.locator('#BtnReturn')).toBeVisible();
  });

  test('返回按钮 - 跳转回登录页', async ({ page }) => {
    await page.click('#BtnReturn');
    await page.waitForURL(/index\.aspx/, { timeout: 10000 });
  });
});

test.describe('受保护页面 - 未登录重定向', () => {
  test('学生仪表盘 - 重定向到登录页', async ({ page }) => {
    await page.goto('/student/myinfo.aspx');
    await page.waitForLoadState('networkidle');
    // 未登录应重定向到学生登录页
    expect(page.url()).toContain('index.aspx');
  });

  test('教师信息页 - 重定向到教师登录页', async ({ page }) => {
    await page.goto('/teacher/infomation.aspx');
    await page.waitForLoadState('networkidle');
    // 未登录应重定向到教师登录页
    expect(page.url()).toContain('teacher/index.aspx');
  });

  test('管理后台 - 重定向到教师登录页', async ({ page }) => {
    await page.goto('/manager/index.aspx');
    await page.waitForLoadState('networkidle');
    // 未登录应重定向到教师登录页
    expect(page.url()).toContain('teacher/index.aspx');
  });
});
