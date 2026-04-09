<%@ Page Title="" Language="C#" StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="studentedit.aspx.cs" Inherits="Teacher_studentedit" ResponseEncoding="utf-8" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <meta charset="utf-8" />
<title></title>
    <link href="../App_Themes/Teacher/admin-form.css" rel="stylesheet" />
    <style type="text/css">
        .student-edit-page {
            --admin-form-page-bg: linear-gradient(180deg, #f8fafc 0%, #eef6ff 100%);
            --admin-form-hero-bg: linear-gradient(135deg, #1e40af 0%, #2563eb 55%, #60a5fa 100%);
            --admin-form-hero-shadow: 0 22px 45px -28px rgba(37, 99, 235, 0.72);
            --admin-form-primary-bg: #2563eb;
            --admin-form-primary-hover: #1d4ed8;
            --admin-form-primary-shadow: 0 14px 24px -18px rgba(37, 99, 235, 0.85);
            --admin-form-secondary-border: #bfdbfe;
            --admin-form-secondary-bg: #eff6ff;
            --admin-form-secondary-hover: #dbeafe;
            --admin-form-secondary-fg: #1d4ed8;
            --admin-form-focus: #3b82f6;
            --admin-form-focus-ring: rgba(59, 130, 246, 0.14);
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="admin-form-page student-edit-page">
            <div class="admin-form-shell">
                <section class="admin-form-hero">
                    <div class="admin-form-hero-content">
                        <div class="admin-form-eyebrow">Student Profile</div>
                        <h1 class="admin-form-title">编辑学生信息</h1>
                        <p class="admin-form-subtitle">维护学生档案信息。学号、入学年份、表现和成绩仍按现有规则保持只读。</p>
                    </div>
                </section>

                <section class="admin-form-panel">
                    <h2 class="admin-form-section-title">基本信息</h2>
                    <p class="admin-form-section-desc">可修改姓名、班级、密码、联系方式和家庭信息，其余只读字段保留原有控制方式。</p>
                    <div class="admin-form-grid">
                        <div class="admin-form-field">
                            <label class="admin-form-label" for="<%= Tsnum.ClientID %>">学号</label>
                            <asp:TextBox ID="Tsnum" runat="server" SkinID="TextBoxNormal" Width="110px" ReadOnly="True" ToolTip="学号不可修改！" CssClass="admin-form-input admin-form-readonly"></asp:TextBox>
                        </div>
                        <div class="admin-form-field">
                            <label class="admin-form-label" for="<%= Tsname.ClientID %>">姓名</label>
                            <asp:TextBox ID="Tsname" runat="server" SkinID="TextBoxNormal" Width="110px"
                                BackColor="Cornsilk" CssClass="admin-form-input"></asp:TextBox>
                        </div>
                        <div class="admin-form-field">
                            <label class="admin-form-label" for="<%= Tsyear.ClientID %>">入学年份</label>
                            <asp:TextBox ID="Tsyear" runat="server" SkinID="TextBoxNormal" Width="110px" ReadOnly="True" CssClass="admin-form-input admin-form-readonly"></asp:TextBox>
                        </div>
                        <div class="admin-form-field">
                            <label class="admin-form-label" for="<%= DDLgrade.ClientID %>">年级</label>
                            <asp:DropDownList ID="DDLgrade" runat="server" Font-Size="9pt"
                                Width="60px" BackColor="Cornsilk" Enabled="False" CssClass="admin-form-select"></asp:DropDownList>
                        </div>
                        <div class="admin-form-field">
                            <label class="admin-form-label" for="<%= DDLclass.ClientID %>">班级</label>
                            <asp:DropDownList ID="DDLclass" runat="server" Font-Size="9pt" Width="60px"
                                BackColor="Cornsilk" CssClass="admin-form-select"></asp:DropDownList>
                        </div>
                        <div class="admin-form-field">
                            <label class="admin-form-label" for="<%= Tsheadtheacher.ClientID %>">主任</label>
                            <asp:TextBox ID="Tsheadtheacher" runat="server" SkinID="TextBoxNormal"
                                Width="110px" BackColor="Cornsilk" CssClass="admin-form-input"></asp:TextBox>
                        </div>
                        <div class="admin-form-field">
                            <label class="admin-form-label" for="<%= Tspwd.ClientID %>">密码</label>
                            <asp:TextBox ID="Tspwd" runat="server" SkinID="TextBoxNormal" Width="110px"
                                ToolTip="密码可修改！" BackColor="Cornsilk" CssClass="admin-form-input"></asp:TextBox>
                        </div>
                        <div class="admin-form-field">
                            <label class="admin-form-label" for="<%= DDLsex.ClientID %>">性别</label>
                            <asp:DropDownList ID="DDLsex" runat="server" Font-Size="9pt" Width="60px"
                                BackColor="Cornsilk" CssClass="admin-form-select"></asp:DropDownList>
                        </div>
                        <div class="admin-form-field">
                            <label class="admin-form-label" for="<%= Tsparents.ClientID %>">父母</label>
                            <asp:TextBox ID="Tsparents" runat="server" SkinID="TextBoxNormal"
                                Width="110px" BackColor="Cornsilk" CssClass="admin-form-input"></asp:TextBox>
                        </div>
                        <div class="admin-form-field">
                            <label class="admin-form-label" for="<%= Tsattitude.ClientID %>">表现</label>
                            <asp:TextBox ID="Tsattitude" runat="server" SkinID="TextBoxNormal" Width="110px" ReadOnly="True" ToolTip="表现不可修改！" CssClass="admin-form-input admin-form-readonly"></asp:TextBox>
                        </div>
                        <div class="admin-form-field">
                            <label class="admin-form-label" for="<%= Tsscore.ClientID %>">成绩</label>
                            <asp:TextBox ID="Tsscore" runat="server" SkinID="TextBoxNormal" Width="110px" ReadOnly="True" ToolTip="成绩不可修改！" CssClass="admin-form-input admin-form-readonly"></asp:TextBox>
                        </div>
                        <div class="admin-form-field">
                            <label class="admin-form-label" for="<%= Tsphone.ClientID %>">电话</label>
                            <asp:TextBox ID="Tsphone" runat="server" SkinID="TextBoxNormal"
                                Width="110px" BackColor="Cornsilk" CssClass="admin-form-input"></asp:TextBox>
                        </div>
                        <div class="admin-form-field admin-form-field-wide">
                            <label class="admin-form-label" for="<%= Tsaddress.ClientID %>">地址</label>
                            <asp:TextBox ID="Tsaddress" runat="server" SkinID="TextBoxNormal"
                                Width="510px" BackColor="Cornsilk" CssClass="admin-form-input"></asp:TextBox>
                        </div>
                    </div>
                </section>

                <section class="admin-form-actions">
                    <div class="admin-form-action-row">
                        <asp:Button ID="Btnsedit" runat="server" OnClick="BtnsEdit_Click" Text="保存修改" CssClass="admin-form-btn admin-form-btn--primary" />
                    </div>
                </section>
            </div>
        </div>
    </form>
</body>
</html>
