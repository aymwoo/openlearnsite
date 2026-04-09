<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="typechineseedit.aspx.cs" Inherits="Teacher_typechineseedit" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link href="../App_Themes/Teacher/course-content-add.css" rel="stylesheet" />
    <style type="text/css">
        .typechinese-edit-page {
            --content-add-page-bg: linear-gradient(180deg, #fdfaf6 0%, #fff7ed 100%);
            --content-add-hero-bg: linear-gradient(135deg, #7c2d12 0%, #c2410c 52%, #fb923c 100%);
            --content-add-hero-shadow: 0 22px 45px -28px rgba(194, 65, 12, 0.72);
            --content-add-primary-bg: #c2410c;
            --content-add-primary-hover: #9a3412;
            --content-add-primary-shadow: 0 14px 24px -18px rgba(194, 65, 12, 0.85);
            --content-add-secondary-border: #fdba74;
            --content-add-secondary-bg: #fff7ed;
            --content-add-secondary-hover: #ffedd5;
            --content-add-secondary-fg: #9a3412;
            --content-add-focus: #f97316;
            --content-add-focus-ring: rgba(249, 115, 22, 0.14);
        }

        .typechinese-edit-editor textarea {
            width: 100%;
            min-height: 420px;
            padding: 1rem 1.1rem;
            resize: vertical;
            line-height: 1.8;
        }

        .typechinese-edit-tip {
            margin: 0;
            color: #475569;
            font-size: 0.92rem;
            line-height: 1.8;
        }
    </style>

    <div class="content-add-page typechinese-edit-page">
        <div class="content-add-shell is-medium">
            <section class="content-add-hero">
                <div class="content-add-hero-content">
                    <div class="content-add-eyebrow">Pinyin Practice</div>
                    <h1 class="content-add-title">编辑拼音词语</h1>
                    <p class="content-add-subtitle">保持原有纯文本编码与保存逻辑，让词语维护和调整更直观。</p>
                </div>
            </section>

            <section class="content-add-panel">
                <h2 class="content-add-section-title">词语设置</h2>
                <p class="content-add-section-desc">适合维护拼音训练词组、短句和成段内容。</p>
                <div class="content-add-grid">
                    <div class="content-add-field content-add-field-wide">
                        <label class="content-add-label" for="<%= Ttitle.ClientID %>">拼音词语标题</label>
                        <asp:TextBox ID="Ttitle" runat="server" Width="220px" SkinID="TextBoxNormal" CssClass="content-add-input"></asp:TextBox>
                    </div>
                </div>
            </section>

            <section class="content-add-editor typechinese-edit-editor">
                <div class="content-add-editor-toolbar">
                    <div>
                        <h2 class="content-add-section-title">词语内容</h2>
                        <p class="content-add-section-desc">需要去掉网页格式时，可先清除格式再保存。</p>
                    </div>
                    <asp:Button ID="BtnNoSet" runat="server" Text="清除格式" OnClick="BtnNoSet_Click"
                        ToolTip="系统限制汉字长度为210个"
                        CssClass="content-add-secondary" />
                </div>
                <div class="content-add-editor-stage">
                    <asp:TextBox ID="Tcontent" runat="server" Height="500px" TextMode="MultiLine"
                        Width="650px" BorderColor="#DFDFDF" BorderStyle="Solid" BorderWidth="1px"
                        BackColor="White" CssClass="content-add-input"></asp:TextBox>
                </div>
            </section>

            <section class="content-add-feedback">
                <asp:Label ID="Labelmsg" runat="server" CssClass="typechinese-edit-tip">文章长度无限制，词语分隔符使用中文逗号和句号！</asp:Label>
            </section>

            <section class="content-add-actions">
                <asp:Button ID="BtnEdit" runat="server" Text="保存修改" OnClick="BtnEdit_Click"
                    CssClass="content-add-primary" />
                <asp:Button ID="Btnreturn" runat="server" Text="返回列表" OnClick="Btnreturn_Click"
                    CssClass="content-add-secondary" />
            </section>
        </div>
    </div>
</asp:Content>
