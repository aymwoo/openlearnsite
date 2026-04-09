using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Student_ware : System.Web.UI.Page
{
    LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
    protected string WareUrl = "";
    protected string Id = "";


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
            LearnSite.Model.Mission model = new LearnSite.Model.Mission();
            LearnSite.BLL.Mission mn = new LearnSite.BLL.Mission();
            model = mn.GetModel(Int32.Parse(Mid));
            if (model != null)
            {
                LabelMtitle.Text = model.Mtitle;
                if (!String.IsNullOrEmpty(model.Mback))
                {
                    WareUrl = model.Mback;
                }
            }
            LearnSite.BLL.Works bll = new LearnSite.BLL.Works();
            LearnSite.Model.Works work = new LearnSite.Model.Works();
            string Snum = cook.Snum;
            work = bll.GetModelByStu(Int32.Parse(Mid), Snum);
            if (work != null)
            {
                string Wthumbnail = work.Wthumbnail;
                if (!string.IsNullOrEmpty(Wthumbnail))
                {
                    Thumbnail.ImageUrl = Wthumbnail + "?temp=" + DateTime.Now.Millisecond.ToString();
                }
                else
                {
                    Thumbnail.ImageUrl = "~/images/finished.png";
                }
            }

        }
    }

}
