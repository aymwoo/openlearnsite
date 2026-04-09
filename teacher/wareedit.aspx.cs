using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class teacher_wareedit : System.Web.UI.Page
{
    protected string Cid = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (Request.QueryString["mcid"] != null)
        {
            if (!IsPostBack)
            {
                Master.Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle() + "网页课件添加";
                missionview();
            }
                Cid = Request.QueryString["mcid"].ToString();
        }
        else
        {
            Response.Redirect("~/teacher/course.aspx", false);
        }
    }
    private void missionview()
    {
        if (Request.QueryString["mid"] != null)
        {
            int Mid = Int32.Parse(Request.QueryString["mid"].ToString());
            LearnSite.Model.Mission mission = new LearnSite.Model.Mission();
            LearnSite.BLL.Mission missionbll = new LearnSite.BLL.Mission();
            mission = missionbll.GetModel(Mid);
            CheckPublish.Checked = mission.Mpublish;
            Texttitle.Text = mission.Mtitle;

            if (mission.Mback != "")
            {
                TextBoxHtml.Text = Server.UrlDecode(mission.Mback);
            }
        }
    }
    protected void Btnedit_Click(object sender, EventArgs e)
    {
        if (Texttitle.Text != "" && TextBoxHtml.Text != "")
        {
            if (Request.QueryString["mcid"] != null)
            {
                int Mid = Int32.Parse(Request.QueryString["mid"].ToString());
                string Mcidstr = Request.QueryString["mcid"].ToString();
                int Mcid = Int32.Parse(Mcidstr);
                LearnSite.BLL.Mission missionbll = new LearnSite.BLL.Mission();
                LearnSite.BLL.ListMenu lbll = new LearnSite.BLL.ListMenu();
                int maxSort = lbll.GetMaxLsort(Mcid) + 1;

                LearnSite.Model.Mission mission = new LearnSite.Model.Mission();
                mission.Mid = Mid;
                mission.Mcid = Mcid;
                mission.Mtitle = HttpUtility.HtmlEncode(Texttitle.Text.Trim());
                mission.Msort = maxSort;
                mission.Mupload = true;
                mission.Mcategory = 38;//网页课件
                mission.Mback = TextBoxHtml.Text;

                mission.Mpublish = CheckPublish.Checked;
                mission.Mcontent = "";
                mission.Mfiletype = "ware";
                mission.Mdate = DateTime.Now;
                mission.Mhit = 0;
                mission.Mgroup = false;
                
                missionbll.Update(mission);

                LearnSite.Model.ListMenu lmodel = new LearnSite.Model.ListMenu();
                lmodel.Lcid = Mcid;
                lmodel.Lshow = CheckPublish.Checked;
                lmodel.Lsort = maxSort;
                lmodel.Ltitle = Texttitle.Text.Trim();
                lmodel.Ltype = 38;//网页课件
                lmodel.Lxid = Mid;

                lbll.UpdateMenuThree(lmodel);
                System.Threading.Thread.Sleep(500);
                string url = "~/teacher/courseshow.aspx?cid=" + Mcid.ToString();
                Response.Redirect(url, false);
            }

        }
    }
    protected void BtnCourse_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["mcid"] != null)
        {
            string url = "~/teacher/courseshow.aspx?cid=" + Cid;
            Response.Redirect(url, false);
        }
    }

}