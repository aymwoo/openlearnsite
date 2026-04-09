using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class python_matchshow : System.Web.UI.Page
{
    LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            showmatch();
            showproblem();
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
            Checkcpublish.Checked = model.Mpublish;
        }

    }
    
    private void showproblem()
    {
        if (Request.QueryString["mid"] != null )
        {
            string mid = Request.QueryString["mid"].ToString();
            LearnSite.BLL.TurtleQuestion bll = new LearnSite.BLL.TurtleQuestion();
            GVProblem.DataSource = bll.GetListQuestion(mid);
            GVProblem.DataBind();
        }
    }
    protected void Btnadd_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["mid"] != null)
        {
            string mid = Request.QueryString["mid"].ToString();
            string url = "~/python/questionedit.aspx?mid=" + mid;
            Response.Redirect(url, false);
        }
    }

    protected void GVProblem_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int RowIndex = Convert.ToInt32(e.CommandArgument);
        string qid = GVProblem.DataKeys[RowIndex].Value.ToString();
        string id = Request.QueryString["mid"].ToString();
        LearnSite.BLL.TurtleQuestion mbll = new LearnSite.BLL.TurtleQuestion();

        if (e.CommandName == "Del")
        {
            mbll.Delete(Int32.Parse(qid));
        }

        if (e.CommandName == "Top")
        {
            if (RowIndex == 0)
            {
                mbll.Qsortnew(Int32.Parse(qid));//如果首行，初始化序号
            }
            if (RowIndex > 0)
            {
                int toplid = Convert.ToInt32(GVProblem.DataKeys[RowIndex - 1].Value.ToString());//获取上个导航编号
                mbll.updateQsort(Int32.Parse(qid), false);//当前导航减１向上
                mbll.updateQsort(toplid, true);//上个导航增１向下
            }
        }
        if (e.CommandName == "Bottom")
        {
            int rowscount = GVProblem.Rows.Count;
            if (RowIndex < rowscount - 1)
            {
                int bottomlid = Convert.ToInt32(GVProblem.DataKeys[RowIndex + 1].Value.ToString());//获取下个导航编号
                mbll.updateQsort(bottomlid, false);//下个导航减１向上
                mbll.updateQsort(Int32.Parse(qid), true);//当前导航增１向下
            }
        }
        System.Threading.Thread.Sleep(200);
        showproblem();
    }
    protected void GVProblem_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex > -1)
        {
            e.Row.Cells[0].Text = Convert.ToString(e.Row.RowIndex + 1);
            string strjs = "if(confirm('您确定要删除该试题吗？'))return true;else return false; ";
            ((LinkButton)e.Row.FindControl("BtnDel")).OnClientClick = strjs;
            string id = GVProblem.DataKeys[e.Row.RowIndex].Value.ToString();

            string mid = Request.QueryString["mid"].ToString();
            string url = "~/python/questionedit.aspx?mid=" + mid + "&id=" + id;
            ((HyperLink)e.Row.FindControl("HyperLinkPid")).NavigateUrl = url;

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


}