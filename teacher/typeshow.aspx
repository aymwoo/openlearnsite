<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="typeshow.aspx.cs" Inherits="Teacher_typeshow" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link href="../App_Themes/Teacher/typing-admin.css" rel="stylesheet" />
    <style type="text/css">
        .typing-article-detail-page {
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
    </style>
    <div class="typing-admin-page typing-article-detail-page">
        <div class="typing-admin-shell">
            <section class="typing-admin-hero">
                <div class="typing-admin-hero-content">
                    <div class="typing-admin-eyebrow">Typing Practice</div>
                    <h1 class="typing-admin-title">打字文章详情</h1>
                    <p class="typing-admin-subtitle">查看当前打字文章正文、类型和用途信息。</p>
                </div>
            </section>

            <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
            <section class="typing-admin-detail">
                <div class="typing-admin-detail-card">
                    <div class="typing-admin-detail-header">
                        <div class="typing-admin-toolbar">
                            <h2 class="typing-admin-detail-title"><%# Eval("Ttitle") %></h2>
                            <asp:Button ID="BtnEdit" runat="server" Text="编辑内容" ToolTip="点击修改"
                                OnClick="BtnEdit_Click" CssClass="typing-admin-btn typing-admin-btn--primary" />
                        </div>
                    </div>
                    <div class="typing-admin-detail-body">
                        <div class="typing-admin-meta">
                            <div class="typing-admin-meta-item">文章编号：<%# Eval("Tid") %></div>
                            <div class="typing-admin-meta-item">文章类型：<%# Eval("Ttype") %></div>
                            <div class="typing-admin-meta-item">文章用途：<%# Eval("Tuse") %></div>
                        </div>
                        <div class="typing-admin-detail-content"><%# Eval("Tcontent") %></div>
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
