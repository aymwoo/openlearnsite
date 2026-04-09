using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Student_console : System.Web.UI.Page
{
    protected bool isBegin = false;
    protected bool isPass = true;
    LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            if (!IsPostBack)
            {
                showConsole();
            }
        }
        else
        {
            LearnSite.Common.CookieHelp.JudgeStudentCookies();
        }
    }
    
    private void showConsole()
    {
        if (Request.QueryString["lid"] != null)
        {
            string lid = Request.QueryString["lid"].ToString();
            LabelLid.Text = lid;

            LearnSite.BLL.ListMenu lbll = new LearnSite.BLL.ListMenu();
            LearnSite.Model.ListMenu lmodel = new LearnSite.Model.ListMenu();
            lmodel = lbll.GetModel(Int32.Parse(lid));
            int cid = lmodel.Lcid.Value;
            int nid = lmodel.Lxid.Value;
            LabelCid.Text = cid.ToString();
            LabelNid.Text = nid.ToString();

            LearnSite.Model.Consoles model = new LearnSite.Model.Consoles();
            LearnSite.BLL.Consoles bll = new LearnSite.BLL.Consoles();
            model = bll.GetModel(nid);
            if (model != null)
            {
                LabelMtitle.Text = model.Ntitle;
                Mcontent.InnerHtml = HttpUtility.HtmlDecode(model.Ncontent);

                int sid = cook.Sid;
                string fnum = cook.Snum;
                LearnSite.BLL.Problems pbll = new LearnSite.BLL.Problems();
                GVSolve.DataSource = pbll.GetListNidSid(nid, sid);
                GVSolve.DataBind();           

                string url = "~/student/pysolve.aspx?nid=" + nid;
                Hlsolve.NavigateUrl = url;

                string sgrade = cook.Sgrade.ToString();
                string sclass = cook.Sclass.ToString();
                string sterm = cook.ThisTerm.ToString();
                string syear = cook.Syear.ToString();

                isBegin = model.Nbegin;
                if (!isBegin)
                {
                    Btnclock.ImageUrl = "~/images/clockred.gif" + "?temp=" + DateTime.Now.Millisecond.ToString();
                    BtnIdle.Enabled = false;
                }
                else
                {
                    Btnclock.ImageUrl = "~/images/clock.gif" + "?temp=" + DateTime.Now.Millisecond.ToString();
                    BtnIdle.Enabled = true;
                }
                if (fnum.IndexOf('s') > -1)
                {
                    Btnclock.Visible = true;
                }
                else
                {
                    Btnclock.Visible = false;
                }

                LearnSite.BLL.Solves vbll = new LearnSite.BLL.Solves();
                vbll.UpdateSidle(sid.ToString(), syear, sgrade, sclass, sterm);

            }
            else
            {
                Mcontent.InnerHtml = "此学案活动不存在！";
            }
        }
    }


    protected void BtnIdle_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["lid"] != null)
        {
            string lid = Request.QueryString["lid"].ToString();
            string url = "~/student/pythonIdle.aspx?lid=" + lid;
            Response.Redirect(url);
        }
    }
    protected void GVSolve_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex > -1)
        {
            e.Row.Cells[0].Text = "第" + Convert.ToString(e.Row.RowIndex + 1) + "题";
            string score = ((Label)(e.Row.FindControl("Labelscore"))).Text;
            Label lb = ((Label)(e.Row.FindControl("Labelflag")));
            if (string.IsNullOrEmpty(score))
            {
                lb.Text = "✖"; 
                lb.ForeColor = System.Drawing.Color.OrangeRed;
                Imagepass.ImageUrl = "~/images/none.gif";     
            }
            else
            {
                lb.Text = "✔";
                lb.ForeColor = System.Drawing.Color.LightSeaGreen;
            }
        }
    }
    protected void Btnclock_Click(object sender, ImageClickEventArgs e)
    {
        if (Request.QueryString["lid"] != null )
        {
            string nid = LabelNid.Text;
            LearnSite.BLL.Consoles bll = new LearnSite.BLL.Consoles();
            isBegin = !isBegin;
            bll.UpdateNbegin(Int32.Parse(nid));
            System.Threading.Thread.Sleep(500);
            showConsole();
        }
    }
}