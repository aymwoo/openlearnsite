using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Teacher_consolepreview : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (!IsPostBack)
        {
            showProblem();
        }
    }

    private void showProblem() {
        if (Request.QueryString["nid"] != null && Request.QueryString["ncid"] != null && Request.QueryString["lid"] != null)
        {
            string nid = Request.QueryString["nid"].ToString();
            string cid = Request.QueryString["ncid"].ToString();
            string lid = Request.QueryString["lid"].ToString();
            LearnSite.BLL.Problems bll = new LearnSite.BLL.Problems();
            string json = bll.GetListJson(Int32.Parse(nid));   
            hidenjson.Value=json;
            hidennid.Value = nid;
            hidencid.Value = cid;
            hidenlid.Value = lid;
        }
    }
}