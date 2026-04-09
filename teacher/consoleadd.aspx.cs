using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Teacher_consoleadd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        Master.Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle() + "测评添加修改页面";
        if (Request.QueryString["cid"] != null)
        {
            if (!IsPostBack)
            {
                showconsole();
            }
        }
        else
        {
            Response.Redirect("~/teacher/course.aspx", false);
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
    private void showconsole()
    {
        if (Request.QueryString["nid"] != null)
        {
            Btnadd.Text = "修改测评";
            string nid = Request.QueryString["nid"].ToString();
            LearnSite.Model.Consoles nmodel = new LearnSite.Model.Consoles();
            LearnSite.BLL.Consoles nbll = new LearnSite.BLL.Consoles();
            nmodel = nbll.GetModel(Int32.Parse(nid));
            Texttitle.Text = nmodel.Ntitle;
            Publish.Checked = nmodel.Npublish;
            mcontent.Value = HttpUtility.HtmlDecode(nmodel.Ncontent);
        }
    }

    protected void Btnadd_Click(object sender, EventArgs e)
    {
        string fckstr = LearnSite.Common.MarkdownContentGuard.NormalizeCodeFences(mcontent.Value);
        if (Texttitle.Text != "" && fckstr != "")
        {
            if (Request.QueryString["cid"] != null)
            {
                LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();
                string hidstr = tcook.Hid.ToString();
                int Ncid = Int32.Parse(Request.QueryString["cid"].ToString());
                LearnSite.BLL.Consoles nbll = new LearnSite.BLL.Consoles();
                LearnSite.Model.Consoles nmodel = new LearnSite.Model.Consoles();
                nmodel.Ncid = Ncid;
                nmodel.Npublish = Publish.Checked;
                nmodel.Ntitle = HttpUtility.HtmlEncode(Texttitle.Text.Trim());
                nmodel.Ncontent = HttpUtility.HtmlEncode(fckstr);
                nmodel.Ndate = DateTime.Now;
                nmodel.Nhid = Int32.Parse(hidstr);

                //Vcid,Vhid,Vtitle,Vcontent,Vtype,Vclose,Vpoint,Vdate
                string url = "";

                LearnSite.Model.ListMenu lmodel = new LearnSite.Model.ListMenu();
                LearnSite.BLL.ListMenu lbll = new LearnSite.BLL.ListMenu();
                lmodel.Lcid = Ncid;
                lmodel.Lshow = Publish.Checked;
                lmodel.Lsort = lbll.GetMaxLsort(Ncid) + 1;
                lmodel.Ltitle = Texttitle.Text.Trim();
                lmodel.Ltype = 9;//交互编程测评

                if (Request.QueryString["nid"] != null)
                {
                    int nid = Int32.Parse(Request.QueryString["nid"].ToString());
                    nmodel.Nid = nid;
                    nbll.Update(nmodel);//更新到测评表中
                    lmodel.Lxid = nid;
                    lbll.UpdateLtitle(lmodel);//更新到导航中
                    url = "~/teacher/consoleshow.aspx?ncid=" + Ncid + "&nid=" + nid;
                    if (Request.QueryString["lid"] != null)
                    {
                        url += "&lid=" + Request.QueryString["lid"].ToString();
                    }

                    System.Threading.Thread.Sleep(500);
                }
                else
                {
                    int newnid = nbll.Add(nmodel);//增加到测评表中
                    lmodel.Lxid = newnid;
                    lbll.Add(lmodel);//增加到导航中
                    System.Threading.Thread.Sleep(500);
                    url = "~/teacher/consoleshow.aspx?ncid=" + Ncid + "&nid=" + newnid;
                }
                Response.Redirect(url, false);
            }
        }
        else
        {
            Labelmsg.Text = "内容及标题不能为空！";
        }
    }
    protected void BtnCourse_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["cid"] != null)
        {
            string Cid = Request.QueryString["cid"].ToString();
            string url = "~/teacher/courseshow.aspx?cid=" + Cid;
            if (Request.QueryString["nid"] != null)
            {
                string nid = Request.QueryString["nid"].ToString();
                url = "~/teacher/consoleshow.aspx?ncid=" + Cid + "&nid=" + nid;
                if (Request.QueryString["lid"] != null)
                {
                    url += "&lid=" + Request.QueryString["lid"].ToString();
                }
            }
            Response.Redirect(url, false);
        }
    }
}
