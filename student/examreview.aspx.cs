using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using LearnSite.Model;
using LearnSite.BLL;

public partial class student_examreview : System.Web.UI.Page
{
    protected Cook cook = new Cook();
    protected int ExamId { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!cook.IsExist())
        {
            Response.Redirect("~/index.aspx");
            return;
        }

        int examId;
        if (!int.TryParse(Request.QueryString["examId"], out examId))
        {
            Response.Redirect("~/student/examlist.aspx");
            return;
        }

        ExamId = examId;

        if (!IsPostBack)
        {
            LoadReview(examId);
        }
    }

    private void LoadReview(int examId)
    {
        // 获取答卷
        var answerBll = new LearnSite.BLL.ExamAnswer();
        var answer = answerBll.GetAnswerByExamAndStudent(examId, cook.Sid.ToString());

        if (answer == null || answer.Status == 0)
        {
            Response.Redirect("~/student/examlist.aspx");
            return;
        }

        // 获取考试信息
        var examBll = new LearnSite.BLL.Exam();
        var exam = examBll.GetExamById(examId);
        ltlExamName.Text = exam != null ? exam.ExamName : "";

        ltlSubmitTime.Text = answer.SubmitTime != null ? string.Format("{0:yyyy-MM-dd HH:mm}", answer.SubmitTime) : "-";
        ltlDuration.Text = FormatDuration(answer.Duration);
        ltlScore.Text = answer.TotalScore.ToString("0.0");

        // 获取试卷信息
        var paperBll = new LearnSite.BLL.ExamPaper();
        var paper = paperBll.GetPaperById(answer.PaperId);
        ltlTotalScore.Text = paper != null ? paper.TotalScore.ToString() : "100";

        // 解析评分详情
        var scoreDetails = new List<ScoreDetail>();
        if (!string.IsNullOrEmpty(answer.ScoreDetails))
        {
            scoreDetails = JsonConvert.DeserializeObject<List<ScoreDetail>>(answer.ScoreDetails);
        }

        // 获取题目信息
        var questions = paperBll.GetPaperQuestions(answer.PaperId);

        // 合并数据
        var reviewItems = new List<dynamic>();
        int correctCount = 0;
        int wrongCount = 0;

        foreach (var pq in questions)
        {
            if (pq.Question == null) continue;

            var detail = scoreDetails.Find(d => d.QuestionId == pq.QuestionId) ?? new ScoreDetail
            {
                QuestionId = pq.QuestionId,
                MaxScore = pq.Score
            };

            var item = new
            {
                QuestionId = pq.QuestionId,
                QuestionContent = pq.Question.QuestionContent,
                QuestionType = GetTypeName(pq.Question.QuestionType),
                Options = pq.Question.Options,
                UserAnswer = detail.UserAnswer ?? "-",
                CorrectAnswer = detail.CorrectAnswer ?? pq.Question.Answer,
                IsCorrect = detail.IsCorrect,
                Score = detail.Score,
                MaxScore = detail.MaxScore,
                Analysis = pq.Question.Analysis
            };

            reviewItems.Add(item);

            if (detail.IsCorrect) correctCount++;
            else if (detail.Score == 0) wrongCount++;
        }

        rptQuestions.DataSource = reviewItems;
        rptQuestions.DataBind();

        ltlCorrectCount.Text = correctCount.ToString();
        ltlWrongCount.Text = wrongCount.ToString();
    }

    protected void rptQuestions_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            dynamic item = e.Item.DataItem;
            var pnlOptions = e.Item.FindControl("pnlOptions") as Panel;
            var pnlFillBlank = e.Item.FindControl("pnlFillBlank") as Panel;
            var pnlTextAnswer = e.Item.FindControl("pnlTextAnswer") as Panel;

            string type = item.QuestionType;
            string userAnswer = item.UserAnswer;
            string correctAnswer = item.CorrectAnswer;
            string options = item.Options;

            if (type == "单选题" || type == "多选题" || type == "判断题")
            {
                pnlOptions.Visible = true;
                var optionsList = string.IsNullOrEmpty(options) ? new List<QuestionOption>() :
                    JsonConvert.DeserializeObject<List<QuestionOption>>(options);

                if (type == "判断题")
                {
                    optionsList = new List<QuestionOption>
                    {
                        new QuestionOption { Label = "T", Content = "正确" },
                        new QuestionOption { Label = "F", Content = "错误" }
                    };
                }

                foreach (var opt in optionsList)
                {
                    bool isUserAnswer = userAnswer.Contains(opt.Label);
                    bool isCorrect = correctAnswer.Contains(opt.Label);

                    string cssClass = "option";
                    if (isCorrect) cssClass += " correct";
                    if (isUserAnswer && !isCorrect) cssClass += " user-wrong";
                    if (isUserAnswer && isCorrect) cssClass += " user-correct";

                    var div = new HtmlGenericControl("div");
                    div.Attributes["class"] = cssClass;
                    div.InnerHtml = string.Format("<span class=\"label\">{0}</span>{1}", opt.Label, opt.Content);
                    pnlOptions.Controls.Add(div);
                }
            }
            else if (type == "填空题")
            {
                pnlFillBlank.Visible = true;
                var userAnswers = userAnswer.Split('|');
                var correctAnswers = correctAnswer.Split('|');

                for (int i = 0; i < Math.Max(userAnswers.Length, correctAnswers.Length); i++)
                {
                    string ua = i < userAnswers.Length ? userAnswers[i] : "-";
                    string ca = i < correctAnswers.Length ? correctAnswers[i] : "-";
                    bool isCorrect = ua == ca;

                    var div = new HtmlGenericControl("div");
                    div.Attributes["class"] = string.Format("fillblank-answer {0}", isCorrect ? "correct" : "wrong");
                    div.InnerHtml = string.Format("第{0}空：你的答案 <strong>{1}</strong> | 正确答案 <strong>{2}</strong>", i + 1, ua, ca);
                    pnlFillBlank.Controls.Add(div);
                }
            }
            else if (type == "简答题")
            {
                pnlTextAnswer.Visible = true;
                var div = new HtmlGenericControl("div");
                div.InnerHtml = string.Format("<p><strong>你的答案：</strong></p><p>{0}</p><p><strong>参考答案：</strong></p><p>{1}</p>", userAnswer, correctAnswer);
                pnlTextAnswer.Controls.Add(div);
            }
        }
    }

    private string GetTypeName(int type)
    {
        switch (type)
        {
            case 1: return "单选题";
            case 2: return "多选题";
            case 3: return "判断题";
            case 4: return "填空题";
            case 5: return "简答题";
            default: return "未知";
        }
    }

    private string FormatDuration(int seconds)
    {
        int mins = seconds / 60;
        int secs = seconds % 60;
        return string.Format("{0}分{1}秒", mins, secs);
    }
}
