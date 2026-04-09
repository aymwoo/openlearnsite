<%@ WebHandler Language="C#" Class="uploadexam" %>

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using LearnSite.DBUtility;
using Newtonsoft.Json;

public class uploadexam : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.BufferOutput = false;
        context.Response.Charset = "utf-8";
        context.Response.ContentType = "text/event-stream";
        context.Response.CacheControl = "no-cache";
        context.Response.Headers["X-Accel-Buffering"] = "no";

        LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
        if (!cook.IsExist())
        {
            WriteEvent(context, "failed", "{" + JsonPair("message", "未登录或登录已失效。") + "}");
            return;
        }

        string selectstr = HttpContext.Current.Request.QueryString["selectstr"] ?? string.Empty;
        string score = HttpContext.Current.Request.QueryString["score"] ?? "0";
        string lidstr = HttpContext.Current.Request.QueryString["lidstr"] ?? "0";
        string cidstr = HttpContext.Current.Request.QueryString["cidstr"] ?? "0";
        string vidstr = HttpContext.Current.Request.QueryString["vidstr"] ?? "0";
        string vtypestr = HttpContext.Current.Request.QueryString["vtypestr"] ?? "0";
        string answerlog = HttpContext.Current.Request.QueryString["answerlog"] ?? string.Empty;
        string qcount = HttpContext.Current.Request.QueryString["qcount"] ?? "0";
        int vid = Int32.Parse(vidstr);
        bool enableAiAssessment = IsSurveyAiAssessmentEnabled(vid);

        try
        {
            WriteProgress(context, 1, "正在提交测验结果...");
            string Wtime = cook.LoginTime;
            DateTime Wdate = DateTime.Now;
            LearnSite.Model.SurveyFeedback fmodel = new LearnSite.Model.SurveyFeedback();
            fmodel.Fnum = cook.Snum;
            fmodel.Fyear = cook.Syear;
            fmodel.Fgrade = cook.Sgrade;
            fmodel.Fclass = cook.Sclass;
            fmodel.Fterm = cook.ThisTerm;
            fmodel.Fcid = Int32.Parse(cidstr);
            fmodel.Fvid = Int32.Parse(vidstr);
            fmodel.Fvtype = Int32.Parse(vtypestr);
            fmodel.Fselect = selectstr;
            fmodel.Fscore = Int32.Parse(score);
            fmodel.Fdate = DateTime.Now;
            fmodel.Fsid = cook.Sid;
            fmodel.Flid = Int32.Parse(lidstr);

            LearnSite.BLL.SurveyFeedback fbll = new LearnSite.BLL.SurveyFeedback();
            int fid = fbll.Add(fmodel);
            if (fid <= 0)
            {
                WriteEvent(context, "failed", "{" + JsonPair("message", "测验提交失败，请稍后重试。") + "}");
                return;
            }

            LearnSite.Model.MenuWorks kmodel = new LearnSite.Model.MenuWorks();
            kmodel.Klid = Int32.Parse(lidstr);
            kmodel.Ksid = cook.Sid;
            kmodel.Ktime = LearnSite.Common.Computer.GoneMinute(DateTime.Parse(Wtime), Wdate);
            kmodel.Kseconds = Int32.Parse(LearnSite.Common.Computer.Datagone(DateTime.Parse(Wtime), Wdate));
            kmodel.Kcheck = false;
            LearnSite.BLL.MenuWorks kbll = new LearnSite.BLL.MenuWorks();
            kbll.Add(kmodel);

            LearnSite.BLL.AIStudentExamGenerator generator = new LearnSite.BLL.AIStudentExamGenerator();
            LearnSite.BLL.StudentExamAssessmentContext assessContext = new LearnSite.BLL.StudentExamAssessmentContext();
            assessContext.Fid = fid;
            assessContext.Sid = cook.Sid;
            assessContext.Snum = cook.Snum;
            assessContext.Sname = HttpUtility.UrlDecode(cook.Sname);
            assessContext.Cid = Int32.Parse(cidstr);
            assessContext.Lid = Int32.Parse(lidstr);
            assessContext.Vid = vid;
            assessContext.ExamTitle = GetSurveyTitle(vid);
            assessContext.Score = Int32.Parse(score);
            assessContext.QuestionCount = Int32.Parse(qcount);
            assessContext.AnswerLogJson = answerlog;
            assessContext.LearningLog = BuildLearningLog(assessContext, Wtime, Wdate, answerlog);

            LearnSite.BLL.StudentExamAssessmentResult result;
            if (enableAiAssessment)
            {
                LearnSite.BLL.AIStudentExamGenerator.EnsureDefaultStudentExamSkill();
                WriteProgress(context, 2, "正在调用 AI Provider “" + LearnSite.BLL.AIStudentExamGenerator.GetDefaultProviderDisplayName() + "” 生成测验评估...");
                result = generator.Generate(assessContext, delegate(string stage, string message)
                {
                    if (stage == "ai" || stage == "fallback")
                    {
                        WriteProgress(context, 2, message);
                    }
                });
            }
            else
            {
                WriteProgress(context, 2, "当前测验未启用 AI 评价，正在生成规则评估摘要...");
                result = generator.GenerateRuleBasedAssessment(assessContext);
            }

            if (DbHelperSQL.TabExists("AIStudentExamAssessment"))
            {
                WriteProgress(context, 3, enableAiAssessment ? "正在保存 AI 测验评估结果..." : "正在保存规则评估摘要...");
                generator.SaveAssessment(assessContext, result);
            }
            else
            {
                WriteProgress(context, 3, "当前数据库尚未创建 AI 测验评估表，本次只完成测验提交。请先执行 upgrade.aspx 升级数据库，升级后即可在教师端查看 AI 评估。");
            }

            string doneJson = "{" +
                JsonPair("message", DbHelperSQL.TabExists("AIStudentExamAssessment") ? (enableAiAssessment ? "提交成功，AI 测验评估已生成。" : "提交成功，规则评估摘要已生成。") : "提交成功，但当前数据库尚未启用 AI 测验评估存储，请执行 upgrade.aspx 完成升级。") + "," +
                JsonPair("provider", result.ProviderDisplayName ?? string.Empty) + "," +
                JsonPair("summary", result.Summary ?? string.Empty) + "," +
                JsonPair("fallback", result.UsedFallback ? "1" : "0") +
                "}";
            WriteEvent(context, "done", doneJson);
        }
        catch (Exception ex)
        {
            WriteEvent(context, "failed", "{" + JsonPair("message", ex.Message.Replace("\"", "'").Replace("\r", " ").Replace("\n", " ")) + "}");
        }
    }

    private static string GetSurveyTitle(int vid)
    {
        LearnSite.BLL.Survey bll = new LearnSite.BLL.Survey();
        LearnSite.Model.Survey model = bll.GetModel(vid);
        return model == null ? "课堂测验" : model.Vtitle;
    }

    private static bool IsSurveyAiAssessmentEnabled(int vid)
    {
        if (!DbHelperSQL.ColumnExists("Survey", "Venableai"))
            return false;

        LearnSite.BLL.Survey bll = new LearnSite.BLL.Survey();
        LearnSite.Model.Survey model = bll.GetModel(vid);
        return model != null && model.Venableai;
    }

    private static string BuildLearningLog(LearnSite.BLL.StudentExamAssessmentContext context, string loginTime, DateTime submitTime, string answerLog)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("学生：").Append(context.Sname).Append("（").Append(context.Snum).Append("）\n");
        sb.Append("测验：").Append(context.ExamTitle).Append("\n");
        sb.Append("得分：").Append(context.Score).Append(" / ").Append(context.QuestionCount).Append("\n");
        sb.Append("进入时间：").Append(loginTime).Append("\n");
        sb.Append("提交时间：").Append(submitTime.ToString("yyyy-MM-dd HH:mm:ss")).Append("\n");
        sb.Append("答题日志：\n").Append(answerLog);
        return sb.ToString();
    }

    private static void WriteProgress(HttpContext context, int step, string message)
    {
        string json = "{" + JsonPair("step", step.ToString()) + "," + JsonPair("message", message) + "}";
        WriteEvent(context, "progress", json);
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

    public bool IsReusable {
        get {
            return false;
        }
    }

}
