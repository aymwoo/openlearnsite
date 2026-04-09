using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Teacher_pythonshow : System.Web.UI.Page
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
            LabelMfiletype.Text = model.Mfiletype;
            if (model.Mfiletype == "py")
            {
                LearnSite.BLL.JudgeArg jbll = new LearnSite.BLL.JudgeArg();
                HLauto.NavigateUrl = "~/teacher/judgeedit.aspx?mcid=" + Mcid + "&mid=" + Mid;
                if (jbll.Exists(Int32.Parse(Mid)))
                {
                    Imgauto.ImageUrl = "~/images/flashview.png";
                }
                else
                {
                    Imgauto.ImageUrl = "~/images/flasherror.png";
                }

                CheckBack.Checked = model.Microworld;
                Checkhelp.Checked = model.Mhelp;
                if (model.Mcategory == 13)
                {
                    Checkblock.Checked = true;
                }
                else
                {
                    Checkblock.Checked = false;
                }
                if (model.Mcategory == 14)
                {
                    Checkblockpy.Checked = true;
                }
                else
                {
                    Checkblockpy.Checked = false;
                }

                if (!string.IsNullOrEmpty(model.Mexample))
                {
                    HlExample.NavigateUrl = model.Mexample;
                }
                else
                {
                    HlExample.NavigateUrl = "";
                }

                LabelMtitle.Text = model.Mtitle;
                Mcontent.InnerHtml = HttpUtility.HtmlDecode(model.Mcontent);

                CheckPublish.Checked = model.Mpublish;
                LabelMdate.Text = model.Mdate.ToString();
                ImageType.ImageUrl = "~/images/filetype/" + LabelMfiletype.Text.ToLower() + ".gif";
                int Mgid = model.Mgid.Value;
                if (Mgid != 0)
                    HLMgid.NavigateUrl = "~/teacher/gaugeitem.aspx?gid=" + Mgid.ToString();

            }
            else
                Mcontent.InnerHtml = "这里是python编程页面，你走错地方了!";
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
            string url = "~/teacher/pythonedit.aspx?mcid=" + Mcid + "&mid=" + Mid;
            Response.Redirect(url, false);
        }
    }
}
