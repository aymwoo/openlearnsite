using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Student_coding : System.Web.UI.Page
{

    protected string sbfile = "../scratch/project.sb3" + "?mill=" + DateTime.Now.Millisecond.ToString();//如果不为空，则获取原来的作品链接
    protected string sbtitle = "未命名";
    protected string Filename = "";
    protected string Id = "";
    protected string Microworld = "false";
    protected string Owner = "";
    protected string Fpage = "#";
    protected string Mcontents = "";
    protected string Titles = "";
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
                string Snum = cook.Snum;

                Microworld = model.Microworld.ToString().ToLower();
                Mcontents = HttpUtility.HtmlDecode(model.Mcontent);
                Owner = HttpUtility.UrlDecode(Sname);
                Titles = model.Mtitle;
                string examurl = model.Mexample;
                LearnSite.BLL.Works wbll = new LearnSite.BLL.Works();
                LearnSite.Model.Works wmodel = new LearnSite.Model.Works();
                wmodel = wbll.GetModelByStu(Int32.Parse(Mid), Snum);
                int statnum = 0;
                string wtype = model.Mfiletype;
                if (wmodel != null)
                {
                    sbfile = wmodel.Wurl.Replace("~", "..") + "?mill=" + DateTime.Now.Millisecond.ToString();//如果不为空，则获取原来的作品链接
                    sbtitle = wmodel.Wtitle;
                    statnum = 1;//有作品保存过，直接获取
                }
                else
                {
                    if (model.Microworld)
                    {
                        LearnSite.BLL.Courses cbll = new LearnSite.BLL.Courses();
                        int mycid = Int32.Parse(Cid);
                        string wurl = cbll.getPrework(mycid, Snum, wtype); 
                        if (!string.IsNullOrEmpty(wurl))
                        {
                            sbfile = wurl.Replace("~", "..") + "?mill=" + DateTime.Now.Millisecond.ToString();//如果不为空，则获取任务半成品链接  
                            statnum = 2;//继承作品，上一节有作品，直接获取
                        }
                        else
                            statnum = 3;//继承作品，上一节没有作品，则使用默认文件
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(examurl))
                        {
                            sbfile = examurl.Replace("~", "..") + "?mill=" + DateTime.Now.Millisecond.ToString();//如果不为空，则获取任务半成品链接   
                            statnum = 4;//从零开始制作，如果有实例作品，则直接使用实例
                        }
                        else
                            statnum = 5;//从零开始制作，如果没有实例作品，则直接使用默认文件
                    }
                }


                this.Page.Title = HttpUtility.UrlDecode(Sname) + " " + Snum + " " + model.Mtitle + " " + statnum.ToString();
            }
        }

    }
}