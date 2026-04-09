using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Student_masterwork : System.Web.UI.Page
{
    LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            if (!IsPostBack)
            {
                ShowGrade();
                ShowTerm();
                ShowCourse();
                ShowMasterWorks();
            }
        }
        else
        {
            LearnSite.Common.CookieHelp.JudgeStudentCookies();
        }
    }

    private void ShowGrade()
    {
        LearnSite.BLL.Room rbll = new LearnSite.BLL.Room();
        DDLgrade.DataSource = rbll.GetAllGrade();
        DDLgrade.DataTextField = "Rgrade";
        DDLgrade.DataValueField = "Rgrade";
        DDLgrade.DataBind();
        if (LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            string Cobj = cook.Sgrade.ToString();
            DDLgrade.SelectedValue = Cobj;
        }
        if (Request.QueryString["cobj"] != null)
        {
            DDLgrade.SelectedValue = Request.QueryString["cobj"].ToString();
        }
    }

    private void ShowTerm()
    {
        DDLterm.SelectedValue = LearnSite.Common.XmlHelp.GetTerm();
        if (Request.QueryString["cterm"] != null)
        {
            DDLterm.SelectedValue = Request.QueryString["cterm"].ToString();
        }
    }

    private void ShowCourse()
    {
        int cobj = Int32.Parse(DDLgrade.SelectedValue);
        int cterm = Int32.Parse(DDLterm.SelectedValue);
        LearnSite.BLL.Courses cbll = new LearnSite.BLL.Courses();
        if (LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            LearnSite.BLL.Room rbll = new LearnSite.BLL.Room();
            string Rhid = rbll.GetRoomRhid(cook.Sgrade, cook.Sclass);
            GVcourse.DataSource = cbll.GetAllGoodListByChid(cobj, cterm,Int32.Parse(Rhid));//获得指定年级、指定学期的所有学案，无内容，标题
            GVcourse.DataBind();
        }
        else
        {
            GVcourse.DataSource = cbll.GetAllGoodList(cobj, cterm);//获得指定年级、指定学期的所有学案，无内容，标题
            GVcourse.DataBind();
        }
    }

    private void ShowMasterWorks()
    {
        if (Request.QueryString["cid"] != null)
        {
            int Cid = Int32.Parse(Request.QueryString["cid"].ToString());
            LearnSite.BLL.Works wbll = new LearnSite.BLL.Works();
            DLworks.DataSource = wbll.ShowCourseGoodWorks(Cid);//显示该学案所有优秀作品            
            DLworks.DataBind();
            string ctitle = "";
            if (Request.QueryString["ctitle"] != null)
            {
                ctitle = Request.QueryString["ctitle"].ToString();
            }
            if (DLworks.Items.Count > 0)
                Labelmsg.Text = "[" + ctitle + "] 作品数：" + DLworks.Items.Count.ToString() + "件";
            else
                Labelmsg.Text = "暂无推荐作品！";
        }
        else
        {
            Labelmsg.Text = "请选择右侧课节！";
        }
    }
    protected void GVcourse_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex > -1)
        {
            HyperLink hl = (HyperLink)e.Row.FindControl("HyperLink1");
            string ctitle = hl.ToolTip;
            hl.Text = LearnSite.Common.WordProcess.SubString(ctitle, 22);
            string acid = ((Label)e.Row.FindControl("LabelCid")).Text;
            hl.NavigateUrl = "masterwork.aspx?cid=" + acid + "&cobj=" + DDLgrade.SelectedValue + "&cterm=" + DDLterm.SelectedValue + "&ctitle=" + Server.UrlEncode(ctitle);
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
    protected void DDLgrade_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowCourse();
    }
    protected void DDLterm_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowCourse();
    }
    protected void DLworks_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemIndex > -1)
        {
            Label lb = (Label)e.Item.FindControl("Labeltype");
            Image img = (Image)e.Item.FindControl("Image1");
            img.ImageUrl = "~/images/filetype/" + lb.Text + ".gif";
        }
    }
    protected void DLworks_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "S")
        {
            string Wurl = e.CommandArgument.ToString();
            if (Wurl != "")
            {
                string Wid = ((Label)e.Item.FindControl("LabelWid")).Text;
                LearnSite.BLL.Works wbll = new LearnSite.BLL.Works();
                LearnSite.Model.Works wmodel = new LearnSite.Model.Works();
                wmodel = wbll.GetModel(Int32.Parse(Wid));
                string ext = LearnSite.Common.WordProcess.getext(Wurl);
                //Literal1.Text = LearnSite.Common.WordProcess.SelectWriteStuNew(ext, Wurl, false);
                Literal1.Text = LearnSite.Common.ViewPage.SelectWritePlugin(Wid, ext, Wurl, wmodel.Wcode, wmodel.Wthumbnail, true, false,wmodel.Wnum);

            }
        }
    }
}