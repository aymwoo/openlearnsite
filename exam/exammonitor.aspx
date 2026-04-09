<%@ Page Language="C#" AutoEventWireup="true" CodeFile="exammonitor.aspx.cs" Inherits="exam_exammonitor" MasterPageFile="~/teacher/Teach.master" %><asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .monitor-page { min-height: calc(100vh - 8rem); padding: 1.5rem; background: #f8fafc; }
        .monitor-shell { display: flex; flex-direction: column; gap: 1.25rem; }
        .monitor-hero, .monitor-card, .student-list, .realtime-panel, .stat-card { border: 1px solid rgba(148, 163, 184, 0.18); border-radius: 1.25rem; background: #ffffff; box-shadow: 0 12px 32px -28px rgba(15, 23, 42, 0.28); }
        .monitor-hero { display: flex; justify-content: space-between; align-items: flex-start; gap: 1rem; padding: 1.5rem; background: linear-gradient(135deg, #ffffff 0%, #f8fafc 100%); }
        .monitor-title { margin: 0; color: #0f172a; font-size: 1.625rem; font-weight: 700; line-height: 1.2; }
        .monitor-subtitle { margin: 0.75rem 0 0; color: #475569; font-size: 0.95rem; line-height: 1.7; }
        .hero-actions { display: flex; flex-wrap: wrap; gap: 0.75rem; }
        .page-btn, .hero-actions input { display: inline-flex; align-items: center; justify-content: center; min-height: 2.75rem; padding: 0 1rem; border: 1px solid transparent; border-radius: 0.9rem; font-size: 0.875rem; font-weight: 600; text-decoration: none; cursor: pointer; transition: all 0.2s ease; }
        .page-btn-secondary { background: #ffffff; color: #475569; border-color: #cbd5e1; }
        .page-btn-secondary:hover { color: #1e293b; border-color: #94a3b8; background: #f8fafc; }
        .hero-actions input, .page-btn-primary { background: #2563eb; color: #ffffff; box-shadow: 0 10px 20px -14px rgba(37, 99, 235, 0.85); }
        .hero-actions input:hover, .page-btn-primary:hover { background: #1d4ed8; }
        .stats-cards { display: grid; grid-template-columns: repeat(4, minmax(0, 1fr)); gap: 1rem; }
        .stat-card { padding: 1.25rem; text-align: center; }
        .stat-card .value { font-size: 2rem; font-weight: 700; color: #2563eb; }
        .stat-card .label { margin-top: 0.45rem; color: #64748b; font-size: 0.85rem; }
        .stat-card.warning .value { color: #d97706; }
        .stat-card.success .value { color: #15803d; }
        .monitor-content { display: grid; grid-template-columns: minmax(0, 2fr) minmax(280px, 1fr); gap: 1rem; }
        .student-list, .realtime-panel { padding: 1.25rem; }
        .student-list h3, .realtime-panel h3 { margin: 0 0 1rem; color: #0f172a; font-size: 1rem; font-weight: 700; }
        .filter-bar { display: flex; flex-wrap: wrap; align-items: center; gap: 0.75rem; margin-bottom: 1rem; }
        .filter-bar select, .filter-bar input[type='text'] { min-height: 2.5rem; padding: 0 0.9rem; border: 1px solid #cbd5e1; border-radius: 0.9rem; background: #f8fafc; color: #0f172a; }
        .filter-bar label { display: inline-flex; align-items: center; gap: 0.45rem; padding: 0.55rem 0.8rem; border-radius: 999px; background: #eff6ff; color: #1d4ed8; }
        .table-wrap { overflow-x: auto; }
        .student-table { width: 100%; border-collapse: separate; border-spacing: 0; min-width: 760px; }
        .student-table th, .student-table td { padding: 0.9rem 1rem; text-align: left; border-bottom: 1px solid #e2e8f0; font-size: 0.85rem; }
        .student-table th { background: #f8fafc; color: #475569; font-weight: 700; text-transform: uppercase; letter-spacing: 0.04em; font-size: 0.76rem; }
        .status-badge { display: inline-flex; align-items: center; justify-content: center; padding: 0.3rem 0.75rem; border-radius: 999px; font-size: 0.75rem; font-weight: 700; }
        .status-answering { background: #dbeafe; color: #1d4ed8; }
        .status-submitted { background: #dcfce7; color: #15803d; }
        .progress-bar { height: 0.5rem; background: #e2e8f0; border-radius: 999px; overflow: hidden; }
        .progress-bar .progress { height: 100%; background: linear-gradient(90deg, #60a5fa 0%, #2563eb 100%); transition: width 0.3s; }
        .realtime-item { padding: 0.85rem 0; border-bottom: 1px solid #e2e8f0; font-size: 0.85rem; color: #334155; }
        .realtime-item:last-child { border-bottom: 0; }
        .realtime-item .time { margin-top: 0.3rem; color: #94a3b8; font-size: 0.75rem; }
        @media (max-width: 1100px) {
            .stats-cards { grid-template-columns: repeat(2, minmax(0, 1fr)); }
            .monitor-content { grid-template-columns: 1fr; }
        }
        @media (max-width: 900px) {
            .monitor-page { padding: 1rem; }
            .monitor-hero { flex-direction: column; }
            .stats-cards { grid-template-columns: 1fr; }
        }
    </style>
    <script type="text/javascript">
        function autoRefresh() {
            var chk = document.getElementById('<%= chkAutoRefresh.ClientID %>');
            if (chk && chk.checked) {
                setTimeout(function () {
                    __doPostBack('Refresh', '');
                }, 30000);
            }
        }
    </script>
</asp:Content><asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div class="monitor-page">
        <div class="monitor-shell">
        <section class="monitor-hero">
            <div>
                <h2 class="monitor-title"><asp:Literal ID="ltlExamName" runat="server"></asp:Literal></h2>
                <p class="monitor-subtitle">考试时间：<asp:Literal ID="ltlExamTime" runat="server"></asp:Literal></p>
            </div>
            <div class="hero-actions">
                <asp:Button ID="btnRefresh" runat="server" Text="刷新" CssClass="page-btn page-btn-primary" OnClick="btnRefresh_Click" />
                <a href="examlist.aspx" class="page-btn page-btn-secondary">返回列表</a>
            </div>
        </section>

        <div class="stats-cards">
            <div class="stat-card">
                <div class="value"><asp:Literal ID="ltlTotalCount" runat="server">0</asp:Literal></div>
                <div class="label">应考人数</div>
            </div>
            <div class="stat-card">
                <div class="value"><asp:Literal ID="ltlAnsweringCount" runat="server">0</asp:Literal></div>
                <div class="label">答题中</div>
            </div>
            <div class="stat-card success">
                <div class="value"><asp:Literal ID="ltlSubmittedCount" runat="server">0</asp:Literal></div>
                <div class="label">已提交</div>
            </div>
            <div class="stat-card warning">
                <div class="value"><asp:Literal ID="ltlAvgScore" runat="server">0</asp:Literal></div>
                <div class="label">平均分</div>
            </div>
        </div>

        <div class="monitor-content">
            <div class="student-list">
                <h3>学生状态</h3>
                <div class="filter-bar">
                    <asp:DropDownList ID="ddlClass" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlClass_SelectedIndexChanged">
                        <asp:ListItem Value="">全部班级</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                        <asp:ListItem Value="">全部状态</asp:ListItem>
                        <asp:ListItem Value="0">答题中</asp:ListItem>
                        <asp:ListItem Value="1">已提交</asp:ListItem>
                    </asp:DropDownList>
                    <label style="margin-left:15px;">
                        <asp:CheckBox ID="chkAutoRefresh" runat="server" Checked="true" />
                        自动刷新（30秒）
                    </label>
                </div>
                <div class="table-wrap">
                <asp:Repeater ID="rptStudents" runat="server">
                    <HeaderTemplate>
                        <table class="student-table">
                            <thead>
                                <tr>
                                    <th>学号</th>
                                    <th>姓名</th>
                                    <th>班级</th>
                                    <th>状态</th>
                                    <th>答题进度</th>
                                    <th>分数</th>
                                    <th>用时</th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("StudentId") %></td>
                            <td><%# Eval("StudentName") %></td>
                            <td><%# Eval("ClassName") %></td>
                            <td>
                                <span class="status-badge status-<%# (int)Eval("Status") == 0 ? "answering" : "submitted" %>">
                                    <%# (int)Eval("Status") == 0 ? "答题中" : "已提交" %>
                                </span>
                            </td>
                            <td>
                                <div class="progress-bar">
                                    <div class="progress" style="width:<%# GetProgress(Eval("Answers")) %>%"></div>
                                </div>
                            </td>
                            <td><%# Eval("TotalScore") %></td>
                            <td><%# FormatDuration(Eval("Duration")) %></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                            </tbody>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
                </div>
            </div>

            <div class="realtime-panel">
                <h3>实时动态</h3>
                <asp:Repeater ID="rptRealtime" runat="server">
                    <ItemTemplate>
                        <div class="realtime-item">
                            <div><%# Eval("StudentName") %> <%# Eval("Action") %></div>
                            <div class="time"><%# Eval("Time") %></div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
        </div>
    </div>
    <script type="text/javascript">autoRefresh();</script>
</asp:Content>
