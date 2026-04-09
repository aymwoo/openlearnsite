<%@ Page Language="C#" AutoEventWireup="true" CodeFile="examreview.aspx.cs" Inherits="student_examreview" MasterPageFile="~/student/Stud.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Cphs" runat="server">
    <style>
        .review-page { padding: 20px; max-width: 800px; margin: 0 auto; }
        .review-header { background: #fff; border-radius: 8px; padding: 20px; margin-bottom: 20px; box-shadow: 0 2px 8px rgba(0,0,0,0.1); }
        .review-header h2 { margin: 0 0 10px 0; }
        .review-header .info { font-size: 14px; color: #666; }
        .review-header .score-summary { margin-top: 15px; padding-top: 15px; border-top: 1px solid #f0f0f0; display: flex; justify-content: space-around; }
        .score-item { text-align: center; }
        .score-item .value { font-size: 24px; font-weight: bold; color: #1890ff; }
        .score-item .label { font-size: 13px; color: #999; }
        .question-card { background: #fff; border-radius: 8px; padding: 20px; margin-bottom: 15px; box-shadow: 0 2px 8px rgba(0,0,0,0.1); }
        .question-title { font-size: 16px; margin-bottom: 15px; line-height: 1.6; }
        .question-title .q-num { display: inline-block; width: 28px; height: 28px; background: #1890ff; color: #fff; border-radius: 50%; text-align: center; line-height: 28px; margin-right: 10px; }
        .question-title .q-type { color: #1890ff; margin-left: 10px; }
        .question-title .q-score { color: #666; margin-left: 10px; font-size: 14px; }
        .options-list { margin-bottom: 15px; }
        .options-list .option { padding: 10px 15px; border: 1px solid #e8e8e8; border-radius: 8px; margin-bottom: 8px; }
        .options-list .option.correct { background: #f6ffed; border-color: #b7eb8f; }
        .options-list .option.wrong { background: #fff1f0; border-color: #ffa39e; }
        .options-list .option.user-correct { background: #f6ffed; border-color: #52c41a; }
        .options-list .option.user-wrong { background: #fff1f0; border-color: #ff4d4f; }
        .options-list .option .label { display: inline-block; width: 24px; height: 24px; line-height: 24px; text-align: center; border-radius: 50%; background: #f0f0f0; margin-right: 10px; }
        .options-list .option.correct .label { background: #52c41a; color: #fff; }
        .analysis-box { background: #f9f9f9; border-radius: 8px; padding: 15px; margin-top: 15px; }
        .analysis-box h4 { margin: 0 0 10px 0; font-size: 14px; color: #1890ff; }
        .analysis-box .your-answer { margin-bottom: 10px; }
        .analysis-box .correct-answer { color: #52c41a; font-weight: 600; }
        .score-result { margin-top: 15px; padding-top: 15px; border-top: 1px solid #f0f0f0; }
        .score-result .score-value { font-size: 18px; font-weight: bold; }
        .score-result .score-value.correct { color: #52c41a; }
        .score-result .score-value.wrong { color: #ff4d4f; }
        .actions { text-align: center; margin-top: 20px; }
        .btn { display: inline-block; padding: 10px 30px; border-radius: 4px; text-decoration: none; font-size: 14px; margin: 0 5px; }
        .btn-primary { background: #1890ff; color: #fff; }
        .btn-default { background: #f0f0f0; color: #333; }
        .fillblank-answer { padding: 8px 12px; background: #f5f5f5; border-radius: 4px; margin: 5px 0; }
        .fillblank-answer.correct { background: #f6ffed; }
        .fillblank-answer.wrong { background: #fff1f0; }
    </style>

    <div class="review-page">
        <div class="review-header">
            <h2><asp:Literal ID="ltlExamName" runat="server"></asp:Literal></h2>
            <div class="info">
                提交时间：<asp:Literal ID="ltlSubmitTime" runat="server"></asp:Literal> | 
                用时：<asp:Literal ID="ltlDuration" runat="server"></asp:Literal>
            </div>
            <div class="score-summary">
                <div class="score-item">
                    <div class="value"><asp:Literal ID="ltlScore" runat="server">0</asp:Literal></div>
                    <div class="label">总得分</div>
                </div>
                <div class="score-item">
                    <div class="value"><asp:Literal ID="ltlTotalScore" runat="server">100</asp:Literal></div>
                    <div class="label">满分</div>
                </div>
                <div class="score-item">
                    <div class="value"><asp:Literal ID="ltlCorrectCount" runat="server">0</asp:Literal></div>
                    <div class="label">正确</div>
                </div>
                <div class="score-item">
                    <div class="value"><asp:Literal ID="ltlWrongCount" runat="server">0</asp:Literal></div>
                    <div class="label">错误</div>
                </div>
            </div>
        </div>

        <asp:Repeater ID="rptQuestions" runat="server" OnItemDataBound="rptQuestions_ItemDataBound">
            <ItemTemplate>
                <div class="question-card">
                    <asp:HiddenField ID="hfQuestionId" runat="server" Value='<%# Eval("QuestionId") %>' />
                    <asp:HiddenField ID="hfUserAnswer" runat="server" Value='<%# Eval("UserAnswer") %>' />
                    <asp:HiddenField ID="hfCorrectAnswer" runat="server" Value='<%# Eval("CorrectAnswer") %>' />
                    <asp:HiddenField ID="hfIsCorrect" runat="server" Value='<%# Eval("IsCorrect") %>' />
                    <asp:HiddenField ID="hfUserScore" runat="server" Value='<%# Eval("Score") %>' />
                    <asp:HiddenField ID="hfMaxScore" runat="server" Value='<%# Eval("MaxScore") %>' />

                    <div class="question-title">
                        <span class="q-num"><%# Container.ItemIndex + 1 %></span>
                        <%# Eval("QuestionContent") %>
                        <span class="q-type">[<%# Eval("QuestionType") %>]</span>
                        <span class="q-score">(<%# Eval("MaxScore") %>分)</span>
                    </div>

                    <asp:Panel ID="pnlOptions" runat="server" CssClass="options-list" Visible="false"></asp:Panel>
                    <asp:Panel ID="pnlFillBlank" runat="server" Visible="false"></asp:Panel>
                    <asp:Panel ID="pnlTextAnswer" runat="server" Visible="false"></asp:Panel>

                    <div class="analysis-box">
                        <h4>答案解析</h4>
                        <div class="your-answer">
                            你的答案：<strong><%# Eval("UserAnswer") %></strong>
                        </div>
                        <div class="correct-answer">
                            正确答案：<%# Eval("CorrectAnswer") %>
                        </div>
                    </div>

                    <div class="score-result">
                        <span class="score-value <%# (bool)Eval("IsCorrect") ? "correct" : "wrong" %>">
                            得分：<%# Eval("Score") %> / <%# Eval("MaxScore") %> 分
                        </span>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>

        <div class="actions">
            <a href='<%= ResolveUrl("~/student/examresult.aspx") %>?examId=<%= ExamId %>' class="btn btn-primary">查看成绩</a>
            <a href='<%= ResolveUrl("~/student/examlist.aspx") %>' class="btn btn-default">返回列表</a>
        </div>
    </div>
</asp:Content>
