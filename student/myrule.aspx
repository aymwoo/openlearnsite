<%@ Page Title="" Language="C#" StylesheetTheme="Student" AutoEventWireup="true" CodeFile="myrule.aspx.cs" Inherits="Student_myrule" ResponseEncoding="utf-8" %>
    
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
        <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
<title>课堂守则</title>   
    <link href="../App_Themes/student/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
    <style>
        body {
            font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, "Noto Sans SC", sans-serif;
            background: linear-gradient(180deg, #f8fafc 0%, #eef2ff 100%);
            -webkit-font-smoothing: antialiased;
            margin: 0;
            min-height: 100vh;
        }
        .rule-page {
            min-height: 100vh;
            padding: 1.5rem;
        }
        .rule-shell {
            max-width: 980px;
            margin: 0 auto;
            display: grid;
            gap: 1.25rem;
        }
        .rule-hero,
        .rule-card,
        .rule-note {
            border: 1px solid rgba(148, 163, 184, 0.18);
            border-radius: 1.5rem;
            background: #ffffff;
            box-shadow: 0 18px 40px -30px rgba(15, 23, 42, 0.28);
        }
        .rule-hero {
            position: relative;
            overflow: hidden;
            padding: 1.8rem;
            background: linear-gradient(135deg, #312e81 0%, #4338ca 55%, #6366f1 100%);
            color: #ffffff;
        }
        .rule-hero:before,
        .rule-hero:after {
            content: "";
            position: absolute;
            border-radius: 9999px;
            background: rgba(255, 255, 255, 0.08);
        }
        .rule-hero:before {
            width: 220px;
            height: 220px;
            right: -70px;
            top: -110px;
        }
        .rule-hero:after {
            width: 180px;
            height: 180px;
            right: 12%;
            bottom: -110px;
        }
        .rule-hero-body {
            position: relative;
            z-index: 1;
        }
        .rule-eyebrow {
            display: inline-flex;
            align-items: center;
            padding: 0.36rem 0.8rem;
            border-radius: 9999px;
            border: 1px solid rgba(255, 255, 255, 0.2);
            background: rgba(255, 255, 255, 0.1);
            font-size: 0.82rem;
            letter-spacing: 0.04em;
        }
        .rule-title {
            margin: 1rem 0 0.6rem;
            font-size: 2rem;
            font-weight: 800;
            line-height: 1.15;
            color: #ffffff;
        }
        .rule-subtitle {
            margin: 0;
            max-width: 36rem;
            color: rgba(255, 255, 255, 0.88);
            font-size: 0.96rem;
            line-height: 1.8;
        }
        .rule-card {
            padding: 1.35rem;
        }
        .rule-list {
            display: grid;
            gap: 0.85rem;
        }
        .rule-item {
            display: flex;
            align-items: flex-start;
            gap: 0.9rem;
            padding: 1rem 1.05rem;
            border-radius: 1.1rem;
            background: linear-gradient(160deg, #ffffff 0%, #f8fafc 100%);
            border: 1px solid #e2e8f0;
            transition: transform 0.2s ease, box-shadow 0.2s ease, border-color 0.2s ease;
        }
        .rule-item:hover {
            transform: translateY(-1px);
            border-color: #c7d2fe;
            box-shadow: 0 16px 28px -24px rgba(79, 70, 229, 0.3);
        }
        .rule-index {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            width: 2rem;
            height: 2rem;
            border-radius: 9999px;
            background: #eef2ff;
            color: #4338ca;
            font-size: 0.9rem;
            font-weight: 800;
            flex-shrink: 0;
        }
        .rule-text {
            color: #334155;
            font-size: 0.95rem;
            line-height: 1.8;
        }
        .rule-note {
            padding: 1rem 1.1rem;
            display: flex;
            align-items: flex-start;
            gap: 0.8rem;
            background: linear-gradient(135deg, #fff7ed 0%, #fffbeb 100%);
        }
        .rule-note-icon {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            width: 2rem;
            height: 2rem;
            border-radius: 9999px;
            background: rgba(245, 158, 11, 0.16);
            color: #d97706;
            font-weight: 800;
            flex-shrink: 0;
        }
        .rule-note-title {
            margin: 0;
            color: #9a3412;
            font-size: 0.92rem;
            font-weight: 800;
        }
        .rule-note-text {
            margin: 0.25rem 0 0;
            color: #b45309;
            font-size: 0.84rem;
            line-height: 1.7;
        }
        .rule-actions {
            display: flex;
            justify-content: center;
        }
        .rule-btn {
            min-width: 9rem;
            min-height: 2.85rem;
            padding: 0 1.2rem;
            border-radius: 0.95rem;
            border: 1px solid #cbd5e1;
            background: #ffffff;
            color: #475569;
            font-size: 0.92rem;
            font-weight: 700;
            cursor: pointer;
            transition: all 0.2s ease;
        }
        .rule-btn:hover {
            background: #f8fafc;
            color: #1e293b;
            border-color: #94a3b8;
        }
        @media (max-width: 640px) {
            .rule-page { padding: 1rem; }
            .rule-hero, .rule-card { padding: 1.2rem; }
            .rule-title { font-size: 1.6rem; }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="rule-page">
        <div class="rule-shell">
            <section class="rule-hero">
                <div class="rule-hero-body">
                    <div class="rule-eyebrow">Class Rules</div>
                    <h1 class="rule-title">课堂守则</h1>
                    <p class="rule-subtitle">以下规则帮助大家共同维护良好的机房学习环境。遵守守则，才能更专注地完成课堂任务与实践活动。</p>
                </div>
            </section>

            <section class="rule-card">
                <div class="rule-list">
                    <div class="rule-item"><span class="rule-index">1</span><div class="rule-text">无请假缺席：每人扣 1 分</div></div>
                    <div class="rule-item"><span class="rule-index">2</span><div class="rule-text">迟到：每人扣 0.1 分</div></div>
                    <div class="rule-item"><span class="rule-index">3</span><div class="rule-text">吃零食带饮料：每人扣 0.1 分</div></div>
                    <div class="rule-item"><span class="rule-index">4</span><div class="rule-text">乱丢垃圾：每人扣 0.1 分且负责拖地一次</div></div>
                    <div class="rule-item"><span class="rule-index">5</span><div class="rule-text">未经老师允许玩游戏：每人扣 0.1 分</div></div>
                    <div class="rule-item"><span class="rule-index">6</span><div class="rule-text">带存储设备（mp3、U 盘）并使用：每人扣 0.1 分</div></div>
                    <div class="rule-item"><span class="rule-index">7</span><div class="rule-text">故意搞乱电脑硬件：扣 1 分</div></div>
                    <div class="rule-item"><span class="rule-index">8</span><div class="rule-text">未经老师允许，私自下座位或换座位：扣 1 分</div></div>
                </div>
            </section>

            <section class="rule-note">
                <span class="rule-note-icon">!</span>
                <div>
                    <p class="rule-note-title">学习提示</p>
                    <p class="rule-note-text">保持安静、爱护设备、按老师安排操作，不仅能减少扣分，也能让自己和同学拥有更高效的课堂体验。</p>
                </div>
            </section>

            <div class="rule-actions">
                <asp:Button ID="Btnreturn" runat="server" Text="关闭" BorderStyle="None" CssClass="rule-btn" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
