using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class student_webspace : System.Web.UI.Page
{
    LearnSite.Model.Cook cook = new LearnSite.Model.Cook();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            if (!IsPostBack)
            {

                ListSelectShare();
            }
        }
        else
        {
            Response.Write("请重新登录……！");
        }
    }

    protected void Dlfilelist_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "D")
        {
            LearnSite.BLL.Students sbll = new LearnSite.BLL.Students();
            string furl = e.CommandArgument.ToString();
            LearnSite.Common.ShareDisk.DelFile(furl);//删除
            LearnSite.BLL.ShareDisk kbll = new LearnSite.BLL.ShareDisk();
            kbll.Deletefile(cook.Snum, furl);//删除数据库记录
            System.Threading.Thread.Sleep(300);
            ListSelectShare();
        }
    }
    protected void Dlfilelist_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        string fname = ((HyperLink)e.Item.FindControl("HLfname")).Text;
        string Sname = Server.UrlDecode(cook.Sname);
        ImageButton imgbtn = (ImageButton)e.Item.FindControl("ImgBtnDelete");
        Sname = LearnSite.Common.WordProcess.FilterSpecial(Sname).Trim();
        string fnum = ((Label)e.Item.FindControl("Labelnum")).Text;

        if (String.IsNullOrEmpty(fnum))
        {
            imgbtn.Enabled = false;//如果是网页则不可删除
            imgbtn.ImageUrl = "~/images/deleteno.gif";
            imgbtn.ToolTip = "网页";
        }
        else
        {
            string strjs = "if(confirm('确定删除" + fname + "文件吗？'))return true;else return false; ";
            imgbtn.OnClientClick = strjs;
        }
    }

    private void ListSelectShare()
    {
        ListShareFiles(cook.Snum);//列出网盘文件
        Labeltitle.Text = Server.UrlDecode(cook.Sname) + "网页空间";
    }

    /// <summary>
    /// 列表网盘文件
    /// </summary>
    /// <param name="Syear">入学年份</param>
    /// <param name="Sclass">班级</param>
    /// <param name="sharedir">学号或组号</param>
    private void ListShareFiles(string Snum)
    {
        Dlfilelist.Visible = true;
        LearnSite.Common.ShareDisk.WebInfo dk = new LearnSite.Common.ShareDisk.WebInfo(Snum);
        Dlfilelist.DataSource = dk.Dw;
        Dlfilelist.DataBind();
    }

    /// <summary>
    /// 列表网盘文件
    /// </summary>
    /// <param name="Syear">入学年份</param>
    /// <param name="Sclass">班级</param>
    /// <param name="sharedir">学号或组号</param>
    private void ListCommonFiles(string Syear)
    {
        Dlfilelist.Visible = true;
        DateTime dt1 = DateTime.Now;
        LearnSite.Common.ShareDisk.DiskInfoCommon dk = new LearnSite.Common.ShareDisk.DiskInfoCommon(Syear);
        Dlfilelist.DataSource = dk.Dw;
        Dlfilelist.DataBind();

        //Labeldisk.Text =  dk.Dsize.ToString("F1") +  "  " + dk.Dcount.ToString() + "个文件 " ;

    }


}