using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Teach : System.Web.UI.MasterPage
{
    protected string SiteTitle { get; private set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        SiteTitle = LearnSite.Common.XmlHelp.SiteTitle();
        if (!IsPostBack)
        {
            LabelVer.Text = LearnSite.Common.WordProcess.SystemVersion();
            LabelVer.ToolTip = LearnSite.Common.WordProcess.SysVerUpdate();
        }
        LogoutBtn.Visible = (Request.Cookies[LearnSite.Common.CookieHelp.teaCookieNname] != null);
        
        // 设置CSS和JS路径
        css1.Text = string.Format("<link rel='stylesheet' type='text/css' href='{0}' />", ResolveUrl("~/App_Themes/Teacher/StyleSheet.css"));
        css2.Text = string.Format("<link rel='stylesheet' type='text/css' href='{0}' />", ResolveUrl("~/App_Themes/Teacher/course-workspace.css"));
        css3.Text = string.Format("<link rel='stylesheet' type='text/css' href='{0}' />", ResolveUrl("~/App_Themes/Teacher/course-content-add.css"));
        css4.Text = string.Format("<link rel='stylesheet' type='text/css' href='{0}' />", ResolveUrl("~/js/css/tailwind-utilities-2.2.19.min.css"));
        
        js1.Text = string.Format("<script src='{0}' type='text/javascript'></script>", ResolveUrl("~/js/MenuCookie.js"));
        js2.Text = string.Format("<script src='{0}' type='text/javascript'></script>", ResolveUrl("~/js/jquery-1.8.2.min.js"));
        js3.Text = string.Format("<script src='{0}' type='text/javascript'></script>", ResolveUrl("~/kindeditor/plugins/code/prettify.js"));
        js4.Text = string.Format("<script src='{0}' type='text/javascript'></script>", ResolveUrl("~/js/ruffle.js"));
    }
}
