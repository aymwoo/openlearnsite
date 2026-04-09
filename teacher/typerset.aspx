<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher"  AutoEventWireup="true" CodeFile="typerset.aspx.cs" Inherits="Teacher_typerset" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link href="../App_Themes/Teacher/typing-admin.css" rel="stylesheet" />
    <style type="text/css">
        .typing-article-set-page {
            --typing-admin-page-bg: linear-gradient(180deg, #f8fafc 0%, #eef6ff 100%);
            --typing-admin-hero-bg: linear-gradient(135deg, #0f766e 0%, #0f9b8e 55%, #22c55e 100%);
            --typing-admin-hero-shadow: 0 22px 45px -28px rgba(15, 118, 110, 0.72);
            --typing-admin-primary-bg: #0f766e;
            --typing-admin-primary-hover: #0d675f;
            --typing-admin-primary-shadow: 0 14px 24px -18px rgba(15, 118, 110, 0.85);
            --typing-admin-secondary-border: #99f6e4;
            --typing-admin-secondary-bg: #ecfeff;
            --typing-admin-secondary-hover: #cffafe;
            --typing-admin-secondary-fg: #115e59;
        }

        .typing-admin-choices .typerset {
            margin: 0;
        }
    </style>
    <div class="typing-admin-page typing-article-set-page">
        <div class="typing-admin-shell">
            <section class="typing-admin-hero">
                <div class="typing-admin-hero-content">
                    <div class="typing-admin-eyebrow">Typing Practice</div>
                    <h1 class="typing-admin-title">中文打字设置</h1>
                    <p class="typing-admin-subtitle">为所教年级分配可用的打字文章，切换年级后可以立即查看当前选中项。</p>
                </div>
            </section>

            <section class="typing-admin-panel">
                <div class="typing-admin-toolbar">
                    <div>
                        <h2 class="typing-admin-section-title">年级与文章选择</h2>
                        <p class="typing-admin-section-desc">勾选后提交即可更新所教年级的打字文章配置。</p>
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
                                Text='<%# Eval("Ttitle") %>'  />
                            <asp:Label ID="Lbtid" runat="server" Text='<%# Eval("Tid") %>' Visible="False"></asp:Label>
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
