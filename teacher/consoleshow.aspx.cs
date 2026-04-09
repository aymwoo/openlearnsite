using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Teacher_consoleshow : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        Master.Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle() + "测评显示页面";

        if (!IsPostBack)
        {
            showconsole();
            showproblem();

            if (Request.QueryString["cold"] != null)
            {
                BtnEdit.Enabled = false;
                Btnadd.Enabled = false;
            }
        }
    }
    private void showconsole()
    {
        if (Request.QueryString["nid"] != null && Request.QueryString["ncid"] != null)
        {
            string nid = Request.QueryString["nid"].ToString();
            string cid = Request.QueryString["ncid"].ToString();
            LearnSite.Model.Consoles nmodel = new LearnSite.Model.Consoles();
            LearnSite.BLL.Consoles nbll = new LearnSite.BLL.Consoles();
            nmodel = nbll.GetModel(Int32.Parse(nid));

            Lbdate.Text = nmodel.Ndate.ToString();
            Lbtitle.Text = nmodel.Ntitle;
            vcontent.InnerHtml = HttpUtility.HtmlDecode(nmodel.Ncontent);
            bool isBegin = nmodel.Nbegin;
            if (!isBegin)
            {
                Btnclock.Text = "已暂停";
                Btnclock.ToolTip = "点击启用测评";
                Btnclock.CssClass = "admin-form-btn admin-form-btn--secondary";
            }
            else
            {
                Btnclock.Text = "已启用";
                Btnclock.ToolTip = "点击暂停测评";
                Btnclock.CssClass = "admin-form-btn admin-form-btn--primary";
            }
        }
    }

    private void showproblem()
    {
        Hkconsole.Visible = false;
        if (Request.QueryString["nid"] != null && Request.QueryString["ncid"] != null)
        {
            string nid = Request.QueryString["nid"].ToString();
            string cid = Request.QueryString["ncid"].ToString();
            LearnSite.BLL.Problems qbll = new LearnSite.BLL.Problems();
            GVProblem.DataSource = qbll.GetListNid(Int32.Parse(nid));
            GVProblem.DataBind();

            if (Request.QueryString["lid"] != null)
            {
                string lid = Request.QueryString["lid"].ToString();
                string url = "~/teacher/consolepreview.aspx?nid=" + nid + "&ncid=" + cid + "&lid=" + lid;
                Hkconsole.NavigateUrl = url;
                Hkconsole.Visible = true;
            }
        }
    }
    protected void Btnadd_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["nid"] != null && Request.QueryString["ncid"] != null)
        {
            string nid = Request.QueryString["nid"].ToString();
            string cid = Request.QueryString["ncid"].ToString();
            string url = "~/teacher/problem.aspx?nid=" + nid + "&ncid=" + cid;
            if (Request.QueryString["lid"] != null)
            {
                url += "&lid=" + Request.QueryString["lid"].ToString();
            }
            Response.Redirect(url, false);
        }
    }
    protected void GVProblem_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int RowIndex = Convert.ToInt32(e.CommandArgument);
        string pid = GVProblem.DataKeys[RowIndex].Value.ToString();
        string nid = Request.QueryString["nid"].ToString();
        LearnSite.BLL.Problems mbll = new LearnSite.BLL.Problems();

        if (e.CommandName == "Del")
        {
            mbll.Delete(Int32.Parse(pid));
        }

        if (e.CommandName == "Top")
        {
            if (RowIndex == 0)
            {
                mbll.Psortnew(Int32.Parse(nid));//如果首行，初始化序号
            }
            if (RowIndex > 0)
            {
                int toplid = Convert.ToInt32(GVProblem.DataKeys[RowIndex - 1].Value.ToString());//获取上个导航编号
                mbll.updatePsort(Int32.Parse(pid), false);//当前导航减１向上
                mbll.updatePsort(toplid, true);//上个导航增１向下
            }
        }
        if (e.CommandName == "Bottom")
        {
            int rowscount = GVProblem.Rows.Count;
            if (RowIndex < rowscount - 1)
            {
                int bottomlid = Convert.ToInt32(GVProblem.DataKeys[RowIndex + 1].Value.ToString());//获取下个导航编号
                mbll.updatePsort(bottomlid, false);//下个导航减１向上
                mbll.updatePsort(Int32.Parse(pid), true);//当前导航增１向下
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
            string strjs = "if(confirm('您确定要删除该测评试题吗？'))return true;else return false; ";
            ((LinkButton)e.Row.FindControl("BtnDel")).OnClientClick = strjs;
            string pid= GVProblem.DataKeys[e.Row.RowIndex].Value.ToString();
            if (Request.QueryString["nid"] != null && Request.QueryString["ncid"] != null)
            {
                string nid = Request.QueryString["nid"].ToString();
                string cid = Request.QueryString["ncid"].ToString();
                string url = "~/teacher/problem.aspx?nid=" + nid + "&ncid=" + cid + "&pid=" + pid;
                if (Request.QueryString["lid"] != null)
                {
                    url += "&lid=" + Request.QueryString["lid"].ToString();
                }
                ((HyperLink)e.Row.FindControl("HyperLinkPid")).NavigateUrl = url;
            }
        }
    }
    protected void Btnreturn_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["nid"] != null && Request.QueryString["ncid"] != null)
        {
            string cid = Request.QueryString["ncid"].ToString();
            string url = "~/teacher/courseshow.aspx?cid=" + cid;
            Response.Redirect(url, false);
        }
    }
    protected void BtnEdit_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["nid"] != null && Request.QueryString["ncid"] != null)
        {
            string nid = Request.QueryString["nid"].ToString();
            string cid = Request.QueryString["ncid"].ToString();
            string url = "~/teacher/consoleadd.aspx?nid=" + nid + "&cid=" + cid;
            if (Request.QueryString["lid"] != null)
            {
                url += "&lid=" + Request.QueryString["lid"].ToString();
            }
            Response.Redirect(url, true);
        }
    }

    protected void Btnclock_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["nid"] != null)
        {
            string nid = Request.QueryString["nid"].ToString();
            LearnSite.BLL.Consoles bll = new LearnSite.BLL.Consoles();
            bll.UpdateNbegin(Int32.Parse(nid));
            System.Threading.Thread.Sleep(200);
            showconsole();
        }
    }
}
