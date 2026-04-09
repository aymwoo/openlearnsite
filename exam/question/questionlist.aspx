<%@ Page Language="C#" AutoEventWireup="true" CodeFile="questionlist.aspx.cs" Inherits="exam_question_questionlist" MasterPageFile="~/teacher/Teach.master" %><asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- 使用绝对路径重新引用JS文件，覆盖Master中的相对路径 -->
    <script src="/js/MenuCookie.js" type="text/javascript"></script>
    <script src="/js/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/kindeditor/plugins/code/prettify.js" type="text/javascript"></script>
    <script src="/js/ruffle.js" type="text/javascript"></script>
</asp:Content><asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <style>
        .question-page { min-height: calc(100vh - 8rem); padding: 1.5rem; background: #f8fafc; }
        .question-shell { display: flex; flex-direction: column; gap: 1.25rem; }
        .question-hero, .filter-card, .table-card, .select-bar { border: 1px solid rgba(148, 163, 184, 0.18); border-radius: 1.25rem; background: #ffffff; box-shadow: 0 12px 32px -28px rgba(15, 23, 42, 0.28); }
        .question-hero { display: flex; justify-content: space-between; align-items: flex-start; gap: 1rem; padding: 1.5rem; background: linear-gradient(135deg, #ffffff 0%, #f8fafc 100%); }
        .question-title { margin: 0; color: #0f172a; font-size: 1.625rem; font-weight: 700; }
        .question-subtitle { margin: 0.75rem 0 0; color: #475569; font-size: 0.95rem; line-height: 1.7; }
        .hero-actions { display: flex; flex-wrap: wrap; gap: 0.75rem; }
        .page-btn, .hero-actions input { display: inline-flex; align-items: center; justify-content: center; min-height: 2.75rem; padding: 0 1rem; border: 1px solid transparent; border-radius: 0.9rem; font-size: 0.875rem; font-weight: 600; text-decoration: none; cursor: pointer; transition: all 0.2s ease; }
        .page-btn-primary { background: #2563eb; color: #ffffff; box-shadow: 0 10px 20px -14px rgba(37, 99, 235, 0.85); }
        .page-btn-success, .hero-actions input { background: #16a34a; color: #ffffff; box-shadow: 0 10px 20px -14px rgba(22, 163, 74, 0.9); }
        .page-btn-secondary { background: #ffffff; color: #475569; border-color: #cbd5e1; }
        .filter-card { padding: 1rem; }
        .filter-bar { display: flex; flex-wrap: wrap; align-items: center; gap: 0.75rem; }
        .filter-bar select, .filter-bar input { min-height: 2.75rem; padding: 0 0.9rem; border: 1px solid #cbd5e1; border-radius: 0.9rem; background: #f8fafc; color: #0f172a; }
        .filter-bar input[type='submit'] { background: #2563eb; color: #ffffff; border-color: transparent; box-shadow: 0 10px 20px -14px rgba(37, 99, 235, 0.85); }
        .select-bar { padding: 1rem 1.25rem; background: linear-gradient(135deg, #eff6ff 0%, #f8fbff 100%); }
        .select-bar .selected-info { color: #1d4ed8; font-weight: 700; }
        .table-card { padding: 0; overflow: hidden; }
        .table-wrap { overflow-x: auto; }
        .question-table { width: 100%; border-collapse: separate; border-spacing: 0; min-width: 980px; }
        .question-table th, .question-table td { padding: 0.95rem 1rem; text-align: left; border-bottom: 1px solid #e2e8f0; }
        .question-table th { background: #f8fafc; color: #475569; font-size: 0.76rem; font-weight: 700; text-transform: uppercase; letter-spacing: 0.04em; }
        .question-table tr:hover td { background: #f8fafc; }
        .type-tag { display: inline-flex; align-items: center; justify-content: center; padding: 0.28rem 0.75rem; border-radius: 999px; font-size: 0.75rem; font-weight: 700; }
        .type-1 { background: #dbeafe; color: #1d4ed8; }
        .type-2 { background: #dcfce7; color: #15803d; }
        .type-3 { background: #fef3c7; color: #b45309; }
        .type-4 { background: #ede9fe; color: #6d28d9; }
        .type-5 { background: #fee2e2; color: #dc2626; }
        .difficulty-1 { color: #15803d; font-weight: 700; }
        .difficulty-2 { color: #d97706; font-weight: 700; }
        .difficulty-3 { color: #dc2626; font-weight: 700; }
        .question-content { max-width: 420px; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; color: #334155; }
        .actions a, .actions input { display: inline-flex; align-items: center; justify-content: center; margin-right: 0.55rem; padding: 0.38rem 0.75rem; border-radius: 999px; text-decoration: none; font-size: 0.78rem; font-weight: 600; }
        .actions a, .btn-select { background: #eff6ff; color: #2563eb; border: 1px solid #dbeafe; }
        .actions .danger { background: #fef2f2; color: #dc2626; border: 1px solid #fecaca; }
        .pagination { padding: 1rem 1.25rem; text-align: center; }
        .pagination a, .pagination span { display: inline-flex; align-items: center; justify-content: center; min-width: 2.2rem; height: 2.2rem; padding: 0 0.7rem; margin: 0 0.15rem; border: 1px solid #cbd5e1; border-radius: 0.8rem; color: #475569; text-decoration: none; }
        .pagination .current { background: #2563eb; border-color: #2563eb; color: #ffffff; }
        @media (max-width: 900px) { .question-page { padding: 1rem; } .question-hero { flex-direction: column; } }
    </style>

    <div class="question-page">
        <div class="question-shell">
            <section class="question-hero">
                <div>
                    <h2 class="question-title"><asp:Literal ID="ltlBankName" runat="server"></asp:Literal> - 题目管理</h2>
                    <p class="question-subtitle">集中管理题库中的题目内容、难度、分值与正确率，也支持从试卷编辑流程中直接选题。</p>
                </div>
                <div>
                    <asp:Panel ID="pnlNormalButtons" runat="server" CssClass="hero-actions">
                        <a href="questionadd.aspx?bankId=<%= BankId %>" class="page-btn page-btn-primary">+ 添加题目</a>
                        <a href="questionimport.aspx?bankId=<%= BankId %>" class="page-btn page-btn-success">批量导入</a>
                        <a href="banklist.aspx" class="page-btn page-btn-secondary">返回题库</a>
                    </asp:Panel>
                    <asp:Panel ID="pnlSelectButtons" runat="server" Visible="false" CssClass="hero-actions">
                        <asp:Button ID="btnConfirmSelect" runat="server" Text="确认选择" CssClass="page-btn page-btn-success" OnClick="btnConfirmSelect_Click" />
                        <a href="../paper/paperadd.aspx" class="page-btn page-btn-secondary">取消</a>
                    </asp:Panel>
                </div>
            </section>

            <asp:Panel ID="pnlSelectBar" runat="server" Visible="false" CssClass="select-bar">
                <span>已选择 <span class="selected-info" id="selectedCount">0</span> 道题目</span>
                <input type="hidden" id="selectedIds" name="selectedIds" runat="server" />
            </asp:Panel>

            <section class="filter-card">
                <div class="filter-bar">
                    <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Filter_Changed">
                        <asp:ListItem Value="">全部题型</asp:ListItem>
                        <asp:ListItem Value="1">单选题</asp:ListItem>
                        <asp:ListItem Value="2">多选题</asp:ListItem>
                        <asp:ListItem Value="3">判断题</asp:ListItem>
                        <asp:ListItem Value="4">填空题</asp:ListItem>
                        <asp:ListItem Value="5">简答题</asp:ListItem>
                        <asp:ListItem Value="6">连线题</asp:ListItem>
                        <asp:ListItem Value="7">分类题</asp:ListItem>
                        <asp:ListItem Value="8">组合题</asp:ListItem>
                        <asp:ListItem Value="9">多项填空</asp:ListItem>
                        <asp:ListItem Value="10">下拉选择</asp:ListItem>
                        <asp:ListItem Value="11">打分题</asp:ListItem>
                        <asp:ListItem Value="12">矩阵单选</asp:ListItem>
                        <asp:ListItem Value="13">矩阵多选</asp:ListItem>
                        <asp:ListItem Value="14">NPS评分</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlDifficulty" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Filter_Changed">
                        <asp:ListItem Value="">全部难度</asp:ListItem>
                        <asp:ListItem Value="1">简单</asp:ListItem>
                        <asp:ListItem Value="2">中等</asp:ListItem>
                        <asp:ListItem Value="3">困难</asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox ID="txtKeyword" runat="server" placeholder="搜索题目内容..."></asp:TextBox>
                    <asp:Button ID="btnSearch" runat="server" Text="搜索" OnClick="btnSearch_Click" />
                </div>
            </section>

            <section class="table-card">
                <div class="table-wrap">
                <asp:Repeater ID="rptQuestions" runat="server" OnItemCommand="rptQuestions_ItemCommand" OnItemDataBound="rptQuestions_ItemDataBound">
                    <HeaderTemplate>
                        <table class="question-table">
                            <thead>
                                <tr>
                                    <th style="width:5%"><asp:Literal ID="ltlSelectHeader" runat="server"></asp:Literal></th>
                                    <th style="width:5%">ID</th>
                                    <th style="width:10%">题型</th>
                                    <th style="width:40%">题目内容</th>
                                    <th style="width:10%">难度</th>
                                    <th style="width:8%">分值</th>
                                    <th style="width:10%">正确率</th>
                                    <th style="width:10%">操作</th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><asp:Literal ID="ltlCheckbox" runat="server"></asp:Literal></td>
                            <td><%# Eval("QuestionId") %></td>
                            <td><span class="type-tag type-<%# Eval("QuestionType") %>"><%# GetTypeName(Eval("QuestionType")) %></span></td>
                            <td class="question-content" title="<%# Eval("QuestionText") %>"><%# Eval("QuestionText") %></td>
                            <td class="difficulty-<%# Eval("Difficulty") %>"><%# GetDifficultyName(Eval("Difficulty")) %></td>
                            <td><%# Eval("Score") %>分</td>
                            <td><%# Eval("CorrectRate") %>%</td>
                            <td class="actions">
                                <asp:Panel ID="pnlNormalActions" runat="server">
                                    <a href="questionadd.aspx?id=<%# Eval("QuestionId") %>&bankId=<%# BankId %>">编辑</a>
                                    <asp:LinkButton ID="lbtnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("QuestionId") %>' CssClass="danger" OnClientClick="return confirm('确定删除此题目吗？');">删除</asp:LinkButton>
                                </asp:Panel>
                                <asp:Panel ID="pnlSelectActions" runat="server" Visible="false">
                                    <a href="javascript:void(0)" onclick="toggleSelect(<%# Eval("QuestionId") %>, this)" class="btn-select">选择</a>
                                </asp:Panel>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                            </tbody>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
                </div>

                <div class="pagination">
                    <asp:Literal ID="ltlPagination" runat="server"></asp:Literal>
                </div>
            </section>
        </div>
    </div>

    <script>
        var selectedQuestions = [];
        
        function toggleSelect(questionId, btn) {
            var index = selectedQuestions.indexOf(questionId);
            if (index > -1) {
                selectedQuestions.splice(index, 1);
                btn.innerText = '选择';
                btn.style.color = '#1890ff';
            } else {
                selectedQuestions.push(questionId);
                btn.innerText = '已选';
                btn.style.color = '#52c41a';
            }
            document.getElementById('selectedCount').innerText = selectedQuestions.length;
            document.getElementById('<%= selectedIds.ClientID %>').value = selectedQuestions.join(',');
        }
    </script>
</asp:Content>
