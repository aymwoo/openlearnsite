using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class student_mqtt : System.Web.UI.Page
{
    protected string Id = "";
    protected string Fpage = "#";
    protected string Mcontents = "";
    protected string Titles = "";
    protected string Snum = "";
    protected string workDevice = "";
    protected string serverIp = "";

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
        string Lid = Request.QueryString["lid"].ToString();
        if (LearnSite.Common.WordProcess.IsNum(Lid))
        {
            LearnSite.BLL.ListMenu lbll = new LearnSite.BLL.ListMenu();
            LearnSite.Model.ListMenu lmodel = new LearnSite.Model.ListMenu();
            lmodel = lbll.GetModel(Int32.Parse(Lid));

            string Cid = lmodel.Lcid.ToString();
            string Mid = lmodel.Lxid.ToString();
            Id = Cid + "-" + Mid + "-" + Lid;
            int mill = DateTime.Now.Millisecond;
            Fpage = "program.aspx?lid=" + Lid + "&mill=" + mill;

            LearnSite.Model.Mission model = new LearnSite.Model.Mission();
            LearnSite.BLL.Mission mn = new LearnSite.BLL.Mission();
            model = mn.GetModel(Int32.Parse(Mid));
            if (model != null)
            {
                LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
                string Sname = cook.Sname;
                Snum = cook.Snum;

                Mcontents = HttpUtility.HtmlDecode(model.Mcontent);
                Titles = model.Mtitle;

                workDevice = model.Mexample;

                this.Page.Title = HttpUtility.UrlDecode(Sname) + " " + Snum + " " + model.Mtitle;
                serverIp = LearnSite.Common.Computer.GetServerIp();
            }
        }

    }

}