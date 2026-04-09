<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="typer.aspx.cs" Inherits="Teacher_typer" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link href="../App_Themes/Teacher/typing-admin.css" rel="stylesheet" />
    <style type="text/css">
        .typing-article-page {
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
    <div class="typing-admin-page typing-article-page">
        <div class="typing-admin-shell">
            <section class="typing-admin-hero">
                <div class="typing-admin-hero-content">
                    <div class="typing-admin-eyebrow">Typing Practice</div>
                    <h1 class="typing-admin-title">打字文章管理</h1>
                    <p class="typing-admin-subtitle">管理中文打字练习文章，维护文章列表，并按需要清理学生打字成绩。</p>
                </div>
            </section>

            <section class="typing-admin-panel">
                <div class="typing-admin-toolbar">
                    <div>
                        <h2 class="typing-admin-section-title">文章列表</h2>
                        <p class="typing-admin-section-desc">支持浏览、编辑、删除和分页查看现有打字文章。</p>
                    </div>
                    <div class="typing-admin-action-row">
                        <asp:Button ID="BtnTypeSet" runat="server" Text="打字设置"
                            onclick="BtnTypeSet_Click" CssClass="typing-admin-btn typing-admin-btn--secondary" />
                        <asp:Button ID="BtnAdd" runat="server" Text="文章添加"
                            onclick="BtnAdd_Click" CssClass="typing-admin-btn typing-admin-btn--primary" />
                    </div>
                </div>
                <div class="typing-admin-grid-wrap">
                    <asp:GridView ID="GVType" runat="server" AllowPaging="True"
                    AutoGenerateColumns="False" CellPadding="5" SkinID="GridViewInfo"
                    PageSize="20" Width="98%" onpageindexchanging="GVType_PageIndexChanging"
                    onrowdatabound="GVType_RowDataBound" EnableModelValidation="True">
                    <Columns>
                        <asp:BoundField HeaderText="序号" />
                        <asp:HyperLinkField DataNavigateUrlFields="Tid" 
                            DataNavigateUrlFormatString="typeshow.aspx?tid={0}" DataTextField="Ttitle" 
                            HeaderText="文章标题">
                        <ItemStyle HorizontalAlign="Left" />
                        </asp:HyperLinkField>
                        <asp:BoundField DataField="Ttype" HeaderText="文章类型" />
                        <asp:BoundField DataField="Tuse" HeaderText="文章范围">
                        <ControlStyle Width="30px" />
                        </asp:BoundField>
                        <asp:HyperLinkField DataNavigateUrlFields="Tid" 
                            DataNavigateUrlFormatString="typeedit.aspx?tid={0}" Text="编辑">
                        <ControlStyle Width="30px" />
                        </asp:HyperLinkField>
                        <asp:HyperLinkField DataNavigateUrlFields="Tid" 
                            DataNavigateUrlFormatString="typedel.aspx?tid={0}" Text="删除" />
                    </Columns>
                    <pagertemplate>
                        <div  class="pagediv">
                            第<asp:Label ID="lblPageIndex" runat="server" 
                                text="<%# ((GridView)Container.Parent.Parent).PageIndex + 1  %>" />
                            页  共<asp:Label ID="lblPageCount" runat="server" 
                                text="<%# ((GridView)Container.Parent.Parent).PageCount  %>" />
                            页 
                            <asp:LinkButton ID="btnFirst" runat="server" causesvalidation="False" 
                                commandargument="First" commandname="Page" Font-Underline="False" 
                                ForeColor="Black" text="首页" />
                            <asp:LinkButton ID="btnPrev" runat="server" causesvalidation="False" 
                                commandargument="Prev" commandname="Page" Font-Underline="False" 
                                ForeColor="Black" text="上一页" />
                            <asp:LinkButton ID="btnNext" runat="server" causesvalidation="False" 
                                commandargument="Next" commandname="Page" Font-Underline="False" 
                                ForeColor="Black" text="下一页" />
                            <asp:LinkButton ID="btnLast" runat="server" causesvalidation="False" 
                                commandargument="Last" commandname="Page" Font-Underline="False" 
                                ForeColor="Black" text="尾页" />
                        </div>
                    </pagertemplate>
                </asp:GridView>
                </div>
            </section>

            <section class="typing-admin-feedback">
                <asp:Label ID="labelmsg" runat="server" SkinID="LabelMsgRed" Width="160px"></asp:Label>
            </section>

            <section class="typing-admin-actions">
                <div class="typing-admin-toolbar">
                    <div>
                        <h2 class="typing-admin-section-title">成绩处理</h2>
                        <p class="typing-admin-section-desc">按速度阈值清理记录，或直接清除班级中文打字和指法打字成绩。</p>
                    </div>
                    <div class="typing-admin-inline-controls">
                        <asp:DropDownList ID="DDLpscore" runat="server" Font-Size="9pt" Width="60px" CssClass="typing-admin-select">
                    <asp:ListItem>200</asp:ListItem>
                    <asp:ListItem Selected="True">100</asp:ListItem>
                    <asp:ListItem>300</asp:ListItem>
                    <asp:ListItem>400</asp:ListItem>
                    <asp:ListItem>500</asp:ListItem>
                </asp:DropDownList>
                        <span>以上速度</span>
                        <asp:Button ID="ButtonClearThis" runat="server" Text="清除"
                            onclick="ButtonClearThis_Click" ToolTip="清除超过指定速度的中文打字成绩" CssClass="typing-admin-btn typing-admin-btn--secondary" />
                        <asp:Button ID="ButtonClearType" runat="server" Text="清除中文打字成绩"
                            onclick="ButtonClearType_Click" Width="140px" CssClass="typing-admin-btn typing-admin-btn--secondary" />
                        <asp:Button ID="ButtonClearFinger" runat="server" Text="清除指法打字成绩"
                            onclick="ButtonClearFinger_Click" Width="140px" CssClass="typing-admin-btn typing-admin-btn--secondary" />
                        <asp:HyperLink ID="HLprint" runat="server"
                            NavigateUrl="~/teacher/printtyper.aspx" Target="_blank" Height="18px" CssClass="typing-admin-link">排行榜打印</asp:HyperLink>
                        <asp:HyperLink ID="HLfinger" runat="server"
                            NavigateUrl="~/en.aspx" Target="_blank" Height="18px" CssClass="typing-admin-link">指法英文字典</asp:HyperLink>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
