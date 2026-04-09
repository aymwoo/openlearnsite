using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Teacher_courseedit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (!IsPostBack)
        {
            Master.Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle() + "学案编辑页面";
            if (Request.QueryString["cid"] != null)
            {
                ShowTypename();
                Grade();
                Showcourse();
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
    protected void Btnedit_Click(object sender, EventArgs e)
    {
        string fckstr = LearnSite.Common.MarkdownContentGuard.NormalizeCodeFences(mcontent.Value);
            if (fckstr != "")
            {
                if (Request.Cookies[LearnSite.Common.CookieHelp.teaCookieNname] != null)
                {
                    string Mcidstr = Request.QueryString["cid"].ToString();                
                    int myCid = Int32.Parse(Mcidstr);
                    LearnSite.Model.Courses course = new LearnSite.Model.Courses();
                    course.Cid = myCid;
                    course.Ctitle = HttpUtility.HtmlEncode(Texttitle.Text.Trim());
                    course.Cclass = DDLclass.SelectedValue;
                    course.Ccontent = HttpUtility.HtmlEncode(fckstr);
                    course.Cdate = DateTime.Now;
                    course.Cks = Int32.Parse(DDLCks.SelectedValue);
                    course.Cobj = Int32.Parse(DDLcobj.SelectedValue);
                    course.Cterm = Int32.Parse(DDLCterm.SelectedValue);
                    LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();
                    course.Chid = tcook.Hid;
                    course.Cpublish = CheckPublish.Checked;

                    string hiddenUrl = HiddenBannerUrl.Value;
                    course.Cbanner = !string.IsNullOrEmpty(hiddenUrl) ? hiddenUrl : HLbanner.NavigateUrl;

                    LearnSite.BLL.Courses coursebll = new LearnSite.BLL.Courses();
                    coursebll.UpdateCourse(course);
                    System.Threading.Thread.Sleep(500);
                    string msg = "更新学案内容成功！";
                    LearnSite.Common.MessageBox.Alert(msg, this.Page);
                    string url = "~/teacher/courseshow.aspx?cid=" + myCid.ToString();
                    Response.Redirect(url, false);
                }
           }
        
    }
    private void Showcourse()
    {
        int myCid = Int32.Parse(Request.QueryString["cid"].ToString());
        LearnSite.Model.Courses course = new LearnSite.Model.Courses();
        LearnSite.BLL.Courses cbll = new LearnSite.BLL.Courses();
        course = cbll.GetModel(myCid);
        Texttitle.Text = course.Ctitle;
        DDLclass.SelectedValue = course.Cclass;
        DDLcobj.SelectedValue = course.Cobj.ToString();
        DDLCterm.SelectedValue = course.Cterm.ToString();
        DDLCks.SelectedValue = course.Cks.ToString();
        mcontent.Value = HttpUtility.HtmlDecode(course.Ccontent);
        CheckPublish.Checked = course.Cpublish;
        LiteralBannerPreviewTitle.Text = course.Ctitle;
        HLbanner.NavigateUrl = course.Cbanner;
        HiddenBannerUrl.Value = course.Cbanner;
        HLbanner.Visible = !string.IsNullOrEmpty(course.Cbanner);
    }
    private void Grade()
    {
        LearnSite.BLL.Room room = new LearnSite.BLL.Room();
        DDLcobj.DataSource = room.GetAllGrade();
        DDLcobj.DataTextField = "Rgrade";
        DDLcobj.DataValueField = "Rgrade";
        DDLcobj.DataBind();
    }
    private void ShowTypename()
    {
        DDLclass.DataSource = LearnSite.Common.TypeNameList.CourseType();
        DDLclass.DataBind();
        DDLCks.DataSource = LearnSite.Common.TypeNameList.CoursePeriod();
        DDLCks.DataBind();
    }

    protected void Btnreturn_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["cid"] != null)
        {
            string Cid = Request.QueryString["cid"].ToString();
            string url = "~/teacher/courseshow.aspx?cid=" + Cid;
            Response.Redirect(url, false);
        }
    }
}
