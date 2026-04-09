using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Lessons_thinkshow : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.Cookies[LearnSite.Common.CookieHelp.teaCookieNname] != null)
            {
                ShowThink();
            }
        }
    }

    private void ShowThink()
    {
        if (Request.QueryString["cid"] != null)
        {
            int Fcid = Int32.Parse(Request.QueryString["cid"].ToString());
            LearnSite.BLL.Flection flection = new LearnSite.BLL.Flection();
            if (flection.ExistsFcid(Fcid))
            {
                Repeater1.DataSource = flection.GetListCid(Fcid);
                Repeater1.DataBind();
                LearnSite.BLL.Courses cs = new LearnSite.BLL.Courses();
                LabelTitle.Text = cs.GetTitle(Fcid);
            }
            else
            {
                string url = "~/lessons/thinkadd.aspx?cid=" + Fcid + "&modal=1";
                Response.Redirect(url, false);
            }
        }
    }

    protected void BtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        string Cid = Request.QueryString["cid"].ToString();
        string url = "~/lessons/thinkedit.aspx?cid=" + Cid + "&modal=1";
        Response.Redirect(url, true);
    }
}
