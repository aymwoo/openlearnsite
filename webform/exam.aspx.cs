using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public partial class webform_exam : System.Web.UI.Page
{
    public string Examjson = "";
    protected string Cid = "0";
    protected string Eid = "0";
    protected string Fpage = "#"; 
    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (Request.QueryString["cid"] != null)
        {
            if (!IsPostBack)
            {
                Master.Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle() + "创建考试";
                showCidEid();
            }
        }
        else
        {
            Response.Redirect("~/teacher/course.aspx", false);
        }
    }
    protected void showCidEid()
    {
        if (Request.QueryString["cid"] != null)
        {
            Cid = Request.QueryString["cid"].ToString();
            Fpage =  "../teacher/courseshow.aspx?cid=" + Cid;
        }
        if (Request.QueryString["eid"] != null)
        {
            Eid = Request.QueryString["eid"].ToString();
            LearnSite.Model.Exams emodel = new LearnSite.Model.Exams();
            LearnSite.BLL.Exams ebll = new LearnSite.BLL.Exams();
            emodel = ebll.GetModel(Int32.Parse(Eid));
            Examjson = emodel.Edata;
        } 
    }
}