<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master"  StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="topicshow.aspx.cs" Inherits="Teacher_topicshow" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <script src="../markdown/lib/marked.min.js"></script>
    <link rel="stylesheet" href="../js/vendors/reveal/dist/reveal.css" />
    <link rel="stylesheet" href="../js/vendors/reveal/dist/theme/white.css" />
    <link rel="stylesheet" href="../js/vendors/highlight/github.min.css" />
    <script src="../webform/highlight.min.js"></script>
    <link href="../App_Themes/Teacher/content-show-markdown.css" rel="stylesheet" />
    <link href="../App_Themes/Teacher/admin-form.css" rel="stylesheet" />
    <style type="text/css">
        .topicshow-page {
            --admin-form-page-bg: linear-gradient(180deg, #f8fafc 0%, #eef2ff 100%);
            --admin-form-hero-bg: linear-gradient(135deg, #7c3aed 0%, #8b5cf6 55%, #06b6d4 100%);
            --admin-form-hero-shadow: 0 22px 45px -28px rgba(124, 58, 237, 0.72);
            --admin-form-primary-bg: #7c3aed;
            --admin-form-primary-hover: #6d28d9;
            --admin-form-primary-shadow: 0 14px 24px -18px rgba(124, 58, 237, 0.85);
            --admin-form-secondary-border: #c4b5fd;
            --admin-form-secondary-bg: #f5f3ff;
            --admin-form-secondary-hover: #ede9fe;
            --admin-form-secondary-fg: #6d28d9;
        }

        .topicshow-page .topicshow-content {
            line-height: 1.8;
            word-break: break-word;
        }
    </style>
    <div class="admin-form-page topicshow-page">
        <div class="admin-form-shell">
            <section class="admin-form-hero">
                <div class="admin-form-hero-content">
                    <div class="admin-form-eyebrow">Topic Discussion</div>
                    <h1 class="admin-form-title">主题讨论详情</h1>
                    <p class="admin-form-subtitle">预览当前讨论主题、发布时间、状态和正文内容。</p>
                </div>
            </section>

            <section class="admin-form-panel">
                <div class="admin-form-toolbar">
                    <div>
                        <h2 class="admin-form-section-title">主题预览</h2>
                        <p class="admin-form-section-desc">当前主题讨论的名称、日期和状态如下。</p>
                    </div>
                    <div class="admin-form-action-row">
                        <asp:Button ID="Btnclock" runat="server" Text="讨论状态"
                            OnClick="Btnclock_Click" CssClass="admin-form-btn admin-form-btn--secondary" />
                        <asp:Button ID="BtnEdit" runat="server" Text="编辑内容" ToolTip="点击修改"
                            OnClick="BtnEdit_Click" CssClass="admin-form-btn admin-form-btn--primary" />
                    </div>
                </div>
                <asp:Label ID="Labeltid"  runat="server" Visible="false" ></asp:Label>
                <div class="admin-form-kv">
                    <div class="admin-form-kv-item">
                        <span class="admin-form-kv-label">主题讨论名称</span>
                        <span class="admin-form-kv-value"><asp:Label ID="LabelTtitle" runat="server"></asp:Label></span>
                    </div>
                    <div class="admin-form-kv-item">
                        <span class="admin-form-kv-label">日期</span>
                        <span class="admin-form-kv-value"><asp:Label ID="LabelTdate" runat="server"></asp:Label></span>
                    </div>
                    <div class="admin-form-kv-item">
                        <span class="admin-form-kv-label">学案编号</span>
                        <span class="admin-form-kv-value"><asp:Label ID="LabelMcid" runat="server"></asp:Label></span>
                    </div>
                </div>
            </section>

            <section class="admin-form-panel">
                <h2 class="admin-form-section-title">主题内容</h2>
                <p class="admin-form-section-desc">以下为当前讨论主题正文内容。</p>
                <div id="Tcontent" class="topicshow-content" runat="server"></div>
            </section>

            <section class="admin-form-actions">
                <div class="admin-form-action-row">
                    <asp:Button ID="Btnreturn" runat="server" Text="返回学案" OnClick="Btnreturn_Click" CssClass="admin-form-btn admin-form-btn--secondary" />
                </div>
            </section>
        </div>
    </div>
    <script type="text/javascript">
        window.__contentShowMarkdown = {
            contentId: '<%= Tcontent.ClientID %>'
        };
    </script>
    <script type="text/javascript" src="../js/content-show-markdown.js"></script>
</asp:Content>
