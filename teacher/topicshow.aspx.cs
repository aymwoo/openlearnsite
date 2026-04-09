using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Teacher_topicshow : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (!IsPostBack)
        {
            Master.Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle() + "主题讨论浏览页面";
            ShowTopic();
            if (Request.QueryString["cold"] != null)
            {
                BtnEdit.Enabled = false;
            }
        }
    }
    protected void Btnreturn_Click(object sender, EventArgs e)
    {
        string url = "~/teacher/courseshow.aspx?cid=" + LabelMcid.Text;
        if (Request.QueryString["cold"] != null)
        {
            url = url + "&cold=T";
        }
        Response.Redirect(url, false);
    }
    private void ShowTopic()
    {
        if (Request.QueryString["tid"] != null)
        {
            int Tid = Int32.Parse(Request.QueryString["tid"].ToString());
            LearnSite.BLL.TopicDiscuss bll = new LearnSite.BLL.TopicDiscuss();
            LearnSite.Model.TopicDiscuss model = new LearnSite.Model.TopicDiscuss();
            model = bll.GetModel(Tid);
            Labeltid.Text = model.Tid.ToString();
            LabelTtitle.Text = model.Ttitle;
            LabelMcid.Text = model.Tcid.ToString();
            LabelTdate.Text = model.Tdate.ToString();
            bool isClose = model.Tclose;
            if (isClose)
            {
                Btnclock.Text = "已暂停";
                Btnclock.ToolTip = "点击开启讨论";
                Btnclock.CssClass = "admin-form-btn admin-form-btn--secondary";
            }
            else
            {
                Btnclock.Text = "已开启";
                Btnclock.ToolTip = "点击暂停讨论";
                Btnclock.CssClass = "admin-form-btn admin-form-btn--primary";
            }
            Tcontent.InnerHtml = HttpUtility.HtmlDecode(model.Tcontent);
        }
    }
    protected void BtnEdit_Click(object sender, EventArgs e)
    {
        string url = "~/teacher/topicedit.aspx?tcid=" + LabelMcid.Text + "&tid=" + Labeltid.Text;
        Response.Redirect(url, false);
    }
    protected void Btnclock_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["tid"] != null)
        {
            int tid = Int32.Parse(Request.QueryString["tid"].ToString());
            LearnSite.BLL.TopicDiscuss tdbll = new LearnSite.BLL.TopicDiscuss();
            tdbll.UpdateTclose(tid);//更新
            System.Threading.Thread.Sleep(500);
            ShowTopic();
        }
    }
}
