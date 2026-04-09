<%@ Page Title="" Language="C#"  StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="studentadd.aspx.cs" Inherits="Teacher_studentadd" ResponseEncoding="utf-8" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <meta charset="utf-8" />
<title></title>
    <link href="../App_Themes/Teacher/admin-form.css" rel="stylesheet" />
    <style type="text/css">
        .student-admin-page {
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
            --admin-form-focus: #3b82f6;
            --admin-form-focus-ring: rgba(59, 130, 246, 0.14);
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="admin-form-page student-admin-page">
            <div class="admin-form-shell">
                <section class="admin-form-hero">
                    <div class="admin-form-hero-content">
                        <div class="admin-form-eyebrow">Student Profile</div>
                        <h1 class="admin-form-title">新增学生</h1>
                        <p class="admin-form-subtitle">录入学生基础档案信息。学号、年级、班级和入学年份仍按原有规则自动生成或锁定。</p>
                    </div>
                </section>

                <section class="admin-form-panel">
                    <h2 class="admin-form-section-title">基本信息</h2>
                    <p class="admin-form-section-desc">填写姓名、性别、联系方式和家庭信息，系统会保留默认初始成绩与表现值。</p>
                    <div class="admin-form-grid">
                        <div class="admin-form-field">
                            <label class="admin-form-label" for="<%= Tsnum.ClientID %>">学号</label>
                            <asp:TextBox ID="Tsnum" runat="server" BorderColor="Gainsboro"
                                BorderStyle="Solid" BorderWidth="1px" Width="110px"
                                ToolTip="自动生成！" CssClass="admin-form-input admin-form-readonly" ReadOnly="True"></asp:TextBox>
                        </div>
                        <div class="admin-form-field">
                            <label class="admin-form-label" for="<%= Tsname.ClientID %>">姓名</label>
                            <asp:TextBox ID="Tsname" runat="server" BackColor="Cornsilk" BorderColor="#E0E0E0"
                                BorderStyle="Solid" BorderWidth="1px" Width="110px" CssClass="admin-form-input"></asp:TextBox>
                        </div>
                        <div class="admin-form-field">
                            <label class="admin-form-label" for="<%= DDLyear.ClientID %>">入学年份</label>
                            <asp:DropDownList ID="DDLyear" runat="server" Font-Size="9pt" Width="60px"
                                BackColor="Cornsilk" CssClass="admin-form-select"></asp:DropDownList>
                        </div>
                        <div class="admin-form-field">
                            <label class="admin-form-label" for="<%= DDLgrade.ClientID %>">年级</label>
                            <asp:DropDownList ID="DDLgrade" runat="server" Font-Size="9pt"
                                Width="60px" BackColor="Cornsilk" CssClass="admin-form-select"></asp:DropDownList>
                        </div>
                        <div class="admin-form-field">
                            <label class="admin-form-label" for="<%= DDLclass.ClientID %>">班级</label>
                            <asp:DropDownList ID="DDLclass" runat="server" Font-Size="9pt" Width="60px"
                                BackColor="Cornsilk" CssClass="admin-form-select"></asp:DropDownList>
                        </div>
                        <div class="admin-form-field">
                            <label class="admin-form-label" for="<%= Tsheadtheacher.ClientID %>">主任</label>
                            <asp:TextBox ID="Tsheadtheacher" runat="server" BackColor="Cornsilk" BorderColor="#E0E0E0"
                                BorderStyle="Solid" BorderWidth="1px" Width="110px" CssClass="admin-form-input"></asp:TextBox>
                        </div>
                        <div class="admin-form-field">
                            <label class="admin-form-label" for="<%= Tspwd.ClientID %>">密码</label>
                            <asp:TextBox ID="Tspwd" runat="server" BackColor="White" BorderColor="Gainsboro"
                                BorderStyle="Solid" BorderWidth="1px" Width="110px" ReadOnly="True" ToolTip="密码不可修改！" CssClass="admin-form-input admin-form-readonly">12345</asp:TextBox>
                        </div>
                        <div class="admin-form-field">
                            <label class="admin-form-label" for="<%= DDLsex.ClientID %>">性别</label>
                            <asp:DropDownList ID="DDLsex" runat="server" Font-Size="9pt" Width="60px"
                                BackColor="Cornsilk" CssClass="admin-form-select"></asp:DropDownList>
                        </div>
                        <div class="admin-form-field">
                            <label class="admin-form-label" for="<%= Tsparents.ClientID %>">父母</label>
                            <asp:TextBox ID="Tsparents" runat="server" BackColor="Cornsilk" BorderColor="#E0E0E0"
                                BorderStyle="Solid" BorderWidth="1px" Width="110px" CssClass="admin-form-input"></asp:TextBox>
                        </div>
                        <div class="admin-form-field">
                            <label class="admin-form-label" for="<%= Tsattitude.ClientID %>">表现</label>
                            <asp:TextBox ID="Tsattitude" runat="server" BackColor="White" BorderColor="Gainsboro"
                                BorderStyle="Solid" BorderWidth="1px" Width="110px" ReadOnly="True" ToolTip="表现不可修改！" CssClass="admin-form-input admin-form-readonly">0</asp:TextBox>
                        </div>
                        <div class="admin-form-field">
                            <label class="admin-form-label" for="<%= Tsscore.ClientID %>">成绩</label>
                            <asp:TextBox ID="Tsscore" runat="server" BackColor="White" BorderColor="Gainsboro"
                                BorderStyle="Solid" BorderWidth="1px" Width="110px" ReadOnly="True" ToolTip="成绩不可修改！" CssClass="admin-form-input admin-form-readonly">0</asp:TextBox>
                        </div>
                        <div class="admin-form-field">
                            <label class="admin-form-label" for="<%= Tsphone.ClientID %>">电话</label>
                            <asp:TextBox ID="Tsphone" runat="server" BackColor="Cornsilk" BorderColor="#E0E0E0"
                                BorderStyle="Solid" BorderWidth="1px" Width="110px" CssClass="admin-form-input"></asp:TextBox>
                        </div>
                        <div class="admin-form-field admin-form-field-wide">
                            <label class="admin-form-label" for="<%= Tsaddress.ClientID %>">地址</label>
                            <asp:TextBox ID="Tsaddress" runat="server" BackColor="Cornsilk" BorderColor="#E0E0E0"
                                BorderStyle="Solid" BorderWidth="1px" Width="508px" CssClass="admin-form-input"></asp:TextBox>
                        </div>
                    </div>
                </section>

                <section class="admin-form-feedback">
                    <asp:Label ID="Labelmsg" runat="server"></asp:Label>
                </section>

                <section class="admin-form-actions">
                    <div class="admin-form-action-row">
                        <asp:Button ID="Btnadd" runat="server" OnClick="Btnadd_Click" Text="添加学生" CssClass="admin-form-btn admin-form-btn--primary" />
                    </div>
                </section>
            </div>
        </div>
    </form>
</body>
</html>
