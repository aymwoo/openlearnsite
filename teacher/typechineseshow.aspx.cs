using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Teacher_typechineseshow : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (!IsPostBack)
        {
            Master.Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle() + "拼音词语显示页面";
            if (Request.QueryString["nid"] != null)
            {
                ShowChinese();
            }
            else
            {
                Response.Redirect("~/teacher/typechinese.aspx", false);
            }
        }
    }

    private void ShowChinese()
    {
        if (Request.QueryString["nid"] != null)
        {
            int Nid = Int32.Parse(Request.QueryString["nid"].ToString());
            LearnSite.BLL.Chinese cbll = new LearnSite.BLL.Chinese();
            Repeater1.DataSource = cbll.GetOneChinese(Nid);
            Repeater1.DataBind();
        }
    }

    protected void BtnEdit_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["nid"] != null)
        {
            string Nid = Request.QueryString["nid"].ToString();
            string url = "~/teacher/typechineseedit.aspx?nid=" + Nid;
            Response.Redirect(url, false);
        }
    }
    protected void Btnreturn_Click(object sender, EventArgs e)
    {
        string url = "~/teacher/typechinese.aspx";
        Response.Redirect(url, false);
    }
}
