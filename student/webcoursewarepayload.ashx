<%@ WebHandler Language="C#" Class="webcoursewarepayload" %>

using System;
using System.Web;
using Newtonsoft.Json;

public class webcoursewarepayload : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentEncoding = System.Text.Encoding.UTF8;
        context.Response.Charset = "utf-8";
        context.Response.ContentType = "application/json; charset=utf-8";
        context.Response.AddHeader("Cache-Control", "no-cache, no-store");

        try
        {
            if (context.Request.Cookies[LearnSite.Common.CookieHelp.stuCookieNname] == null)
            {
                context.Response.Write("{\"success\":false,\"msg\":\"Unauthorized\"}");
                return;
            }

            int listMenuId = ParsePositiveInt(context.Request["lid"]);
            int missionId = ParsePositiveInt(context.Request["mid"]);
            if (listMenuId <= 0 && missionId <= 0)
            {
                context.Response.Write("{\"success\":false,\"msg\":\"Missing lid or mid.\"}");
                return;
            }

            LearnSite.Model.ListMenu listMenu = null;
            if (listMenuId > 0)
            {
                listMenu = new LearnSite.BLL.ListMenu().GetModel(listMenuId);
                if (listMenu == null || listMenu.Lid <= 0 || listMenu.Ltype.GetValueOrDefault() != 38 || !listMenu.Lxid.HasValue)
                {
                    context.Response.Write("{\"success\":false,\"msg\":\"Published web courseware not found.\"}");
                    return;
                }

                missionId = listMenu.Lxid.Value;
            }

            LearnSite.Model.Mission mission = new LearnSite.BLL.Mission().GetModel(missionId);
            if (mission == null || !mission.Mcid.HasValue || mission.Mcategory != 38)
            {
                context.Response.Write("{\"success\":false,\"msg\":\"Published web courseware not found.\"}");
                return;
            }

            LearnSite.Model.Courses course = new LearnSite.BLL.Courses().GetModel(mission.Mcid.Value);
            if (course == null || !course.Chid.HasValue || course.Chid.Value <= 0)
            {
                context.Response.Write("{\"success\":false,\"msg\":\"Published course not found.\"}");
                return;
            }

            LearnSite.BLL.CourseActivityPlanDraft draftBll = new LearnSite.BLL.CourseActivityPlanDraft();
            LearnSite.Model.CourseActivityPlanDraft record = draftBll.GetCurrentByCourse(mission.Mcid.Value, course.Chid.Value);
            if (record == null)
            {
                context.Response.Write("{\"success\":false,\"msg\":\"Web courseware draft not found.\"}");
                return;
            }

            LearnSite.Common.ActivityPlanSavedDraftPayload savedDraft = LearnSite.Common.AIActivityPlanSavedDraftHelper.ParseFullLessonRecord(record, mission.Mcid.Value, record.Hid);
            LearnSite.Common.WebCoursewareRuntimePayloadResult payload = LearnSite.Common.WebCoursewareRuntimePayloadResolver.Resolve(savedDraft, listMenuId, missionId);
            if (payload == null || payload.WebCourseware == null)
            {
                context.Response.Write("{\"success\":false,\"msg\":\"Published web courseware payload not found.\"}");
                return;
            }

            context.Response.Write(JsonConvert.SerializeObject(new
            {
                success = true,
                data = new
                {
                    topic = payload.Topic,
                    blockKey = payload.BlockKey,
                    lid = payload.ListMenuId,
                    mid = payload.MissionId,
                    webCourseware = new
                    {
                        mtitle = payload.WebCourseware.Mtitle,
                        mcategory = payload.WebCourseware.Mcategory,
                        mfiletype = payload.WebCourseware.Mfiletype,
                        mback = payload.WebCourseware.Mback,
                        mupload = payload.WebCourseware.Mupload,
                        ltype = payload.WebCourseware.Ltype,
                        lessonSummary = payload.WebCourseware.LessonSummary,
                        teachingGoals = payload.WebCourseware.TeachingGoals,
                        explanationCards = payload.WebCourseware.ExplanationCards,
                        keywords = payload.WebCourseware.Keywords,
                        practiceItems = payload.WebCourseware.PracticeItems,
                        lessonWrapUp = payload.WebCourseware.LessonWrapUp
                    }
                }
            }));
        }
        catch (Exception ex)
        {
            context.Response.Write("{\"success\":false,\"msg\":\"" + ex.Message.Replace("\"", "'") + "\"}");
        }
    }

    public bool IsReusable
    {
        get { return false; }
    }

    private static int ParsePositiveInt(string value)
    {
        int parsed;
        return int.TryParse(value, out parsed) && parsed > 0 ? parsed : 0;
    }
}
