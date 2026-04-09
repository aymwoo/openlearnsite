using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Teacher_reorder : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LearnSite.Common.CookieHelp.JudgeTeacherCookies();
            showGrade();
            showClass();
        }
    }

    private void showGrade()
    {
        LearnSite.BLL.Room room = new LearnSite.BLL.Room();        
        Gradeselect.DataSource = room.GetAllGrade();
        Gradeselect.DataTextField = "Rgrade";
        Gradeselect.DataValueField = "Rgrade";
        Gradeselect.DataBind();
    }
    private void showClass()
    {
        string grade=Gradeselect.SelectedValue;
        LearnSite.BLL.Room rm = new LearnSite.BLL.Room();
        if (!string.IsNullOrEmpty(grade))
        {
            DataSet ds = rm.GetLimitAllClass(Int32.Parse(grade));
            Classone.DataSource = ds;
            Classone.DataTextField = "Rclass";
            Classone.DataValueField = "Rclass";
            Classone.DataBind();


            Classtwo.DataSource = ds;
            Classtwo.DataTextField = "Rclass";
            Classtwo.DataValueField = "Rclass";
            Classtwo.DataBind();


            SetClass.DataSource = ds;
            SetClass.DataTextField = "Rclass";
            SetClass.DataValueField = "Rclass";
            SetClass.DataBind();
        }
    }

    protected void Btnorder_Click(object sender, EventArgs e)
    {
        string grade = Gradeselect.SelectedValue;
        string clasone = Classone.SelectedValue;
        string clastwo = Classtwo.SelectedValue;
        string setclass = SetClass.SelectedValue;
        DateTime dt = DateTime.Today.AddDays(-1);
        int a = 0;
        int b = 0;
        int c = 0;
        LearnSite.BLL.Students stu = new LearnSite.BLL.Students();
        if (!string.IsNullOrEmpty(grade) && !string.IsNullOrEmpty(clasone) && !string.IsNullOrEmpty(clastwo) && !string.IsNullOrEmpty(setclass))
        {


            a = stu.UpdateStat(Int32.Parse(grade), Int32.Parse(clasone), Int32.Parse(clastwo), dt);//设定标志
            System.Threading.Thread.Sleep(500);
            b = stu.UpdateClass(Int32.Parse(grade), Int32.Parse(clasone), Int32.Parse(clastwo), Int32.Parse(setclass));//给标志班级更换班级号
            System.Threading.Thread.Sleep(500);
            if (setclass == clasone)
                setclass = clastwo;
            else
                setclass = clasone;
            c = stu.UpdateClassNoSign(Int32.Parse(grade), Int32.Parse(clasone), Int32.Parse(clastwo), Int32.Parse(setclass));//给标志班级更换班级号

        }
        string msg = "有提交作品人数：" + a.ToString() + " 更换班级号人数：" + b.ToString() + " 无标志更换班级号人数：" + c.ToString();
        LearnSite.Common.WordProcess.Alert("", this.Page);

    }

}