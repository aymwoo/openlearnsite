using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Student_showcourse : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            if (Request.QueryString["cid"] != null)
            {
                LearnSite.Common.CookieHelp.KickStudent();
                if (!IsPostBack)
                {
                    ShowCourse();
                }
            }
        }
        else
        {
            LearnSite.Common.CookieHelp.JudgeStudentCookies();
        }

    }
    private void ShowCourse()
    {
        string Cid = Request.QueryString["cid"].ToString();
        if (LearnSite.Common.WordProcess.IsNum(Cid))
        {
            LearnSite.Model.Courses model = new LearnSite.Model.Courses();
            LearnSite.BLL.Courses cs = new LearnSite.BLL.Courses();
            model = cs.GetModel(Int32.Parse(Cid));
            if (model != null)
            {
                LabelCtitle.Text = model.Ctitle;
                // LabelCtitle.Visible = false;
                Ccontent.InnerHtml = HttpUtility.HtmlDecode(model.Ccontent);
            }
            else
            {
                Ccontent.InnerHtml = "此学案不存在！";
            } 
        }
    }

}
