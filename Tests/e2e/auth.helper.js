/**
 * LearnSite E2E 测试 - 认证辅助模块
 * 
 * 由于 Cookie 名称由服务端动态生成 (角色前缀 + CookiesFix + MD5(serverName+version))，
 * 且 Cookie 值使用 DES 加密 (密钥 "20171227")，我们无法在浏览器端直接伪造有效 Cookie。
 * 
 * 因此所有登录操作都通过实际 UI 交互完成。
 */

/**
 * 学生登录
 * @param {import('@playwright/test').Page} page
 * @param {string} snum - 学号
 * @param {string} spwd - 密码
 */
export async function studentLogin(page, snum, spwd) {
  await page.goto('/index.aspx');
  await page.waitForLoadState('domcontentloaded');

  // 学生登录页不使用 master page，ID 直接渲染
  await page.fill('#TextBoxuser', snum);
  await page.fill('#TextBoxpwd', spwd);
  await page.click('#Btnlogin');

  // 服务端有 Thread.Sleep(200)，等待跳转完成
  await page.waitForURL(/\/(student\/showcourse\.aspx|student\/myinfo\.aspx)/, {
    timeout: 15000,
  });
}

/**
 * 教师/管理员登录
 * 教师登录页使用 master page (Teach.master)，ID 带 ctl00_Content_ 前缀
 * @param {import('@playwright/test').Page} page
 * @param {string} hname - 账号
 * @param {string} hpwd - 密码
 */
export async function teacherLogin(page, hname, hpwd) {
  await page.goto('/teacher/index.aspx');
  await page.waitForLoadState('domcontentloaded');

  // master page 导致 ID munging: ctl00_Content_Textname
  await page.fill('[id$="Textname"]', hname);
  await page.fill('[id$="Textpwd"]', hpwd);
  await page.click('[id$="Btnlogin"]');

  // 管理员跳 manager/index.aspx，普通教师跳 teacher/infomation.aspx
  await page.waitForURL(/\/(manager\/index\.aspx|teacher\/infomation\.aspx)/, {
    timeout: 15000,
  });
}

/**
 * 检测当前页面是否已登录（通过检查 URL 判断，非精确方法）
 * @param {import('@playwright/test').Page} page 
 * @returns {boolean}
 */
export function isLoggedIn(page) {
  const url = page.url();
  return (
    url.includes('/student/') ||
    url.includes('/teacher/infomation') ||
    url.includes('/manager/')
  );
}

/**
 * 等待 ASP.NET WebForms PostBack 完成
 * WebForms 的 __doPostBack 会触发完整页面刷新
 * @param {import('@playwright/test').Page} page
 */
export async function waitForPostBack(page) {
  await page.waitForLoadState('networkidle', { timeout: 15000 });
}

/**
 * 默认测试账户
 */
export const TEST_ACCOUNTS = {
  admin: { username: 'admin', password: '12345' },
  // 以下账户需要在测试数据库中预先创建
  // teacher: { username: 'teacher1', password: '12345' },
  // student: { snum: '20240001', password: '12345' },
};
