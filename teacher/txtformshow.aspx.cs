using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Teacher_txtformshow : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (!IsPostBack)
        {
            Master.Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle() + "学案表单显示页面";
            if (Request.QueryString["mcid"] != null && Request.QueryString["mid"] != null)
            {
                showtxtform();
                if (Request.QueryString["cold"] != null)
                {
                    BtnEdit.Enabled = false;
                }
            }
            else
            {
                Response.Redirect("~/teacher/course.aspx", false);
            }
        }
    }
    private void showtxtform()
    {
        string Mcid = Request.QueryString["mcid"].ToString();
        string Mid = Request.QueryString["mid"].ToString();


        LearnSite.Model.TxtForm tmodel = new LearnSite.Model.TxtForm();
        LearnSite.BLL.TxtForm tbll = new LearnSite.BLL.TxtForm();

        tmodel = tbll.GetModel(Int32.Parse(Mid));

        if (tmodel != null)
        {
            LabelMtitle.Text = tmodel.Mtitle;
            CheckPublish.Checked = tmodel.Mpublish;
            Mcontent.InnerHtml = HttpUtility.HtmlDecode(tmodel.Mcontent);
            LabelMdate.Text = tmodel.Mdate.ToString();
            CheckCollabo.Checked = tmodel.Mcollabo;
        }
    }
    protected void LinkBtn_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["mcid"] != null)
        {
            string Cid = Request.QueryString["mcid"].ToString();
            string url = "~/teacher/courseshow.aspx?cid=" + Cid;
            if (Request.QueryString["cold"] != null)
            {
                url = url + "&cold=T";
            }
            Response.Redirect(url, false);
        }
    }
    protected void BtnEdit_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["mcid"] != null && Request.QueryString["mid"] != null)
        {
            string Mcid = Request.QueryString["mcid"].ToString();
            string Mid = Request.QueryString["mid"].ToString();
            string url = "~/teacher/txtformedit.aspx?mcid=" + Mcid + "&mid=" + Mid;
            Response.Redirect(url, false);
        }
    }
    protected void BtnReturnSmall_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["mcid"] != null)
        {
            string Cid = Request.QueryString["mcid"].ToString();
            string url = "~/teacher/courseshow.aspx?cid=" + Cid;
            if (Request.QueryString["cold"] != null)
            {
                url = url + "&cold=T";
            }
            Response.Redirect(url, false);
        }
    }
}
