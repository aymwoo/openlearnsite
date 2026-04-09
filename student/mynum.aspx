<%@ Page Title="" Language="C#" StylesheetTheme="Student" AutoEventWireup="true"
    CodeFile="mynum.aspx.cs" Inherits="Student_mynum" ResponseEncoding="utf-8" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
        <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
<title>学号查询</title>
    <link href="../App_Themes/Student/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../js/tooltip.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.8.2.min.js" type="text/javascript"></script>
    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
    <style>
        body {
            font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, "Noto Sans SC", sans-serif;
            -webkit-font-smoothing: antialiased;
            margin: 0;
            min-height: 100vh;
        }
        .bg-pattern {
            background-color: #f3f4f6;
            background-image: url('data:image/svg+xml,%3Csvg width="60" height="60" viewBox="0 0 60 60" xmlns="http://www.w3.org/2000/svg"%3E%3Cg fill="none" fill-rule="evenodd"%3E%3Cg fill="%239C92AC" fill-opacity="0.08"%3E%3Cpath d="M36 34v-4h-2v4h-4v2h4v4h2v-4h4v-2h-4zm0-30V0h-2v4h-4v2h4v4h2V6h4V4h-4zM6 34v-4H4v4H0v2h4v4h2v-4h4v-2H6zM6 4V0H4v4H0v2h4v4h2V6h4V4H6z"/%3E%3C/g%3E%3C/g%3E%3C/svg%3E');
        }
        .compact-shell {
            min-height: 100vh;
        }
        .index-header {
            display: block;
            width: 100%;
            background: #ffffff;
            border-bottom: 1px solid #e5e7eb;
            box-shadow: 0 8px 24px rgba(15, 23, 42, 0.05);
        }
        .index-footer {
            display: block;
            width: 100%;
            background: rgba(255, 255, 255, 0.88);
            border-top: 1px solid #e5e7eb;
        }
        .index-shell {
            max-width: 72rem;
            margin: 0 auto;
            padding: 0.75rem 1rem;
            display: flex;
            justify-content: space-between;
            align-items: center;
            gap: 1rem;
        }
        .index-brand {
            display: flex;
            align-items: center;
            min-width: 0;
        }
        .index-logo {
            width: 2rem;
            height: 2rem;
            margin-right: 0.75rem;
            color: #2563eb;
            flex-shrink: 0;
        }
        .index-site-title {
            margin: 0;
            color: #1f2937;
            font-size: 1.5rem;
            font-weight: 700;
            line-height: 1.2;
        }
        .index-welcome {
            color: #6b7280;
            font-size: 0.875rem;
            white-space: nowrap;
        }
        .index-footer-inner {
            max-width: 72rem;
            margin: 0 auto;
            padding: 0 1rem;
            display: flex;
            justify-content: space-between;
            align-items: center;
            gap: 0.75rem 1rem;
            flex-wrap: wrap;
            color: #6b7280;
            font-size: 0.75rem;
        }
        .index-footer-meta,
        .index-footer-status {
            display: flex;
            align-items: center;
            gap: 0.75rem 1rem;
            flex-wrap: wrap;
        }
        .index-footer-link {
            display: inline-flex;
            align-items: center;
            padding: 0.25rem 0.75rem;
            border-radius: 9999px;
            border: 1px solid #e5e7eb;
            background: #f3f4f6;
            color: #4b5563;
            text-decoration: none;
            font-weight: 500;
        }
        .index-footer-note {
            margin-top: 0.125rem;
            text-align: center;
            color: #9ca3af;
            font-size: 0.75rem;
        }
        .index-info {
            display: inline-flex;
            align-items: center;
        }
        .index-info-icon {
            width: 1rem;
            height: 1rem;
            margin-right: 0.25rem;
            color: #9ca3af;
        }
        .mynum-page {
            min-height: 100vh;
            padding: 1.5rem;
        }
        .mynum-shell {
            max-width: 1200px;
            margin: 0 auto;
            display: grid;
            gap: 1.25rem;
        }
        .mynum-hero,
        .mynum-card,
        .mynum-tip {
            border: 1px solid rgba(148, 163, 184, 0.18);
            border-radius: 1.5rem;
            background: #ffffff;
            box-shadow: 0 18px 40px -30px rgba(15, 23, 42, 0.28);
        }
        .mynum-hero {
            position: relative;
            overflow: hidden;
            padding: 1.8rem;
            background: linear-gradient(135deg, #312e81 0%, #4338ca 55%, #6366f1 100%);
            color: #ffffff;
        }
        .mynum-hero:before,
        .mynum-hero:after {
            content: "";
            position: absolute;
            border-radius: 9999px;
            background: rgba(255, 255, 255, 0.08);
        }
        .mynum-hero:before {
            width: 220px;
            height: 220px;
            right: -70px;
            top: -110px;
        }
        .mynum-hero:after {
            width: 180px;
            height: 180px;
            right: 12%;
            bottom: -110px;
        }
        .mynum-hero-body {
            position: relative;
            z-index: 1;
        }
        .mynum-eyebrow {
            display: inline-flex;
            align-items: center;
            gap: 0.5rem;
            padding: 0.36rem 0.8rem;
            border-radius: 9999px;
            border: 1px solid rgba(255, 255, 255, 0.2);
            background: rgba(255, 255, 255, 0.1);
            font-size: 0.82rem;
            letter-spacing: 0.04em;
        }
        .mynum-title {
            margin: 1rem 0 0.6rem;
            font-size: 2rem;
            font-weight: 800;
            line-height: 1.15;
            color: #ffffff;
        }
        .mynum-subtitle {
            margin: 0;
            max-width: 36rem;
            color: rgba(255, 255, 255, 0.88);
            font-size: 0.96rem;
            line-height: 1.8;
        }
        .mynum-card {
            padding: 1.35rem;
        }
        .mynum-toolbar {
            display: flex;
            flex-wrap: wrap;
            gap: 0.9rem;
            align-items: end;
        }
        .mynum-field {
            display: grid;
            gap: 0.42rem;
            min-width: 9rem;
        }
        .mynum-label {
            color: #475569;
            font-size: 0.84rem;
            font-weight: 700;
        }
        .mynum-select,
        .mynum-pwd {
            min-height: 2.8rem;
            padding: 0.68rem 0.9rem;
            border: 1px solid #cbd5e1;
            border-radius: 0.95rem;
            box-sizing: border-box;
            background: #f8fafc;
            color: #0f172a;
            font-size: 0.9rem;
        }
        .mynum-select:focus {
            outline: none;
            border-color: #93c5fd;
            background: #ffffff;
            box-shadow: 0 0 0 4px rgba(191, 219, 254, 0.65);
        }
        .mynum-query-btn {
            min-height: 2.8rem;
            padding: 0 1.2rem;
            border-radius: 0.95rem;
            border: none;
            background: linear-gradient(135deg, #2563eb 0%, #4f46e5 100%);
            color: #ffffff;
            font-size: 0.9rem;
            font-weight: 700;
            cursor: pointer;
            box-shadow: 0 14px 26px -20px rgba(37, 99, 235, 0.95);
        }
        .mynum-password-wrap {
            margin-left: auto;
            display: grid;
            gap: 0.42rem;
        }
        .mynum-pwd {
            width: 7rem;
            border: 1px dashed #a5b4fc;
            background: #eef2ff;
            color: #db2777;
            font-size: 1.15rem;
            font-weight: 800;
            text-align: center;
            cursor: pointer;
        }
        .mynum-list-card {
            padding: 1.25rem;
            overflow-x: auto;
        }
        .mynum-list-head {
            display: flex;
            justify-content: space-between;
            align-items: center;
            gap: 1rem;
            margin-bottom: 1rem;
        }
        .mynum-list-title {
            margin: 0;
            color: #0f172a;
            font-size: 1.05rem;
            font-weight: 800;
        }
        .mynum-list-desc {
            margin: 0.25rem 0 0;
            color: #64748b;
            font-size: 0.84rem;
        }
        .mynum-grid {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(108px, 1fr));
            gap: 0.9rem;
        }
        .stunum {
            display: flex;
            flex-direction: column;
            align-items: center;
            gap: 0.55rem;
            padding: 0.95rem 0.7rem;
            border-radius: 1.1rem;
            border: 1px solid #e2e8f0;
            background: linear-gradient(160deg, #ffffff 0%, #f8fafc 100%);
            transition: transform 0.2s ease, box-shadow 0.2s ease, border-color 0.2s ease;
        }
        .stunum:hover {
            transform: translateY(-1px);
            border-color: #c7d2fe;
            box-shadow: 0 16px 28px -24px rgba(79, 70, 229, 0.3);
        }
        .stuimg {
            width: 54px;
            height: 54px;
            border-radius: 9999px;
            object-fit: cover;
            border: 2px solid #dbeafe;
            background: #ffffff;
        }
        .stulink {
            color: #1e293b;
            font-size: 0.84rem;
            font-weight: 700;
            line-height: 1.5;
            text-align: center;
            text-decoration: none;
            word-break: break-word;
        }
        .stulink:hover {
            color: #4338ca;
        }
        .mynum-tip {
            padding: 1rem 1.1rem;
            display: flex;
            align-items: flex-start;
            gap: 0.8rem;
            background: linear-gradient(135deg, #eff6ff 0%, #f8fbff 100%);
        }
        .mynum-tip-icon {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            width: 2rem;
            height: 2rem;
            border-radius: 9999px;
            background: rgba(79, 70, 229, 0.12);
            color: #4f46e5;
            font-weight: 800;
            flex-shrink: 0;
        }
        .mynum-tip-title {
            margin: 0;
            color: #312e81;
            font-size: 0.92rem;
            font-weight: 800;
        }
        .mynum-tip-text {
            margin: 0.25rem 0 0;
            color: #475569;
            font-size: 0.84rem;
            line-height: 1.7;
        }
        @media (max-width: 900px) {
            .mynum-page { padding: 1rem; }
            .mynum-password-wrap { margin-left: 0; }
        }
        @media (max-width: 640px) {
            .index-shell,
            .index-footer-inner {
                padding-left: 1rem;
                padding-right: 1rem;
            }
            .index-site-title {
                font-size: 1.25rem;
            }
            .index-welcome {
                display: none;
            }
            .mynum-hero, .mynum-card, .mynum-list-card { padding: 1.1rem; }
            .mynum-title { font-size: 1.6rem; }
            .mynum-toolbar { align-items: stretch; }
            .mynum-field, .mynum-password-wrap { width: 100%; }
            .mynum-pwd { width: 100%; }
        }
    </style>
</head>
<body class="bg-pattern min-h-screen flex flex-col font-sans text-gray-800">
    <form id="form1" runat="server" class="compact-shell flex-grow flex flex-col">
    <header class="index-header w-full bg-white border-b border-gray-200">
        <div class="index-shell max-w-6xl mx-auto px-4 sm:px-6 lg:px-8 py-2.5 sm:py-3 flex justify-between items-center">
            <div class="index-brand flex items-center">
                <svg class="index-logo h-8 w-8 text-blue-600 mr-3" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6.253v13m0-13C10.832 5.477 9.246 5 7.5 5S4.168 5.477 3 6.253v13C4.168 18.477 5.754 18 7.5 18s3.332.477 4.5 1.253m0-13C13.168 5.477 14.754 5 16.5 5c1.747 0 3.332.477 4.5 1.253v13C19.832 18.477 18.246 18 16.5 18c-1.746 0-3.332.477-4.5 1.253" />
                </svg>
                <h1 class="index-site-title text-xl sm:text-2xl font-bold text-gray-800 tracking-tight"><%= SiteTitle %></h1>
            </div>
            <div>
                <span class="index-welcome text-sm text-gray-500">欢迎来到学习平台</span>
            </div>
        </div>
    </header>

    <main class="flex-grow">
    <div class="mynum-page">
        <div class="mynum-shell">
            <section class="mynum-hero">
                <div class="mynum-hero-body">
                    <div class="mynum-eyebrow">Student Finder</div>
                    <h1 class="mynum-title">学号查询</h1>
                    <p class="mynum-subtitle">按年级和班级查看学生列表，快速获取登录密码。点击密码可直接复制，点击学生卡片可进入登录入口。</p>
                </div>
            </section>

            <section class="mynum-card">
                <div class="mynum-toolbar">
                    <div class="mynum-field">
                        <label class="mynum-label">年级</label>
                        <asp:DropDownList ID="DDLgrade" runat="server" AutoPostBack="True" onselectedindexchanged="DDLgrade_SelectedIndexChanged" CssClass="mynum-select"></asp:DropDownList>
                    </div>
                    <div class="mynum-field">
                        <label class="mynum-label">班级</label>
                        <asp:DropDownList ID="DDLclass" runat="server" AutoPostBack="True" onselectedindexchanged="DDLclass_SelectedIndexChanged" CssClass="mynum-select"></asp:DropDownList>
                    </div>
                    <asp:Button ID="BtnSearch" runat="server" OnClick="BtnSearch_Click" Text="查询" BorderStyle="None" CssClass="mynum-query-btn" />
                    <div class="mynum-password-wrap">
                        <label class="mynum-label">班级密码</label>
                        <asp:TextBox ID="TextBoxPwd" runat="server" ReadOnly="True" CssClass="mynum-pwd" onClick="copy()">123</asp:TextBox>
                    </div>
                </div>
            </section>

            <section class="mynum-card mynum-list-card">
                <div class="mynum-list-head">
                    <div>
                        <h2 class="mynum-list-title">学生列表</h2>
                        <p class="mynum-list-desc">头像、机号和姓名会一起展示，方便快速核对学生身份。</p>
                    </div>
                </div>
                <asp:DataList ID="DataListsnum" runat="server" RepeatDirection="Horizontal" RepeatColumns="10" CellPadding="8" OnItemDataBound="DataListsnum_ItemDataBound" HorizontalAlign="Center" CellSpacing="2">
                    <ItemTemplate>
                        <div class="stunum">
                            <asp:HyperLink ID="HLImage" runat="server">
                                <asp:Image ID="ImageStu" class="stuimg" runat="server" Visible="True" />
                            </asp:HyperLink>
                            <asp:HyperLink ID="HLSnum" runat="server" Text='<%# Eval("Sname") %>' ToolTip='<%# Eval("Snum") %>' CssClass="stulink"></asp:HyperLink>
                        </div>
                    </ItemTemplate>
                </asp:DataList>
            </section>

            <section class="mynum-tip">
                <span class="mynum-tip-icon">i</span>
                <div>
                    <p class="mynum-tip-title">使用说明</p>
                    <p class="mynum-tip-text">如果当前班级为班级密码登录模式，可直接复制右侧密码给学生使用。若头像未显示，通常是因为当前不在同一网段。</p>
                </div>
            </section>
        </div>
    </div>
    </main>

    <footer class="index-footer w-full backdrop-blur-md border-t border-gray-200 py-3 mt-auto">
        <div class="index-footer-inner max-w-6xl mx-auto px-4 sm:px-6 lg:px-8 flex flex-col md:flex-row justify-between items-center text-xs text-gray-500 space-y-2.5 md:space-y-0">
            <div class="index-footer-meta flex items-center space-x-4">
                <asp:Label ID="Labelversion" runat="server"></asp:Label>
                <asp:HyperLink ID="HLTeacher" runat="server" NavigateUrl="~/teacher/index.aspx" Target="_blank" CssClass="index-footer-link font-medium text-gray-600 hover:text-blue-600 transition-colors bg-gray-100 px-3 py-1 rounded-full border border-gray-200">
                    教师平台
                </asp:HyperLink>
            </div>

            <div class="index-footer-status flex flex-wrap justify-center md:justify-end items-center gap-x-4 gap-y-2">
                <span class="index-info flex items-center">
                    <svg class="index-info-icon h-4 w-4 mr-1 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 12a9 9 0 01-9 9m9-9a9 9 0 00-9-9m9 9H3m9 9a9 9 0 01-9-9m9 9c1.657 0 3-4.03 3-9s-1.343-9-3-9m0 18c-1.657 0-3-4.03-3-9s1.343-9 3-9m-9 9a9 9 0 019-9" />
                    </svg>
                    IP:
                    <asp:Label ID="Labelip" runat="server" CssClass="ml-1 font-mono"></asp:Label>
                </span>
                <span class="index-info flex items-center">
                    <svg class="index-info-icon h-4 w-4 mr-1 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 3v2m6-2v2M9 19v2m6-2v2M5 9H3m2 6H3m18-6h-2m2 6h-2M7 19h10a2 2 0 002-2V7a2 2 0 00-2-2H7a2 2 0 00-2 2v10a2 2 0 002 2zM9 9h6v6H9V9z" />
                    </svg>
                    主机:
                    <asp:Label ID="Labelhostname" runat="server" CssClass="ml-1 font-mono"></asp:Label>
                </span>
                <span class="index-info flex items-center">
                    <svg class="index-info-icon h-4 w-4 mr-1 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" />
                    </svg>
                    第<asp:Label ID="Labelterm" runat="server" CssClass="mx-1 font-semibold text-gray-700"></asp:Label>学期
                </span>
            </div>
        </div>
        <div class="index-footer-note text-center mt-0.5 text-gray-400 text-[11px] sm:text-xs">
            <asp:Label ID="Labelloadtime" runat="server" Font-Italic="True"></asp:Label>
        </div>
    </footer>

    <script src="../js/ToolTip.js" type="text/javascript"></script>
    <script type="text/javascript">
        var msg = document.getElementById("TextBoxPwd");  
        msg.title ='点击复制';
        function copy() {          
            // 使用示例
            copyTextToClipboard(msg.value);
            msg.title ='已复制';
        }
        async function copyTextToClipboard(text) {
            try {
                await navigator.clipboard.writeText(text);
                console.log('已复制');
            } catch (err) {
                console.error('Failed to copy: ', err);
            }
        }
    </script>
    </form>
</body>
</html>
