<%@ WebHandler Language="C#" Class="QuestionBankApi" %>

using System;
using System.Web;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using LearnSite.DBUtility;

public class QuestionBankApi : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.Charset = "utf-8";

        string action = context.Request["action"] ?? "";
        try
        {
            if (!DbHelperSQL.TabExists("ExamQuestionBank") || !DbHelperSQL.TabExists("ExamQuestion"))
            {
                context.Response.Write(JsonConvert.SerializeObject(new { 
                    success = false, 
                    needUpgrade = true,
                    message = "题库表尚未创建，请先访问升级页面创建题库表" 
                }));
                return;
            }

            switch (action)
            {
                case "getBanks":
                    GetBanks(context);
                    break;
                case "getQuestions":
                    GetQuestions(context);
                    break;
                case "importQuestions":
                    ImportQuestions(context);
                    break;
                default:
                    context.Response.Write(JsonConvert.SerializeObject(new { success = false, message = "未知操作" }));
                    break;
            }
        }
        catch (Exception ex)
        {
            context.Response.Write(JsonConvert.SerializeObject(new { success = false, message = ex.Message }));
        }
    }

    private void GetBanks(HttpContext context)
    {
        LearnSite.DAL.ExamQuestionBank bankDal = new LearnSite.DAL.ExamQuestionBank();
        List<LearnSite.Model.ExamQuestionBank> banks = bankDal.GetBankList();
        
        var result = new List<object>();
        foreach (var bank in banks)
        {
            result.Add(new
            {
                bankId = bank.BankId,
                bankName = bank.BankName,
                questionCount = bank.QuestionCount
            });
        }
        
        context.Response.Write(JsonConvert.SerializeObject(new { success = true, data = result }));
    }

    private void GetQuestions(HttpContext context)
    {
        int bankId = 0;
        int.TryParse(context.Request["bankId"], out bankId);
        
        int pageIndex = 1;
        int.TryParse(context.Request["pageIndex"], out pageIndex);
        if (pageIndex < 1) pageIndex = 1;
        
        int pageSize = 20;
        int.TryParse(context.Request["pageSize"], out pageSize);
        if (pageSize < 1) pageSize = 20;
        
        int? questionType = null;
        string qtStr = context.Request["questionType"];
        if (!string.IsNullOrEmpty(qtStr) && qtStr != "0")
        {
            int qt;
            if (int.TryParse(qtStr, out qt))
                questionType = qt;
        }
        
        int? difficulty = null;
        string diffStr = context.Request["difficulty"];
        if (!string.IsNullOrEmpty(diffStr) && diffStr != "0")
        {
            int diff;
            if (int.TryParse(diffStr, out diff))
                difficulty = diff;
        }
        
        string keyword = context.Request["keyword"];
        if (string.IsNullOrWhiteSpace(keyword)) keyword = null;

        LearnSite.DAL.ExamQuestion questionDal = new LearnSite.DAL.ExamQuestion();
        var tuple = questionDal.GetQuestionList(bankId, pageIndex, pageSize, questionType, difficulty, keyword);
        var questions = tuple.Item1;
        int total = tuple.Item2;

        var result = new List<object>();
        foreach (var q in questions)
        {
            result.Add(new
            {
                questionId = q.QuestionId,
                questionType = q.QuestionType,
                questionTypeName = GetQuestionTypeName(q.QuestionType),
                questionText = q.QuestionText,
                questionContent = q.QuestionContent,
                options = q.Options,
                answer = q.Answer,
                score = q.Score,
                difficulty = q.Difficulty,
                difficultyName = GetDifficultyName(q.Difficulty),
                knowledgePoint = q.KnowledgePoint
            });
        }

        context.Response.Write(JsonConvert.SerializeObject(new
        {
            success = true,
            data = result,
            total = total,
            pageIndex = pageIndex,
            pageSize = pageSize,
            totalPages = (int)Math.Ceiling((double)total / pageSize)
        }));
    }

    private void ImportQuestions(HttpContext context)
    {
        string questionIdsStr = context.Request["questionIds"] ?? "";
        if (string.IsNullOrEmpty(questionIdsStr))
        {
            context.Response.Write(JsonConvert.SerializeObject(new { success = false, message = "请选择要导入的题目" }));
            return;
        }

        string[] idArray = questionIdsStr.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        List<long> questionIds = new List<long>();
        foreach (string idStr in idArray)
        {
            long id;
            if (long.TryParse(idStr.Trim(), out id))
                questionIds.Add(id);
        }

        if (questionIds.Count == 0)
        {
            context.Response.Write(JsonConvert.SerializeObject(new { success = false, message = "无效的题目ID" }));
            return;
        }

        LearnSite.DAL.ExamQuestion questionDal = new LearnSite.DAL.ExamQuestion();
        var result = new List<object>();

        foreach (long qid in questionIds)
        {
            var q = questionDal.GetQuestionById(qid);
            if (q != null)
            {
                var examQuestion = ConvertToExamQuestion(q);
                result.Add(examQuestion);
            }
        }

        context.Response.Write(JsonConvert.SerializeObject(new { success = true, data = result }));
    }

    private object ConvertToExamQuestion(LearnSite.Model.ExamQuestion q)
    {
        string questionType = MapQuestionType(q.QuestionType);
        var options = ParseOptions(q.Options, q.QuestionType);
        var answer = ParseAnswer(q.Answer, q.QuestionType);

        return new
        {
            questionId = q.QuestionId.ToString(),
            questionType = questionType,
            questionTitle = q.QuestionText ?? q.QuestionContent ?? "",
            options = options,
            answer = answer,
            score = (int)q.Score,
            analysis = q.Analysis ?? "",
            difficulty = q.Difficulty
        };
    }

    private string MapQuestionType(int dbType)
    {
        switch (dbType)
        {
            case 1: return "single_choice";
            case 2: return "multiple_choice";
            case 3: return "true_false";
            case 4: return "fill_blank";
            case 5: return "short_answer";
            case 6: return "matching";
            case 7: return "sort_question";
            default: return "single_choice";
        }
    }

    private object ParseOptions(string optionsJson, int questionType)
    {
        if (string.IsNullOrEmpty(optionsJson))
            return new List<object>();

        try
        {
            switch (questionType)
            {
                case 1:
                case 2:
                    JArray opts = JArray.Parse(optionsJson);
                    var result = new List<object>();
                    foreach (JToken token in opts)
                    {
                        JObject opt = token as JObject;
                        if (opt != null)
                        {
                            string label = opt["Label"] != null ? opt["Label"].ToString() : "";
                            string content = opt["Content"] != null ? opt["Content"].ToString() : "";
                            bool isCorrect = opt["IsCorrect"] != null && Convert.ToBoolean(opt["IsCorrect"]);
                            result.Add(new { label = label, content = content, isCorrect = isCorrect });
                        }
                    }
                    return result;
                case 3:
                    return new List<object>
                    {
                        new { label = "A", content = "对", isCorrect = optionsJson.Trim() == "对" },
                        new { label = "B", content = "错", isCorrect = optionsJson.Trim() == "错" }
                    };
                case 4:
                    return optionsJson;
                default:
                    return optionsJson;
            }
        }
        catch
        {
            return optionsJson;
        }
    }

    private object ParseAnswer(string answerJson, int questionType)
    {
        if (string.IsNullOrEmpty(answerJson))
            return "";

        try
        {
            switch (questionType)
            {
                case 1:
                case 3:
                    return answerJson.Trim();
                case 2:
                    return answerJson.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                case 4:
                    return answerJson.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                default:
                    return answerJson;
            }
        }
        catch
        {
            return answerJson;
        }
    }

    private string GetQuestionTypeName(int questionType)
    {
        switch (questionType)
        {
            case 1: return "单选题";
            case 2: return "多选题";
            case 3: return "判断题";
            case 4: return "填空题";
            case 5: return "简答题";
            case 6: return "连线题";
            case 7: return "排序题";
            default: return "未知题型";
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

    public bool IsReusable
    {
        get { return false; }
    }
}
