using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class student_htmledit : System.Web.UI.Page
{
    protected string Id = " ";
    protected string Owner = " ";
    protected string Mcontents = " ";
    protected string Snum = " ";

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

            string Mid = lmodel.Lxid.ToString();

            LearnSite.Model.Mission model = new LearnSite.Model.Mission();
            LearnSite.BLL.Mission mn = new LearnSite.BLL.Mission();
            model = mn.GetModel(Int32.Parse(Mid));
            if (model != null)
            {
                LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
                Mcontents = HttpUtility.HtmlDecode(model.Mcontent);
                Owner = HttpUtility.UrlDecode(cook.Sname);
                Snum = cook.Snum;
            }
        }
    }
    /// <summary>
    /// 字符串转base64
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private string Base64(string str)
    {
        byte[] b = System.Text.Encoding.Default.GetBytes(str);
        //转成 Base64 形式的 System.String  
        str = Convert.ToBase64String(b);
        return str;

    }
}