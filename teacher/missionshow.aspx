<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher" Validaterequest="false"  AutoEventWireup="true" CodeFile="missionshow.aspx.cs" Inherits="Teacher_missionshow" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link href="../App_Themes/Teacher/admin-form.css" rel="stylesheet" />
    <link href="../App_Themes/Teacher/course-content-add.css" rel="stylesheet" />
    <link href="../App_Themes/Teacher/content-show-markdown.css" rel="stylesheet" />
    <link href="../kindeditor/plugins/code/prettify.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../js/vendors/reveal/dist/reveal.css" />
    <link rel="stylesheet" href="../js/vendors/reveal/dist/theme/white.css" />
    <link rel="stylesheet" href="../js/vendors/highlight/github.min.css" />
    <script src="../kindeditor/plugins/code/prettify.js" type="text/javascript"></script>
    <script src="../markdown/lib/marked.min.js"></script>
    <script src="../webform/highlight.min.js"></script>
    

    <div class="admin-form-page mission-show-page">
        <div class="admin-form-shell">
            <section class="admin-form-hero">
                <div class="admin-form-hero-content">
                    <div class="admin-form-eyebrow">Course Mission</div>
                    <h1 class="admin-form-title"><asp:Label ID="LabelMtitle" runat="server"></asp:Label></h1>
                    <p class="admin-form-subtitle">预览当前学案活动说明、提交状态、合作设置和评价标准。</p>
                </div>
            </section>

            <section class="admin-form-panel">
                <div class="admin-form-toolbar">
                    <div>
                        <h2 class="admin-form-section-title">活动预览</h2>
                        <p class="admin-form-section-desc">这里显示活动提交、上次作品、小组合作和发布状态。</p>
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
                </div>
                <div class="mission-show-actions" style="margin-top:1rem;">
                    <label class="content-add-checks"><asp:CheckBox ID="CkMupload" runat="server" Text="是否提交" Enabled="false" /></label>
                    <label class="content-add-checks"><asp:CheckBox ID="CheckMicoWorld" runat="server" Text="上次作品" Enabled="False" /></label>
                    <label class="content-add-checks"><asp:CheckBox ID="CheckPublish" runat="server" Text="是否发布" Enabled="False" /></label>
                    <label class="content-add-checks"><asp:CheckBox ID="CheckGroup" runat="server" Text="小组合作" Enabled="false" /></label>
                    <asp:HyperLink ID="HLMgid" runat="server" CssClass="admin-form-link">评价标准</asp:HyperLink>
                </div>
            </section>

            <section class="admin-form-panel">
                <h2 class="admin-form-section-title">活动内容</h2>
                <p class="admin-form-section-desc">以下为当前学案活动正文内容。</p>
                <div class="mission-show-toggle-grid">
                    <div class="mission-show-toggle-card">
                        <div class="mission-show-toggle-row">
                            <div class="mission-show-toggle-copy">
                                <div class="mission-show-toggle-title">Markdown 渲染</div>
                                <div class="mission-show-toggle-desc">开启后自动解析 Markdown、Mermaid 和代码高亮。</div>
                            </div>
                            <button type="button" id="markdownToggle" runat="server" class="mission-show-toggle-switch" aria-pressed="false"><span class="mission-show-toggle-knob"></span></button>
                        </div>
                        <div id="markdownToggleStatus" runat="server" class="mission-show-toggle-status">当前：关闭</div>
                    </div>
                    <div class="mission-show-toggle-card">
                        <div class="mission-show-toggle-row">
                            <div class="mission-show-toggle-copy">
                                <div class="mission-show-toggle-title">Reveal 演示文稿</div>
                                <div class="mission-show-toggle-desc">检测到幻灯片分隔符时，允许按 Reveal.js 方式渲染。</div>
                            </div>
                            <button type="button" id="revealToggle" runat="server" class="mission-show-toggle-switch" aria-pressed="false"><span class="mission-show-toggle-knob"></span></button>
                        </div>
                        <div id="revealToggleStatus" runat="server" class="mission-show-toggle-status">当前：关闭</div>
                    </div>
                </div>
                <asp:HiddenField ID="HiddenMissionRaw" runat="server" />
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
        window.__missionshowConfig = {
            mcontentId: '<%= Mcontent.ClientID %>',
            hiddenMissionRawId: '<%= HiddenMissionRaw.ClientID %>',
            markdownToggleId: '<%= markdownToggle.ClientID %>',
            markdownToggleStatusId: '<%= markdownToggleStatus.ClientID %>',
            revealToggleId: '<%= revealToggle.ClientID %>',
            revealToggleStatusId: '<%= revealToggleStatus.ClientID %>'
        };
    </script>
    <script type="text/javascript" src="../js/content-show-markdown.js"></script>
    <script type="text/javascript" src="../js/missionshow.js"></script>
</asp:Content>
