using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class student_markdown : System.Web.UI.Page
{
    protected string Id = " ";
    protected string Owner = " ";
    protected string Fpage = "#";
    protected string Mcontents = " ";
    protected string codefile = "";
    protected string Snum = " ";
    protected string Mypage = "";
    protected string Mytitle = "新文档";

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
            Fpage = "../student/program.aspx?lid=" + Lid + "&mill=" + mill;
            LearnSite.Model.Mission model = new LearnSite.Model.Mission();
            LearnSite.BLL.Mission mn = new LearnSite.BLL.Mission();
            model = mn.GetModel(Int32.Parse(Mid));
            if (model != null)
            {
                LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
                string Sname = cook.Sname;
                Snum = cook.Snum;

                Mcontents = HttpUtility.HtmlDecode(model.Mcontent);
                Owner = HttpUtility.UrlDecode(Sname);
                LearnSite.BLL.Works wbll = new LearnSite.BLL.Works();
                LearnSite.Model.Works wmodel = new LearnSite.Model.Works();
                wmodel = wbll.GetModelByStu(Int32.Parse(Mid), Snum);
                if (wmodel != null)
                {
                    Mytitle = wmodel.Wtitle;
                    codefile = wmodel.Wcode;//如果不为空，则获取原来的作品链接
                }
            }
        }
    }
}