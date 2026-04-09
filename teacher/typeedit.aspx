<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher" Validaterequest="false" AutoEventWireup="true" CodeFile="typeedit.aspx.cs" Inherits="Teacher_typeedit" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link href="../App_Themes/Teacher/course-content-add.css" rel="stylesheet" />
    <style type="text/css">
        .type-edit-page {
            --content-add-page-bg: linear-gradient(180deg, #f8fafc 0%, #eef6ff 100%);
            --content-add-hero-bg: linear-gradient(135deg, #1d4ed8 0%, #2563eb 52%, #38bdf8 100%);
            --content-add-hero-shadow: 0 22px 45px -28px rgba(37, 99, 235, 0.72);
            --content-add-primary-bg: #2563eb;
            --content-add-primary-hover: #1d4ed8;
            --content-add-primary-shadow: 0 14px 24px -18px rgba(37, 99, 235, 0.85);
            --content-add-secondary-border: #bfdbfe;
            --content-add-secondary-bg: #eff6ff;
            --content-add-secondary-hover: #dbeafe;
            --content-add-secondary-fg: #1d4ed8;
            --content-add-focus: #3b82f6;
            --content-add-focus-ring: rgba(59, 130, 246, 0.14);
        }

        .type-edit-editor textarea {
            width: 100%;
            min-height: 320px;
            padding: 1rem 1.1rem;
            resize: vertical;
            line-height: 1.8;
        }

        .type-edit-tip {
            margin: 0;
            color: #475569;
            font-size: 0.92rem;
            line-height: 1.8;
        }
    </style>

    <div class="content-add-page type-edit-page">
        <div class="content-add-shell is-medium">
            <section class="content-add-hero">
                <div class="content-add-hero-content">
                    <div class="content-add-eyebrow">Typing Practice</div>
                    <h1 class="content-add-title">编辑打字文章</h1>
                    <p class="content-add-subtitle">保留原有纯文本保存方式，同时把文章标题、用途、范围和正文整理为更清晰的编辑界面。</p>
                </div>
            </section>

            <section class="content-add-panel">
                <h2 class="content-add-section-title">文章设置</h2>
                <p class="content-add-section-desc">标题必填，正文仍按现有逻辑进行 HTML 编码和长度控制。</p>
                <div class="content-add-grid">
                    <div class="content-add-field">
                        <label class="content-add-label" for="<%= Ttitle.ClientID %>">文章标题</label>
                        <asp:TextBox ID="Ttitle" runat="server" Width="200px" SkinID="TextBoxNormal" CssClass="content-add-input"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="Ttitle" Font-Size="9pt" Width="10px"></asp:RequiredFieldValidator>
                    </div>
                    <div class="content-add-field">
                        <label class="content-add-label" for="<%= DDLuse.ClientID %>">文章用途</label>
                        <asp:DropDownList ID="DDLuse" runat="server" Font-Size="9pt" Width="71px" Font-Names="Arial" CssClass="content-add-select">
                            <asp:ListItem Selected="True" Value="11">中文练习</asp:ListItem>
                            <asp:ListItem Value="12">中文比赛</asp:ListItem>
                            <asp:ListItem Value="21">英文练习</asp:ListItem>
                            <asp:ListItem Value="22">英文比赛</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="content-add-field">
                        <label class="content-add-label" for="<%= DDLtype.ClientID %>">文章范围</label>
                        <asp:DropDownList ID="DDLtype" runat="server" Font-Size="8pt" Width="34px" Font-Names="Arial" CssClass="content-add-select">
                            <asp:ListItem>0</asp:ListItem>
                            <asp:ListItem>1</asp:ListItem>
                            <asp:ListItem>2</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </section>

            <section class="content-add-editor type-edit-editor">
                <div class="content-add-editor-toolbar">
                    <div>
                        <h2 class="content-add-section-title">文章正文</h2>
                        <p class="content-add-section-desc">适合直接粘贴纯文本打字素材。需要清除网页格式时，可使用右侧按钮。</p>
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
                <asp:Label ID="Labelmsg" runat="server" CssClass="type-edit-tip"></asp:Label>
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
