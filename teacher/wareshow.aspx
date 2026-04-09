<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" AutoEventWireup="true" CodeFile="wareshow.aspx.cs" Inherits="teacher_wareshow" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link href="../App_Themes/Teacher/admin-form.css" rel="stylesheet" />
    <style type="text/css">
        .ware-show-page {
            --admin-form-page-bg: linear-gradient(180deg, #f8fafc 0%, #eef2ff 100%);
            --admin-form-hero-bg: linear-gradient(135deg, #0f172a 0%, #1d4ed8 55%, #06b6d4 100%);
            --admin-form-hero-shadow: 0 22px 45px -28px rgba(29, 78, 216, 0.72);
            --admin-form-primary-bg: #2563eb;
            --admin-form-primary-hover: #1d4ed8;
            --admin-form-primary-shadow: 0 14px 24px -18px rgba(37, 99, 235, 0.85);
            --admin-form-secondary-border: #bfdbfe;
            --admin-form-secondary-bg: #eff6ff;
            --admin-form-secondary-hover: #dbeafe;
            --admin-form-secondary-fg: #1d4ed8;
        }

        .ware-show-frame {
            width: 100%;
            min-height: 80vh;
            border: 1px solid #cbd5e1;
            border-radius: 1rem;
            background: #ffffff;
        }
    </style>
    <div class="admin-form-page ware-show-page">
        <div class="admin-form-shell">
            <section class="admin-form-hero">
                <div class="admin-form-hero-content">
                    <div class="admin-form-eyebrow">Web Courseware</div>
                    <h1 class="admin-form-title"><asp:Label ID="LabelMtitle" runat="server"></asp:Label></h1>
                    <p class="admin-form-subtitle">查看网页课件首页、发布状态和课件文件类型。</p>
                </div>
            </section>

            <section class="admin-form-panel">
                <div class="admin-form-toolbar">
                    <div>
                        <h2 class="admin-form-section-title">课件信息</h2>
                        <p class="admin-form-section-desc">可以直接预览当前课件首页，并查看当前网页课件主题信息。</p>
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
                        <span class="admin-form-kv-label">课件首页</span>
                        <span class="admin-form-kv-value"><asp:HyperLink ID="HyperLinkHtml" runat="server" Target="_blank" CssClass="admin-form-link">课件首页</asp:HyperLink></span>
                    </div>
                </div>
            </section>

            <section class="admin-form-panel">
                <h2 class="admin-form-section-title">课件预览</h2>
                <p class="admin-form-section-desc">以下 iframe 直接预览当前配置的网页课件首页效果。</p>
                <iframe id="htmliframe" src="<%=WareUrl %>" class="ware-show-frame"></iframe>
            </section>
        </div>
    </div>
</asp:Content>
