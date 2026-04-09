using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class student_myportfolio : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            LearnSite.Common.CookieHelp.KickStudent();
            if (!IsPostBack)
            {
                ShowWorks();
            }
        }
        else
        {
            LearnSite.Common.CookieHelp.JudgeStudentCookies();
        }
    }

    private void ShowWorks()
    {
        if (Request.QueryString["Snum"] != null)
        {
            string mysnum = Request.QueryString["Snum"].ToString();
            LearnSite.BLL.Students sbll = new LearnSite.BLL.Students();
            string myname = sbll.GetSnameBySnum(mysnum);
            Labeltitle.Text = Server.UrlDecode(myname) + "同学作品集";
            LearnSite.BLL.Works wbll = new LearnSite.BLL.Works();
            RepeaterWork.DataSource = wbll.ShowMyAllWork(mysnum);
            RepeaterWork.DataBind();
        }
    }
}