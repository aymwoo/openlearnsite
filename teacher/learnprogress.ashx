<%@ WebHandler Language="C#" Class="learnprogress" %>

using System;
using System.Collections.Generic;
using System.Web;
using LearnSite.DBUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/// <summary>
/// 教师端轮询接口：获取指定班级学生的实时学习状态
/// </summary>
public class learnprogress : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.AddHeader("Cache-Control", "no-cache, no-store");

        try
        {
            // 验证教师身份
            if (context.Request.Cookies[LearnSite.Common.CookieHelp.teaCookieNname] == null)
            {
                context.Response.Write("{\"ok\":false,\"msg\":\"not authorized\"}");
                return;
            }

            string action = context.Request.QueryString["action"] ?? "status";
            string sgrade = context.Request.QueryString["sgrade"] ?? "0";
            string sclass = context.Request.QueryString["sclass"] ?? "0";
            string scid = context.Request.QueryString["cid"] ?? "0";

            int grade = int.Parse(sgrade);
            int cls = int.Parse(sclass);
            int cid = int.Parse(scid);

            switch (action)
            {
                case "status":
                    // 返回该班级所有学生的学习状态列表
                    string statusJson = LearnSite.Common.LearnStatus.GetClassStatusJson(grade, cls, cid);
                    context.Response.Write("{\"ok\":true,\"data\":" + statusJson + "}");
                    break;

                case "progress":
                    // 返回该班级的学习进度统计
                    string progressJson = LearnSite.Common.LearnStatus.GetClassProgressJson(grade, cls, cid);
                    context.Response.Write("{\"ok\":true,\"data\":" + progressJson + "}");
                    break;

                case "all":
                    // 返回完整数据（状态列表 + 进度统计）
                    string allStatus = EnhanceStudentsWithAssessment(LearnSite.Common.LearnStatus.GetClassStatusJson(grade, cls, cid), cid);
                    string allProgress = LearnSite.Common.LearnStatus.GetClassProgressJson(grade, cls, cid);
                    context.Response.Write("{\"ok\":true,\"students\":" + allStatus + ",\"progress\":" + allProgress + "}");
                    break;

                case "studentdetail":
                    GetStudentDetail(context, cid);
                    break;

                default:
                    context.Response.Write("{\"ok\":false,\"msg\":\"unknown action\"}");
                    break;
            }
        }
        catch (Exception ex)
        {
            context.Response.Write("{\"ok\":false,\"msg\":\"" + ex.Message.Replace("\"", "'") + "\"}");
        }
    }

    public bool IsReusable
    {
        get { return false; }
    }

    private void GetStudentDetail(HttpContext context, int cid)
    {
        int sid;
        int lid;
        if (!int.TryParse(context.Request.QueryString["sid"], out sid) || sid <= 0)
        {
            context.Response.Write("{\"ok\":false,\"msg\":\"invalid sid\"}");
            return;
        }
        int.TryParse(context.Request.QueryString["lid"], out lid);

        LearnSite.BLL.Students stuBll = new LearnSite.BLL.Students();
        LearnSite.Model.Students stu = stuBll.GetModel(sid);
        if (stu == null)
        {
            context.Response.Write("{\"ok\":false,\"msg\":\"student not found\"}");
            return;
        }

        LearnSite.Model.AIStudentExamAssessment assessment = null;
        if (DbHelperSQL.TabExists("AIStudentExamAssessment"))
        {
            assessment = GetMatchedAssessment(sid, cid, lid);
        }
        Dictionary<string, string> questionTitles = new Dictionary<string, string>();
        Dictionary<string, string> optionTexts = new Dictionary<string, string>();
        Dictionary<string, string> blankAnswers = new Dictionary<string, string>();
        if (assessment != null && assessment.Vid.HasValue && assessment.Vid.Value > 0)
        {
            LearnSite.BLL.SurveyQuestion qBll = new LearnSite.BLL.SurveyQuestion();
            LearnSite.BLL.SurveyItem itemBll = new LearnSite.BLL.SurveyItem();
            List<LearnSite.Model.SurveyQuestion> questions = qBll.GetModelList("Qvid=" + assessment.Vid.Value + " order by Qid asc");
            foreach (LearnSite.Model.SurveyQuestion question in questions)
            {
                if (question != null)
                {
                    questionTitles[question.Qid.ToString()] = HttpUtility.HtmlDecode(question.Qtitle ?? string.Empty);
                    List<LearnSite.Model.SurveyItem> items = itemBll.GetModelList("Mqid=" + question.Qid + " order by Mid asc");
                    foreach (LearnSite.Model.SurveyItem item in items)
                    {
                        if (item != null)
                        {
                            optionTexts[item.Mid.ToString()] = HttpUtility.HtmlDecode(item.Mitem ?? string.Empty);
                            blankAnswers[item.Mid.ToString()] = HttpUtility.HtmlDecode(item.Mitem ?? string.Empty);
                        }
                    }
                }
            }
        }
        var data = new
        {
            sid = sid,
            snum = stu.Snum,
            sname = stu.Sname,
            hasAssessment = assessment != null,
            questionTitles = questionTitles,
            optionTexts = optionTexts,
            blankAnswers = blankAnswers,
            assessment = assessment == null ? null : new
            {
                providerName = assessment.ProviderName,
                skillName = assessment.SkillName,
                summary = assessment.Summary,
                assessmentContent = assessment.AssessmentContent,
                learningLog = assessment.LearningLog,
                answerLog = assessment.AnswerLog,
                score = assessment.Score,
                questionCount = assessment.QuestionCount,
                isFallback = assessment.IsFallback,
                createdAt = assessment.CreatedAt.HasValue ? assessment.CreatedAt.Value.ToString("yyyy-MM-dd HH:mm:ss") : ""
            }
        };
        context.Response.Write("{\"ok\":true,\"data\":" + JsonConvert.SerializeObject(data) + "}");
    }

    private string EnhanceStudentsWithAssessment(string studentsJson, int cid)
    {
        JArray students = JsonConvert.DeserializeObject<JArray>(studentsJson);
        if (students == null || students.Count == 0)
        {
            return studentsJson;
        }

        if (!DbHelperSQL.TabExists("AIStudentExamAssessment"))
        {
            foreach (JToken token in students)
            {
                if (token is JObject)
                {
                    ((JObject)token)["HasAssessment"] = false;
                    ((JObject)token)["AssessmentTime"] = string.Empty;
                    ((JObject)token)["AssessmentFallback"] = false;
                }
            }
            return students.ToString(Formatting.None);
        }

        LearnSite.BLL.AIStudentExamAssessment assessmentBll = new LearnSite.BLL.AIStudentExamAssessment();
        foreach (JToken token in students)
        {
            int sid = token["Sid"] == null ? 0 : token["Sid"].Value<int>();
            int lid = token["Lid"] == null ? 0 : token["Lid"].Value<int>();
            LearnSite.Model.AIStudentExamAssessment assessment = sid > 0 ? GetMatchedAssessment(assessmentBll, sid, cid, lid) : null;
            bool hasAssessment = assessment != null;
            if (token is JObject)
            {
                ((JObject)token)["HasAssessment"] = hasAssessment;
                ((JObject)token)["AssessmentTime"] = assessment != null && assessment.CreatedAt.HasValue ? assessment.CreatedAt.Value.ToString("MM-dd HH:mm") : string.Empty;
                ((JObject)token)["AssessmentFallback"] = assessment != null && assessment.IsFallback;
            }
        }

        return students.ToString(Formatting.None);
    }

    private LearnSite.Model.AIStudentExamAssessment GetMatchedAssessment(int sid, int cid, int lid)
    {
        LearnSite.BLL.AIStudentExamAssessment assessmentBll = new LearnSite.BLL.AIStudentExamAssessment();
        return GetMatchedAssessment(assessmentBll, sid, cid, lid);
    }

    private LearnSite.Model.AIStudentExamAssessment GetMatchedAssessment(LearnSite.BLL.AIStudentExamAssessment assessmentBll, int sid, int cid, int lid)
    {
        LearnSite.Model.AIStudentExamAssessment assessment = null;
        if (sid <= 0 || cid <= 0)
            return null;

        if (lid > 0)
        {
            assessment = assessmentBll.GetLatestByStudentCourseLesson(sid, cid, lid);
        }

        if (assessment == null)
        {
            assessment = assessmentBll.GetLatestByStudentCourse(sid, cid);
        }

        return assessment;
    }
}
