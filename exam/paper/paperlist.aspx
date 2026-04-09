<%@ Page Language="C#" AutoEventWireup="true" CodeFile="paperlist.aspx.cs" Inherits="exam_paper_paperlist" MasterPageFile="~/teacher/Teach.master" %><asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/js/MenuCookie.js" type="text/javascript"></script>
    <script src="/js/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/kindeditor/plugins/code/prettify.js" type="text/javascript"></script>
    <script src="/js/ruffle.js" type="text/javascript"></script>
</asp:Content><asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <style>
        .paper-page { min-height: calc(100vh - 8rem); padding: 1.5rem; background: #f8fafc; }
        .paper-shell { display: flex; flex-direction: column; gap: 1.25rem; }
        .paper-hero, .paper-filter, .paper-card, .empty-data { border: 1px solid rgba(148, 163, 184, 0.18); border-radius: 1.25rem; background: #ffffff; box-shadow: 0 12px 32px -28px rgba(15, 23, 42, 0.28); }
        .paper-hero { display: flex; justify-content: space-between; align-items: flex-start; gap: 1rem; padding: 1.5rem; background: linear-gradient(135deg, #ffffff 0%, #f8fafc 100%); }
        .paper-title-main { margin: 0; color: #0f172a; font-size: 1.625rem; font-weight: 700; }
        .paper-subtitle { margin: 0.75rem 0 0; color: #475569; font-size: 0.95rem; line-height: 1.7; }
        .hero-actions { display: flex; flex-wrap: wrap; gap: 0.75rem; }
        .page-btn, .hero-actions input, .empty-data input { display: inline-flex; align-items: center; justify-content: center; min-height: 2.75rem; padding: 0 1rem; border: 1px solid transparent; border-radius: 0.9rem; font-size: 0.875rem; font-weight: 600; text-decoration: none; cursor: pointer; transition: all 0.2s ease; }
        .page-btn-primary, .hero-actions input, .empty-data input { background: #2563eb; color: #ffffff; box-shadow: 0 10px 20px -14px rgba(37, 99, 235, 0.85); }
        .page-btn-secondary { background: #ffffff; color: #475569; border-color: #cbd5e1; }
        .page-btn-secondary:hover { color: #1e293b; border-color: #94a3b8; background: #f8fafc; }
        .paper-filter { padding: 1rem; }
        .paper-filter select { min-height: 2.75rem; padding: 0 0.9rem; border: 1px solid #cbd5e1; border-radius: 0.9rem; background: #f8fafc; color: #0f172a; }
        .paper-list { display: grid; gap: 1rem; }
        .paper-card { padding: 1.25rem; }
        .paper-card-row { display: flex; justify-content: space-between; align-items: flex-start; gap: 1rem; }
        .paper-badge { display: inline-flex; align-items: center; justify-content: center; padding: 0.3rem 0.75rem; border-radius: 999px; font-size: 0.75rem; font-weight: 700; margin-bottom: 0.8rem; }
        .status-0 { background: #e2e8f0; color: #475569; }
        .status-1 { background: #dbeafe; color: #1d4ed8; }
        .paper-title { font-size: 1rem; font-weight: 700; color: #0f172a; margin-bottom: 0.55rem; }
        .paper-info { font-size: 0.84rem; color: #64748b; margin-bottom: 0.35rem; line-height: 1.7; }
        .paper-actions { display: flex; flex-wrap: wrap; gap: 0.6rem; }
        .paper-actions a, .paper-actions input { display: inline-flex; align-items: center; justify-content: center; padding: 0.45rem 0.85rem; border-radius: 999px; font-size: 0.78rem; font-weight: 600; text-decoration: none; cursor: pointer; }
        .btn-outline-primary { background: #eff6ff; border: 1px solid #dbeafe; color: #2563eb; }
        .btn-outline-danger { background: #fef2f2; border: 1px solid #fecaca; color: #dc2626; }
        .empty-data { padding: 3rem 1.5rem; text-align: center; color: #94a3b8; }
        @media (max-width: 900px) { .paper-page { padding: 1rem; } .paper-hero, .paper-card-row { flex-direction: column; } }
    </style>

    <div class="paper-page">
        <div class="paper-shell">
            <section class="paper-hero">
                <div>
                    <h2 class="paper-title-main">试卷管理</h2>
                    <p class="paper-subtitle">集中管理考试试卷、题量、总分与发布时间，视觉风格与考试页和教师端管理页统一。</p>
                </div>
                <div class="hero-actions">
                    <asp:Button ID="btnAdd" runat="server" Text="添加试卷" CssClass="page-btn page-btn-primary" OnClick="btnAdd_Click" />
                    <a href="../examlist.aspx" class="page-btn page-btn-secondary">返回</a>
                </div>
            </section>

            <section class="paper-filter">
                <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                    <asp:ListItem Value="-1">全部状态</asp:ListItem>
                    <asp:ListItem Value="0">未发布</asp:ListItem>
                    <asp:ListItem Value="1">已发布</asp:ListItem>
                </asp:DropDownList>
            </section>

            <div class="paper-list">
            <asp:Repeater ID="rptPapers" runat="server" OnItemCommand="rptPapers_ItemCommand">
                <ItemTemplate>
                    <div class="paper-card">
                        <div class="paper-card-row">
                            <div>
                                <span class="paper-badge status-<%# Eval("Status") %>"><%# GetStatusName(Eval("Status")) %></span>
                                <div class="paper-title"><%# Eval("PaperName") %></div>
                                <div class="paper-info">题目数：<%# Eval("QuestionCount") %> | 总分：<%# Eval("TotalScore") %>分 | 时长：<%# Eval("Duration") %>分钟 | 类型：<%# GetPaperTypeName(Eval("PaperType")) %></div>
                                <div class="paper-info">创建时间：<%# Eval("CreateTime", "{0:yyyy-MM-dd HH:mm}") %></div>
                            </div>
                            <div class="paper-actions">
                                <asp:LinkButton ID="lnkEdit" runat="server" CssClass="btn-outline-primary" CommandName="Edit" CommandArgument='<%# Eval("PaperId") %>'>编辑</asp:LinkButton>
                                <asp:LinkButton ID="lnkDelete" runat="server" CssClass="btn-outline-danger" CommandName="Delete" CommandArgument='<%# Eval("PaperId") %>' OnClientClick="return confirm('确定删除此试卷？')">删除</asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            </div>

            <asp:Panel ID="pnlEmpty" runat="server" CssClass="empty-data" Visible="false">
                <p>暂无试卷数据</p>
                <asp:Button ID="btnAddEmpty" runat="server" Text="添加试卷" CssClass="page-btn page-btn-primary" OnClick="btnAdd_Click" />
            </asp:Panel>
        </div>
    </div>
</asp:Content>
