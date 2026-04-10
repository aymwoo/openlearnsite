<%@ Page Language="C#" AutoEventWireup="true"  StylesheetTheme="Student" CodeFile="register.aspx.cs" Inherits="Student_register" ResponseEncoding="utf-8" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
        <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
<title>新学员注册</title>   
    <link href="../App_Themes/student/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
    <style>
        body {
            font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, "Noto Sans SC", sans-serif;
            margin: 0;
            min-height: 100vh;
            -webkit-font-smoothing: antialiased;
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
        .register-page {
            min-height: 100vh;
            padding: 1.5rem;
        }
        .register-shell {
            max-width: 1100px;
            margin: 0 auto;
            display: grid;
            grid-template-columns: minmax(0, 1.05fr) minmax(320px, 0.95fr);
            gap: 1.5rem;
            align-items: stretch;
        }
        .register-hero,
        .register-card,
        .register-tip {
            border: 1px solid rgba(148, 163, 184, 0.18);
            border-radius: 1.5rem;
            background: #ffffff;
            box-shadow: 0 18px 40px -30px rgba(15, 23, 42, 0.28);
        }
        .register-hero {
            position: relative;
            overflow: hidden;
            padding: 2rem;
            background: linear-gradient(135deg, #312e81 0%, #4338ca 55%, #6366f1 100%);
            color: #ffffff;
        }
        .register-hero:before,
        .register-hero:after {
            content: "";
            position: absolute;
            border-radius: 9999px;
            background: rgba(255, 255, 255, 0.08);
            pointer-events: none;
        }
        .register-hero:before {
            width: 220px;
            height: 220px;
            right: -70px;
            top: -110px;
        }
        .register-hero:after {
            width: 180px;
            height: 180px;
            right: 12%;
            bottom: -110px;
        }
        .register-hero-body {
            position: relative;
            z-index: 1;
        }
        .register-eyebrow {
            display: inline-flex;
            align-items: center;
            gap: 0.5rem;
            padding: 0.38rem 0.85rem;
            border-radius: 9999px;
            border: 1px solid rgba(255, 255, 255, 0.2);
            background: rgba(255, 255, 255, 0.1);
            font-size: 0.82rem;
            letter-spacing: 0.04em;
        }
        .register-title {
            margin: 1rem 0 0.65rem;
            font-size: 2rem;
            line-height: 1.15;
            font-weight: 800;
            color: #ffffff;
        }
        .register-subtitle {
            margin: 0;
            max-width: 32rem;
            color: rgba(255, 255, 255, 0.88);
            font-size: 0.97rem;
            line-height: 1.8;
        }
        .register-steps {
            position: relative;
            z-index: 1;
            display: grid;
            gap: 0.85rem;
            margin-top: 1.5rem;
        }
        .register-step {
            display: flex;
            align-items: flex-start;
            gap: 0.85rem;
            padding: 0.9rem 1rem;
            border-radius: 1rem;
            background: rgba(255, 255, 255, 0.12);
            border: 1px solid rgba(255, 255, 255, 0.12);
        }
        .register-step-index {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            width: 1.9rem;
            height: 1.9rem;
            border-radius: 9999px;
            background: rgba(255, 255, 255, 0.16);
            font-size: 0.85rem;
            font-weight: 800;
            flex-shrink: 0;
        }
        .register-step strong {
            display: block;
            font-size: 0.95rem;
            color: #ffffff;
        }
        .register-step span {
            display: block;
            margin-top: 0.25rem;
            color: rgba(255, 255, 255, 0.82);
            font-size: 0.84rem;
            line-height: 1.6;
        }
        .register-card {
            padding: 1.6rem;
        }
        .register-card-head {
            margin-bottom: 1.25rem;
        }
        .register-card-title {
            margin: 0;
            color: #0f172a;
            font-size: 1.35rem;
            font-weight: 800;
        }
        .register-card-desc {
            margin: 0.55rem 0 0;
            color: #64748b;
            font-size: 0.9rem;
            line-height: 1.7;
        }
        .register-form {
            display: grid;
            gap: 1rem;
        }
        .register-field {
            display: grid;
            gap: 0.45rem;
        }
        .register-label {
            color: #334155;
            font-size: 0.88rem;
            font-weight: 700;
        }
        .register-input,
        .register-select {
            width: 100%;
            min-height: 2.9rem;
            padding: 0.72rem 0.95rem;
            border: 1px solid #cbd5e1;
            border-radius: 0.95rem;
            box-sizing: border-box;
            background: #f8fafc;
            color: #0f172a;
            font-size: 0.92rem;
            transition: border-color 0.2s ease, box-shadow 0.2s ease, background 0.2s ease;
        }
        .register-input:focus,
        .register-select:focus {
            outline: none;
            border-color: #93c5fd;
            background: #ffffff;
            box-shadow: 0 0 0 4px rgba(191, 219, 254, 0.65);
        }
        .register-msg {
            min-height: 1.4rem;
            padding: 0.15rem 0;
            text-align: center;
            color: #dc2626;
            font-size: 0.88rem;
            font-weight: 700;
        }
        .register-actions {
            display: flex;
            gap: 0.8rem;
            flex-wrap: wrap;
            padding-top: 0.25rem;
        }
        .register-btn,
        .register-btn-secondary {
            min-width: 8.5rem;
            min-height: 2.9rem;
            padding: 0 1.2rem;
            border-radius: 0.95rem;
            border: 1px solid transparent;
            font-size: 0.92rem;
            font-weight: 700;
            cursor: pointer;
            transition: all 0.2s ease;
        }
        .register-btn {
            background: linear-gradient(135deg, #2563eb 0%, #4f46e5 100%);
            color: #ffffff;
            box-shadow: 0 14px 26px -20px rgba(37, 99, 235, 0.95);
        }
        .register-btn:hover {
            filter: brightness(1.03);
        }
        .register-btn-secondary {
            background: #ffffff;
            color: #475569;
            border-color: #cbd5e1;
        }
        .register-btn-secondary:hover {
            background: #f8fafc;
            color: #1e293b;
            border-color: #94a3b8;
        }
        .register-tip {
            margin-top: 1rem;
            padding: 1rem 1.1rem;
            display: flex;
            align-items: flex-start;
            gap: 0.8rem;
            background: linear-gradient(135deg, #fff7ed 0%, #fffbeb 100%);
        }
        .register-tip-icon {
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
        .register-tip-title {
            margin: 0;
            color: #9a3412;
            font-size: 0.92rem;
            font-weight: 800;
        }
        .register-tip-text {
            margin: 0.25rem 0 0;
            color: #b45309;
            font-size: 0.84rem;
            line-height: 1.7;
        }
        @media (max-width: 960px) {
            .register-shell {
                grid-template-columns: 1fr;
            }
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
            .register-page {
                padding: 1rem;
            }
            .register-hero,
            .register-card {
                padding: 1.2rem;
            }
            .register-title {
                font-size: 1.65rem;
            }
            .register-actions {
                flex-direction: column;
            }
            .register-btn,
            .register-btn-secondary {
                width: 100%;
            }
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
                <h1 class="index-site-title text-xl sm:text-2xl font-bold text-gray-800 tracking-tight"><asp:Literal ID="LitSiteTitle" runat="server" /></h1>
            </div>
            <div>
                <span class="index-welcome text-sm text-gray-500">欢迎来到学习平台</span>
            </div>
        </div>
    </header>

    <main class="flex-grow">
    <div class="register-page">
        <div class="register-shell">
            <section class="register-hero">
                <div class="register-hero-body">
                    <div class="register-eyebrow">Student Register</div>
                    <h1 class="register-title">欢迎加入本节学习班级</h1>
                    <p class="register-subtitle">完成基础信息登记后，系统会自动为你分配学号和默认密码。注册成功后会直接进入学生学习入口。</p>
                    <div class="register-steps">
                        <div class="register-step">
                            <span class="register-step-index">1</span>
                            <div>
                                <strong>先确认班级</strong>
                                <span>请按老师通知选择正确的年级和班级，避免注册到错误班级。</span>
                            </div>
                        </div>
                        <div class="register-step">
                            <span class="register-step-index">2</span>
                            <div>
                                <strong>使用中文姓名</strong>
                                <span>姓名需要使用中文，长度不能过长，系统会据此生成你的学习身份。</span>
                            </div>
                        </div>
                        <div class="register-step">
                            <span class="register-step-index">3</span>
                            <div>
                                <strong>注册后立即进入</strong>
                                <span>注册成功后系统会自动登录，并根据当前开放课程跳转到学习页。</span>
                            </div>
                        </div>
                    </div>
                </div>
            </section>

            <div>
                <section class="register-card">
                    <div class="register-card-head">
                        <h2 class="register-card-title">新学员注册</h2>
                        <p class="register-card-desc">请准确填写以下信息。带有班级开放注册时，即可完成新学员登记。</p>
                    </div>

                    <div class="register-form">
                        <div class="register-field">
                            <asp:Label runat="server" AssociatedControlID="DDLgrade" CssClass="register-label">年级选择</asp:Label>
                            <asp:DropDownList ID="DDLgrade" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLgrade_SelectedIndexChanged" CssClass="register-select"></asp:DropDownList>
                        </div>

                        <div class="register-field">
                            <asp:Label runat="server" AssociatedControlID="DDLclass" CssClass="register-label">班级选择</asp:Label>
                            <asp:DropDownList ID="DDLclass" runat="server" CssClass="register-select"></asp:DropDownList>
                        </div>

                        <div class="register-field">
                            <asp:Label runat="server" AssociatedControlID="DDLsex" CssClass="register-label">性别选择</asp:Label>
                            <asp:DropDownList ID="DDLsex" runat="server" CssClass="register-select"></asp:DropDownList>
                        </div>

                        <div class="register-field">
                            <asp:Label runat="server" AssociatedControlID="Tsname" CssClass="register-label">姓名</asp:Label>
                            <asp:TextBox ID="Tsname" runat="server" CssClass="register-input"></asp:TextBox>
                        </div>

                        <asp:Label ID="labelmsg" runat="server" SkinID="LabelMsgRed" CssClass="register-msg"></asp:Label>

                        <div class="register-actions">
                            <asp:Button ID="BtnRegister" runat="server" onclick="BtnRegister_Click" Text="确定注册" CssClass="register-btn" />
                            <asp:Button ID="BtnReturn" runat="server" onclick="BtnReturn_Click" Text="返回首页" CssClass="register-btn-secondary" />
                        </div>
                    </div>
                </section>

                <section class="register-tip">
                    <span class="register-tip-icon">!</span>
                    <div>
                        <p class="register-tip-title">友情提示</p>
                        <p class="register-tip-text">请选择老师指定的年级和班级进行注册，以免错班后无法正常进入课程或被老师统一管理。</p>
                    </div>
                </section>
            </div>
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
    </form>
</body>
</html>
