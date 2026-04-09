using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Student_codeshare : System.Web.UI.Page
{
    protected string sbfile = "";
    protected string sbtitle = "未命名";
    protected string isplayer = "true";
    protected string Owner = "";
    protected string Pic ="";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            showScratch();
        }
    }

    protected void showScratch()
    {
        if (Request.QueryString["id"] != null)
        {
            int wid = Int32.Parse(Request.QueryString["id"].ToString());
            LearnSite.BLL.Works wbll = new LearnSite.BLL.Works();
            LearnSite.Model.Works model = new LearnSite.Model.Works();

            model = wbll.GetModel(wid);
            sbfile = model.Wurl.Replace("~", "..") ;//如果不为空，则获取原来的作品链接
        }
    }
}