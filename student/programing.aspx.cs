using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public partial class Student_programing : System.Web.UI.Page
{
    protected string Filename = "";
    protected string Id = "";
    protected string Microworld = "false";
    protected string Owner = "";
    protected string Fpage = "#";
    protected string Mcontents = "";
    protected string Titles = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            if (Request.QueryString["mid"] != null && Request.QueryString["cid"] != null)
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
        string Mid = Request.QueryString["mid"].ToString();
        string Cid = Request.QueryString["cid"].ToString();
        Id = Cid + "-" + Mid;
        int mill= DateTime.Now.Millisecond;
        Fpage = "program.aspx?cid=" + Cid + "&mid=" + Mid + "&mill=" + mill;
                
        if (LearnSite.Common.WordProcess.IsNum(Mid))
        {
            LearnSite.Model.Mission model = new LearnSite.Model.Mission();
            LearnSite.BLL.Mission mn = new LearnSite.BLL.Mission();
            model = mn.GetModel(Int32.Parse(Mid));
            if (model != null)
            {
                LearnSite.Model.Cook cook = new LearnSite.Model.Cook();

                string Sname = cook.Sname;
                string Snum = cook.Snum; 
                
                this.Page.Title = HttpUtility.UrlDecode(Sname) + " " + model.Mtitle + "—>" + DateTime.Now.ToString();
                Microworld = model.Microworld.ToString().ToLower();
                Mcontents =HttpUtility.HtmlDecode(model.Mcontent);
                Owner = HttpUtility.UrlDecode(Sname);
                Titles = model.Mtitle;
                Filename = LearnSite.Common.WordProcess.getshortfname(model.Mexample);
                LearnSite.BLL.Works wbll = new LearnSite.BLL.Works();
                if (!Snum.Contains("s"))
                {
                    if (wbll.ExistsMyMissonWork(Int32.Parse(Mid), Snum))
                        Filename = Owner + " " + Filename;
                }
                else
                    Owner = "老师";
            }
        }

    }
}