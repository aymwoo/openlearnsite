<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" AutoEventWireup="true" CodeFile="excelshow.aspx.cs" Inherits="teacher_excelshow" ResponseEncoding="utf-8" %>

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
            --admin-form-hero-bg: linear-gradient(135deg, #0f766e 0%, #059669 55%, #14b8a6 100%);
            --admin-form-hero-shadow: 0 22px 45px -28px rgba(5, 150, 105, 0.72);
            --admin-form-primary-bg: #059669;
            --admin-form-primary-hover: #047857;
            --admin-form-primary-shadow: 0 14px 24px -18px rgba(5, 150, 105, 0.85);
            --admin-form-secondary-border: #99f6e4;
            --admin-form-secondary-bg: #ecfeff;
            --admin-form-secondary-hover: #ccfbf1;
            --admin-form-secondary-fg: #0f766e;
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
                    <div class="admin-form-eyebrow">Spreadsheet Activity</div>
                    <h1 class="admin-form-title"><asp:Label ID="LabelMtitle" runat="server"></asp:Label></h1>
                    <p class="admin-form-subtitle">预览当前表格处理活动内容、示例资源与协作表格入口。</p>
                </div>
            </section>

            <section class="admin-form-panel">
                <div class="admin-form-toolbar">
                    <div>
                        <h2 class="admin-form-section-title">活动预览</h2>
                        <p class="admin-form-section-desc">这里显示当前表格处理活动的基础属性和相关链接。</p>
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
                        <span class="admin-form-kv-label">示例资源</span>
                        <span class="admin-form-kv-value"><asp:HyperLink ID="Hlexample" runat="server" CssClass="admin-form-link"></asp:HyperLink></span>
                    </div>
                    <div class="admin-form-kv-item">
                        <span class="admin-form-kv-label">评价标准</span>
                        <span class="admin-form-kv-value"><asp:HyperLink ID="HLMgid" runat="server" CssClass="admin-form-link">评价标准</asp:HyperLink></span>
                    </div>
                    <div class="admin-form-kv-item">
                        <span class="admin-form-kv-label">协作表格</span>
                        <span class="admin-form-kv-value"><asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/luckysheetbottle/index.html" Target="_blank" ToolTip="协作表格静态网址" CssClass="admin-form-link">协作表格</asp:HyperLink></span>
                    </div>
                </div>
            </section>

            <section class="admin-form-panel">
                <h2 class="admin-form-section-title">活动内容</h2>
                <p class="admin-form-section-desc">以下为当前表格处理活动说明内容。</p>
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
