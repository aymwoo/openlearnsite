<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="typechineseadd.aspx.cs" Inherits="Teacher_typechineseadd" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link href="../App_Themes/Teacher/course-content-add.css" rel="stylesheet" />
    <style type="text/css">
        .typechinese-add-page {
            --content-add-page-bg: linear-gradient(180deg, #fdfaf6 0%, #fff7ed 100%);
            --content-add-hero-bg: linear-gradient(135deg, #9a3412 0%, #ea580c 55%, #f59e0b 100%);
            --content-add-hero-shadow: 0 22px 45px -28px rgba(234, 88, 12, 0.72);
            --content-add-primary-bg: #ea580c;
            --content-add-primary-hover: #c2410c;
            --content-add-primary-shadow: 0 14px 24px -18px rgba(234, 88, 12, 0.85);
            --content-add-secondary-border: #fdba74;
            --content-add-secondary-bg: #fff7ed;
            --content-add-secondary-hover: #ffedd5;
            --content-add-secondary-fg: #c2410c;
            --content-add-focus: #f97316;
            --content-add-focus-ring: rgba(249, 115, 22, 0.14);
        }

        .typechinese-add-editor textarea {
            width: 100%;
            min-height: 420px;
            padding: 1rem 1.1rem;
            resize: vertical;
            line-height: 1.8;
        }

        .typechinese-add-tip {
            margin: 0;
            color: #475569;
            font-size: 0.92rem;
            line-height: 1.8;
        }
    </style>

    <div class="content-add-page typechinese-add-page">
        <div class="content-add-shell is-medium">
            <section class="content-add-hero">
                <div class="content-add-hero-content">
                    <div class="content-add-eyebrow">Pinyin Practice</div>
                    <h1 class="content-add-title">新增拼音词语</h1>
                    <p class="content-add-subtitle">录入拼音训练标题和正文内容，保持现有纯文本规则，方便学生进行拼音输入练习。</p>
                </div>
            </section>

            <section class="content-add-panel">
                <h2 class="content-add-section-title">词语设置</h2>
                <p class="content-add-section-desc">正文不做富文本处理，保留当前分词和清洗逻辑。</p>
                <div class="content-add-grid">
                    <div class="content-add-field content-add-field-wide">
                        <label class="content-add-label" for="<%= Ttitle.ClientID %>">拼音词语标题</label>
                        <asp:TextBox ID="Ttitle" runat="server" Width="220px" SkinID="TextBoxNormal" CssClass="content-add-input"></asp:TextBox>
                    </div>
                </div>
            </section>

            <section class="content-add-editor typechinese-add-editor">
                <div class="content-add-editor-toolbar">
                    <div>
                        <h2 class="content-add-section-title">词语内容</h2>
                        <p class="content-add-section-desc">支持直接粘贴多行词语内容，建议使用中文逗号、句号或空格作为分隔符。</p>
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
                <asp:Label ID="Labelmsg" runat="server" CssClass="typechinese-add-tip">文章长度无限制，词语分隔符使用中文逗号、句号或空格！</asp:Label>
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
