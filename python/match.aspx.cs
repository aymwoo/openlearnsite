using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class python_match : System.Web.UI.Page
{
    LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Listall();
        }
    }

    private void Listall()
    {
        LearnSite.BLL.TurtleMatch bll = new LearnSite.BLL.TurtleMatch();
        GVMatch.DataSource = bll.GetAllList();
        GVMatch.DataBind();
        if (tcook.IsExist())
        {
            Hlmatch.Visible = true;//如果教师登录，则只显示创建比赛
        }
        else
        {
            Hlmatch.Visible = false;   
        }

    }
    protected void GVMatch_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex > -1)
        {
            e.Row.Cells[0].Text = Convert.ToString(GVMatch.PageIndex * GVMatch.PageSize + e.Row.RowIndex + 1);
            string mhid = GVMatch.DataKeys[e.Row.RowIndex].Value.ToString();
            HyperLink hlk = (HyperLink)e.Row.FindControl("HyperLinkEdit");
            if (tcook.IsExist())
            {
                if (mhid == tcook.Hid.ToString())
                {
                    hlk.Visible = true;
                }
                else
                {
                    hlk.Visible = false;                
                }
            }
            else
            {
                hlk.Visible = false;
            }
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
    protected void GVMatch_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView theGrid = sender as GridView;  // refer to the GridView
        int newPageIndex = 0;

        if (-2 == e.NewPageIndex)
        { // when click the "GO" Button
            TextBox txtNewPageIndex = null;

            GridViewRow pagerRow = theGrid.BottomPagerRow;

            if (null != pagerRow)
            {
                txtNewPageIndex = pagerRow.FindControl("txtNewPageIndex") as TextBox;   // refer to the TextBox with the NewPageIndex value
            }

            if (null != txtNewPageIndex)
            {

                newPageIndex = int.Parse(txtNewPageIndex.Text) - 1; // get the NewPageIndex
            }
        }
        else
        {  // when click the first, last, previous and next Button
            newPageIndex = e.NewPageIndex;
        }

        // check to prevent form the NewPageIndex out of the range
        newPageIndex = newPageIndex < 0 ? 0 : newPageIndex;
        newPageIndex = newPageIndex >= theGrid.PageCount ? theGrid.PageCount - 1 : newPageIndex;
        theGrid.PageIndex = newPageIndex;
        if (Request.Cookies[LearnSite.Common.CookieHelp.teaCookieNname] != null)
        {
            string Hid = tcook.Hid.ToString();
            Session[Hid + "pageindex"] = theGrid.PageIndex;
        }
        Listall();
    }
}