using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using LearnSite.Model;
using LearnSite.BLL;

public partial class exam_examlist : System.Web.UI.Page
{
    protected TeaCook tcook = new TeaCook();

    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (!tcook.IsExist())
        {
            Response.Redirect("~/index.aspx");
            return;
        }

        if (!IsPostBack)
        {
            BindExamList();
        }
    }

    private void BindExamList()
    {
        var examBll = new LearnSite.BLL.Exam();
        int status = int.Parse(ddlStatus.SelectedValue);
        var list = examBll.GetExamList(tcook.Hid.ToString(), status);

        // 关键字过滤
        string keyword = txtKeyword.Text.Trim();
        if (!string.IsNullOrEmpty(keyword))
        {
            list = list.FindAll(e => e.ExamName.Contains(keyword));
        }

        rptExamList.DataSource = list;
        rptExamList.DataBind();
    }

    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindExamList();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindExamList();
    }

    protected void lbtnPublish_Command(object sender, CommandEventArgs e)
    {
        int examId = int.Parse(e.CommandArgument.ToString());
        var examBll = new LearnSite.BLL.Exam();
        if (examBll.PublishExam(examId, tcook.Hid.ToString()))
        {
            BindExamList();
        }
    }

    protected void lbtnDelete_Command(object sender, CommandEventArgs e)
    {
        int examId = int.Parse(e.CommandArgument.ToString());
        var examBll = new LearnSite.BLL.Exam();
        if (examBll.DeleteExam(examId))
        {
            BindExamList();
        }
    }

    protected string GetStatusText(object status)
    {
        switch ((int)status)
        {
            case 0: return "未发布";
            case 1: return "已发布";
            case 2: return "进行中";
            case 3: return "已结束";
            case 4: return "已归档";
            default: return "未知";
        }
    }

    protected string GetTimeDisplay(LearnSite.Model.Exam exam)
    {
        if (exam == null) return "";

        if (exam.TimeMode == 2)
        {
            string validText = exam.ValidDays > 0 ? string.Format("{0}天内有效", exam.ValidDays) : "永久有效";
            return string.Format("{0:MM-dd HH:mm} 起 <small style=\"color:#52c41a\">({1})</small>", exam.StartTime, validText);
        }
        else
        {
            return string.Format("{0:MM-dd HH:mm} ~ {1:MM-dd HH:mm}", exam.StartTime, exam.EndTime);
        }
    }
}
