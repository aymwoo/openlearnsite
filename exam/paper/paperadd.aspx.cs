using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using LearnSite.Model;
using LearnSite.BLL;

public partial class exam_paper_paperadd : System.Web.UI.Page
{
    protected TeaCook tcook = new TeaCook();
    private List<PaperQuestionItem> SelectedQuestions
    {
        get
        {
            if (Session["SelectedQuestions"] == null)
            {
                Session["SelectedQuestions"] = new List<PaperQuestionItem>();
            }
            var result = Session["SelectedQuestions"] as List<PaperQuestionItem>;
            if (result == null)
            {
                result = new List<PaperQuestionItem>();
                Session["SelectedQuestions"] = result;
            }
            return result;
        }
        set
        {
            Session["SelectedQuestions"] = value;
        }
    }

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
            // 检查是否从题库选择返回
            if (Request.QueryString["qids"] != null)
            {
                AddSelectedQuestions(Request.QueryString["qids"]);
            }
            BindQuestionList();
        }
    }

    private void AddSelectedQuestions(string qids)
    {
        if (string.IsNullOrEmpty(qids)) return;

        string[] idArray = qids.Split(',');
        foreach (string idStr in idArray)
        {
            long qid;
            if (long.TryParse(idStr, out qid))
            {
                // 检查是否已存在
                if (!SelectedQuestions.Exists(q => q.QuestionId == qid))
                {
                    SelectedQuestions.Add(new PaperQuestionItem
                    {
                        QuestionId = qid,
                        Score = 5,
                        SortOrder = SelectedQuestions.Count + 1
                    });
                }
            }
        }
    }

    private void BindQuestionList()
    {
        if (SelectedQuestions.Count == 0)
        {
            pnlNoQuestion.Visible = true;
            lblQuestionCount.Text = "0";
            lblScoreSum.Text = "0";
            return;
        }

        pnlNoQuestion.Visible = false;
        
        // 获取题目详情
        var questionBll = new LearnSite.BLL.ExamQuestion();
        decimal totalScore = 0;
        
        string html = "";
        foreach (var item in SelectedQuestions)
        {
            var question = questionBll.GetQuestionById(item.QuestionId);
            if (question != null)
            {
                totalScore += item.Score;
                html += string.Format(@"
                <div class='question-item'>
                    <div class='d-flex justify-content-between align-items-start'>
                        <div>
                            <span class='badge bg-secondary me-2'>{0}</span>
                            <strong>{1}</strong>
                            <small class='text-muted ms-3'>难度: {2}</small>
                        </div>
                        <div>
                            <input type='number' class='form-control form-control-sm d-inline-block' style='width:80px' 
                                   value='{3}' onchange='updateScore({4}, this.value)' /> 分
                            <a href='?remove={4}' class='btn btn-sm btn-outline-danger ms-2' onclick=""return confirm('确定移除此题？')"">移除</a>
                        </div>
                    </div>
                </div>", GetTypeName(question.QuestionType), question.QuestionContent, GetDifficultyName(question.Difficulty), item.Score, item.QuestionId);
            }
        }

        ltlQuestionList.Text = html;
        lblQuestionCount.Text = SelectedQuestions.Count.ToString();
        lblScoreSum.Text = totalScore.ToString();
    }

    private string GetTypeName(int type)
    {
        switch (type)
        {
            case 1: return "单选";
            case 2: return "多选";
            case 3: return "判断";
            case 4: return "填空";
            case 5: return "简答";
            default: return "其他";
        }
    }

    private string GetDifficultyName(int difficulty)
    {
        switch (difficulty)
        {
            case 1: return "简单";
            case 2: return "中等";
            case 3: return "困难";
            default: return "未知";
        }
    }

    protected void btnAddQuestion_Click(object sender, EventArgs e)
    {
        Response.Redirect("../question/banklist.aspx?select=1");
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtPaperName.Text.Trim()))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('请输入试卷名称！');", true);
            return;
        }

        if (SelectedQuestions.Count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('请至少添加一道题目！');", true);
            return;
        }

        // 构建试卷结构
        var sections = new List<PaperSection>
        {
            new PaperSection
            {
                SectionName = "default",
                SectionDesc = "",
                Questions = new List<SectionQuestion>()
            }
        };

        decimal totalScore = 0;
        int sortOrder = 1;
        foreach (var item in SelectedQuestions)
        {
            sections[0].Questions.Add(new SectionQuestion
            {
                QuestionId = item.QuestionId,
                Score = item.Score,
                SortOrder = sortOrder++
            });
            totalScore += item.Score;
        }

        var paper = new LearnSite.Model.ExamPaper
        {
            PaperName = txtPaperName.Text.Trim(),
            PaperType = int.Parse(ddlPaperType.SelectedValue),
            TotalScore = decimal.Parse(txtTotalScore.Text),
            PassScore = decimal.Parse(txtPassScore.Text),
            Duration = int.Parse(txtDuration.Text),
            Description = txtDescription.Text.Trim(),
            QuestionCount = SelectedQuestions.Count,
            Sections = Newtonsoft.Json.JsonConvert.SerializeObject(sections),
            Status = 0,
            CreateBy = tcook.Hid.ToString(),
            CreateTime = DateTime.Now
        };

        var paperBll = new LearnSite.BLL.ExamPaper();
        int paperId = paperBll.AddPaper(paper);

        if (paperId > 0)
        {
            // 保存题目关联
            paperBll.SavePaperQuestions(paperId, sections);
            Session["SelectedQuestions"] = null;
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('试卷创建成功！');location.href='paperlist.aspx';", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('创建失败，请重试！');", true);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Session["SelectedQuestions"] = null;
        Response.Redirect("paperlist.aspx");
    }
}

/// <summary>
/// 试卷题目项（用于Session存储）
/// </summary>
[Serializable]
public class PaperQuestionItem
{
    public long QuestionId { get; set; }
    public decimal Score { get; set; }
    public int SortOrder { get; set; }
}
