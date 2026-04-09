using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Teacher_index : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (LearnSite.Common.CookieHelp.IsTeacherLogin())
        {
            Response.Redirect("~/teacher/infomation.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return;
        }
        else
        {
            if (LearnSite.Common.CookieHelp.IsManagerLogin())
            {
                Response.Redirect("~/manager/index.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }
            else {
                Request.Cookies.Clear();
            }
        }
        if (!IsPostBack)
        {
            this.Page.Title = "教师登录页面";
            Textname.Focus();
            if (!verChecking())
                return;
        }
    }
    protected void Btnlogin_Click(object sender, EventArgs e)
    {
        bool issamenet = LearnSite.Common.XmlHelp.GetLogin();
        bool checkresult = LearnSite.Common.Computer.IsSameNet();
        if (issamenet)
        {
            if (checkresult)
                LoginCode();//如果受限制，且在同网段内，则允许访问
            else
            {
                Labelmsg.Text = "『许可访问→内网√』";
                Textname.Text = "";
                Textpwd.Text = "";
                LearnSite.Common.WordProcess.Alert("许可访问为：仅内网访问!", this.Page);
            }
        }
        else
        {
            LoginCode();
        }
    }
    /// <summary>
    /// 教师平台
    /// </summary>
    private void LoginCode()
    {
        string aaaastr = "teacherlogintime";
        string Hname = Textname.Text.Trim();
        string Hpwd = Textpwd.Text.Trim();
        if (Hname != "" && Hpwd != "")
        {
            LearnSite.Model.Teacher Tmodel = new LearnSite.Model.Teacher();
            LearnSite.BLL.Teacher Tbll = new LearnSite.BLL.Teacher();
            Tmodel = Tbll.GetTeacherModel(Hname, Hpwd);
            if (Tmodel != null)
            {
                //登录cookie设置和跳转
                bool hpermiss = Tmodel.Hpermiss;
                Request.Cookies.Clear();
                if (LearnSite.Common.CookieHelp.SetTMCookies(Tmodel, hpermiss))
                {
                    System.Threading.Thread.Sleep(200);
                    if (hpermiss)
                    {
                        Response.Redirect("~/manager/index.aspx", false);
                    }
                    else
                    {
                        LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();
                        Session[tcook.Hid.ToString() + aaaastr] = DateTime.Now.ToString();
                        Response.Redirect("~/teacher/infomation.aspx", false);
                    }
                }
            }

            else
            {
                Labelmsg.Text = "用户名或密码错误！";
                Textname.Text = "";
                Textpwd.Text = "";
            }
        }
        else
        {
            Labelmsg.Text = "输入不能为空！";
        }
    }

    /// <summary>
    /// 检测数据库和表
    /// </summary>
    private bool verChecking()
    {
        if (!LearnSite.DBUtility.SqlHelper.DatabaseExist())
        {
            string cc = "你的数据库连接不上，现在将跳转到更新程序UpGrade.aspx，修改后请执行更新！";
            LearnSite.Common.WordProcess.Alert(cc, this.Page);
            Response.Redirect("~/upgrade.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return false;
        }
        else
        {
            //LearnSite.DBUtility.UpdateGrade.UpdateEnglishLevel3();//编程英语自动补丁
            //LearnSite.DBUtility.UpdateGrade.UpdateIdleClass();//在线测评自动补丁
            if (!LearnSite.DBUtility.UpdateGrade.VersionCheck())
            {
                string ch = "您的数据库未更新，现在将跳到更新程序UpGrade.aspx，请执行更新，不影响原有数据！";
                LearnSite.Common.WordProcess.Alert(ch, this.Page);
                Response.Redirect("~/upgrade.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return false;
            }
        }

        return true;
    }
}
