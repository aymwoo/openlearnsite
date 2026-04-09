using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using LearnSite.Model;
using LearnSite.BLL;

public partial class exam_question_questionlist : System.Web.UI.Page
{
    protected TeaCook tcook = new TeaCook();
    protected int BankId { get; set; }
    protected bool IsSelectMode { get; set; }
    private const int PageSize = 20;

    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (!tcook.IsExist())
        {
            Response.Redirect("~/index.aspx");
            return;
        }

        int bankId;
        if (!int.TryParse(Request.QueryString["bankId"], out bankId))
        {
            // 如果没有指定题库，显示所有题库的题目供选择
            bankId = 0;
        }

        BankId = bankId;
        
        // 检查是否为选择模式
        IsSelectMode = Request.QueryString["select"] == "1";

        if (!IsPostBack)
        {
            LoadBankInfo();
            BindQuestions(1);
            
            if (IsSelectMode)
            {
                SetupSelectMode();
            }
        }
    }

    private void SetupSelectMode()
    {
        pnlNormalButtons.Visible = false;
        pnlSelectButtons.Visible = true;
        pnlSelectBar.Visible = true;
    }

    private void LoadBankInfo()
    {
        if (BankId > 0)
        {
            var bankBll = new LearnSite.BLL.ExamQuestionBank();
            var bank = bankBll.GetBankById(BankId);
            if (bank != null)
            {
                ltlBankName.Text = bank.BankName;
            }
        }
        else
        {
            ltlBankName.Text = "所有题库";
        }
    }

    private void BindQuestions(int pageIndex)
    {
        int? type = null;
        int? difficulty = null;

        if (!string.IsNullOrEmpty(ddlType.SelectedValue))
            type = int.Parse(ddlType.SelectedValue);

        if (!string.IsNullOrEmpty(ddlDifficulty.SelectedValue))
            difficulty = int.Parse(ddlDifficulty.SelectedValue);

        var questionBll = new LearnSite.BLL.ExamQuestion();
        var result = questionBll.GetQuestionList(BankId, pageIndex, PageSize, type, difficulty, txtKeyword.Text.Trim());
        var list = result.Item1;
        var total = result.Item2;
        rptQuestions.DataSource = list;
        rptQuestions.DataBind();

        // 分页
        int totalPages = (total + PageSize - 1) / PageSize;
        string selectParam = IsSelectMode ? "&select=1" : "";
        ltlPagination.Text = BuildPagination(pageIndex, totalPages, selectParam);
    }

    private string BuildPagination(int currentPage, int totalPages, string extraParams = "")
    {
        if (totalPages <= 1) return "";

        var sb = new StringBuilder();

        if (currentPage > 1)
        {
            sb.Append(string.Format("<a href=\"?bankId={0}&page={1}{2}\">上一页</a>", BankId, currentPage - 1, extraParams));
        }

        int start = Math.Max(1, currentPage - 2);
        int end = Math.Min(totalPages, currentPage + 2);

        if (start > 1)
        {
            sb.Append(string.Format("<a href=\"?bankId={0}&page=1{1}\">1</a>", BankId, extraParams));
            if (start > 2) sb.Append("<span>...</span>");
        }

        for (int i = start; i <= end; i++)
        {
            if (i == currentPage)
            {
                sb.Append(string.Format("<span class=\"current\">{0}</span>", i));
            }
            else
            {
                sb.Append(string.Format("<a href=\"?bankId={0}&page={1}{2}\">{1}</a>", BankId, i, extraParams));
            }
        }

        if (end < totalPages)
        {
            if (end < totalPages - 1) sb.Append("<span>...</span>");
            sb.Append(string.Format("<a href=\"?bankId={0}&page={1}{2}\">{3}</a>", BankId, totalPages, extraParams, totalPages));
        }

        if (currentPage < totalPages)
        {
            sb.Append(string.Format("<a href=\"?bankId={0}&page={1}{2}\">下一页</a>", BankId, currentPage + 1, extraParams));
        }

        return sb.ToString();
    }

    protected void Filter_Changed(object sender, EventArgs e)
    {
        BindQuestions(1);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindQuestions(1);
    }

    protected void rptQuestions_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            long questionId = long.Parse(e.CommandArgument.ToString());
            var questionBll = new LearnSite.BLL.ExamQuestion();
            questionBll.DeleteQuestion(questionId);

            // 更新题库题目数量
            var bankBll = new LearnSite.BLL.ExamQuestionBank();
            bankBll.UpdateQuestionCount(BankId);

            BindQuestions(1);
        }
    }

    protected void rptQuestions_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            var question = e.Item.DataItem as LearnSite.Model.ExamQuestion;
            if (question != null)
            {
                var pnlNormalActions = e.Item.FindControl("pnlNormalActions") as Panel;
                var pnlSelectActions = e.Item.FindControl("pnlSelectActions") as Panel;
                
                if (pnlNormalActions != null && pnlSelectActions != null)
                {
                    pnlNormalActions.Visible = !IsSelectMode;
                    pnlSelectActions.Visible = IsSelectMode;
                }
            }
        }
    }

    protected void btnConfirmSelect_Click(object sender, EventArgs e)
    {
        string selectedQuestionIds = selectedIds.Value;
        if (!string.IsNullOrEmpty(selectedQuestionIds))
        {
            Response.Redirect(string.Format("../paper/paperadd.aspx?qids={0}", selectedQuestionIds));
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('请至少选择一道题目！');", true);
        }
    }

    protected string GetTypeName(object type)
    {
        switch ((int)type)
        {
            case 1: return "单选";
            case 2: return "多选";
            case 3: return "判断";
            case 4: return "填空";
            case 5: return "简答";
            case 6: return "连线";
            case 7: return "分类";
            case 8: return "组合";
            case 9: return "多填空";
            case 10: return "下拉";
            case 11: return "打分";
            case 12: return "矩阵单选";
            case 13: return "矩阵多选";
            case 14: return "NPS";
            default: return "未知";
        }
    }

    protected string GetDifficultyName(object difficulty)
    {
        switch ((int)difficulty)
        {
            case 1: return "简单";
            case 2: return "中等";
            case 3: return "困难";
            default: return "未知";
        }
    }
}
