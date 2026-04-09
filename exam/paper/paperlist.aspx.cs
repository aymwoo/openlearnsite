using System;
using System.Collections.Generic;
using LearnSite.Model;
using LearnSite.BLL;

public partial class exam_paper_paperlist : System.Web.UI.Page
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
            BindPaperList();
        }
    }

    private void BindPaperList()
    {
        int status = int.Parse(ddlStatus.SelectedValue);
        var paperBll = new LearnSite.BLL.ExamPaper();
        var papers = paperBll.GetPaperList(tcook.Hid.ToString(), status);

        if (papers == null || papers.Count == 0)
        {
            pnlEmpty.Visible = true;
            rptPapers.Visible = false;
        }
        else
        {
            pnlEmpty.Visible = false;
            rptPapers.Visible = true;
            rptPapers.DataSource = papers;
            rptPapers.DataBind();
        }
    }

    protected string GetPaperTypeName(object type)
    {
        if (type == null) return "未知";
        int t = int.Parse(type.ToString());
        switch (t)
        {
            case 1: return "普通试卷";
            case 2: return "随机试卷";
            case 3: return "AB卷";
            default: return "其他";
        }
    }

    protected string GetStatusName(object status)
    {
        if (status == null) return "未知";
        int s = int.Parse(status.ToString());
        switch (s)
        {
            case 0: return "未发布";
            case 1: return "已发布";
            default: return "其他";
        }
    }

    protected string GetStatusBadgeClass(object status)
    {
        if (status == null) return "bg-secondary";
        int s = int.Parse(status.ToString());
        switch (s)
        {
            case 0: return "bg-warning text-dark";
            case 1: return "bg-success";
            default: return "bg-secondary";
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("paperadd.aspx");
    }

    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindPaperList();
    }

    protected void rptPapers_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
    {
        int paperId = int.Parse(e.CommandArgument.ToString());
        var paperBll = new LearnSite.BLL.ExamPaper();

        switch (e.CommandName)
        {
            case "Edit":
                Response.Redirect(string.Format("paperadd.aspx?id={0}", paperId));
                break;
            case "Delete":
                if (paperBll.DeletePaper(paperId))
                {
                    BindPaperList();
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('删除失败，试卷可能正在使用中！');", true);
                }
                break;
        }
    }
}
