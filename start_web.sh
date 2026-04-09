#!/bin/bash

# LearnSite Web 应用启动脚本 (Mono + XSP4)
# 解决 Mono 下 System.Data.SqlClient 的 Socket 非阻塞异常

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PORT="${1:-9080}"

# 关键：修复 Mono SqlClient 非阻塞 Socket 异常
# MONO_THREADS_PER_CPU 增加线程池大小，减少 Socket 阻塞概率
# MONO_TLS_PROVIDER 使用 btls 避免 TLS 握手阻塞
export MONO_THREADS_PER_CPU=50
export MONO_TLS_PROVIDER=btls
# 禁用 IO 层的异步优化，强制使用阻塞模式
# export MONO_MANAGED_WATCHER=disabled
export MONO_DISABLE_AIO=1

echo "Starting LearnSite on http://localhost:$PORT"
echo "Press Ctrl+C to stop."
echo ""

exec xsp4 --port "$PORT" --root "$SCRIPT_DIR" --nonstop
