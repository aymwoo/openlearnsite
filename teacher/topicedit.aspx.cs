using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Teacher_topicedit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (Request.QueryString["tcid"] != null)
        {
            if (!IsPostBack)
            {
                Master.Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle() + "学案讨论主题添加页面";
                showtopic();
            }
        }
        else
        {
            Response.Redirect("~/teacher/course.aspx", false);
        }
    }
    protected string myCid()
    {
        if (Request.QueryString["tcid"] != null)
        {
            return Request.QueryString["tcid"].ToString();
        }
        else
        {
            return "";
        }
    }
    private void showtopic()
    {
        if (Request.QueryString["tid"] != null)
        {
            int tid = Int32.Parse(Request.QueryString["tid"]);
            LearnSite.BLL.TopicDiscuss bll = new LearnSite.BLL.TopicDiscuss();
            LearnSite.Model.TopicDiscuss model = new LearnSite.Model.TopicDiscuss();
            model = bll.GetModel(tid);
            Texttitle.Text = model.Ttitle;
            mcontent.Value = HttpUtility.HtmlDecode(model.Tcontent);
            CheckClose.Checked = model.Tclose;
        }
    }
    protected void Btnedit_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["tcid"] != null)
        {
            string fckstr = LearnSite.Common.MarkdownContentGuard.NormalizeCodeFences(mcontent.Value);
            if (Texttitle.Text != "" && fckstr != "")
            {

                LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();
                string hidstr = tcook.Hid.ToString();
                int Tcid = Int32.Parse(Request.QueryString["tcid"].ToString());
                int Tid = Int32.Parse(Request.QueryString["tid"].ToString());
                LearnSite.BLL.TopicDiscuss bll = new LearnSite.BLL.TopicDiscuss();

                string Ttitle = HttpUtility.HtmlEncode(Texttitle.Text.Trim());
                string Tcontent = HttpUtility.HtmlEncode(fckstr);
                bool Tclose = CheckClose.Checked;
                bll.UpdateTopic(Tid, Ttitle, Tcontent, Tclose);

                LearnSite.Model.ListMenu lmodel = new LearnSite.Model.ListMenu();
                LearnSite.BLL.ListMenu lbll = new LearnSite.BLL.ListMenu();

                lmodel.Lcid = Tcid;
                lmodel.Lxid = Tid;
                lmodel.Ltype = 3;
                lmodel.Ltitle = Texttitle.Text.Trim();
                lmodel.Lshow = Tclose;
                lbll.UpdateLtitle(lmodel);

                System.Threading.Thread.Sleep(200);
                string url = "~/teacher/topicshow.aspx?tid=" + Tid;
                Response.Redirect(url, false);
            }
            else
            {
                Labelmsg.Text = "内容及标题不能为空！";
            }
        }
    }
    protected void BtnCourse_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["tid"] != null)
        {
            string Tid = Request.QueryString["tid"].ToString();
            string url = "~/teacher/topicshow.aspx?tid=" + Tid;
            Response.Redirect(url, false);
        }
    }
}
