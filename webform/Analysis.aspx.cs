using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Text;

public partial class webform_Analysis : System.Web.UI.Page
{
    protected int TotalStudents = 0;
    protected int Persons = 0;
    protected int avgScore = 0;
    protected int avgSpent = 0;
    protected int NoPersons = 0;
    protected int ExcellentCount = 0;
    protected int GoodCount = 0;
    protected int PassCount = 0;
    protected int FailCount = 0;
    protected string PageStatus = string.Empty;
    protected string StudentJsonData = "[]";

    LearnSite.Model.Cook cook = new LearnSite.Model.Cook();

    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.KickStudent();
        if (!IsPostBack)
        {
            showAnswers();
        }
    }

    protected void BtnRefresh_Click(object sender, EventArgs e)
    {
        showAnswers();
    }

    protected void showAnswers()
    {
        int Eid = 1;
        if (Request.QueryString["eid"] != null)
        {
            Eid = Int32.Parse(Request.QueryString["eid"].ToString());
        }

        LearnSite.BLL.Answers abll = new LearnSite.BLL.Answers();
        System.Data.DataTable dt = abll.GetListClassScore(Eid, cook.Sgrade, cook.Sclass);

        DataTable dtNo = abll.GetListClassSname(Eid, cook.Sgrade, cook.Sclass);
        RepeaterNo.DataSource = dtNo;
        RepeaterNo.DataBind();
        NoPersons = dtNo.Rows.Count;

        TotalStudents = dt.Rows.Count + NoPersons;
        Persons = dt.Rows.Count;

        DataTable dtStudentList = CreateStudentListTable();
        Dictionary<string, QuestionStats> questionStatsDict = new Dictionary<string, QuestionStats>();
        List<object> studentJsonList = new List<object>();

        if (Persons > 0)
        {
            int allscore = 0;
            int allspent = 0;

            for (int i = 0; i < Persons; i++)
            {
                int score = int.Parse(dt.Rows[i]["Ascore"].ToString());
                int spent = int.Parse(dt.Rows[i]["Aspent"].ToString());
                string name = HttpUtility.UrlDecode(dt.Rows[i]["Asname"].ToString());
                string num = dt.Rows[i]["Asnum"].ToString();
                string adata = dt.Rows[i]["Adata"].ToString();

                allscore += score;
                allspent += spent;

                int wrongCount = 0;
                List<object> answerDetails = new List<object>();
                int totalQuestions = 0;
                int correctQuestions = 0;

                if (!string.IsNullOrEmpty(adata))
                {
                    try
                    {
                        JObject jsonObj = JObject.Parse(adata);
                        JArray answersArray = (JArray)jsonObj["answers"];

                        if (answersArray != null)
                        {
                            foreach (JObject answer in answersArray)
                            {
                                totalQuestions++;
                                string questionId = answer["questionId"].ToString();
                                string questionTitle = answer["questionTitle"] != null ? answer["questionTitle"].ToString() : "";
                                string questionType = answer["questionType"] != null ? answer["questionType"].ToString() : "";
                                bool isCorrect = GetIsCorrect(answer);

                                if (!isCorrect) wrongCount++;

                                string studentAnswer = GetStudentAnswer(answer, questionType);
                                string correctAnswer = GetCorrectAnswer(answer, questionType);

                                answerDetails.Add(new
                                {
                                    questionId = questionId,
                                    title = StripHtml(questionTitle),
                                    typeText = GetQuestionTypeText(questionType),
                                    isCorrect = isCorrect,
                                    studentAnswer = studentAnswer,
                                    correctAnswer = correctAnswer
                                });

                                if (isCorrect) correctQuestions++;

                                if (!questionStatsDict.ContainsKey(questionId))
                                {
                                    questionStatsDict[questionId] = new QuestionStats
                                    {
                                        QuestionId = questionId,
                                        QuestionTitle = questionTitle,
                                        QuestionType = questionType,
                                        QuestionTypeText = GetQuestionTypeText(questionType),
                                        CorrectCount = 0,
                                        TotalAttempts = 0,
                                        WrongAnswers = new List<string>()
                                    };
                                }

                                var stats = questionStatsDict[questionId];
                                stats.TotalAttempts++;
                                if (isCorrect)
                                {
                                    stats.CorrectCount++;
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(studentAnswer))
                                    {
                                        stats.WrongAnswers.Add(studentAnswer);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                DataRow row = dtStudentList.NewRow();
                row["Asname"] = name;
                row["Asnum"] = num;
                row["Ascore"] = score;
                row["Aspent"] = spent;
                row["WrongCount"] = wrongCount;
                dtStudentList.Rows.Add(row);

                string level = GetScoreLevel(score);
                double accuracy = totalQuestions > 0 ? Math.Round((double)correctQuestions / totalQuestions * 100, 1) : 0;

                studentJsonList.Add(new
                {
                    name = name,
                    num = num,
                    score = score,
                    spent = spent,
                    level = level,
                    accuracy = accuracy,
                    wrongCount = wrongCount,
                    answers = answerDetails
                });

                if (score >= 90) ExcellentCount++;
                else if (score >= 80) GoodCount++;
                else if (score >= 60) PassCount++;
                else FailCount++;
            }

            avgScore = Persons > 0 ? allscore / Persons : 0;
            avgSpent = Persons > 0 ? allspent / Persons : 0;
        }

        RepeaterList.DataSource = dtStudentList;
        RepeaterList.DataBind();

        DataTable dtAnalysis = CreateAnalysisTable();
        DataTable dtHighError = CreateHighErrorTable();
        bool hasHighError = false;

        foreach (var stats in questionStatsDict.Values)
        {
            double accuracy = stats.TotalAttempts > 0 ? Math.Round((double)stats.CorrectCount / stats.TotalAttempts * 100, 2) : 0;
            double errorRate = Math.Round(100 - accuracy, 2);
            int wrongCount = stats.TotalAttempts - stats.CorrectCount;

            DataRow row = dtAnalysis.NewRow();
            row["QuestionId"] = stats.QuestionId;
            row["QuestionTitle"] = StripHtml(stats.QuestionTitle);
            row["QuestionType"] = stats.QuestionType;
            row["QuestionTypeText"] = stats.QuestionTypeText;
            row["CorrectCount"] = stats.CorrectCount;
            row["WrongCount"] = wrongCount;
            row["TotalAttempts"] = stats.TotalAttempts;
            row["Accuracy"] = accuracy;
            row["ErrorRate"] = errorRate;
            dtAnalysis.Rows.Add(row);

            if (errorRate >= 50)
            {
                hasHighError = true;
                string commonWrong = GetMostCommonWrongAnswer(stats.WrongAnswers);
                DataRow highRow = dtHighError.NewRow();
                highRow["QuestionTypeText"] = stats.QuestionTypeText;
                highRow["QuestionTitle"] = StripHtml(stats.QuestionTitle);
                highRow["ErrorRate"] = errorRate;
                highRow["CommonWrongAnswer"] = commonWrong;
                dtHighError.Rows.Add(highRow);
            }
        }

        dtAnalysis.DefaultView.Sort = "ErrorRate DESC";
        RepeaterAnalysis.DataSource = dtAnalysis.DefaultView.ToTable();
        RepeaterAnalysis.DataBind();

        dtHighError.DefaultView.Sort = "ErrorRate DESC";
        RepeaterHighError.DataSource = dtHighError.DefaultView.ToTable();
        RepeaterHighError.DataBind();

        LabelNoHighError.Visible = !hasHighError;
        LabelNoAbsent.Visible = (NoPersons == 0);

        StudentJsonData = JsonConvert.SerializeObject(studentJsonList);

        PageStatus = "最近刷新：" + DateTime.Now.ToString("HH:mm:ss") + 
                     "，班级共 " + TotalStudents + " 人，已参与 " + Persons + " 人，未参与 " + NoPersons + " 人";
    }

    private DataTable CreateStudentListTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Asname", typeof(string));
        dt.Columns.Add("Asnum", typeof(string));
        dt.Columns.Add("Ascore", typeof(int));
        dt.Columns.Add("Aspent", typeof(int));
        dt.Columns.Add("WrongCount", typeof(int));
        return dt;
    }

    private DataTable CreateAnalysisTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("QuestionId", typeof(string));
        dt.Columns.Add("QuestionTitle", typeof(string));
        dt.Columns.Add("QuestionType", typeof(string));
        dt.Columns.Add("QuestionTypeText", typeof(string));
        dt.Columns.Add("CorrectCount", typeof(int));
        dt.Columns.Add("WrongCount", typeof(int));
        dt.Columns.Add("TotalAttempts", typeof(int));
        dt.Columns.Add("Accuracy", typeof(double));
        dt.Columns.Add("ErrorRate", typeof(double));
        return dt;
    }

    private DataTable CreateHighErrorTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("QuestionTypeText", typeof(string));
        dt.Columns.Add("QuestionTitle", typeof(string));
        dt.Columns.Add("ErrorRate", typeof(double));
        dt.Columns.Add("CommonWrongAnswer", typeof(string));
        return dt;
    }

    private bool GetIsCorrect(JObject answer)
    {
        JToken isCorrectToken = answer["isCorrect"];
        if (isCorrectToken == null) return false;

        if (isCorrectToken.Type == JTokenType.Boolean)
        {
            return (bool)isCorrectToken;
        }
        else if (isCorrectToken.ToString() != "")
        {
            bool result;
            bool.TryParse(isCorrectToken.ToString(), out result);
            return result;
        }
        return false;
    }

    private string GetStudentAnswer(JObject answer, string questionType)
    {
        try
        {
            JToken userAnswer = answer["userAnswer"];
            if (userAnswer == null) return "";

            if (questionType == "single_choice" || questionType == "true_false")
            {
                return userAnswer.ToString();
            }
            else if (questionType == "multiple_choice")
            {
                if (userAnswer.Type == JTokenType.Array)
                {
                    return string.Join(", ", userAnswer.ToObject<string[]>());
                }
                return userAnswer.ToString();
            }
            else if (questionType == "fill_blank")
            {
                if (userAnswer.Type == JTokenType.Array)
                {
                    var arr = userAnswer.ToObject<string[]>();
                    return string.Join(" | ", arr);
                }
                return userAnswer.ToString();
            }
            else if (questionType == "matching")
            {
                return "连线题";
            }
            else if (questionType == "sort_question")
            {
                return "排序题";
            }
            return userAnswer.ToString();
        }
        catch
        {
            return "";
        }
    }

    private string GetCorrectAnswer(JObject answer, string questionType)
    {
        try
        {
            JToken correctAns = answer["correctAnswer"];
            if (correctAns == null)
            {
                JToken answerToken = answer["answer"];
                if (answerToken != null)
                {
                    if (questionType == "multiple_choice" && answerToken.Type == JTokenType.Array)
                    {
                        return string.Join(", ", answerToken.ToObject<string[]>());
                    }
                    return answerToken.ToString();
                }
                return "";
            }
            return correctAns.ToString();
        }
        catch
        {
            return "";
        }
    }

    private string GetMostCommonWrongAnswer(List<string> wrongAnswers)
    {
        if (wrongAnswers == null || wrongAnswers.Count == 0)
            return "-";

        Dictionary<string, int> counts = new Dictionary<string, int>();
        foreach (var ans in wrongAnswers)
        {
            if (string.IsNullOrEmpty(ans)) continue;
            string key = ans.Length > 30 ? ans.Substring(0, 30) + "..." : ans;
            if (counts.ContainsKey(key))
                counts[key]++;
            else
                counts[key] = 1;
        }

        string mostCommon = "";
        int maxCount = 0;
        foreach (var kvp in counts)
        {
            if (kvp.Value > maxCount)
            {
                maxCount = kvp.Value;
                mostCommon = kvp.Key;
            }
        }

        return string.IsNullOrEmpty(mostCommon) ? "-" : mostCommon + " (" + maxCount + "人)";
    }

    private string StripHtml(string html)
    {
        if (string.IsNullOrEmpty(html)) return "";
        string result = System.Text.RegularExpressions.Regex.Replace(html, "<.*?>", "");
        result = System.Text.RegularExpressions.Regex.Replace(result, "&nbsp;", " ");
        result = System.Text.RegularExpressions.Regex.Replace(result, "&lt;", "<");
        result = System.Text.RegularExpressions.Regex.Replace(result, "&gt;", ">");
        result = System.Text.RegularExpressions.Regex.Replace(result, "&amp;", "&");
        if (result.Length > 80)
            result = result.Substring(0, 80) + "...";
        return result.Trim();
    }

    protected string GetShortTitle(string title)
    {
        if (string.IsNullOrEmpty(title)) return "";
        string result = StripHtml(title);
        if (result.Length > 60)
            return result.Substring(0, 60) + "...";
        return result;
    }

    protected string GetScoreClass(object scoreObj)
    {
        int score = 0;
        int.TryParse(scoreObj.ToString(), out score);
        if (score >= 90) return "excellent";
        if (score >= 80) return "good";
        if (score >= 60) return "pass";
        return "fail";
    }

    protected string GetScoreLevel(object scoreObj)
    {
        int score = 0;
        int.TryParse(scoreObj.ToString(), out score);
        if (score >= 90) return "优秀";
        if (score >= 80) return "良好";
        if (score >= 60) return "及格";
        return "不及格";
    }

    protected string GetAccuracyClass(object accuracyObj)
    {
        double accuracy = 0;
        double.TryParse(accuracyObj.ToString(), out accuracy);
        if (accuracy >= 70) return "high";
        if (accuracy >= 40) return "medium";
        return "low";
    }

    protected string GetBarHeight(int count)
    {
        if (Persons == 0) return "0";
        int height = (int)Math.Round((double)count / Persons * 100);
        return Math.Max(height, 5).ToString();
    }

    private string GetQuestionTypeText(string type)
    {
        if (string.IsNullOrEmpty(type)) return "未知题型";

        switch (type.ToLower())
        {
            case "single_choice":
                return "单选题";
            case "multiple_choice":
                return "多选题";
            case "fill_blank":
                return "填空题";
            case "true_false":
                return "判断题";
            case "matching":
                return "连线题";
            case "sort_question":
                return "排序题";
            case "table_question":
                return "表格题";
            case "short_answer":
                return "简答题";
            default:
                return "未知题型";
        }
    }

    private class QuestionStats
    {
        public string QuestionId { get; set; }
        public string QuestionTitle { get; set; }
        public string QuestionType { get; set; }
        public string QuestionTypeText { get; set; }
        public int CorrectCount { get; set; }
        public int TotalAttempts { get; set; }
        public List<string> WrongAnswers { get; set; }
    }
}
