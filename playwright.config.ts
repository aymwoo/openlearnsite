import { defineConfig, devices } from '@playwright/test';

/**
 * LearnSite Playwright E2E 测试配置
 * 
 * 测试目标: http://localhost:8080 (Mono XSP4)
 * 运行方式: npx playwright test
 */
export default defineConfig({
  testDir: './Tests/e2e',
  fullyParallel: false, // 学校系统有状态依赖，串行更稳定
  forbidOnly: !!process.env.CI,
  retries: process.env.CI ? 2 : 0,
  workers: 1, // 单worker避免登录冲突
  reporter: [
    ['list'], // 终端列表输出
    ['json', { outputFile: 'TestResults/e2e-results.json' }],
  ],
  
  use: {
    baseURL: process.env.BASE_URL || 'http://localhost:8080',
    trace: 'on-first-retry',
    screenshot: 'only-on-failure',
    // 超时设置 - ASP.NET WebForms 响应较慢
    actionTimeout: 15000,
    navigationTimeout: 30000,
  },

  timeout: 60000, // 单个测试60秒超时

  projects: [
    {
      name: 'chromium',
      use: { ...devices['Desktop Chrome'] },
    },
  ],

  // 不自动启动 web server — 用户通过 start_web.sh 手动启动
  // 如果需要自动启动可以取消注释：
  // webServer: {
  //   command: 'bash start_web.sh',
  //   url: 'http://localhost:8080',
  //   reuseExistingServer: true,
  //   timeout: 30000,
  // },
});
