using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using LearnSite.Model;
using LearnSite.BLL;

public partial class exam_exammonitor : System.Web.UI.Page
{
    protected TeaCook tcook = new TeaCook();
    protected int ExamId { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (!tcook.IsExist())
        {
            Response.Redirect("~/index.aspx");
            return;
        }

        int examId;
        if (!int.TryParse(Request.QueryString["id"], out examId))
        {
            Response.Redirect("examlist.aspx");
            return;
        }

        ExamId = examId;

        if (!IsPostBack)
        {
            LoadExamInfo();
            BindClasses();
            BindStudentList();
            LoadStatistics();
        }
    }

    private void LoadExamInfo()
    {
        var examBll = new LearnSite.BLL.Exam();
        var exam = examBll.GetExamById(ExamId);
        if (exam == null)
        {
            Response.Redirect("examlist.aspx");
            return;
        }

        ltlExamName.Text = exam.ExamName;
        ltlExamTime.Text = string.Format("{0:MM-dd HH:mm} ~ {1:MM-dd HH:mm}", exam.StartTime, exam.EndTime);
    }

    private void BindClasses()
    {
        // 获取该考试涉及的班级
        var answerBll = new LearnSite.BLL.ExamAnswer();
        var answers = answerBll.GetAnswerList(ExamId);

        var classes = new Dictionary<int, string>();
        foreach (var a in answers)
        {
            if (a.ClassId.HasValue && !classes.ContainsKey(a.ClassId.Value))
            {
                classes[a.ClassId.Value] = a.ClassName;
            }
        }

        foreach (var c in classes)
        {
            ddlClass.Items.Add(new ListItem(c.Value, c.Key.ToString()));
        }
    }

    private void BindStudentList()
    {
        var answerBll = new LearnSite.BLL.ExamAnswer();
        int? classId = null;
        int? status = null;

        if (!string.IsNullOrEmpty(ddlClass.SelectedValue))
            classId = int.Parse(ddlClass.SelectedValue);

        if (!string.IsNullOrEmpty(ddlStatus.SelectedValue))
            status = int.Parse(ddlStatus.SelectedValue);

        var list = answerBll.GetAnswerList(ExamId, classId);

        if (status.HasValue)
        {
            list = list.FindAll(a => a.Status == status.Value);
        }

        rptStudents.DataSource = list;
        rptStudents.DataBind();

        // 绑定实时动态
        var realtimeList = new List<dynamic>();
        foreach (var a in list)
        {
            if (a.SubmitTime.HasValue)
            {
                realtimeList.Add(new
                {
                    StudentName = a.StudentName,
                    Action = "提交答卷",
                    Time = a.SubmitTime.Value.ToString("HH:mm:ss")
                });
            }
        }
        rptRealtime.DataSource = realtimeList;
        rptRealtime.DataBind();
    }

    private void LoadStatistics()
    {
        var answerBll = new LearnSite.BLL.ExamAnswer();
        var answers = answerBll.GetAnswerList(ExamId);

        int total = answers.Count;
        int answering = 0;
        int submitted = 0;
        decimal avgScore = 0;
        decimal totalScore = 0;

        foreach (var a in answers)
        {
            if (a.Status == 0) answering++;
            else { submitted++; totalScore += a.TotalScore; }
        }

        if (submitted > 0) avgScore = Math.Round(totalScore / submitted, 1);

        ltlTotalCount.Text = total.ToString();
        ltlAnsweringCount.Text = answering.ToString();
        ltlSubmittedCount.Text = submitted.ToString();
        ltlAvgScore.Text = avgScore.ToString();
    }

    protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindStudentList();
    }

    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindStudentList();
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        BindStudentList();
        LoadStatistics();
    }

    protected string GetProgress(object answers)
    {
        // 根据答案JSON计算完成进度
        if (answers == null || string.IsNullOrEmpty(answers.ToString()))
            return "0";

        try
        {
            var dict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(answers.ToString());
            return dict != null && dict.Count > 0 ? Math.Min(100, dict.Count * 10).ToString() : "0";
        }
        catch
        {
            return "0";
        }
    }

    protected string FormatDuration(object duration)
    {
        if (duration == null) return "0分";
        int seconds = Convert.ToInt32(duration);
        int minutes = seconds / 60;
        int secs = seconds % 60;
        return string.Format("{0}分{1}秒", minutes, secs);
    }
}
