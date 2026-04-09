<%@ Page Title="" Language="C#" StylesheetTheme="Teacher" ValidateRequest="false" AutoEventWireup="true" CodeFile="attitude.aspx.cs" Inherits="Teacher_attitude" ResponseEncoding="utf-8" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <meta charset="utf-8" />
<title></title>
    <link href="../App_Themes/Teacher/admin-form.css" rel="stylesheet" />
    <style type="text/css">
        .popup-page {
            --admin-form-page-bg: linear-gradient(180deg, #f8fafc 0%, #eef6ff 100%);
            --admin-form-hero-bg: linear-gradient(135deg, #1d4ed8 0%, #2563eb 55%, #38bdf8 100%);
            --admin-form-hero-shadow: 0 22px 45px -28px rgba(37, 99, 235, 0.72);
            --admin-form-primary-bg: #2563eb;
            --admin-form-primary-hover: #1d4ed8;
            --admin-form-primary-shadow: 0 14px 24px -18px rgba(37, 99, 235, 0.85);
            --admin-form-focus: #3b82f6;
            --admin-form-focus-ring: rgba(59, 130, 246, 0.14);
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="admin-form-page popup-page">
        <div class="admin-form-shell">
            <section class="admin-form-hero">
                <div class="admin-form-hero-content">
                    <div class="admin-form-eyebrow">Attitude Score</div>
                    <h1 class="admin-form-title">课堂评分</h1>
                    <p class="admin-form-subtitle">对 <asp:Label ID="Labelname" runat="server" Font-Bold="True"></asp:Label> 同学进行课堂表现评分。</p>
                </div>
            </section>
            <section class="admin-form-panel">
                <h2 class="admin-form-section-title">评分与评语</h2>
                <p class="admin-form-section-desc">可先选择分值与快捷评语，再补充自定义课堂评语。</p>
                <div class="admin-form-grid">
                    <div class="admin-form-field">
                        <label class="admin-form-label" for="<%= DDLatt.ClientID %>">评分</label>
                        <asp:DropDownList ID="DDLatt" runat="server" Font-Size="9pt" CssClass="admin-form-select">
                            <asp:ListItem>5</asp:ListItem>
                            <asp:ListItem>4</asp:ListItem>
                            <asp:ListItem>3</asp:ListItem>
                            <asp:ListItem>2</asp:ListItem>
                            <asp:ListItem>1</asp:ListItem>
                            <asp:ListItem>0</asp:ListItem>
                            <asp:ListItem>-1</asp:ListItem>
                            <asp:ListItem>-2</asp:ListItem>
                            <asp:ListItem>-3</asp:ListItem>
                            <asp:ListItem>-4</asp:ListItem>
                            <asp:ListItem>-5</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="admin-form-field admin-form-field-wide">
                        <label class="admin-form-label">快捷评语</label>
                        <asp:RadioButtonList ID="RBLattitude" runat="server"
                            Height="120px" onselectedindexchanged="RBLattitude_SelectedIndexChanged"
                            Width="100%" RepeatColumns="2" AutoPostBack="True">
                            <asp:ListItem Value="2" >乐于助人</asp:ListItem>
                            <asp:ListItem Value="1">表现优秀</asp:ListItem>
                            <asp:ListItem Value="-1">有开小差</asp:ListItem>
                            <asp:ListItem Value="-2">乱扔垃圾</asp:ListItem>
                            <asp:ListItem Value="-3">上课迟到</asp:ListItem>
                            <asp:ListItem Value="-4">损坏公物</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <div class="admin-form-field admin-form-field-wide">
                        <label class="admin-form-label" for="<%= TextBox2.ClientID %>">自定义课堂评语</label>
                        <asp:TextBox ID="TextBox2" runat="server" BackColor="#FFE7CE" Height="60px" ToolTip="填写好自定义评语后，请手动上面您的评分！"
                            Width="240px" TextMode="MultiLine" CssClass="admin-form-input admin-form-textarea"></asp:TextBox>
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
