<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master"  StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="kitymindshow.aspx.cs" Inherits="teacher_kitymindshow" ResponseEncoding="utf-8" %>

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
        .mindmap-show-page {
            --admin-form-page-bg: linear-gradient(180deg, #f8fafc 0%, #f5f3ff 100%);
            --admin-form-hero-bg: linear-gradient(135deg, #5b21b6 0%, #7c3aed 55%, #a78bfa 100%);
            --admin-form-hero-shadow: 0 22px 45px -28px rgba(124, 58, 237, 0.72);
            --admin-form-primary-bg: #7c3aed;
            --admin-form-primary-hover: #6d28d9;
            --admin-form-primary-shadow: 0 14px 24px -18px rgba(124, 58, 237, 0.85);
            --admin-form-secondary-border: #ddd6fe;
            --admin-form-secondary-bg: #f5f3ff;
            --admin-form-secondary-hover: #ede9fe;
            --admin-form-secondary-fg: #6d28d9;
        }

        .mindmap-show-content {
            line-height: 1.8;
            word-break: break-word;
        }

        .mindmap-show-actions {
            display: flex;
            flex-wrap: wrap;
            gap: 0.75rem;
            align-items: center;
        }
    </style>

    <div class="admin-form-page mindmap-show-page">
        <div class="admin-form-shell">
            <section class="admin-form-hero">
                <div class="admin-form-hero-content">
                    <div class="admin-form-eyebrow">Mind Map Mission</div>
                    <h1 class="admin-form-title"><asp:Label ID="LabelMtitle" runat="server"></asp:Label></h1>
                    <p class="admin-form-subtitle">预览当前思维导图活动内容、实例文件和发布状态。</p>
                </div>
            </section>

            <section class="admin-form-panel">
                <div class="admin-form-toolbar">
                    <div>
                        <h2 class="admin-form-section-title">主题预览</h2>
                        <p class="admin-form-section-desc">这里显示发布状态、实例文件入口和评价标准入口。</p>
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
                <div class="mindmap-show-actions" style="margin-top:1rem;">
                    <label class="content-add-checks"><asp:CheckBox ID="CheckPublish" runat="server" Text="是否发布" Enabled="False" /></label>
                    <asp:HyperLink ID="Hlexample" runat="server" CssClass="admin-form-link">实例文件</asp:HyperLink>
                    <asp:HyperLink ID="HLMgid" runat="server" CssClass="admin-form-link">评价标准</asp:HyperLink>
                </div>
            </section>

            <section class="admin-form-panel">
                <h2 class="admin-form-section-title">导图说明</h2>
                <p class="admin-form-section-desc">以下为当前思维导图活动说明内容。</p>
                <div id="Mcontent" class="mindmap-show-content" runat="server"></div>
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
