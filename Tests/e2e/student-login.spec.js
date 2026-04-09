/**
 * E2E 测试: 学生登录流程
 * 
 * 测试页面: /index.aspx (无 master page, ID 直接渲染)
 * 前提条件: 数据库中需要有测试学生账户，或使用班级密码模式
 */
import { test, expect } from '@playwright/test';

test.describe('学生登录页面 (/index.aspx)', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('/index.aspx');
    await page.waitForLoadState('domcontentloaded');
  });

  test('页面加载 - 显示登录表单', async ({ page }) => {
    // 页面标题包含"学习网站"
    await expect(page).toHaveTitle(/学习网站|学习/);

    // 登录表单元素存在
    await expect(page.locator('#TextBoxuser')).toBeVisible();
    await expect(page.locator('#TextBoxpwd')).toBeVisible();
    await expect(page.locator('#Btnlogin')).toBeVisible();

    // 显示登录窗口标题
    await expect(page.locator('.indexhead')).toContainText('登录窗口');
  });

  test('页面加载 - 显示功能链接', async ({ page }) => {
    // 学员注册链接
    const regLink = page.locator('#HyperLinkReg');
    await expect(regLink).toBeVisible();
    await expect(regLink).toContainText('学员注册');

    // 学号查询链接
    const numLink = page.locator('#HyperLinkSnum');
    await expect(numLink).toBeVisible();
    await expect(numLink).toContainText('学号查询');

    // 教师平台链接
    const teacherLink = page.locator('#HLTeacher');
    await expect(teacherLink).toBeVisible();
    await expect(teacherLink).toContainText('教师平台');
  });

  test('页面加载 - 显示IP和学期信息', async ({ page }) => {
    // 当前IP标签不为空
    const ipLabel = page.locator('#Labelip');
    await expect(ipLabel).toBeVisible();
    const ipText = await ipLabel.textContent();
    expect(ipText?.length).toBeGreaterThan(0);

    // 学期标签不为空
    const termLabel = page.locator('#Labelterm');
    await expect(termLabel).toBeVisible();
  });

  test('空输入提交 - 不触发跳转', async ({ page }) => {
    // 不输入任何内容直接点击登录
    await page.click('#Btnlogin');
    await page.waitForLoadState('networkidle');

    // 应该仍然在登录页面
    expect(page.url()).toContain('index.aspx');
  });

  test('错误凭据 - 显示错误消息', async ({ page }) => {
    await page.fill('#TextBoxuser', '99999999');
    await page.fill('#TextBoxpwd', 'wrongpassword');
    await page.click('#Btnlogin');

    await page.waitForLoadState('networkidle');

    // 应该显示错误信息
    const msgLabel = page.locator('#Labelmsg');
    const msgText = await msgLabel.textContent();
    expect(msgText?.length).toBeGreaterThan(0);

    // 应该仍然在登录页面
    expect(page.url()).toContain('index.aspx');
  });

  test('非法字符输入 - 显示验证错误', async ({ page }) => {
    // 学号必须为数字，输入字母
    await page.fill('#TextBoxuser', 'abc');
    await page.fill('#TextBoxpwd', '12345');
    await page.click('#Btnlogin');

    await page.waitForLoadState('networkidle');

    const msgLabel = page.locator('#Labelmsg');
    const msgText = await msgLabel.textContent();
    // 应提示学号必须为数字
    expect(msgText).toContain('非法字符');
  });

  test('教师平台链接 - 跳转到教师登录页', async ({ page }) => {
    const teacherLink = page.locator('#HLTeacher');
    const href = await teacherLink.getAttribute('href');
    expect(href).toContain('teacher/index.aspx');
  });

  test('学员注册链接 - 跳转到注册页', async ({ page }) => {
    const regLink = page.locator('#HyperLinkReg');
    const href = await regLink.getAttribute('href');
    expect(href).toContain('student/register.aspx');
  });
});
