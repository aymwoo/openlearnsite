using System;
using System.Web.UI;

public partial class Student_Profile_Pf : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            LearnSite.Common.CookieHelp.JudgeStudentCookies();
        }
    }
}
