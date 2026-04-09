<%@ Page Language="C#" AutoEventWireup="true" CodeFile="paperadd.aspx.cs" Inherits="exam_paper_paperadd" MasterPageFile="~/teacher/Teach.master" %><asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/js/MenuCookie.js" type="text/javascript"></script>
    <script src="/js/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/kindeditor/plugins/code/prettify.js" type="text/javascript"></script>
    <script src="/js/ruffle.js" type="text/javascript"></script>
</asp:Content><asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <style>
        .paper-form-page { min-height: calc(100vh - 8rem); padding: 1.5rem; background: #f8fafc; }
        .paper-form-shell { max-width: 1100px; margin: 0 auto; display: flex; flex-direction: column; gap: 1.25rem; }
        .form-hero, .form-card, .question-item, .empty-tip { border: 1px solid rgba(148, 163, 184, 0.18); border-radius: 1.25rem; background: #ffffff; box-shadow: 0 12px 32px -28px rgba(15, 23, 42, 0.28); }
        .form-hero { display: flex; justify-content: space-between; align-items: flex-start; gap: 1rem; padding: 1.5rem; background: linear-gradient(135deg, #ffffff 0%, #f8fafc 100%); }
        .form-title { margin: 0; color: #0f172a; font-size: 1.625rem; font-weight: 700; }
        .form-subtitle { margin: 0.75rem 0 0; color: #475569; font-size: 0.95rem; line-height: 1.7; }
        .hero-actions { display: flex; flex-wrap: wrap; gap: 0.75rem; }
        .page-btn, .form-actions input, .hero-actions a, .question-header input { display: inline-flex; align-items: center; justify-content: center; min-height: 2.75rem; padding: 0 1rem; border: 1px solid transparent; border-radius: 0.9rem; font-size: 0.875rem; font-weight: 600; text-decoration: none; cursor: pointer; transition: all 0.2s ease; }
        .page-btn-primary, .form-actions input[id$='btnSave'] { background: #2563eb; color: #ffffff; box-shadow: 0 10px 20px -14px rgba(37, 99, 235, 0.85); }
        .page-btn-secondary, .form-actions input[id$='btnCancel'], .hero-actions a { background: #ffffff; color: #475569; border-color: #cbd5e1; }
        .btn-outline-primary, .question-header input { background: #eff6ff; color: #2563eb; border: 1px solid #dbeafe; }
        .form-card { padding: 1.5rem; }
        .section-title { margin: 0 0 1rem; color: #0f172a; font-size: 1rem; font-weight: 700; }
        .form-group { margin-bottom: 1rem; }
        .form-group label { display: block; margin-bottom: 0.45rem; font-weight: 600; color: #334155; }
        .form-group label span.required { color: #dc2626; }
        .form-control { width: 100%; min-height: 2.75rem; padding: 0.7rem 0.9rem; border: 1px solid #cbd5e1; border-radius: 0.9rem; box-sizing: border-box; background: #f8fafc; color: #0f172a; }
        .form-row { display: flex; gap: 1rem; margin-bottom: 1rem; }
        .form-row .form-group { flex: 1; }
        textarea.form-control { min-height: 7rem; resize: vertical; }
        .question-section { margin-top: 0.5rem; }
        .question-header { display: flex; justify-content: space-between; align-items: center; gap: 1rem; margin-bottom: 1rem; }
        .question-header h4 { margin: 0; color: #0f172a; font-size: 1rem; }
        .question-item { padding: 1rem 1.25rem; margin-bottom: 0.85rem; background: #ffffff; }
        .question-content { margin-bottom: 0.65rem; color: #334155; }
        .question-meta { font-size: 0.78rem; color: #94a3b8; }
        .question-actions { text-align: right; margin-top: 0.75rem; }
        .question-actions .btn { padding: 0.4rem 0.8rem; font-size: 0.78rem; }
        .empty-tip { text-align: center; padding: 3rem 1.5rem; color: #94a3b8; background: #f8fafc; }
        .form-actions { display: flex; justify-content: space-between; align-items: center; gap: 1rem; margin-top: 0.75rem; }
        .score-info { color: #64748b; font-size: 0.9rem; }
        @media (max-width: 900px) { .paper-form-page { padding: 1rem; } .form-hero, .form-row, .question-header, .form-actions { flex-direction: column; align-items: stretch; } }
    </style>

    <div class="paper-form-page">
        <div class="paper-form-shell">
        <section class="form-hero">
            <div>
                <h2 class="form-title">添加试卷</h2>
                <p class="form-subtitle">设置试卷基础信息、分值和题目组成，整体风格与考试管理页、题库页保持一致。</p>
            </div>
            <div class="hero-actions">
                <a href="paperlist.aspx" class="page-btn page-btn-secondary">返回列表</a>
            </div>
        </section>

        <section class="form-card">
        <h3 class="section-title">基础设置</h3>

        <div class="form-row">
            <div class="form-group" style="flex: 2;">
                <label><span class="required">*</span> 试卷名称</label>
                <asp:TextBox ID="txtPaperName" runat="server" CssClass="form-control" placeholder="请输入试卷名称"></asp:TextBox>
            </div>
            <div class="form-group" style="flex: 1;">
                <label>试卷类型</label>
                <asp:DropDownList ID="ddlPaperType" runat="server" CssClass="form-control">
                    <asp:ListItem Value="1">普通试卷</asp:ListItem>
                    <asp:ListItem Value="2">随机试卷</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>

        <div class="form-row">
            <div class="form-group">
                <label>总分</label>
                <asp:TextBox ID="txtTotalScore" runat="server" CssClass="form-control" Text="100"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>及格分</label>
                <asp:TextBox ID="txtPassScore" runat="server" CssClass="form-control" Text="60"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>建议时长(分钟)</label>
                <asp:TextBox ID="txtDuration" runat="server" CssClass="form-control" Text="60"></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <label>试卷说明</label>
            <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" placeholder="请输入试卷说明"></asp:TextBox>
        </div>
        </section>

        <section class="form-card">
        <div class="question-section">
            <div class="question-header">
                <h4>试题列表</h4>
                <asp:Button ID="btnAddQuestion" runat="server" Text="从题库添加题目" CssClass="page-btn btn-outline-primary" OnClick="btnAddQuestion_Click" />
            </div>

            <asp:Literal ID="ltlQuestionList" runat="server"></asp:Literal>

            <asp:Panel ID="pnlNoQuestion" runat="server" CssClass="empty-tip">
                <p>暂无题目，请点击"从题库添加题目"按钮添加试题</p>
            </asp:Panel>
        </div>

        <div class="form-actions">
            <div class="score-info">
                共 <asp:Label ID="lblQuestionCount" runat="server" Text="0"></asp:Label> 题，总分 <asp:Label ID="lblScoreSum" runat="server" Text="0"></asp:Label> 分
            </div>
            <div>
                <asp:Button ID="btnSave" runat="server" Text="保存试卷" CssClass="page-btn page-btn-primary" OnClick="btnSave_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="取消" CssClass="page-btn page-btn-secondary" OnClick="btnCancel_Click" />
                <a href="paperlist.aspx" class="page-btn page-btn-secondary">返回</a>
            </div>
        </div>
        </section>
        </div>
    </div>
</asp:Content>
