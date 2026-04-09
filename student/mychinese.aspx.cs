using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Student_mychinese : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            if (!IsPostBack)
            {
                showChinese();
            }
        }
        else
        {
            LearnSite.Common.CookieHelp.JudgeStudentCookies();
        }
    }
    private void showChinese()
    {
        LearnSite.Model.Cook cook = new LearnSite.Model.Cook();

        LearnSite.BLL.Room rbll = new LearnSite.BLL.Room();
        string Nids = rbll.GetRchineseByClass(cook.Sgrade, cook.Sclass);

        LearnSite.BLL.Chinese cbll = new LearnSite.BLL.Chinese();
        DataList1.DataSource = cbll.ShowAllNid(Nids);
        DataList1.DataBind();
        if (DataList1.Items.Count > 0)
            Lbnid.Text = ((Label)DataList1.Items[0].FindControl("Lbid")).Text;
    }
    protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        Label lbntitle = (Label)e.Item.FindControl("Lbtitle");
        Label lbnid = (Label)e.Item.FindControl("Lbid");
        string id = lbnid.Text;
        string jslb = "GetWords(" + id + ");";
        lbntitle.Attributes.Add("onclick", jslb);
    }
}