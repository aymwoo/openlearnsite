using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Teacher_judgeedit : System.Web.UI.Page
{
    protected string code;
    protected string arg1;
    protected string arg2;
    protected string arg3;
    protected string res1;
    protected string res2;
    protected string res3;
    protected string Id;
    protected string Fpage;
    protected string Cid;
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
            Response.Redirect("~/teacher/course.aspx", false);
        }
    }
    protected void showJudge()
    {
        string Mcid = Request.QueryString["mcid"].ToString();
        Mid = Request.QueryString["mid"].ToString();

        if (Mcid != null&Mid != null)
        {
            Fpage = "pythonshow.aspx?mcid=" + Mcid + "&mid=" + Mid ;
            int jmid = Int32.Parse(Mid);
            Id = jmid.ToString();
            Cid = Mcid;
            LearnSite.Model.JudgeArg jmodel = new LearnSite.Model.JudgeArg();
            LearnSite.BLL.JudgeArg jbll = new LearnSite.BLL.JudgeArg();

            LearnSite.BLL.Mission mbll = new LearnSite.BLL.Mission();
            LabelTitle.Text = mbll.GetMissionTitle(jmid);

            jmodel = jbll.GetModelByMid(jmid);
            if (jmodel != null)
            {
                code = jmodel.Jcode;
                arg1 = jmodel.Jinone;
                arg2 = jmodel.Jintwo;
                arg3 = jmodel.Jinthree;
                res1 = jmodel.Joutone;
                res2 = jmodel.Joutwo;
                res3 = jmodel.Jouthree;
            }
        }

    }

}