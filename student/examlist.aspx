<%@ Page Language="C#" AutoEventWireup="true" CodeFile="examlist.aspx.cs" Inherits="student_examlist" MasterPageFile="~/student/Stud.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Cphs" runat="server">
    <style>
        .exam-list { padding: 20px; max-width: 1200px; margin: 0 auto; display: block; width: 100%; box-sizing: border-box; }
        .list-header { margin-bottom: 20px; text-align: center; }
        .list-header h2 { margin: 0 0 10px 0; font-size: 20px; }
        .exam-tabs { display: flex; gap: 10px; margin-bottom: 20px; justify-content: center; }
        .exam-tabs a { padding: 8px 20px; border-radius: 20px; text-decoration: none; color: #666; background: #f5f5f5; }
        .exam-tabs a:hover { background: #e6f7ff; color: #1890ff; }
        .exam-tabs a.active { background: #1890ff; color: #fff; }
        .exam-grid { display: flex; flex-wrap: wrap; gap: 15px; max-width: 1200px; margin: 0 auto !important; width: 100% !important; justify-content: center; }
        .exam-card { background: #fff; border-radius: 10px; padding: 20px; box-shadow: 0 2px 8px rgba(0,0,0,0.1); transition: all 0.3s; width: 320px; max-width: 400px; flex: 0 0 auto; }
        .exam-card:hover { box-shadow: 0 4px 16px rgba(0,0,0,0.15); transform: translateY(-3px); }
        .exam-card .title { font-size: 16px; font-weight: 600; margin-bottom: 10px; }
        .exam-card .info { font-size: 13px; color: #666; margin-bottom: 8px; }
        .exam-card .info span { margin-right: 15px; }
        .exam-card .status { display: inline-block; padding: 3px 10px; border-radius: 12px; font-size: 12px; margin-top: 10px; }
        .status-ongoing { background: #f6ffed; color: #52c41a; }
        .status-completed { background: #f0f0f0; color: #666; }
        .status-upcoming { background: #e6f7ff; color: #1890ff; }
        .exam-card .actions { margin-top: 15px; padding-top: 15px; border-top: 1px solid #f0f0f0; }
        .btn { display: inline-block; padding: 8px 20px; border-radius: 4px; text-decoration: none; font-size: 14px; cursor: pointer; border: none; }
        .btn-primary { background: #1890ff; color: #fff; }
        .btn-primary:hover { background: #40a9ff; }
        .btn-success { background: #52c41a; color: #fff; }
        .btn-default { background: #f0f0f0; color: #333; }
        .empty-state { text-align: center; padding: 60px 20px; color: #999; }
        .empty-state img { width: 120px; margin-bottom: 20px; opacity: 0.5; }
        .countdown { color: #ff4d4f; font-weight: 600; }
    </style>

    <div class="exam-list">
        <div class="list-header">
            <h2>我的考试</h2>
        </div>

        <div class="exam-tabs">
            <a href="?tab=ongoing" class="<%= Tab == "ongoing" ? "active" : "" %>">进行中</a>
            <a href="?tab=upcoming" class="<%= Tab == "upcoming" ? "active" : "" %>">即将开始</a>
            <a href="?tab=completed" class="<%= Tab == "completed" ? "active" : "" %>">已结束</a>
        </div>

        <div style="max-width: 1200px; margin: 0 auto; width: 100%;">
            <div class="exam-grid">
                <asp:Repeater ID="rptExams" runat="server" OnItemDataBound="rptExams_ItemDataBound">
                <ItemTemplate>
                    <div class="exam-card">
                        <asp:HiddenField ID="hfExamId" runat="server" Value='<%# Eval("ExamId") %>' />
                        <asp:HiddenField ID="hfStartTime" runat="server" Value='<%# Eval("StartTime", "{0:yyyy-MM-ddTHH:mm:ss}") %>' />
                        <asp:HiddenField ID="hfEndTime" runat="server" Value='<%# Eval("EndTime", "{0:yyyy-MM-ddTHH:mm:ss}") %>' />
                        <div class="title"><%# Eval("ExamName") %></div>
                        <div class="info">
                            <span>试卷：<%# Eval("PaperName") %></span>
                        </div>
                        <div class="info">
                            <span>时长：<%# Eval("Duration") %>分钟</span>
                            <span>总分：<%# Eval("TotalScore") ?? "100" %>分</span>
                        </div>
                        <div class="info">
                            时间：<%# GetTimeDisplay(Container.DataItem as LearnSite.Model.Exam) %>
                        </div>
                        
                        <!-- 进行中 -->
                        <asp:Panel ID="pnlOngoing" runat="server" Visible="false">
                            <span class="status status-ongoing">进行中</span>
                            <div class="actions">
                                <asp:Panel ID="pnlNotSubmitted" runat="server">
                                    <a href='<%# ResolveUrl("~/student/examstart.aspx?id=" + Eval("ExamId")) %>' class="btn btn-primary">进入考试</a>
                                </asp:Panel>
                                <asp:Panel ID="pnlSubmitted" runat="server" Visible="false">
                                    <span style="color:#52c41a;">已提交</span>
                                    <a href='<%# ResolveUrl("~/student/examreview.aspx?examId=" + Eval("ExamId")) %>' class="btn btn-default">查看答卷</a>
                                </asp:Panel>
                            </div>
                        </asp:Panel>
                        
                        <!-- 即将开始 -->
                        <asp:Panel ID="pnlUpcoming" runat="server" Visible="false">
                            <span class="status status-upcoming">即将开始</span>
                            <div class="info countdown" style="margin-top:10px;">
                                距离开始：<span class="countdown-timer" data-start="<%# Eval("StartTime", "{0:yyyy-MM-ddTHH:mm:ss}") %>">计算中...</span>
                            </div>
                        </asp:Panel>
                        
                        <!-- 已结束 -->
                        <asp:Panel ID="pnlCompleted" runat="server" Visible="false">
                            <span class="status status-completed">已结束</span>
                            <div class="actions">
                                <a href='<%# ResolveUrl("~/student/examreview.aspx?examId=" + Eval("ExamId")) %>' class="btn btn-default">查看答卷</a>
                                <a href='<%# ResolveUrl("~/student/examresult.aspx?examId=" + Eval("ExamId")) %>' class="btn btn-success">我的成绩</a>
                            </div>
                        </asp:Panel>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            </div>
        </div>

        <asp:Panel ID="pnlEmpty" runat="server" Visible="false" CssClass="empty-state">
            <div>暂无考试数据</div>
        </asp:Panel>
    </div>

    <script type="text/javascript">
        // 倒计时
        document.querySelectorAll('.countdown-timer').forEach(function(el) {
            var startTime = new Date(el.getAttribute('data-start'));
            function updateCountdown() {
                var now = new Date();
                var diff = startTime - now;
                if (diff <= 0) {
                    el.textContent = '已开始';
                    location.reload();
                    return;
                }
                var hours = Math.floor(diff / 3600000);
                var minutes = Math.floor((diff % 3600000) / 60000);
                var seconds = Math.floor((diff % 60000) / 1000);
                el.textContent = hours + '时' + minutes + '分' + seconds + '秒';
            }
            updateCountdown();
            setInterval(updateCountdown, 1000);
        });
    </script>
</asp:Content>
