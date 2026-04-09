<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master"  StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="pixelshow.aspx.cs" Inherits="Teacher_pixelshow" ResponseEncoding="utf-8" %>

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
            --admin-form-hero-bg: linear-gradient(135deg, #1d4ed8 0%, #2563eb 55%, #06b6d4 100%);
            --admin-form-hero-shadow: 0 22px 45px -28px rgba(37, 99, 235, 0.72);
            --admin-form-primary-bg: #2563eb;
            --admin-form-primary-hover: #1d4ed8;
            --admin-form-primary-shadow: 0 14px 24px -18px rgba(37, 99, 235, 0.85);
            --admin-form-secondary-border: #bfdbfe;
            --admin-form-secondary-bg: #eff6ff;
            --admin-form-secondary-hover: #dbeafe;
            --admin-form-secondary-fg: #1d4ed8;
        }

        .mission-show-content {
            line-height: 1.8;
            word-break: break-word;
        }

        .mission-show-page .admin-form-chip {
            display: inline-flex;
            align-items: center;
            gap: 0.5rem;
            min-height: 2rem;
            padding: 0.2rem 0.8rem;
            border-radius: 9999px;
            font-size: 0.82rem;
            font-weight: 700;
        }

        .mission-show-page .admin-form-chip-icon {
            width: 1.1rem;
            height: 1.1rem;
            object-fit: contain;
            flex-shrink: 0;
        }

        .mission-show-page .admin-form-kv-value img {
            margin-right: 0.35rem;
            vertical-align: middle;
        }

        .mission-show-page .mission-show-route {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
            gap: 1rem;
        }

        .mission-show-page .mission-show-route-item {
            padding: 0.95rem 1rem;
            border: 1px solid #e2e8f0;
            border-radius: 1rem;
            background: #f8fafc;
        }

        .mission-show-page .mission-show-route-item strong {
            display: block;
            margin-bottom: 0.35rem;
            color: #0f172a;
        }

        .mission-show-page .mission-show-path {
            color: #2563eb;
            font-weight: 700;
            word-break: break-all;
        }

        .mission-show-page .mission-show-example {
            margin-top: 1rem;
            padding: 1rem 1.05rem;
            border: 1px solid #dbeafe;
            border-radius: 1rem;
            background: linear-gradient(135deg, #eff6ff 0%, #f8fbff 100%);
            color: #334155;
            line-height: 1.75;
        }

        .mission-show-page .mission-show-example strong {
            display: block;
            margin-bottom: 0.35rem;
            color: #0f172a;
        }
    </style>
    <div class="admin-form-page mission-show-page">
        <div class="admin-form-shell">
            <section class="admin-form-hero">
                <div class="admin-form-hero-content">
                    <div class="admin-form-eyebrow">Custom Activity Preview</div>
                    <h1 class="admin-form-title"><asp:Label ID="LabelMtitle" runat="server"></asp:Label></h1>
                    <p class="admin-form-subtitle">预览当前在线活动的基础属性、开展入口和说明内容，便于教师确认学生将进入的实际工具页面。</p>
                </div>
            </section>

            <section class="admin-form-panel">
                <div class="admin-form-toolbar">
                    <div>
                        <h2 class="admin-form-section-title">活动预览</h2>
                        <p class="admin-form-section-desc">这里显示当前在线活动的基础属性。</p>
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
                        <span class="admin-form-kv-label">作品类型</span>
                        <span class="admin-form-kv-value"><asp:Image ID="ImageType" runat="server" /> <asp:Label ID="LabelMfiletype" runat="server"></asp:Label></span>
                    </div>
                    <div class="admin-form-kv-item">
                        <span class="admin-form-kv-label">发布状态</span>
                        <span class="admin-form-kv-value"><asp:CheckBox ID="CheckPublish" runat="server" Text="是否发布" Enabled="False" /></span>
                    </div>
                    <div class="admin-form-kv-item">
                        <span class="admin-form-kv-label">评价标准</span>
                        <span class="admin-form-kv-value"><asp:HyperLink ID="HLMgid" runat="server" CssClass="admin-form-link">评价标准</asp:HyperLink></span>
                    </div>
                    <div class="admin-form-kv-item">
                        <span class="admin-form-kv-label">活动类型</span>
                        <span class="admin-form-kv-value">
                            <span class="admin-form-chip" id="ActivityChip" runat="server">
                                <asp:Image ID="ImageActivityIcon" runat="server" CssClass="admin-form-chip-icon" AlternateText="" />
                                <asp:Label ID="LabelActivityName" runat="server"></asp:Label>
                            </span>
                        </span>
                    </div>
                </div>
            </section>

            <section class="admin-form-panel">
                <h2 class="admin-form-section-title">开展方式</h2>
                <p class="admin-form-section-desc">本页展示教师侧的预览结果。学生真正开展任务时，会按活动类型跳转到对应页面。</p>
                <div class="mission-show-route">
                    <div class="mission-show-route-item">
                        <strong>教师端说明</strong>
                        <asp:Label ID="LabelActivityDescription" runat="server"></asp:Label>
                    </div>
                    <div class="mission-show-route-item">
                        <strong>学生端入口</strong>
                        <span class="mission-show-path"><asp:Label ID="LabelStudentEntry" runat="server"></asp:Label></span>
                    </div>
                    <div class="mission-show-route-item">
                        <strong>编辑重点</strong>
                        <asp:Label ID="LabelEditFocus" runat="server"></asp:Label>
                    </div>
                </div>
                <asp:Panel ID="PanelExampleSummary" runat="server" CssClass="mission-show-example" Visible="False">
                    <strong>专属配置</strong>
                    <asp:Label ID="LabelExampleSummary" runat="server"></asp:Label>
                </asp:Panel>
            </section>

            <section class="admin-form-panel">
                <h2 class="admin-form-section-title">活动内容</h2>
                <p class="admin-form-section-desc">以下为当前活动向学生展示的说明内容。</p>
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
