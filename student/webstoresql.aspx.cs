using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class student_webstoresql : System.Web.UI.Page
{
    protected string mysnum = "test11";
    protected string Sname = "Tester";
    protected string Fpage = "#";
    protected string Id = " ";
    LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ShowStudent();
        }

    }


    private void ShowStudent()
    {
        if (cook.IsExist())
        {
            Sname = Server.UrlDecode(cook.Sname);
            Labelname.Text = Sname;
            mysnum = cook.Snum;
            //Imageface.ImageUrl = LearnSite.Common.Photo.GetStudentPhotoUrl(mysnum, cook.Sex);
        }
        if (Request.QueryString["lid"] != null)
        {
            string Lid = Request.QueryString["lid"].ToString();
            LearnSite.BLL.ListMenu lbll = new LearnSite.BLL.ListMenu();
            LearnSite.Model.ListMenu lmodel = new LearnSite.Model.ListMenu();
            lmodel = lbll.GetModel(Int32.Parse(Lid));

            string Cid = lmodel.Lcid.ToString();
            string Mid = lmodel.Lxid.ToString();
            Id = Cid + "-" + Mid + "-" + Lid;

            int mill = DateTime.Now.Millisecond;
            Fpage = "../student/program.aspx?lid=" + Lid + "&mill=" + mill;
        }
    }
}