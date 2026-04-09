<%@ WebHandler Language="C#" Class="GetExamQuestions" %>

using System;
using System.Web;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using LearnSite.BLL;

public class GetExamQuestions : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        
        try
        {
            string eid = context.Request.QueryString["eid"];
            if (string.IsNullOrEmpty(eid))
            {
                context.Response.Write("{\"success\":false,\"message\":\"缺少参数\"}");
                return;
            }
            
            LearnSite.BLL.Exams examsBll = new LearnSite.BLL.Exams();
            var exam = examsBll.GetModel(int.Parse(eid));
            
            if (exam == null)
            {
                context.Response.Write("{\"success\":false,\"message\":\"课堂测验不存在\"}");
                return;
            }
            
            List<object> questions = new List<object>();
            
            if (!string.IsNullOrEmpty(exam.Edata))
            {
                try
                {
                    string jsonStr = exam.Edata;
                    System.Diagnostics.Trace.WriteLine("原始数据前100字符: " + (jsonStr.Length > 100 ? jsonStr.Substring(0, 100) : jsonStr));
                    
                    // 多次URL解码，直到得到有效的JSON
                    string decoded = jsonStr;
                    for (int i = 0; i < 5; i++)
                    {
                        try
                        {
                            string temp = HttpUtility.UrlDecode(decoded);
                            System.Diagnostics.Trace.WriteLine("第" + (i + 1) + "次URL解码后前50字符: " + (temp.Length > 50 ? temp.Substring(0, 50) : temp));
                            if (temp == decoded || temp.StartsWith("{") || temp.StartsWith("["))
                            {
                                decoded = temp;
                                break;
                            }
                            decoded = temp;
                        }
                        catch
                        {
                            break;
                        }
                    }
                    
                    System.Diagnostics.Trace.WriteLine("最终解码后前100字符: " + (decoded.Length > 100 ? decoded.Substring(0, 100) : decoded));
                    
                    // 尝试Base64解码
                    try
                    {
                        byte[] data = Convert.FromBase64String(decoded);
                        decoded = System.Text.Encoding.UTF8.GetString(data);
                        System.Diagnostics.Trace.WriteLine("Base64解码后前100字符: " + (decoded.Length > 100 ? decoded.Substring(0, 100) : decoded));
                        
                        // Base64解码后可能还需要URL解码
                        if (decoded.Contains("%"))
                        {
                            string urlDecoded = HttpUtility.UrlDecode(decoded);
                            if (urlDecoded.StartsWith("{") || urlDecoded.StartsWith("["))
                            {
                                decoded = urlDecoded;
                                System.Diagnostics.Trace.WriteLine("Base64后URL解码前100字符: " + (decoded.Length > 100 ? decoded.Substring(0, 100) : decoded));
                            }
                        }
                    }
                    catch { }
                    
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    serializer.MaxJsonLength = Int32.MaxValue;
                    
                    var examData = serializer.Deserialize<Dictionary<string, object>>(decoded);
                    
                    if (examData != null && examData.ContainsKey("questions"))
                    {
                        string questionsJson = serializer.Serialize(examData["questions"]);
                        var questionList = serializer.Deserialize<List<Dictionary<string, object>>>(questionsJson);
                        
                        foreach (var question in questionList)
                        {
                            string type = question.ContainsKey("type") ? question["type"].ToString() : "";
                            string title = question.ContainsKey("title") ? question["title"].ToString() : "";
                            decimal score = 5;
                            try { score = question.ContainsKey("score") ? Convert.ToDecimal(question["score"]) : 5; } catch { }
                            
                            questions.Add(new
                            {
                                id = question.ContainsKey("id") ? question["id"] : "",
                                type = type,
                                typeText = GetTypeText(type),
                                title = StripHtml(title),
                                score = score,
                                data = question
                            });
                        }
                    }
                }
                catch (Exception jsonEx)
                {
                    var errorResponse = new
                    {
                        success = false,
                        message = "JSON解析失败：" + jsonEx.Message,
                        error = jsonEx.GetType().Name,
                        rawData = exam.Edata.Length > 100 ? exam.Edata.Substring(0, 100) : exam.Edata
                    };
                    JavaScriptSerializer errSerializer = new JavaScriptSerializer();
                    context.Response.Write(errSerializer.Serialize(errorResponse));
                    return;
                }
            }
            
            var response = new
            {
                success = true,
                questions = questions
            };
            
            JavaScriptSerializer serializer2 = new JavaScriptSerializer();
            serializer2.MaxJsonLength = Int32.MaxValue;
            context.Response.Write(serializer2.Serialize(response));
        }
        catch (Exception ex)
        {
            var errorResponse = new
            {
                success = false,
                message = ex.Message,
                error = ex.GetType().Name
            };
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            context.Response.Write(serializer.Serialize(errorResponse));
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
    
    private string StripHtml(string html)
    {
        if (string.IsNullOrEmpty(html))
            return html;
        
        try
        {
            return System.Text.RegularExpressions.Regex.Replace(html, "<[^>]*>", "");
        }
        catch
        {
            return html;
        }
    }
    
    public bool IsReusable
    {
        get { return false; }
    }
}
