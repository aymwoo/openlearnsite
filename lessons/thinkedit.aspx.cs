using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Lessons_thinkedit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.Cookies[LearnSite.Common.CookieHelp.teaCookieNname] != null)
            {
                Showthink();
            }
        }
    }
    protected string myCid()
    {
        if (Request.QueryString["cid"] != null)
        {
            return Request.QueryString["cid"].ToString();
        }
        else
        {
            return "";
        }
    }
    protected void BtnEdit_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["cid"] != null)
        {
            string Fcid = Request.QueryString["cid"].ToString();
            string Fcontent = HttpUtility.HtmlEncode(mcontent.InnerText);
            Labelmsg.Text = "添加反思成功";
            LearnSite.Model.Flection flection = new LearnSite.Model.Flection();
            flection.Fcid = Int32.Parse(Fcid);
            flection.Fcontent = Fcontent;
            LearnSite.BLL.Flection flectionbll = new LearnSite.BLL.Flection();
            flectionbll.Update(flection);
            System.Threading.Thread.Sleep(500);
            if (Request.QueryString["modal"] == "1")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "closemodal", "window.parent.notifyLessonModalSuccess(true);", true);
            }
            else
            {
                string url = "~/lessons/thinkshow.aspx?cid=" + Int32.Parse(Fcid);
                Response.Redirect(url, false);
            }
        }
    }

    private void Showthink()
    {
        if (Request.QueryString["cid"] != null)
        {
            string Fcid = Request.QueryString["cid"].ToString();
            LearnSite.Model.Flection flection = new LearnSite.Model.Flection();
            LearnSite.BLL.Flection flectionbll = new LearnSite.BLL.Flection();
            flection = flectionbll.GetModel(Int32.Parse(Fcid));
            LearnSite.BLL.Courses cs = new LearnSite.BLL.Courses();
            Texttitle.Text = cs.GetTitle(Int32.Parse(Fcid));
            mcontent.InnerText = HttpUtility.HtmlDecode(flection.Fcontent);
        }
    }
    protected void BtnCourse_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["cid"] != null)
        {
            string Cid = Request.QueryString["cid"].ToString();
            if (Request.QueryString["modal"] == "1")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "closemodal", "window.parent.closeLessonModal();", true);
            }
            else
            {
                string url = "~/lessons/thinkshow.aspx?cid=" + Cid;
                Response.Redirect(url, false);
            }
        }
        else
        {
            Labelmsg.Text = "没有主学案";
        }
    }
}
