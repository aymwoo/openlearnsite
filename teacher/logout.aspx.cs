using System;
using System.Web;
using System.Web.UI;

public partial class Teacher_logout : System.Web.UI.Page
{
    LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Cookies[LearnSite.Common.CookieHelp.teaCookieNname] != null)
        {
            int Rhid = tcook.Hid;
            LearnSite.BLL.Room bll = new LearnSite.BLL.Room();
            bll.UnlineClass(Rhid);
            LearnSite.Common.CookieHelp.ClearTeacherCookies();
            LearnSite.Common.CookieHelp.ClearStudentCookies();
            LearnSite.Common.App.AppUserMatchRemove("s" + Rhid.ToString());
            LearnSite.Common.App.CurrentClassRemove(Rhid);
            Session.Abandon();
            Session.RemoveAll();
            Session.Clear();
            LearnSite.Common.Others.ClearClientPageCache();
            Request.Cookies.Clear();
            System.Threading.Thread.Sleep(200);
        }
        string rurl = "~/teacher/index.aspx?qt=" + DateTime.Now.Millisecond.ToString();
        Response.Redirect(rurl, false);
    }
}
