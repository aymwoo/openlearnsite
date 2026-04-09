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
    }
}
