using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Teacher_missionadd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (Request.QueryString["mcid"] != null)
        {
            if (!IsPostBack)
            {
                Master.Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle() + "学案活动添加页面";
                ShowTypename();
                ShowMgid();
                ShowCourseFiled();
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
    private void ShowMgid()
    {
        LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();
        int hid = tcook.Hid;

        LearnSite.BLL.Gauge gbll = new LearnSite.BLL.Gauge();
        DDLMgid.DataSource=gbll.GetListGauge(hid);
        DDLMgid.DataTextField = "Gtitle";
        DDLMgid.DataValueField = "Gid";
        DDLMgid.DataBind();
    }

    private void ShowCourseFiled()
    {
        if (Request.QueryString["mcid"] != null)
        {
            int Mcid = Int32.Parse(Request.QueryString["mcid"].ToString());
            LearnSite.BLL.Courses cbll = new LearnSite.BLL.Courses();
            LearnSite.Model.Courses cmodel = new LearnSite.Model.Courses();
            cmodel = cbll.GetModel(Mcid);
            DDLmfiletype.SelectedValue = cmodel.Cfiletype;
        }
    }
    protected void Btnadd_Click(object sender, EventArgs e)
    {
        string payloadEditorContent = Request.Form["editorContentPayload"] ?? string.Empty;
        string rawEditorContent = !string.IsNullOrWhiteSpace(payloadEditorContent)
            ? payloadEditorContent
            : (Request.Form["textareaItem"] ?? string.Empty);
        string fckstr = LearnSite.Common.MarkdownContentGuard.NormalizeCodeFences(rawEditorContent.Trim());
        string formTitle = Request.Form[Texttitle.UniqueID] ?? string.Empty;
        string title = !string.IsNullOrEmpty(formTitle) ? formTitle.Trim() : (Texttitle.Text ?? string.Empty).Trim();
        string fileType = DDLmfiletype.SelectedValue ?? string.Empty;
        string gaugeId = DDLMgid.SelectedValue ?? string.Empty;

        if (title != "" && fckstr != "")
        {
            if (Request.QueryString["mcid"] != null)
            {
                string Mcidstr = Request.QueryString["mcid"].ToString();
                int Mcid = Int32.Parse(Mcidstr);
                string coursePath = LearnSite.Store.CourseStore.CoursePath(Mcidstr);
                if (CheckRemote.Checked)
                    fckstr = LearnSite.Common.ImageDown.UploadRemote(fckstr, coursePath);
                LearnSite.BLL.Mission missionbll = new LearnSite.BLL.Mission();
                LearnSite.Model.Mission mission = new LearnSite.Model.Mission();
                LearnSite.BLL.ListMenu lbll = new LearnSite.BLL.ListMenu();
                int maxSort = lbll.GetMaxLsort(Mcid) + 1;
                mission.Mcid = Mcid;
                mission.Mtitle = HttpUtility.HtmlEncode(title);
                mission.Msort = maxSort;
                bool uploadcan= CheckUpload.Checked;
                mission.Mupload = uploadcan;
                if (uploadcan)
                    mission.Mcategory = 0;//有作业提交
                else
                    mission.Mcategory = 1;//无作业提交
                mission.Mexample = "";
                mission.Microworld = CheckMicoWorld.Checked;

                mission.Mpublish = CheckPublish.Checked;
                mission.Mcontent = HttpUtility.HtmlEncode(fckstr);
                mission.Mfiletype = fileType;
                mission.Mdate = DateTime.Now;
                mission.Mhit = 0;
                mission.Mgroup = CheckGroup.Checked;
                if (gaugeId != "")
                    mission.Mgid = Int32.Parse(gaugeId);
                else
                    mission.Mgid = 0;
                int mid= missionbll.Add(mission);
                LearnSite.Model.ListMenu lmodel = new LearnSite.Model.ListMenu();
                lmodel.Lcid = Mcid;
                lmodel.Lshow = CheckPublish.Checked;
                lmodel.Lsort = maxSort;
                lmodel.Ltitle = title;
                if (uploadcan)
                    lmodel.Ltype = 1;
                else
                    lmodel.Ltype = 6;//描述页面
                lmodel.Lxid = mid;
                lbll.Add(lmodel);
                System.Threading.Thread.Sleep(500);
                string url = "~/teacher/courseshow.aspx?cid=" + Mcid.ToString();
                Response.Redirect(url, false);
            }
        }
        else
        {
            string toastMessage = string.IsNullOrEmpty(title)
                ? "活动标题不能为空！"
                : "活动说明不能为空！";
            string toastScript = "window.setTimeout(function(){if(window.showToast){window.showToast('" + toastMessage + "', 'error');}}, 0);";
            ClientScript.RegisterStartupScript(GetType(), "missionadd-empty", toastScript, true);
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
    private void ShowTypename()
    {
        DDLmfiletype.DataSource = LearnSite.Common.TypeNameList.WorksType();
        DDLmfiletype.DataBind();
    }
}
