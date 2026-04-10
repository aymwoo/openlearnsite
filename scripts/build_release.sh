#!/bin/bash

# LearnSite 预编译脚本
# 用于编译 App_Code 中的 C# 代码并准备发布包

set -e

echo "=========================================="
echo "  LearnSite 预编译脚本"
echo "=========================================="
echo ""

# 项目根目录
PROJECT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
cd "$PROJECT_DIR"

# 输出目录
OUTPUT_DIR="${PROJECT_DIR}/publish_output"

echo "[INFO] 清理旧的输出目录..."
rm -rf "$OUTPUT_DIR"
mkdir -p "$OUTPUT_DIR"

echo "[INFO] 编译 App_Code 代码..."
cd "$PROJECT_DIR/Benchmark"

# 使用 TestBuild.csproj 编译 App_Code
if ! dotnet build TestBuild.csproj --configuration Release; then
    echo "[ERROR] 编译失败！"
    exit 1
fi

# 找到编译好的 DLL
DLL_PATH="$PROJECT_DIR/Benchmark/bin/Release/LearnSite.dll"
if [ ! -f "$DLL_PATH" ]; then
    echo "[ERROR] 找不到编译后的 DLL 文件: $DLL_PATH"
    exit 1
fi

echo "[SUCCESS] 编译成功！"
echo "[INFO] DLL 位置: $DLL_PATH"
echo ""

cd "$PROJECT_DIR"

echo "[INFO] 准备发布文件..."

# 复制所有需要的文件到输出目录（排除不需要编译的源文件）
echo "[INFO] 复制项目文件..."

# 使用 rsync 或者直接复制，但排除不需要的文件
# 先复制所有文件，然后再处理
cp -r ./* "$OUTPUT_DIR/" 2>/dev/null || true

# 清理不需要的目录和文件
echo "[INFO] 清理不需要的文件..."
rm -rf "$OUTPUT_DIR/.git"
rm -rf "$OUTPUT_DIR/.github"
rm -rf "$OUTPUT_DIR/Tests"
rm -rf "$OUTPUT_DIR/Benchmark"
rm -rf "$OUTPUT_DIR/CompilerTest"
rm -rf "$OUTPUT_DIR/PerfBench"
rm -rf "$OUTPUT_DIR/node_modules" 2>/dev/null || true
rm -rf "$OUTPUT_DIR/TestResults" 2>/dev/null || true
rm -f "$OUTPUT_DIR/.gitignore"
rm -f "$OUTPUT_DIR/package.json"
rm -f "$OUTPUT_DIR/package-lock.json" 2>/dev/null || true
rm -f "$OUTPUT_DIR/run_tests.sh"
rm -f "$OUTPUT_DIR/start_web.sh"
rm -f "$OUTPUT_DIR/start_dev.sh"
rm -f "$OUTPUT_DIR/build_release.sh"
rm -f "$OUTPUT_DIR/Dockerfile"
rm -f "$OUTPUT_DIR/web.config.docker"

# 将编译好的 DLL 复制到 Bin 目录
echo "[INFO] 复制编译好的 DLL 到 Bin 目录..."
mkdir -p "$OUTPUT_DIR/Bin"
cp "$DLL_PATH" "$OUTPUT_DIR/Bin/"

# 确保所有依赖的 DLL 都在 Bin 目录中
echo "[INFO] 复制所有 Bin 目录中的 DLL..."
if [ -d "$PROJECT_DIR/Bin" ]; then
    cp "$PROJECT_DIR/Bin"/*.dll "$OUTPUT_DIR/Bin/" 2>/dev/null || true
fi

# 移除 App_Code（因为已经编译到 DLL 中）
echo "[INFO] 移除 App_Code 源文件（已编译到 DLL）..."
rm -rf "$OUTPUT_DIR/App_Code"

# 注意：保留 .aspx.cs 和 .master.cs 文件，因为 XSP4 需要这些文件来运行 ASP.NET Web Forms

echo ""
echo "=========================================="
echo "[SUCCESS] 预编译完成！"
echo "[INFO] 输出目录: $OUTPUT_DIR"
echo "=========================================="
