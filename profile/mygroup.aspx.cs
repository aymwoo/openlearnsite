using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Profile_mygroup : System.Web.UI.Page
{
    LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            if (!IsPostBack)
            {
        
                showGroupmsg();
                showGroup();
            }
        }
        else
        {
            LearnSite.Common.CookieHelp.JudgeStudentCookies();
        }

    }

    private void showGroupmsg()
    {
        int mysid = cook.Sid;
        LearnSite.BLL.Students sbll = new LearnSite.BLL.Students();
        string sgtitle = sbll.GetSgtitle(mysid);
        if (!string.IsNullOrEmpty(sgtitle))
        {
            PanelSgtitle.Visible = true;
            TextBox1.Text = sgtitle;
        }
        else
        {
            PanelSgtitle.Visible = false;
        }
    }
    private void showGroup()
    {
        int sgrade = cook.Sgrade;
        int sclass = cook.Sclass;
        LearnSite.BLL.Students sbll = new LearnSite.BLL.Students();
        GVgroup.DataSource = sbll.ClassGroup(sgrade, sclass);
        GVgroup.DataBind();
        Labelfree.Text = sbll.FreeMember(sgrade, sclass);
    }

    protected void GVgroup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex > -1)
        {
            e.Row.Cells[0].Text = Convert.ToString(e.Row.RowIndex + 1);
            string sid = GVgroup.DataKeys[e.Row.RowIndex].Value.ToString();
            int sgrade = cook.Sgrade;
            int sclass = cook.Sclass; 
            Label lb = (Label)e.Row.FindControl("Labelmember");
            LearnSite.BLL.Students sbll = new LearnSite.BLL.Students();
            lb.Text = sbll.GroupMember(sgrade, sclass, int.Parse(sid));

            string strjs = "if(confirm('你确定参加该小组吗?'))return true;else return false; ";
            ((LinkButton)e.Row.FindControl("LinkButton1")).OnClientClick = strjs;
            string strjstwo = "if(confirm('你确定退出该小组吗?'))return true;else return false; ";
            ((LinkButton)e.Row.FindControl("LinkButton2")).OnClientClick = strjstwo;

            //当鼠标放上去的时候 先保存当前行的背景颜色 并给附一颜色 
            e.Row.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#E1E8E1',this.style.fontWeight='';");
            //当鼠标离开的时候 将背景颜色还原的以前的颜色 
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor,this.style.fontWeight='';");
            //单击行改变行背景颜色 
            e.Row.Attributes.Add("onclick", "this.style.backgroundColor='#D8E0D8'; this.style.color='buttontext';this.style.cursor='default';");
        }
    }
    protected void GVgroup_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string sid = e.CommandArgument.ToString();
        LearnSite.BLL.Students bll = new LearnSite.BLL.Students();
        int sgrade = cook.Sgrade;
        int sclass = cook.Sclass;
        int mysid = cook.Sid;
        string snum = cook.Snum;
        if (e.CommandName.Equals("AddGroup"))
        {
            int groupmax = 6;
            LearnSite.BLL.Room rbll = new LearnSite.BLL.Room();
            groupmax = rbll.GetRgroupMax(sgrade, sclass); // LearnSite.Common.XmlHelp.GetGroupMax();
            if (groupmax == 0)
                groupmax = 6;
            if (bll.GetGroupCount(sgrade, sclass, Int32.Parse(sid)) < groupmax + 1)
            {
                int res = bll.AddThisGroup(snum, Int32.Parse(sid));//每小组人数少于小组上限则可参加
                if (res > 0)
                {
                    System.Threading.Thread.Sleep(200);
                    LearnSite.Common.WordProcess.Alert("参加成功！", this.Page);
                    showGroup();
                }
                else
                {
                    LearnSite.Common.WordProcess.Alert("参加失败,你已经有组队！", this.Page);
                }
            }
            else
            {
                string ch = "小组人数已满" + groupmax + "位，请参加其他小组！";
                LearnSite.Common.WordProcess.Alert(ch, this.Page);
            }
        }
        if (e.CommandName.Equals("outGroup"))
        {
            int res = bll.OutThisGroup(snum);//每小组人数少于小组上限则可参加
            if (res > 0)
            {
                System.Threading.Thread.Sleep(200);
                LearnSite.Common.WordProcess.Alert("退出小组成功！", this.Page);
                showGroup();
            }
        }
    }

    protected void BtnSgtitle_Click(object sender, EventArgs e)
    {
        string inputmsg = TextBox1.Text.Trim();
        int wcount = inputmsg.Length;
        if (wcount > 2 && wcount < 9)
        {
            int mysid = cook.Sid;
            LearnSite.BLL.Students sbll = new LearnSite.BLL.Students();
            if (sbll.UpdateSgtitle(mysid, inputmsg) > 0)
            {
                LearnSite.Common.WordProcess.Alert("修改成功！", this.Page);
                showGroup();
            }
            else
                LearnSite.Common.WordProcess.Alert("修改失败！", this.Page);

        }
        else
            LearnSite.Common.WordProcess.Alert("请输入2～6个汉字长度！", this.Page);
    }
}