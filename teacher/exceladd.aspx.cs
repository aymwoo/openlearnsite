using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class teacher_exceladd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (Request.QueryString["mcid"] != null)
        {
            if (!IsPostBack)
            {
                Master.Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle() + "表格处理主题添加";
                ShowMgid();
            }
        }
        else
        {
            Response.Redirect("~/teacher/course.aspx", false);
        }
    }
    protected string myCid()
    {
        if (Request.QueryString["mcid"] != null)
        {
            return Request.QueryString["mcid"].ToString();
        }
        else
        {
            return "";
        }
    }
    protected void Btnadd_Click(object sender, EventArgs e)
    {
        string fckstr = LearnSite.Common.MarkdownContentGuard.NormalizeCodeFences(Request.Form["textareaItem"].Trim());
        if (Texttitle.Text != "" && fckstr != "")
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
                mission.Mcategory = 16;//表格处理页面

                mission.Mpublish = CheckPublish.Checked;
                mission.Mcontent = HttpUtility.HtmlEncode(fckstr);
                mission.Mfiletype = "sheet";
                mission.Mdate = DateTime.Now;
                mission.Mhit = 0;
                mission.Mgroup = false;
                if (DDLMgid.SelectedValue != "")
                    mission.Mgid = Int32.Parse(DDLMgid.SelectedValue);
                else
                    mission.Mgid = 0;
                string exampleurl = "";//实例路径
                if (Fupload.HasFile)
                {
                    string xmlfilename = Fupload.FileName;
                    string savePath = LearnSite.Store.CourseStore.GetSaveUrl("Course", Mcidstr);
                    string shortFileName = Path.GetFileName(xmlfilename);
                    string savefilename = savePath + shortFileName;
                    string sbpath = this.Server.MapPath(savefilename);
                    Fupload.SaveAs(sbpath);
                    exampleurl = savefilename;
                }
                mission.Mexample = exampleurl;//编程实例

                int mid = missionbll.Add(mission);
                LearnSite.Model.ListMenu lmodel = new LearnSite.Model.ListMenu();
                lmodel.Lcid = Mcid;
                lmodel.Lshow = CheckPublish.Checked;
                lmodel.Lsort = maxSort;
                lmodel.Ltitle = Texttitle.Text.Trim();
                lmodel.Ltype = 16;//页面类型为16 表格处理
                lmodel.Lxid = mid;
                lbll.Add(lmodel);
                System.Threading.Thread.Sleep(500);
                string url = "~/teacher/courseshow.aspx?cid=" + Mcid.ToString();
                Response.Redirect(url, false);
            }

        }
        else
        {
            Labelmsg.Text = "请填写主题！";
        }
    }
    protected void BtnCourse_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["mcid"] != null)
        {
            string Cid = Request.QueryString["mcid"].ToString();
            string url = "~/teacher/courseshow.aspx?cid=" + Cid;
            Response.Redirect(url, false);
        }
    }

    private void ShowMgid()
    {
        LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();
        int hid = tcook.Hid;

        LearnSite.BLL.Gauge gbll = new LearnSite.BLL.Gauge();
        DDLMgid.DataSource = gbll.GetListGauge(hid);
        DDLMgid.DataTextField = "Gtitle";
        DDLMgid.DataValueField = "Gid";
        DDLMgid.DataBind();
    }
}
