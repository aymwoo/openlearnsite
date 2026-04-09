<%@ Page Language="C#" AutoEventWireup="true" CodeFile="examresult.aspx.cs" Inherits="student_examresult" MasterPageFile="~/student/Stud.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Cphs" runat="server">
    <style>
        .result-page { padding: 20px; max-width: 600px; margin: 0 auto; }
        .result-card { background: #fff; border-radius: 12px; padding: 30px; text-align: center; box-shadow: 0 2px 12px rgba(0,0,0,0.1); margin-bottom: 20px; }
        .result-card .score { font-size: 72px; font-weight: bold; color: #1890ff; margin: 20px 0; }
        .result-card .score.pass { color: #52c41a; }
        .result-card .score.fail { color: #ff4d4f; }
        .result-card .total { font-size: 18px; color: #666; }
        .result-card .exam-name { font-size: 20px; margin-bottom: 10px; }
        .result-card .status { display: inline-block; padding: 5px 15px; border-radius: 20px; font-size: 14px; margin-top: 10px; }
        .result-card .status.pass { background: #f6ffed; color: #52c41a; }
        .result-card .status.fail { background: #fff1f0; color: #ff4d4f; }
        .stats-row { display: flex; justify-content: center; gap: 30px; margin-top: 20px; }
        .stats-item { text-align: center; }
        .stats-item .value { font-size: 24px; font-weight: bold; color: #333; }
        .stats-item .label { font-size: 13px; color: #999; margin-top: 5px; }
        .detail-card { background: #fff; border-radius: 8px; padding: 20px; box-shadow: 0 2px 8px rgba(0,0,0,0.1); margin-bottom: 15px; }
        .detail-card h3 { margin: 0 0 15px 0; font-size: 16px; }
        .detail-row { display: flex; justify-content: space-between; padding: 10px 0; border-bottom: 1px solid #f0f0f0; }
        .detail-row:last-child { border-bottom: none; }
        .detail-row .label { color: #666; }
        .detail-row .value { font-weight: 600; }
        .actions { margin-top: 20px; text-align: center; }
        .btn { display: inline-block; padding: 10px 30px; border-radius: 4px; text-decoration: none; font-size: 14px; margin: 0 5px; }
        .btn-primary { background: #1890ff; color: #fff; }
        .btn-default { background: #f0f0f0; color: #333; }
    </style>

    <div class="result-page">
        <div class="result-card">
            <div class="exam-name"><asp:Literal ID="ltlExamName" runat="server"></asp:Literal></div>
            <div class="total">满分 <asp:Literal ID="ltlTotalScore" runat="server">100</asp:Literal> 分</div>
            <div class="score" id="scoreDiv" runat="server">
                <asp:Literal ID="ltlScore" runat="server">0</asp:Literal>
            </div>
            <asp:Label ID="lblStatus" runat="server" CssClass="status">及格</asp:Label>
            
            <div class="stats-row">
                <div class="stats-item">
                    <div class="value"><asp:Literal ID="ltlCorrect" runat="server">0</asp:Literal></div>
                    <div class="label">正确</div>
                </div>
                <div class="stats-item">
                    <div class="value"><asp:Literal ID="ltlWrong" runat="server">0</asp:Literal></div>
                    <div class="label">错误</div>
                </div>
                <div class="stats-item">
                    <div class="value"><asp:Literal ID="ltlRank" runat="server">-</asp:Literal></div>
                    <div class="label">排名</div>
                </div>
            </div>
        </div>

        <div class="detail-card">
            <h3>答题详情</h3>
            <div class="detail-row">
                <span class="label">客观题得分</span>
                <span class="value"><asp:Literal ID="ltlObjectiveScore" runat="server">0</asp:Literal> 分</span>
            </div>
            <div class="detail-row">
                <span class="label">主观题得分</span>
                <span class="value"><asp:Literal ID="ltlSubjectiveScore" runat="server">0</asp:Literal> 分</span>
            </div>
            <div class="detail-row">
                <span class="label">答题用时</span>
                <span class="value"><asp:Literal ID="ltlDuration" runat="server">0分0秒</asp:Literal></span>
            </div>
            <div class="detail-row">
                <span class="label">提交时间</span>
                <span class="value"><asp:Literal ID="ltlSubmitTime" runat="server">-</asp:Literal></span>
            </div>
        </div>

        <div class="actions">
            <a href='<%= ResolveUrl("~/student/examreview.aspx") %>?examId=<%= ExamId %>' class="btn btn-primary">查看答卷</a>
            <a href='<%= ResolveUrl("~/student/examlist.aspx") %>' class="btn btn-default">返回列表</a>
        </div>
    </div>
</asp:Content>
