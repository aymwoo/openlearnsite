using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class python_question : System.Web.UI.Page
{
    LearnSite.BLL.TurtleAnswer bll = new LearnSite.BLL.TurtleAnswer();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            showmatch();
            showproblem();
            showface();
        }
    }

    private void showmatch()
    {
        if (Request.QueryString["mid"] != null)
        {
            string mid = Request.QueryString["mid"].ToString();
            LearnSite.Model.TurtleMatch model = new LearnSite.Model.TurtleMatch();
            LearnSite.BLL.TurtleMatch bll = new LearnSite.BLL.TurtleMatch();
            model = bll.GetModel(Int32.Parse(mid));
            Labeltitle.Text = model.Mtitle;
            Labeldate.Text = model.Mdate.ToString();
        }

    }

    private void showproblem()
    {
        if (Request.QueryString["mid"] != null)
        {
            string mid = Request.QueryString["mid"].ToString();
            LearnSite.BLL.TurtleQuestion bll = new LearnSite.BLL.TurtleQuestion();
            GVProblem.DataSource = bll.GetListQuestion(mid);
            GVProblem.DataBind();
        }
    }

    protected void GVProblem_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex > -1)
        {
            e.Row.Cells[0].Text = Convert.ToString(e.Row.RowIndex + 1);
            string id = GVProblem.DataKeys[e.Row.RowIndex].Value.ToString();
             HyperLink hlk= (HyperLink)e.Row.FindControl("HyperLinkPid");
            if (bll.Isdone(Int32.Parse(id)))
            {
                ((Image)e.Row.FindControl("Imagedone")).ImageUrl = "~/images/sucess.png";
                hlk.Text = "继续";
            } 
            string mid = Request.QueryString["mid"].ToString();
            string url = "~/python/code.aspx?mid=" + mid + "&id=" + id;
            hlk.NavigateUrl = url;

        }
    }
    protected void Btnreturn_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["mid"] != null)
        {
            string id = Request.QueryString["mid"].ToString();
            string url = "~/python/match.aspx?mid=" + id;
            Response.Redirect(url, false);
        }
    }


    protected void Buttonrank_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["mid"] != null)
        {
            string id = Request.QueryString["mid"].ToString();
            string url = "~/python/matchrank.aspx?mid=" + id;
            Response.Redirect(url, false);
        }
    }

    private void showface()
    {
        LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();
        LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
        if (tcook.IsExist())
        {
            Imageface.ImageUrl = "~/images/happy.gif";
            Imageface.ToolTip = Server.UrlDecode(tcook.Hnick);
        }
        else
        {
            if (cook.IsExist())
            {
                string murl = LearnSite.Common.Photo.GetStudentPhotoUrl(cook.Snum, cook.Sex);
                Imageface.ImageUrl = murl + "?temp=" + DateTime.Now.Millisecond.ToString();
                Imageface.ToolTip = Server.UrlDecode(cook.Sname);
            }
            else
            {
                Imageface.ImageUrl = "~/images/nothing.gif";
                Imageface.ToolTip = "游客";
            }
        }
    }
}