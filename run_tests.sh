#!/bin/bash

# LearnSite 全量测试运行脚本
# 运行三类测试:
#   1. C# xUnit 单元测试 (dotnet test)
#   2. JS 单元测试 (Vitest)
#   3. E2E 端到端测试 (Playwright) — 需要 Web 服务运行中
#
# 用法:
#   ./run_tests.sh              # 运行全部测试（E2E 需要服务已启动）
#   ./run_tests.sh --no-e2e     # 跳过 E2E 测试
#   ./run_tests.sh --only-e2e   # 仅运行 E2E 测试
#   ./run_tests.sh --only-unit  # 仅运行单元测试 (C# + JS)

set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
RESULTS_DIR="$SCRIPT_DIR/TestResults"
TIMESTAMP="$(date +%Y%m%d_%H%M%S)"

# 参数解析
RUN_CSHARP=true
RUN_VITEST=true
RUN_E2E=true

for arg in "$@"; do
    case "$arg" in
        --no-e2e)    RUN_E2E=false ;;
        --only-e2e)  RUN_CSHARP=false; RUN_VITEST=false ;;
        --only-unit) RUN_E2E=false ;;
        --help|-h)
            echo "用法: $0 [选项]"
            echo "  --no-e2e     跳过 E2E 测试"
            echo "  --only-e2e   仅运行 E2E 测试"
            echo "  --only-unit  仅运行单元测试 (C# + JS)"
            exit 0
            ;;
    esac
done

# 颜色输出
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
CYAN='\033[0;36m'
BOLD='\033[1m'
DIM='\033[2m'
NC='\033[0m'

info()  { echo -e "${CYAN}[INFO]${NC}  $*"; }
ok()    { echo -e "${GREEN}[PASS]${NC}  $*"; }
warn()  { echo -e "${YELLOW}[WARN]${NC}  $*"; }
fail()  { echo -e "${RED}[FAIL]${NC}  $*"; }

# 分隔线
LINE="━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
THIN="─────────────────────────────────────────────────────────────────────"

# 全局计数
TOTAL_SUITES=0
PASSED_SUITES=0
FAILED_SUITES=0
SKIPPED_SUITES=0
FAILED_NAMES=()

# ============================================================
# 解析 TRX 文件，在终端输出每条用例的结果
# ============================================================
parse_trx() {
    local trx_file="$1"
    [ -f "$trx_file" ] || return

    local total passed failed
    total=$(grep -oP 'total="\K[0-9]+' "$trx_file" | head -1)
    passed=$(grep -oP 'passed="\K[0-9]+' "$trx_file" | head -1)
    failed=$(grep -oP 'failed="\K[0-9]+' "$trx_file" | head -1)

    while IFS= read -r line; do
        local name outcome duration
        name=$(echo "$line" | grep -oP 'testName="\K[^"]+')
        outcome=$(echo "$line" | grep -oP 'outcome="\K[^"]+')
        duration=$(echo "$line" | grep -oP 'duration="\K[^"]+')

        local short_name="${name##*.}"

        local display_time=""
        if [ -n "$duration" ]; then
            local secs
            secs=$(echo "$duration" | awk -F: '{printf "%.4f", $1*3600 + $2*60 + $3}')
            if (( $(echo "$secs < 0.001" | bc -l 2>/dev/null || echo 0) )); then
                display_time="<1ms"
            elif (( $(echo "$secs < 1" | bc -l 2>/dev/null || echo 0) )); then
                display_time="$(echo "$secs" | awk '{printf "%.0fms", $1*1000}')"
            else
                display_time="$(echo "$secs" | awk '{printf "%.2fs", $1}')"
            fi
        fi

        if [ "$outcome" = "Passed" ]; then
            printf "    ${GREEN}✓${NC} %-50s ${DIM}%s${NC}\n" "$short_name" "$display_time"
        elif [ "$outcome" = "Failed" ]; then
            printf "    ${RED}✗${NC} %-50s ${DIM}%s${NC}\n" "$short_name" "$display_time"
            local msg
            msg=$(grep -oP 'Message>\K[^<]+' "$trx_file" 2>/dev/null | head -3)
            [ -n "$msg" ] && echo -e "      ${RED}$msg${NC}"
        else
            printf "    ${YELLOW}○${NC} %-50s ${DIM}%s${NC}\n" "$short_name" "$display_time"
        fi
    done < <(grep -o '<UnitTestResult[^/]*/>' "$trx_file" 2>/dev/null || grep -o '<UnitTestResult[^>]*>' "$trx_file" 2>/dev/null)

    echo ""
    printf "    总计: %s  ${GREEN}通过: %s${NC}" "${total:-0}" "${passed:-0}"
    if [ "${failed:-0}" -gt 0 ]; then
        printf "  ${RED}失败: %s${NC}" "$failed"
    else
        printf "  失败: 0"
    fi
    echo ""
}

# ============================================================
# 解析 Cobertura XML，在终端输出覆盖率表格（仅被测源码）
# ============================================================
parse_coverage() {
    local cov_file="$1"
    local label="$2"
    [ -f "$cov_file" ] || return

    echo -e "  ${BOLD}$label - 覆盖率:${NC}"
    echo "  $THIN"
    echo -e "  ${DIM}(覆盖率报告已生成在文件中)${NC}"
    echo ""
    return
}

# ============================================================
# 运行单个 C# 测试项目
# ============================================================
run_test_project() {
    local project_path="$1"
    local project_name="$2"
    local out_dir="$RESULTS_DIR/$project_name"
    mkdir -p "$out_dir"

    local trx_name="${project_name}.trx"
    local test_ok=false

    if dotnet test "$project_path" \
        --framework net8.0 \
        --logger "trx;LogFileName=$trx_name" \
        --collect:"XPlat Code Coverage" \
        --results-directory "$out_dir" \
        --no-restore \
        --verbosity quiet \
        -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.IncludeTestAssembly=true \
        > /dev/null 2>&1; then
        test_ok=true
    fi

    RESULT_TRX=$(find "$out_dir" -name "$trx_name" -type f 2>/dev/null | head -1)
    RESULT_COV=$(find "$out_dir" -name "coverage.cobertura.xml" -type f 2>/dev/null | head -1)
    [ "$test_ok" = true ] && return 0 || return 1
}

# ============================================================
# 开始
# ============================================================
echo ""
echo -e "$LINE"
echo -e "  ${BOLD}LearnSite 测试套件${NC}    $TIMESTAMP"
echo -e "$LINE"
echo ""

info "清理旧的测试结果..."
rm -rf "$RESULTS_DIR"
mkdir -p "$RESULTS_DIR"

# 存储覆盖率文件
declare -a COV_FILES=()
declare -a COV_LABELS=()

# ════════════════════════════════════════════════════════════
# 第一部分: C# xUnit 单元测试
# ════════════════════════════════════════════════════════════
if [ "$RUN_CSHARP" = true ]; then
    echo -e "${BOLD}  ┌─ 第一部分: C# 单元测试 (xUnit + dotnet test)${NC}"
    echo ""

    if ! command -v dotnet &> /dev/null; then
        warn "未检测到 dotnet SDK，正在安装 dotnet-sdk-8.0 ..."
        if command -v pacman &> /dev/null; then
            sudo pacman -Sy --noconfirm dotnet-sdk-8.0
        elif command -v apt-get &> /dev/null; then
            sudo apt-get update && sudo apt-get install -y dotnet-sdk-8.0
        else
            fail "无法自动安装 .NET SDK，请手动安装后重试"
            exit 1
        fi
    fi

    DOTNET_VERSION="$(dotnet --version 2>/dev/null)"
    info "dotnet SDK 版本: $DOTNET_VERSION"

    TEST_PROJECTS=(
        "Tests/EnDeCodeTests/EnDeCodeTests.csproj"
        "Tests/ImageCheckTests/ImageCheckTests.csproj"
        "Tests/SharpZipTests/SharpZipTests.csproj"
    )

    for PROJECT in "${TEST_PROJECTS[@]}"; do
        PROJECT_PATH="$SCRIPT_DIR/$PROJECT"
        PROJECT_NAME="$(basename "$(dirname "$PROJECT")")"
        TOTAL_SUITES=$((TOTAL_SUITES + 1))

        if [ ! -f "$PROJECT_PATH" ]; then
            warn "项目文件不存在，跳过: $PROJECT"
            SKIPPED_SUITES=$((SKIPPED_SUITES + 1))
            continue
        fi

        echo -e "  ${BOLD}$PROJECT_NAME${NC}"
        echo -e "  ${DIM}$PROJECT${NC}"
        echo ""

        dotnet restore "$PROJECT_PATH" --verbosity quiet > /dev/null 2>&1 || true

        RESULT_TRX=""
        RESULT_COV=""

        if run_test_project "$PROJECT_PATH" "$PROJECT_NAME"; then
            [ -n "$RESULT_TRX" ] && parse_trx "$RESULT_TRX"
            ok "$PROJECT_NAME 全部通过"
            PASSED_SUITES=$((PASSED_SUITES + 1))
        else
            [ -n "$RESULT_TRX" ] && parse_trx "$RESULT_TRX"
            fail "$PROJECT_NAME 测试失败"
            FAILED_SUITES=$((FAILED_SUITES + 1))
            FAILED_NAMES+=("C#: $PROJECT_NAME")
        fi

        if [ -n "$RESULT_COV" ]; then
            COV_FILES+=("$RESULT_COV")
            COV_LABELS+=("$PROJECT_NAME")
        fi

        echo ""
        echo "  $THIN"
        echo ""
    done
fi

# ════════════════════════════════════════════════════════════
# 第二部分: JS 单元测试 (Vitest)
# ════════════════════════════════════════════════════════════
if [ "$RUN_VITEST" = true ]; then
    echo -e "${BOLD}  ┌─ 第二部分: JS 单元测试 (Vitest)${NC}"
    echo ""
    TOTAL_SUITES=$((TOTAL_SUITES + 1))

    if ! command -v node &> /dev/null; then
        warn "未检测到 Node.js，跳过 JS 单元测试"
        SKIPPED_SUITES=$((SKIPPED_SUITES + 1))
    elif [ ! -f "$SCRIPT_DIR/node_modules/.bin/vitest" ]; then
        info "安装 npm 依赖..."
        npm install --prefix "$SCRIPT_DIR" > /dev/null 2>&1
    fi

    if command -v node &> /dev/null && [ -f "$SCRIPT_DIR/node_modules/.bin/vitest" ]; then
        NODE_VERSION="$(node --version 2>/dev/null)"
        info "Node.js 版本: $NODE_VERSION"

        # 运行 Vitest，输出到终端（list reporter），同时捕获退出码
        VITEST_OK=true
        echo ""
        if npx vitest run --reporter=verbose 2>&1; then
            ok "JS 单元测试全部通过"
            PASSED_SUITES=$((PASSED_SUITES + 1))
        else
            fail "JS 单元测试失败"
            FAILED_SUITES=$((FAILED_SUITES + 1))
            FAILED_NAMES+=("JS: Vitest")
            VITEST_OK=false
        fi

        echo ""
        echo "  $THIN"
        echo ""
    fi
fi

# ════════════════════════════════════════════════════════════
# 第三部分: E2E 端到端测试 (Playwright)
# ════════════════════════════════════════════════════════════
if [ "$RUN_E2E" = true ]; then
    echo -e "${BOLD}  ┌─ 第三部分: E2E 端到端测试 (Playwright)${NC}"
    echo ""
    TOTAL_SUITES=$((TOTAL_SUITES + 1))

    BASE_URL="${BASE_URL:-http://localhost:9080}"

    # 检测 Web 服务是否运行
    if ! curl -s --connect-timeout 3 "$BASE_URL" > /dev/null 2>&1; then
        warn "Web 服务未运行 ($BASE_URL)，跳过 E2E 测试"
        warn "请先运行 ./start_dev.sh && ./start_web.sh 启动服务"
        SKIPPED_SUITES=$((SKIPPED_SUITES + 1))
    else
        info "Web 服务已就绪: $BASE_URL"

        if [ ! -f "$SCRIPT_DIR/node_modules/.bin/playwright" ]; then
            info "安装 npm 依赖..."
            npm install --prefix "$SCRIPT_DIR" > /dev/null 2>&1
        fi

        echo ""
        # Playwright 使用 list reporter 直接终端输出
        if BASE_URL="$BASE_URL" npx playwright test --reporter=list 2>&1; then
            ok "E2E 测试全部通过"
            PASSED_SUITES=$((PASSED_SUITES + 1))
        else
            fail "E2E 测试失败"
            FAILED_SUITES=$((FAILED_SUITES + 1))
            FAILED_NAMES+=("E2E: Playwright")
        fi

        echo ""
        echo "  $THIN"
        echo ""
    fi
fi

# ════════════════════════════════════════════════════════════
# 覆盖率终端报告 (C# 部分)
# ════════════════════════════════════════════════════════════
if [ ${#COV_FILES[@]} -gt 0 ]; then
    echo -e "${BOLD}  代码覆盖率报告 (C#)${NC}"
    echo ""
    for i in "${!COV_FILES[@]}"; do
        parse_coverage "${COV_FILES[$i]}" "${COV_LABELS[$i]}"
    done
fi

# ════════════════════════════════════════════════════════════
# 最终汇总
# ════════════════════════════════════════════════════════════
echo -e "$LINE"
echo -e "  ${BOLD}测试汇总${NC}"
echo -e "$LINE"
echo ""
echo "  测试套件总数:  $TOTAL_SUITES"
echo -e "  ${GREEN}通过:          $PASSED_SUITES${NC}"
if [ $FAILED_SUITES -gt 0 ]; then
    echo -e "  ${RED}失败:          $FAILED_SUITES${NC}"
    echo ""
    fail "失败的套件:"
    for name in "${FAILED_NAMES[@]}"; do
        echo "    - $name"
    done
else
    echo "  失败:          0"
fi
if [ $SKIPPED_SUITES -gt 0 ]; then
    echo -e "  ${YELLOW}跳过:          $SKIPPED_SUITES${NC}"
fi
echo ""
echo -e "$LINE"

# 退出码
[ $FAILED_SUITES -gt 0 ] && exit 1
exit 0
