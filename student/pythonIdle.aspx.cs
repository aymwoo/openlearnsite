using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Student_pythonIdle : System.Web.UI.Page
{
    LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
    protected string Fpage = "#";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            if (Request.QueryString["lid"] != null)
            {
                LearnSite.Common.CookieHelp.KickStudent();
                if (!IsPostBack)
                {
                    showProblem();
                }
            }
        }
        else
        {
            LearnSite.Common.CookieHelp.JudgeStudentCookies();
        }
    }

    private void showProblem()
    {
        if (Request.QueryString["lid"] != null )
        {
            string Lid = Request.QueryString["lid"].ToString();
            LearnSite.BLL.ListMenu lbll = new LearnSite.BLL.ListMenu();
            LearnSite.Model.ListMenu lmodel = new LearnSite.Model.ListMenu();
            lmodel = lbll.GetModel(Int32.Parse(Lid));

            string cid = lmodel.Lcid.ToString();
            string nid = lmodel.Lxid.ToString();

            LearnSite.BLL.Problems bll = new LearnSite.BLL.Problems();
            string json = bll.GetListJson(Int32.Parse(nid));
            hidenjson.Value = json;
            hidennid.Value = nid;
            hidencid.Value = cid;
            hidenlid.Value = Lid;
            
            if (Request.Cookies[LearnSite.Common.CookieHelp.stuCookieNname] != null)
            {
                LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
                string stuName = cook.Sname;
                string stuNum = cook.Snum;

                this.Page.Title = HttpUtility.UrlDecode(stuName) + " Python交互式测评";
            }
        }
    }
}