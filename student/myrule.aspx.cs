using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Student_myrule : System.Web.UI.Page
{
    protected string SiteTitle { get; private set; }

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
        }

        Btnreturn.Attributes.Add("onclick", "window.opener=null;window.open('','_self'); window.close()");
        this.Page.Title =LearnSite.Common.CookieHelp.SetMainPageTitle()+ " 课堂守则";
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
}
