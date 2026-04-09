using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class python_visit : System.Web.UI.Page
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
        string strWhere = " Thid=0 and Tsid=0 ";
        DlTurtle.DataSource = bll.GetVisitList(strWhere);//显示访客作品
        DlTurtle.DataBind();
    }

}