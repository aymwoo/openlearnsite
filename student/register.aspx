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
            background: linear-gradient(180deg, #f8fafc 0%, #eef2ff 100%);
            -webkit-font-smoothing: antialiased;
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
<body>
    <form id="form1" runat="server">
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
                            <label class="register-label" for="<%= DDLgrade.ClientID %>">年级选择</label>
                            <asp:DropDownList ID="DDLgrade" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLgrade_SelectedIndexChanged" CssClass="register-select"></asp:DropDownList>
                        </div>

                        <div class="register-field">
                            <label class="register-label" for="<%= DDLclass.ClientID %>">班级选择</label>
                            <asp:DropDownList ID="DDLclass" runat="server" CssClass="register-select"></asp:DropDownList>
                        </div>

                        <div class="register-field">
                            <label class="register-label" for="<%= DDLsex.ClientID %>">性别选择</label>
                            <asp:DropDownList ID="DDLsex" runat="server" CssClass="register-select"></asp:DropDownList>
                        </div>

                        <div class="register-field">
                            <label class="register-label" for="<%= Tsname.ClientID %>">姓名</label>
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
    </form>
</body>
</html>
