using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using LearnSite.Model;
using LearnSite.BLL;

public partial class student_examstart : System.Web.UI.Page
{
    protected Cook cook = new Cook();
    protected int TotalQuestions { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!cook.IsExist())
        {
            Response.Redirect("~/index.aspx");
            return;
        }

        if (!IsPostBack)
        {
            int examId = 0;
            if (int.TryParse(Request.QueryString["id"], out examId))
            {
                InitExam(examId);
            }
            else
            {
                Response.Redirect("~/student/examlist.aspx");
            }
        }
    }

    private void InitExam(int examId)
    {
        hfExamId.Value = examId.ToString();

        var examBll = new LearnSite.BLL.Exam();
        var exam = examBll.GetExamById(examId);
        
        if (exam == null)
        {
            Response.Redirect("~/student/examlist.aspx?msg=" + Uri.EscapeDataString("考试不存在"));
            return;
        }

        // 检查考试时间
        var now = DateTime.Now;
        if (now < exam.StartTime)
        {
            Response.Redirect("~/student/examlist.aspx?msg=" + Uri.EscapeDataString("考试尚未开始"));
            return;
        }
        if (now > exam.EndTime)
        {
            Response.Redirect("~/student/examlist.aspx?msg=" + Uri.EscapeDataString("考试已结束"));
            return;
        }

        ltlExamName.Text = exam.ExamName;

        // 检查是否已有答卷
        var answerBll = new LearnSite.BLL.ExamAnswer();
        var existingAnswer = answerBll.GetAnswerByExamAndStudent(examId, cook.Sid.ToString());

        long answerId = 0;
        if (existingAnswer != null && existingAnswer.Status > 0)
        {
            // 已提交，跳转到结果页
            Response.Redirect("~/student/examresult.aspx?examId=" + examId);
            return;
        }

        if (existingAnswer != null && existingAnswer.Status == 0)
        {
            // 继续答题
            answerId = existingAnswer.AnswerId;
            hfAnswerId.Value = answerId.ToString();
        }
        else
        {
            // 开始新答题
            answerId = answerBll.StartAnswer(
                examId,
                exam.PaperId,
                cook.Sid.ToString(),
                cook.Sname,
                cook.Sclass,
                Request.UserHostAddress,
                Request.UserAgent
            );
            hfAnswerId.Value = answerId.ToString();
        }

        // 加载试卷
        LoadPaper(exam.PaperId, existingAnswer != null ? existingAnswer.Answers : null);

        // 设置倒计时
        int duration = exam.Duration * 60;
        if (existingAnswer != null && existingAnswer.Status == 0)
        {
            var elapsed = (DateTime.Now - existingAnswer.StartTime).TotalSeconds;
            duration = Math.Max(0, (int)(duration - elapsed));
        }
        hfRemainingTime.Value = duration.ToString();

        // 防作弊设置
        hfMaxSwitch.Value = "3"; // 默认值，可从exam.AntiCheat解析
    }

    private void LoadPaper(int paperId, string tempAnswers)
    {
        var paperBll = new LearnSite.BLL.ExamPaper();
        var paper = paperBll.GetPaperById(paperId);
        var questions = paperBll.GetPaperQuestions(paperId);

        ltlTotalScore.Text = paper.TotalScore.ToString();
        ltlQuestionCount.Text = questions.Count.ToString();
        TotalQuestions = questions.Count;

        var questionList = questions.Select(pq => pq.Question).Where(q => q != null).ToList();
        rptQuestions.DataSource = questionList;
        rptQuestions.DataBind();
    }

    protected void rptQuestions_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            var question = e.Item.DataItem as LearnSite.Model.ExamQuestion;

            var pnlRadio = e.Item.FindControl("pnlRadio") as Panel;
            var pnlCheckbox = e.Item.FindControl("pnlCheckbox") as Panel;
            var pnlJudge = e.Item.FindControl("pnlJudge") as Panel;
            var pnlFillBlank = e.Item.FindControl("pnlFillBlank") as Panel;
            var pnlTextarea = e.Item.FindControl("pnlTextarea") as Panel;
            var pnlSelect = e.Item.FindControl("pnlSelect") as Panel;
            var pnlScore = e.Item.FindControl("pnlScore") as Panel;
            var pnlNps = e.Item.FindControl("pnlNps") as Panel;
            var pnlMatrix = e.Item.FindControl("pnlMatrix") as Panel;
            var pnlMultipleBlank = e.Item.FindControl("pnlMultipleBlank") as Panel;

            switch (question.QuestionType)
            {
                case 1: // 单选
                    pnlRadio.Visible = true;
                    var rblOptions = e.Item.FindControl("rblOptions") as RadioButtonList;
                    BindOptions(rblOptions, question.Options);
                    break;

                case 2: // 多选
                    pnlCheckbox.Visible = true;
                    var cblOptions = e.Item.FindControl("cblOptions") as CheckBoxList;
                    BindOptions(cblOptions, question.Options);
                    break;

                case 3: // 判断
                    pnlJudge.Visible = true;
                    break;

                case 4: // 填空
                    pnlFillBlank.Visible = true;
                    var phFillBlanks = e.Item.FindControl("phFillBlanks") as PlaceHolder;
                    BindFillBlanks(phFillBlanks, question.Answer);
                    break;

                case 5: // 简答
                    pnlTextarea.Visible = true;
                    break;

                case 6: // 连线 - 暂用简答方式
                case 7: // 分类 - 暂用简答方式
                case 8: // 组合 - 暂用简答方式
                    pnlTextarea.Visible = true;
                    break;

                case 9: // 多项填空
                    pnlMultipleBlank.Visible = true;
                    var phMultipleBlanks = e.Item.FindControl("phMultipleBlanks") as PlaceHolder;
                    BindMultipleBlanks(phMultipleBlanks, question.Options);
                    break;

                case 10: // 下拉选择
                    pnlSelect.Visible = true;
                    var ddlSelect = e.Item.FindControl("ddlSelect") as DropDownList;
                    BindSelectOptions(ddlSelect, question.Options);
                    break;

                case 11: // 打分
                    pnlScore.Visible = true;
                    BindScoreQuestion(e.Item, question.Options);
                    break;

                case 12: // 矩阵单选
                case 13: // 矩阵多选
                    pnlMatrix.Visible = true;
                    var ltlMatrixTable = e.Item.FindControl("ltlMatrixTable") as Literal;
                    BindMatrixQuestion(ltlMatrixTable, question.Options, question.QuestionType == 13, e.Item.ItemIndex);
                    break;

                case 14: // NPS
                    pnlNps.Visible = true;
                    BindNpsQuestion(e.Item, question.Options);
                    break;
            }
        }
    }

    private void BindOptions(ListControl list, string optionsJson)
    {
        if (string.IsNullOrEmpty(optionsJson)) return;
        var options = JsonConvert.DeserializeObject<List<QuestionOption>>(optionsJson);
        foreach (var opt in options)
        {
            list.Items.Add(new ListItem(opt.Content, opt.Label));
        }
    }

    private void BindFillBlanks(PlaceHolder ph, string answerJson)
    {
        if (string.IsNullOrEmpty(answerJson)) return;
        var answers = answerJson.Split('|');
        for (int i = 0; i < answers.Length; i++)
        {
            ph.Controls.Add(new Literal { Text = string.Format("第{0}空：<br/>", i + 1) });
            var txt = new TextBox { ID = string.Format("txtBlank{0}", i), CssClass = "fillblank-input" };
            ph.Controls.Add(txt);
            ph.Controls.Add(new Literal { Text = "<br/>" });
        }
    }

    private void BindMultipleBlanks(PlaceHolder ph, string optionsJson)
    {
        if (string.IsNullOrEmpty(optionsJson)) return;
        try
        {
            var config = JsonConvert.DeserializeObject<MultipleBlankConfig>(optionsJson);
            if (config.Blanks != null)
            {
                for (int i = 0; i < config.Blanks.Count; i++)
                {
                    var blank = config.Blanks[i];
                    var div = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
                    div.Attributes["class"] = "multiple-blank-item";
                    
                    var label = new Label { Text = blank.Label + "：", AssociatedControlID = string.Format("txtMB{0}", i) };
                    div.Controls.Add(label);
                    
                    var txt = new TextBox { ID = string.Format("txtMB{0}", i) };
                    txt.Attributes["placeholder"] = blank.Placeholder;
                    div.Controls.Add(txt);
                    
                    ph.Controls.Add(div);
                }
            }
        }
        catch
        {
            // 如果解析失败，使用默认方式
            ph.Controls.Add(new TextBox { ID = "txtMB0", CssClass = "fillblank-input" });
        }
    }

    private void BindSelectOptions(DropDownList ddl, string optionsJson)
    {
        ddl.Items.Add(new ListItem("请选择...", ""));
        if (string.IsNullOrEmpty(optionsJson)) return;
        var options = JsonConvert.DeserializeObject<List<QuestionOption>>(optionsJson);
        foreach (var opt in options)
        {
            ddl.Items.Add(new ListItem(opt.Content, opt.Label));
        }
    }

    private void BindScoreQuestion(RepeaterItem item, string optionsJson)
    {
        int min = 1, max = 5, def = 3;
        string lowLabel = "不满意", highLabel = "非常满意";
        
        if (!string.IsNullOrEmpty(optionsJson))
        {
            try
            {
                var config = JsonConvert.DeserializeObject<ScoreConfig>(optionsJson);
                min = config.Min;
                max = config.Max;
                def = config.Default;
                lowLabel = config.LowLabel ?? lowLabel;
                highLabel = config.HighLabel ?? highLabel;
            }
            catch { }
        }

        var hfScoreValue = item.FindControl("hfScoreValue") as HiddenField;
        hfScoreValue.Value = def.ToString();

        // 注册客户端脚本
        var script = string.Format(@"
        (function() {{
            var container = document.getElementById('scoreStars');
            var hf = document.getElementById('{0}');
            var min = {1}, max = {2}, def = {3};
            for (var i = min; i <= max; i++) {{
                var star = document.createElement('span');
                star.className = 'score-star' + (i <= def ? ' active' : '');
                star.innerHTML = '★';
                star.dataset.value = i;
                star.onclick = function() {{
                    container.querySelectorAll('.score-star').forEach(function(s, idx) {{
                        s.classList.toggle('active', idx + min <= parseInt(this.dataset.value));
                    }}.bind(this));
                    hf.value = this.dataset.value;
                }};
                container.appendChild(star);
            }}
        }})();
        ", hfScoreValue.ClientID, min, max, def);
        ScriptManager.RegisterStartupScript(item, item.GetType(), "score_" + item.ItemIndex, script, true);
    }

    private void BindNpsQuestion(RepeaterItem item, string optionsJson)
    {
        string lowText = "不满意", midText = "一般", highText = "非常满意";
        
        if (!string.IsNullOrEmpty(optionsJson))
        {
            try
            {
                var config = JsonConvert.DeserializeObject<NpsConfig>(optionsJson);
                lowText = config.LowText ?? lowText;
                midText = config.MidText ?? midText;
                highText = config.HighText ?? highText;
            }
            catch { }
        }

        var hfNpsValue = item.FindControl("hfNpsValue") as HiddenField;
        var ltlScript = new Literal();
        
        ltlScript.Text = string.Format(@"
        <script>
        (function() {{
            var container = document.getElementById('npsScale');
            var hf = document.getElementById('{0}');
            for (var i = 0; i <= 10; i++) {{
                var item = document.createElement('div');
                item.className = 'nps-item';
                item.textContent = i;
                item.dataset.value = i;
                item.onclick = function() {{
                    container.querySelectorAll('.nps-item').forEach(function(n) {{
                        n.classList.remove('active', 'nps-low', 'nps-mid', 'nps-high');
                    }});
                    var v = parseInt(this.dataset.value);
                    this.classList.add('active');
                    if (v <= 6) this.classList.add('nps-low');
                    else if (v <= 8) this.classList.add('nps-mid');
                    else this.classList.add('nps-high');
                    hf.value = v;
                }};
                container.appendChild(item);
            }}
        }})();
        </script>", hfNpsValue.ClientID);
        
        item.Controls.Add(ltlScript);
    }

    private void BindMatrixQuestion(Literal ltl, string optionsJson, bool allowMultiple, int itemIndex)
    {
        if (string.IsNullOrEmpty(optionsJson))
        {
            ltl.Text = "<p>题目配置错误</p>";
            return;
        }

        try
        {
            var config = JsonConvert.DeserializeObject<MatrixConfig>(optionsJson);
            var sb = new System.Text.StringBuilder();
            string inputType = allowMultiple ? "checkbox" : "radio";
            string inputName = string.Format("matrix_{0}", itemIndex);

            sb.Append("<table class='matrix-table'><thead><tr><th></th>");
            foreach (var col in config.Cols)
            {
                sb.Append(string.Format("<th>{0}</th>", col));
            }
            sb.Append("</tr></thead><tbody>");

            foreach (var row in config.Rows)
            {
                sb.Append(string.Format("<tr><td>{0}</td>", row));
                for (int i = 0; i < config.Cols.Count; i++)
                {
                    string inputId = string.Format("matrix_{0}_{1}_{2}", itemIndex, row, i);
                    string value = config.Cols[i];
                    if (allowMultiple)
                    {
                        sb.Append(string.Format("<td><input type='{0}' id='{1}' name='{2}_{3}' value='{4}' /></td>", inputType, inputId, inputName, row, value));
                    }
                    else
                    {
                        sb.Append(string.Format("<td><input type='{0}' id='{1}' name='{2}_{3}' value='{4}' /></td>", inputType, inputId, inputName, row, value));
                    }
                }
                sb.Append("</tr>");
            }
            sb.Append("</tbody></table>");

            ltl.Text = sb.ToString();
        }
        catch
        {
            ltl.Text = "<p>题目配置错误</p>";
        }
    }

    protected string GetTypeName(object type)
    {
        switch ((int)type)
        {
            case 1: return "单选题";
            case 2: return "多选题";
            case 3: return "判断题";
            case 4: return "填空题";
            case 5: return "简答题";
            case 6: return "连线题";
            case 7: return "分类题";
            case 8: return "组合题";
            case 9: return "多项填空";
            case 10: return "下拉选择";
            case 11: return "打分题";
            case 12: return "矩阵单选";
            case 13: return "矩阵多选";
            case 14: return "NPS评分";
            default: return "未知题型";
        }
    }

    protected void btnTempSave_Click(object sender, EventArgs e)
    {
        var answers = CollectAnswers();
        var answerBll = new LearnSite.BLL.ExamAnswer();
        answerBll.TempSaveAnswer(long.Parse(hfAnswerId.Value), answers);
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        var answers = CollectAnswers();
        var answerBll = new LearnSite.BLL.ExamAnswer();
        var result = answerBll.SubmitAnswer(
            long.Parse(hfAnswerId.Value),
            answers,
            Request.UserHostAddress
        );

        if (result.Success)
        {
            Response.Redirect(string.Format("~/student/examresult.aspx?examId={0}&score={1}", hfExamId.Value, result.TotalScore));
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alert", 
                string.Format("alert('{0}');", result.Message), true);
        }
    }

    private string CollectAnswers()
    {
        var answers = new Dictionary<long, string>();

        foreach (RepeaterItem item in rptQuestions.Items)
        {
            var hfQuestionId = item.FindControl("hfQuestionId") as HiddenField;
            long questionId = long.Parse(hfQuestionId.Value);

            var pnlRadio = item.FindControl("pnlRadio") as Panel;
            var pnlCheckbox = item.FindControl("pnlCheckbox") as Panel;
            var pnlJudge = item.FindControl("pnlJudge") as Panel;
            var pnlFillBlank = item.FindControl("pnlFillBlank") as Panel;
            var pnlTextarea = item.FindControl("pnlTextarea") as Panel;
            var pnlSelect = item.FindControl("pnlSelect") as Panel;
            var pnlScore = item.FindControl("pnlScore") as Panel;
            var pnlNps = item.FindControl("pnlNps") as Panel;
            var pnlMatrix = item.FindControl("pnlMatrix") as Panel;
            var pnlMultipleBlank = item.FindControl("pnlMultipleBlank") as Panel;

            string answer = "";

            if (pnlRadio.Visible)
            {
                var rbl = item.FindControl("rblOptions") as RadioButtonList;
                answer = rbl.SelectedValue;
            }
            else if (pnlCheckbox.Visible)
            {
                var cbl = item.FindControl("cblOptions") as CheckBoxList;
                var selected = new List<string>();
                foreach (ListItem li in cbl.Items)
                {
                    if (li.Selected) selected.Add(li.Value);
                }
                answer = string.Join(",", selected);
            }
            else if (pnlJudge.Visible)
            {
                string judgeKey = string.Format("judge_{0}", item.ItemIndex);
                answer = Request.Form[judgeKey] ?? "";
            }
            else if (pnlFillBlank.Visible)
            {
                var blanks = new List<string>();
                var ph = item.FindControl("phFillBlanks") as PlaceHolder;
                foreach (Control c in ph.Controls)
                {
                    var txt = c as TextBox;
                    if (txt != null)
                    {
                        blanks.Add(txt.Text.Trim());
                    }
                }
                answer = string.Join("|", blanks);
            }
            else if (pnlTextarea.Visible)
            {
                var txt = item.FindControl("txtAnswer") as TextBox;
                answer = txt.Text.Trim();
            }
            else if (pnlSelect.Visible)
            {
                var ddl = item.FindControl("ddlSelect") as DropDownList;
                answer = ddl.SelectedValue;
            }
            else if (pnlScore.Visible)
            {
                var hf = item.FindControl("hfScoreValue") as HiddenField;
                answer = hf.Value;
            }
            else if (pnlNps.Visible)
            {
                var hf = item.FindControl("hfNpsValue") as HiddenField;
                answer = hf.Value;
            }
            else if (pnlMatrix.Visible)
            {
                // 收集矩阵题答案
                var matrixAnswers = new Dictionary<string, object>();
                // 需要从Request.Form中解析
                foreach (string key in Request.Form.AllKeys)
                {
                    string prefix = string.Format("matrix_{0}_", item.ItemIndex);
                    if (key != null && key.StartsWith(prefix))
                    {
                        var row = key.Substring(prefix.Length);
                        var value = Request.Form[key];
                        if (!string.IsNullOrEmpty(value))
                        {
                            if (matrixAnswers.ContainsKey(row))
                            {
                                // 多选情况
                                var existing = matrixAnswers[row] as string;
                                if (existing != null)
                                {
                                    matrixAnswers[row] = existing + "," + value;
                                }
                            }
                            else
                            {
                                matrixAnswers[row] = value;
                            }
                        }
                    }
                }
                answer = JsonConvert.SerializeObject(matrixAnswers);
            }
            else if (pnlMultipleBlank.Visible)
            {
                var blanks = new List<string>();
                var ph = item.FindControl("phMultipleBlanks") as PlaceHolder;
                foreach (Control c in ph.Controls)
                {
                    var div = c as System.Web.UI.HtmlControls.HtmlGenericControl;
                    if (div != null)
                    {
                        foreach (Control cc in div.Controls)
                        {
                            var txt = cc as TextBox;
                            if (txt != null)
                            {
                                blanks.Add(txt.Text.Trim());
                            }
                        }
                    }
                }
                answer = string.Join("|", blanks);
            }

            answers[questionId] = answer;
        }

        return JsonConvert.SerializeObject(answers);
    }
}
