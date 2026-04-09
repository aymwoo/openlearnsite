using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class Student_myfinger : System.Web.UI.Page
{
    protected string mysnum;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            if (!IsPostBack)
            {
                showSnum();
                // 控制游戏链接显示
                ControlGamesDisplay();
            }
        }
        else
        {
            LearnSite.Common.CookieHelp.JudgeStudentCookies();
        }
    }

    /// <summary>
    /// 控制游戏链接显示
    /// </summary>
    private void ControlGamesDisplay()
    {
        // 从website.xml中获取游戏开关状态
        string gamesSetting = LearnSite.Common.XmlHelp.GetTypeName("EnableGames");
        bool gamesEnabled = true; // 默认启用

        if (!string.IsNullOrEmpty(gamesSetting))
        {
            bool.TryParse(gamesSetting, out gamesEnabled);
        }

        // 根据开关状态显示或隐藏游戏链接
        gamesDiv.Visible = gamesEnabled;
    }
    private void showSnum()
    {
        LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
        mysnum = cook.Snum;
        LearnSite.BLL.Pfinger fbll = new LearnSite.BLL.Pfinger();
        oldspd.InnerText = "历史记录：" + fbll.GetPsnum(mysnum) + "个/分";
    }

}
