using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Teacher_pixeledit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (Request.QueryString["mcid"] != null && Request.QueryString["mid"] != null)
        {
            if (!IsPostBack)
            {
                Master.Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle() + "自定义主题修改页面";
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

                mission.Mpublish = CheckPublish.Checked;
                mission.Mcontent = HttpUtility.HtmlEncode(fckstr);
                string titleValue = DDLTitle.SelectedValue;
                LearnSite.Common.CustomActivityExampleResult exampleResult = LearnSite.Common.CustomActivityCatalog.BuildExampleValue(titleValue, GetSelectedDeviceValues(), Texturl.Text);
                if (!exampleResult.IsValid)
                {
                    Labelmsg.Text = exampleResult.ErrorMessage;
                    return;
                }
                mission.Mfiletype = LearnSite.Common.CustomActivityCatalog.GetFileType(titleValue);
                mission.Mcategory = Int32.Parse(titleValue);//自定义主题页面
                mission.Mdate = DateTime.Now;
                mission.Mhit = 0;
                mission.Mgroup = false;
                if (DDLMgid.SelectedValue != "")
                    mission.Mgid = Int32.Parse(DDLMgid.SelectedValue);
                else
                    mission.Mgid = 0;

                mission.Mexample = exampleResult.ExampleValue;//编程实例

                LearnSite.BLL.Mission missionbll = new LearnSite.BLL.Mission();
                missionbll.Update(mission);

                LearnSite.Model.ListMenu lmodel = new LearnSite.Model.ListMenu();
                LearnSite.BLL.ListMenu lbll = new LearnSite.BLL.ListMenu();

                lmodel.Lcid = Int32.Parse(Mcid);
                lmodel.Lxid = Int32.Parse(Mid);
                lmodel.Ltype = Int32.Parse(titleValue);//页面类型为 自定义主题
                lmodel.Lshow = CheckPublish.Checked;
                lmodel.Ltitle = Texttitle.Text.Trim();
                lbll.UpdateMenuThree(lmodel);
                System.Threading.Thread.Sleep(500);
                string url = "~/teacher/pixelshow.aspx?mcid=" + Mcid + "&mid=" + Mid;
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
            DDLTitle.SelectedValue = mission.Mcategory.ToString();
            string mgid = mission.Mgid.ToString();
                string exampleurl = mission.Mexample;
            if (DDLMgid.Items.FindByValue(mgid) != null)
                DDLMgid.SelectedValue = mgid;
            if (DDLTitle.SelectedValue == "24")
            {
                PanelDeviceConfig.Visible = true;
                Ckdevice.Visible = true;
                if (exampleurl != "")
                {
                    FillCheckBoxList(exampleurl, Ckdevice);
                }
            }
            else
            {
                PanelDeviceConfig.Visible = false;
                Ckdevice.Visible = false;
            }
            if (DDLTitle.SelectedValue == "34")
            {
                PanelIframeConfig.Visible = true;
                Texturl.Visible = true;
                Texturl.Text = exampleurl;
            }
            else
            {
                PanelIframeConfig.Visible = false;
                Texturl.Visible = false;
            }
        }
    }

    protected string GetActivityDisplayName()
    {
        return GetCurrentActivityMeta().DisplayName;
    }

    protected string GetActivityDescription()
    {
        return GetCurrentActivityMeta().Description;
    }

    protected string GetStudentEntryUrl()
    {
        return GetCurrentActivityMeta().StudentEntryUrl;
    }

    protected string GetEditFocusText()
    {
        return GetCurrentActivityMeta().EditFocus;
    }

    protected string GetActivityIconUrl()
    {
        return ResolveUrl(GetCurrentActivityMeta().IconUrl);
    }

    protected string GetActivityBadgeBackground()
    {
        return GetCurrentActivityMeta().BadgeBackground;
    }

    protected string GetActivityBadgeForeground()
    {
        return GetCurrentActivityMeta().BadgeForeground;
    }

    private LearnSite.Common.CustomActivityMeta GetCurrentActivityMeta()
    {
        return LearnSite.Common.CustomActivityCatalog.GetMeta(DDLTitle.SelectedValue);
    }

    private IEnumerable<string> GetSelectedDeviceValues()
    {
        foreach (ListItem li in Ckdevice.Items)
        {
            if (li.Selected)
            {
                yield return li.Value;
            }
        }
    }

    /// <param name="str">字符串，格式要求为“A,B,C”</param>
    /// <param name="checkBoxList">CheckBoxList控件</param>

    public void FillCheckBoxList(string str, CheckBoxList checkBoxList)
    {
        string[] items = str.Split(',');
        //遍历items
        foreach (string item in items)
        {
            //如果值相等，则选中该项
            foreach (ListItem listItem in checkBoxList.Items)
            {
                if (item == listItem.Value)
                    listItem.Selected = true;
                else
                    continue;
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
