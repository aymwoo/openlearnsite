using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Teacher_txtformedit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (Request.QueryString["mcid"] != null && Request.QueryString["mid"] != null)
        {
            if (!IsPostBack)
            {
                Master.Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle() + "学案表单编辑页面";
                txtformview();
            }
        }
        else
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
        if (Request.QueryString["mcid"] != null && Request.QueryString["mid"] != null)
        {
            string Mcid = Request.QueryString["mcid"].ToString();
            string Mid = Request.QueryString["mid"].ToString();
            string url = "~/teacher/txtformshow.aspx?mcid=" + Mcid + "&mid=" + Mid;
            Response.Redirect(url, false);
        }
    }
    protected void Btnedit_Click(object sender, EventArgs e)
    {
        string fckstr = LearnSite.Common.MarkdownContentGuard.NormalizeCodeFences(mcontent.Value);
        string mtitle = HttpUtility.HtmlEncode(Texttitle.Text.Trim());
        if (mtitle != "" && fckstr != "")
        {
            if (Request.QueryString["mcid"] != null && Request.QueryString["mid"] != null)
            {
                Labelmsg.Text = "无";
                string Mcid = Request.QueryString["mcid"].ToString();
                string Mid = Request.QueryString["mid"].ToString();
                LearnSite.Model.TxtForm tmode = new LearnSite.Model.TxtForm();
                tmode.Mid = Int32.Parse(Mid);
                tmode.Mcid = Int32.Parse(Mcid);
                tmode.Mtitle = mtitle;
                tmode.Mpublish = CheckPublish.Checked;
                tmode.Mcontent = HttpUtility.HtmlEncode(fckstr);
                tmode.Mdate = DateTime.Now;
                tmode.Mhit = 0;
                tmode.Mdelete = false;
                tmode.Mcollabo = CheckCollabo.Checked;
                LearnSite.BLL.TxtForm tfmbll = new LearnSite.BLL.TxtForm();
                tfmbll.Update(tmode);

                //string msg = tmode.Mtitle + "<br>\r\n" + tmode.Mcontent + "<br>\r\n" + tmode.Mdate.ToString() + "<br>\r\n" + tmode.Mpublish.ToString() + "<br>\r\n" + Mid;
                // Labelmsg.Text = msg;

                LearnSite.Model.ListMenu lmodel = new LearnSite.Model.ListMenu();
                LearnSite.BLL.ListMenu lbll = new LearnSite.BLL.ListMenu();

                lmodel.Lcid = Int32.Parse(Mcid);
                lmodel.Lxid = Int32.Parse(Mid);
                lmodel.Ltype = 4;
                lmodel.Lshow = CheckPublish.Checked;
                lmodel.Ltitle = mtitle;
                lbll.UpdateMenuThree(lmodel);

                System.Threading.Thread.Sleep(500);
                string url = "~/teacher/txtformshow.aspx?mcid=" + Mcid + "&mid=" + Mid;
                Response.Redirect(url, false);
            }
            else
            {
                Labelmsg.Text = "取不到表单编号Mid！";
            }
        }
        else
        {
            Labelmsg.Text = "内容及标题不能为空！";
        }
    }
    private void txtformview()
    {
        if (Request.QueryString["mid"] != null)
        {
            int Mid = Int32.Parse(Request.QueryString["mid"].ToString());
            LearnSite.Model.TxtForm tmodel = new LearnSite.Model.TxtForm();
            LearnSite.BLL.TxtForm tbll = new LearnSite.BLL.TxtForm();
            tmodel = tbll.GetModel(Mid);

            mcontent.Value = HttpUtility.HtmlDecode(tmodel.Mcontent);
            CheckPublish.Checked = tmodel.Mpublish;
            Texttitle.Text = tmodel.Mtitle;
            CheckCollabo.Checked = tmodel.Mcollabo;
        }
    }
}
