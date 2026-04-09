using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Teacher_programshow : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (!IsPostBack)
        {
            Master.Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle() + "编程主题页面";
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
            LabelMtitle.Text = model.Mtitle;
            Mcontent.InnerHtml = HttpUtility.HtmlDecode(model.Mcontent);
            CheckMicoWorld.Checked = model.Microworld;
            if (!CheckMicoWorld.Checked)
            {
                string sburl = model.Mexample;
                Hlexample.NavigateUrl = sburl;
                Hlexample.Text = LearnSite.Common.WordProcess.getshortfname(sburl);
            }
            else
                Hlexample.Visible = false;
            CheckPublish.Checked = model.Mpublish;
            LabelMdate.Text = model.Mdate.ToString();
            LabelMfiletype.Text = model.Mfiletype;
            ImageType.ImageUrl = "~/images/filetype/" + LabelMfiletype.Text.ToLower() + ".gif";
            int Mgid = model.Mgid.Value;
            if (Mgid != 0)
                HLMgid.NavigateUrl = "~/teacher/gaugeitem.aspx?gid=" + Mgid.ToString();
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
            string url = "~/teacher/programedit.aspx?mcid=" + Mcid + "&mid=" + Mid;
            Response.Redirect(url, false);
        }
    }
}
