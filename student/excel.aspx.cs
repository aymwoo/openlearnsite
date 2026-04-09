using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class student_excel : System.Web.UI.Page
{
    protected string Id = "";
    protected string Owner = "";
    protected string Fpage = "#";
    protected string Mcontents = "";
    protected string Titles = "";
    protected string codefile = "";
    protected string Snum = "";
    protected string mback = "";
    protected string Exampleurl = "";
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
            Fpage = "program.aspx?lid=" + Lid + "&mill=" + mill;

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
                Titles = model.Mtitle;
                string examurl = model.Mexample;
                if (!string.IsNullOrEmpty(model.Mback))
                    mback = model.Mback;
                LearnSite.BLL.Works wbll = new LearnSite.BLL.Works();
                LearnSite.Model.Works wmodel = new LearnSite.Model.Works();
                wmodel = wbll.GetModelByStu(Int32.Parse(Mid), Snum);
                if (wmodel != null)
                {
                    codefile = wmodel.Wcode; //如果不为空，则获取原来的作品链接
                }
                else
                {
                    if (!string.IsNullOrEmpty(examurl))
                    {
                        Exampleurl = examurl.Replace("~","../..");
                    }
                }

                this.Page.Title = HttpUtility.UrlDecode(Sname) + " " + Snum + " " + model.Mtitle;
                serverIp = LearnSite.Common.Computer.GetServerIp();
            }
        }

    }
    /*******************
在WEB编程中，经常需要通过JS传递参数给C#后台代码，如果传递的参数包括中文，则需要在JS中通过encodeURIComponent编码，对应C#中的HttpUtility.UrlEncode编码。
1、由于JS中通过encodeURIComponent编码时，将中文或者"="、空格等特殊字符转换为大写，但是C#中HttpUtility.UrlEncode编码时，则会将这些字符转换为小写。例如 .NET中方法HttpUtility.UrlEncode会将‘=’编码成‘%3d’，而不是%3D。
2、HttpUtility.UrlEncode会把“空格”编码为“+”，实际上应该编码为“%20”，我们需要手动将“+”替换为“%20”。
3、鉴于以上两点，我们需要利用HttpUtility.UrlEncode，重新封装一个C#编码方法，与JS中encodeURIComponent编码对应起来。
    ************************/


    /// <summary>
    /// 小写转大写，特殊字符特换
    /// </summary>
    /// <param name="strSrc">原字符串</param>
    /// <param name="encoding">编码方式</param>
    /// <param name="bToUpper">是否转大写</param>
    /// <returns></returns>
    private string UrlEncode(string strSrc, System.Text.Encoding encoding, bool bToUpper)
    {
        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
        for (int i = 0; i < strSrc.Length; i++)
        {
            string t = strSrc[i].ToString();
            string k = HttpUtility.UrlEncode(t, encoding);
            if (t == k)
            {
                stringBuilder.Append(t);
            }
            else
            {
                if (bToUpper)
                    stringBuilder.Append(k.ToUpper());
                else
                    stringBuilder.Append(k);
            }
        }
        if (bToUpper)
            return stringBuilder.ToString().Replace("+", "%20");
        else
            return stringBuilder.ToString();
    }

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