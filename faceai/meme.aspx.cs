using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class faceai_meme : System.Web.UI.Page
{
    protected string Id = "";
    protected int Lid ;

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
        Lid = Int32.Parse(Request.QueryString["lid"].ToString());
        LearnSite.BLL.ListMenu lbll = new LearnSite.BLL.ListMenu();
        LearnSite.Model.ListMenu lmodel = new LearnSite.Model.ListMenu();
        lmodel = lbll.GetModel(Lid);

        string Cid = lmodel.Lcid.ToString();
        string Mid = lmodel.Lxid.ToString();
        Id = Cid + "-" + Mid + "-" + Lid;

    }

}