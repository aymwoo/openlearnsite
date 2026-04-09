using System;
using LearnSite.Model;
using LearnSite.BLL;

public partial class student_examresult : System.Web.UI.Page
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
            LoadResult(examId);
        }
    }

    private void LoadResult(int examId)
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
        if (exam != null)
        {
            ltlExamName.Text = exam.ExamName;
        }

        // 获取试卷信息
        var paperBll = new LearnSite.BLL.ExamPaper();
        var paper = paperBll.GetPaperById(answer.PaperId);
        decimal totalScore = 100;
        decimal passScore = 60;
        if (paper != null)
        {
            totalScore = paper.TotalScore;
            passScore = paper.PassScore;
        }

        ltlTotalScore.Text = totalScore.ToString();
        ltlScore.Text = answer.TotalScore.ToString("0.0");

        // 及格状态
        bool isPassed = answer.TotalScore >= passScore;
        scoreDiv.Attributes["class"] = "score " + (isPassed ? "pass" : "fail");
        lblStatus.Text = isPassed ? "及格" : "不及格";
        lblStatus.CssClass = "status " + (isPassed ? "pass" : "fail");

        // 统计
        ltlObjectiveScore.Text = answer.ObjectiveScore.ToString("0.0");
        ltlSubjectiveScore.Text = (answer.SubjectiveScore ?? 0).ToString("0.0");
        ltlDuration.Text = FormatDuration(answer.Duration);
        ltlSubmitTime.Text = answer.SubmitTime != null ? string.Format("{0:yyyy-MM-dd HH:mm:ss}", answer.SubmitTime) : "-";

        // 统计正确/错误
        int correct = 0, wrong = 0;
        if (!string.IsNullOrEmpty(answer.ScoreDetails))
        {
            var details = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<ScoreDetail>>(answer.ScoreDetails);
            foreach (var d in details)
            {
                if (d.Score >= d.MaxScore) correct++;
                else if (d.Score == 0) wrong++;
            }
        }
        ltlCorrect.Text = correct.ToString();
        ltlWrong.Text = wrong.ToString();

        // 排名
        var resultBll = new LearnSite.DAL.ExamResult();
        var results = resultBll.GetResultList(examId);
        int rank = 1;
        foreach (var r in results)
        {
            if (r.StudentId == cook.Sid.ToString())
            {
                ltlRank.Text = rank.ToString();
                break;
            }
            rank++;
        }
    }

    private string FormatDuration(int seconds)
    {
        int mins = seconds / 60;
        int secs = seconds % 60;
        return string.Format("{0}分{1}秒", mins, secs);
    }
}
