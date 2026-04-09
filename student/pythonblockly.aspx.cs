using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class student_pythonblockly : System.Web.UI.Page
{
    protected string Id = " ";
    protected string Owner = " ";
    protected string Fpage = "#";
    protected string Mcontents = " ";
    protected string Titles = "";
    protected string codedict = " ";
    protected string codefile = " ";
    protected string Snum = " ";
    protected string mback = "";
    protected string argin = "";
    protected string argcode = "";
    protected string argout0 = "";
    protected string argout1 = "";
    protected string argout2 = "";
    protected string argimg = "";
    protected string mhelp = "";
    protected string mpass = "";

    protected string Midurl = "../images/none.gif ";

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

                    if (Request.Cookies[LearnSite.Common.CookieHelp.stuCookieNname] != null)
                    {
                        LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
                        string stuName = cook.Sname;
                        string stuNum = cook.Snum;

                        this.Page.Title = HttpUtility.UrlDecode(stuName) + " Python Blockly 积木编程";
                    }
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

                LearnSite.BLL.Works wbll = new LearnSite.BLL.Works();
                LearnSite.Model.Works wmodel = new LearnSite.Model.Works();
                wmodel = wbll.GetModelByStu(Int32.Parse(Mid), Snum);
                if (wmodel != null)
                {
                    codefile = HttpUtility.UrlEncode(wmodel.Wcode).Replace("+", "%20"); //如果不为空，则获取原来的作品链接
                    codedict = wmodel.Wdict;
                    if (wmodel.Wpass)
                    {
                        mpass = "1";
                    }
                    else
                    {
                        mpass = "0";
                    }
                }
                else
                {
                    mpass = "-1";
                }

                this.Page.Title = HttpUtility.UrlDecode(Sname) + " " + Snum + " " + model.Mtitle;

                LearnSite.BLL.JudgeArg jbll = new LearnSite.BLL.JudgeArg();
                LearnSite.Model.JudgeArg jmodel = new LearnSite.Model.JudgeArg();
                jmodel = jbll.GetModelByMid(Int32.Parse(Mid));
                if (jmodel != null)
                {
                    argin = jmodel.Jinone + "#" + jmodel.Jintwo + "#" + jmodel.Jinthree;
                    argout0 = Base64(HttpUtility.UrlEncode(jmodel.Joutone).Replace("+", "%20"));
                    argout1 = Base64(HttpUtility.UrlEncode(jmodel.Joutwo).Replace("+", "%20"));
                    argout2 = Base64(HttpUtility.UrlEncode(jmodel.Jouthree).Replace("+", "%20"));
                    argcode = jmodel.Jcode;
                    argimg = jmodel.Jimg;

                    if (string.IsNullOrEmpty(jmodel.Jthumb))
                    {
                        //过渡
                        string imgurl = "~/store/" + Cid + "/resimg" + Mid + ".png";
                        string imgdoturl = "../store/" + Cid + "/resimg" + Mid + ".png";
                        string imgpath = Server.MapPath(imgurl);
                        if (System.IO.File.Exists(imgpath))
                        {
                            Midurl = imgdoturl;
                        }
                    }
                    else
                    {
                        if (model.Mhelp)
                            Midurl = jmodel.Jthumb.Replace("~", "..");
                    }
                }
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