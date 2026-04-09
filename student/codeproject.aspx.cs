using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Student_codeproject : System.Web.UI.Page
{
    protected string sbfile = "../scratch/project.sb3";
    protected string sbtitle = "未命名";
    protected string isplayer = "true";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ShowMission();
        }
    }
    private void ShowMission()
    {
        if (Request.QueryString["id"] != null)
        {
            int wid = Int32.Parse(Request.QueryString["id"].ToString());
            LearnSite.BLL.Works wbll = new LearnSite.BLL.Works();
            LearnSite.Model.Works model = new LearnSite.Model.Works();
            string ipwid = "ip" + LearnSite.Common.Computer.MyIp().Replace('.', 'a') + "_" + wid.ToString();
            if (Session[ipwid] == null)
            {
                wbll.UpdateWhit(wid);
                Session[ipwid] = wid;
            }

            model = wbll.GetModel(wid);
            sbtitle = model.Wtitle;
            sbfile = model.Wurl.Replace("~", "..") + "?mill=" + DateTime.Now.Millisecond.ToString();//如果不为空，则获取原来的作品链接
            this.Page.Title = sbtitle + "  " + model.Wname;
            int Wcid = model.Wcid.Value;
            int Wgrade = model.Wgrade.Value;

            if (Request.Cookies[LearnSite.Common.CookieHelp.stuCookieNname] != null)
            {
                LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
        
                LearnSite.BLL.Room rbll = new LearnSite.BLL.Room();
                string result = rbll.GetRcid(cook.Sgrade, cook.Sclass);
                if (!string.IsNullOrEmpty(result))
                {
                    int cid = Int32.Parse(result);
                    if (cid == Wcid && cook.Sgrade == Wgrade)
                    {
                        //如果是正在上的课节内容，则不显示作品的脚本
                        isplayer = "true";
                    }
                    if (Request.Cookies[LearnSite.Common.CookieHelp.teaCookieNname] != null)
                    {
                        isplayer = "false";
                    }
                }
            }
        }
    }
}