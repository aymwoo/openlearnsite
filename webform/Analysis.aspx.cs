using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;

public partial class webform_Analysis : System.Web.UI.Page
{
    protected int Persons = 0;
    protected int avgScore = 0;
    protected int avgSpent = 0;
    protected int NoPersons = 0;
    protected string PageStatus = string.Empty;

    LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.KickStudent();
        if (!IsPostBack)
        {
            showAnswers();
        }

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
        RepeaterList.DataSource = dt;
        RepeaterList.DataBind();
        Persons = dt.Rows.Count;

        DataTable dtNo = abll.GetListClassSname(Eid, cook.Sgrade, cook.Sclass);//获得未参加班级测验的学生列表
        RepeaterNo.DataSource = dtNo;
        RepeaterNo.DataBind();
        NoPersons = dtNo.Rows.Count;

        // 创建一个DataTable来存储题目分析结果
        DataTable dtAnalysis = new DataTable();
        dtAnalysis.Columns.Add("QuestionId", typeof(string));
        dtAnalysis.Columns.Add("QuestionTitle", typeof(string));
        dtAnalysis.Columns.Add("QuestionType", typeof(string));
        dtAnalysis.Columns.Add("QuestionTypeText", typeof(string)); // 新增：存中文类型
        dtAnalysis.Columns.Add("CorrectCount", typeof(int));
        dtAnalysis.Columns.Add("TotalAttempts", typeof(int));
        dtAnalysis.Columns.Add("Accuracy", typeof(double));

        if (Persons > 0)
        {
            int allscore = 0;
            int allspent = 0;
            for (int i = 0; i < Persons; i++)
            {
                allscore += int.Parse(dt.Rows[i]["Ascore"].ToString());
                allspent += int.Parse(dt.Rows[i]["Aspent"].ToString());
                string Adata = dt.Rows[i]["Adata"].ToString();

                if (!string.IsNullOrEmpty(Adata))
                {
                    try
                    {
                        JObject jsonObj = JObject.Parse(Adata);
                        JArray answersArray = (JArray)jsonObj["answers"];

                        if (answersArray != null)
                        {
                            foreach (JObject answer in answersArray)
                            {
                                string questionId = answer["questionId"].ToString();
                                string questionTitle = answer["questionTitle"].ToString();
                                string questionType = answer["questionType"].ToString();
                                bool isCorrect = false;

                                // 处理isCorrect字段
                                JToken isCorrectToken = answer["isCorrect"];
                                if (isCorrectToken.Type == JTokenType.Boolean)
                                {
                                    isCorrect = (bool)isCorrectToken;
                                }
                                else if (isCorrectToken != null && isCorrectToken.ToString() != "")
                                {
                                    bool.TryParse(isCorrectToken.ToString(), out isCorrect);
                                }
                                
                                // 获取题型中文名
                                string typeText = GetQuestionTypeText(questionType);

                                // 查找是否已存在该题目的记录
                                DataRow[] existingRows = dtAnalysis.Select("QuestionId = '" + questionId.Replace("'", "''") + "'");
                                DataRow row;

                                if (existingRows.Length > 0)
                                {
                                    // 已存在，更新统计
                                    row = existingRows[0];
                                }
                                else
                                {
                                    // 不存在，创建新行
                                    row = dtAnalysis.NewRow();
                                    row["QuestionId"] = questionId;
                                    row["QuestionTitle"] = questionTitle;  // 使用清理后的标题
                                    row["QuestionType"] = questionType;      // 英文类型
                                    row["QuestionTypeText"] = typeText;     // 中文类型
                                    row["CorrectCount"] = 0;
                                    row["TotalAttempts"] = 0;
                                    dtAnalysis.Rows.Add(row);
                                }

                                // 更新统计信息
                                row["TotalAttempts"] = (int)row["TotalAttempts"] + 1;
                                if (isCorrect)
                                {
                                    row["CorrectCount"] = (int)row["CorrectCount"] + 1;
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // 忽略解析错误，继续处理下一个学生
                    }
                }
            }

            // 计算正确率
            foreach (DataRow row in dtAnalysis.Rows)
            {
                int totalAttempts = (int)row["TotalAttempts"];
                int correctCount = (int)row["CorrectCount"];

                if (totalAttempts > 0)
                {
                    double accuracy = Math.Round((double)correctCount / totalAttempts * 100, 2);
                    row["Accuracy"] = accuracy;
                }
            }
            avgScore = allscore / Persons;
            avgSpent = allspent / Persons;

            // 按正确率排序
            dtAnalysis.DefaultView.Sort = "Accuracy ASC";
            dtAnalysis = dtAnalysis.DefaultView.ToTable();
        }

        RepeaterAnalysis.DataSource = dtAnalysis;
        RepeaterAnalysis.DataBind();
        PageStatus = "最近刷新：" + DateTime.Now.ToString("HH:mm:ss") + "，已参与 " + Persons.ToString() + " 人，未参与 " + NoPersons.ToString() + " 人";
    }

    // 直接使用switch-case，不需要字典初始化
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

}
