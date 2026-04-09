using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Teacher_pixelshow : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (!IsPostBack)
        {
            Master.Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle() + "主题创作页面";
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
            string category = model.Mcategory.HasValue ? model.Mcategory.Value.ToString() : "11";
            LearnSite.Common.CustomActivityMeta meta = LearnSite.Common.CustomActivityCatalog.GetMeta(category);
            LabelMfiletype.Text = model.Mfiletype;
            LabelMtitle.Text = model.Mtitle;
            Mcontent.InnerHtml = HttpUtility.HtmlDecode(model.Mcontent);
            LabelActivityName.Text = meta.DisplayName;
            LabelActivityDescription.Text = meta.Description;
            LabelStudentEntry.Text = meta.StudentEntryUrl;
            LabelEditFocus.Text = meta.EditFocus;
            ImageActivityIcon.ImageUrl = ResolveUrl(meta.IconUrl);
            ActivityChip.Style["background"] = meta.BadgeBackground;
            ActivityChip.Style["color"] = meta.BadgeForeground;

            CheckPublish.Checked = model.Mpublish;
            LabelMdate.Text = model.Mdate.ToString();
            ImageType.ImageUrl = "~/images/filetype/" + LabelMfiletype.Text.ToLower() + ".gif";
            int Mgid = model.Mgid.Value;
            if (Mgid != 0)
                HLMgid.NavigateUrl = "~/teacher/gaugeitem.aspx?gid=" + Mgid.ToString();

            string exampleSummary = LearnSite.Common.CustomActivityCatalog.GetExampleSummary(category, model.Mexample);
            PanelExampleSummary.Visible = !String.IsNullOrEmpty(exampleSummary);
            if (PanelExampleSummary.Visible)
            {
                LabelExampleSummary.Text = HttpUtility.HtmlEncode(exampleSummary);
            }
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
            string url = "~/teacher/pixeledit.aspx?mcid=" + Mcid + "&mid=" + Mid;
            Response.Redirect(url, false);
        }
    }
}
