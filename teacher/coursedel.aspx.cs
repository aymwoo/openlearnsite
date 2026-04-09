using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Teacher_coursedel : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();

        if (!IsPostBack)
        {
            Master.Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle() + "学案删除页面";
            if (Request.QueryString["cid"] != null)
            {
                LearnSite.BLL.Courses cbll = new LearnSite.BLL.Courses();
                LabelID.Text = cbll.GetTitle(Int32.Parse(Request.QueryString["cid"].ToString()));
                ButtonDel.Enabled = true;
            }
            else
            {
                ButtonDel.Enabled = false;
            }
        }
    }
    protected void ButtonDel_Click(object sender, EventArgs e)
    {
        if (Request.Cookies[LearnSite.Common.CookieHelp.teaCookieNname] != null)
        {
            int Cid = Int32.Parse(Request.QueryString["cid"].ToString());
            string Grade = Request.QueryString["grade"].ToString();
            LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();
            int Chid = tcook.Hid;
            LearnSite.BLL.Courses coursebll = new LearnSite.BLL.Courses();
            coursebll.DeleteCourse(Cid, Chid);
            System.Threading.Thread.Sleep(500);
            string url = "~/teacher/courseold.aspx?Cgrade="+Grade;
            Response.Redirect(url, false);
        }
    }
    protected void ButtonCancle_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/teacher/course.aspx", false);
    }
}
