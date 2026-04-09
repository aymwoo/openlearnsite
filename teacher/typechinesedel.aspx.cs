using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Teacher_typechinesedel : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (!IsPostBack)
        {
            Master.Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle() + "拼音词语删除页面";
            if (Request.QueryString["nid"] != null)
            {
                LabelTid.Text = Request.QueryString["nid"].ToString();
            }
            else
            {
                Response.Redirect("~/teacher/typechinese.aspx", false);
            }
        }
    }
    protected void LinkBtnDel_Click(object sender, EventArgs e)
    {
        int Nid = Int32.Parse(Request.QueryString["nid"].ToString());
        LearnSite.BLL.Chinese cbll = new LearnSite.BLL.Chinese();
        cbll.Delete(Nid);
        System.Threading.Thread.Sleep(500);
        string url = "~/teacher/typechinese.aspx";
        Response.Redirect(url, false);
    }
    protected void LinkBtncancel_Click(object sender, EventArgs e)
    {
        string url = "~/teacher/typechinese.aspx";
        Response.Redirect(url, false);
    }
}