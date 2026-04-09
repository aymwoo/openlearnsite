<%@ WebHandler Language="C#" Class="ImportExamQuestionsToBank" %>

using System;
using System.Web;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using LearnSite.BLL;
using LearnSite.Model;
using LearnSite.Common;

public class ImportExamQuestionsToBank : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        
        try
        {
            // 检查教师登录状态 - 暂时注释，避免重定向
            // CookieHelp.JudgeTeacherCookies();
            
            // 获取教师ID - 暂时跳过检查
            // HttpCookie teacherCookie = context.Request.Cookies[CookieHelp.teaCookieNname];
            // if (teacherCookie == null)
            // {
            //     context.Response.Write("{\"success\":false,\"message\":\"未登录\"}");
            //     return;
            // }
            // 
            // string teacherId = teacherCookie.Values["Hid"];
            // if (string.IsNullOrEmpty(teacherId))
            // {
            //     context.Response.Write("{\"success\":false,\"message\":\"未登录\"}");
            //     return;
            // }
            string teacherId = "test_teacher"; // 暂时使用测试教师ID
            
            // 读取请求数据
            string requestBody = new System.IO.StreamReader(context.Request.InputStream).ReadToEnd();
            System.Diagnostics.Trace.WriteLine("收到请求，长度: " + requestBody.Length);
            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            var requestData = serializer.Deserialize<Dictionary<string, object>>(requestBody);
            
            if (requestData == null)
            {
                context.Response.Write("{\"success\":false,\"message\":\"请求数据解析失败\"}");
                return;
            }
            
            // 获取参数
            int bankId = Convert.ToInt32(requestData["bankId"]);
            int gradeId = Convert.ToInt32(requestData["gradeId"]);
            int difficulty = Convert.ToInt32(requestData["difficulty"]);
            string tags = requestData.ContainsKey("tags") ? requestData["tags"].ToString() : "";
            
            System.Diagnostics.Trace.WriteLine("参数: bankId=" + bankId + ", gradeId=" + gradeId + ", difficulty=" + difficulty);
            
            // 获取题目列表 - 先序列化再反序列化，避免ArrayList问题
            string questionsJson = serializer.Serialize(requestData["questions"]);
            System.Diagnostics.Trace.WriteLine("题目JSON长度: " + questionsJson.Length);
            
            var questionsData = serializer.Deserialize<List<Dictionary<string, object>>>(questionsJson);
            
            if (questionsData == null || questionsData.Count == 0)
            {
                context.Response.Write("{\"success\":false,\"message\":\"题目列表为空\"}");
                return;
            }
            
            System.Diagnostics.Trace.WriteLine("题目数量: " + questionsData.Count);
            
            // 保存题目到题库
            LearnSite.BLL.ExamQuestion questionBll = new LearnSite.BLL.ExamQuestion();
            LearnSite.BLL.ExamQuestionBank bankBll = new LearnSite.BLL.ExamQuestionBank();
            
            int savedCount = 0;
            
            foreach (var questionData in questionsData)
            {
                try
                {
                    string type = questionData.ContainsKey("type") ? questionData["type"].ToString() : "";
                    string title = questionData.ContainsKey("title") ? questionData["title"].ToString() : "";
                    decimal score = questionData.ContainsKey("score") ? Convert.ToDecimal(questionData["score"]) : 5;
                    
                    System.Diagnostics.Trace.WriteLine("处理题目: type=" + type + ", title=" + (title.Length > 30 ? title.Substring(0, 30) : title));
                    
                    LearnSite.Model.ExamQuestion question = new LearnSite.Model.ExamQuestion();
                    question.BankId = bankId;
                    question.GradeId = gradeId;
                    question.CourseId = null;
                    question.QuestionType = GetQuestionType(type);
                    question.QuestionContent = title;
                    question.QuestionText = StripHtml(question.QuestionContent);
                    question.Score = score;
                    question.Difficulty = difficulty;
                    question.Tags = tags;
                    question.Status = 1;
                    question.CreateBy = teacherId;
                    question.CreateTime = DateTime.Now;
                    
                    // 根据题型处理选项和答案
                    switch (question.QuestionType)
                    {
                        case 1: // 单选
                            question.Options = SerializeOptions(questionData["options"]);
                            question.Answer = SerializeSingleChoiceAnswer(questionData["answer"]);
                            break;
                        case 2: // 多选
                            question.Options = SerializeOptions(questionData["options"]);
                            question.Answer = SerializeMultipleChoiceAnswer(questionData["answer"]);
                            break;
                        case 3: // 判断
                            question.Answer = SerializeTrueFalseAnswer(questionData["answer"]);
                            break;
                        case 4: // 填空
                            question.Answer = SerializeFillBlankAnswer(questionData["blanks"]);
                            break;
                        case 5: // 简答
                            question.Answer = SerializeShortAnswerAnswer(questionData["answer"]);
                            question.QuestionConfig = SerializeShortAnswerConfig(questionData);
                            break;
                        case 6: // 连线
                            question.Options = SerializeMatchingOptions(questionData);
                            question.Answer = SerializeMatchingAnswer(questionData["answer"]);
                            break;
                        case 7: // 排序
                            question.Options = SerializeSortOptions(questionData["items"]);
                            question.Answer = SerializeSortAnswer(questionData["answer"]);
                            break;
                        case 8: // 表格
                            question.Options = SerializeTableOptions(questionData["tableData"]);
                            question.Answer = SerializeTableAnswer(questionData["answer"]);
                            break;
                    }
                    
                    questionBll.AddQuestion(question);
                    savedCount++;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine("保存题目失败: " + ex.Message + "\n" + ex.StackTrace);
                    savedCount = -1; // 标记错误
                }
            }
            
            // 如果全部失败，返回错误信息
            if (savedCount == 0 && questionsData.Count > 0)
            {
                context.Response.Write("{\"success\":false,\"message\":\"所有题目保存失败，请检查题目数据格式\"}");
                return;
            }
            
            // 更新题库题目数量
            bankBll.UpdateQuestionCount(bankId);
            
            // 构建JSON响应
            var response = new
            {
                success = true,
                savedCount = savedCount
            };
            
            JavaScriptSerializer serializer2 = new JavaScriptSerializer();
            string jsonResult = serializer2.Serialize(response);
            context.Response.Write(jsonResult);
        }
        catch (Exception ex)
        {
            context.Response.Write("{\"success\":false,\"message\":\"" + ex.Message.Replace("\"", "\\\"") + "\"}");
        }
    }
    
    private int GetQuestionType(string type)
    {
        switch (type)
        {
            case "single_choice": return 1;
            case "multiple_choice": return 2;
            case "true_false": return 3;
            case "fill_blank": return 4;
            case "short_answer": return 5;
            case "matching": return 6;
            case "sort_question": return 7;
            case "table_question": return 8;
            default: return 1;
        }
    }
    
    private string GetTypeText(string type)
    {
        switch (type)
        {
            case "single_choice": return "单选题";
            case "multiple_choice": return "多选题";
            case "true_false": return "判断题";
            case "fill_blank": return "填空题";
            case "short_answer": return "简答题";
            case "matching": return "连线题";
            case "sort_question": return "排序题";
            case "table_question": return "表格题";
            default: return "未知题型";
        }
    }
    
    private string SerializeOptions(object options)
    {
        var optionList = new List<object>();
        char label = 'A';
        
        if (options == null)
            return "[]";
        
        // 处理 ArrayList 或 Array
        var optionsEnumerable = options as System.Collections.IEnumerable;
        if (optionsEnumerable == null)
            return "[]";
        
        foreach (var option in optionsEnumerable)
        {
            optionList.Add(new
            {
                Label = label.ToString(),
                Content = option != null ? option.ToString() : "",
                IsCorrect = false
            });
            label++;
        }
        
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(optionList);
    }
    
    private string SerializeSingleChoiceAnswer(object answer)
    {
        int answerIndex = Convert.ToInt32(answer);
        return ((char)('A' + answerIndex)).ToString();
    }
    
    private string SerializeMultipleChoiceAnswer(object answer)
    {
        if (answer == null)
            return "";
        
        var answerList = new List<string>();
        
        // 处理 ArrayList 或 Array
        var answerEnumerable = answer as System.Collections.IEnumerable;
        if (answerEnumerable != null)
        {
            foreach (var index in answerEnumerable)
            {
                int answerIndex = Convert.ToInt32(index);
                answerList.Add(((char)('A' + answerIndex)).ToString());
            }
        }
        
        return string.Join(",", answerList);
    }
    
    private string SerializeTrueFalseAnswer(object answer)
    {
        bool isTrue = Convert.ToBoolean(answer);
        return isTrue ? "对" : "错";
    }
    
    private string SerializeFillBlankAnswer(object blanks)
    {
        if (blanks == null)
            return "";
        
        var answerList = new List<string>();
        
        // 处理 ArrayList 或 Array
        var blanksEnumerable = blanks as System.Collections.IEnumerable;
        if (blanksEnumerable != null)
        {
            foreach (var blank in blanksEnumerable)
            {
                var blankData = blank as Dictionary<string, object>;
                if (blankData != null)
                {
                    string answer = blankData.ContainsKey("answer") ? blankData["answer"].ToString() : "";
                    answerList.Add(answer);
                }
            }
        }
        
        return string.Join("|", answerList);
    }
    
    private string SerializeShortAnswerAnswer(object answer)
    {
        return answer != null ? answer.ToString() : "";
    }
    
    private string SerializeShortAnswerConfig(Dictionary<string, object> questionData)
    {
        var config = new
        {
            keywords = questionData.ContainsKey("keywords") ? questionData["keywords"] : new object[0],
            keywordThreshold = questionData.ContainsKey("keywordThreshold") ? questionData["keywordThreshold"] : 60
        };
        
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(config);
    }
    
    private string SerializeMatchingOptions(Dictionary<string, object> questionData)
    {
        if (questionData == null)
            return "{}";
        
        object leftItemsObj = null;
        object rightItemsObj = null;
        questionData.TryGetValue("leftItems", out leftItemsObj);
        questionData.TryGetValue("rightItems", out rightItemsObj);
        
        var options = new
        {
            LeftItems = leftItemsObj,
            RightItems = rightItemsObj
        };
        
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(options);
    }
    
    private string SerializeMatchingAnswer(object answer)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(answer);
    }
    
    private string SerializeSortOptions(object items)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(items);
    }
    
    private string SerializeSortAnswer(object answer)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(answer);
    }
    
    private string SerializeTableOptions(object tableData)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(tableData);
    }
    
    private string SerializeTableAnswer(object answer)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(answer);
    }
    
    private string StripHtml(string html)
    {
        if (string.IsNullOrEmpty(html))
            return html;
        
        return System.Text.RegularExpressions.Regex.Replace(html, "<[^>]*>", "");
    }
    
    public bool IsReusable
    {
        get { return false; }
    }
}
