using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Student_summary : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            if (!IsPostBack)
            {
                showSummary();
            }
        }
        else
        {
            LearnSite.Common.CookieHelp.JudgeStudentCookies();
        }
    }

    private void showSummary()
    {
        if (Request.QueryString["scid"] != null)
        {
            LearnSite.Model.Cook cook = new LearnSite.Model.Cook();

            int Sgrade = cook.Sgrade;
            int Sclass = cook.Sclass;
            int Scid = Int32.Parse(Request.QueryString["scid"].ToString());
            LearnSite.Model.Summary smodel = new LearnSite.Model.Summary();
            LearnSite.BLL.Summary sbll = new LearnSite.BLL.Summary();
            int hid = cook.Rhid;

            if (hid != 0)
            {
                LearnSite.BLL.Courses cbll = new LearnSite.BLL.Courses();
                Label1.Text = cbll.GetTitle(Scid);
                smodel = sbll.GetModelByClass(Scid, hid, Sgrade, Sclass);
                if (smodel != null)
                {
                    // LearnSite.BLL.Mission mbll = new LearnSite.BLL.Mission();
                    if (smodel.Sshow.Value)
                    {
                        contents.InnerHtml = HttpUtility.HtmlDecode(smodel.Scontent);
                    }
                    else
                    {
                        contents.InnerHtml = "隐藏内容！";
                    }
                    Label6.Text = smodel.Sdate.ToString();
                }
                else
                {
                    contents.InnerHtml = "老师还未填写总结！";
                }
            }
            int Syear = cook.Syear;
            string Snum = cook.Snum;

            string teasnum = "s" + hid + Syear.ToString() + Sgrade.ToString() + Sclass.ToString();
            if (teasnum == Snum)
            {
                BtnEdit.Enabled = true;
                BtnEdit.ToolTip = "修改或添加总结！";
            }
        }
    }
    protected void BtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        string Scid = Request.QueryString["scid"].ToString();
        string smurl = "~/student/summaryedit.aspx?Scid="+Scid;
        Response.Redirect(smurl, true);
    }
}
