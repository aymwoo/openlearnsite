using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Lessons_thinkadd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.Cookies[LearnSite.Common.CookieHelp.teaCookieNname] != null)
            {
                if (Request.QueryString["cid"] != null)
                {
                    string Cid = Request.QueryString["cid"].ToString();
                    LearnSite.BLL.Courses cs = new LearnSite.BLL.Courses();
                    Texttitle.Text = cs.GetTitle(Int32.Parse(Cid));
                }
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
    protected void Btnadd_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["cid"] != null)
        {
            string Fcid = Request.QueryString["cid"].ToString();
            string Fcontent =HttpUtility.HtmlEncode( Request.Form["textareaItem"].Trim());
            Labelmsg.Text = "添加反思成功";
            LearnSite.Model.Flection flection = new LearnSite.Model.Flection();
            flection.Fcontent = Fcontent;
            flection.Fdate = DateTime.Now;
            LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();
            flection.Fhid = tcook.Hid;
            flection.Fcid = Int32.Parse(Fcid);
            LearnSite.BLL.Flection flectionbll = new LearnSite.BLL.Flection();
            flectionbll.Add(flection);
            System.Threading.Thread.Sleep(500);
            if (Request.QueryString["modal"] == "1")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "closemodal", "window.parent.notifyLessonModalSuccess(true);", true);
            }
            else
            {
                string url = "~/lessons/thinkshow.aspx?cid=" +Int32.Parse( Fcid);
                Response.Redirect(url, false);
            }
        }
    }
    protected void BtnCourse_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["cid"] != null)
        {
            string Cid = Request.QueryString["cid"].ToString();
            string url = "~/teacher/course.aspx" ;
            Response.Redirect(url, false);
        }
        else
        {
            Labelmsg.Text = "没有主学案";
        }
    }
}
