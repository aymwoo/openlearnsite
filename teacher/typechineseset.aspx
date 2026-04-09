<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher"  AutoEventWireup="true" CodeFile="typechineseset.aspx.cs" Inherits="Teacher_typechineseset" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link href="../App_Themes/Teacher/typing-admin.css" rel="stylesheet" />
    <style type="text/css">
        .typing-chinese-set-page {
            --typing-admin-page-bg: linear-gradient(180deg, #fdfaf6 0%, #fff7ed 100%);
            --typing-admin-hero-bg: linear-gradient(135deg, #9a3412 0%, #ea580c 55%, #f59e0b 100%);
            --typing-admin-hero-shadow: 0 22px 45px -28px rgba(234, 88, 12, 0.72);
            --typing-admin-primary-bg: #ea580c;
            --typing-admin-primary-hover: #c2410c;
            --typing-admin-primary-shadow: 0 14px 24px -18px rgba(234, 88, 12, 0.85);
            --typing-admin-secondary-border: #fdba74;
            --typing-admin-secondary-bg: #fff7ed;
            --typing-admin-secondary-hover: #ffedd5;
            --typing-admin-secondary-fg: #c2410c;
        }

        .typing-admin-choices .typerset {
            margin: 0;
        }
    </style>
    <div class="typing-admin-page typing-chinese-set-page">
        <div class="typing-admin-shell">
            <section class="typing-admin-hero">
                <div class="typing-admin-hero-content">
                    <div class="typing-admin-eyebrow">Pinyin Practice</div>
                    <h1 class="typing-admin-title">拼音词语打字设置</h1>
                    <p class="typing-admin-subtitle">按年级选择可用的拼音词语内容，保存后立即生效。</p>
                </div>
            </section>

            <section class="typing-admin-panel">
                <div class="typing-admin-toolbar">
                    <div>
                        <h2 class="typing-admin-section-title">年级与词语选择</h2>
                        <p class="typing-admin-section-desc">切换年级可查看当前配置，并批量调整本年级可见词语。</p>
                    </div>
                    <div class="typing-admin-inline-controls">
                        <span>选择年级</span>
                        <asp:DropDownList ID="DDLgrade" runat="server" Font-Size="9pt"
                            EnableTheming="True" Font-Names="Arial" AutoPostBack="True"
                            onselectedindexchanged="DDLgrade_SelectedIndexChanged" CssClass="typing-admin-select">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="typing-admin-choices">
                    <asp:DataList ID="DataListTyper" runat="server" RepeatColumns="5" Width="100%"
                            CellPadding="3" CellSpacing="3"
                            onitemdatabound="DataListTyper_ItemDataBound" RepeatLayout="Flow">
                        <ItemTemplate>
                        <div class="typing-admin-choice">
                            <asp:CheckBox ID="ChkTyper" runat="server"
                                Text='<%# Eval("Ntitle") %>'  />
                            <asp:Label ID="Lbtid" runat="server" Text='<%# Eval("Nid") %>' Visible="False"></asp:Label>
                        </div>
                        </ItemTemplate>
                    </asp:DataList>
                </div>
            </section>

            <section class="typing-admin-actions">
                <div class="typing-admin-action-row">
                    <asp:Button ID="BtnSelect" runat="server" Text="提交选择" onclick="BtnSelect_Click"
                        CssClass="typing-admin-btn typing-admin-btn--primary" />
                    <asp:Button ID="BtnReturn" runat="server" Text="返回列表" onclick="BtnReturn_Click"
                        CssClass="typing-admin-btn typing-admin-btn--secondary" />
                </div>
            </section>
            <asp:Label ID="LabelTids" runat="server" Visible="False"></asp:Label>
        </div>
    </div>
</asp:Content>
