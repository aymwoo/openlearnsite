import { defineConfig } from 'vitest/config';

/**
 * LearnSite Vitest 单元测试配置
 * 
 * 测试目标: webform/exam.js, webform/preview.js 等前端JS模块
 * 运行方式: npx vitest run
 */
export default defineConfig({
  test: {
    include: ['Tests/unit/**/*.test.{js,ts}'],
    environment: 'jsdom',
    globals: true,
    // 覆盖率配置
    coverage: {
      provider: 'v8',
      include: ['webform/**/*.js', 'code/**/*.js', 'js/**/*.js'],
      exclude: ['**/node_modules/**', '**/Tests/**'],
      reporter: ['text'], // 终端文本输出
    },
    // 模拟 DOM 环境的初始化
    setupFiles: ['Tests/unit/setup.js'],
  },
});
