using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class teacher_wareadd : System.Web.UI.Page
{
    protected string Cid = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (Request.QueryString["mcid"] != null)
        {
            if (!IsPostBack)
            {
                Master.Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle() + "网页课件";                
            }
            Cid = Request.QueryString["mcid"].ToString();
        }
        else
        {
            Response.Redirect("~/teacher/course.aspx", false);
        }
    }
    protected void Btnadd_Click(object sender, EventArgs e)
    {
        if (Texttitle.Text != "" && TextBoxHtml.Text != "")
        {
            if (Request.QueryString["mcid"] != null)
            {
                string Mcidstr = Request.QueryString["mcid"].ToString();
                int Mcid = Int32.Parse(Mcidstr);
                LearnSite.BLL.Mission missionbll = new LearnSite.BLL.Mission();
                LearnSite.BLL.ListMenu lbll = new LearnSite.BLL.ListMenu();
                int maxSort = lbll.GetMaxLsort(Mcid) + 1;

                LearnSite.Model.Mission mission = new LearnSite.Model.Mission();
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
                
                int mid = missionbll.Add(mission);
                LearnSite.Model.ListMenu lmodel = new LearnSite.Model.ListMenu();
                lmodel.Lcid = Mcid;
                lmodel.Lshow = CheckPublish.Checked;
                lmodel.Lsort = maxSort;
                lmodel.Ltitle = Texttitle.Text.Trim();
                lmodel.Ltype = 38;//网页课件
                lmodel.Lxid = mid;
                lbll.Add(lmodel);
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