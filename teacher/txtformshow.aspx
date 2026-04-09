<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master"  StylesheetTheme="Teacher"  AutoEventWireup="true" CodeFile="txtformshow.aspx.cs" Inherits="Teacher_txtformshow" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <script src="../markdown/lib/marked.min.js"></script>
    <link rel="stylesheet" href="../js/vendors/reveal/dist/reveal.css" />
    <link rel="stylesheet" href="../js/vendors/reveal/dist/theme/white.css" />
    <link rel="stylesheet" href="../js/vendors/highlight/github.min.css" />
    <script src="../webform/highlight.min.js"></script>
    <link href="../App_Themes/Teacher/content-show-markdown.css" rel="stylesheet" />
    <link href="../App_Themes/Teacher/admin-form.css" rel="stylesheet" />
    <style type="text/css">
        .mission-show-page {
            --admin-form-page-bg: linear-gradient(180deg, #f8fafc 0%, #eef2ff 100%);
            --admin-form-hero-bg: linear-gradient(135deg, #7c2d12 0%, #c2410c 55%, #fb923c 100%);
            --admin-form-hero-shadow: 0 22px 45px -28px rgba(194, 65, 12, 0.72);
            --admin-form-primary-bg: #c2410c;
            --admin-form-primary-hover: #9a3412;
            --admin-form-primary-shadow: 0 14px 24px -18px rgba(194, 65, 12, 0.85);
            --admin-form-secondary-border: #fdba74;
            --admin-form-secondary-bg: #fff7ed;
            --admin-form-secondary-hover: #ffedd5;
            --admin-form-secondary-fg: #9a3412;
        }

        .mission-show-content {
            line-height: 1.8;
            word-break: break-word;
        }
    </style>
    <div class="admin-form-page mission-show-page">
        <div class="admin-form-shell">
            <section class="admin-form-hero">
                <div class="admin-form-hero-content">
                    <div class="admin-form-eyebrow">Text Form</div>
                    <h1 class="admin-form-title"><asp:Label ID="LabelMtitle" runat="server"></asp:Label></h1>
                    <p class="admin-form-subtitle">预览当前学案表单内容、发布时间和协同状态。</p>
                </div>
            </section>

            <section class="admin-form-panel">
                <div class="admin-form-toolbar">
                    <div>
                        <h2 class="admin-form-section-title">表单预览</h2>
                        <p class="admin-form-section-desc">这里显示当前学案表单的基础属性。</p>
                    </div>
                    <div class="admin-form-action-row">
                        <asp:Button ID="BtnEdit" runat="server" Text="编辑内容" ToolTip="点击修改"
                            OnClick="BtnEdit_Click" CssClass="admin-form-btn admin-form-btn--primary" />
                        <asp:Button ID="BtnReturnSmall" runat="server" Text="返回学案" ToolTip="返回"
                            OnClick="BtnReturnSmall_Click" CssClass="admin-form-btn admin-form-btn--secondary" />
                    </div>
                </div>
                <div class="admin-form-kv">
                    <div class="admin-form-kv-item">
                        <span class="admin-form-kv-label">日期</span>
                        <span class="admin-form-kv-value"><asp:Label ID="LabelMdate" runat="server"></asp:Label></span>
                    </div>
                    <div class="admin-form-kv-item">
                        <span class="admin-form-kv-label">发布状态</span>
                        <span class="admin-form-kv-value"><asp:CheckBox ID="CheckPublish" runat="server" Text="是否发布" Enabled="False" /></span>
                    </div>
                    <div class="admin-form-kv-item">
                        <span class="admin-form-kv-label">协同状态</span>
                        <span class="admin-form-kv-value"><asp:CheckBox ID="CheckCollabo" runat="server" Text="是否协同" Enabled="False" /></span>
                    </div>
                </div>
            </section>

            <section class="admin-form-panel">
                <h2 class="admin-form-section-title">表单内容</h2>
                <p class="admin-form-section-desc">以下为当前学案表单正文内容。</p>
                <div id="Mcontent" class="mission-show-content" runat="server"></div>
            </section>

            <section class="admin-form-actions">
                <div class="admin-form-action-row">
                    <asp:LinkButton ID="LinkBtn" runat="server" OnClick="LinkBtn_Click" CssClass="admin-form-btn admin-form-btn--secondary">返回学案</asp:LinkButton>
                </div>
            </section>
        </div>
    </div>
    <script type="text/javascript">
        window.__contentShowMarkdown = {
            contentId: '<%= Mcontent.ClientID %>'
        };
    </script>
    <script type="text/javascript" src="../js/content-show-markdown.js"></script>
</asp:Content>
