using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Student_description : System.Web.UI.Page
{
    protected int mystar = 0;
    LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
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
            string Wcid = lmodel.Lcid.ToString();
            string Wmid = lmodel.Lxid.ToString();

            LearnSite.Model.Mission model = new LearnSite.Model.Mission();
            LearnSite.BLL.Mission mn = new LearnSite.BLL.Mission();

            model = mn.GetModel(lmodel.Lxid.Value);
            if (model != null)
            {
                LabelMtitle.Text = model.Mtitle;
                Mcontent.InnerHtml = HttpUtility.HtmlDecode(model.Mcontent);

                int sid = cook.Sid;
                LearnSite.BLL.MenuWorks kbll = new LearnSite.BLL.MenuWorks();
                LearnSite.Model.MenuWorks kmodel = new LearnSite.Model.MenuWorks();
                kmodel = kbll.GetModelme(sid, Int32.Parse(Lid));
                if (kmodel!=null)
                {
                    Btnread.Visible = false;
                    mystar = kmodel.Kstar;
                }
                else
                    Btnread.Visible = true;
            }
            else
            {
                Mcontent.InnerHtml = "此学案活动不存在！";
            }
        }

    }

    protected void Btnread_Click(object sender, EventArgs e)
    {
        string Lid = Request.QueryString["lid"].ToString();
        string star = Request.Form["TextBoxStar"];
        if (LearnSite.Common.WordProcess.IsNum(Lid))
        {
            string LoginTime = cook.LoginTime;
            int sid = cook.Sid;
            DateTime Wdate = DateTime.Now;
            //添加课堂活动记录
            LearnSite.Model.MenuWorks kmodel = new LearnSite.Model.MenuWorks();
            LearnSite.BLL.MenuWorks kbll = new LearnSite.BLL.MenuWorks();

            kmodel.Klid = Int32.Parse(Lid);
            kmodel.Ksid = sid;
            kmodel.Ktime = LearnSite.Common.Computer.GoneMinute(DateTime.Parse(LoginTime), Wdate);
            kmodel.Kseconds = Int32.Parse(LearnSite.Common.Computer.Datagone(DateTime.Parse(LoginTime), Wdate));
            kmodel.Kcheck = false;
            kmodel.Kstar = Int32.Parse(star);
            kbll.Add(kmodel);
            Btnread.Visible = false;
            string url = "~/student/description.aspx?lid=" + Lid;
            Response.Redirect(url);
        }

    }
}
