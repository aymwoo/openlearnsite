<%@ Page Language="C#" AutoEventWireup="true" CodeFile="examresult.aspx.cs" Inherits="exam_examresult" MasterPageFile="~/teacher/Teach.master" %><asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <style>
        .result-page { min-height: calc(100vh - 8rem); padding: 1.5rem; background: #f8fafc; }
        .result-shell { display: flex; flex-direction: column; gap: 1.25rem; }
        .result-hero, .stat-item, .score-distribution, .result-table-card { border: 1px solid rgba(148, 163, 184, 0.18); border-radius: 1.25rem; background: #ffffff; box-shadow: 0 12px 32px -28px rgba(15, 23, 42, 0.28); }
        .result-hero { display: flex; justify-content: space-between; align-items: flex-start; gap: 1rem; padding: 1.5rem; background: linear-gradient(135deg, #ffffff 0%, #f8fafc 100%); }
        .result-title { margin: 0; color: #0f172a; font-size: 1.625rem; font-weight: 700; line-height: 1.2; }
        .result-subtitle { margin: 0.75rem 0 0; color: #475569; font-size: 0.95rem; line-height: 1.7; }
        .hero-actions { display: flex; flex-wrap: wrap; gap: 0.75rem; }
        .page-btn, .hero-actions input { display: inline-flex; align-items: center; justify-content: center; min-height: 2.75rem; padding: 0 1rem; border: 1px solid transparent; border-radius: 0.9rem; font-size: 0.875rem; font-weight: 600; text-decoration: none; cursor: pointer; transition: all 0.2s ease; }
        .page-btn-secondary { background: #ffffff; color: #475569; border-color: #cbd5e1; }
        .page-btn-secondary:hover { color: #1e293b; border-color: #94a3b8; background: #f8fafc; }
        .hero-actions input { background: #16a34a; color: #ffffff; box-shadow: 0 10px 20px -14px rgba(22, 163, 74, 0.9); }
        .hero-actions input:hover { background: #15803d; }
        .stats-summary { display: grid; grid-template-columns: repeat(5, minmax(0, 1fr)); gap: 1rem; }
        .stat-item { padding: 1.1rem; text-align: center; }
        .stat-item .value { font-size: 1.85rem; font-weight: 700; color: #2563eb; }
        .stat-item .label { margin-top: 0.45rem; color: #64748b; font-size: 0.84rem; }
        .score-distribution { padding: 1.25rem; }
        .score-distribution h3 { margin: 0 0 1rem; color: #0f172a; font-size: 1rem; font-weight: 700; }
        .bar-chart { display: flex; align-items: flex-end; min-height: 180px; gap: 0.9rem; padding: 0 0.5rem; }
        .bar-item { flex: 1; display: flex; flex-direction: column; align-items: center; }
        .bar { width: 100%; background: linear-gradient(180deg, #60a5fa 0%, #2563eb 100%); border-radius: 0.85rem 0.85rem 0 0; min-height: 4px; }
        .bar-label { margin-top: 0.5rem; font-size: 0.76rem; color: #64748b; }
        .bar-value { margin-top: 0.3rem; font-size: 0.76rem; color: #2563eb; font-weight: 700; }
        .result-table-card { padding: 1.25rem; }
        .filter-bar { display: flex; flex-wrap: wrap; align-items: center; gap: 0.75rem; margin-bottom: 1rem; }
        .filter-bar select, .filter-bar input { min-height: 2.75rem; padding: 0 0.9rem; border: 1px solid #cbd5e1; border-radius: 0.9rem; background: #f8fafc; color: #0f172a; }
        .filter-bar input[type='submit'] { background: #2563eb; color: #ffffff; border-color: transparent; box-shadow: 0 10px 20px -14px rgba(37, 99, 235, 0.85); }
        .table-wrap { overflow-x: auto; }
        .result-table { width: 100%; border-collapse: separate; border-spacing: 0; min-width: 980px; }
        .result-table th, .result-table td { padding: 0.9rem 1rem; text-align: left; border-bottom: 1px solid #e2e8f0; }
        .result-table th { background: #f8fafc; color: #475569; font-size: 0.76rem; font-weight: 700; text-transform: uppercase; letter-spacing: 0.04em; }
        .result-table tr:hover td { background: #f8fafc; }
        .rank-badge { display: inline-flex; align-items: center; justify-content: center; min-width: 1.8rem; padding: 0.2rem 0.45rem; border-radius: 999px; font-size: 0.75rem; font-weight: 700; }
        .rank-1 { background: #fde68a; color: #92400e; }
        .rank-2 { background: #e2e8f0; color: #334155; }
        .rank-3 { background: #fed7aa; color: #9a3412; }
        .score-pass { color: #15803d; font-weight: 700; }
        .score-fail { color: #dc2626; font-weight: 700; }
        .actions a { display: inline-flex; align-items: center; justify-content: center; padding: 0.4rem 0.8rem; border: 1px solid #dbeafe; border-radius: 999px; background: #eff6ff; color: #2563eb; font-size: 0.78rem; font-weight: 600; text-decoration: none; }
        .actions a:hover { background: #dbeafe; color: #1d4ed8; }
        @media (max-width: 1100px) { .stats-summary { grid-template-columns: repeat(2, minmax(0, 1fr)); } }
        @media (max-width: 900px) {
            .result-page { padding: 1rem; }
            .result-hero { flex-direction: column; }
            .stats-summary { grid-template-columns: 1fr; }
        }
    </style>

    <div class="result-page">
        <div class="result-shell">
        <section class="result-hero">
            <div>
                <h2 class="result-title"><asp:Literal ID="ltlExamName" runat="server"></asp:Literal> - 成绩管理</h2>
                <p class="result-subtitle">查看考试分布、成绩概况与学生答卷结果，整体视觉与教师端管理页保持统一。</p>
            </div>
            <div class="hero-actions">
                <asp:Button ID="btnExport" runat="server" Text="导出Excel" CssClass="page-btn" OnClick="btnExport_Click" />
                <a href="examlist.aspx" class="page-btn page-btn-secondary">返回列表</a>
            </div>
        </section>

        <div class="stats-summary">
            <div class="stat-item">
                <div class="value"><asp:Literal ID="ltlTotalCount" runat="server">0</asp:Literal></div>
                <div class="label">参考人数</div>
            </div>
            <div class="stat-item">
                <div class="value"><asp:Literal ID="ltlAvgScore" runat="server">0</asp:Literal></div>
                <div class="label">平均分</div>
            </div>
            <div class="stat-item">
                <div class="value"><asp:Literal ID="ltlMaxScore" runat="server">0</asp:Literal></div>
                <div class="label">最高分</div>
            </div>
            <div class="stat-item">
                <div class="value"><asp:Literal ID="ltlMinScore" runat="server">0</asp:Literal></div>
                <div class="label">最低分</div>
            </div>
            <div class="stat-item">
                <div class="value"><asp:Literal ID="ltlPassRate" runat="server">0</asp:Literal>%</div>
                <div class="label">及格率</div>
            </div>
        </div>

        <div class="score-distribution">
            <h3>分数段分布</h3>
            <div class="bar-chart">
                <asp:Repeater ID="rptDistribution" runat="server">
                    <ItemTemplate>
                        <div class="bar-item">
                            <div class="bar" style="height:<%# GetBarHeight(Eval("Count")) %>px;"></div>
                            <div class="bar-value"><%# Eval("Count") %></div>
                            <div class="bar-label"><%# Eval("Range") %></div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>

        <section class="result-table-card">
        <div class="filter-bar">
            <asp:DropDownList ID="ddlClass" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlClass_SelectedIndexChanged">
                <asp:ListItem Value="">全部班级</asp:ListItem>
            </asp:DropDownList>
            <asp:DropDownList ID="ddlScoreRange" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlScoreRange_SelectedIndexChanged">
                <asp:ListItem Value="">全部分数</asp:ListItem>
                <asp:ListItem Value="90">优秀(&gt;=90)</asp:ListItem>
                <asp:ListItem Value="80">良好(&gt;=80)</asp:ListItem>
                <asp:ListItem Value="60">及格(&gt;=60)</asp:ListItem>
                <asp:ListItem Value="0">不及格(&lt;60)</asp:ListItem>
            </asp:DropDownList>
            <asp:TextBox ID="txtKeyword" runat="server" placeholder="学号/姓名"></asp:TextBox>
            <asp:Button ID="btnSearch" runat="server" Text="搜索" OnClick="btnSearch_Click" />
        </div>

        <div class="table-wrap">
        <asp:Repeater ID="rptResults" runat="server">
            <HeaderTemplate>
                <table class="result-table">
                    <thead>
                        <tr>
                            <th style="width:5%">排名</th>
                            <th style="width:10%">学号</th>
                            <th style="width:10%">姓名</th>
                            <th style="width:10%">班级</th>
                            <th style="width:10%">成绩</th>
                            <th style="width:8%">客观题</th>
                            <th style="width:8%">主观题</th>
                            <th style="width:8%">用时</th>
                            <th style="width:12%">提交时间</th>
                            <th style="width:8%">操作</th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <asp:PlaceHolder runat="server" Visible='<%# (int)Eval("RankInClass") <= 3 && (int)Eval("RankInClass") > 0 %>'>
                            <span class="rank-badge rank-<%# Eval("RankInClass") %>"><%# Eval("RankInClass") %></span>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder runat="server" Visible='<%# (int)Eval("RankInClass") > 3 || (int)Eval("RankInClass") == 0 %>'>
                            <%# Eval("RankInClass") %>
                        </asp:PlaceHolder>
                    </td>
                    <td><%# Eval("StudentId") %></td>
                    <td><%# Eval("StudentName") %></td>
                    <td><%# Eval("ClassName") %></td>
                    <td>
                        <span class='<%# (decimal)Eval("TotalScore") >= (decimal)Eval("PassScore") ? "score-pass" : "score-fail" %>'>
                            <%# Eval("TotalScore") %>
                        </span>
                    </td>
                    <td><%# Eval("ObjectiveScore") %></td>
                    <td><%# Eval("SubjectiveScore") %></td>
                    <td><%# FormatDuration(Eval("Duration")) %></td>
                    <td><%# Eval("SubmitTime", "{0:MM-dd HH:mm}") %></td>
                    <td class="actions">
                        <a href="examreview.aspx?id=<%# Eval("AnswerId") %>">查看答卷</a>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                    </tbody>
                </table>
            </FooterTemplate>
        </asp:Repeater>
        </div>
        </section>
        </div>
    </div>
</asp:Content>
