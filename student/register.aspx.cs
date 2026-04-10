using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Student_register : System.Web.UI.Page
{
    protected string SiteTitle { get; private set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        SiteTitle = LearnSite.Common.XmlHelp.SiteTitle();
        if (string.IsNullOrEmpty(SiteTitle))
        {
            SiteTitle = "信息科技学习网站";
        }

        if (LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            LearnSite.Model.Cook cook = new LearnSite.Model.Cook();

            int Qgrade = cook.Sgrade;
            int Qclass = cook.Sclass;
            OpenJump(Qgrade, Qclass);//跳转选择
        }
        else
        {
            if (!IsPostBack)
            {
                ShowFoot();
                this.Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle() + " 学员注册";
                SetGrade();
                SetClass();
                SetSex();
            }
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
    protected void BtnRegister_Click(object sender, EventArgs e)
    {
        string g = DDLgrade.SelectedValue;
        string c = DDLclass.SelectedValue;
        string x = DDLsex.SelectedValue;
        string n = Tsname.Text.Trim();
        if (g.Length > 0 && c.Length > 0 && x.Length > 0 && n.Length > 0 && n.Length < 10)
        {
            if (LearnSite.Common.WordProcess.IsChina(n))
            {
                int Sgrade = Int32.Parse(g);
                int Sclass = Int32.Parse(c);
                LearnSite.BLL.Students stubll = new LearnSite.BLL.Students();
                long NewSnum = stubll.GetMaxSnum(Sgrade, Sclass);
                LearnSite.BLL.DelStudents dbll = new LearnSite.BLL.DelStudents();
                string mySyear = stubll.GetYear(Sgrade);
                int Syear = Int32.Parse(mySyear);
                LearnSite.Model.Students student = new LearnSite.Model.Students();
                student.Syear = Syear;
                student.Sgrade = Sgrade;
                student.Sclass = Sclass;
                student.Sname = n;
                student.Sex = DDLsex.SelectedValue;
                string myPwd = "12345"; //LearnSite.Common.WordProcess.GetRandomNumber(3);
                student.Spwd = myPwd;
                student.Saddress = "";
                student.Sphone = "";
                student.Sparents = "";
                student.Sheadtheacher = "在线注册";
                student.Sscore = 0;
                student.Sattitude = 0;
                string Tsnum = dbll.GetNewSnum(NewSnum);//获取删除列表中不存在的新学号
                student.Snum = Tsnum;
                int Sid = stubll.AddStudent(student);
                if (Sid > 1)
                {
                    student.Sid = Sid;//修正注册后cookies中的Sid值  2014-9-28号

                    System.Threading.Thread.Sleep(200);
                    LearnSite.Common.WordProcess.Alert("注册成功，你的学号为" + Tsnum + "密码为" + myPwd , this.Page);
                    string lbip = Page.Request.UserHostAddress;
                    int Qterm = Int32.Parse(LearnSite.Common.XmlHelp.GetTerm());
                    if (LearnSite.Common.CookieHelp.SetStudentCookies(student, lbip))//写cookies
                    {
                        DateTime LoginTime = DateTime.Now;
                        LearnSite.BLL.Signin gbll = new LearnSite.BLL.Signin();
                        string Qtitle = ""; // 新注册学生暂无课程
                        string Qsession = LearnSite.Common.TimeSlotHelper.GetCurrentTimeSlot(); // 当前时间段
                        gbll.SigninToday(Tsnum, LoginTime, lbip, Sgrade, Qterm, Sid, n, Sclass, Syear, Qtitle, Qsession);//签到
                        System.Threading.Thread.Sleep(200);
                        OpenJump(Sgrade, Sclass);//跳转选择
                    }
                }
                else
                {
                    labelmsg.Text = "自动申请的学号已被使用，请点击注册继续申请！";
                }
            }
            else
            {
                labelmsg.Text = "注册名必须为中文！";
            }
        }
        else
        {
            labelmsg.Text = "注册失败！<br/>（当前无班级可注册或姓名长度超过限制！）";
        }
    }
    private void SetSex()
    {
        ListItem lim = new ListItem();
        lim.Text = "男";
        lim.Value = "男";
        DDLsex.Items.Add(lim);
        ListItem liw = new ListItem();
        liw.Text = "女";
        liw.Value = "女";
        DDLsex.Items.Add(liw);
    }
    private void SetGrade()
    {
        LearnSite.BLL.Room rm = new LearnSite.BLL.Room();
        DDLgrade.DataSource = rm.GetAllRegGrade();
        DDLgrade.DataTextField = "Rgrade";
        DDLgrade.DataValueField = "Rgrade";
        DDLgrade.DataBind();
    }

    private void SetClass()
    {
        string Cgrade=DDLgrade.SelectedValue;
        if (string.IsNullOrEmpty(Cgrade))
        {
            BtnRegister.Enabled = false;
            labelmsg.Text = "暂停注册";
        }
        else
        {
            int Rgrade = Int32.Parse(Cgrade);
            LearnSite.BLL.Room rm = new LearnSite.BLL.Room();
            DDLclass.DataSource = rm.GetRegClass(Rgrade);
            DDLclass.DataTextField = "Rclass";
            DDLclass.DataValueField = "Rclass";
            DDLclass.DataBind();
            labelmsg.Text = "";
        }
    }

    protected void DDLgrade_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetClass();
    }

    private void OpenJump(int Sgrade, int Sclass)
    {
        LearnSite.BLL.Room rm = new LearnSite.BLL.Room();
        string Cid = rm.IsRopenRcid(Sgrade, Sclass);

        if (string.IsNullOrEmpty(Cid))
        {
            Response.Redirect("~/student/myinfo.aspx", false);//如果返回Cid为空
        }
        else
        {
            string myurl = "~/student/showcourse.aspx?cid=" + Cid;//快速模式为真，且返回Cid不为空
            Response.Redirect(myurl, true);
        }
    }
    protected void BtnReturn_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/index.aspx", false);
    }
}
