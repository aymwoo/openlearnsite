using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Student_summaryedit : System.Web.UI.Page
{
    LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
    protected string contentstr = "";
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
    protected string myCid()
    {
        if (Request.QueryString["scid"] != null)
        {
            return Request.QueryString["scid"].ToString();
        }
        else
        {
            return "";
        }
    }
    private void showSummary()
    {
        if (Request.QueryString["scid"] != null)
        {
            int Sgrade = cook.Sgrade;
            int Sclass = cook.Sclass;
            int Scid = Int32.Parse(Request.QueryString["scid"].ToString());
            LearnSite.Model.Summary smodel = new LearnSite.Model.Summary();
            LearnSite.BLL.Summary sbll = new LearnSite.BLL.Summary();
            int hid = cook.Rhid;
            smodel = sbll.GetModelByClass(Scid, hid, Sgrade, Sclass);
            LearnSite.BLL.Courses cbll = new LearnSite.BLL.Courses();
            Label1.Text = cbll.GetTitle(Scid);
            if (smodel != null)
            {
                contentstr= HttpUtility.HtmlDecode(smodel.Scontent);
                Label6.Text = smodel.Sdate.ToString();
                ButtonEdit.Text = "保存修改";
                ButtonEdit.ToolTip = smodel.Sid.ToString();//临时保存要修改的总结ID
            }
            else
            {
                ButtonEdit.Text = "添加总结";
                ButtonEdit.ToolTip = "";
            }
            int Syear = cook.Syear;
            string Snum = cook.Snum;

            string teasnum = "s" + hid + Syear.ToString() + Sgrade.ToString() + Sclass.ToString();
            if (teasnum == Snum)
            {
                ButtonEdit.Enabled = true;
            }
        }
    }
    protected void ButtonEdit_Click(object sender, EventArgs e)
    {
        string fckstr = Request.Form["textareaItem"].Trim();
        if (fckstr.Length > 5)
        {
            LearnSite.Model.Summary smodel = new LearnSite.Model.Summary();
            LearnSite.BLL.Summary sbll = new LearnSite.BLL.Summary();
            string sCid = Request.QueryString["scid"].ToString();
            smodel.Scid = Int32.Parse(sCid);
            smodel.Sclass = cook.Sclass;
            smodel.Scontent = HttpUtility.HtmlEncode(fckstr);
            smodel.Sdate = DateTime.Now;
            smodel.Sgrade = cook.Sgrade;
            smodel.Shid = cook.Rhid;            
            smodel.Sshow = true;
            smodel.Syear = DateTime.Now.Year;
            string msg = "修改课堂活动总结成功！";
            if (ButtonEdit.ToolTip != "")
            {
                smodel.Sid = Int32.Parse(ButtonEdit.ToolTip);
                sbll.Update(smodel);
            }
            else
            {
                sbll.Add(smodel);
                msg = "添加课堂活动总结成功！";
            }            
            LearnSite.Common.WordProcess.Alert(msg, this.Page);
            string surl = "~/student/summary.aspx?cid=" + sCid + "&scid=" + sCid;
            Response.Redirect(surl);
        }
        else
        {
            string msg = "不能输入少于6个汉字！";
            LearnSite.Common.WordProcess.Alert(msg, this.Page);
        }
    }
}