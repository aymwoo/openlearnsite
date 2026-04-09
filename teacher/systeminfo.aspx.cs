using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Teacher_systeminfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (!IsPostBack)
        {
            Master.Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle() + "网站基本情况";
            ShowSystemInfo();
            ShowStatistic();
            showLoginStat();
        }
    }

    private void ShowSystemInfo()
        {
            Label1.Text = LearnSite.Common.Computer.GetServerIp();
            Label2.Text = LearnSite.Common.Computer.GetServerName();
            Label3.Text = LearnSite.Common.Computer.GetServerOs();
            Label4.Text = LearnSite.Common.Computer.GetServerCpu() + "个";
            Label5.Text = LearnSite.Common.Computer.GetServerCpuClass();
            Label6.Text ="LearnSite"+ LearnSite.Common.WordProcess.NewVersion()+"版" + LearnSite.Common.WordProcess.SysVerUpdate();
            Label7.Text = LearnSite.Common.Computer.GetServerSoftWare();
            Label8.Text = "asp.net" + LearnSite.Common.Computer.GetServerDotNetVer();
            Label9.Text = LearnSite.Common.Computer.GetServerScriptTimeout() + "秒";
            Label10.Text = LearnSite.Common.Computer.GetServerTickCount();
            Label11.Text = LearnSite.Common.Computer.GetServerProcessStartTime();
            Label12.Text = LearnSite.Common.Computer.GetServerAspNetWorkingSet() + "MB";
            Label13.Text = LearnSite.Common.Computer.GetServerAspNetCpuTime() + "分钟";
            Label14.Text = LearnSite.Common.Computer.GetServerCurrentThreadsNum() + "个";
            Label21.Text = LearnSite.Common.Computer.GetServerLanguage();
            Label22.Text = LearnSite.Common.Computer.GetSessionCount()+"个";
            Label23.Text = LearnSite.Common.App.AppCounts().ToString();
            Labelcomputer.Text = Environment.OSVersion.Platform.ToString();
            
            // 显示系统详细信息
            Label24.Text = Environment.OSVersion.ToString() + " (" + Environment.Version.ToString() + ")";
            Label25.Text = LearnSite.Common.Computer.GetMemoryInfo();
            Label26.Text = LearnSite.Common.Computer.GetDiskInfo();
            Label27.Text = LearnSite.Common.Computer.GetSystemLoad();
            Label28.Text = LearnSite.Common.Computer.GetNetworkUsage();
            Label29.Text = LearnSite.Common.Computer.GetTopProcesses();
        }
    private void showLoginStat()
    {
        bool teacherlogin = LearnSite.Common.XmlHelp.GetLogin();
        if (teacherlogin)
        {
            ImageLogin.ImageUrl = "~/images/red.gif?temp=" + DateTime.Now.Millisecond.ToString();
            ImageLogin.ToolTip = "教师平台限同网段访问";
        }
        else
        {
            ImageLogin.ImageUrl = "~/images/green.gif?temp=" + DateTime.Now.Millisecond.ToString();
            ImageLogin.ToolTip = "教师平台访问不受限制";
        }
    }
    private void ShowStatistic()
    {
        Label15.Text = LearnSite.DBUtility.DbHelperSQL.TableCounts("Courses").ToString();
        Label16.Text = LearnSite.DBUtility.DbHelperSQL.TableCounts("Works").ToString();
        Label17.Text = LearnSite.DBUtility.DbHelperSQL.TableCounts("Students").ToString();
        Label18.Text = LearnSite.DBUtility.DbHelperSQL.TableCounts("Signin").ToString();
        Label19.Text = LearnSite.DBUtility.DbHelperSQL.TableCounts("Ptyper").ToString();
        Label20.Text = LearnSite.DBUtility.DbHelperSQL.TableCounts("Soft").ToString();
    }
}
