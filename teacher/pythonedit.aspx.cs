using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class Teacher_pythonedit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (Request.QueryString["mcid"] != null && Request.QueryString["mid"] != null)
        {
            if (!IsPostBack)
            {
                Master.Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle() + "Python编程修改页面";
                ShowMgid();
                missionview();
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
    protected void Btnedit_Click(object sender, EventArgs e)
    {
        string fckstr = LearnSite.Common.MarkdownContentGuard.NormalizeCodeFences(mcontent.Value);
        if (Texttitle.Text != "" && fckstr != "")
        {
            if (Request.QueryString["mcid"] != null && Request.QueryString["mid"] != null)
            {
                string Mcid = Request.QueryString["mcid"].ToString();
                string Mid = Request.QueryString["mid"].ToString();

                LearnSite.Model.Mission mission = new LearnSite.Model.Mission();
                mission.Mid = Int32.Parse(Mid);
                mission.Mtitle = HttpUtility.HtmlEncode(Texttitle.Text.Trim());
                mission.Mupload = true;
                int oldLtype=Int32.Parse(LabelLtype.Text);
                mission.Mcategory = 8;//python编程页面
                if (Checkblock.Checked)
                {
                    mission.Mcategory = 13;//python拼图编程页面
                }
                if (Checkblockpy.Checked)
                {
                    mission.Mcategory = 14;//python积木编程页面
                }   
                mission.Mpublish = CheckPublish.Checked;
                mission.Mcontent = HttpUtility.HtmlEncode(fckstr);
                mission.Mfiletype = "py";
                mission.Mdate = DateTime.Now;
                mission.Mhit = 0;
                mission.Mgroup = false;
                if (DDLMgid.SelectedValue != "")
                    mission.Mgid = Int32.Parse(DDLMgid.SelectedValue);
                else
                    mission.Mgid = 0;

                mission.Mexample = HlExample.NavigateUrl;

                if (Fupload.HasFile)
                {
                    string pyfilename = Fupload.FileName;
                    string savePath = LearnSite.Store.CourseStore.GetSaveUrl("Course", Mcid);
                    string shortFileName = Path.GetFileName(pyfilename);
                    string savefilename = savePath + shortFileName;
                    string pypath = this.Server.MapPath(savefilename);
                    Fupload.SaveAs(pypath);
                    mission.Mexample = savefilename;
                }

                mission.Microworld = CheckBack.Checked;
                mission.Mhelp = Checkhelp.Checked;

                LearnSite.BLL.Mission missionbll = new LearnSite.BLL.Mission();
                missionbll.Update(mission);

                LearnSite.Model.ListMenu lmodel = new LearnSite.Model.ListMenu();
                LearnSite.BLL.ListMenu lbll = new LearnSite.BLL.ListMenu();

                lmodel.Lcid = Int32.Parse(Mcid);
                lmodel.Lxid = Int32.Parse(Mid);
                lmodel.Ltype = 8;//python编程页面
                if (Checkblock.Checked)
                {
                    lmodel.Ltype = 13;//python拼图编程页面
                }
                if (Checkblockpy.Checked)
                {
                    lmodel.Ltype = 14;//python拼图编程页面
                }
                lmodel.Lshow = CheckPublish.Checked;
                lmodel.Ltitle = Texttitle.Text.Trim();
                lbll.UpdateMenuThree(lmodel,oldLtype);
                System.Threading.Thread.Sleep(500);
                string url = "~/teacher/pythonshow.aspx?mcid=" + Mcid + "&mid=" + Mid;
                Response.Redirect(url, false);
            }
            else
            {
                Labelmsg.Text = "取不到主题编号Mid！";
            }
        }
        else
        {
            Labelmsg.Text = "内容及标题不能为空！";
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
            mcontent.Value = HttpUtility.HtmlDecode(mission.Mcontent);
            
            string mgid = mission.Mgid.ToString();
            if (DDLMgid.Items.FindByValue(mgid) != null)
                DDLMgid.SelectedValue = mgid;

            CheckBack.Checked = mission.Microworld;
            Checkhelp.Checked = mission.Mhelp;
            LabelLtype.Text = mission.Mcategory.ToString();//保存旧的活动类型
            Checkblock.Checked = false;
            Checkblockpy.Checked = false;
            if (mission.Mcategory == 13)
            {
                Checkblock.Checked = true;
            }
            if (mission.Mcategory == 14)
            {
                Checkblockpy.Checked = true;
            }
            if (!string.IsNullOrEmpty(mission.Mexample))
            {
                HlExample.NavigateUrl = mission.Mexample;
            }
            else
            {
                HlExample.NavigateUrl = "";
            }
        }
    }
    protected void BtnCourse_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["mcid"] != null && Request.QueryString["mid"] != null)
        {
            string Mcid = Request.QueryString["mcid"].ToString();
            string url = "~/teacher/courseshow.aspx?cid=" + Mcid.ToString();
            Response.Redirect(url, false);
        }
    }
}
