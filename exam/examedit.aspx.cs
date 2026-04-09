using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using LearnSite.Model;
using LearnSite.BLL;

public partial class exam_examedit : System.Web.UI.Page
{
    protected TeaCook tcook = new TeaCook();
    protected int ExamId { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (!tcook.IsExist())
        {
            Response.Redirect("~/index.aspx");
            return;
        }

        int examId;
        if (!int.TryParse(Request.QueryString["id"], out examId))
        {
            Response.Redirect("examlist.aspx");
            return;
        }

        ExamId = examId;

        if (!IsPostBack)
        {
            BindPapers();
            BindClasses();
            LoadExamData();
        }
    }

    private void BindPapers()
    {
        var paperBll = new LearnSite.BLL.ExamPaper();
        var papers = paperBll.GetPaperList(tcook.Hid.ToString(), -1);

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
        var classBll = new LearnSite.BLL.Classes();
        var classes = classBll.GetClassList(tcook.Hid.ToString());

        cblClasses.Items.Clear();
        foreach (var c in classes)
        {
            cblClasses.Items.Add(new ListItem(c.ClassName, c.ClassId.ToString()));
        }
    }

    private void LoadExamData()
    {
        var examBll = new LearnSite.BLL.Exam();
        var exam = examBll.GetExamById(ExamId);

        if (exam == null)
        {
            Response.Redirect("examlist.aspx");
            return;
        }

        if (exam.Status > 0)
        {
            pnlInfo.Visible = true;
            ltlInfo.Text = "此考试已发布，部分设置不可修改。";
        }

        txtExamName.Text = exam.ExamName;
        ddlPaper.SelectedValue = exam.PaperId.ToString();
        ddlExamType.SelectedValue = exam.ExamType.ToString();

        rbFixedTime.Checked = exam.TimeMode == 1;
        rbValidDays.Checked = exam.TimeMode == 2;
        pnlFixedTime.Visible = exam.TimeMode == 1;
        pnlValidDays.Visible = exam.TimeMode == 2;

        if (exam.TimeMode == 1)
        {
            txtStartTime.Text = exam.StartTime.ToString("yyyy-MM-ddTHH:mm");
            txtEndTime.Text = exam.EndTime.ToString("yyyy-MM-ddTHH:mm");
        }
        else
        {
            txtPublishTime.Text = exam.StartTime.ToString("yyyy-MM-ddTHH:mm");
            ddlValidDays.SelectedValue = exam.ValidDays.ToString();
        }

        txtDuration.Text = exam.Duration.ToString();
        txtLateMinutes.Text = exam.LateMinutes.ToString();

        ddlParticipantType.SelectedValue = exam.ParticipantType.ToString();
        pnlClassSelect.Visible = exam.ParticipantType == 1;
        pnlStudentSelect.Visible = exam.ParticipantType == 3;

        if (exam.ParticipantType == 1 && !string.IsNullOrEmpty(exam.Participants))
        {
            var selectedClasses = exam.Participants.Split(',');
            foreach (ListItem item in cblClasses.Items)
            {
                item.Selected = Array.IndexOf(selectedClasses, item.Value) >= 0;
            }
        }
        else if (exam.ParticipantType == 3)
        {
            txtStudents.Text = exam.Participants;
        }

        txtPassword.Text = exam.Password;
        chkShowAnswer.Checked = exam.ShowAnswer == 1;
        chkShowScore.Checked = exam.ShowScore == 1;
        chkShowRank.Checked = exam.ShowRank == 1;

        if (!string.IsNullOrEmpty(exam.AntiCheat))
        {
            try
            {
                var antiCheat = JsonConvert.DeserializeObject<AntiCheatConfig>(exam.AntiCheat);
                if (antiCheat != null)
                {
                    chkDisableCopy.Checked = antiCheat.DisableCopy;
                    chkDisablePaste.Checked = antiCheat.DisablePaste;
                    chkDetectSwitch.Checked = antiCheat.DetectSwitch;
                    txtMaxSwitch.Text = antiCheat.MaxSwitchCount.ToString();
                }
            }
            catch
            {
            }
        }

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
            if (!DateTime.TryParse(txtPublishTime.Text, out startTime))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('请正确设置发布时间！');", true);
                return;
            }
            validDays = int.Parse(ddlValidDays.SelectedValue);

            if (validDays > 0)
            {
                endTime = startTime.AddDays(validDays);
            }
            else
            {
                endTime = startTime.AddYears(10);
            }
        }

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

        var antiCheat = new AntiCheatConfig
        {
            DisableCopy = chkDisableCopy.Checked,
            DisablePaste = chkDisablePaste.Checked,
            DisableRightClick = true,
            DetectSwitch = chkDetectSwitch.Checked,
            MaxSwitchCount = int.Parse(txtMaxSwitch.Text),
            AutoSubmitOnSwitch = true
        };

        var examBll = new LearnSite.BLL.Exam();
        var exam = examBll.GetExamById(ExamId);

        if (exam == null)
        {
            Response.Redirect("examlist.aspx");
            return;
        }

        exam.ExamName = txtExamName.Text.Trim();
        exam.PaperId = int.Parse(ddlPaper.SelectedValue);
        exam.ExamType = int.Parse(ddlExamType.SelectedValue);
        exam.TimeMode = timeMode;
        exam.StartTime = startTime;
        exam.EndTime = endTime;
        exam.ValidDays = validDays;
        exam.Duration = int.Parse(txtDuration.Text);
        exam.LateMinutes = int.Parse(txtLateMinutes.Text);
        exam.ShowAnswer = chkShowAnswer.Checked ? 1 : 0;
        exam.ShowScore = chkShowScore.Checked ? 1 : 0;
        exam.ShowRank = chkShowRank.Checked ? 1 : 0;
        exam.Password = txtPassword.Text.Trim();
        exam.ParticipantType = participantType;
        exam.Participants = participants;
        exam.AntiCheat = JsonConvert.SerializeObject(antiCheat);

        bool result = examBll.UpdateExam(exam);

        if (result)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('保存成功！');window.location.href='examlist.aspx';", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('保存失败，请重试！');", true);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("examlist.aspx");
    }
}
