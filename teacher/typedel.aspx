<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="typedel.aspx.cs" Inherits="Teacher_typedel" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link href="../App_Themes/Teacher/admin-form.css" rel="stylesheet" />
    <style type="text/css">
        .danger-page {
            --admin-form-page-bg: linear-gradient(180deg, #fff7ed 0%, #ffedd5 100%);
            --admin-form-hero-bg: linear-gradient(135deg, #991b1b 0%, #dc2626 55%, #f97316 100%);
            --admin-form-hero-shadow: 0 22px 45px -28px rgba(220, 38, 38, 0.72);
            --admin-form-primary-bg: #dc2626;
            --admin-form-primary-hover: #b91c1c;
            --admin-form-primary-shadow: 0 14px 24px -18px rgba(220, 38, 38, 0.85);
            --admin-form-secondary-border: #fdba74;
            --admin-form-secondary-bg: #fff7ed;
            --admin-form-secondary-hover: #ffedd5;
            --admin-form-secondary-fg: #9a3412;
        }
    </style>
    <div class="admin-form-page danger-page">
        <div class="admin-form-shell">
            <section class="admin-form-hero">
                <div class="admin-form-hero-content">
                    <div class="admin-form-eyebrow">Delete Article</div>
                    <h1 class="admin-form-title">删除打字文章</h1>
                    <p class="admin-form-subtitle">请确认是否删除当前打字文章，删除后当前条目将无法继续在文章列表中使用。</p>
                </div>
            </section>
            <section class="admin-form-panel">
                <div class="admin-form-kv">
                    <div class="admin-form-kv-item">
                        <span class="admin-form-kv-label">文章编号</span>
                        <span class="admin-form-kv-value"><asp:Label ID="LabelTid" runat="server" BackColor="#E1EEE4" Font-Bold="True" Font-Names="Arial"></asp:Label></span>
                    </div>
                </div>
            </section>
            <section class="admin-form-actions">
                <div class="admin-form-action-row">
                    <asp:LinkButton ID="LinkBtnDel" runat="server" OnClick="LinkBtnDel_Click" CssClass="admin-form-btn admin-form-btn--primary">确定</asp:LinkButton>
                    <asp:LinkButton ID="LinkBtncancel" runat="server" OnClick="LinkBtncancel_Click" CssClass="admin-form-btn admin-form-btn--secondary">返回列表</asp:LinkButton>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
