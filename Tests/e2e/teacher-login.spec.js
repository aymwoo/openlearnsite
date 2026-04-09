/**
 * E2E 测试: 教师/管理员登录流程
 * 
 * 测试页面: /teacher/index.aspx (使用 Teach.master, ID 带 ctl00_Content_ 前缀)
 * 默认管理员: admin / 12345
 */
import { test, expect } from '@playwright/test';
import { teacherLogin, TEST_ACCOUNTS } from './auth.helper.js';

test.describe('教师登录页面 (/teacher/index.aspx)', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('/teacher/index.aspx');
    await page.waitForLoadState('domcontentloaded');
  });

  test('页面加载 - 显示登录表单', async ({ page }) => {
    await expect(page).toHaveTitle(/教师登录/);

    // master page ID munging: 使用 ends-with 选择器
    await expect(page.locator('[id$="Textname"]')).toBeVisible();
    await expect(page.locator('[id$="Textpwd"]')).toBeVisible();
    await expect(page.locator('[id$="Btnlogin"]')).toBeVisible();

    // 显示"教师登录"标题
    await expect(page.locator('.phead')).toContainText('教师登录');
  });

  test('空输入提交 - 显示错误信息', async ({ page }) => {
    await page.click('[id$="Btnlogin"]');
    await page.waitForLoadState('networkidle');

    const msgLabel = page.locator('[id$="Labelmsg"]');
    const msgText = await msgLabel.textContent();
    expect(msgText).toContain('不能为空');
  });

  test('错误凭据 - 显示错误消息', async ({ page }) => {
    await page.fill('[id$="Textname"]', 'wronguser');
    await page.fill('[id$="Textpwd"]', 'wrongpwd');
    await page.click('[id$="Btnlogin"]');

    await page.waitForLoadState('networkidle');

    const msgLabel = page.locator('[id$="Labelmsg"]');
    const msgText = await msgLabel.textContent();
    expect(msgText).toContain('用户名或密码错误');

    // 仍在登录页
    expect(page.url()).toContain('teacher/index.aspx');
  });

  test('管理员登录 - 跳转到管理后台', async ({ page }) => {
    const { username, password } = TEST_ACCOUNTS.admin;

    await page.fill('[id$="Textname"]', username);
    await page.fill('[id$="Textpwd"]', password);
    await page.click('[id$="Btnlogin"]');

    // Thread.Sleep(200) + redirect to manager/index.aspx
    await page.waitForURL(/manager\/index\.aspx/, { timeout: 15000 });

    // 验证已进入管理后台
    expect(page.url()).toContain('manager/index.aspx');
  });
});

test.describe('管理后台 (/manager/index.aspx)', () => {
  test.beforeEach(async ({ page }) => {
    // 先登录
    await teacherLogin(page, TEST_ACCOUNTS.admin.username, TEST_ACCOUNTS.admin.password);
  });

  test('登录后 - 显示操作流程图', async ({ page }) => {
    // 管理后台的"操作流程图"工作流文本框
    const flowLabel = page.locator('[id$="TextBox1"]');
    await expect(flowLabel).toBeVisible();
    const flowText = await flowLabel.inputValue();
    expect(flowText).toContain('操作流程图');
  });

  test('登录后 - 显示系统退出按钮', async ({ page }) => {
    const logoutBtn = page.locator('[id$="Btnlogout"]');
    await expect(logoutBtn).toBeVisible();
    await expect(logoutBtn).toContainText('系统退出');
  });

  test('登录后 - 左侧导航栏包含管理链接', async ({ page }) => {
    // Manage.master 的左侧导航
    const nav = page.locator('#navigul');
    await expect(nav).toBeVisible();

    // 验证关键管理链接存在
    await expect(page.locator('a[href*="setting.aspx"]')).toBeVisible();
    await expect(page.locator('a[href*="createroom.aspx"]')).toBeVisible();
    await expect(page.locator('a[href*="teacher.aspx"]')).toBeVisible();
    await expect(page.locator('a[href*="studentimport.aspx"]')).toBeVisible();
  });

  test('系统退出 - 返回教师登录页', async ({ page }) => {
    const logoutBtn = page.locator('[id$="Btnlogout"]');
    await logoutBtn.click();

    await page.waitForURL(/teacher\/index\.aspx/, { timeout: 15000 });
    expect(page.url()).toContain('teacher/index.aspx');
  });
});
