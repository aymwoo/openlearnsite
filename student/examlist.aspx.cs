using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using LearnSite.Model;
using LearnSite.BLL;

public partial class student_examlist : System.Web.UI.Page
{
    protected Cook cook = new Cook();
    protected string Tab { get; set; }
    private DateTime _now;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!cook.IsExist())
        {
            Response.Redirect("~/index.aspx");
            return;
        }

        Tab = Request.QueryString["tab"] ?? "ongoing";
        _now = DateTime.Now;

        if (!IsPostBack)
        {
            BindExamList();
        }
    }

    private void BindExamList()
    {
        var examBll = new LearnSite.BLL.Exam();
        var allExams = examBll.GetStudentExamList(cook.Sid.ToString(), cook.Sclass);

        // 根据tab过滤
        IEnumerable<LearnSite.Model.Exam> filteredExams;

        switch (Tab)
        {
            case "ongoing":
                // 进行中的考试（在时间范围内）
                filteredExams = allExams.Where(e => e.StartTime <= _now && e.EndTime >= _now);
                break;
            case "upcoming":
                // 即将开始的考试
                filteredExams = allExams.Where(e => e.StartTime > _now);
                break;
            case "completed":
                // 已结束的考试
                filteredExams = allExams.Where(e => e.EndTime < _now);
                break;
            default:
                filteredExams = allExams;
                break;
        }

        rptExams.DataSource = filteredExams.ToList();
        rptExams.DataBind();

        pnlEmpty.Visible = !filteredExams.Any();
    }

    protected string GetTimeDisplay(LearnSite.Model.Exam exam)
    {
        if (exam == null) return "";

        if (exam.TimeMode == 2)
        {
            // 时间段模式
            string validText = exam.ValidDays > 0 ? string.Format("发布后{0}天内有效", exam.ValidDays) : "永久有效";
            return string.Format("{0:MM-dd HH:mm} 起 <small style=\"color:#52c41a\">({1})</small>", exam.StartTime, validText);
        }
        else
        {
            // 固定时间模式
            return string.Format("{0:MM-dd HH:mm} ~ {1:MM-dd HH:mm}", exam.StartTime, exam.EndTime);
        }
    }

    protected void rptExams_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            var exam = e.Item.DataItem as LearnSite.Model.Exam;
            if (exam == null) return;

            var pnlOngoing = e.Item.FindControl("pnlOngoing") as Panel;
            var pnlUpcoming = e.Item.FindControl("pnlUpcoming") as Panel;
            var pnlCompleted = e.Item.FindControl("pnlCompleted") as Panel;
            var pnlNotSubmitted = e.Item.FindControl("pnlNotSubmitted") as Panel;
            var pnlSubmitted = e.Item.FindControl("pnlSubmitted") as Panel;

            // 根据时间判断状态
            if (exam.StartTime <= _now && exam.EndTime >= _now)
            {
                // 进行中
                pnlOngoing.Visible = true;
                pnlUpcoming.Visible = false;
                pnlCompleted.Visible = false;

                // 检查是否已提交
                var answerBll = new LearnSite.BLL.ExamAnswer();
                var answer = answerBll.GetAnswerByExamAndStudent(exam.ExamId, cook.Sid.ToString());
                bool hasSubmitted = answer != null && answer.Status > 0;

                pnlNotSubmitted.Visible = !hasSubmitted;
                pnlSubmitted.Visible = hasSubmitted;
            }
            else if (exam.StartTime > _now)
            {
                // 即将开始
                pnlOngoing.Visible = false;
                pnlUpcoming.Visible = true;
                pnlCompleted.Visible = false;
            }
            else
            {
                // 已结束
                pnlOngoing.Visible = false;
                pnlUpcoming.Visible = false;
                pnlCompleted.Visible = true;
            }
        }
    }
}
