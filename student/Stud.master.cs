using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Student_Stud : System.Web.UI.MasterPage
{
    protected string SiteTitle = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SiteTitle = LearnSite.Common.XmlHelp.SiteTitle();
            LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
            if (cook.IsExist())
            {
                int timepass = LearnSite.Common.Computer.TimePassed();
                this.Page.Title = HttpUtility.UrlDecode(cook.Sname) + " " + cook.Snum+" ("+timepass+")";
            }
        }
        
        // 检查学习汇总开关状态
        bool summaryEnabled = false;
        if (Application["SummaryEnabled"] != null)
        {
            summaryEnabled = Convert.ToBoolean(Application["SummaryEnabled"]);
        }
        else if (Session["SummaryEnabled"] != null)
        {
            summaryEnabled = Convert.ToBoolean(Session["SummaryEnabled"]);
        }
        else
        {
            // 从XML配置文件读取
            string summarySetting = LearnSite.Common.XmlHelp.GetTypeName("EnableSummary");
            if (!string.IsNullOrEmpty(summarySetting))
            {
                summaryEnabled = Convert.ToBoolean(summarySetting);
            }
        }
        
        // 检查荣誉榜开关状态
        bool honorsEnabled = false;
        if (Application["HonorsEnabled"] != null)
        {
            honorsEnabled = Convert.ToBoolean(Application["HonorsEnabled"]);
        }
        else if (Session["HonorsEnabled"] != null)
        {
            honorsEnabled = Convert.ToBoolean(Session["HonorsEnabled"]);
        }
        else
        {
            // 从XML配置文件读取
            string honorsSetting = LearnSite.Common.XmlHelp.GetTypeName("EnableHonors");
            if (!string.IsNullOrEmpty(honorsSetting))
            {
                honorsEnabled = Convert.ToBoolean(honorsSetting);
            }
        }
        
        // 根据开关状态控制链接显示
        if (hlSummary != null)
        {
            hlSummary.Visible = summaryEnabled;
        }
        if (hlSummaryMobile != null)
        {
            hlSummaryMobile.Visible = summaryEnabled;
        }
        if (hlHonors != null)
        {
            hlHonors.Visible = honorsEnabled;
        }
        if (hlHonorsMobile != null)
        {
            hlHonorsMobile.Visible = honorsEnabled;
        }
    }

}
