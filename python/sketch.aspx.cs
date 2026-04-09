using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Python_sketch : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
             Listall();
        }
    }

    private void Listall()
    {
        LearnSite.BLL.Turtle bll = new LearnSite.BLL.Turtle();
        LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();
        if (tcook.IsExist())
        {
            string strWhere = "Tsid<>0 ";
            DlTurtle.DataSource = bll.GetSketchList(strWhere);//老师查看所有学生作品
            DlTurtle.DataBind();
        }

        LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
        if (cook.IsExist())
        {
            string strWhere = " Tsid=" + cook.Sid.ToString();
            DlTurtle.DataSource = bll.GetSketchList(strWhere);//学生查看自己作品
            DlTurtle.DataBind();
        }
    }

}