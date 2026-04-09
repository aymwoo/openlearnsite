using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Student_pysolve : System.Web.UI.Page
{
    LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            if (!IsPostBack)
            {
                showSovle();
            }
        }
        else
        {
            LearnSite.Common.CookieHelp.JudgeStudentCookies();
        }
    }

    private void showSovle() {
        if (Request.QueryString["nid"] != null )
        {
            string nid = Request.QueryString["nid"].ToString();
            int sgrade = cook.Sgrade;
            int sclass = cook.Sclass;
            LearnSite.BLL.Students sbll = new LearnSite.BLL.Students();
            GVsolve.DataSource= sbll.SolveAll(sgrade, sclass, Int32.Parse(nid));
            GVsolve.DataBind();
            LearnSite.BLL.Consoles cbll = new LearnSite.BLL.Consoles();
            Labeltitle.Text = cbll.GetCTitle(Int32.Parse(nid)) + "《" + cbll.GetTitle(Int32.Parse(nid)) + "》" + sgrade.ToString() + "年级" + sclass + "班Python测评反馈表";
        }
    
    }
}