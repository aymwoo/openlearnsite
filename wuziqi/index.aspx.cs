using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class wuziqi_index : System.Web.UI.Page
{
    protected string gsave = "0";
    protected string gnote = "";
    protected string gstart = "0";
    protected string gpass = "0";
    protected string grank = "";
    protected string guser = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ShowBox();
        }
    }
    private void ShowBox()
    {
        LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
        guser = Server.UrlDecode(cook.Sname);

        LearnSite.BLL.Game bll = new LearnSite.BLL.Game();
        int sid = cook.Sid;
        string title = "wuziqi";
        LearnSite.Model.Game model = new LearnSite.Model.Game();
        model = bll.GetModelGameMax(sid, title);
        if (model != null)
        {
            gsave = model.Gsave.ToString();//Level难度
            gnote = model.Gnote;//history
        }

        grank = bll.GetWuziqiRank(50, title);
        int timepass = LearnSite.Common.Computer.TimePassed();
        gpass = timepass.ToString();
    }
}