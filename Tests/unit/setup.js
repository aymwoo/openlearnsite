/**
 * Vitest 全局 setup
 * 
 * 为非 ES 模块的浏览器脚本模拟必要的 DOM 环境
 * jsdom 环境由 vitest.config.ts 中 environment: 'jsdom' 提供
 */

// 模拟 localStorage
if (typeof localStorage === 'undefined') {
  const store = {};
  global.localStorage = {
    getItem: (key) => store[key] || null,
    setItem: (key, value) => { store[key] = String(value); },
    removeItem: (key) => { delete store[key]; },
    clear: () => { Object.keys(store).forEach(k => delete store[k]); },
  };
}
