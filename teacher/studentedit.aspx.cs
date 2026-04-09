using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Teacher_studentedit : System.Web.UI.Page
{
    LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["sid"] != null)
            {
                if (Request.Cookies[LearnSite.Common.CookieHelp.teaCookieNname] != null)
                {
                    SetSex();
                    myGrade();
                    ShowStudent();
                }
            }
        }
    }
    protected void BtnsEdit_Click(object sender, EventArgs e)
    {
        if (Request.Cookies[LearnSite.Common.CookieHelp.teaCookieNname] != null)
        {
            int Sid = Int32.Parse(Request.QueryString["sid"].ToString());
            LearnSite.Model.Students student = new LearnSite.Model.Students();
            student.Sid = Sid;
            student.Syear = Int32.Parse(Tsyear.Text.Trim());
            student.Sgrade = Int32.Parse(DDLgrade.SelectedValue);
            student.Sclass = Int32.Parse(DDLclass.SelectedValue);
            student.Sname = HttpUtility.HtmlEncode(Tsname.Text.Trim());
            student.Sex = HttpUtility.HtmlEncode(DDLsex.SelectedValue);
            student.Spwd = HttpUtility.HtmlEncode(Tspwd.Text.Trim());
            student.Saddress = HttpUtility.HtmlEncode(Tsaddress.Text.Trim());
            student.Sphone = HttpUtility.HtmlEncode(Tsphone.Text.Trim());
            student.Sparents = HttpUtility.HtmlEncode(Tsparents.Text.Trim());
            student.Sheadtheacher = HttpUtility.HtmlEncode(Tsheadtheacher.Text.Trim());
            LearnSite.BLL.Students stubll = new LearnSite.BLL.Students();
            stubll.Update(student);
            Btnsedit.Text = "修改成功";

            System.Threading.Thread.Sleep(1000);
            if (Request.QueryString["modal"] == "1")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "closemodal", "window.parent.notifyLessonModalSuccess(true);", true);
            }
            else
            {
                string url = "~/teacher/studentshow.aspx?sid=" + Sid;
                Response.Redirect(url, false);
            }
        }
    }
    private void ShowStudent()
    {
        int Sid =Int32.Parse( Request.QueryString["sid"].ToString());
        LearnSite.Model.Students student = new LearnSite.Model.Students();
        LearnSite.BLL.Students stubll = new LearnSite.BLL.Students();
        student = stubll.GetModel(Sid);
        Tsnum.Text = student.Snum;
        Tsyear.Text = student.Syear.ToString();
        DDLgrade.SelectedValue = student.Sgrade.ToString();
        myClass(student.Sgrade.Value);//2014-10-28修改　因为初始化的年级获取的班级数不足，所以要获取
        DDLclass.SelectedValue = student.Sclass.ToString();
        Tsname.Text = student.Sname;
        Tspwd.Text = student.Spwd;
        DDLsex.SelectedValue = student.Sex;
        Tsaddress.Text = student.Saddress;
        Tsphone.Text = student.Sphone;
        Tsparents.Text = student.Sparents;
        Tsheadtheacher.Text = student.Sheadtheacher;
        Tsscore.Text = student.Sscore.ToString();
        Tsattitude.Text = student.Sattitude.ToString();
        string Hid = tcook.Hid.ToString();
        Session[Hid + "grade"] = DDLgrade.SelectedValue;
        Session[Hid + "class"] = DDLclass.SelectedValue;
    }
    protected void Btnreturn_Click(object sender, EventArgs e)
    {
        string url = "~/teacher/teachermanage.aspx" ;        
        Response.Redirect(url, false);
    }

    private void myGrade()
    {
        LearnSite.BLL.Room room = new LearnSite.BLL.Room();
        string Hid = tcook.Hid.ToString();
        DDLgrade.DataSource = room.GetGrade(Int32.Parse(Hid));
        DDLgrade.DataTextField = "Rgrade";
        DDLgrade.DataValueField = "Rgrade";
        DDLgrade.DataBind();
    }
    private void myClass(int Rgrade)
    {
        LearnSite.BLL.Room room = new LearnSite.BLL.Room();
        DDLclass.DataSource = room.GetLimitAllClass(Rgrade);
        DDLclass.DataTextField = "Rclass";
        DDLclass.DataValueField = "Rclass";
        DDLclass.DataBind();
    }
    private void SetSex()
    {
        ListItem lim = new ListItem();
        lim.Text = "男";
        lim.Value = "男";
        DDLsex.Items.Add(lim);
        ListItem liw = new ListItem();
        liw.Text = "女";
        liw.Value = "女";
        DDLsex.Items.Add(liw);
    }
}
