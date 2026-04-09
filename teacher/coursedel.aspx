<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="coursedel.aspx.cs" Inherits="Teacher_coursedel" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link href="../App_Themes/Teacher/admin-form.css" rel="stylesheet" />
    <style type="text/css">
        .course-delete-page {
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
    <div class="admin-form-page course-delete-page">
        <div class="admin-form-shell">
            <section class="admin-form-hero">
                <div class="admin-form-hero-content">
                    <div class="admin-form-eyebrow">Delete Course</div>
                    <h1 class="admin-form-title">删除学案</h1>
                    <p class="admin-form-subtitle">请再次确认是否删除当前学案，删除后将无法通过当前页面恢复。</p>
                </div>
            </section>

            <section class="admin-form-panel">
                <h2 class="admin-form-section-title">待删除对象</h2>
                <p class="admin-form-section-desc">下方显示当前准备删除的学案标识，请确认无误后继续。</p>
                <div class="admin-form-kv">
                    <div class="admin-form-kv-item">
                        <span class="admin-form-kv-label">学案信息</span>
                        <span class="admin-form-kv-value"><asp:Label ID="LabelID" runat="server" Font-Bold="True"></asp:Label></span>
                    </div>
                </div>
            </section>

            <section class="admin-form-actions">
                <div class="admin-form-action-row">
                    <asp:Button ID="ButtonDel" runat="server" Text="确定" EnableViewState="False" OnClick="ButtonDel_Click" CssClass="admin-form-btn admin-form-btn--primary" />
                    <asp:Button ID="ButtonCancle" runat="server" Text="取消" OnClick="ButtonCancle_Click" CssClass="admin-form-btn admin-form-btn--secondary" />
                </div>
            </section>
        </div>
    </div>
</asp:Content>
