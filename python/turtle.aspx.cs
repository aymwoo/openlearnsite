using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Python_turtle : System.Web.UI.Page
{
    protected string Id = "";
    protected string title = "";
    protected string Codefile = "";
    protected string Snum = " ";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            showTurtle();
        }
    }

    private void showTurtle() 
    {
        if (Request.QueryString["id"] != null)
        {
            Id = Request.QueryString["id"].ToString();
            LearnSite.BLL.Turtle bll = new LearnSite.BLL.Turtle();
            LearnSite.Model.Turtle model = new LearnSite.Model.Turtle();
            model = bll.GetModel(Int32.Parse(Id));
            title = model.Ttilte;
            Codefile = model.Tcode;

            LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
            if (cook.IsExist())
            {
                Snum = cook.Snum;
            }
        }    
    }
}