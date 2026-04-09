<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher" Validaterequest="false" AutoEventWireup="true" CodeFile="pythonshow.aspx.cs"  inherits="Teacher_pythonshow" ResponseEncoding="utf-8" %>

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
        .python-show-page {
            --admin-form-page-bg: linear-gradient(180deg, #f8fafc 0%, #eff6ff 100%);
            --admin-form-hero-bg: linear-gradient(135deg, #1d4ed8 0%, #2563eb 55%, #60a5fa 100%);
            --admin-form-hero-shadow: 0 22px 45px -28px rgba(37, 99, 235, 0.72);
            --admin-form-primary-bg: #2563eb;
            --admin-form-primary-hover: #1d4ed8;
            --admin-form-primary-shadow: 0 14px 24px -18px rgba(37, 99, 235, 0.85);
            --admin-form-secondary-border: #bfdbfe;
            --admin-form-secondary-bg: #eff6ff;
            --admin-form-secondary-hover: #dbeafe;
            --admin-form-secondary-fg: #1d4ed8;
        }

        .python-show-content {
            line-height: 1.8;
            word-break: break-word;
        }

        .python-show-actions {
            display: flex;
            flex-wrap: wrap;
            gap: 0.75rem;
            align-items: center;
        }
    </style>

    <div class="admin-form-page python-show-page">
        <div class="admin-form-shell">
            <section class="admin-form-hero">
                <div class="admin-form-hero-content">
                    <div class="admin-form-eyebrow">Python Mission</div>
                    <h1 class="admin-form-title"><asp:Label ID="LabelMtitle" runat="server"></asp:Label></h1>
                    <p class="admin-form-subtitle">预览当前 Python 编程活动内容、自动批改入口和示例资源。</p>
                </div>
            </section>

            <section class="admin-form-panel">
                <div class="admin-form-toolbar">
                    <div>
                        <h2 class="admin-form-section-title">运行预览</h2>
                        <p class="admin-form-section-desc">这里显示自动批改入口、发布状态、模式开关和示例文件链接。</p>
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
                <div class="python-show-actions" style="margin-top:1rem;">
                    <asp:Image ID="Imgauto" runat="server" />
                    <asp:HyperLink ID="HLauto" runat="server" CssClass="admin-form-link">自动批改</asp:HyperLink>
                    <asp:HyperLink ID="HlExample" runat="server" Target="_blank" CssClass="admin-form-link">编程实例</asp:HyperLink>
                    <asp:HyperLink ID="HLMgid" runat="server" CssClass="admin-form-link">评价标准</asp:HyperLink>
                    <label class="content-add-checks"><asp:CheckBox ID="CheckPublish" runat="server" Text="发布" Enabled="False" /></label>
                    <label class="content-add-checks"><asp:CheckBox ID="CheckBack" runat="server" Text="分步" Enabled="False" /></label>
                    <label class="content-add-checks"><asp:CheckBox ID="Checkhelp" runat="server" Text="绘图" Enabled="False" /></label>
                    <label class="content-add-checks"><asp:CheckBox ID="Checkblock" runat="server" Text="拼图" Enabled="False" /></label>
                    <label class="content-add-checks"><asp:CheckBox ID="Checkblockpy" runat="server" Text="积木" Enabled="False" /></label>
                </div>
            </section>

            <section class="admin-form-panel">
                <h2 class="admin-form-section-title">编程说明</h2>
                <p class="admin-form-section-desc">以下为当前 Python 编程活动说明内容。</p>
                <div id="Mcontent" class="python-show-content" runat="server"></div>
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
