using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.IO;
using System.Web.UI.HtmlControls;
using LearnSite.DBUtility;
using LearnSite.Model;
using System.Data.SqlClient; // 添加这行引用
// 移除无效的 LearnSite.Utility 引用

/// <summary>
/// 时间节次类
/// </summary>
public class TimeSlot
{
    private int _slotID;
    private string _slotName;
    private TimeSpan _startTime;
    private TimeSpan _endTime;
    private int _displayOrder;

    public int SlotID
    {
        get { return _slotID; }
        set { _slotID = value; }
    }

    public string SlotName
    {
        get { return _slotName; }
        set { _slotName = value; }
    }

    public TimeSpan StartTime
    {
        get { return _startTime; }
        set { _startTime = value; }
    }

    public TimeSpan EndTime
    {
        get { return _endTime; }
        set { _endTime = value; }
    }

    public int DisplayOrder
    {
        get { return _displayOrder; }
        set { _displayOrder = value; }
    }
}
public partial class kcb_CourseSchedule : System.Web.UI.Page
{
    private CourseScheduleBLL bll = new CourseScheduleBLL();

    protected void Page_Load(object sender, EventArgs e)    
    {
        try
        {
            // 检查教师登录状态
            LearnSite.Common.CookieHelp.JudgeTeacherCookies();
            
            // 检查是否为弹出窗口模式
            string popup = Request.QueryString["popup"];
            if (popup == "1")
            {
                HeaderSection.Visible = false;
            }
            
            if (!IsPostBack)
            {
                // 初始化学年下拉框
                InitializeSchoolYears();
                InitializeTerms();
                InitializeTimeSlots();
                BindTimeSlots();
                InitializeClassDropDownLists();
            }
        }
        catch (Exception ex)
        {
            ShowError("页面加载错误", ex);
        }
    }
    
    private void InitializeTerms()
    {
        ddlTerm.Items.Clear();
        ddlTerm.Items.Add(new ListItem("1期", "1"));
        ddlTerm.Items.Add(new ListItem("2期", "2"));

        // 根据当前日期自动选择期次
        // 1期：9月-2月（下半年到次年2月）
        // 2期：2月-8月（上半年）
        int currentMonth = DateTime.Now.Month;
        if (currentMonth >= 9 || currentMonth <= 2)
        {
            ddlTerm.SelectedValue = "1"; // 9月-2月为1期
        }
        else
        {
            ddlTerm.SelectedValue = "2"; // 3月-8月为2期
        }
    }
    private void InitializeSchoolYears()
    {
        ddlYear.Items.Clear();

        int currentYear = DateTime.Now.Year;
        int currentMonth = DateTime.Now.Month;
        int currentDay = DateTime.Now.Day;

        // 根据当前日期确定学年
        // 1期（9月-次年1月底/2月初）：学年为当前年份（例如2025年9月-2026年2月，学年为2025）
        // 2期（2月中旬-8月10日）：学年为当前年份（例如2026年2月中旬-2026年8月10日，学年为2026）
        int defaultYear;
        if (currentMonth >= 9)
        {
            // 9月-12月：学年为当前年份（例如2025年9月-12月，学年为2025）
            defaultYear = currentYear;
        }
        else if (currentMonth == 1 || (currentMonth == 2 && currentDay <= 10))
        {
            // 1月-2月10日：学年为前一年（例如2026年1月-2月10日，学年为2025）
            defaultYear = currentYear - 1;
        }
        else if (currentMonth == 2 && currentDay > 10)
        {
            // 2月11日及以后：学年为当前年份（例如2026年2月11日，学年为2026）
            defaultYear = currentYear;
        }
        else if (currentMonth >= 3 && currentMonth <= 8)
        {
            // 3月-8月：学年为当前年份（例如2026年3月-8月，学年为2026）
            defaultYear = currentYear;
        }
        else
        {
            // 默认情况
            defaultYear = currentYear;
        }

        // 只从CourseSchedule表获取学年数据
        DataTable dt = new DataTable();
        string sql = "SELECT DISTINCT SchoolYear FROM CourseSchedule ORDER BY SchoolYear DESC";

        try
        {
            dt = DbHelperSQL.Query(sql).Tables[0];
        }
        catch (Exception ex)
        {
            // CourseSchedule表不存在或查询失败，返回空表
            dt = new DataTable();
        }

        bool hasDefaultYear = false;

        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                int year = Convert.ToInt32(row["SchoolYear"]);
                ddlYear.Items.Add(new ListItem(year.ToString(), year.ToString()));
                if (year == defaultYear)
                {
                    hasDefaultYear = true;
                }
            }
        }

        // 如果没有默认学年，添加到列表
        if (!hasDefaultYear)
        {
            ddlYear.Items.Insert(0, new ListItem(defaultYear.ToString(), defaultYear.ToString()));
        }

        // 设置默认选中当前学年
        try
        {
            ddlYear.SelectedValue = defaultYear.ToString();
        }
        catch
        {
            // 如果列表中没有该学年，尝试选择最近的一个学年
            if (dt != null && dt.Rows.Count > 0)
            {
                ddlYear.SelectedIndex = 0; // 选择列表中的第一个（最大的）学年
            }
        }

        // 如果没有数据，添加当前学年和前后两个学年
        if (dt == null || dt.Rows.Count == 0)
        {
            ddlYear.Items.Clear();
            ddlYear.Items.Add(new ListItem((defaultYear - 1).ToString(), (defaultYear - 1).ToString()));
            ddlYear.Items.Add(new ListItem(defaultYear.ToString(), defaultYear.ToString()));
            ddlYear.Items.Add(new ListItem((defaultYear + 1).ToString(), (defaultYear + 1).ToString()));
            ddlYear.SelectedValue = defaultYear.ToString();
        }
    }
    private void InitializeTimeSlots()
    {
        // 检查是否已初始化节次数据
        if (Convert.ToInt32(DbHelperSQL.GetSingle("SELECT COUNT(*) FROM TimeSlots")) == 0)
        {
            // 插入默认节次数据
            List<TimeSlot> defaultSlots = new List<TimeSlot>();
            
            TimeSlot slot1 = new TimeSlot();
            slot1.SlotID = 1;
            slot1.SlotName = "第一节";
            slot1.StartTime = new TimeSpan(8, 0, 0);
            slot1.EndTime = new TimeSpan(8, 40, 0);
            slot1.DisplayOrder = 1;
            defaultSlots.Add(slot1);
            
            TimeSlot slot2 = new TimeSlot();
            slot2.SlotID = 2;
            slot2.SlotName = "第二节";
            slot2.StartTime = new TimeSpan(8, 55, 0);
            slot2.EndTime = new TimeSpan(9, 35, 0);
            slot2.DisplayOrder = 2;
            defaultSlots.Add(slot2);

            foreach (TimeSlot slot in defaultSlots)
            {
                string sql = @"INSERT INTO TimeSlots (SlotID, SlotName, StartTime, EndTime, DisplayOrder) 
                          VALUES (@SlotID, @SlotName, @StartTime, @EndTime, @DisplayOrder)";
                DbHelperSQL.ExecuteSql(sql, 
                    new System.Data.SqlClient.SqlParameter("@SlotID", slot.SlotID),
                    new System.Data.SqlClient.SqlParameter("@SlotName", slot.SlotName),
                    new System.Data.SqlClient.SqlParameter("@StartTime", slot.StartTime),
                    new System.Data.SqlClient.SqlParameter("@EndTime", slot.EndTime),
                    new System.Data.SqlClient.SqlParameter("@DisplayOrder", slot.DisplayOrder));
            }
        }
    }

    private void BindTimeSlots()
    {
        try
        {
            // 从数据库获取时间段数据
            string sql = "SELECT SlotID, SlotName, StartTime, EndTime FROM TimeSlots ORDER BY DisplayOrder";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            rptTimeSlots.DataSource = dt;
            rptTimeSlots.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception("绑定时间段失败: " + ex.Message);
        }
    }
    private void InitializeClassDropDownLists()
    {
        if (rptTimeSlots.Items == null || rptTimeSlots.Items.Count == 0)
        {
            throw new Exception("时间段列表未初始化");
        }

        // 从Room表获取班级数据，按年级班级排序
        string sql = "SELECT DISTINCT Rgrade, Rclass FROM Room WHERE Rgrade IS NOT NULL AND Rclass IS NOT NULL ORDER BY Rgrade, Rclass";
        DataTable dt = DbHelperSQL.Query(sql).Tables[0];
        
        if (dt == null)
        {
            throw new Exception("班级列表为空");
        }

        foreach (RepeaterItem item in rptTimeSlots.Items)
        {
            if (item == null) continue;

            for (int i = 1; i <= 7; i++)
            {
                DropDownList ddl = item.FindControl("ddlClass" + i) as DropDownList;
                if (ddl == null)
                {
                    throw new Exception(string.Format("未找到下拉列表控件: ddlClass{0}", i));
                }

                ddl.Items.Clear();
                ddl.Items.Add(new ListItem("  ", ""));

                foreach (DataRow row in dt.Rows)
                {
                    int rgrade = Convert.ToInt32(row["Rgrade"]);
                    int rclass = Convert.ToInt32(row["Rclass"]);
                    string className = string.Format("{0}.{1}", rgrade, rclass);
                    string classValue = string.Format("{0}-{1}", rgrade, rclass);
                    ddl.Items.Add(new ListItem(className, classValue));
                }
            }
        }

        // 如果有年份，加载课程表数据
        if (!string.IsNullOrEmpty(ddlYear.SelectedValue))
        {
            LoadScheduleData();
        }
    }

    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            LoadScheduleData();
        }
        catch (Exception ex)
        {
            ShowError("加载数据失败", ex);
        }
    }
    
    protected void ddlTerm_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            LoadScheduleData();
        }
        catch (Exception ex)
        {
            ShowError("加载数据失败", ex);
        }
    }

    protected void ddlSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            LoadScheduleData();
        }
        catch (Exception ex)
        {
            ShowError("加载数据失败", ex);
        }
    }

    private void LoadScheduleData()
    {
        if (string.IsNullOrEmpty(ddlYear.SelectedValue) || string.IsNullOrEmpty(ddlTerm.SelectedValue) || string.IsNullOrEmpty(ddlSubject.SelectedValue))
        {
            return;
        }

        int year;
        int term;
        if (!int.TryParse(ddlYear.SelectedValue, out year) || !int.TryParse(ddlTerm.SelectedValue, out term))
        {
            ShowError("无效的学年或学期值", null);
            return;
        }

        string subject = ddlSubject.SelectedValue;
        int teacherID = GetCurrentTeacherID();

        try
        {
            DataTable dt = bll.GetScheduleByYearTermAndSubject(year, term, subject, teacherID);
            if (dt == null) return;

            foreach (DataRow row in dt.Rows)
            {
                int timeSlot = Convert.ToInt32(row["TimeSlot"]);
                int weekDay = Convert.ToInt32(row["WeekDay"]);
                string className = row["ClassName"].ToString();

                if (timeSlot >= 1 && weekDay >= 1 && weekDay <= 7)
                {
                    if (timeSlot - 1 < rptTimeSlots.Items.Count)
                    {
                        RepeaterItem item = rptTimeSlots.Items[timeSlot - 1];
                        if (item != null)
                        {
                            DropDownList ddl = item.FindControl("ddlClass" + weekDay) as DropDownList;
                            if (ddl != null && !string.IsNullOrEmpty(className))
                            {
                                try
                                {
                                    ddl.SelectedValue = className;
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                }
            }

            if (lblMessage.Visible)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "hideMessage", string.Format("setTimeout(function() {{ document.getElementById('{0}').style.display = 'none'; }}, 3000);", lblMessage.ClientID), true);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("加载课程表数据失败: " + ex.Message);
        }
    }

    private int GetCurrentTeacherID()
    {
        LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();
        return tcook.Hid;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            List<CourseScheduleModel> schedules = new List<CourseScheduleModel>();
            int year = Convert.ToInt32(ddlYear.SelectedValue);
            int term = Convert.ToInt32(ddlTerm.SelectedValue);
            int teacherID = GetCurrentTeacherID();

            foreach (RepeaterItem item in rptTimeSlots.Items)
            {
                int timeSlot = item.ItemIndex + 1;
                for (int weekDay = 1; weekDay <= 7; weekDay++)
                {
                    DropDownList ddl = item.FindControl("ddlClass" + weekDay) as DropDownList;
                    if (ddl != null && !string.IsNullOrEmpty(ddl.SelectedValue))
                    {
                        CourseScheduleModel schedule = new CourseScheduleModel();
                        schedule.SchoolYear = year;
                        schedule.Term = term;
                        schedule.WeekDay = weekDay;
                        schedule.TimeSlot = timeSlot;
                        schedule.ClassName = ddl.SelectedValue;
                        schedule.Subject = ddlSubject.SelectedValue;
                        schedule.TeacherID = teacherID;
                        schedules.Add(schedule);
                    }
                }
            }

            bool saveResult = bll.SaveSchedule(year, term, schedules);
            
            if (saveResult)
            {
                lblMessage.Text = "课程表保存成功";
                lblMessage.CssClass = "message success";
            }
            else
            {
                lblMessage.Text = "课程表保存失败";
                lblMessage.CssClass = "message error";
            }
            lblMessage.Visible = true;
            
            ClientScript.RegisterStartupScript(this.GetType(), "hideMessage", string.Format("setTimeout(function() {{ document.getElementById('{0}').style.display = 'none'; }}, 3000);", lblMessage.ClientID), true);
        }
        catch (Exception ex)
        {
            lblMessage.Text = "保存时发生错误: " + ex.Message;
            lblMessage.CssClass = "message error";
            lblMessage.Visible = true;
            
            ClientScript.RegisterStartupScript(this.GetType(), "hideMessage", string.Format("setTimeout(function() {{ document.getElementById('{0}').style.display = 'none'; }}, 3000);", lblMessage.ClientID), true);
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", 
                string.Format("attachment;filename=CourseSchedule_{0}.xls", ddlYear.SelectedValue));
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            
            rptTimeSlots.RenderControl(hw);
            
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
        catch (Exception ex)
        {
            ShowError("导出失败", ex);
        }
    }

    /*protected string GetTimeSlotText(int slot)
    {
        switch (slot)
        {
            case 1: return "1 8:20-9:00";
            case 2: return "2 9:15-9:55";
            case 3: return "3 10:35-11:15";
            case 4: return "4 11:30-12:10";
            case 5: return "5 13:50-14:30";
            case 6: return "6 14:45-15:25";
            default: return "";
        }
    }*/

    private void ShowError(string message, Exception ex)
    {
        if (ex != null)
        {
            message += ": " + ex.Message;
        }
        lblMessage.Text = message;
        lblMessage.CssClass = "message error";
        lblMessage.Visible = true;
        
        // 所有消息3秒后自动消失
        ClientScript.RegisterStartupScript(this.GetType(), "hideMessage", string.Format("setTimeout(function() {{ document.getElementById('{0}').style.display = 'none'; }}, 3000);", lblMessage.ClientID), true);
    }

    private void ShowSuccess(string message)
    {
        lblMessage.Text = message;
        lblMessage.CssClass = "message success";
        lblMessage.Visible = true;
        
        // 为成功消息添加3秒后自动消失的功能
        ClientScript.RegisterStartupScript(this.GetType(), "hideMessage", string.Format("setTimeout(function() {{ document.getElementById('{0}').style.display = 'none'; }}, 3000);", lblMessage.ClientID), true);
    }

    protected string GetTimeSlotText(int slotIndex)
    {
        // 直接从数据库获取节次信息
        string sql = "SELECT StartTime, EndTime FROM TimeSlots WHERE SlotID = @SlotID ORDER BY DisplayOrder";
        DataTable dt = DbHelperSQL.Query(sql, new SqlParameter("@SlotID", slotIndex)).Tables[0];
        
        if (dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];
            TimeSpan startTime = (TimeSpan)row["StartTime"];
            TimeSpan endTime = (TimeSpan)row["EndTime"];
            return string.Format("{0:HH\\:mm}-{1:HH\\:mm}", 
                DateTime.Today.Add(startTime),
                DateTime.Today.Add(endTime));
        }
        
        // 默认值
        switch (slotIndex)
        {
            case 1: return "08:20-09:00";
            case 2: return "09:15-09:55";
            case 3: return "10:35-11:15";
            case 4: return "11:30-12:10";
            case 5: return "13:50-14:30";
            case 6: return "14:45-15:25";
            default: return string.Format("第{0}节", slotIndex);
        }
    }

    
    protected void btnCopySchedule_Click(object sender, EventArgs e)
    {
        try
        {
            int currentYear = Convert.ToInt32(ddlYear.SelectedValue);
            int currentTerm = Convert.ToInt32(ddlTerm.SelectedValue);
            int teacherID = GetCurrentTeacherID();
            
            int targetYear = currentYear;
            int targetTerm = 1;
            
            if (currentTerm == 1)
            {
                targetYear++;
                targetTerm = 2;
            }
            else
            {
                targetTerm = 1;
            }
            
            DataTable dtSource = bll.GetScheduleByYearTermAndSubject(currentYear, currentTerm, ddlSubject.SelectedValue, teacherID);
            
            if (dtSource == null || dtSource.Rows.Count == 0)
            {
                lblMessage.Text = string.Format("{0}年{1}期没有课程表数据可复制", currentYear, currentTerm);
                lblMessage.CssClass = "message error";
                lblMessage.Visible = true;
                
                ClientScript.RegisterStartupScript(this.GetType(), "hideMessage", string.Format("setTimeout(function() {{ document.getElementById('{0}').style.display = 'none'; }}, 3000);", lblMessage.ClientID), true);
                return;
            }
            
            List<CourseScheduleModel> schedules = new List<CourseScheduleModel>();
            
            foreach (DataRow row in dtSource.Rows)
            {
                CourseScheduleModel schedule = new CourseScheduleModel();
                schedule.SchoolYear = targetYear;
                schedule.Term = targetTerm;
                schedule.WeekDay = Convert.ToInt32(row["WeekDay"]);
                schedule.TimeSlot = Convert.ToInt32(row["TimeSlot"]);
                schedule.ClassName = row["ClassName"].ToString();
                schedule.Subject = row["Subject"].ToString();
                schedule.TeacherID = teacherID;
                schedules.Add(schedule);
            }
            
            bool success = bll.SaveSchedule(targetYear, targetTerm, schedules);
            
            if (success)
            {
                InitializeSchoolYears();
                
                ddlYear.SelectedValue = targetYear.ToString();
                ddlTerm.SelectedValue = targetTerm.ToString();
                
                LoadScheduleData();
                
                lblMessage.Text = string.Format("成功复制{0}年{1}期的课程表到{2}年{3}期", 
                    currentYear, currentTerm, targetYear, targetTerm);
                lblMessage.CssClass = "message success";
            }
            else
            {
                lblMessage.Text = "复制课程表失败";
                lblMessage.CssClass = "message error";
            }
            lblMessage.Visible = true;
            
            ClientScript.RegisterStartupScript(this.GetType(), "hideMessage", string.Format("setTimeout(function() {{ document.getElementById('{0}').style.display = 'none'; }}, 3000);", lblMessage.ClientID), true);
        }
        catch (Exception ex)
        {
            lblMessage.Text = "操作出错：" + ex.Message;
            lblMessage.CssClass = "message error";
            lblMessage.Visible = true;
            
            ClientScript.RegisterStartupScript(this.GetType(), "hideMessage", string.Format("setTimeout(function() {{ document.getElementById('{0}').style.display = 'none'; }}, 3000);", lblMessage.ClientID), true);
        }
    }

    protected void btnReturn_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/teacher/teachermanage.aspx", false);
    }
}

