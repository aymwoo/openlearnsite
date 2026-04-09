<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master"  StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="typeadd.aspx.cs" Inherits="Teacher_typeadd" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link href="../App_Themes/Teacher/course-content-add.css" rel="stylesheet" />
    <style type="text/css">
        .type-article-page {
            --content-add-page-bg: linear-gradient(180deg, #f8fafc 0%, #eef6ff 100%);
            --content-add-hero-bg: linear-gradient(135deg, #0f766e 0%, #0f9b8e 55%, #22c55e 100%);
            --content-add-hero-shadow: 0 22px 45px -28px rgba(15, 118, 110, 0.72);
            --content-add-primary-bg: #0f766e;
            --content-add-primary-hover: #0d675f;
            --content-add-primary-shadow: 0 14px 24px -18px rgba(15, 118, 110, 0.85);
            --content-add-secondary-border: #99f6e4;
            --content-add-secondary-bg: #ecfeff;
            --content-add-secondary-hover: #cffafe;
            --content-add-secondary-fg: #115e59;
            --content-add-focus: #14b8a6;
            --content-add-focus-ring: rgba(20, 184, 166, 0.14);
        }

        .type-article-editor textarea {
            width: 100%;
            min-height: 320px;
            padding: 1rem 1.1rem;
            resize: vertical;
            line-height: 1.8;
        }

        .type-article-note {
            margin: 0;
            color: #475569;
            font-size: 0.92rem;
            line-height: 1.8;
        }
    </style>

    <div class="content-add-page type-article-page">
        <div class="content-add-shell is-medium">
            <section class="content-add-hero">
                <div class="content-add-hero-content">
                    <div class="content-add-eyebrow">Typing Practice</div>
                    <h1 class="content-add-title">新增打字文章</h1>
                    <p class="content-add-subtitle">录入标题和正文，系统会按原有规则清理格式、控制长度，并用于中文打字练习。</p>
                </div>
            </section>

            <section class="content-add-panel">
                <h2 class="content-add-section-title">基本信息</h2>
                <p class="content-add-section-desc">保持纯文本录入，避免影响打字训练内容判定。</p>
                <div class="content-add-grid">
                    <div class="content-add-field content-add-field-wide">
                        <label class="content-add-label" for="<%= Ttitle.ClientID %>">文章标题</label>
                        <asp:TextBox ID="Ttitle" runat="server" Width="220px" SkinID="TextBoxNormal"
                            CssClass="content-add-input"></asp:TextBox>
                    </div>
                </div>
            </section>

            <section class="content-add-editor type-article-editor">
                <div class="content-add-editor-toolbar">
                    <div>
                        <h2 class="content-add-section-title">文章内容</h2>
                        <p class="content-add-section-desc">输入练习正文。系统限制为 210 个汉字，提交前可一键清除格式。</p>
                    </div>
                    <asp:Button ID="BtnNoSet" runat="server" Text="清除格式" OnClick="BtnNoSet_Click"
                        ToolTip="系统限制汉字长度为210个"
                        CssClass="content-add-secondary" />
                </div>
                <div class="content-add-editor-stage">
                    <asp:TextBox ID="Tcontent" runat="server" Height="180px" MaxLength="300" TextMode="MultiLine"
                        Width="650px" BorderColor="#DFDFDF" BorderStyle="Solid" BorderWidth="1px"
                        BackColor="White" CssClass="content-add-input"></asp:TextBox>
                </div>
            </section>

            <section class="content-add-feedback">
                <asp:Label ID="Labelmsg" runat="server" CssClass="type-article-note">系统限制文章长度为210个汉字，添加时自动去空格并裁剪！</asp:Label>
            </section>

            <section class="content-add-actions">
                <asp:Button ID="BtnAdd" runat="server" Text="添加文章" OnClick="BtnAdd_Click"
                    CssClass="content-add-primary" />
                <asp:Button ID="Btnreturn" runat="server" Text="返回列表" OnClick="Btnreturn_Click"
                    CssClass="content-add-secondary" />
            </section>
        </div>
    </div>
</asp:Content>
