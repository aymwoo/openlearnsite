<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Analysis.aspx.cs" Inherits="webform_Analysis" ResponseEncoding="utf-8" Culture="zh-CN" UICulture="zh-CN" %>

<!DOCTYPE html>
<html lang="zh-CN">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>学生成绩分析</title>
    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
    <link rel="stylesheet" type="text/css" href="../App_Themes/Teacher/Analysis.css" />
    <style type="text/css">
        .analysis-page { background: #f8fafc; min-height: 100vh; }
        .analysis-header { background: linear-gradient(135deg, #312e81 0%, #4338ca 100%); color: white; padding: 2rem; }
        .analysis-header h1 { margin: 0 0 0.5rem 0; font-size: 1.75rem; }
        .analysis-header p { margin: 0; opacity: 0.9; }
        .analysis-body { padding: 1.5rem; max-width: 1400px; margin: 0 auto; }
        .stats-row { display: flex; gap: 1rem; flex-wrap: wrap; margin-bottom: 1.5rem; }
        .stat-card { background: white; border-radius: 0.75rem; padding: 1.25rem; flex: 1; min-width: 150px; box-shadow: 0 1px 3px rgba(0,0,0,0.1); }
        .stat-card h3 { margin: 0; font-size: 1.75rem; color: #1e40af; }
        .stat-card p { margin: 0.25rem 0 0 0; color: #64748b; font-size: 0.875rem; }
        .stat-card.excellent { border-left: 4px solid #22c55e; }
        .stat-card.good { border-left: 4px solid #3b82f6; }
        .stat-card.pass { border-left: 4px solid #f59e0b; }
        .stat-card.fail { border-left: 4px solid #ef4444; }
        .section { background: white; border-radius: 0.75rem; padding: 1.5rem; margin-bottom: 1.5rem; box-shadow: 0 1px 3px rgba(0,0,0,0.1); }
        .section-title { font-size: 1.125rem; font-weight: 600; margin: 0 0 1rem 0; color: #1e293b; border-bottom: 1px solid #e2e8f0; padding-bottom: 0.75rem; }
        .score-table { width: 100%; border-collapse: collapse; }
        .score-table th, .score-table td { padding: 0.75rem; text-align: left; border-bottom: 1px solid #e2e8f0; }
        .score-table th { background: #f8fafc; font-weight: 600; color: #475569; }
        .score-table tr:hover { background: #f8fafc; }
        .score-table .clickable { cursor: pointer; }
        .score-table .clickable:hover { background: #eef2ff; }
        .score-badge { display: inline-block; padding: 0.25rem 0.75rem; border-radius: 9999px; font-size: 0.75rem; font-weight: 600; }
        .score-badge.excellent { background: #dcfce7; color: #166534; }
        .score-badge.good { background: #dbeafe; color: #1e40af; }
        .score-badge.pass { background: #fef3c7; color: #92400e; }
        .score-badge.fail { background: #fee2e2; color: #991b1b; }
        .accuracy-bar { display: flex; align-items: center; gap: 0.5rem; }
        .accuracy-bar .bar { flex: 1; height: 8px; background: #e2e8f0; border-radius: 4px; overflow: hidden; }
        .accuracy-bar .bar-fill { height: 100%; border-radius: 4px; }
        .accuracy-bar .bar-fill.high { background: #22c55e; }
        .accuracy-bar .bar-fill.medium { background: #f59e0b; }
        .accuracy-bar .bar-fill.low { background: #ef4444; }
        .error-list { display: flex; flex-wrap: wrap; gap: 0.5rem; }
        .error-tag { background: #fee2e2; color: #991b1b; padding: 0.25rem 0.75rem; border-radius: 0.5rem; font-size: 0.75rem; }
        .no-student { color: #94a3b8; font-style: italic; }
        .modal { display: none; position: fixed; top: 0; left: 0; width: 100%; height: 100%; background: rgba(0,0,0,0.5); z-index: 1000; }
        .modal.show { display: flex; align-items: center; justify-content: center; }
        .modal-content { background: white; border-radius: 1rem; max-width: 800px; width: 90%; max-height: 90vh; overflow-y: auto; }
        .modal-header { padding: 1.25rem 1.5rem; border-bottom: 1px solid #e2e8f0; display: flex; justify-content: space-between; align-items: center; }
        .modal-header h3 { margin: 0; }
        .modal-close { background: none; border: none; font-size: 1.5rem; cursor: pointer; color: #64748b; }
        .modal-body { padding: 1.5rem; }
        .detail-item { margin-bottom: 1rem; }
        .detail-item label { font-weight: 600; color: #475569; display: block; margin-bottom: 0.25rem; }
        .detail-item .value { color: #1e293b; }
        .question-result { padding: 1rem; border-radius: 0.5rem; margin-bottom: 0.75rem; }
        .question-result.correct { background: #f0fdf4; border-left: 4px solid #22c55e; }
        .question-result.wrong { background: #fef2f2; border-left: 4px solid #ef4444; }
        .distribution-chart { display: flex; height: 120px; align-items: flex-end; gap: 1rem; padding: 1rem 0; }
        .distribution-bar { flex: 1; display: flex; flex-direction: column; align-items: center; }
        .distribution-bar .bar { width: 100%; border-radius: 0.5rem 0.5rem 0 0; }
        .distribution-bar .label { margin-top: 0.5rem; font-size: 0.75rem; color: #64748b; }
        .distribution-bar .count { font-weight: 600; color: #1e293b; }
        .refresh-info { text-align: center; color: #64748b; font-size: 0.875rem; margin-bottom: 1rem; }
        .action-btn { background: #3b82f6; color: white; border: none; padding: 0.5rem 1rem; border-radius: 0.5rem; cursor: pointer; font-size: 0.875rem; }
        .action-btn:hover { background: #2563eb; }
        .view-detail { color: #3b82f6; cursor: pointer; font-size: 0.875rem; }
        .view-detail:hover { text-decoration: underline; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="analysis-page">
            <div class="analysis-header">
                <h1>学生成绩分析</h1>
                <p>查看班级测验得分、用时、题目正确率与未参加名单，支持详细错误分析。</p>
            </div>

            <div class="analysis-body">
                <div class="refresh-info">
                    <%=PageStatus %>
                    <asp:Button ID="BtnRefresh" runat="server" Text="刷新数据" OnClick="BtnRefresh_Click" CssClass="action-btn" style="margin-left: 1rem;" />
                </div>

                <div class="stats-row">
                    <div class="stat-card">
                        <h3><%=TotalStudents %>人</h3>
                        <p>班级总人数</p>
                    </div>
                    <div class="stat-card">
                        <h3><%=Persons %>人</h3>
                        <p>已参与测验</p>
                    </div>
                    <div class="stat-card">
                        <h3><%=avgScore %>分</h3>
                        <p>平均分</p>
                    </div>
                    <div class="stat-card">
                        <h3><%=avgSpent %>分钟</h3>
                        <p>平均用时</p>
                    </div>
                </div>

                <div class="section">
                    <h3 class="section-title">成绩分布</h3>
                    <div class="stats-row">
                        <div class="stat-card excellent">
                            <h3><%=ExcellentCount %>人</h3>
                            <p>优秀 (≥90分)</p>
                        </div>
                        <div class="stat-card good">
                            <h3><%=GoodCount %>人</h3>
                            <p>良好 (80-89分)</p>
                        </div>
                        <div class="stat-card pass">
                            <h3><%=PassCount %>人</h3>
                            <p>及格 (60-79分)</p>
                        </div>
                        <div class="stat-card fail">
                            <h3><%=FailCount %>人</h3>
                            <p>不及格 (<60分)</p>
                        </div>
                    </div>
                    <div class="distribution-chart">
                        <div class="distribution-bar">
                            <div class="bar" style="height: <%=GetBarHeight(ExcellentCount) %>%; background: #22c55e;"></div>
                            <span class="count"><%=ExcellentCount %></span>
                            <span class="label">优秀</span>
                        </div>
                        <div class="distribution-bar">
                            <div class="bar" style="height: <%=GetBarHeight(GoodCount) %>%; background: #3b82f6;"></div>
                            <span class="count"><%=GoodCount %></span>
                            <span class="label">良好</span>
                        </div>
                        <div class="distribution-bar">
                            <div class="bar" style="height: <%=GetBarHeight(PassCount) %>%; background: #f59e0b;"></div>
                            <span class="count"><%=PassCount %></span>
                            <span class="label">及格</span>
                        </div>
                        <div class="distribution-bar">
                            <div class="bar" style="height: <%=GetBarHeight(FailCount) %>%; background: #ef4444;"></div>
                            <span class="count"><%=FailCount %></span>
                            <span class="label">不及格</span>
                        </div>
                    </div>
                </div>

                <div class="section">
                    <h3 class="section-title">学生成绩详情 <span style="font-weight: normal; font-size: 0.875rem; color: #64748b;">（点击姓名查看详细答题情况）</span></h3>
                    <asp:Repeater ID="RepeaterList" runat="server">
                        <HeaderTemplate>
                            <table class="score-table">
                                <thead>
                                    <tr>
                                        <th style="width: 50px;">序号</th>
                                        <th style="width: 100px;">姓名</th>
                                        <th style="width: 80px;">成绩</th>
                                        <th style="width: 80px;">等级</th>
                                        <th style="width: 80px;">用时</th>
                                        <th>错题数</th>
                                        <th style="width: 80px;">操作</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="clickable" onclick="showStudentDetail(<%# Container.ItemIndex %>)">
                                <td><%# Container.ItemIndex + 1 %></td>
                                <td><%# HttpUtility.UrlDecode(Eval("Asname").ToString()) %></td>
                                <td><strong><%# Eval("Ascore") %></strong>分</td>
                                <td><span class="score-badge <%# GetScoreClass(Eval("Ascore")) %>"><%# GetScoreLevel(Eval("Ascore")) %></span></td>
                                <td><%# Eval("Aspent") %>分钟</td>
                                <td><%# Eval("WrongCount") %>题</td>
                                <td><span class="view-detail">查看详情</span></td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                                </tbody>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>

                <div class="section">
                    <h3 class="section-title">题目分析（按错误率排序）</h3>
                    <asp:Repeater ID="RepeaterAnalysis" runat="server">
                        <HeaderTemplate>
                            <table class="score-table">
                                <thead>
                                    <tr>
                                        <th style="width: 50px;">序号</th>
                                        <th style="width: 80px;">题型</th>
                                        <th>题目内容</th>
                                        <th style="width: 80px;">答对数</th>
                                        <th style="width: 80px;">答错数</th>
                                        <th style="width: 120px;">正确率</th>
                                        <th style="width: 120px;">错误率</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td><%# Container.ItemIndex + 1 %></td>
                                <td><%# Eval("QuestionTypeText") %></td>
                                <td><%# GetShortTitle(Eval("QuestionTitle").ToString()) %></td>
                                <td style="color: #22c55e;"><%# Eval("CorrectCount") %></td>
                                <td style="color: #ef4444;"><%# Eval("WrongCount") %></td>
                                <td>
                                    <div class="accuracy-bar">
                                        <span><%# Eval("Accuracy") %>%</span>
                                        <div class="bar"><div class="bar-fill <%# GetAccuracyClass(Eval("Accuracy")) %>" style="width: <%# Eval("Accuracy") %>%;"></div></div>
                                    </div>
                                </td>
                                <td>
                                    <div class="accuracy-bar">
                                        <span style="color: #ef4444;"><%# Eval("ErrorRate") %>%</span>
                                        <div class="bar"><div class="bar-fill low" style="width: <%# Eval("ErrorRate") %>%;"></div></div>
                                    </div>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                                </tbody>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>

                <div class="section">
                    <h3 class="section-title">高频错题（错误率≥50%）</h3>
                    <asp:Repeater ID="RepeaterHighError" runat="server">
                        <HeaderTemplate>
                            <table class="score-table">
                                <thead>
                                    <tr>
                                        <th style="width: 50px;">序号</th>
                                        <th style="width: 80px;">题型</th>
                                        <th>题目内容</th>
                                        <th style="width: 80px;">错误率</th>
                                        <th>主要错误</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr style="background: #fef2f2;">
                                <td><%# Container.ItemIndex + 1 %></td>
                                <td><%# Eval("QuestionTypeText") %></td>
                                <td><%# GetShortTitle(Eval("QuestionTitle").ToString()) %></td>
                                <td style="color: #ef4444; font-weight: 600;"><%# Eval("ErrorRate") %>%</td>
                                <td><%# Eval("CommonWrongAnswer") %></td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                                </tbody>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                    <asp:Label ID="LabelNoHighError" runat="server" Text="暂无高频错题" CssClass="no-student" Visible="false"></asp:Label>
                </div>

                <div class="section">
                    <h3 class="section-title">未测验学生名单 (<%=NoPersons %>人)</h3>
                    <div class="error-list">
                        <asp:Repeater ID="RepeaterNo" runat="server">
                            <ItemTemplate>
                                <span class="error-tag"><%# Eval("Sname") %></span>
                            </ItemTemplate>
                        </asp:Repeater>
                        <asp:Label ID="LabelNoAbsent" runat="server" Text="全部学生已完成测验" CssClass="no-student" Visible="false"></asp:Label>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal" id="studentModal">
            <div class="modal-content">
                <div class="modal-header">
                    <h3 id="modalTitle">学生答题详情</h3>
                    <button type="button" class="modal-close" onclick="closeModal()">&times;</button>
                </div>
                <div class="modal-body" id="modalBody">
                </div>
            </div>
        </div>

        <asp:HiddenField ID="HiddenStudentData" runat="server" />
    </form>

    <script type="text/javascript">
        var studentData = <%=StudentJsonData %>;

        function showStudentDetail(index) {
            var student = studentData[index];
            if (!student) return;

            var title = student.name + ' 的答题详情';
            document.getElementById('modalTitle').innerText = title;

            var html = '<div class="detail-item"><label>学号</label><span class="value">' + student.num + '</span></div>';
            html += '<div class="detail-item"><label>成绩</label><span class="value"><strong>' + student.score + '</strong> 分 (' + student.level + ')</span></div>';
            html += '<div class="detail-item"><label>用时</label><span class="value">' + student.spent + ' 分钟</span></div>';
            html += '<div class="detail-item"><label>正确率</label><span class="value">' + student.accuracy + '%</span></div>';
            html += '<div class="detail-item"><label>错题数</label><span class="value" style="color: #ef4444;">' + student.wrongCount + ' 题</span></div>';
            html += '<hr style="margin: 1rem 0; border: none; border-top: 1px solid #e2e8f0;"/>';
            html += '<h4 style="margin: 0 0 1rem 0;">答题详情</h4>';

            if (student.answers && student.answers.length > 0) {
                for (var i = 0; i < student.answers.length; i++) {
                    var ans = student.answers[i];
                    var cls = ans.isCorrect ? 'correct' : 'wrong';
                    var status = ans.isCorrect ? '正确' : '错误';
                    html += '<div class="question-result ' + cls + '">';
                    html += '<div style="display: flex; justify-content: space-between; margin-bottom: 0.5rem;">';
                    html += '<strong>第' + (i + 1) + '题 (' + ans.typeText + ')</strong>';
                    html += '<span style="color: ' + (ans.isCorrect ? '#22c55e' : '#ef4444') + ';">' + status + '</span>';
                    html += '</div>';
                    html += '<div style="font-size: 0.875rem; color: #475569; margin-bottom: 0.5rem;">' + ans.title + '</div>';
                    if (!ans.isCorrect) {
                        html += '<div style="font-size: 0.875rem;"><span style="color: #ef4444;">学生答案：</span>' + ans.studentAnswer + '</div>';
                        html += '<div style="font-size: 0.875rem;"><span style="color: #22c55e;">正确答案：</span>' + ans.correctAnswer + '</div>';
                    }
                    html += '</div>';
                }
            } else {
                html += '<p class="no-student">暂无答题详情</p>';
            }

            document.getElementById('modalBody').innerHTML = html;
            document.getElementById('studentModal').classList.add('show');
        }

        function closeModal() {
            document.getElementById('studentModal').classList.remove('show');
        }

        document.getElementById('studentModal').addEventListener('click', function(e) {
            if (e.target === this) {
                closeModal();
            }
        });
    </script>
</body>
</html>
