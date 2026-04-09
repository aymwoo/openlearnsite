<%@ Page Title="" Language="C#" StylesheetTheme="Teacher" ValidateRequest="false"  AutoEventWireup="true" CodeFile="attitudegroup.aspx.cs" Inherits="Teacher_attitudegroup" ResponseEncoding="utf-8" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <meta charset="utf-8" />
<title></title>
    <link href="../App_Themes/Teacher/admin-form.css" rel="stylesheet" />
    <style type="text/css">
        .popup-page {
            --admin-form-page-bg: linear-gradient(180deg, #f8fafc 0%, #eef6ff 100%);
            --admin-form-hero-bg: linear-gradient(135deg, #0f766e 0%, #0f9b8e 55%, #22c55e 100%);
            --admin-form-hero-shadow: 0 22px 45px -28px rgba(15, 118, 110, 0.72);
            --admin-form-primary-bg: #0f766e;
            --admin-form-primary-hover: #0d675f;
            --admin-form-primary-shadow: 0 14px 24px -18px rgba(15, 118, 110, 0.85);
            --admin-form-focus: #14b8a6;
            --admin-form-focus-ring: rgba(20, 184, 166, 0.14);
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="admin-form-page popup-page">
        <div class="admin-form-shell">
            <section class="admin-form-hero">
                <div class="admin-form-hero-content">
                    <div class="admin-form-eyebrow">Group Score</div>
                    <h1 class="admin-form-title">小组评价</h1>
                    <p class="admin-form-subtitle">对 <asp:Label ID="Labelname" runat="server" Font-Bold="True"></asp:Label> 同学填写小组评语与评分。</p>
                </div>
            </section>
            <section class="admin-form-panel">
                <h2 class="admin-form-section-title">评分与评语</h2>
                <p class="admin-form-section-desc">填写小组评价内容，并选择对应分值后提交。</p>
                <div class="admin-form-grid">
                    <div class="admin-form-field">
                        <label class="admin-form-label" for="<%= DDLatt.ClientID %>">评分</label>
                        <asp:DropDownList ID="DDLatt" runat="server" Font-Size="9pt" CssClass="admin-form-select">
                            <asp:ListItem>10</asp:ListItem>
                            <asp:ListItem>9</asp:ListItem>
                            <asp:ListItem>8</asp:ListItem>
                            <asp:ListItem>7</asp:ListItem>
                            <asp:ListItem>6</asp:ListItem>
                            <asp:ListItem Selected="True">5</asp:ListItem>
                            <asp:ListItem>4</asp:ListItem>
                            <asp:ListItem>3</asp:ListItem>
                            <asp:ListItem>2</asp:ListItem>
                            <asp:ListItem>1</asp:ListItem>
                            <asp:ListItem>-1</asp:ListItem>
                            <asp:ListItem>-2</asp:ListItem>
                            <asp:ListItem>-3</asp:ListItem>
                            <asp:ListItem>-4</asp:ListItem>
                            <asp:ListItem>-5</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="admin-form-field admin-form-field-wide">
                        <label class="admin-form-label" for="<%= TextBox2.ClientID %>">小组评语</label>
                        <asp:TextBox ID="TextBox2" runat="server" BackColor="#FFE7CE" Height="60px"
                            TextMode="MultiLine" ToolTip="填写好自定义评语后，请手动上面您的评分！"
                            Width="240px" CssClass="admin-form-input admin-form-textarea"></asp:TextBox>
                    </div>
                </div>
                <div style="margin-top:1rem;"><asp:Label ID="Labelmsg" runat="server"></asp:Label></div>
            </section>
            <section class="admin-form-actions">
                <div class="admin-form-action-row">
                    <asp:Button ID="Btnattitude" runat="server" Text="确定"
                        onclick="Btnattitude_Click" CssClass="admin-form-btn admin-form-btn--primary" />
                </div>
            </section>
        </div>
    </div>
    </form>
</body>
</html>
