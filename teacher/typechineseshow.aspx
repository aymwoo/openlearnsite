<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master"  StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="typechineseshow.aspx.cs" Inherits="Teacher_typechineseshow" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link href="../App_Themes/Teacher/typing-admin.css" rel="stylesheet" />
    <style type="text/css">
        .typing-chinese-detail-page {
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
    </style>
    <div class="typing-admin-page typing-chinese-detail-page">
        <div class="typing-admin-shell">
            <section class="typing-admin-hero">
                <div class="typing-admin-hero-content">
                    <div class="typing-admin-eyebrow">Pinyin Practice</div>
                    <h1 class="typing-admin-title">拼音词语详情</h1>
                    <p class="typing-admin-subtitle">查看当前拼音词语原文与展示效果。</p>
                </div>
            </section>

            <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
            <section class="typing-admin-detail">
                <div class="typing-admin-detail-card">
                    <div class="typing-admin-detail-header">
                        <div class="typing-admin-toolbar">
                            <h2 class="typing-admin-detail-title"><%# Eval("Ntitle") %></h2>
                            <asp:Button ID="BtnEdit" runat="server" Text="编辑内容" ToolTip="点击修改"
                                OnClick="BtnEdit_Click" CssClass="typing-admin-btn typing-admin-btn--primary" />
                        </div>
                    </div>
                    <div class="typing-admin-detail-body">
                        <div class="typing-admin-detail-content"><%# Eval("Ncontent") %></div>
                    </div>
                </div>
            </section>
            </ItemTemplate>
            </asp:Repeater>

            <section class="typing-admin-actions">
                <div class="typing-admin-action-row">
                    <asp:Button ID="Btnreturn" runat="server" Text="返回列表" OnClick="Btnreturn_Click" CssClass="typing-admin-btn typing-admin-btn--secondary" />
                </div>
            </section>
        </div>
    </div>
</asp:Content>
