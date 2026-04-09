using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Python_index : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Listall();
            showface();
        }
    }

    private void Listall() 
    {
        LearnSite.BLL.Turtle bll = new LearnSite.BLL.Turtle();
        DlTurtle.DataSource = bll.GetTeacherList();
        DlTurtle.DataBind();    
    }

    private void showface()
    {
        LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();
        LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
        if (tcook.IsExist())
        {
            Imageface.ImageUrl = "~/images/happy.gif";
            Imageface.ToolTip = Server.UrlDecode(tcook.Hnick);
        }
        else {
            if (cook.IsExist())
            {
                string murl = LearnSite.Common.Photo.GetStudentPhotoUrl(cook.Snum, cook.Sex);
                Imageface.ImageUrl = murl + "?temp=" + DateTime.Now.Millisecond.ToString();
                Imageface.ToolTip = Server.UrlDecode(cook.Sname);
            }
            else
            {
                Imageface.ImageUrl = "~/images/nothing.gif";
                Imageface.ToolTip = "游客";          
            }
        }
    }
}