using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Student_mynum : System.Web.UI.Page
{
    protected int loginMode = LearnSite.Common.XmlHelp.LoginMode();
    protected bool isSameNet = LearnSite.Common.Computer.IsSameNet();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GradeClass();
            ListSnum();
            ShowPwd();
        }

    }
    private void ListSnum()
    {
        if (DDLgrade.SelectedValue != null && DDLclass.SelectedValue != null)
        {
            int Sgrade = Int32.Parse(DDLgrade.SelectedValue);
            int Sclass = Int32.Parse(DDLclass.SelectedValue);
            LearnSite.BLL.Students st = new LearnSite.BLL.Students();
            DataSet ds = st.GetNameNum(Sgrade, Sclass);
            
            // 创建新的DataTable，用于存储学生信息和机号
            DataTable dt = new DataTable();
            dt.Columns.Add("Snum", typeof(string));
            dt.Columns.Add("Sname", typeof(string));
            dt.Columns.Add("MachineName", typeof(string));
            
            // 遍历每个学生，获取机号并添加到DataTable
            LearnSite.BLL.Computers cbll = new LearnSite.BLL.Computers();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string snum = row["Snum"].ToString();
                string sname = row["Sname"].ToString();
                string machineName = cbll.GetMachineBySnum(snum);
                
                DataRow newRow = dt.NewRow();
                newRow["Snum"] = snum;
                newRow["Sname"] = sname;
                newRow["MachineName"] = machineName;
                dt.Rows.Add(newRow);
            }
            
            // 按机号排序
            dt.DefaultView.Sort = "MachineName ASC";
            DataListsnum.DataSource = dt.DefaultView;
            DataListsnum.DataBind();
        }
    }

    private void GradeClass()
    {
        LearnSite.BLL.Room room = new LearnSite.BLL.Room();
        DDLgrade.DataSource = room.GetAllGrade();
        DDLgrade.DataTextField = "Rgrade";
        DDLgrade.DataValueField = "Rgrade";
        DDLgrade.DataBind();

        LearnSite.BLL.Room rm = new LearnSite.BLL.Room();
        DDLclass.DataSource = rm.GetAllClass();
        DDLclass.DataTextField = "Rclass";
        DDLclass.DataValueField = "Rclass";
        DDLclass.DataBind();
        if (null != Application["MyNumGrade"])
        {
            DDLgrade.SelectedValue = Application["MyNumGrade"].ToString();
        }
        if (null != Application[Application["MyNumGrade"] + "MyNumClass"])
        {
            DDLclass.SelectedValue = Application[Application["MyNumGrade"] + "MyNumClass"].ToString();
        }
    }
    protected void DataListsnum_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        HyperLink hl = new HyperLink();
        hl = (HyperLink)e.Item.FindControl("HLSnum");
        
        // 查找图片的超链接控件
        HyperLink hlImage = (HyperLink)e.Item.FindControl("HLImage");
        
        // 从数据项中获取学生信息
        DataRowView drv = (DataRowView)e.Item.DataItem;
        string snum = drv["Snum"].ToString();
        string sname = drv["Sname"].ToString();
        string machineName = drv["MachineName"].ToString();
        
        // 调试信息 - 显示获取的机器名
        System.Diagnostics.Debug.WriteLine("学号: " + snum + ", 机器名: " + (string.IsNullOrEmpty(machineName) ? "空" : machineName));
        
        // 在学号前面显示机号
        if (!string.IsNullOrEmpty(machineName))
        {
            hl.Text = machineName + ":" + sname;
            hl.ToolTip = machineName + ":" + snum;
            
            // 设置图片超链接的提示信息
            if (hlImage != null)
            {
                hlImage.ToolTip = machineName + ":" + snum;
            }
        }
        else
        {
            // 如果没有获取到机器名，显示提示信息
            hl.Text = sname;
            hl.ToolTip = snum;
            
            // 设置图片超链接的提示信息
            if (hlImage != null)
            {
                hlImage.ToolTip = snum;
            }
        }
        
        // 设置导航地址
        string navigateUrl = "~/index.aspx?mysnum=" + snum + "&myname=" + sname;
        hl.NavigateUrl = navigateUrl;
        
        // 为图片超链接设置相同的导航地址
        if (hlImage != null)
        {
            hlImage.NavigateUrl = navigateUrl;
        }
        
        string ssex = "无";
        Image img = new Image();
        img = (Image)e.Item.FindControl("ImageStu"); 
        if (isSameNet)
        {
            img.ImageUrl = LearnSite.Common.Photo.GetStudentPhotoUrl(snum, ssex) + "?temp=" + DateTime.Now.Millisecond.ToString();
        }
        else
        {
            //如果同一网段，显示头像，否则不显示
            img.ImageUrl = "~/images/nothing.gif";
            img.ToolTip = "头像隐藏";
        }
    }
    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        ListSnum();
        ShowPwd();
        System.Threading.Thread.Sleep(200);
    }
    private void ShowPwd()
    {
        if (loginMode == 1)
        {
            //LoginMode为1 表示班级模式，则显示密码，否则不执行下面代码
            int Sgrade = Int32.Parse(DDLgrade.SelectedValue);
            int Sclass = Int32.Parse(DDLclass.SelectedValue);
            LearnSite.BLL.Room rm = new LearnSite.BLL.Room();
            TextBoxPwd.Text = rm.GetRoomPwd(Sgrade, Sclass);
        }
    }
    protected void DDLclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        ListSnum();
        ShowPwd();

    }
    protected void DDLgrade_SelectedIndexChanged(object sender, EventArgs e)
    {
        ListSnum();
        ShowPwd();
    }
}
