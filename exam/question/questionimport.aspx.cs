using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using LearnSite.Model;

public partial class exam_question_questionimport : System.Web.UI.Page
{
    protected TeaCook tcook = new TeaCook();
    protected int BankId { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (!tcook.IsExist())
        {
            Response.Redirect("~/index.aspx");
            return;
        }

        int bankId;
        if (!int.TryParse(Request.QueryString["bankId"], out bankId) || bankId <= 0)
        {
            Response.Redirect("banklist.aspx");
            return;
        }

        BankId = bankId;

        if (!IsPostBack)
        {
            LoadBankInfo();
        }
    }

    private void LoadBankInfo()
    {
        var bankBll = new LearnSite.BLL.ExamQuestionBank();
        var bank = bankBll.GetBankById(BankId);
        if (bank != null)
        {
            ltlBankName.Text = bank.BankName + " - 批量导入题目";
            Page.Title = bank.BankName + " - 批量导入题目";
        }
    }

    protected void btnImport_Click(object sender, EventArgs e)
    {
        string content = txtContent.Text.Trim();
        if (string.IsNullOrEmpty(content))
        {
            ShowError("请输入题目内容");
            return;
        }

        var lines = content.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        var questions = new List<ExamQuestion>();
        var errors = new List<string>();

        int lineNumber = 0;
        foreach (var line in lines)
        {
            lineNumber++;
            string trimmedLine = line.Trim();
            if (string.IsNullOrEmpty(trimmedLine)) continue;

            try
            {
                var question = ParseQuestion(trimmedLine, lineNumber);
                if (question != null)
                {
                    questions.Add(question);
                }
            }
            catch (Exception ex)
            {
                errors.Add(string.Format("第{0}行: {1}", lineNumber, ex.Message));
            }
        }

        if (questions.Count == 0)
        {
            ShowError("没有有效的题目可导入");
            if (errors.Count > 0)
            {
                ShowErrorDetails(errors);
            }
            return;
        }

        // 执行导入
        var questionBll = new LearnSite.BLL.ExamQuestion();
        int successCount = questionBll.ImportQuestions(BankId, questions, tcook.Hid.ToString());

        int failedCount = questions.Count - successCount + errors.Count;

        // 显示结果
        pnlImport.Visible = false;
        pnlResult.Visible = true;

        ltlTotal.Text = lines.Length.ToString();
        ltlSuccess.Text = successCount.ToString();
        ltlFailed.Text = failedCount.ToString();

        if (successCount > 0 && failedCount == 0)
        {
            resultBox.Attributes["class"] = "result-box result-success";
            ltlResult.Text = "导入成功！";
        }
        else if (successCount > 0)
        {
            resultBox.Attributes["class"] = "result-box result-warning";
            ltlResult.Text = "部分导入成功，请检查错误详情。";
        }
        else
        {
            resultBox.Attributes["class"] = "result-box result-error";
            ltlResult.Text = "导入失败，请检查题目格式。";
        }

        if (errors.Count > 0)
        {
            ShowErrorDetails(errors);
        }
    }

    private ExamQuestion ParseQuestion(string line, int lineNumber)
    {
        var parts = line.Split('|');
        if (parts.Length < 4)
        {
            throw new Exception("格式错误，至少需要题型、题目内容、选项、答案四个字段");
        }

        int questionType;
        if (!int.TryParse(parts[0].Trim(), out questionType) || questionType < 1 || questionType > 14)
        {
            throw new Exception("题型必须是1-14之间的数字");
        }

        string questionContent = parts[1].Trim();
        if (string.IsNullOrEmpty(questionContent))
        {
            throw new Exception("题目内容不能为空");
        }

        string options = parts.Length > 2 ? parts[2].Trim() : "";
        string answer = parts.Length > 3 ? parts[3].Trim() : "";
        string analysis = parts.Length > 4 ? parts[4].Trim() : "";
        
        decimal score = 2;
        if (parts.Length > 5 && !string.IsNullOrEmpty(parts[5].Trim()))
        {
            if (!decimal.TryParse(parts[5].Trim(), out score) || score <= 0)
            {
                score = 2;
            }
        }

        int difficulty = 1;
        if (parts.Length > 6 && !string.IsNullOrEmpty(parts[6].Trim()))
        {
            if (!int.TryParse(parts[6].Trim(), out difficulty) || difficulty < 1 || difficulty > 3)
            {
                difficulty = 1;
            }
        }

        string knowledgePoint = parts.Length > 7 ? parts[7].Trim() : "";

        // 验证必填项
        if (questionType <= 3 && string.IsNullOrEmpty(answer))
        {
            throw new Exception("选择题、判断题的答案不能为空");
        }
        
        // 填空题和多项填空题的答案验证
        if ((questionType == 4 || questionType == 9) && string.IsNullOrEmpty(answer))
        {
            throw new Exception("填空题的答案不能为空");
        }

        // 转换选项格式
        string optionsJson = ConvertOptionsToJson(options, questionType);

        var question = new ExamQuestion
        {
            BankId = BankId,
            QuestionType = questionType,
            QuestionContent = questionContent,
            QuestionText = StripHtml(questionContent),
            Options = optionsJson,
            Answer = answer,
            Analysis = analysis,
            Score = score,
            Difficulty = difficulty,
            KnowledgePoint = knowledgePoint,
            CreateBy = tcook.Hid.ToString()
        };

        return question;
    }

    private string ConvertOptionsToJson(string options, int questionType)
    {
        if (string.IsNullOrEmpty(options))
        {
            // 判断题、简答题、打分题、NPS可以没有选项
            if (questionType == 3 || questionType == 5 || questionType == 11 || questionType == 14)
            {
                return "";
            }
            return "";
        }

        // 打分题选项格式：1##2##3##4##5
        if (questionType == 11)
        {
            var scoreConfig = new LearnSite.Model.ScoreConfig();
            var parts = options.Split(new[] { "##" }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 2)
            {
                int min, max;
                if (int.TryParse(parts[0].Trim(), out min)) scoreConfig.Min = min;
                if (int.TryParse(parts[parts.Length - 1].Trim(), out max)) scoreConfig.Max = max;
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(scoreConfig);
        }

        // NPS评分配置
        if (questionType == 14)
        {
            var npsConfig = new LearnSite.Model.NpsConfig();
            return Newtonsoft.Json.JsonConvert.SerializeObject(npsConfig);
        }

        // 矩阵题选项格式：行1,行2##列1,列2
        if (questionType == 12 || questionType == 13)
        {
            var matrixConfig = new LearnSite.Model.MatrixConfig();
            var parts = options.Split(new[] { "##" }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 2)
            {
                matrixConfig.Rows = parts[0].Split(',').Select(x => x.Trim()).ToList();
                matrixConfig.Cols = parts[1].Split(',').Select(x => x.Trim()).ToList();
                matrixConfig.AllowMultiple = (questionType == 13);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(matrixConfig);
        }

        // 多项填空：格式为 空1提示##空2提示##空3提示
        if (questionType == 9)
        {
            var blankConfig = new LearnSite.Model.MultipleBlankConfig
            {
                Blanks = new List<LearnSite.Model.BlankItem>()
            };
            var parts = options.Split(new[] { "##" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < parts.Length; i++)
            {
                blankConfig.Blanks.Add(new LearnSite.Model.BlankItem
                {
                    Label = "空" + (i + 1),
                    Placeholder = parts[i].Trim()
                });
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(blankConfig);
        }

        // 普通选择题选项
        var optionList = new List<QuestionOption>();
        var optParts = options.Split(new[] { "##" }, StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < optParts.Length; i++)
        {
            string opt = optParts[i].Trim();
            string label = ((char)('A' + i)).ToString();
            string content = opt;

            // 如果选项已经包含标签（如 "A. xxx"），则提取内容和标签
            if (opt.Length > 2 && (opt[1] == '.' || opt[1] == '、' || opt[1] == ':'))
            {
                label = opt[0].ToString().ToUpper();
                content = opt.Substring(2).Trim();
            }

            optionList.Add(new QuestionOption
            {
                Label = label,
                Content = content,
                IsCorrect = false
            });
        }

        return Newtonsoft.Json.JsonConvert.SerializeObject(optionList);
    }

    private string StripHtml(string html)
    {
        if (string.IsNullOrEmpty(html)) return "";
        return System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", "");
    }

    private void ShowError(string message)
    {
        pnlImport.Visible = true;
        pnlResult.Visible = true;
        resultBox.Attributes["class"] = "result-box result-error";
        ltlResult.Text = message;
        statsBox.Visible = false;
    }

    private void ShowErrorDetails(List<string> errors)
    {
        pnlErrorDetails.Visible = true;
        var sb = new StringBuilder();
        sb.Append("<div style='background:#fff;padding:10px;border-radius:4px;max-height:300px;overflow-y:auto;'>");
        foreach (var error in errors)
        {
            sb.Append(string.Format("<div style='color:#ff4d4f;margin:5px 0;'>{0}</div>", HttpUtility.HtmlEncode(error)));
        }
        sb.Append("</div>");
        ltlErrorDetails.Text = sb.ToString();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtContent.Text = "";
    }

    protected void btnContinue_Click(object sender, EventArgs e)
    {
        pnlImport.Visible = true;
        pnlResult.Visible = false;
        txtContent.Text = "";
        pnlErrorDetails.Visible = false;
    }
}
