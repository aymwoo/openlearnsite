using System;

public partial class Teacher_student : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Redirect("~/teacher/teachermanage.aspx", false);
    }
}
