using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

public partial class sokoban_index : System.Web.UI.Page
{
    protected string gsave ="0";
    protected string gnote = "";
    protected string gstart = "0";
    protected string gpass = "0";
    protected string grank = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            if (!IsPostBack)
            {
                ShowBox();
            }
        }
        else
        {
            LearnSite.Common.CookieHelp.JudgeStudentCookies();
        }
    }

    private void ShowBox()
    {
        LearnSite.Model.Cook cook = new LearnSite.Model.Cook();

        LearnSite.BLL.Game bll = new LearnSite.BLL.Game();
        int sid = cook.Sid;
        string title = "sokoban";
        LearnSite.Model.Game model = new LearnSite.Model.Game();
        model = bll.GetModelGameMax(sid, title);
        if (model != null)
        {
            gsave = model.Gsave.ToString();
            gnote = model.Gnote;
        }

        grank = bll.GetRank(50, title);

        int timepass = LearnSite.Common.Computer.TimePassed();
        gpass = timepass.ToString();
        if (timepass > 30)
        {
            gstart = "1";
        }
        else
        {
            gstart = "0";
        }

    }
}