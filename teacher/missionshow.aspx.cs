using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Teacher_missionshow : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (!IsPostBack)
        {
            Master.Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle() + "学案活动显示页面";
            if (Request.QueryString["mcid"] != null && Request.QueryString["mid"] != null && Request.QueryString["lid"] != null)
            {
                showmission();
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
            CheckPublish.Checked = model.Mpublish;
            string decodedContent = HttpUtility.HtmlDecode(model.Mcontent);
            HiddenMissionRaw.Value = decodedContent;
            Mcontent.InnerHtml = decodedContent;
            LabelMdate.Text = model.Mdate.ToString();
            LabelMfiletype.Text = model.Mfiletype;
            ImageType.ImageUrl = "~/images/filetype/" + LabelMfiletype.Text.ToLower() + ".gif";
            CkMupload.Checked = model.Mupload;
            if (!model.Mupload) {
                LabelMfiletype.Text = "阅读";
                ImageType.Visible = false;
            }
            CheckGroup.Checked = model.Mgroup;
            CheckMicoWorld.Checked = model.Microworld;
            int Mgid = model.Mgid.Value;
            if (Mgid != 0)
                HLMgid.NavigateUrl = "~/teacher/gaugeitem.aspx?gid=" + Mgid.ToString();
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
        if (Request.QueryString["mcid"] != null && Request.QueryString["mid"] != null && Request.QueryString["lid"] != null)
        {
            string Mcid = Request.QueryString["mcid"].ToString();
            string Mid = Request.QueryString["mid"].ToString();
            string Lid = Request.QueryString["lid"].ToString();
            string url = "~/teacher/missionedit.aspx?mcid=" + Mcid + "&mid=" + Mid + "&lid=" + Lid;
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
