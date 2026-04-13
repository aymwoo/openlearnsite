using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Newtonsoft.Json;
using LearnSite.Model;
using LearnSite.BLL;

public partial class exam_question_questionadd : System.Web.UI.Page
{
    protected TeaCook tcook = new TeaCook();
    protected int BankId { get; set; }
    protected long QuestionId { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (!tcook.IsExist())
        {
            Response.Redirect("~/index.aspx");
            return;
        }

        // 分别解析 bankId 和 id，它们是独立的参数
        int bankId = 0;
        long qid = 0;

        if (int.TryParse(Request.QueryString["bankId"], out bankId))
        {
            BankId = bankId;
        }

        if (long.TryParse(Request.QueryString["id"], out qid))
        {
            QuestionId = qid;
        }

        // 如果两个参数都没有，重定向到题库列表
        if (BankId == 0 && QuestionId == 0)
        {
            Response.Redirect("banklist.aspx");
            return;
        }

        // 如果有 id 但没有 bankId，需要从数据库获取题目的 bankId
        if (QuestionId > 0 && BankId == 0)
        {
            var questionBll = new LearnSite.BLL.ExamQuestion();
            var question = questionBll.GetQuestionById(QuestionId);
            if (question != null)
            {
                BankId = question.BankId;
            }
        }

        hfBankId.Value = BankId.ToString();
        hfQuestionId.Value = QuestionId.ToString();

        if (!IsPostBack)
        {
            if (QuestionId > 0)
            {
                LoadQuestion();
                ltlTitle.Text = "编辑题目";
            }
            else
            {
                ltlTitle.Text = "添加题目";
                CreateOptions(4); // 默认4个选项
            }
        }
        else
        {
            // PostBack时重新创建选项（从Request.Form读取）

            int type = int.Parse(ddlType.SelectedValue);

            // 隐藏所有面板
            pnlOptions.Visible = false;
            pnlJudge.Visible = false;
            pnlFillBlank.Visible = false;
            pnlTextAnswer.Visible = false;
            pnlMultipleBlank.Visible = false;
            pnlScore.Visible = false;
            pnlNps.Visible = false;
            pnlMatrix.Visible = false;
            pnlMatching.Visible = false;
            pnlSorting.Visible = false;

            // 根据题型显示对应面板并重建选项
            switch (type)
            {
                case 1: // 单选
                case 2: // 多选
                case 10: // 下拉选择
                    pnlOptions.Visible = true;
                    ReloadOptionsFromForm(type);
                    break;
                case 3: // 判断
                    pnlJudge.Visible = true;
                    break;
                case 4: // 填空
                    pnlFillBlank.Visible = true;
                    break;
                case 5: // 简答
                    pnlTextAnswer.Visible = true;
                    break;
                case 6: // 连线
                    pnlMatching.Visible = true;
                    break;
                case 7: // 分类
                    pnlSorting.Visible = true;
                    break;
                case 9: // 多项填空
                    pnlMultipleBlank.Visible = true;
                    break;
                case 11: // 打分
                    pnlScore.Visible = true;
                    break;
                case 12: // 矩阵单选
                case 13: // 矩阵多选
                    pnlMatrix.Visible = true;
                    break;
                case 14: // NPS
                    pnlNps.Visible = true;
                    break;
            }
        }
    }

    private void LoadQuestion()
    {
        var questionBll = new LearnSite.BLL.ExamQuestion();
        var question = questionBll.GetQuestionById(QuestionId);
        if (question == null)
        {
            Response.Redirect("banklist.aspx");
            return;
        }

        BankId = question.BankId;
        hfBankId.Value = BankId.ToString();

        // 根据题型加载答案，直接传递题型参数，避免读取ddlType.SelectedValue触发回发
        int questionType = question.QuestionType;

        switch (questionType)
        {
            case 1: // 单选
            case 2: // 多选
            case 10: // 下拉选择
                LoadOptions(question.Options, question.Answer, questionType);
                break;
            case 3: // 判断
                pnlOptions.Visible = false;
                pnlJudge.Visible = true;
                if (question.Answer == "T" || question.Answer == "正确")
                    rbTrue.Checked = true;
                else
                    rbFalse.Checked = true;
                break;
            case 4: // 填空
                pnlOptions.Visible = false;
                pnlFillBlank.Visible = true;
                txtFillAnswer.Text = question.Answer;
                break;
            case 5: // 简答
                pnlOptions.Visible = false;
                pnlTextAnswer.Visible = true;
                txtRefAnswer.Text = question.Answer;
                break;
            case 6: // 连线
                pnlOptions.Visible = false;
                pnlMatching.Visible = true;
                txtMatching.Text = question.Answer;
                break;
            case 7: // 分类
                pnlOptions.Visible = false;
                pnlSorting.Visible = true;
                txtSorting.Text = question.Answer;
                break;
            case 9: // 多项填空
                pnlOptions.Visible = false;
                pnlMultipleBlank.Visible = true;
                txtMultipleBlank.Text = question.Answer;
                break;
            case 11: // 打分
                pnlOptions.Visible = false;
                pnlScore.Visible = true;
                if (!string.IsNullOrEmpty(question.Options))
                {
                    try
                    {
                        var scoreConfig = JsonConvert.DeserializeObject<ScoreConfig>(question.Options);
                        txtScoreMin.Text = scoreConfig.Min.ToString();
                        txtScoreMax.Text = scoreConfig.Max.ToString();
                        txtScoreDefault.Text = scoreConfig.Default.ToString();
                    }
                    catch { }
                }
                break;
            case 12: // 矩阵单选
            case 13: // 矩阵多选
                pnlOptions.Visible = false;
                pnlMatrix.Visible = true;
                if (!string.IsNullOrEmpty(question.Options))
                {
                    try
                    {
                        var matrixConfig = JsonConvert.DeserializeObject<MatrixConfig>(question.Options);
                        txtMatrixRows.Text = string.Join(",", matrixConfig.Rows);
                        txtMatrixCols.Text = string.Join(",", matrixConfig.Cols);
                    }
                    catch { }
                }
                txtMatrixAnswer.Text = question.Answer;
                break;
            case 14: // NPS
                pnlOptions.Visible = false;
                pnlNps.Visible = true;
                if (!string.IsNullOrEmpty(question.Options))
                {
                    try
                    {
                        var npsConfig = JsonConvert.DeserializeObject<NpsConfig>(question.Options);
                        txtNpsLow.Text = npsConfig.LowText;
                        txtNpsHigh.Text = npsConfig.HighText;
                    }
                    catch { }
                }
                break;
        }

        // 最后设置其他字段
        ddlType.SelectedValue = question.QuestionType.ToString();
        ddlDifficulty.SelectedValue = question.Difficulty.ToString();
        txtScore.Text = question.Score.ToString();
        hfContent.Value = question.QuestionContent;
        txtAnalysis.Text = question.Analysis;
        txtKnowledge.Text = question.KnowledgePoint;
        txtTags.Text = question.Tags;

        // 设置编辑器内容的脚本
        string script = string.Format(@"$(document).ready(function() {{
            $('#summernote-content-question').summernote('code', {0});
        }});", JsonConvert.SerializeObject(question.QuestionContent));
        ClientScript.RegisterStartupScript(this.GetType(), "SetEditorContent", script, true);
    }

    private void LoadOptions(string optionsJson, string answer)
    {
        LoadOptions(optionsJson, answer, null);
    }

    /// <summary>
    /// 加载选项（重载方法，可以指定题型）
    /// </summary>
    private void LoadOptions(string optionsJson, string answer, int? forceType)
    {
        phOptions.Controls.Clear();

        // 如果options为空或null，创建默认的4个选项
        if (string.IsNullOrWhiteSpace(optionsJson))
        {
            CreateOptionsWithForceType(4, forceType);
            return;
        }

        try
        {
            var options = JsonConvert.DeserializeObject<List<QuestionOption>>(optionsJson);
            if (options == null || options.Count == 0)
            {
                CreateOptionsWithForceType(4, forceType);
                return;
            }

            var correctAnswers = answer.Split(',');
            // 使用forceType或ddlType.SelectedValue
            int type = forceType ?? int.Parse(ddlType.SelectedValue);

            foreach (var opt in options)
            {
                bool isCorrect = correctAnswers.Contains(opt.Label);
                AddOptionControlWithLabel(opt.Label, opt.Content, isCorrect, type, opt.Image);
            }
        }
        catch (Exception ex)
        {
            // 如果反序列化失败，创建默认的4个选项
            CreateOptionsWithForceType(4, forceType);
        }
    }

    private void CreateOptionsWithForceType(int count, int? forceType)
    {
        phOptions.Controls.Clear();
        string[] labels = { "A", "B", "C", "D", "E", "F", "G", "H" };
        int type = forceType ?? int.Parse(ddlType.SelectedValue);

        for (int i = 0; i < count && i < labels.Length; i++)
        {
            // 使用重载方法，传递题型参数
            AddOptionControlWithLabel(labels[i], "", false, type);
        }
    }

    private void CreateOptions(int count)
    {
        phOptions.Controls.Clear();
        string[] labels = { "A", "B", "C", "D", "E", "F", "G", "H" };
        int type = int.Parse(ddlType.SelectedValue);

        for (int i = 0; i < count && i < labels.Length; i++)
        {
            // 使用重载方法，传递题型参数
            AddOptionControlWithLabel(labels[i], "", false, type);
        }
    }

    /// <summary>
    /// 从Request.Form重建选项（PostBack时调用）
    /// </summary>
    private void ReloadOptionsFromForm(int type)
    {
        phOptions.Controls.Clear();
        string[] labels = { "A", "B", "C", "D", "E", "F", "G", "H" };

        for (int i = 0; i < labels.Length; i++)
        {
            string label = labels[i];

            // 查找匹配的键值（因为ASP.NET会修改ID）
            string content = FindFormValue("txt_" + label);

            if (string.IsNullOrEmpty(content))
            {
                continue; // 如果该选项为空，跳过
            }

            bool isCorrect = false;
            if (type == 1) // 单选
            {
                string rbValue = FindFormValue("rb_" + label);
                isCorrect = (rbValue == "on");
            }
            else if (type == 2) // 多选
            {
                string cbValue = FindFormValue("cb_" + label);
                isCorrect = (cbValue == "on");
            }

            // 使用重载方法，传递题型参数
            AddOptionControlWithLabel(label, content, isCorrect, type);
        }

        // 如果没有选项，至少创建2个
        if (phOptions.Controls.Count == 0)
        {
            CreateOptions(2);
        }
    }

    /// <summary>
    /// 从Request.Form中查找匹配的值（因为ASP.NET会修改动态控件的ID）
    /// </summary>
    private string FindFormValue(string controlId)
    {
        foreach (string key in Request.Form.AllKeys)
        {
            // 方法1：查找以$controlId结尾的键
            if (!string.IsNullOrEmpty(key) && key.EndsWith("$" + controlId))
            {
                return Request.Form[key];
            }
            // 方法2：查找包含controlId的键（处理特殊情况）
            else if (!string.IsNullOrEmpty(key) && key.Contains(controlId))
            {
                return Request.Form[key];
            }
        }
        return null;
    }

    private void AddOptionControl(string label, string content, bool isCorrect)
    {
        int type = int.Parse(ddlType.SelectedValue);
        AddOptionControlWithLabel(label, content, isCorrect, type);
    }

    private void AddOptionControlWithLabel(string label, string content, bool isCorrect, int type)
    {
        AddOptionControlWithLabel(label, content, isCorrect, type, null);
    }

    private void AddOptionControlWithLabel(string label, string content, bool isCorrect, int type, string imageUrl)
    {
        var div = new HtmlGenericControl("div");
        div.Attributes["class"] = "option-item-wrapper";
        div.Attributes["data-label"] = label;

        var labelCtrl = new Label { Text = label + ".", CssClass = "option-label" };
        div.Controls.Add(labelCtrl);

        if (type == 1)
        {
            var rb = new RadioButton { ID = "rb_" + label, GroupName = "correctAnswer", Checked = isCorrect };
            rb.EnableViewState = false;
            rb.Attributes["value"] = label;
            div.Controls.Add(rb);
        }
        else if (type == 2)
        {
            var cb = new CheckBox { ID = "cb_" + label, Checked = isCorrect };
            cb.EnableViewState = false;
            cb.Attributes["value"] = label;
            div.Controls.Add(cb);
        }

        var txt = new TextBox { ID = "txt_" + label, Text = content, CssClass = "option-input" };
        txt.EnableViewState = false;
        div.Controls.Add(txt);

        var imgPreview = new HtmlImage { ID = "img_" + label };
        imgPreview.Attributes["class"] = "option-image-preview";
        imgPreview.Style["display"] = string.IsNullOrEmpty(imageUrl) ? "none" : "block";
        if (!string.IsNullOrEmpty(imageUrl))
        {
            imgPreview.Src = GetImageUrl(imageUrl);
        }
        div.Controls.Add(imgPreview);

        var controlsDiv = new HtmlGenericControl("div");
        controlsDiv.Attributes["class"] = "option-controls";

        var uploadBtn = new HtmlButton();
        uploadBtn.Attributes["type"] = "button";
        uploadBtn.Attributes["class"] = "option-image-btn";
        uploadBtn.InnerText = string.IsNullOrEmpty(imageUrl) ? "📷" : "更换";
        uploadBtn.Attributes["onclick"] = "handleOptionImageUpload('" + label + "')";
        controlsDiv.Controls.Add(uploadBtn);

        var deleteBtn = new HtmlButton();
        deleteBtn.Attributes["type"] = "button";
        deleteBtn.Attributes["class"] = "option-image-btn delete-btn";
        deleteBtn.InnerText = "🗑️";
        deleteBtn.Style["display"] = string.IsNullOrEmpty(imageUrl) ? "none" : "inline-block";
        deleteBtn.Attributes["onclick"] = "removeOptionImage('" + label + "')";
        controlsDiv.Controls.Add(deleteBtn);

        div.Controls.Add(controlsDiv);

        var btn = new Button { Text = "×", CssClass = "btn btn-sm btn-default", CausesValidation = false };
        btn.Click += (s, e) => { div.Visible = false; };
        div.Controls.Add(btn);

        phOptions.Controls.Add(div);
    }

    private string GetImageUrl(string imagePath)
    {
        if (string.IsNullOrEmpty(imagePath))
            return string.Empty;

        if (imagePath.StartsWith("http://") || imagePath.StartsWith("https://"))
            return imagePath;

        return "/webform/uploads/" + imagePath;
    }

    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        int type = int.Parse(ddlType.SelectedValue);

        // 隐藏所有面板
        pnlOptions.Visible = false;
        pnlJudge.Visible = false;
        pnlFillBlank.Visible = false;
        pnlTextAnswer.Visible = false;
        pnlMultipleBlank.Visible = false;
        pnlScore.Visible = false;
        pnlNps.Visible = false;
        pnlMatrix.Visible = false;
        pnlMatching.Visible = false;
        pnlSorting.Visible = false;

        // 根据题型显示对应面板
        switch (type)
        {
            case 1: // 单选
            case 2: // 多选
            case 10: // 下拉选择
                pnlOptions.Visible = true;
                // 创建默认的4个选项（切换题型时重新开始）
                CreateOptions(4);
                break;
            case 3: // 判断
                pnlJudge.Visible = true;
                break;
            case 4: // 填空
                pnlFillBlank.Visible = true;
                break;
            case 5: // 简答
                pnlTextAnswer.Visible = true;
                break;
            case 6: // 连线
                pnlMatching.Visible = true;
                break;
            case 7: // 分类
                pnlSorting.Visible = true;
                break;
            case 9: // 多项填空
                pnlMultipleBlank.Visible = true;
                break;
            case 11: // 打分
                pnlScore.Visible = true;
                break;
            case 12: // 矩阵单选
            case 13: // 矩阵多选
                pnlMatrix.Visible = true;
                break;
            case 14: // NPS
                pnlNps.Visible = true;
                break;
        }
    }

    protected void btnAddOption_Click(object sender, EventArgs e)
    {
        // 添加新选项
        int count = phOptions.Controls.Count;
        string[] labels = { "A", "B", "C", "D", "E", "F", "G", "H" };
        int type = int.Parse(ddlType.SelectedValue);

        if (count < labels.Length)
        {
            AddOptionControlWithLabel(labels[count], "", false, type);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        SaveQuestion();
        // 如果保存成功（没有错误消息），延迟1.5秒后跳转
        string script = @"setTimeout(function() {{
            window.location.href = 'questionlist.aspx?bankId=" + BankId + @"';
        }}, 1500);";
        ClientScript.RegisterStartupScript(this.GetType(), "RedirectAfterSave", script, true);
    }

    protected void btnSaveAdd_Click(object sender, EventArgs e)
    {
        SaveQuestion();
        // 清空表单继续添加
        hfContent.Value = "";
        txtAnalysis.Text = "";
        txtTags.Text = "";
        txtKnowledge.Text = "";
        CreateOptions(4);
        
        // 清空编辑器内容
        string script = "$('#summernote-content-question').summernote('code', '');";
        ClientScript.RegisterStartupScript(this.GetType(), "ClearEditor", script, true);
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(string.Format("questionlist.aspx?bankId={0}", BankId));
    }

    private void SaveQuestion()
    {
        string content = hfContent.Value.Trim();
        if (string.IsNullOrEmpty(content))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('请输入题目内容！');", true);
            return;
        }

        int type = int.Parse(ddlType.SelectedValue);
        string answer = "";
        string optionsJson = "";

        Dictionary<string, string> optionImages = new Dictionary<string, string>();
        if (!string.IsNullOrEmpty(hfOptionImages.Value))
        {
            try
            {
                optionImages = JsonConvert.DeserializeObject<Dictionary<string, string>>(hfOptionImages.Value);
            }
            catch { }
        }

        switch (type)
        {
            case 1: // 单选
            case 2: // 多选
            case 10: // 下拉选择
                var options = new List<QuestionOption>();
                var correctAnswers = new List<string>();

                string jsCorrectAnswers = hfCorrectAnswers.Value;

                if (!string.IsNullOrEmpty(jsCorrectAnswers))
                {
                    var correctLabelSet = new HashSet<string>(jsCorrectAnswers.Split(','));

                    for (char c = 'A'; c <= 'H'; c++)
                    {
                        string label = c.ToString();
                        string contentValue = FindFormValue("txt_" + label);

                        if (!string.IsNullOrEmpty(contentValue))
                        {
                            bool isCorrect = correctLabelSet.Contains(label);
                            string imageUrl = optionImages.ContainsKey(label) ? optionImages[label] : null;
                            options.Add(new QuestionOption { Label = label, Content = contentValue, IsCorrect = isCorrect, Image = imageUrl });
                            if (isCorrect) correctAnswers.Add(label);
                        }
                    }
                }
                else
                {
                    for (char c = 'A'; c <= 'H'; c++)
                    {
                        string label = c.ToString();
                        string contentValue = FindFormValue("txt_" + label);
                        string rbValue = FindFormValue("rb_" + label);
                        string cbValue = FindFormValue("cb_" + label);

                        if (!string.IsNullOrEmpty(contentValue))
                        {
                            bool isCorrect = false;
                            if (type == 1)
                            {
                                isCorrect = !string.IsNullOrEmpty(rbValue);
                            }
                            else if (type == 2)
                            {
                                isCorrect = !string.IsNullOrEmpty(cbValue);
                            }

                            string imageUrl = optionImages.ContainsKey(label) ? optionImages[label] : null;
                            options.Add(new QuestionOption { Label = label, Content = contentValue, IsCorrect = isCorrect, Image = imageUrl });
                            if (isCorrect) correctAnswers.Add(label);
                        }
                    }
                }

                if (options.Count < 2)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert",
                        String.Format("alert('单选和多选题至少需要2个选项！当前只有{0}个选项');", options.Count), true);
                    return;
                }

                if (correctAnswers.Count == 0)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert",
                        "alert('请选择正确答案！');", true);
                    return;
                }

                optionsJson = JsonConvert.SerializeObject(options);
                answer = string.Join(",", correctAnswers);
                break;

            case 3: // 判断
                answer = rbTrue.Checked ? "T" : "F";
                break;

            case 4: // 填空
                answer = txtFillAnswer.Text.Trim();
                break;

            case 5: // 简答
                answer = txtRefAnswer.Text.Trim();
                break;

            case 6: // 连线
                answer = txtMatching.Text.Trim();
                break;

            case 7: // 分类
                answer = txtSorting.Text.Trim();
                break;

            case 9: // 多项填空
                answer = txtMultipleBlank.Text.Trim();
                // 构建配置
                var blanks = new List<BlankItem>();
                var lines = txtMultipleBlank.Text.Trim().Split('\n');
                for (int i = 0; i < lines.Length; i++)
                {
                    var parts = lines[i].Split('|');
                    blanks.Add(new BlankItem
                    {
                        Label = "空" + (i + 1),
                        Answer = parts[0].Trim(),
                        Placeholder = parts.Length > 1 ? parts[1].Trim() : ""
                    });
                }
                optionsJson = JsonConvert.SerializeObject(new MultipleBlankConfig { Blanks = blanks });
                break;

            case 11: // 打分
                int sm, sx, sd;
                int scoreMin = int.TryParse(txtScoreMin.Text, out sm) ? sm : 1;
                int scoreMax = int.TryParse(txtScoreMax.Text, out sx) ? sx : 5;
                int scoreDef = int.TryParse(txtScoreDefault.Text, out sd) ? sd : 3;
                optionsJson = JsonConvert.SerializeObject(new ScoreConfig { Min = scoreMin, Max = scoreMax, Default = scoreDef });
                answer = scoreDef.ToString();
                break;

            case 12: // 矩阵单选
            case 13: // 矩阵多选
                var rows = txtMatrixRows.Text.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToList();
                var cols = txtMatrixCols.Text.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToList();
                optionsJson = JsonConvert.SerializeObject(new MatrixConfig { Rows = rows, Cols = cols, AllowMultiple = type == 13 });
                answer = txtMatrixAnswer.Text.Trim();
                break;

            case 14: // NPS
                var npsConfig = new NpsConfig
                {
                    LowText = txtNpsLow.Text.Trim(),
                    HighText = txtNpsHigh.Text.Trim()
                };
                optionsJson = JsonConvert.SerializeObject(npsConfig);
                answer = "5";
                break;
        }

        var question = new LearnSite.Model.ExamQuestion
        {
            BankId = BankId,
            QuestionType = type,
            QuestionContent = content,
            Options = optionsJson,
            Answer = answer,
            Analysis = txtAnalysis.Text.Trim(),
            Score = decimal.Parse(txtScore.Text),
            Difficulty = int.Parse(ddlDifficulty.SelectedValue),
            KnowledgePoint = txtKnowledge.Text.Trim(),
            Tags = txtTags.Text.Trim(),
            CreateBy = tcook.Hid.ToString()
        };

        var questionBll = new LearnSite.BLL.ExamQuestion();

        if (QuestionId > 0)
        {
            question.QuestionId = QuestionId;
            question.UpdateBy = tcook.Hid.ToString();
            questionBll.UpdateQuestion(question);
        }
        else
        {
            questionBll.AddQuestion(question);
            // 更新题库题目数量
            var bankBll = new LearnSite.BLL.ExamQuestionBank();
            bankBll.UpdateQuestionCount(BankId);
        }
    }
}
