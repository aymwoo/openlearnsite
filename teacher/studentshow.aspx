<%@ Page Title="" Language="C#" StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="studentshow.aspx.cs" Inherits="Teacher_studentshow" ResponseEncoding="utf-8" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <meta charset="utf-8" />
<title></title>
    <link href="../App_Themes/Teacher/admin-form.css" rel="stylesheet" />
    <style type="text/css">
        .student-show-page {
            --admin-form-page-bg: linear-gradient(180deg, #f8fafc 0%, #eef6ff 100%);
            --admin-form-hero-bg: linear-gradient(135deg, #1d4ed8 0%, #2563eb 55%, #38bdf8 100%);
            --admin-form-hero-shadow: 0 22px 45px -28px rgba(37, 99, 235, 0.72);
            --admin-form-primary-bg: #2563eb;
            --admin-form-primary-hover: #1d4ed8;
            --admin-form-primary-shadow: 0 14px 24px -18px rgba(37, 99, 235, 0.85);
            --admin-form-secondary-border: #bfdbfe;
            --admin-form-secondary-bg: #eff6ff;
            --admin-form-secondary-hover: #dbeafe;
            --admin-form-secondary-fg: #1d4ed8;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="admin-form-page student-show-page">
        <div class="admin-form-shell">
            <section class="admin-form-hero">
                <div class="admin-form-hero-content">
                    <div class="admin-form-eyebrow">Student Profile</div>
                    <h1 class="admin-form-title">学生基本信息</h1>
                    <p class="admin-form-subtitle">查看学生个人档案与当前信息，便于核对学生资料。</p>
                </div>
            </section>

            <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
            <section class="admin-form-panel">
                <div class="admin-form-toolbar">
                    <div>
                        <h2 class="admin-form-section-title"><%# Eval("Sname") %></h2>
                        <p class="admin-form-section-desc">学号、班级、成绩与联系方式等当前信息如下。</p>
                    </div>
                    <div class="admin-form-action-row">
                        <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkEdit_Click" CssClass="admin-form-btn admin-form-btn--secondary">编辑资料</asp:LinkButton>
                    </div>
                </div>
                <div class="admin-form-kv">
                    <div class="admin-form-kv-item"><span class="admin-form-kv-label">编号</span><span class="admin-form-kv-value"><%# Eval("Sid") %></span></div>
                    <div class="admin-form-kv-item"><span class="admin-form-kv-label">学号</span><span class="admin-form-kv-value"><%# Eval("Snum") %></span></div>
                    <div class="admin-form-kv-item"><span class="admin-form-kv-label">入学</span><span class="admin-form-kv-value"><%# Eval("Syear") %></span></div>
                    <div class="admin-form-kv-item"><span class="admin-form-kv-label">年级</span><span class="admin-form-kv-value"><%# Eval("Sgrade") %></span></div>
                    <div class="admin-form-kv-item"><span class="admin-form-kv-label">班级</span><span class="admin-form-kv-value"><%# Eval("Sclass") %></span></div>
                    <div class="admin-form-kv-item"><span class="admin-form-kv-label">主任</span><span class="admin-form-kv-value"><%# Eval("Sheadtheacher") %></span></div>
                    <div class="admin-form-kv-item"><span class="admin-form-kv-label">密码</span><span class="admin-form-kv-value"><%# Eval("Spwd") %></span></div>
                    <div class="admin-form-kv-item"><span class="admin-form-kv-label">性别</span><span class="admin-form-kv-value"><%# Eval("Sex") %></span></div>
                    <div class="admin-form-kv-item"><span class="admin-form-kv-label">父母</span><span class="admin-form-kv-value"><%# Eval("Sparents") %></span></div>
                    <div class="admin-form-kv-item"><span class="admin-form-kv-label">表现</span><span class="admin-form-kv-value"><%# Eval("Sattitude") %></span></div>
                    <div class="admin-form-kv-item"><span class="admin-form-kv-label">成绩</span><span class="admin-form-kv-value"><%# Eval("Sscore") %></span></div>
                    <div class="admin-form-kv-item"><span class="admin-form-kv-label">电话</span><span class="admin-form-kv-value"><%# Eval("Sphone") %></span></div>
                    <div class="admin-form-kv-item" style="grid-column: 1 / -1;"><span class="admin-form-kv-label">地址</span><span class="admin-form-kv-value"><%# Eval("Saddress") %></span></div>
                </div>
            </section>
            </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
    </form>
</body>
</html>
