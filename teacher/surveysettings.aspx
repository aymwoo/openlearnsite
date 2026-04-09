<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" AutoEventWireup="true" CodeFile="surveysettings.aspx.cs" Inherits="teacher_surveysettings" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link href="../App_Themes/Teacher/surveysettings.css" rel="stylesheet" />

    <div class="survey-settings-shell">
        <div class="survey-settings-toolbar">
            <div class="survey-settings-toolbar-intro">
                <div class="survey-settings-toolbar-eyebrow">Survey AI</div>
                <div class="survey-settings-toolbar-title">旧版 Survey 设置</div>
                <div class="survey-settings-toolbar-meta">按单个活动控制是否启用 AI 评价，并快速打开教师预览或学生页面检查结果。</div>
            </div>
            <div class="survey-settings-toolbar-actions">
                <asp:HyperLink ID="HyperLinkReturn" runat="server" CssClass="survey-settings-btn">返回学案</asp:HyperLink>
                <asp:HyperLink ID="HyperLinkPreview" runat="server" CssClass="survey-settings-btn" Target="_blank">教师预览</asp:HyperLink>
                <asp:HyperLink ID="HyperLinkStudent" runat="server" CssClass="survey-settings-btn" Target="_blank">学生页面</asp:HyperLink>
                <asp:Button ID="BtnSave" runat="server" CssClass="survey-settings-btn survey-settings-btn-primary" Text="保存设置" OnClick="BtnSave_Click" />
            </div>
        </div>

        <div class="survey-settings-card">
            <div class="survey-settings-hero">
                <span class="survey-settings-kicker">Survey AI</span>
                <h1 class="survey-settings-title"><asp:Literal ID="LiteralTitle" runat="server"></asp:Literal></h1>
                <p class="survey-settings-subtitle">旧版 Survey 调查/测验也支持按单个活动独立控制 AI 评价。开启后，学生提交时调用 AI 生成测验评估；关闭后只生成规则评估摘要。</p>
            </div>
            <div class="survey-settings-body">
                <div class="survey-settings-grid">
                    <div class="survey-settings-info">
                        <div class="survey-settings-info-label">活动类型</div>
                        <div class="survey-settings-info-value"><asp:Literal ID="LiteralType" runat="server"></asp:Literal></div>
                    </div>
                    <div class="survey-settings-info">
                        <div class="survey-settings-info-label">课程编号</div>
                        <div class="survey-settings-info-value"><asp:Literal ID="LiteralCid" runat="server"></asp:Literal></div>
                    </div>
                    <div class="survey-settings-info">
                        <div class="survey-settings-info-label">Survey 编号</div>
                        <div class="survey-settings-info-value"><asp:Literal ID="LiteralVid" runat="server"></asp:Literal></div>
                    </div>
                </div>

                <div class="survey-settings-toggle">
                    <div class="survey-settings-toggle-head">
                        <span class="survey-settings-toggle-badge">AI 评价</span>
                        <span class="survey-settings-toggle-status"><asp:Literal ID="LiteralStatus" runat="server"></asp:Literal></span>
                    </div>
                    <label class="survey-settings-toggle-label">
                        <asp:CheckBox ID="CheckBoxEnableAi" runat="server" />
                        <span>启用 AI 评价</span>
                    </label>
                    <div class="survey-settings-toggle-desc">按当前调查/测验单独控制。该设置不会影响其它 Survey 活动，也不会依赖全局是否默认启用 AI。</div>
                </div>

                <div class="survey-settings-section-title">活动说明</div>
                <div class="survey-settings-content">
                    <asp:Literal ID="LiteralContent" runat="server"></asp:Literal>
                </div>

                <asp:Label ID="LabelMessage" runat="server" CssClass="survey-settings-message" Visible="false"></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>
