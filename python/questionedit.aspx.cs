using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class python_questionedit : System.Web.UI.Page
{
    protected string code;
    protected string Fpage;
    protected string Id;
    protected string Mid;

    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (Request.QueryString["mid"] != null)
        {
            if (!IsPostBack)
            {
                showJudge();
            }
        }
        else
        {
            Response.Redirect("~/python/match.aspx", false);
        }
    }
    protected void showJudge()
    {
        Mid = Request.QueryString["mid"].ToString();
        Fpage = "matchshow.aspx?mid=" +Mid;

        if (Request.QueryString["id"] != null)
        {
            Id = Request.QueryString["id"].ToString();

            LearnSite.Model.TurtleQuestion model = new LearnSite.Model.TurtleQuestion();
            LearnSite.BLL.TurtleQuestion bll = new LearnSite.BLL.TurtleQuestion();

            model = bll.GetModel(Int32.Parse(Id));
            if (model != null)
            {
                code = model.Qcode;
                TextBoxTitle.Text = model.Qtitle;
            }
        }

    }
}