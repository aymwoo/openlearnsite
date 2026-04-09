using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Teacher_missionedit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (Request.QueryString["mcid"] != null && Request.QueryString["mid"] != null && Request.QueryString["lid"] != null)
        {
            if (!IsPostBack)
            {
                Master.Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle() + "学案活动编辑页面";
                ShowTypename();
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
    protected void BtnCourse_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["mcid"] != null && Request.QueryString["mid"] != null && Request.QueryString["lid"] != null)
        {
            string Mcid = Request.QueryString["mcid"].ToString();
            string Mid = Request.QueryString["mid"].ToString();
            string Lid = Request.QueryString["lid"].ToString();
            string url = "~/teacher/missionshow.aspx?mcid=" + Mcid + "&mid=" + Mid + "&lid=" + Lid;
            Response.Redirect(url, false);
        }
    }
    protected void Btnedit_Click(object sender, EventArgs e)
    {
        string payloadEditorContent = Request.Form["editorContentPayload"] ?? string.Empty;
        string formTextareaContent = Request.Form[mcontent.UniqueID] ?? string.Empty;
        string serverTextareaContent = mcontent.Value ?? string.Empty;
        string rawEditorContent = !string.IsNullOrWhiteSpace(payloadEditorContent)
            ? payloadEditorContent
            : (!string.IsNullOrWhiteSpace(formTextareaContent) ? formTextareaContent : serverTextareaContent);
        string fckstr = LearnSite.Common.MarkdownContentGuard.NormalizeCodeFences(rawEditorContent.Trim());
        string formTitle = Request.Form[Texttitle.UniqueID] ?? string.Empty;
        string title = !string.IsNullOrEmpty(formTitle) ? formTitle.Trim() : (Texttitle.Text ?? string.Empty).Trim();

        if (title != "" && fckstr != "")
        {
            if (Request.QueryString["mcid"] != null && Request.QueryString["mid"] != null && Request.QueryString["lid"] != null)
            {
                string Mcid = Request.QueryString["mcid"].ToString();
                string Mid = Request.QueryString["mid"].ToString();
                string Lid = Request.QueryString["lid"].ToString();

                string coursePath = LearnSite.Store.CourseStore.CoursePath(Mcid);
                if (CheckRemote.Checked)
                    fckstr = LearnSite.Common.ImageDown.UploadRemote(fckstr, coursePath);

                LearnSite.Model.Mission mission = new LearnSite.Model.Mission();
                mission.Mid = Int32.Parse(Mid);
                mission.Mtitle = HttpUtility.HtmlEncode(title);
                bool uploadcan = CheckUpload.Checked;
                mission.Mupload = uploadcan;
                if (uploadcan)
                    mission.Mcategory = 0;//有作业提交
                else
                    mission.Mcategory = 1;//无作业提交
                mission.Mexample = "";
                mission.Microworld = CheckMicoWorld.Checked;

                mission.Mpublish = CheckPublish.Checked;
                mission.Mcontent = HttpUtility.HtmlEncode(fckstr);
                mission.Mfiletype = DDLmfiletype.SelectedValue;
                mission.Mdate = DateTime.Now;
                mission.Mhit = 0;
                mission.Mgroup = CheckGroup.Checked;
                if (DDLMgid.SelectedValue != "")
                    mission.Mgid = Int32.Parse(DDLMgid.SelectedValue);
                else
                    mission.Mgid = 0;
                LearnSite.BLL.Mission missionbll = new LearnSite.BLL.Mission();
                missionbll.Update(mission);

                LearnSite.Model.ListMenu lmodel = new LearnSite.Model.ListMenu();
                LearnSite.BLL.ListMenu lbll = new LearnSite.BLL.ListMenu();

                lmodel.Lid = Int32.Parse(Lid);
                if (uploadcan)
                    lmodel.Ltype = 1;
                else
                    lmodel.Ltype = 6;//描述页面
                lmodel.Lshow = CheckPublish.Checked;
                lmodel.Ltitle = title;
                lbll.UpdateMenuMission(lmodel);//专用活动分类更新

                System.Threading.Thread.Sleep(500);
                string url = "~/teacher/missionshow.aspx?mcid=" + Mcid + "&mid=" + Mid + "&lid=" + Lid;
                Response.Redirect(url, false);
            }
            else
            {
                Labelmsg.Text = "取不到活动编号Mid！";
            }
        }
        else
        {
            if (string.IsNullOrEmpty(title))
            {
                Labelmsg.Text = "活动标题不能为空！";
            }
            else
            {
                string syncSource = Request.Form["editorSyncSource"] ?? "unknown";
                string syncLength = Request.Form["editorSyncLength"] ?? "0";
                Labelmsg.Text = "活动说明不能为空！当前同步来源：" + syncSource + "，同步长度：" + syncLength + "，payload长度：" + payloadEditorContent.Length + "，form长度：" + formTextareaContent.Length + "，server长度：" + serverTextareaContent.Length + "。";
            }
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
            mcontent.Value = HttpUtility.HtmlDecode(mission.Mcontent);
            CheckMicoWorld.Checked = mission.Microworld;

            DDLmfiletype.SelectedValue = mission.Mfiletype;
            CheckPublish.Checked = mission.Mpublish;
            Texttitle.Text = mission.Mtitle;
            CheckUpload.Checked = mission.Mupload;
            CheckGroup.Checked = mission.Mgroup;
            string mgid = mission.Mgid.ToString();
            if (DDLMgid.Items.FindByValue(mgid) != null)
                DDLMgid.SelectedValue = mgid;
        }
    }
    private void ShowTypename()
    {
        DDLmfiletype.DataSource = LearnSite.Common.TypeNameList.WorksType();
        DDLmfiletype.DataBind();
    }
}
