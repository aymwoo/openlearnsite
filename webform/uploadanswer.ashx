<%@ WebHandler Language="C#" Class="uploadanswer" %>

using System;
using System.Text;
using System.Web;
using LearnSite.DBUtility;

/// <summary>
/// webform 考试系统的 AI 评估提交处理器
/// 支持：
/// 1. GET + SSE（兼容旧实现）
/// 2. POST + JSON（用于大体积答题数据，避免 URL 过长）
/// </summary>
public class uploadanswer : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        bool useJsonResponse = string.Equals(context.Request.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase);

        context.Response.BufferOutput = false;
        context.Response.Charset = "utf-8";
        context.Response.ContentType = useJsonResponse ? "application/json" : "text/event-stream";
        context.Response.CacheControl = "no-cache";
        if (!useJsonResponse)
        {
            context.Response.Headers["X-Accel-Buffering"] = "no";
        }

        LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
        if (!cook.IsExist())
        {
            WriteError(context, useJsonResponse, "未登录或登录已失效。");
            return;
        }

        string lidstr = GetRequestValue(context, "Lid", "lid", false, "0");
        string cidstr = GetRequestValue(context, "Cid", "cid", false, "0");
        string eidstr = GetRequestValue(context, "Eid", "eid", false, "0");
        string scorestr = GetRequestValue(context, "Ascore", "score", false, "0");
        string spendstr = GetRequestValue(context, "Aspent", "spend", false, "0");
        string qcountstr = GetRequestValue(context, "Qcount", "qcount", false, "0");
        string enableAistr = GetRequestValue(context, "EnableAiAssessment", "enableAiAssessment", false, "0");
        string adata = GetRequestValue(context, "Adata", "adata", true, string.Empty);
        string adata64 = GetRequestValue(context, "adata64", "adata64", true, string.Empty);

        try
        {
            if (string.IsNullOrEmpty(adata) && !string.IsNullOrEmpty(adata64))
            {
                adata = DecodePayload(adata64);
            }

            LearnSite.BLL.AIStudentExamGenerator.EnsureDefaultStudentExamSkill();
            WriteProgress(context, useJsonResponse, 1, "正在提交测验结果...");

            int eid = Int32.Parse(eidstr);
            int lid = Int32.Parse(lidstr);
            int cid = Int32.Parse(cidstr);
            int score = Int32.Parse(scorestr);
            int spend = Int32.Parse(spendstr);
            int qcount = Int32.Parse(qcountstr);
            bool enableAiAssessment = enableAistr == "1" || string.Equals(enableAistr, "true", StringComparison.OrdinalIgnoreCase);

            LearnSite.BLL.Answers ebll = new LearnSite.BLL.Answers();
            LearnSite.Model.Answers emodel = ebll.GetModelme(eid, cook.Sid);
            int aid = 0;

            if (emodel != null)
            {
                aid = emodel.Aid;
                emodel.Atime = DateTime.Now;
                emodel.Ascore = score;
                emodel.Aspent = spend;
                emodel.Adata = adata;
                ebll.Update(emodel);
            }
            else
            {
                LearnSite.Model.Answers newmodel = new LearnSite.Model.Answers();
                newmodel.Eid = eid;
                newmodel.Asid = cook.Sid;
                newmodel.Asnum = cook.Snum;
                newmodel.Asname = cook.Sname;
                newmodel.Asgrade = cook.Sgrade;
                newmodel.Asclass = cook.Sclass;
                newmodel.Atime = DateTime.Now;
                newmodel.Ascore = score;
                newmodel.Aspent = spend;
                newmodel.Adata = adata;
                aid = ebll.Add(newmodel);

                string wtime = cook.LoginTime;
                DateTime wdate = DateTime.Now;
                LearnSite.Model.MenuWorks kmodel = new LearnSite.Model.MenuWorks();
                kmodel.Klid = lid;
                kmodel.Ksid = cook.Sid;
                kmodel.Ktime = LearnSite.Common.Computer.GoneMinute(DateTime.Parse(wtime), wdate);
                kmodel.Kcheck = false;
                LearnSite.BLL.MenuWorks kbll = new LearnSite.BLL.MenuWorks();
                kbll.Add(kmodel);
            }

            if (aid <= 0 && emodel == null)
            {
                WriteError(context, useJsonResponse, "测验提交失败，请稍后重试。");
                return;
            }

            string examTitle = GetExamTitle(eid);

            LearnSite.BLL.AIStudentExamGenerator generator = new LearnSite.BLL.AIStudentExamGenerator();
            LearnSite.BLL.StudentExamAssessmentContext assessContext = new LearnSite.BLL.StudentExamAssessmentContext();
            assessContext.Fid = aid > 0 ? aid : (int?)null;
            assessContext.Sid = cook.Sid;
            assessContext.Snum = cook.Snum;
            assessContext.Sname = HttpUtility.UrlDecode(cook.Sname);
            assessContext.Cid = cid;
            assessContext.Lid = lid;
            assessContext.Vid = eid;
            assessContext.ExamTitle = examTitle;
            assessContext.Score = score;
            assessContext.QuestionCount = qcount;
            assessContext.AnswerLogJson = adata;
            assessContext.LearningLog = BuildLearningLog(assessContext, cook.LoginTime, DateTime.Now, adata);

            LearnSite.BLL.StudentExamAssessmentResult result;
            if (enableAiAssessment)
            {
                WriteProgress(context, useJsonResponse, 2, "正在调用 AI Provider “" + LearnSite.BLL.AIStudentExamGenerator.GetDefaultProviderDisplayName() + "” 生成测验评估...");
                result = generator.Generate(assessContext, delegate(string stage, string message)
                {
                    if (stage == "ai" || stage == "fallback")
                    {
                        WriteProgress(context, useJsonResponse, 2, message);
                    }
                });
            }
            else
            {
                WriteProgress(context, useJsonResponse, 2, "当前试卷未启用 AI 评估，正在生成规则评估摘要...");
                result = generator.GenerateRuleBasedAssessment(assessContext);
            }

            if (DbHelperSQL.TabExists("AIStudentExamAssessment"))
            {
                WriteProgress(context, useJsonResponse, 3, "正在保存 AI 测验评估结果...");
                generator.SaveAssessment(assessContext, result);
            }
            else
            {
                WriteProgress(context, useJsonResponse, 3, "当前数据库尚未创建 AI 测验评估表，本次只完成测验提交。请先执行 upgrade.aspx 升级数据库，升级后即可在教师端查看 AI 评估。");
            }

            string doneJson = "{" +
                JsonPair("message", DbHelperSQL.TabExists("AIStudentExamAssessment") ? (enableAiAssessment ? "提交成功，AI 测验评估已生成。" : "提交成功，规则评估摘要已生成。") : "提交成功，但当前数据库尚未启用 AI 测验评估存储，请执行 upgrade.aspx 完成升级。") + "," +
                JsonPair("aid", aid > 0 ? aid.ToString() : (emodel != null ? emodel.Aid.ToString() : "0")) + "," +
                JsonPair("provider", result.ProviderDisplayName ?? string.Empty) + "," +
                JsonPair("summary", result.Summary ?? string.Empty) + "," +
                JsonPair("fallback", result.UsedFallback ? "1" : "0") +
                "}";

            if (useJsonResponse)
            {
                context.Response.Write("{\"ok\":true," + doneJson.Substring(1));
            }
            else
            {
                WriteEvent(context, "done", doneJson);
            }
        }
        catch (Exception ex)
        {
            WriteError(context, useJsonResponse, ex.Message.Replace("\"", "'").Replace("\r", " ").Replace("\n", " "));
        }
    }

    private static string GetRequestValue(HttpContext context, string formKey, string queryKey, bool allowHtml, string defaultValue)
    {
        string value = null;
        if (allowHtml && context.Request.Unvalidated != null)
        {
            value = context.Request.Unvalidated.Form[formKey] ?? context.Request.Unvalidated.QueryString[queryKey];
        }
        else
        {
            value = context.Request.Form[formKey] ?? context.Request.QueryString[queryKey];
        }
        return value ?? defaultValue;
    }

    private static string GetExamTitle(int eid)
    {
        try
        {
            LearnSite.BLL.Exams ebll = new LearnSite.BLL.Exams();
            LearnSite.Model.Exams emodel = ebll.GetModel(eid);
            return emodel == null ? "课堂测验" : emodel.Etitle;
        }
        catch
        {
            return "课堂测验";
        }
    }

    private static string BuildLearningLog(LearnSite.BLL.StudentExamAssessmentContext context, string loginTime, DateTime submitTime, string answerData)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("学生：").Append(context.Sname).Append("（").Append(context.Snum).Append("）\n");
        sb.Append("测验：").Append(context.ExamTitle).Append("\n");
        sb.Append("得分：").Append(context.Score).Append(" / ").Append(context.QuestionCount).Append("\n");
        sb.Append("进入时间：").Append(loginTime).Append("\n");
        sb.Append("提交时间：").Append(submitTime.ToString("yyyy-MM-dd HH:mm:ss")).Append("\n");
        sb.Append("答题日志：\n").Append(answerData);
        return sb.ToString();
    }

    private static string DecodePayload(string encoded)
    {
        if (string.IsNullOrEmpty(encoded))
            return string.Empty;

        try
        {
            byte[] bytes = Convert.FromBase64String(encoded);
            return Encoding.UTF8.GetString(bytes);
        }
        catch
        {
            return string.Empty;
        }
    }

    private static void WriteProgress(HttpContext context, bool useJsonResponse, int step, string message)
    {
        if (useJsonResponse)
            return;

        string json = "{" + JsonPair("step", step.ToString()) + "," + JsonPair("message", message) + "}";
        WriteEvent(context, "progress", json);
    }

    private static void WriteError(HttpContext context, bool useJsonResponse, string message)
    {
        if (useJsonResponse)
        {
            context.Response.Write("{\"ok\":false," + JsonPair("message", message) + "}");
            return;
        }

        WriteEvent(context, "failed", "{" + JsonPair("message", message) + "}");
    }

    private static void WriteEvent(HttpContext context, string eventName, string data)
    {
        context.Response.Write("event: " + eventName + "\n");
        context.Response.Write("data: " + data + "\n\n");
        context.Response.Flush();
    }

    private static string JsonPair(string key, string value)
    {
        return "\"" + EscapeJson(key) + "\":\"" + EscapeJson(value) + "\"";
    }

    private static string EscapeJson(string value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;
        return value.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r", "\\r").Replace("\n", "\\n").Replace("\t", "\\t");
    }

    public bool IsReusable
    {
        get { return false; }
    }
}
