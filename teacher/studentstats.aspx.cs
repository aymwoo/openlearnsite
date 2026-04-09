using System;
using System.Data;
using System.Web.UI.WebControls;

public partial class Teacher_studentstats : System.Web.UI.Page
{
    LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();

    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (!IsPostBack)
        {
            Master.Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle() + "学生统计";
            InitDDL();
        }
    }

    private void InitDDL()
    {
        LearnSite.BLL.Room rbll = new LearnSite.BLL.Room();
        
        DataTable dtCurrentClass = rbll.GetCurrentTeachingClass(tcook.Hid);
        
        if (dtCurrentClass == null || dtCurrentClass.Rows.Count == 0)
        {
            PanelNoClass.Visible = true;
            PanelContent.Visible = false;
            return;
        }
        
        PanelNoClass.Visible = false;
        PanelContent.Visible = true;
        
        DataTable dtGrade = rbll.GetGrade(tcook.Hid);
        DDLgrade.DataSource = dtGrade;
        DDLgrade.DataTextField = "Rgrade";
        DDLgrade.DataValueField = "Rgrade";
        DDLgrade.DataBind();
        
        int firstGrade = Convert.ToInt32(dtCurrentClass.Rows[0]["Rgrade"]);
        int firstClass = Convert.ToInt32(dtCurrentClass.Rows[0]["Rclass"]);
        
        foreach (ListItem item in DDLgrade.Items)
        {
            if (item.Value == firstGrade.ToString())
            {
                item.Selected = true;
                break;
            }
        }
        
        BindClassDropDown();
        
        foreach (ListItem item in DDLclass.Items)
        {
            if (item.Value == firstClass.ToString())
            {
                item.Selected = true;
                break;
            }
        }
        
        BindTermDropDown();
        BindCourseDropDown();
        ShowStats();
    }

    private void BindClassDropDown()
    {
        if (DDLgrade.Items.Count > 0)
        {
            int Rgrade = Int32.Parse(DDLgrade.SelectedValue);
            LearnSite.BLL.Room rm = new LearnSite.BLL.Room();
            DDLclass.DataSource = rm.GetLimitClass(Rgrade);
            DDLclass.DataTextField = "Rclass";
            DDLclass.DataValueField = "Rclass";
            DDLclass.DataBind();
        }
    }

    private int GetCurrentTerm()
    {
        int currentMonth = DateTime.Now.Month;
        int currentDay = DateTime.Now.Day;

        if (currentMonth >= 9)
        {
            return 1;
        }
        else if (currentMonth == 1 || (currentMonth == 2 && currentDay <= 10))
        {
            return 1;
        }
        else if (currentMonth == 2 && currentDay > 10)
        {
            return 2;
        }
        else if (currentMonth >= 3 && currentMonth <= 8)
        {
            return 2;
        }
        else
        {
            return 1;
        }
    }

    private void BindTermDropDown()
    {
        DDLterm.Items.Clear();
        string configTerm = LearnSite.Common.XmlHelp.GetTerm();
        int currentTerm = 0;
        if (!string.IsNullOrEmpty(configTerm) && int.TryParse(configTerm, out currentTerm))
        {
        }
        else
        {
            currentTerm = GetCurrentTerm();
        }
        DDLterm.Items.Add(new ListItem("本期(" + currentTerm + "期)", currentTerm.ToString()));
        DDLterm.Items.Add(new ListItem("1期(9月-2月)", "1"));
        DDLterm.Items.Add(new ListItem("2期(2月-8月)", "2"));
        DDLterm.Items.Add(new ListItem("全部", "0"));
        DDLterm.SelectedIndex = 0;
    }

    private void BindCourseDropDown()
    {
        DDLcourse.Items.Clear();
        DDLcourse.Items.Add(new ListItem("全部课程", "0"));
        
        if (DDLgrade.Items.Count > 0)
        {
            int Sgrade = Int32.Parse(DDLgrade.SelectedValue);
            int term = Int32.Parse(DDLterm.SelectedValue);
            
            LearnSite.BLL.Courses courseBll = new LearnSite.BLL.Courses();
            DataTable dt = courseBll.GetCoursesByGradeTerm(Sgrade, term);
            
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string cid = row["Cid"].ToString();
                    string cname = row["Ctitle"].ToString();
                    DDLcourse.Items.Add(new ListItem(cname, cid));
                }
            }
        }
    }

    protected void DDLgrade_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindClassDropDown();
        BindCourseDropDown();
        ShowStats();
    }

    protected void DDLclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowStats();
    }

    protected void DDLterm_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindCourseDropDown();
        ShowStats();
    }

    protected void DDLcourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowStats();
    }

    private void ShowStats()
    {
        if (DDLgrade.Items.Count > 0 && DDLclass.Items.Count > 0)
        {
            int Sgrade = Int32.Parse(DDLgrade.SelectedValue);
            int Sclass = Int32.Parse(DDLclass.SelectedValue);
            int term = Int32.Parse(DDLterm.SelectedValue);
            int cid = Int32.Parse(DDLcourse.SelectedValue);

            LearnSite.BLL.Students studentBll = new LearnSite.BLL.Students();
            DataTable dt = studentBll.GetStudentStats(Sgrade, Sclass, term, cid);

            dt.Columns.Add("RowNum", typeof(int));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["RowNum"] = i + 1;
            }

            GVStats.DataSource = dt;
            GVStats.DataBind();

            int totalStudents = dt.Rows.Count;
            int totalSignin = 0;
            int totalWorks = 0;
            int totalAddCount = 0;
            int totalSubCount = 0;
            double totalScore = 0;
            int scoreCount = 0;

            foreach (DataRow row in dt.Rows)
            {
                totalSignin += Convert.ToInt32(row["SignCount"]);
                totalWorks += Convert.ToInt32(row["WorkCount"]);
                totalAddCount += Convert.ToInt32(row["AddCount"]);
                totalSubCount += Convert.ToInt32(row["SubCount"]);
                
                double stenscore = 0;
                if (double.TryParse(row["Stenscore"].ToString(), out stenscore) && stenscore > 0)
                {
                    totalScore += stenscore;
                    scoreCount++;
                }
            }

            lblTotalStudents.Text = totalStudents.ToString();
            lblTotalSignin.Text = totalSignin.ToString();
            lblTotalWorks.Text = totalWorks.ToString();
            lblTotalAddCount.Text = totalAddCount.ToString();
            lblTotalSubCount.Text = totalSubCount.ToString();
            lblAvgScore.Text = scoreCount > 0 ? (totalScore / scoreCount).ToString("F1") : "0";
        }
    }

    protected void BtnReturn_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/teacher/teachermanage.aspx", false);
    }

    protected void GVStats_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVStats.PageIndex = e.NewPageIndex;
        ShowStats();
    }

    protected void GVStats_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[10].CssClass = "add-score";
            e.Row.Cells[12].CssClass = "add-score";
            e.Row.Cells[11].CssClass = "sub-score";
            e.Row.Cells[13].CssClass = "sub-score";
        }
    }
}
