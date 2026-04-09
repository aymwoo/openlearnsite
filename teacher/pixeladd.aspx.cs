using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Teacher_pixeladd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (Request.QueryString["mcid"] != null)
        {
            if (!IsPostBack)
            {
                Master.Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle() + "自定义主题添加";
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

                mission.Mexample = exampleResult.ExampleValue;

                int mid = missionbll.Add(mission);
                LearnSite.Model.ListMenu lmodel = new LearnSite.Model.ListMenu();
                lmodel.Lcid = Mcid;
                lmodel.Lshow = CheckPublish.Checked;
                lmodel.Lsort = maxSort;
                lmodel.Ltitle = Texttitle.Text.Trim();
                lmodel.Ltype = Int32.Parse(titleValue);//页面类型为 自定义主题
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
    protected void DDLTitle_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDLTitle.SelectedValue == "24")
        {
            Ckdevice.Visible = true;
        }
        else {
            Ckdevice.Visible = false;
        }
        if (DDLTitle.SelectedValue == "34")
        {
            Texturl.Visible = true;
        }
        else
        {
            Texturl.Visible = false;
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
}
