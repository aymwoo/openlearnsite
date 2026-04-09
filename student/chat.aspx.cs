using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class student_chat : System.Web.UI.Page
{
    protected string Head = "";
    protected string Sname = "";
    protected string Snum = "";
    protected string Sgtitle = "";
    protected int Sgroup = 0;
    protected string serverIp = "";

    protected string History = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (LearnSite.Common.CookieHelp.IsStudentLogin())
        {
                LearnSite.Common.CookieHelp.KickStudent();
                if (!IsPostBack)
                {
                    showInfo();
                }
        }
        else
        {
            LearnSite.Common.CookieHelp.JudgeStudentCookies();
        }
    }


    private void showInfo() {
        LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
        Sname = Server.UrlDecode(cook.Sname);
        Snum = cook.Snum;
        string Sex = Server.UrlDecode(cook.Sex);
        string imgurl = LearnSite.Common.Photo.GetStudentPhotoUrl(Snum, Sex);
        Head = imgurl.Replace("~", "..");
        LearnSite.BLL.Students dbll = new LearnSite.BLL.Students();
        Sgroup = dbll.GetSgroup(cook.Sid);
        Sgtitle = dbll.GetMySgtitle(cook.Sid);//根据自己的组号获取小组名称
        Rpteam.DataSource = dbll.Teamer(cook.Sgrade, cook.Sclass,Sgroup, Snum, Sname, Sex);
        Rpteam.DataBind();

        Rpemo.DataSource = dbll.Emo();
        Rpemo.DataBind();

        LearnSite.Common.chathistory.DiskInfo df = new LearnSite.Common.chathistory.DiskInfo(cook.Sid.ToString());
        Rpfile.DataSource = df.Dw;
        Rpfile.DataBind();

        History = LearnSite.Common.chathistory.get();
        if (History != "")
        {
            History = HttpUtility.UrlEncode(History).Replace("+", "%20");
        }
        serverIp = LearnSite.Common.Computer.GetServerIp();
    }
}