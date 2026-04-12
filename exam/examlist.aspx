<%@ Page Language="C#" AutoEventWireup="true" CodeFile="examlist.aspx.cs" Inherits="exam_examlist" MasterPageFile="~/teacher/Teach.master" %><asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- 使用绝对路径重新引用JS文件，覆盖Master中的相对路径 -->
    <script src="/js/MenuCookie.js" type="text/javascript"></script>
    <script src="/js/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/kindeditor/plugins/code/prettify.js" type="text/javascript"></script>
    <script src="/js/ruffle.js" type="text/javascript"></script>
</asp:Content><asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <style>
        .exam-page {
            min-height: calc(100vh - 8rem);
            padding: 1.5rem;
            background: #f8fafc;
        }
        .exam-stack {
            display: flex;
            flex-direction: column;
            gap: 1.25rem;
        }
        .exam-hero {
            display: flex;
            justify-content: space-between;
            align-items: flex-start;
            gap: 1rem;
            padding: 1.8rem;
            border: 1px solid rgba(148, 163, 184, 0.18);
            border-radius: 1.25rem;
            background: linear-gradient(135deg, #312e81 0%, #4338ca 55%, #6366f1 100%);
            color: #ffffff;
            box-shadow: 0 12px 32px -24px rgba(15, 23, 42, 0.3);
        }
        .exam-hero-title {
            margin: 0;
            color: #ffffff;
            font-size: 1.625rem;
            font-weight: 700;
            line-height: 1.2;
        }
        .exam-hero-text {
            max-width: 42rem;
        }
        .exam-hero-subtitle {
            margin: 0.75rem 0 0;
            color: rgba(255, 255, 255, 0.85);
            font-size: 0.95rem;
            line-height: 1.7;
        }
        .exam-actions {
            display: flex;
            flex-wrap: wrap;
            justify-content: flex-end;
            gap: 0.75rem;
        }
        .exam-btn {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            min-height: 2.75rem;
            padding: 0 1rem;
            border: 1px solid transparent;
            border-radius: 0.9rem;
            font-size: 0.875rem;
            font-weight: 600;
            text-decoration: none;
            transition: all 0.2s ease;
            cursor: pointer;
        }
        .exam-btn-secondary {
            background: #ffffff;
            color: #475569;
            border-color: #cbd5e1;
        }
        .exam-btn-secondary:hover {
            color: #1e293b;
            border-color: #94a3b8;
            background: #f8fafc;
        }
        .exam-btn-primary {
            background: #2563eb;
            color: #ffffff;
            box-shadow: 0 10px 20px -14px rgba(37, 99, 235, 0.85);
        }
        .exam-btn-primary:hover {
            background: #1d4ed8;
        }
        .exam-filter-card,
        .exam-table-card {
            border: 1px solid rgba(148, 163, 184, 0.18);
            border-radius: 1.25rem;
            background: #ffffff;
            box-shadow: 0 12px 32px -28px rgba(15, 23, 42, 0.28);
        }
        .exam-filter-card {
            padding: 1rem;
        }
        .exam-filter {
            display: flex;
            flex-wrap: wrap;
            align-items: center;
            gap: 0.75rem;
        }
        .exam-filter select,
        .exam-filter input {
            height: 2.75rem;
            padding: 0 0.9rem;
            border: 1px solid #cbd5e1;
            border-radius: 0.9rem;
            background: #f8fafc;
            color: #0f172a;
            font-size: 0.875rem;
            transition: border-color 0.2s ease, box-shadow 0.2s ease, background 0.2s ease;
            outline: none;
        }
        .exam-filter select:focus,
        .exam-filter input:focus {
            border-color: #93c5fd;
            background: #ffffff;
            box-shadow: 0 0 0 4px rgba(191, 219, 254, 0.6);
        }
        .exam-filter input {
            min-width: 16rem;
        }
        .exam-filter input::placeholder {
            color: #94a3b8;
        }
        .exam-filter input[type="submit"],
        .exam-filter button,
        .exam-filter .aspNetDisabled,
        .exam-filter-card input[type="submit"] {
            height: 2.75rem;
            padding: 0 1rem;
            border: 0;
            border-radius: 0.9rem;
            background: #2563eb;
            color: #ffffff;
            font-size: 0.875rem;
            font-weight: 600;
            cursor: pointer;
            box-shadow: 0 10px 20px -14px rgba(37, 99, 235, 0.85);
        }
        .exam-table-wrap {
            overflow-x: auto;
        }
        .exam-table {
            width: 100%;
            border-collapse: separate;
            border-spacing: 0;
            min-width: 880px;
        }
        .exam-table th,
        .exam-table td {
            padding: 1rem 1.1rem;
            border-bottom: 1px solid #e2e8f0;
            vertical-align: middle;
        }
        .exam-table th {
            background: #f8fafc;
            color: #475569;
            font-size: 0.78rem;
            font-weight: 700;
            letter-spacing: 0.04em;
            text-transform: uppercase;
            text-align: left;
        }
        .exam-table td {
            color: #334155;
            font-size: 0.9rem;
        }
        .exam-table tbody tr:hover {
            background: #f8fafc;
        }
        .exam-table tbody tr:last-child td {
            border-bottom: 0;
        }
        .exam-name {
            display: flex;
            flex-direction: column;
            gap: 0.2rem;
        }
        .exam-name strong {
            color: #0f172a;
            font-size: 0.95rem;
        }
        .exam-code {
            color: #94a3b8;
            font-size: 0.78rem;
        }
        .exam-time {
            color: #475569;
            line-height: 1.7;
        }
        .exam-duration,
        .exam-submit {
            white-space: nowrap;
            color: #475569;
        }
        .status {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            min-width: 4.5rem;
            padding: 0.35rem 0.75rem;
            border-radius: 999px;
            font-size: 0.75rem;
            font-weight: 700;
            letter-spacing: 0.02em;
        }
        .status-0 { background: #e2e8f0; color: #475569; }
        .status-1 { background: #dbeafe; color: #1d4ed8; }
        .status-2 { background: #dcfce7; color: #15803d; }
        .status-3 { background: #fef3c7; color: #b45309; }
        .status-4 { background: #f1f5f9; color: #64748b; }
        .actions {
            display: flex;
            flex-wrap: wrap;
            gap: 0.6rem;
        }
        .actions a,
        .actions input[type="submit"],
        .actions .aspNetDisabled {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            padding: 0.4rem 0.8rem;
            border: 1px solid #dbeafe;
            border-radius: 999px;
            background: #eff6ff;
            color: #2563eb;
            font-size: 0.78rem;
            font-weight: 600;
            text-decoration: none;
            white-space: nowrap;
            cursor: pointer;
            transition: all 0.2s ease;
        }
        .actions a:hover,
        .actions input[type="submit"]:hover {
            background: #dbeafe;
            color: #1d4ed8;
        }
        .actions [style*='ff4d4f'] {
            border-color: #fecaca !important;
            background: #fef2f2 !important;
            color: #dc2626 !important;
        }
        .empty-data {
            padding: 3.5rem 1.5rem;
            text-align: center;
            color: #94a3b8;
            font-size: 0.95rem;
        }
        @media (max-width: 900px) {
            .exam-page {
                padding: 1rem;
            }
            .exam-hero {
                flex-direction: column;
            }
            .exam-actions {
                justify-content: flex-start;
            }
            .exam-filter input {
                min-width: 12rem;
                flex: 1 1 12rem;
            }
        }
    </style>

    <div class="exam-page">
        <div class="exam-stack">
            <section class="exam-hero">
                <div class="exam-hero-text">
                    <h2 class="exam-hero-title">考试管理</h2>
                    <p class="exam-hero-subtitle">统一管理考试安排、试卷关联、发布节奏与考试结果。整体视觉和教师端其他页面保持一致，避免出现偏黄或偏灰的旧背景色。</p>
                </div>
                <div class="exam-actions">
                    <a href="paper/paperlist.aspx" class="exam-btn exam-btn-secondary">试卷管理</a>
                    <a href="question/banklist.aspx" class="exam-btn exam-btn-secondary">题库管理</a>
                    <a href="examadd.aspx" class="exam-btn exam-btn-primary">+ 创建考试</a>
                </div>
            </section>

            <section class="exam-filter-card">
                <div class="exam-filter">
                    <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                        <asp:ListItem Value="-1">全部状态</asp:ListItem>
                        <asp:ListItem Value="0">未发布</asp:ListItem>
                        <asp:ListItem Value="1">已发布</asp:ListItem>
                        <asp:ListItem Value="2">进行中</asp:ListItem>
                        <asp:ListItem Value="3">已结束</asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox ID="txtKeyword" runat="server" placeholder="搜索考试名称或考试编号"></asp:TextBox>
                    <asp:Button ID="btnSearch" runat="server" Text="搜索" OnClick="btnSearch_Click" />
                </div>
            </section>

            <section class="exam-table-card">
                <div class="exam-table-wrap">
                    <asp:Repeater ID="rptExamList" runat="server">
                        <HeaderTemplate>
                            <table class="exam-table">
                                <thead>
                                    <tr>
                                        <th style="width:25%">考试名称</th>
                                        <th style="width:12%">试卷</th>
                                        <th style="width:25%">考试时间</th>
                                        <th style="width:8%">时长</th>
                                        <th style="width:10%">提交情况</th>
                                        <th style="width:8%">状态</th>
                                        <th style="width:12%">操作</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <div class="exam-name">
                                        <strong><%# Eval("ExamName") %></strong>
                                        <span class="exam-code">编号：<%# Eval("ExamCode") %></span>
                                    </div>
                                </td>
                                <td><%# Eval("PaperName") %></td>
                                <td class="exam-time"><%# GetTimeDisplay(Container.DataItem as LearnSite.Model.Exam) %></td>
                                <td class="exam-duration"><%# Eval("Duration") %> 分钟</td>
                                <td class="exam-submit"><%# Eval("SubmittedCount") %>/<%# Eval("ParticipantCount") %></td>
                                <td><span class="status status-<%# Eval("Status") %>"><%# GetStatusText(Eval("Status")) %></span></td>
                                <td class="actions">
                                    <asp:PlaceHolder ID="phEdit" runat="server" Visible='<%# (int)Eval("Status") == 0 %>'>
                                        <a href="examedit.aspx?id=<%# Eval("ExamId") %>">编辑</a>
                                    </asp:PlaceHolder>
                                    <a href="exammonitor.aspx?id=<%# Eval("ExamId") %>">监控</a>
                                    <a href="examresult.aspx?id=<%# Eval("ExamId") %>">成绩</a>
                                    <asp:PlaceHolder ID="phPublish" runat="server" Visible='<%# (int)Eval("Status") == 0 %>'>
                                        <asp:LinkButton ID="lbtnPublish" runat="server"
                                            CommandArgument='<%# Eval("ExamId") %>'
                                            OnCommand="lbtnPublish_Command"
                                            OnClientClick="return confirm('确定发布考试吗？发布后不可修改！');">发布</asp:LinkButton>
                                    </asp:PlaceHolder>
                                    <asp:PlaceHolder ID="phDelete" runat="server" Visible='<%# (int)Eval("Status") == 0 %>'>
                                        <asp:LinkButton ID="lbtnDelete" runat="server"
                                            CommandArgument='<%# Eval("ExamId") %>'
                                            OnCommand="lbtnDelete_Command"
                                            OnClientClick="return confirm('确定删除此考试吗？');"
                                            style="color:#ff4d4f">删除</asp:LinkButton>
                                    </asp:PlaceHolder>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                                </tbody>
                            </table>
                            <asp:PlaceHolder ID="phEmpty" runat="server" Visible='<%# ((Repeater)Container.NamingContainer).Items.Count == 0 %>'>
                                <div class="empty-data">暂无考试数据，点击右上角“创建考试”即可开始添加。</div>
                            </asp:PlaceHolder>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
