using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class python_matchnew : System.Web.UI.Page
{
    LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void BtnCreate_Click(object sender, EventArgs e)
    {
        if (tcook.IsExist()) {
            if (Texttitle.Text != "")
            {
                string ctitle = Texttitle.Text.Trim();
                LearnSite.Model.TurtleMatch model = new LearnSite.Model.TurtleMatch();
                model.Mhid = tcook.Hid;
                model.Mtitle = ctitle;
                model.Mcontent = "";
                model.Mbegin = DateTime.Now;
                model.Mend = DateTime.Now.AddYears(1);
                model.Mpublish = Checkcpublish.Checked;
                model.Mdate = DateTime.Now;

                LearnSite.BLL.TurtleMatch bll = new LearnSite.BLL.TurtleMatch();
                int newid= bll.Add(model);

                string url = "~/python/matchshow.aspx?id="+newid.ToString();
                Response.Redirect(url, false);
            }
        }
    }
    protected void Btnreturn_Click(object sender, EventArgs e)
    {
        string url = "~/python/match.aspx" ;
        Response.Redirect(url, false);
    }
}