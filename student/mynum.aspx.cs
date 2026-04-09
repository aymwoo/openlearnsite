using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Student_mynum : System.Web.UI.Page
{
    protected string SiteTitle { get; private set; }
    protected int loginMode = LearnSite.Common.XmlHelp.LoginMode();
    protected bool isSameNet = LearnSite.Common.Computer.IsSameNet();

    protected void Page_Load(object sender, EventArgs e)
    {
        SiteTitle = LearnSite.Common.XmlHelp.SiteTitle();
        if (string.IsNullOrEmpty(SiteTitle))
        {
            SiteTitle = "信息科技学习网站";
        }

        if (!IsPostBack)
        {
            ShowFoot();
            this.Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle() + " 学号查询";
            GradeClass();
            ListSnum();
            ShowPwd();
        }

    }

    private void ShowFoot()
    {
        DateTime dt3 = DateTime.Now;
        string pp = LearnSite.Common.Computer.MyIp();
        DateTime dt4 = DateTime.Now;
        DateTime dt5 = DateTime.Now;
        Labelhostname.Text = GetHostNameMy(pp);
        DateTime dt6 = DateTime.Now;

        Labelterm.Text = LearnSite.Common.XmlHelp.GetTerm();
        Labelip.Text = pp;
        Labelloadtime.Text = "IP：" + LearnSite.Common.Computer.DatagoneMilliseconds(dt3, dt4) + "毫秒&nbsp;&nbsp;" + "&nbsp;&nbsp;主机名：" + LearnSite.Common.Computer.DatagoneMilliseconds(dt5, dt6) + "毫秒&nbsp;";
        Labelversion.Text = LearnSite.Common.XmlHelp.LoginMode() == 1 ? "『班级模式』" : "『个人模式』";
    }

    private string GetHostNameMy(string aPip)
    {
        string msg = "否";
        if (!string.IsNullOrEmpty(aPip))
        {
            LearnSite.BLL.Computers cbll = new LearnSite.BLL.Computers();
            LearnSite.Model.Computers cmodel = cbll.GetModelByIp(aPip);
            bool autohostname = LearnSite.Common.XmlHelp.GetAutoHostName();
            if (cmodel != null)
            {
                msg = cmodel.Pmachine;
                if (!cmodel.Plock && autohostname)
                {
                    string newMachine = LearnSite.Common.Computer.GetGuestHost(aPip);
                    cbll.UpdateByPid(cmodel.Pid, newMachine);
                    msg = newMachine;
                }
            }
            else
            {
                string inum = TryGetInumByNetSegment(aPip);
                if (!string.IsNullOrEmpty(inum))
                {
                    msg = inum;
                }
                else if (autohostname)
                {
                    LearnSite.Model.Computers newmodel = new LearnSite.Model.Computers();
                    newmodel.Pip = aPip;
                    newmodel.Plock = true;
                    string addMachine = LearnSite.Common.Computer.GetGuestHost(aPip);
                    newmodel.Pmachine = addMachine;
                    newmodel.Pdate = DateTime.Now;
                    cbll.Add(newmodel);
                    msg = addMachine;
                }
            }
        }
        else
        {
            msg = "空";
        }
        return msg;
    }

    private string TryGetInumByNetSegment(string ip)
    {
        try
        {
            int? hid = LearnSite.Common.Computer.GetHidByIp(ip);
            if (hid.HasValue)
            {
                LearnSite.BLL.Ip ipBll = new LearnSite.BLL.Ip();
                return ipBll.GetInumByIpAndHid(ip, hid.Value);
            }
        }
        catch
        {
        }
        return string.Empty;
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
