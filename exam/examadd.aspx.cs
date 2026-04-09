using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using LearnSite.Model;
using LearnSite.BLL;

public partial class exam_examadd : System.Web.UI.Page
{
    protected TeaCook tcook = new TeaCook();

    protected string courseId = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (!tcook.IsExist())
        {
            Response.Redirect("~/index.aspx");
            return;
        }

        if (!IsPostBack)
        {
            BindPapers();
            BindClasses();
            SetDefaultTime();
            // 获取课程ID（如果从学案页面跳转过来）
            if (Request.QueryString["cid"] != null)
            {
                courseId = Request.QueryString["cid"].ToString();
            }
        }
    }

    private void BindPapers()
    {
        var paperBll = new LearnSite.BLL.ExamPaper();
        var papers = paperBll.GetPaperList(tcook.Hid.ToString(), -1); // 获取所有状态的试卷

        ddlPaper.Items.Clear();
        ddlPaper.Items.Add(new ListItem("-- 请选择试卷 --", ""));
        foreach (var paper in papers)
        {
            string statusText = paper.Status == 1 ? "[已发布]" : "[草稿]";
            ddlPaper.Items.Add(new ListItem(string.Format("{0} {1} ({2}题/{3}分)", statusText, paper.PaperName, paper.QuestionCount, paper.TotalScore), paper.PaperId.ToString()));
        }
    }

    private void BindClasses()
    {
        // 获取教师所教班级
        var classBll = new LearnSite.BLL.Classes();
        var classes = classBll.GetClassList(tcook.Hid.ToString());

        cblClasses.Items.Clear();
        foreach (var c in classes)
        {
            cblClasses.Items.Add(new ListItem(c.ClassName, c.ClassId.ToString()));
        }
    }

    private void SetDefaultTime()
    {
        // 默认设置为明天开始，持续2小时
        var now = DateTime.Now;
        var startTime = now.Date.AddDays(1).AddHours(8);
        var endTime = startTime.AddHours(2);

        txtStartTime.Text = startTime.ToString("yyyy-MM-ddTHH:mm");
        txtEndTime.Text = endTime.ToString("yyyy-MM-ddTHH:mm");
        txtPublishTime.Text = now.ToString("yyyy-MM-ddTHH:mm");
    }

    protected void TimeMode_Changed(object sender, EventArgs e)
    {
        pnlFixedTime.Visible = rbFixedTime.Checked;
        pnlValidDays.Visible = rbValidDays.Checked;
    }

    protected void ddlParticipantType_SelectedIndexChanged(object sender, EventArgs e)
    {
        int type = int.Parse(ddlParticipantType.SelectedValue);
        pnlClassSelect.Visible = (type == 1);
        pnlStudentSelect.Visible = (type == 3);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        // 验证
        if (string.IsNullOrEmpty(txtExamName.Text.Trim()))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('请输入考试名称！');", true);
            return;
        }

        if (string.IsNullOrEmpty(ddlPaper.SelectedValue))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('请选择试卷！');", true);
            return;
        }

        int timeMode = rbFixedTime.Checked ? 1 : 2;
        DateTime startTime, endTime;
        int validDays = 0;

        if (timeMode == 1)
        {
            // 固定时间模式
            if (!DateTime.TryParse(txtStartTime.Text, out startTime) || !DateTime.TryParse(txtEndTime.Text, out endTime))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('请正确设置考试时间！');", true);
                return;
            }

            if (startTime >= endTime)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('结束时间必须大于开始时间！');", true);
                return;
            }
        }
        else
        {
            // 有效期模式
            if (!DateTime.TryParse(txtPublishTime.Text, out startTime))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('请正确设置发布时间！');", true);
                return;
            }
            validDays = int.Parse(ddlValidDays.SelectedValue);

            // 根据有效天数计算结束时间
            if (validDays > 0)
            {
                endTime = startTime.AddDays(validDays);
            }
            else
            {
                // 永久有效，设置一个较远的结束时间
                endTime = startTime.AddYears(10);
            }
        }

        // 收集参与者
        string participants = "";
        int participantType = int.Parse(ddlParticipantType.SelectedValue);
        if (participantType == 1)
        {
            var selectedClasses = new List<string>();
            foreach (ListItem item in cblClasses.Items)
            {
                if (item.Selected) selectedClasses.Add(item.Value);
            }
            participants = string.Join(",", selectedClasses);
        }
        else if (participantType == 3)
        {
            participants = txtStudents.Text.Trim();
        }

        // 防作弊设置
        var antiCheat = new AntiCheatConfig
        {
            DisableCopy = chkDisableCopy.Checked,
            DisablePaste = chkDisablePaste.Checked,
            DisableRightClick = true,
            DetectSwitch = chkDetectSwitch.Checked,
            MaxSwitchCount = int.Parse(txtMaxSwitch.Text),
            AutoSubmitOnSwitch = true
        };

        var exam = new LearnSite.Model.Exam
        {
            ExamName = txtExamName.Text.Trim(),
            PaperId = int.Parse(ddlPaper.SelectedValue),
            ExamType = int.Parse(ddlExamType.SelectedValue),
            TimeMode = timeMode,
            StartTime = startTime,
            EndTime = endTime,
            ValidDays = validDays,
            Duration = int.Parse(txtDuration.Text),
            LateMinutes = int.Parse(txtLateMinutes.Text),
            ShowAnswer = chkShowAnswer.Checked ? 1 : 0,
            ShowScore = chkShowScore.Checked ? 1 : 0,
            ShowRank = chkShowRank.Checked ? 1 : 0,
            Password = txtPassword.Text.Trim(),
            ParticipantType = participantType,
            Participants = participants,
            AntiCheat = JsonConvert.SerializeObject(antiCheat),
            CreateBy = tcook.Hid.ToString()
        };

        var examBll = new LearnSite.BLL.Exam();
        int examId = examBll.AddExam(exam);

        if (examId > 0)
        {
            // 如果从学案页面跳转过来，添加到学案导航
            if (!string.IsNullOrEmpty(courseId))
            {
                // 添加到学案导航菜单
                LearnSite.BLL.ListMenu lbll = new LearnSite.BLL.ListMenu();
                LearnSite.Model.ListMenu model = new LearnSite.Model.ListMenu();
                model.Lcid = Int32.Parse(courseId);
                model.Lxid = examId;
                model.Ltype = 39; // 课堂测验类型
                model.Ltitle = exam.ExamName;
                model.Lshow = true;
                model.Lsort = lbll.GetMaxLsort(Int32.Parse(courseId)) + 1; // 获取最大序号并加1
                
                int Lid = lbll.Add(model);
                if (Lid > 0)
                {
                    // 同步序号
                    lbll.Lsortsncy(Int32.Parse(courseId));
                    // 重定向回课程显示页面
                    Response.Redirect("~/teacher/courseshow.aspx?cid=" + courseId);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('考试创建成功，但添加到学案失败，请手动添加！');", true);
                    Response.Redirect("examlist.aspx");
                }
            }
            else
            {
                Response.Redirect("examlist.aspx");
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('创建失败，请重试！');", true);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("examlist.aspx");
    }
}
