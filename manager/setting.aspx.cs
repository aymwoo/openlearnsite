using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Manager_setting : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeIsAdmin();
        if (!IsPostBack)
        {
            Master.Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle() + "系统设置页面";
            ShowInfo();
            Btnpublish.Attributes.Add("onclick", "return   confirm('您确定要全部学案收回隐藏吗？');");
        }
    }

    private void ShowInfo()
    {
        TextBoxsite.Text = LearnSite.Common.XmlHelp.SiteTitle().ToString();
        DDLterm.SelectedValue = LearnSite.Common.XmlHelp.GetTerm().ToString();
        CheckBoxDownCan.Checked = LearnSite.Common.XmlHelp.DowncanIs();
        DDLDownTime.SelectedValue = LearnSite.Common.XmlHelp.GetDowntime().ToString();
        DDLLoginMode.SelectedValue = LearnSite.Common.XmlHelp.LoginMode().ToString();
        DDLworkdowntime.SelectedValue = LearnSite.Common.XmlHelp.GetWorkDowntime();
        CheckBoxWorkIp.Checked = LearnSite.Common.XmlHelp.GetWorkIpLimit();
        DDLCookiesPeriod.SelectedValue = LearnSite.Common.XmlHelp.GetStudentCookiesPeriod();
        CheckBoxSingleLogin.Checked = LearnSite.Common.XmlHelp.GetSingleLogin();
        DDLUploadMode.SelectedValue = LearnSite.Common.XmlHelp.GetUploadMode();
        CheckBoxLogin.Checked = LearnSite.Common.XmlHelp.GetLogin();
        CheckBoxLogin.Checked = LearnSite.Common.XmlHelp.GetLogin();
    }

    protected void CheckBoxDownCan_CheckedChanged(object sender, EventArgs e)
    {
        LearnSite.Common.XmlHelp.SetDowncan(CheckBoxDownCan.Checked);
        Labelmsg.Text = "下载限制修改成功！";
    }
    protected void DDLDownTime_SelectedIndexChanged(object sender, EventArgs e)
    {
        LearnSite.Common.XmlHelp.SetDowntime(DDLDownTime.SelectedValue);
        Labelmsg.Text = "下载时间修改成功！";
    }
    protected void DDLterm_SelectedIndexChanged(object sender, EventArgs e)
    {
        LearnSite.Common.XmlHelp.SetTerm(DDLterm.SelectedValue);
        Labelmsg.Text = "学期设置成功！";
    }
    protected void Buttonsite_Click(object sender, EventArgs e)
    {
        LearnSite.Common.XmlHelp.SetSiteTitle(TextBoxsite.Text.Trim());
        Labelmsg.Text = "网站标题修改成功！";
    }
    protected void DDLLoginMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        LearnSite.Common.XmlHelp.SetLoginMode(DDLLoginMode.SelectedValue);
        Labelmsg.Text = "登录模式设置成功！";
    }
    protected void DDLworkdowntime_SelectedIndexChanged(object sender, EventArgs e)
    {
        LearnSite.Common.XmlHelp.SetWorkDowntime(DDLworkdowntime.SelectedValue);
        Labelmsg.Text = "作品查看天数修改成功！";
    }
    protected void CheckBoxWorkIp_CheckedChanged(object sender, EventArgs e)
    {
        LearnSite.Common.XmlHelp.SetWorkIpLimit(CheckBoxWorkIp.Checked);
        Labelmsg.Text = "作品提交Ip限制修改成功！";
    }
    protected void DDLCookiesPeriod_SelectedIndexChanged(object sender, EventArgs e)
    {
        LearnSite.Common.XmlHelp.SetStudentCookiesPeriod(DDLCookiesPeriod.SelectedValue);
        Labelmsg.Text = "学生登录Cookies有效时间修改成功！";
    }
    protected void Btnpublish_Click(object sender, EventArgs e)
    {
        LearnSite.BLL.Courses bll = new LearnSite.BLL.Courses();
        bll.HideCourse();
        Labelmsg.Text = "全部学案收回隐藏成功！";
    }
    protected void CheckBoxSingleLogin_CheckedChanged(object sender, EventArgs e)
    {
        LearnSite.Common.XmlHelp.SetSingleLogin(CheckBoxSingleLogin.Checked.ToString());
        Labelmsg.Text = "单点登录设置修改成功！";
    }

    protected void DDLUploadMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        string upmodel = DDLUploadMode.SelectedValue;
        LearnSite.Common.XmlHelp.SetUploadMode(upmodel);
        Labelmsg.Text = "本平台的作品上传设置为：" + DDLUploadMode.SelectedItem.Text;
    }

    protected void CheckBoxLogin_CheckedChanged(object sender, EventArgs e)
    {
        bool teacherlogin = CheckBoxLogin.Checked;
        LearnSite.Common.XmlHelp.SetLogin(teacherlogin.ToString());
        if (teacherlogin)
        {
            Labelmsg.Text = "当前教师平台访问限制为同网段的局域网";
            ImageLogin.ImageUrl = "~/images/red.gif?temp=" + DateTime.Now.Millisecond.ToString();
        }
        else
        {
            Labelmsg.Text = "当前教师平台访问不受限制";
            ImageLogin.ImageUrl = "~/images/green.gif?temp=" + DateTime.Now.Millisecond.ToString();
        }
    }

}
