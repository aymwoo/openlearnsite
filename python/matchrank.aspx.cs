using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class python_matchrank : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            showQscore();
        }
    }
    private void showQscore()
    {
        if (Request.QueryString["mid"] != null)
        {
            string mid = Request.QueryString["mid"].ToString();
            LearnSite.BLL.TurtleAnswer bll = new LearnSite.BLL.TurtleAnswer();

            if (LearnSite.Common.WordProcess.IsNum(mid))
            {
                GridViewclass.DataSource = bll.GetListRank(Int32.Parse(mid));
                GridViewclass.DataBind();
                Labeltitle.Text = DateTime.Now.ToLongDateString() + "绘画编程比赛排行榜";
            }
        }

    }
    protected void GridViewclass_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex > -1)
        {
            e.Row.Cells[0].Text = Convert.ToString(e.Row.RowIndex + 1);
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //当鼠标放上去的时候 先保存当前行的背景颜色 并给附一颜色 
            e.Row.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#E1E8E1',this.style.fontWeight='';");
            //当鼠标离开的时候 将背景颜色还原的以前的颜色 
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor,this.style.fontWeight='';");
            //单击行改变行背景颜色 
            e.Row.Attributes.Add("onclick", "this.style.backgroundColor='#D8E0D8'; this.style.color='buttontext';this.style.cursor='default';");
        }
    }
    protected void Btnreturn_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["mid"] != null)
        {
            string id = Request.QueryString["mid"].ToString();
            string url = "~/python/question.aspx?mid=" + id;
            Response.Redirect(url, false);
        }

    }
}