<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master"  StylesheetTheme="Teacher" Validaterequest="false" AutoEventWireup="true" CodeFile="programshow.aspx.cs" Inherits="Teacher_programshow" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <script src="../markdown/lib/marked.min.js"></script>
    <link rel="stylesheet" href="../js/vendors/reveal/dist/reveal.css" />
    <link rel="stylesheet" href="../js/vendors/reveal/dist/theme/white.css" />
    <link rel="stylesheet" href="../js/vendors/highlight/github.min.css" />
    <script src="../webform/highlight.min.js"></script>
    <link href="../App_Themes/Teacher/content-show-markdown.css" rel="stylesheet" />
    <link href="../App_Themes/Teacher/admin-form.css" rel="stylesheet" />
    <link href="../App_Themes/Teacher/course-content-add.css" rel="stylesheet" />
    <style type="text/css">
        .mission-show-page {
            --admin-form-page-bg: linear-gradient(180deg, #f8fafc 0%, #eef2ff 100%);
            --admin-form-hero-bg: linear-gradient(135deg, #1e293b 0%, #312e81 55%, #4f46e5 100%);
            --admin-form-hero-shadow: 0 22px 45px -28px rgba(79, 70, 229, 0.72);
            --admin-form-primary-bg: #4f46e5;
            --admin-form-primary-hover: #4338ca;
            --admin-form-primary-shadow: 0 14px 24px -18px rgba(79, 70, 229, 0.85);
            --admin-form-secondary-border: #c7d2fe;
            --admin-form-secondary-bg: #eef2ff;
            --admin-form-secondary-hover: #e0e7ff;
            --admin-form-secondary-fg: #3730a3;
        }

        .mission-show-content {
            line-height: 1.8;
            word-break: break-word;
        }

        .mission-show-actions {
            display: flex;
            flex-wrap: wrap;
            gap: 0.75rem;
            align-items: center;
        }
    </style>
    <div class="admin-form-page mission-show-page">
        <div class="admin-form-shell">
            <section class="admin-form-hero">
                <div class="admin-form-hero-content">
                    <div class="admin-form-eyebrow">Program Mission</div>
                    <h1 class="admin-form-title"><asp:Label ID="LabelMtitle" runat="server"></asp:Label></h1>
                    <p class="admin-form-subtitle">预览当前编程活动内容、作品继承状态和示例资源。</p>
                </div>
            </section>

            <section class="admin-form-panel">
                <div class="admin-form-toolbar">
                    <div>
                        <h2 class="admin-form-section-title">主题预览</h2>
                        <p class="admin-form-section-desc">这里显示发布状态、作品继承、实例下载和评价标准入口。</p>
                    </div>
                    <div class="admin-form-action-row">
                        <asp:Button ID="BtnEdit" runat="server" Text="编辑内容" ToolTip="点击修改" OnClick="BtnEdit_Click" CssClass="admin-form-btn admin-form-btn--primary" />
                        <asp:Button ID="BtnReturnSmall" runat="server" Text="返回学案" ToolTip="返回" OnClick="BtnReturnSmall_Click" CssClass="admin-form-btn admin-form-btn--secondary" />
                    </div>
                </div>
                <div class="admin-form-kv">
                    <div class="admin-form-kv-item">
                        <span class="admin-form-kv-label">日期</span>
                        <span class="admin-form-kv-value"><asp:Label ID="LabelMdate" runat="server"></asp:Label></span>
                    </div>
                    <div class="admin-form-kv-item">
                        <span class="admin-form-kv-label">作品类型</span>
                        <span class="admin-form-kv-value"><asp:Image ID="ImageType" runat="server" /> <asp:Label ID="LabelMfiletype" runat="server"></asp:Label></span>
                    </div>
                </div>
                <div class="mission-show-actions" style="margin-top:1rem;">
                    <asp:HyperLink ID="Hlexample" runat="server" CssClass="admin-form-link">实例下载</asp:HyperLink>
                    <asp:HyperLink ID="HLMgid" runat="server" CssClass="admin-form-link">评价标准</asp:HyperLink>
                    <label class="content-add-checks"><asp:CheckBox ID="CheckPublish" runat="server" Text="是否发布" Enabled="False" /></label>
                    <label class="content-add-checks"><asp:CheckBox ID="CheckMicoWorld" runat="server" Text="作品继承" ToolTip="加载上一节的编程作品，适合项目学习" Enabled="False" /></label>
                </div>
            </section>

            <section class="admin-form-panel">
                <h2 class="admin-form-section-title">活动说明</h2>
                <p class="admin-form-section-desc">以下为当前编程活动说明内容。</p>
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
