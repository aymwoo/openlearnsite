using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using LearnSite.Model;
using LearnSite.BLL;

public partial class exam_examresult : System.Web.UI.Page
{
    protected TeaCook tcook = new TeaCook();
    protected int ExamId { get; set; }
    protected decimal TotalScoreMax = 100;
    protected decimal PassScore = 60;

    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (!tcook.IsExist())
        {
            Response.Redirect("~/index.aspx");
            return;
        }

        int examId;
        // 尝试从eid参数获取考试ID，如果没有则尝试从id参数获取
        if (!int.TryParse(Request.QueryString["eid"], out examId) && !int.TryParse(Request.QueryString["id"], out examId))
        {
            Response.Redirect("examlist.aspx");
            return;
        }

        ExamId = examId;

        if (!IsPostBack)
        {
            LoadExamInfo();
            BindClasses();
            LoadStatistics();
            BindResults();
        }
    }

    private void LoadExamInfo()
    {
        var examBll = new LearnSite.BLL.Exam();
        var exam = examBll.GetExamById(ExamId);
        if (exam == null)
        {
            Response.Redirect("examlist.aspx");
            return;
        }

        ltlExamName.Text = exam.ExamName;

        // 获取试卷总分
        var paperBll = new LearnSite.BLL.ExamPaper();
        var paper = paperBll.GetPaperById(exam.PaperId);
        if (paper != null)
        {
            TotalScoreMax = paper.TotalScore;
            PassScore = paper.PassScore;
        }
    }

    private void BindClasses()
    {
        var resultBll = new LearnSite.DAL.ExamResult();
        var results = resultBll.GetResultList(ExamId);

        var classes = results
            .Where(r => r.ClassId.HasValue)
            .Select(r => new { r.ClassId, r.ClassName })
            .Distinct()
            .ToList();

        foreach (var c in classes)
        {
            ddlClass.Items.Add(new ListItem(c.ClassName, c.ClassId.Value.ToString()));
        }
    }

    private void LoadStatistics()
    {
        var resultBll = new LearnSite.DAL.ExamResult();
        var results = resultBll.GetResultList(ExamId);

        int total = results.Count;
        decimal avgScore = total > 0 ? Math.Round(results.Average(r => r.TotalScore), 1) : 0;
        decimal maxScore = total > 0 ? results.Max(r => r.TotalScore) : 0;
        decimal minScore = total > 0 ? results.Min(r => r.TotalScore) : 0;
        int passCount = results.Count(r => r.TotalScore >= PassScore);
        decimal passRate = total > 0 ? Math.Round((decimal)passCount / total * 100, 1) : 0;

        ltlTotalCount.Text = total.ToString();
        ltlAvgScore.Text = avgScore.ToString();
        ltlMaxScore.Text = maxScore.ToString();
        ltlMinScore.Text = minScore.ToString();
        ltlPassRate.Text = passRate.ToString();

        // 绑定分数段分布
        var distribution = new List<dynamic>
        {
            new { Range = "90-100", Count = results.Count(r => r.TotalScore >= 90) },
            new { Range = "80-89", Count = results.Count(r => r.TotalScore >= 80 && r.TotalScore < 90) },
            new { Range = "70-79", Count = results.Count(r => r.TotalScore >= 70 && r.TotalScore < 80) },
            new { Range = "60-69", Count = results.Count(r => r.TotalScore >= 60 && r.TotalScore < 70) },
            new { Range = "<60", Count = results.Count(r => r.TotalScore < 60) }
        };

        rptDistribution.DataSource = distribution;
        rptDistribution.DataBind();
    }

    private void BindResults()
    {
        var resultBll = new LearnSite.DAL.ExamResult();
        var results = resultBll.GetResultList(ExamId);

        // 班级过滤
        if (!string.IsNullOrEmpty(ddlClass.SelectedValue))
        {
            int classId = int.Parse(ddlClass.SelectedValue);
            results = results.Where(r => r.ClassId == classId).ToList();
        }

        // 分数段过滤
        if (!string.IsNullOrEmpty(ddlScoreRange.SelectedValue))
        {
            int minScore = int.Parse(ddlScoreRange.SelectedValue);
            if (minScore == 90)
                results = results.Where(r => r.TotalScore >= 90).ToList();
            else if (minScore == 80)
                results = results.Where(r => r.TotalScore >= 80).ToList();
            else if (minScore == 60)
                results = results.Where(r => r.TotalScore >= 60).ToList();
            else if (minScore == 0)
                results = results.Where(r => r.TotalScore < 60).ToList();
        }

        // 关键字过滤
        string keyword = txtKeyword.Text.Trim();
        if (!string.IsNullOrEmpty(keyword))
        {
            results = results.Where(r => 
                r.StudentId.Contains(keyword) || 
                r.StudentName.Contains(keyword)).ToList();
        }

        // 计算班级排名
        int rank = 1;
        foreach (var r in results)
        {
            r.RankInClass = rank++;
        }

        rptResults.DataSource = results;
        rptResults.DataBind();
    }

    protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindResults();
    }

    protected void ddlScoreRange_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindResults();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindResults();
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        // 导出Excel
        var resultBll = new LearnSite.DAL.ExamResult();
        var results = resultBll.GetResultList(ExamId);

        var dt = new DataTable();
        dt.Columns.Add("排名");
        dt.Columns.Add("学号");
        dt.Columns.Add("姓名");
        dt.Columns.Add("班级");
        dt.Columns.Add("成绩");
        dt.Columns.Add("客观题得分");
        dt.Columns.Add("主观题得分");
        dt.Columns.Add("用时");
        dt.Columns.Add("提交时间");

        int rank = 1;
        foreach (var r in results)
        {
            dt.Rows.Add(
                rank++,
                r.StudentId,
                r.StudentName,
                r.ClassName,
                r.TotalScore,
                r.ObjectiveScore,
                r.SubjectiveScore,
                FormatDuration(r.Duration),
                r.SubmitTime != null ? r.SubmitTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : null
            );
        }

        ExportToExcel(dt, string.Format("成绩_{0:yyyyMMdd_HHmmss}.xls", DateTime.Now));
    }

    private void ExportToExcel(DataTable dt, string fileName)
    {
        Response.Clear();
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
        Response.Charset = "UTF-8";
        Response.ContentEncoding = System.Text.Encoding.UTF8;

        string html = "<html><head><meta charset='UTF-8'></head><body><table border='1'>";
        html += "<tr>";
        foreach (DataColumn col in dt.Columns)
        {
            html += string.Format("<th>{0}</th>", col.ColumnName);
        }
        html += "</tr>";

        foreach (DataRow row in dt.Rows)
        {
            html += "<tr>";
            foreach (var item in row.ItemArray)
            {
                html += string.Format("<td>{0}</td>", item);
            }
            html += "</tr>";
        }

        html += "</table></body></html>";
        Response.Write(html);
        Response.End();
    }

    protected string GetBarHeight(object count)
    {
        int c = Convert.ToInt32(count);
        int max = Math.Max(1, c);
        return Math.Min(120, c * 10).ToString();
    }

    protected string FormatDuration(object seconds)
    {
        if (seconds == null) return "-";
        int secs = Convert.ToInt32(seconds);
        int mins = secs / 60;
        int s = secs % 60;
        return string.Format("{0}分{1}秒", mins, s);
    }
}
