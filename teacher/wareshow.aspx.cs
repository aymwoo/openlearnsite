using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class teacher_wareshow : System.Web.UI.Page
{
    protected string WareUrl = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (!IsPostBack)
        {
            Master.Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle() + "网页课件";
            if (Request.QueryString["mcid"] != null && Request.QueryString["mid"] != null)
            {
                showmission();
            }
            else
            {
                Response.Redirect("~/teacher/course.aspx", false);
            }
        }
    }
    private void showmission()
    {
        string Mcid = Request.QueryString["mcid"].ToString();
        string Mid = Request.QueryString["mid"].ToString();

        LearnSite.Model.Mission model = new LearnSite.Model.Mission();
        LearnSite.BLL.Mission mn = new LearnSite.BLL.Mission();

        model = mn.GetModel(Int32.Parse(Mid));
        if (model != null)
        {
            LabelMfiletype.Text = model.Mfiletype;
            LabelMtitle.Text = model.Mtitle;
            HyperLinkHtml.NavigateUrl = model.Mback;

            CheckPublish.Checked = model.Mpublish;
            LabelMdate.Text = model.Mdate.ToString();
            ImageType.ImageUrl = "~/images/filetype/" + LabelMfiletype.Text.ToLower() + ".gif";
            WareUrl = model.Mback;

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
            string url = "~/teacher/wareedit.aspx?mcid=" + Mcid + "&mid=" + Mid;
            Response.Redirect(url, false);
        }
    }
}
