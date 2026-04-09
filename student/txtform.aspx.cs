using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Student_txtform : System.Web.UI.Page
{
    protected string Mid = "0";
    protected string Lid = "0";
    protected string Done = "false";
    protected string Snum = "";
    protected string Sname = "";
    protected string Sgroup = "";
    protected string Collabo = "false";
    protected string serverIp = "";


    protected void Page_Load(object sender, EventArgs e)
    {
        if (LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            if (Request.QueryString["lid"] != null)
            {
                LearnSite.Common.CookieHelp.KickStudent();
                if (!IsPostBack)
                {
                    showMid();
                    ShowTxtForm();
                }
            }
        }
        else
        {
            LearnSite.Common.CookieHelp.JudgeStudentCookies();
        }
    }

    protected void showMid()
    {
         Lid = Request.QueryString["lid"].ToString();
         if (LearnSite.Common.WordProcess.IsNum(Lid))
         {
             LearnSite.BLL.ListMenu lbll = new LearnSite.BLL.ListMenu();
             LearnSite.Model.ListMenu lmodel = new LearnSite.Model.ListMenu();
             lmodel = lbll.GetModel(Int32.Parse(Lid));
             Mid = lmodel.Lxid.ToString();
         }
    }

    private void ShowTxtForm()
    {
        LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
        int Wsid = cook.Sid;
        Snum = cook.Snum;
        Sname = HttpUtility.UrlDecode(cook.Sname);
        serverIp = LearnSite.Common.Computer.GetServerIp();

        if (LearnSite.Common.WordProcess.IsNum(Mid))
        {
            LearnSite.BLL.Students sbll = new LearnSite.BLL.Students();
            Sgroup = sbll.GetSgroup(cook.Sid).ToString();

            LearnSite.Model.TxtForm tmodel = new LearnSite.Model.TxtForm();
            LearnSite.BLL.TxtForm tbll = new LearnSite.BLL.TxtForm();

            tmodel = tbll.GetModel(Int32.Parse(Mid));
            if (tmodel != null)
            {
                if (tmodel.Mcollabo)
                {
                    Collabo = "true";
                }
                else
                {
                    Collabo = "false";
                }
                LabelMtitle.Text = tmodel.Mtitle;

                LearnSite.Model.TxtFormBack rmodel = new LearnSite.Model.TxtFormBack();
                LearnSite.BLL.TxtFormBack rbll = new LearnSite.BLL.TxtFormBack();
                int Rid = rbll.GetRid(Wsid.ToString(), Mid);

                if (Rid > 0)
                {
                    rmodel = rbll.GetModel(Rid);
                    Mcontent.InnerHtml = HttpUtility.HtmlDecode(rmodel.Rcontent);
                    Done = "true";
                }
                else
                {
                    Mcontent.InnerHtml = HttpUtility.HtmlDecode(tmodel.Mcontent);
                    hiddencount.Value = tmodel.Mcontent.Length.ToString();
                }
            }
            Hlresult.NavigateUrl = "~/student/txtformresult.aspx?mid=" + Mid;
        }
        else {
            Collabo = "false";
        }
    }

    /// <summary>
    /// 字符串转base64
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private string Base64(string str)
    {
        byte[] b = System.Text.Encoding.UTF8.GetBytes(str);
        //转成 Base64 形式的 System.String  
        str = Convert.ToBase64String(b);
        return str;

    }
   
}