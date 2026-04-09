using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Teacher_topicadd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (!IsPostBack)
            Master.Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle() + "学案讨论主题添加页面";
        if (Request.QueryString["mcid"] == null)
        {
            Response.Redirect("~/teacher/course.aspx", false);
        }
    }
    protected string myCid()
    {
        if (Request.QueryString["mcid"] != null)
        {
            return Request.QueryString["mcid"].ToString();
        }
        else
        {
            return "";
        }
    }
    protected void BtnCourse_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["mcid"] != null)
        {
            string Cid = Request.QueryString["mcid"].ToString();
            string url = "~/teacher/courseshow.aspx?cid=" + Cid;
            Response.Redirect(url, false);
        }
    }
    protected void Btnadd_Click(object sender, EventArgs e)
    {
        string fckstr = LearnSite.Common.MarkdownContentGuard.NormalizeCodeFences(Request.Form["textareaItem"].Trim());
        if (Texttitle.Text != "" && fckstr != "")
        {
            if (Request.QueryString["mcid"] != null)
            {
                LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();
                string hidstr = tcook.Hid.ToString();
                //string serverUrl = LearnSite.Common.WordProcess.ServerUrl();
                //fckstr = fckstr.Replace(serverUrl, "");
                int Mcid = Int32.Parse(Request.QueryString["mcid"].ToString());
                LearnSite.BLL.TopicDiscuss bll = new LearnSite.BLL.TopicDiscuss();
                LearnSite.Model.TopicDiscuss model = new LearnSite.Model.TopicDiscuss();
                model.Tcid = Mcid;
                model.Ttitle = HttpUtility.HtmlEncode(Texttitle.Text.Trim());
                model.Tcontent = HttpUtility.HtmlEncode(fckstr);
                model.Tcount = 0;
                model.Tteacher = Int32.Parse(hidstr);
                model.Tdate = DateTime.Now;
                model.Tclose = CheckPublish.Checked;
                model.Tresult = "";
                int tid = bll.Add(model);

                LearnSite.Model.ListMenu lmodel = new LearnSite.Model.ListMenu();
                LearnSite.BLL.ListMenu lbll = new LearnSite.BLL.ListMenu();
                lmodel.Lcid = Mcid;
                lmodel.Lshow = true;
                lmodel.Lsort = lbll.GetMaxLsort(Mcid) + 1;
                lmodel.Ltitle = Texttitle.Text.Trim();
                lmodel.Ltype = 3;
                lmodel.Lxid = tid;
                lbll.Add(lmodel);

                System.Threading.Thread.Sleep(500);
                //Labelmsg.Text = "添加主题讨论成功";
                string url = "~/teacher/courseshow.aspx?cid=" + Mcid.ToString();
                Response.Redirect(url, false);
            }
        }
        else
        {
            Labelmsg.Text = "内容及标题不能为空！";
        }
    }
}
