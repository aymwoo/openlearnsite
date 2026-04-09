<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="package.aspx.cs" Inherits="Teacher_package" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link href="../App_Themes/Teacher/admin-form.css" rel="stylesheet" />
    <style type="text/css">
        .package-page {
            --admin-form-page-bg: linear-gradient(180deg, #f8fafc 0%, #eef6ff 100%);
            --admin-form-hero-bg: linear-gradient(135deg, #312e81 0%, #4338ca 55%, #6366f1 100%);
            --admin-form-hero-shadow: 0 22px 45px -28px rgba(79, 70, 229, 0.75);
            --admin-form-primary-bg: #4f46e5;
            --admin-form-primary-hover: #4338ca;
            --admin-form-primary-shadow: 0 14px 24px -18px rgba(79, 70, 229, 0.85);
            --admin-form-secondary-border: #c7d2fe;
            --admin-form-secondary-bg: #eef2ff;
            --admin-form-secondary-hover: #e0e7ff;
            --admin-form-secondary-fg: #3730a3;
        }
    </style>
    <div class="admin-form-page package-page">
        <div class="admin-form-shell">
            <section class="admin-form-hero">
                <div class="admin-form-hero-content">
                    <div class="admin-form-eyebrow">Course Package</div>
                    <h1 class="admin-form-title">学案包导出</h1>
                    <p class="admin-form-subtitle">打包当前学案目录内的资源文件，并生成可下载的学案包。</p>
                </div>
            </section>

            <section class="admin-form-panel">
                <h2 class="admin-form-section-title">学案信息</h2>
                <p class="admin-form-section-desc">确认当前学案名称和编号后开始打包，完成后可直接下载生成文件。</p>
                <div class="admin-form-kv">
                    <div class="admin-form-kv-item">
                        <span class="admin-form-kv-label">学案名称</span>
                        <span class="admin-form-kv-value"><asp:Label ID="LabelCtitle" runat="server"></asp:Label></span>
                    </div>
                    <div class="admin-form-kv-item">
                        <span class="admin-form-kv-label">学案 ID</span>
                        <span class="admin-form-kv-value"><asp:Label ID="LabelCid" runat="server"></asp:Label></span>
                    </div>
                </div>
            </section>

            <asp:Panel ID="Panelinfo" runat="server" Visible="False" CssClass="admin-form-panel">
                <h2 class="admin-form-section-title">下载结果</h2>
                <p class="admin-form-section-desc">打包完成后，可通过下面的按钮下载学案包。</p>
                <asp:Label ID="Labelinfo" runat="server"></asp:Label>
                <div class="admin-form-action-row" style="margin-top:1rem;">
                    <asp:Button ID="Btndown" runat="server" onclick="Btndown_Click"
                        Text="下载压缩包" ToolTip="点击下载" CssClass="admin-form-btn admin-form-btn--primary" />
                </div>
            </asp:Panel>

            <section class="admin-form-actions">
                <div class="admin-form-action-row">
                    <asp:Button ID="BtnZip" runat="server" Text="开始打包" onclick="BtnZip_Click" ToolTip="点击开始学案打包" CssClass="admin-form-btn admin-form-btn--primary" />
                    <asp:Button ID="Btnreturn" runat="server" Text="返回学案" onclick="Btnreturn_Click" CssClass="admin-form-btn admin-form-btn--secondary" />
                </div>
            </section>

            <section class="admin-form-feedback">
                <asp:Label ID="Labelmsg" runat="server" SkinID="LabelMsgRed"></asp:Label>
            </section>

            <section class="admin-form-list">
                <h2 class="admin-form-section-title">资源列表</h2>
                <p class="admin-form-section-desc">当前学案目录内将参与打包的资源文件如下。</p>
                <div class="admin-form-list-items">
                    <asp:DataList ID="Dlfilelist" runat="server"
                    RepeatColumns="2" RepeatDirection="Horizontal" Caption="本学案目录内资源列表"
                CaptionAlign="Left" CellPadding="3" CellSpacing="3"
                    onitemdatabound="Dlfilelist_ItemDataBound" Width="100%" RepeatLayout="Flow">
                    <ItemTemplate>
                        <div class="admin-form-list-item">
                            <asp:Label ID="Labelfid" runat="server" Text='<%# Eval("fid") %>' BackColor="#EEF0EF"></asp:Label>&nbsp;
                            <asp:HyperLink ID="HLfname" runat="server" Target="_blank" Text='<%# Eval("fname") %>' ></asp:HyperLink>&nbsp;
                            <asp:Label ID="Labelfsize" runat="server" Text='<%# Eval("fsize") %>' ></asp:Label>
                            <asp:Label ID="Labelfread" runat="server" Text='<%#  Eval("fread") %>'  ToolTip="是否只读（T：只读 | F：可写）"  ForeColor="#00A279"></asp:Label>
                            <asp:Label ID="Labelurl" runat="server" Text='<%# Eval("furl") %>' Visible="false" ></asp:Label>
                        </div>
                    </ItemTemplate>
                    <SeparatorStyle BorderColor="Silver" BorderStyle="Dotted" BorderWidth="1px" />
                </asp:DataList>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
