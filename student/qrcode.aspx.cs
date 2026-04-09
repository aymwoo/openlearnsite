using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class student_qrcode : System.Web.UI.Page
{
    protected string Id = "";
    protected string Owner = "";
    protected string Fpage = "#";
    protected string Mcontents = "";
    protected string Titles = "";
    protected string Words = "中国";
    protected string Thumb = "";
    protected string Snum = "";
    protected string mback = "";
    protected string LsSname = "";
    protected string LsSgrade = "0";
    protected string LsSclass = "0";
    protected string LsSid = "0";
    protected string LsCid = "0";
    protected string LsLid = "0";
    protected string LsLtitle = "";
    protected string LsLtype = "";

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

            LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
            if (cook.IsExist())
            {
                LsSname = cook.Sname;
                LsSgrade = cook.Sgrade.ToString();
                LsSclass = cook.Sclass.ToString();
                LsSid = cook.Sid.ToString();
                LsCid = lmodel.Lcid.ToString();
                LsLid = Lid;
                LsLtitle = lmodel.Ltitle;
                LsLtype = lmodel.Ltype.ToString();
            }

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
                string Sname = cook.Sname;
                Snum = cook.Snum;

                Mcontents = HttpUtility.HtmlDecode(model.Mcontent);
                Owner = HttpUtility.UrlDecode(Sname);
                Titles = model.Mtitle;
                LearnSite.BLL.Works wbll = new LearnSite.BLL.Works();
                LearnSite.Model.Works wmodel = new LearnSite.Model.Works();
                wmodel = wbll.GetModelByStu(Int32.Parse(Mid), Snum);
                if (wmodel != null)
                {
                    Words = wmodel.Wcode;
                    Thumb = wmodel.Wthumbnail;
                }

                this.Page.Title = HttpUtility.UrlDecode(Sname) + " " + Snum + " " + model.Mtitle;
            }
        }

    }

}
