using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Profile_mychange : System.Web.UI.Page
{
    LearnSite.Model.Cook cook = new LearnSite.Model.Cook();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            if (!IsPostBack)
            {
                ShowStudent();
            }
        }
        else
        {
            LearnSite.Common.CookieHelp.JudgeStudentCookies();
        }
    }
    private void ShowStudent()
    {
        LearnSite.BLL.Students stu = new LearnSite.BLL.Students();
        DataListstu.DataSource = stu.GetListTeam(cook.Sgrade, cook.Sclass);
        DataListstu.DataBind();
    }
    protected void DataListstu_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("Vote"))
        {
            int Steam = Int32.Parse(e.CommandArgument.ToString()); 
            HyperLink lkname = (HyperLink)e.Item.FindControl("HyperQname");
            LearnSite.BLL.Students sbll = new LearnSite.BLL.Students();
            sbll.UpdateSidSteam(cook.Sid, Steam);
            string msg = "给" + lkname.Text + "同学推荐组长成功！";
            LearnSite.Common.WordProcess.Alert(msg, this.Page);
            ShowStudent();
        }

        if (e.CommandName.Equals("ChangeGroup"))
        {
            if (cook.IsSimiStu())
            {
                int mySid = Convert.ToInt32(e.CommandArgument.ToString());
                LearnSite.BLL.Students bll = new LearnSite.BLL.Students();
                bll.ChangeSleader(mySid);
                System.Threading.Thread.Sleep(200);
                ShowStudent();
            }
        }
    }
    protected void DataListstu_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemIndex > -1)
        {
            string vpath = "~/images/gcard.gif";
            ImageButton mbtn = (ImageButton)e.Item.FindControl("ImageBtnGroup");
            string sleader = ((Label)e.Item.FindControl("LabelSleader")).Text.ToLower();
            //string snum = ((Label)e.Item.FindControl("LabelSnum")).Text.ToLower();
            //string sid = mbtn.CommandArgument.ToString();
            string sname = ((HyperLink)e.Item.FindControl("HyperQname")).Text;
            if (cook.IsSimiStu())
            {
                if (sleader == "true")
                {                    
                    string gjs = "if(confirm('您确定撤销" + sname + "学号的组长任命吗?'))return true;else return false; ";
                    mbtn.OnClientClick = gjs;
                    mbtn.ToolTip = "点击卸任这位组长职位";
                }
                else
                {
                    string sgjs = "if(confirm('您确定任命" + sname + "学号的同学为组长吗?'))return true;else return false; ";
                    mbtn.OnClientClick = sgjs;
                    mbtn.ToolTip = "点击任命这位同学为组长";
                }
            }
            else {
                mbtn.Enabled = false;
            }

            if (sleader == "true")
            {
                vpath = "~/images/gflag.gif";//如果是组长的话,换图标
            }

            mbtn.ImageUrl = vpath + "?temp=" + DateTime.Now.Millisecond.ToString();
        }
    }
}