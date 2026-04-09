using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class student_iframe : System.Web.UI.Page
{
    private const string DefaultIframeUrl = "https://image.baidu.com";
    protected string Id = "";
    protected string Owner = "";
    protected string Fpage = "#";
    protected string Mcontents = "";
    protected string codefile = "";
    protected string Snum = "";
    protected string Mypage = "";
    protected string Mexample = DefaultIframeUrl;
    protected string Lid = "";
    protected string Ext = "psd";


    protected void Page_Load(object sender, EventArgs e)
    {
        if (LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            if (Request.QueryString["lid"] != null)
            {
                LearnSite.Common.CookieHelp.KickStudent();
                if (!IsPostBack)
                {
                    ShowMission();
                }
            }
        }
        else
        {
            LearnSite.Common.CookieHelp.JudgeStudentCookies();
        }
    }

    private void ShowMission()
    {
        Lid = Request.QueryString["lid"].ToString();
        if (LearnSite.Common.WordProcess.IsNum(Lid))
        {
            LearnSite.BLL.ListMenu lbll = new LearnSite.BLL.ListMenu();
            LearnSite.Model.ListMenu lmodel = new LearnSite.Model.ListMenu();
            lmodel = lbll.GetModel(Int32.Parse(Lid));

            string Cid = lmodel.Lcid.ToString();
            string Mid = lmodel.Lxid.ToString();
            Id = Cid + "-" + Mid + "-" + Lid;
            int mill = DateTime.Now.Millisecond;
            Fpage = "../student/program.aspx?lid=" + Lid + "&mill=" + mill;
            LearnSite.Model.Mission model = new LearnSite.Model.Mission();
            LearnSite.BLL.Mission mn = new LearnSite.BLL.Mission();
            model = mn.GetModel(Int32.Parse(Mid));
            if (model != null)
            {
                if (!String.IsNullOrEmpty(model.Mexample))
                {
                    string missionUrl = model.Mexample.Trim();
                    if (LearnSite.Common.IframeUrlHelper.IsAllowed(missionUrl))
                    {
                        Mexample = ResolveIframeUrl(missionUrl);
                    }
                }
                Mcontents = model.Mcontent;
                //Ext = model.Mfiletype;
            }
        }
    }

    private string ResolveIframeUrl(string url)
    {
        if (url.StartsWith("~/"))
        {
            return ResolveUrl(url);
        }

        return url;
    }
}
