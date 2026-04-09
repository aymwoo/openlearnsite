<%@ Page Language="C#"  StylesheetTheme="Teacher"  AutoEventWireup="true" CodeFile="workpackage.aspx.cs" Inherits="Teacher_workpackage" ResponseEncoding="utf-8" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <meta charset="utf-8" />
<title></title>
    <link href="../App_Themes/Teacher/admin-form.css" rel="stylesheet" />
    <style type="text/css">
        .workpackage-page {
            --admin-form-page-bg: linear-gradient(180deg, #f8fafc 0%, #eef2ff 100%);
            --admin-form-hero-bg: linear-gradient(135deg, #1e1b4b 0%, #4338ca 55%, #6366f1 100%);
            --admin-form-hero-shadow: 0 22px 45px -28px rgba(79, 70, 229, 0.75);
            --admin-form-primary-bg: #4f46e5;
            --admin-form-primary-hover: #4338ca;
            --admin-form-primary-shadow: 0 14px 24px -18px rgba(79, 70, 229, 0.85);
            --admin-form-secondary-border: #c7d2fe;
            --admin-form-secondary-bg: #eef2ff;
            --admin-form-secondary-hover: #e0e7ff;
            --admin-form-secondary-fg: #3730a3;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="admin-form-page workpackage-page">
        <div class="admin-form-shell">
            <section class="admin-form-hero">
                <div class="admin-form-hero-content">
                    <div class="admin-form-eyebrow">Work Package</div>
                    <h1 class="admin-form-title">班级作品打包</h1>
                    <p class="admin-form-subtitle">按年级、班级和学案选择作品目录，生成并下载当前学案作品包。</p>
                </div>
            </section>

            <section class="admin-form-panel">
                <h2 class="admin-form-section-title">打包条件</h2>
                <p class="admin-form-section-desc">切换年级、班级和学案后，可重新生成作品压缩包。</p>
                <div class="admin-form-grid">
                    <div class="admin-form-field">
                        <label class="admin-form-label" for="<%= DDLgrade.ClientID %>">年级</label>
                        <asp:DropDownList ID="DDLgrade" runat="server" Font-Size="9pt"
                            Width="50px" EnableTheming="True" AutoPostBack="True"
                            onselectedindexchanged="DDLgrade_SelectedIndexChanged" CssClass="admin-form-select">
                        </asp:DropDownList>
                    </div>
                    <div class="admin-form-field">
                        <label class="admin-form-label" for="<%= DDLclass.ClientID %>">班级</label>
                        <asp:DropDownList ID="DDLclass" runat="server" Font-Size="9pt"
                            Width="50px" EnableTheming="True" AutoPostBack="True"
                            onselectedindexchanged="DDLclass_SelectedIndexChanged" CssClass="admin-form-select">
                        </asp:DropDownList>
                    </div>
                    <div class="admin-form-field admin-form-field-wide">
                        <label class="admin-form-label" for="<%= DDLCid.ClientID %>">学案名称</label>
                        <asp:DropDownList ID="DDLCid" runat="server" Font-Names="Arial"
                          Font-Size="9pt" AutoPostBack="True"
                          onselectedindexchanged="DDLCid_SelectedIndexChanged" CssClass="admin-form-select">
                      </asp:DropDownList>
                    </div>
                </div>
            </section>

            <section class="admin-form-actions">
                <div class="admin-form-action-row">
                    <asp:Button ID="Button1" runat="server" Text="作品打包" onclick="Button1_Click"
                        CssClass="admin-form-btn admin-form-btn--primary" />
                </div>
            </section>

            <section class="admin-form-panel">
                <h2 class="admin-form-section-title">打包结果</h2>
                <p class="admin-form-section-desc">打包完成后，可从下方直接下载生成的压缩包。</p>
                <asp:Label ID="Labelyear" runat="server" Visible="False"></asp:Label>
                <div class="admin-form-kv">
                    <div class="admin-form-kv-item">
                        <span class="admin-form-kv-label">下载文件</span>
                        <span class="admin-form-kv-value"><asp:HyperLink ID="HyperLink1" runat="server" CssClass="admin-form-link">本学案作品包下载</asp:HyperLink></span>
                    </div>
                    <div class="admin-form-kv-item">
                        <span class="admin-form-kv-label">状态信息</span>
                        <span class="admin-form-kv-value"><asp:Label ID="Labelmsg" runat="server">打包时请耐心等待几秒！</asp:Label></span>
                    </div>
                </div>
            </section>
        </div>
    </div>
    </form>
</body>
</html>
