using System;
using System.Data;
using System.Web.UI.WebControls;

public partial class Teacher_seatmanage : System.Web.UI.Page
{
    LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();

    /// <summary>
    /// 检测教师登录状态
    /// </summary>
    /// <returns>是否已登录</returns>
    private bool CheckTeacherLogin()
    {
        // 使用CookieHelp中的标准方法检测登录状态
        if (!LearnSite.Common.CookieHelp.IsTeacherLogin())
        {
            LearnSite.Common.CookieHelp.JudgeTeacherCookies();
            return false;
        }

        // 再次验证TeaCook对象是否有效
        if (tcook == null || !tcook.IsExist())
        {
            LearnSite.Common.CookieHelp.JudgeTeacherCookies();
            return false;
        }

        return true;
    }

    /// <summary>
    /// 获取当前教师的Session键名前缀
    /// </summary>
    /// <returns>Session键名前缀（教师ID）</returns>
    private string GetSessionPrefix()
    {
        if (tcook == null || tcook.Hid == 0)
        {
            return "";
        }
        return tcook.Hid.ToString();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // 检测教师登录状态
        if (!CheckTeacherLogin())
        {
            return;
        }

        if (!IsPostBack)
        {
            Master.Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle() + "座位管理";
            InitDDL();

            // 获取Session前缀
            string sessionPrefix = GetSessionPrefix();
            if (string.IsNullOrEmpty(sessionPrefix))
            {
                return;
            }

            // 优先从Session中获取年级和班级（来自start页面）
            if (Session[sessionPrefix + "grade"] != null && Session[sessionPrefix + "class"] != null)
            {
                string sessionGrade = Session[sessionPrefix + "grade"].ToString();
                string sessionClass = Session[sessionPrefix + "class"].ToString();

                // 如果Session中的年级和班级在下拉列表中存在，则自动选中并显示
                if (DDLgrade.Items.FindByValue(sessionGrade) != null)
                {
                    DDLgrade.SelectedValue = sessionGrade;

                    // 重新加载班级
                    int Sgrade = Int32.Parse(DDLgrade.SelectedValue);
                    LearnSite.BLL.Room rm = new LearnSite.BLL.Room();
                    DDLclass.DataSource = rm.GetLimitClass(Sgrade);
                    DDLclass.DataTextField = "Rclass";
                    DDLclass.DataValueField = "Rclass";
                    DDLclass.DataBind();

                    if (DDLclass.Items.FindByValue(sessionClass) != null)
                    {
                        DDLclass.SelectedValue = sessionClass;
                        ShowStudents();
                    }
                }
            }
            // 其次，如果从URL参数传入年级和班级，则自动选中
            else if (Request.QueryString["grade"] != null && Request.QueryString["class"] != null)
            {
                string grade = Request.QueryString["grade"];
                string classValue = Request.QueryString["class"];

                if (DDLgrade.Items.FindByValue(grade) != null)
                {
                    DDLgrade.SelectedValue = grade;
                    // 重新加载班级
                    int Sgrade = Int32.Parse(DDLgrade.SelectedValue);
                    LearnSite.BLL.Room rm = new LearnSite.BLL.Room();
                    DDLclass.DataSource = rm.GetLimitClass(Sgrade);
                    DDLclass.DataTextField = "Rclass";
                    DDLclass.DataValueField = "Rclass";
                    DDLclass.DataBind();

                    if (DDLclass.Items.FindByValue(classValue) != null)
                    {
                        DDLclass.SelectedValue = classValue;
                        ShowStudents();
                    }
                }
            }
        }
    }

    private void InitDDL()
    {
        // 初始化年级
        LearnSite.BLL.Room rbll = new LearnSite.BLL.Room();
        DDLgrade.DataSource = rbll.GetGrade(tcook.Hid);
        DDLgrade.DataTextField = "Rgrade";
        DDLgrade.DataValueField = "Rgrade";
        DDLgrade.DataBind();
        string Hid = tcook.Hid.ToString();
        if (Session[Hid + "grade"] != null)
        {
            DDLgrade.SelectedValue = Session[Hid + "grade"].ToString();
        }

        // 初始化班级
        int Rgrade = Int32.Parse(DDLgrade.SelectedValue);
        LearnSite.BLL.Room rm = new LearnSite.BLL.Room();
        DDLclass.DataSource = rm.GetLimitClass(Rgrade);
        DDLclass.DataTextField = "Rclass";
        DDLclass.DataValueField = "Rclass";
        DDLclass.DataBind();
        if (Session[Hid + "class"] != null)
        {
            DDLclass.SelectedValue = Session[Hid + "class"].ToString();
        }

        // 初始化机房
        LearnSite.BLL.House hbll = new LearnSite.BLL.House();
        DDLhouse.DataSource = hbll.GetListHouse();
        DDLhouse.DataTextField = "Hname";
        DDLhouse.DataValueField = "Hid";
        DDLhouse.DataBind();
    }

    protected void DDLgrade_SelectedIndexChanged(object sender, EventArgs e)
    {
        int Sgrade = Int32.Parse(DDLgrade.SelectedValue);
        LearnSite.BLL.Students sbll = new LearnSite.BLL.Students();
        DDLclass.DataSource = sbll.GetClassList(Sgrade);
        DDLclass.DataTextField = "Class";
        DDLclass.DataValueField = "Class";
        DDLclass.DataBind();

        ShowStudents();
    }

    protected void DDLclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowStudents();
    }

    protected void DDLhouse_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowStudents();
    }

    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        string keyword = TBsearchKeyword.Text.Trim();

        if (string.IsNullOrEmpty(keyword))
        {
            lblSearchMsg.Text = "请输入学号或座位号！";
            return;
        }

        if (DDLgrade.Items.Count == 0 || DDLclass.Items.Count == 0)
        {
            lblSearchMsg.Text = "请先选择年级和班级！";
            return;
        }

        int Sgrade = Int32.Parse(DDLgrade.SelectedValue);
        int Sclass = Int32.Parse(DDLclass.SelectedValue);

        LearnSite.BLL.Students sbll = new LearnSite.BLL.Students();
        DataSet ds = sbll.GetListStudents(Sgrade, Sclass, "Snum");

        DataTable dt = ds.Tables[0];
        DataTable filteredDt = dt.Clone();

        bool searchBySnum = RBSearchBySnum.Checked;

        foreach (DataRow row in dt.Rows)
        {
            bool match = false;

            if (searchBySnum)
            {
                // 按学号查找
                match = row["Snum"].ToString().Equals(keyword, StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                // 按座位号查找
                string seat = row["Sseat"].ToString();
                match = seat.Equals(keyword, StringComparison.OrdinalIgnoreCase);
            }

            if (match)
            {
                filteredDt.ImportRow(row);
            }
        }

        // 添加序号列
        filteredDt.Columns.Add("RowNum", typeof(int));
        for (int i = 0; i < filteredDt.Rows.Count; i++)
        {
            filteredDt.Rows[i]["RowNum"] = i + 1;
        }

        if (filteredDt.Rows.Count > 0)
        {
            GVstudents.DataSource = filteredDt;
            GVstudents.DataBind();
            lblSearchMsg.Text = string.Format("找到 {0} 条匹配记录", filteredDt.Rows.Count);
        }
        else
        {
            lblSearchMsg.Text = "未找到匹配的学生";
            // 清空表格
            GVstudents.DataSource = null;
            GVstudents.DataBind();
        }
    }

    protected void BtnClearSearch_Click(object sender, EventArgs e)
    {
        TBsearchKeyword.Text = "";
        lblSearchMsg.Text = "";
        ShowStudents();
    }

    private void ShowStudents()
    {
        if (DDLgrade.Items.Count > 0 && DDLclass.Items.Count > 0)
        {
            int Sgrade = Int32.Parse(DDLgrade.SelectedValue);
            int Sclass = Int32.Parse(DDLclass.SelectedValue);

            LearnSite.BLL.Students sbll = new LearnSite.BLL.Students();
            DataSet ds = sbll.GetListStudents(Sgrade, Sclass, "Snum");

            // 添加序号列
            DataTable dt = ds.Tables[0];
            dt.Columns.Add("RowNum", typeof(int));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["RowNum"] = i + 1;
            }

            GVstudents.DataSource = dt;
            GVstudents.DataBind();
        }
    }

    protected void BtnAssignBySnum_Click(object sender, EventArgs e)
    {
        if (DDLgrade.Items.Count > 0 && DDLclass.Items.Count > 0 && DDLhouse.Items.Count > 0)
        {
            int Sgrade = Int32.Parse(DDLgrade.SelectedValue);
            int Sclass = Int32.Parse(DDLclass.SelectedValue);
            int HouseId = Int32.Parse(DDLhouse.SelectedValue);

            LearnSite.BLL.Students sbll = new LearnSite.BLL.Students();
            int assignedCount = sbll.AssignSeatsBySnum(Sgrade, Sclass, HouseId);

            lblBatchMsg.Text = string.Format("成功为 {0} 位学生分配了座位！", assignedCount);
            lblSingleMsg.Text = "";

            ShowStudents();
        }
    }

    protected void BtnImportFromSignin_Click(object sender, EventArgs e)
    {
        if (DDLgrade.Items.Count > 0 && DDLclass.Items.Count > 0)
        {
            int Sgrade = Int32.Parse(DDLgrade.SelectedValue);
            int Sclass = Int32.Parse(DDLclass.SelectedValue);

            LearnSite.BLL.Students sbll = new LearnSite.BLL.Students();
            int importCount = sbll.ImportSeatsFromSignin(Sgrade, Sclass);

            lblBatchMsg.Text = string.Format("从登录记录导入成功！为 {0} 位学生设置了固定座位。", importCount);
            lblSingleMsg.Text = "";

            ShowStudents();
        }
    }

    protected void BtnClearAll_Click(object sender, EventArgs e)
    {
        if (DDLgrade.Items.Count > 0 && DDLclass.Items.Count > 0)
        {
            int Sgrade = Int32.Parse(DDLgrade.SelectedValue);
            int Sclass = Int32.Parse(DDLclass.SelectedValue);

            LearnSite.BLL.Students sbll = new LearnSite.BLL.Students();
            sbll.ClearAllSeats(Sgrade, Sclass);

            lblBatchMsg.Text = "已清空所有学生的座位绑定！";
            lblSingleMsg.Text = "";

            ShowStudents();
        }
    }

    protected void BtnAssignSeat_Click(object sender, EventArgs e)
    {
        string snum = TBsnum.Text.Trim();
        string seatNumStr = TBseatNum.Text.Trim();

        if (string.IsNullOrEmpty(snum) || string.IsNullOrEmpty(seatNumStr))
        {
            lblSingleMsg.Text = "请输入学号和座位号！";
            return;
        }

        int seatNum;
        if (!int.TryParse(seatNumStr, out seatNum))
        {
            lblSingleMsg.Text = "座位号必须是数字！";
            return;
        }

        if (DDLhouse.Items.Count == 0)
        {
            lblSingleMsg.Text = "请先选择机房！";
            return;
        }

        int HouseId = Int32.Parse(DDLhouse.SelectedValue);

        LearnSite.BLL.Students sbll = new LearnSite.BLL.Students();
        bool success = sbll.AssignSeatToStudent(snum, HouseId, seatNum);

        if (success)
        {
            lblSingleMsg.Text = "座位分配成功！";
            lblBatchMsg.Text = "";
            ShowStudents();
        }
        else
        {
            lblSingleMsg.Text = "分配失败：该机房中没有该座位号！";
        }
    }

    protected void BtnClearSeat_Click(object sender, EventArgs e)
    {
        string snum = TBsnum.Text.Trim();

        if (string.IsNullOrEmpty(snum))
        {
            lblSingleMsg.Text = "请输入学号！";
            return;
        }

        LearnSite.BLL.Students sbll = new LearnSite.BLL.Students();
        sbll.ClearStudentSeat(snum);

        lblSingleMsg.Text = "已解除该学生的座位绑定！";
        lblBatchMsg.Text = "";
        ShowStudents();
    }

    protected void BtnReturn_Click(object sender, EventArgs e)
    {
        // 返回教师管理首页
        Response.Redirect("~/teacher/teachermanage.aspx", false);
    }

    protected void BtnTempAssignSeat_Click(object sender, EventArgs e)
    {
        string snum = TBsnum.Text.Trim();
        string seatNumStr = TBseatNum.Text.Trim();

        if (string.IsNullOrEmpty(snum) || string.IsNullOrEmpty(seatNumStr))
        {
            lblSingleMsg.Text = "请输入学号和座位号！";
            return;
        }

        int seatNum;
        if (!int.TryParse(seatNumStr, out seatNum))
        {
            lblSingleMsg.Text = "座位号必须是数字！";
            return;
        }

        if (DDLhouse.Items.Count == 0)
        {
            lblSingleMsg.Text = "请先选择机房！";
            return;
        }

        int HouseId = Int32.Parse(DDLhouse.SelectedValue);

        // 获取座位对应的IP
        LearnSite.BLL.House hbll = new LearnSite.BLL.House();
        string ip = hbll.GetIpBySeat(HouseId, seatNum);

        if (string.IsNullOrEmpty(ip))
        {
            lblSingleMsg.Text = "该机房中没有该座位号！";
            return;
        }

        LearnSite.BLL.Students sbll = new LearnSite.BLL.Students();
        // 默认45分钟有效（一节课）
        bool success = sbll.TempAssignSeat(snum, ip, seatNum.ToString(), 45);

        if (success)
        {
            lblSingleMsg.Text = "临时座位分配成功！下节课将自动恢复原座位。";
            lblBatchMsg.Text = "";
            ShowStudents();
        }
        else
        {
            lblSingleMsg.Text = "临时座位分配失败！";
        }
    }

    protected void BtnRowTempSeat_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        string snum = btn.CommandArgument;

        if (DDLhouse.Items.Count == 0)
        {
            lblSingleMsg.Text = "请先选择机房！";
            return;
        }

        int HouseId = Int32.Parse(DDLhouse.SelectedValue);

        LearnSite.BLL.Students sbll = new LearnSite.BLL.Students();
        // 清除该学生原有的临时座位
        sbll.ClearTempSeat(snum);

        // 临时座位不指定具体IP，允许学生在任意空位登录
        // 默认45分钟有效（一节课）
        bool success = sbll.TempAssignSeat(snum, "", "任意", 45);

        if (success)
        {
            lblSingleMsg.Text = "已允许学生在任意空位临时登录！下节课将自动恢复原座位。";
            lblBatchMsg.Text = "";
            ShowStudents();
        }
        else
        {
            lblSingleMsg.Text = "临时座位设置失败！";
        }
    }

    protected void GVstudents_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVstudents.PageIndex = e.NewPageIndex;
        ShowStudents();
    }
}
