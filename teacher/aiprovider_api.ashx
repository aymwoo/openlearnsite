<%@ WebHandler Language="C#" Class="aiprovider_api" %>

using System;
using System.Web;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;

public class aiprovider_api : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "application/json";
        
        // Check teacher cookie exists (ashx cannot use JudgeTeacherCookies which returns void and does redirect)
        if (context.Request.Cookies[LearnSite.Common.CookieHelp.teaCookieNname] == null)
        {
            context.Response.Write("{\"success\":false,\"msg\":\"Unauthorized\"}");
            return;
        }

        string action = context.Request["action"];
        
        try
        {
            switch (action)
            {
                case "list":
                    GetList(context);
                    break;
                case "save":
                    Save(context);
                    break;
                case "delete":
                    Delete(context);
                    break;
                case "setdefault":
                    SetDefault(context);
                    break;
                case "test":
                    TestConnection(context);
                    break;
                case "import":
                    ImportConfig(context);
                    break;
                case "chat":
                    Chat(context);
                    break;
                case "activityPlan":
                    ActivityPlan(context);
                    break;
                case "activityPlanRegenerateSection":
                    ActivityPlanRegenerateSection(context);
                    break;
                case "activityPlanDraftStatus":
                    ActivityPlanDraftStatus(context);
                    break;
                case "activityPlanSaveDraft":
                    ActivityPlanSaveDraft(context);
                    break;
                case "activityPlanLoadDraft":
                    ActivityPlanLoadDraft(context);
                    break;
                case "activityPlanDeleteDraft":
                    ActivityPlanDeleteDraft(context);
                    break;
                case "activityPlanPublish":
                    ActivityPlanPublish(context);
                    break;
                case "fullLessonGenerate":
                    FullLessonGenerate(context);
                    break;
                case "fullLessonRegenerateBlock":
                    FullLessonRegenerateBlock(context);
                    break;
                case "fullLessonDraftStatus":
                    FullLessonDraftStatus(context);
                    break;
                case "fullLessonSaveDraft":
                    FullLessonSaveDraft(context);
                    break;
                case "fullLessonLoadDraft":
                    FullLessonLoadDraft(context);
                    break;
                case "fullLessonDeleteDraft":
                    FullLessonDeleteDraft(context);
                    break;
                case "listSkills":
                    GetSkillList(context);
                    break;
                case "saveSkill":
                    SaveSkill(context);
                    break;
                case "deleteSkill":
                    DeleteSkill(context);
                    break;
                case "listCustomSkills":
                    GetCustomSkillList(context);
                    break;
                case "saveCustomSkill":
                    SaveCustomSkill(context);
                    break;
                case "deleteCustomSkill":
                    DeleteCustomSkill(context);
                    break;
                default:
                    context.Response.Write("{\"success\":false,\"msg\":\"Unknown action\"}");
                    break;
            }
        }
        catch (Exception ex)
        {
            context.Response.Write("{\"success\":false,\"msg\":\"" + ex.Message.Replace("\"", "'").Replace("\r", "").Replace("\n", " ") + "\"}");
        }
    }

    private void GetList(HttpContext context)
    {
        LearnSite.BLL.AIProvider bll = new LearnSite.BLL.AIProvider();
        List<LearnSite.Model.AIProvider> list = bll.GetModelList("");
        
        // Obscure API keys before sending to the client
        foreach (var item in list)
        {
            if (!string.IsNullOrEmpty(item.ApiKey))
            {
                if (item.ApiKey.Length > 4)
                {
                    item.ApiKey = "********" + item.ApiKey.Substring(item.ApiKey.Length - 4);
                }
                else
                {
                    item.ApiKey = "********";
                }
            }
        }
        
        string json = JsonConvert.SerializeObject(new { success = true, data = list });
        context.Response.Write(json);
    }

    private void Save(HttpContext context)
    {
        string idStr = context.Request["id"];
        string displayName = context.Request["displayName"];
        string providerName = context.Request["providerName"];
        string modelName = context.Request["modelName"];
        string apiKey = context.Request["apiKey"];
        string baseUrl = context.Request["baseUrl"];

        if (string.IsNullOrEmpty(displayName) || string.IsNullOrEmpty(providerName) || string.IsNullOrEmpty(modelName))
        {
            context.Response.Write("{\"success\":false,\"msg\":\"Please fill in all required fields.\"}");
            return;
        }

        LearnSite.Model.AIProvider model = new LearnSite.Model.AIProvider();
        model.DisplayName = displayName;
        model.ProviderName = providerName;
        model.ModelName = modelName;
        model.BaseUrl = baseUrl;

        LearnSite.BLL.AIProvider bll = new LearnSite.BLL.AIProvider();
        
        if (string.IsNullOrEmpty(idStr) || idStr == "0")
        {
            // Add
            model.ApiKey = apiKey;
            model.IsDefault = false;
            if (bll.GetModelList("").Count == 0)
            {
                model.IsDefault = true; // Make default if it's the first one
            }
            int id = bll.Add(model);
            if (id > 0)
            {
                context.Response.Write("{\"success\":true,\"msg\":\"Added successfully.\"}");
            }
            else
            {
                context.Response.Write("{\"success\":false,\"msg\":\"Failed to add.\"}");
            }
        }
        else
        {
            // Update
            int id = int.Parse(idStr);
            LearnSite.Model.AIProvider oldModel = bll.GetModel(id);
            if (oldModel != null)
            {
                // Do not overwrite API key if it's the obscured placeholder
                if (!string.IsNullOrEmpty(apiKey) && apiKey.StartsWith("********"))
                {
                    model.ApiKey = oldModel.ApiKey;
                }
                else
                {
                    model.ApiKey = apiKey;
                }
                
                model.Id = id;
                model.IsDefault = oldModel.IsDefault;
                if (bll.Update(model))
                {
                    context.Response.Write("{\"success\":true,\"msg\":\"Updated successfully.\"}");
                }
                else
                {
                    context.Response.Write("{\"success\":false,\"msg\":\"Failed to update.\"}");
                }
            }
            else
            {
                context.Response.Write("{\"success\":false,\"msg\":\"Record not found.\"}");
            }
        }
    }

    private void Delete(HttpContext context)
    {
        string idStr = context.Request["id"];
        if (!string.IsNullOrEmpty(idStr))
        {
            int id = int.Parse(idStr);
            LearnSite.BLL.AIProvider bll = new LearnSite.BLL.AIProvider();
            if (bll.Delete(id))
            {
                context.Response.Write("{\"success\":true,\"msg\":\"Deleted successfully.\"}");
            }
            else
            {
                context.Response.Write("{\"success\":false,\"msg\":\"Failed to delete.\"}");
            }
        }
        else
        {
            context.Response.Write("{\"success\":false,\"msg\":\"Invalid ID.\"}");
        }
    }

    private void SetDefault(HttpContext context)
    {
        string idStr = context.Request["id"];
        if (!string.IsNullOrEmpty(idStr))
        {
            int id = int.Parse(idStr);
            LearnSite.BLL.AIProvider bll = new LearnSite.BLL.AIProvider();
            if (bll.SetDefault(id))
            {
                context.Response.Write("{\"success\":true,\"msg\":\"Default provider set successfully.\"}");
            }
            else
            {
                context.Response.Write("{\"success\":false,\"msg\":\"Failed to set default.\"}");
            }
        }
        else
        {
            context.Response.Write("{\"success\":false,\"msg\":\"Invalid ID.\"}");
        }
    }

    private void TestConnection(HttpContext context)
    {
        string apiKey = context.Request["apiKey"];
        string baseUrl = context.Request["baseUrl"];
        string modelName = context.Request["modelName"];
        string idStr = context.Request["id"];

        // If apiKey is the obscured placeholder, fetch the real key from DB
        if (!string.IsNullOrEmpty(apiKey) && apiKey.StartsWith("********"))
        {
            int id = 0;
            int.TryParse(idStr, out id);
            if (id > 0)
            {
                var bll = new LearnSite.BLL.AIProvider();
                var existing = bll.GetModel(id);
                if (existing != null)
                {
                    apiKey = existing.ApiKey;
                }
            }
        }
        
        if (string.IsNullOrEmpty(baseUrl))
        {
            context.Response.Write("{\"success\":false,\"msg\":\"Base URL is required for testing.\"}");
            return;
        }

        try
        {
            // Prepare OpenAI compatible request payload
            string testUrl = baseUrl.TrimEnd('/') + "/chat/completions";
            string payload = "{\"model\":\"" + modelName + "\",\"messages\":[{\"role\":\"user\",\"content\":\"hi\"}],\"max_tokens\":10}";
            
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(testUrl);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Timeout = 10000; // 10 seconds timeout
            
            if (!string.IsNullOrEmpty(apiKey))
            {
                request.Headers.Add("Authorization", "Bearer " + apiKey);
            }

            byte[] byteArray = Encoding.UTF8.GetBytes(payload);
            request.ContentLength = byteArray.Length;

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            string responseFromServer = reader.ReadToEnd();
                            // Parse response just to check if it's valid JSON from OpenAI format
                            JObject jsonResp = JsonConvert.DeserializeObject<JObject>(responseFromServer);
                            if (jsonResp != null && jsonResp["choices"] != null)
                            {
                                context.Response.Write("{\"success\":true,\"msg\":\"Connection successful!\"}");
                            }
                            else
                            {
                                context.Response.Write("{\"success\":false,\"msg\":\"Connection succeeded, but response format is not standard OpenAI compatible.\"}");
                            }
                        }
                    }
                }
                else
                {
                    context.Response.Write("{\"success\":false,\"msg\":\"HTTP Error: " + response.StatusCode + "\"}");
                }
            }
        }
        catch (WebException wex)
        {
            string errorMsg = wex.Message;
            if (wex.Response != null)
            {
                using (HttpWebResponse errorResponse = (HttpWebResponse)wex.Response)
                {
                    errorMsg += " Status code: " + (int)errorResponse.StatusCode;
                    using (Stream responseStream = errorResponse.GetResponseStream())
                    {
                        if (responseStream != null)
                        {
                            using (StreamReader reader = new StreamReader(responseStream))
                            {
                                errorMsg += " Details: " + reader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            context.Response.Write("{\"success\":false,\"msg\":\"Connection failed: " + errorMsg.Replace("\"", "'").Replace("\r", "").Replace("\n", " ") + "\"}");
        }
        catch (Exception ex)
        {
            context.Response.Write("{\"success\":false,\"msg\":\"Test error: " + ex.Message.Replace("\"", "'").Replace("\r", "").Replace("\n", " ") + "\"}");
        }
    }
    
    private void ImportConfig(HttpContext context)
    {
        string jsonConfig = context.Request["config"];
        if (string.IsNullOrEmpty(jsonConfig))
        {
            context.Response.Write("{\"success\":false,\"msg\":\"Configuration JSON is empty.\"}");
            return;
        }

        try
        {
            List<LearnSite.Model.AIProvider> providers = JsonConvert.DeserializeObject<List<LearnSite.Model.AIProvider>>(jsonConfig);
            if (providers != null && providers.Count > 0)
            {
                LearnSite.BLL.AIProvider bll = new LearnSite.BLL.AIProvider();
                int successCount = 0;
                foreach (var provider in providers)
                {
                    if (!string.IsNullOrEmpty(provider.DisplayName) && !string.IsNullOrEmpty(provider.ModelName))
                    {
                        bll.Add(provider);
                        successCount++;
                    }
                }
                context.Response.Write("{\"success\":true,\"msg\":\"Successfully imported " + successCount + " provider(s).\"}");
            }
            else
            {
                context.Response.Write("{\"success\":false,\"msg\":\"Invalid JSON format or empty list.\"}");
            }
        }
        catch (Exception ex)
        {
            context.Response.Write("{\"success\":false,\"msg\":\"Parse error: " + ex.Message.Replace("\"", "'").Replace("\r", "").Replace("\n", " ") + "\"}");
        }
    }

    
    private void Chat(HttpContext context)
    {
        context.Server.ScriptTimeout = 180;

        string prompt = context.Request["prompt"];
        if (string.IsNullOrEmpty(prompt))
        {
            string respStr = JsonConvert.SerializeObject(new { success = false, msg = "Prompt is required." });
            context.Response.Write(respStr);
            return;
        }

        WriteAiChatResponse(context, prompt, 0.7, 1200, "Chat error: ");
    }

    private void ActivityPlan(HttpContext context)
    {
        context.Server.ScriptTimeout = 180;

        string topic = LearnSite.Common.AIActivityPlanPromptBuilder.BoundText(context.Request["topic"], LearnSite.Common.AIActivityPlanPromptBuilder.MaxTopicLength);
        if (string.IsNullOrEmpty(topic))
        {
            string respStr = JsonConvert.SerializeObject(new { success = false, msg = "Topic is required." });
            context.Response.Write(respStr);
            return;
        }

        LearnSite.Common.AIActivityPlanPromptRequest request = new LearnSite.Common.AIActivityPlanPromptRequest
        {
            Topic = topic,
            Grade = LearnSite.Common.AIActivityPlanPromptBuilder.BoundText(context.Request["grade"], LearnSite.Common.AIActivityPlanPromptBuilder.MaxGradeLength),
            Duration = LearnSite.Common.AIActivityPlanPromptBuilder.BoundText(context.Request["duration"], LearnSite.Common.AIActivityPlanPromptBuilder.MaxDurationLength),
            TeachingGoals = LearnSite.Common.AIActivityPlanPromptBuilder.BoundText(context.Request["teachingGoals"], LearnSite.Common.AIActivityPlanPromptBuilder.MaxTeachingGoalsLength),
            ExistingCourseContent = LearnSite.Common.AIActivityPlanPromptBuilder.BoundText(context.Request["existingCourseContent"], 4000)
        };

        LearnSite.BLL.AIActivityPlanDraftGenerator generator = new LearnSite.BLL.AIActivityPlanDraftGenerator();
        LearnSite.BLL.ActivityPlanDraftGenerationResult result = generator.Generate(request);
        if (!result.Success || result.Draft == null)
        {
            string failResp = JsonConvert.SerializeObject(new
            {
                success = false,
                msg = result == null ? "活动计划生成失败。" : result.Message,
                providerDisplayName = result == null ? string.Empty : result.ProviderDisplayName,
                skillName = result == null ? string.Empty : result.SkillName
            });
            context.Response.Write(failResp);
            return;
        }

        string successResp = JsonConvert.SerializeObject(new
        {
            success = true,
            data = new
            {
                providerDisplayName = result.ProviderDisplayName,
                skillName = result.SkillName,
                message = result.Message,
                draft = new
                {
                    teachingGoals = result.Draft.TeachingGoals,
                    activitySteps = result.Draft.ActivitySteps.Select(step => new
                    {
                        sort = step.Sort,
                        title = step.Title,
                        minutes = step.Minutes,
                        teacherAction = step.TeacherAction,
                        studentAction = step.StudentAction,
                        interactionMethod = step.InteractionMethod,
                        resourceSuggestion = step.ResourceSuggestion,
                        assessmentCheck = step.AssessmentCheck
                    }).ToList(),
                    resources = result.Draft.Resources,
                    assessment = result.Draft.Assessment,
                    teacherReminder = result.Draft.TeacherReminder
                }
            }
        });
        context.Response.Write(successResp);
    }

    private void ActivityPlanRegenerateSection(HttpContext context)
    {
        context.Server.ScriptTimeout = 180;

        string topic = LearnSite.Common.AIActivityPlanPromptBuilder.BoundText(context.Request["topic"], LearnSite.Common.AIActivityPlanPromptBuilder.MaxTopicLength);
        if (string.IsNullOrEmpty(topic))
        {
            context.Response.Write(JsonConvert.SerializeObject(new { success = false, msg = "Topic is required." }));
            return;
        }

        string sectionTarget = context.Request["sectionTarget"];
        if (!LearnSite.Common.AIActivityPlanDraftHelper.IsSupportedSectionTarget(sectionTarget))
        {
            context.Response.Write(JsonConvert.SerializeObject(new { success = false, msg = "Section target is invalid." }));
            return;
        }

        LearnSite.Common.ActivityPlanDraft currentDraft = LearnSite.Common.AIActivityPlanDraftHelper.ParseDraft(context.Request["currentDraft"]);
        if (!LearnSite.Common.AIActivityPlanDraftHelper.IsValidDraft(currentDraft))
        {
            context.Response.Write(JsonConvert.SerializeObject(new { success = false, msg = "Current draft is invalid." }));
            return;
        }

        LearnSite.Common.AIActivityPlanSectionRegenerationRequest request = new LearnSite.Common.AIActivityPlanSectionRegenerationRequest
        {
            Topic = topic,
            Grade = LearnSite.Common.AIActivityPlanPromptBuilder.BoundText(context.Request["grade"], LearnSite.Common.AIActivityPlanPromptBuilder.MaxGradeLength),
            Duration = LearnSite.Common.AIActivityPlanPromptBuilder.BoundText(context.Request["duration"], LearnSite.Common.AIActivityPlanPromptBuilder.MaxDurationLength),
            TeachingGoals = LearnSite.Common.AIActivityPlanPromptBuilder.BoundText(context.Request["teachingGoals"], LearnSite.Common.AIActivityPlanPromptBuilder.MaxTeachingGoalsLength),
            ExistingCourseContent = LearnSite.Common.AIActivityPlanPromptBuilder.BoundText(context.Request["existingCourseContent"], 4000),
            SectionTarget = sectionTarget,
            CurrentDraft = currentDraft
        };

        LearnSite.BLL.AIActivityPlanDraftGenerator generator = new LearnSite.BLL.AIActivityPlanDraftGenerator();
        LearnSite.BLL.ActivityPlanDraftGenerationResult result = generator.RegenerateSection(request);
        if (!result.Success || result.Draft == null)
        {
            context.Response.Write(JsonConvert.SerializeObject(new
            {
                success = false,
                msg = result == null ? "活动计划局部重生成失败。" : result.Message,
                providerDisplayName = result == null ? string.Empty : result.ProviderDisplayName,
                skillName = result == null ? string.Empty : result.SkillName
            }));
            return;
        }

        string successResp = JsonConvert.SerializeObject(new
        {
            success = true,
            data = new
            {
                providerDisplayName = result.ProviderDisplayName,
                skillName = result.SkillName,
                message = result.Message,
                draft = new
                {
                    teachingGoals = result.Draft.TeachingGoals,
                    activitySteps = result.Draft.ActivitySteps.Select(step => new
                    {
                        sort = step.Sort,
                        title = step.Title,
                        minutes = step.Minutes,
                        teacherAction = step.TeacherAction,
                        studentAction = step.StudentAction,
                        interactionMethod = step.InteractionMethod,
                        resourceSuggestion = step.ResourceSuggestion,
                        assessmentCheck = step.AssessmentCheck
                    }).ToList(),
                    resources = result.Draft.Resources,
                    assessment = result.Draft.Assessment,
                    teacherReminder = result.Draft.TeacherReminder
                }
            }
        });
        context.Response.Write(successResp);
    }

    private void ActivityPlanDraftStatus(HttpContext context)
    {
        int cid;
        if (!TryGetAuthorizedCourse(context, out cid, out _))
        {
            return;
        }

        LearnSite.BLL.CourseActivityPlanDraft draftBll = new LearnSite.BLL.CourseActivityPlanDraft();
        LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();
        LearnSite.Model.CourseActivityPlanDraft record = draftBll.GetCurrentByCourse(cid, tcook.Hid);

        context.Response.Write(JsonConvert.SerializeObject(new
        {
            success = true,
            data = new
            {
                hasDraft = record != null,
                updatedAt = record == null ? string.Empty : record.UpdatedAt.ToString("s")
            }
        }));
    }

    private void ActivityPlanSaveDraft(HttpContext context)
    {
        int cid;
        LearnSite.Model.Courses course;
        if (!TryGetAuthorizedCourse(context, out cid, out course))
        {
            return;
        }

        LearnSite.Common.ActivityPlanDraft draft = LearnSite.Common.AIActivityPlanDraftHelper.ParseDraft(context.Request["currentDraft"]);
        LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();
        LearnSite.Model.CourseActivityPlanDraft record = LearnSite.Common.AIActivityPlanSavedDraftHelper.BuildRecord(
            cid,
            tcook.Hid,
            context.Request["topic"],
            context.Request["grade"],
            context.Request["duration"],
            context.Request["teachingGoals"],
            context.Request["existingCourseContent"],
            draft);

        if (record == null)
        {
            context.Response.Write(JsonConvert.SerializeObject(new { success = false, msg = "Saved draft is invalid." }));
            return;
        }

        record.ExistingCourseContentSnapshot = LearnSite.Common.AIActivityPlanPromptBuilder.BoundText(record.ExistingCourseContentSnapshot, 4000);
        if (course != null && string.IsNullOrEmpty(record.ExistingCourseContentSnapshot))
        {
            record.ExistingCourseContentSnapshot = LearnSite.Common.AIActivityPlanPromptBuilder.BoundText(course.Ccontent, 4000);
        }

        LearnSite.BLL.CourseActivityPlanDraft draftBll = new LearnSite.BLL.CourseActivityPlanDraft();
        bool saved = draftBll.UpsertCurrent(record);
        context.Response.Write(JsonConvert.SerializeObject(new
        {
            success = saved,
            msg = saved ? "Saved draft updated." : "Failed to save draft."
        }));
    }

    private void ActivityPlanLoadDraft(HttpContext context)
    {
        int cid;
        if (!TryGetAuthorizedCourse(context, out cid, out _))
        {
            return;
        }

        LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();
        LearnSite.BLL.CourseActivityPlanDraft draftBll = new LearnSite.BLL.CourseActivityPlanDraft();
        LearnSite.Model.CourseActivityPlanDraft record = draftBll.GetCurrentByCourse(cid, tcook.Hid);
        LearnSite.Common.ActivityPlanSavedDraftPayload payload = LearnSite.Common.AIActivityPlanSavedDraftHelper.ParseRecord(record, cid, tcook.Hid);
        if (payload == null)
        {
            context.Response.Write(JsonConvert.SerializeObject(new { success = false, msg = "Saved draft is invalid." }));
            return;
        }

        context.Response.Write(JsonConvert.SerializeObject(new
        {
            success = true,
            data = new
            {
                topic = payload.Topic,
                grade = payload.Grade,
                duration = payload.Duration,
                teachingGoals = payload.TeachingGoals,
                existingCourseContent = payload.ExistingCourseContent,
                updatedAt = payload.UpdatedAt.ToString("s"),
                draft = new
                {
                    teachingGoals = payload.Draft.TeachingGoals,
                    activitySteps = payload.Draft.ActivitySteps.Select(step => new
                    {
                        sort = step.Sort,
                        title = step.Title,
                        minutes = step.Minutes,
                        teacherAction = step.TeacherAction,
                        studentAction = step.StudentAction,
                        interactionMethod = step.InteractionMethod,
                        resourceSuggestion = step.ResourceSuggestion,
                        assessmentCheck = step.AssessmentCheck
                    }).ToList(),
                    resources = payload.Draft.Resources,
                    assessment = payload.Draft.Assessment,
                    teacherReminder = payload.Draft.TeacherReminder
                }
            }
        }));
    }

    private void ActivityPlanDeleteDraft(HttpContext context)
    {
        int cid;
        if (!TryGetAuthorizedCourse(context, out cid, out _))
        {
            return;
        }

        LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();
        LearnSite.BLL.CourseActivityPlanDraft draftBll = new LearnSite.BLL.CourseActivityPlanDraft();
        bool deleted = draftBll.DeleteCurrent(cid, tcook.Hid);
        context.Response.Write(JsonConvert.SerializeObject(new
        {
            success = deleted,
            msg = deleted ? "Saved draft deleted." : "Saved draft not found."
        }));
    }

    private void ActivityPlanPublish(HttpContext context)
    {
        int cid;
        LearnSite.Model.Courses course;
        if (!TryGetAuthorizedCourse(context, out cid, out course))
        {
            return;
        }

        List<string> selectedSections = ParseSelectedSections(context.Request["selectedSections"]);
        if (selectedSections == null || selectedSections.Count == 0)
        {
            context.Response.Write(JsonConvert.SerializeObject(new { success = false, msg = "Selected sections are required." }));
            return;
        }

        LearnSite.Common.ActivityPlanDraft draft = LearnSite.Common.AIActivityPlanDraftHelper.ParseDraft(context.Request["currentDraft"]);
        if (!LearnSite.Common.AIActivityPlanDraftHelper.IsValidDraft(draft))
        {
            context.Response.Write(JsonConvert.SerializeObject(new { success = false, msg = "Current draft is invalid." }));
            return;
        }

        LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();
        bool publishToStudents = string.Equals(context.Request["publishToStudents"], "true", StringComparison.OrdinalIgnoreCase)
            || string.Equals(context.Request["publishToStudents"], "1", StringComparison.OrdinalIgnoreCase);

        LearnSite.Model.AIActivityPlanPublishRequest request = new LearnSite.Model.AIActivityPlanPublishRequest
        {
            Cid = cid,
            Hid = tcook.Hid,
            Topic = context.Request["topic"],
            PublishToStudents = publishToStudents,
            SelectedSectionKeys = selectedSections,
            Draft = draft
        };

        LearnSite.Model.AIActivityPlanPublishResult result = new LearnSite.BLL.AIActivityPlanPublisher().Publish(request);
        if (result == null)
        {
            context.Response.Write(JsonConvert.SerializeObject(new { success = false, msg = "活动计划发布失败。" }));
            return;
        }

        context.Response.Write(JsonConvert.SerializeObject(new
        {
            success = true,
            data = new
            {
                missionId = result.MissionId,
                listMenuId = result.ListMenuId,
                missionTitle = result.MissionTitle,
                publishedToStudents = result.PublishedToStudents,
                updatedCourseContent = result.UpdatedCourseContent
            }
        }));
    }

    private void FullLessonGenerate(HttpContext context)
    {
        int cid;
        LearnSite.Model.Courses course;
        if (!TryGetAuthorizedCourse(context, out cid, out course))
        {
            return;
        }

        string topic = LearnSite.Common.AIActivityPlanPromptBuilder.BoundText(context.Request["topic"], LearnSite.Common.AIActivityPlanPromptBuilder.MaxTopicLength);
        if (string.IsNullOrEmpty(topic))
        {
            context.Response.Write(JsonConvert.SerializeObject(new { success = false, msg = "Topic is required." }));
            return;
        }

        LearnSite.Common.AIActivityPlanPromptRequest request = new LearnSite.Common.AIActivityPlanPromptRequest
        {
            Topic = topic,
            Grade = LearnSite.Common.AIActivityPlanPromptBuilder.BoundText(context.Request["grade"], LearnSite.Common.AIActivityPlanPromptBuilder.MaxGradeLength),
            Duration = LearnSite.Common.AIActivityPlanPromptBuilder.BoundText(context.Request["duration"], LearnSite.Common.AIActivityPlanPromptBuilder.MaxDurationLength),
            TeachingGoals = LearnSite.Common.AIActivityPlanPromptBuilder.BoundText(context.Request["teachingGoals"], LearnSite.Common.AIActivityPlanPromptBuilder.MaxTeachingGoalsLength),
            ExistingCourseContent = LearnSite.Common.AIActivityPlanPromptBuilder.BoundText(context.Request["existingCourseContent"], 4000)
        };

        LearnSite.BLL.AIActivityPlanDraftGenerator generator = new LearnSite.BLL.AIActivityPlanDraftGenerator();
        LearnSite.BLL.ActivityPlanDraftGenerationResult result = generator.Generate(request);
        if (!result.Success || result.Draft == null)
        {
            context.Response.Write(JsonConvert.SerializeObject(new
            {
                success = false,
                msg = result == null ? "整课草案生成失败。" : result.Message,
                providerDisplayName = result == null ? string.Empty : result.ProviderDisplayName,
                skillName = result == null ? string.Empty : result.SkillName
            }));
            return;
        }

        LearnSite.Common.FullLessonDraft fullLessonDraft = BuildFullLessonDraftFromActivityPlan(topic, request.Duration, result.Draft);
        if (!LearnSite.Common.AIActivityPlanDraftHelper.IsValidFullLessonDraft(fullLessonDraft))
        {
            context.Response.Write(JsonConvert.SerializeObject(new { success = false, msg = "整课草案结果无效。" }));
            return;
        }

        context.Response.Write(JsonConvert.SerializeObject(new
        {
            success = true,
            data = new
            {
                providerDisplayName = result.ProviderDisplayName,
                skillName = result.SkillName,
                message = "整课活动草案已生成。",
                draft = SerializeFullLessonDraft(fullLessonDraft)
            }
        }));
    }

    private void FullLessonRegenerateBlock(HttpContext context)
    {
        int cid;
        if (!TryGetAuthorizedCourse(context, out cid, out _))
        {
            return;
        }

        string topic = LearnSite.Common.AIActivityPlanPromptBuilder.BoundText(context.Request["topic"], LearnSite.Common.AIActivityPlanPromptBuilder.MaxTopicLength);
        if (string.IsNullOrEmpty(topic))
        {
            context.Response.Write(JsonConvert.SerializeObject(new { success = false, msg = "Topic is required." }));
            return;
        }

        string blockKey = LearnSite.Common.AIActivityPlanPromptBuilder.BoundText(context.Request["blockKey"], 100);
        if (string.IsNullOrEmpty(blockKey))
        {
            context.Response.Write(JsonConvert.SerializeObject(new { success = false, msg = "Block key is required." }));
            return;
        }

        LearnSite.Common.FullLessonDraft currentDraft = LearnSite.Common.AIActivityPlanDraftHelper.ParseFullLessonDraft(context.Request["currentDraft"]);
        if (!LearnSite.Common.AIActivityPlanDraftHelper.IsValidFullLessonDraft(currentDraft))
        {
            context.Response.Write(JsonConvert.SerializeObject(new { success = false, msg = "Current full lesson draft is invalid." }));
            return;
        }

        bool blockExists = currentDraft.Blocks.Any(block => string.Equals(block.BlockKey, blockKey, StringComparison.OrdinalIgnoreCase));
        if (!blockExists)
        {
            context.Response.Write(JsonConvert.SerializeObject(new { success = false, msg = "Block key is invalid." }));
            return;
        }

        LearnSite.Common.AIActivityPlanPromptRequest request = new LearnSite.Common.AIActivityPlanPromptRequest
        {
            Topic = topic,
            Grade = LearnSite.Common.AIActivityPlanPromptBuilder.BoundText(context.Request["grade"], LearnSite.Common.AIActivityPlanPromptBuilder.MaxGradeLength),
            Duration = LearnSite.Common.AIActivityPlanPromptBuilder.BoundText(context.Request["duration"], LearnSite.Common.AIActivityPlanPromptBuilder.MaxDurationLength),
            TeachingGoals = LearnSite.Common.AIActivityPlanPromptBuilder.BoundText(context.Request["teachingGoals"], LearnSite.Common.AIActivityPlanPromptBuilder.MaxTeachingGoalsLength),
            ExistingCourseContent = LearnSite.Common.AIActivityPlanPromptBuilder.BoundText(context.Request["existingCourseContent"], 4000)
        };

        LearnSite.BLL.AIActivityPlanDraftGenerator generator = new LearnSite.BLL.AIActivityPlanDraftGenerator();
        LearnSite.BLL.ActivityPlanDraftGenerationResult result = generator.Generate(request);
        if (!result.Success || result.Draft == null)
        {
            context.Response.Write(JsonConvert.SerializeObject(new
            {
                success = false,
                msg = result == null ? "整课草案环节重生成失败。" : result.Message,
                providerDisplayName = result == null ? string.Empty : result.ProviderDisplayName,
                skillName = result == null ? string.Empty : result.SkillName
            }));
            return;
        }

        LearnSite.Common.FullLessonDraft generatedDraft = BuildFullLessonDraftFromActivityPlan(topic, request.Duration, result.Draft);
        LearnSite.Common.FullLessonDraft mergedDraft = ReplaceFullLessonBlock(currentDraft, generatedDraft, blockKey);
        if (!LearnSite.Common.AIActivityPlanDraftHelper.IsValidFullLessonDraft(mergedDraft))
        {
            context.Response.Write(JsonConvert.SerializeObject(new { success = false, msg = "整课草案环节结果无效。" }));
            return;
        }

        context.Response.Write(JsonConvert.SerializeObject(new
        {
            success = true,
            data = new
            {
                providerDisplayName = result.ProviderDisplayName,
                skillName = result.SkillName,
                message = "整课草案指定环节已更新。",
                draft = SerializeFullLessonDraft(mergedDraft)
            }
        }));
    }

    private void FullLessonDraftStatus(HttpContext context)
    {
        int cid;
        if (!TryGetAuthorizedCourse(context, out cid, out _))
        {
            return;
        }

        LearnSite.BLL.CourseActivityPlanDraft draftBll = new LearnSite.BLL.CourseActivityPlanDraft();
        LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();
        LearnSite.Model.CourseActivityPlanDraft record = draftBll.GetCurrentByCourse(cid, tcook.Hid);
        LearnSite.Common.ActivityPlanSavedDraftPayload payload = LearnSite.Common.AIActivityPlanSavedDraftHelper.ParseFullLessonRecord(record, cid, tcook.Hid);

        context.Response.Write(JsonConvert.SerializeObject(new
        {
            success = true,
            data = new
            {
                hasDraft = payload != null,
                updatedAt = payload == null ? string.Empty : payload.UpdatedAt.ToString("s")
            }
        }));
    }

    private void FullLessonSaveDraft(HttpContext context)
    {
        int cid;
        LearnSite.Model.Courses course;
        if (!TryGetAuthorizedCourse(context, out cid, out course))
        {
            return;
        }

        LearnSite.Common.FullLessonDraft draft = LearnSite.Common.AIActivityPlanDraftHelper.ParseFullLessonDraft(context.Request["currentDraft"]);
        LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();
        LearnSite.Model.CourseActivityPlanDraft record = LearnSite.Common.AIActivityPlanSavedDraftHelper.BuildFullLessonRecord(
            cid,
            tcook.Hid,
            context.Request["topic"],
            context.Request["grade"],
            context.Request["duration"],
            context.Request["teachingGoals"],
            context.Request["existingCourseContent"],
            draft);

        if (record == null)
        {
            context.Response.Write(JsonConvert.SerializeObject(new { success = false, msg = "Saved full lesson draft is invalid." }));
            return;
        }

        record.ExistingCourseContentSnapshot = LearnSite.Common.AIActivityPlanPromptBuilder.BoundText(record.ExistingCourseContentSnapshot, 4000);
        if (course != null && string.IsNullOrEmpty(record.ExistingCourseContentSnapshot))
        {
            record.ExistingCourseContentSnapshot = LearnSite.Common.AIActivityPlanPromptBuilder.BoundText(course.Ccontent, 4000);
        }

        LearnSite.BLL.CourseActivityPlanDraft draftBll = new LearnSite.BLL.CourseActivityPlanDraft();
        bool saved = draftBll.UpsertCurrent(record);
        context.Response.Write(JsonConvert.SerializeObject(new
        {
            success = saved,
            msg = saved ? "Saved full lesson draft updated." : "Failed to save full lesson draft."
        }));
    }

    private void FullLessonLoadDraft(HttpContext context)
    {
        int cid;
        if (!TryGetAuthorizedCourse(context, out cid, out _))
        {
            return;
        }

        LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();
        LearnSite.BLL.CourseActivityPlanDraft draftBll = new LearnSite.BLL.CourseActivityPlanDraft();
        LearnSite.Model.CourseActivityPlanDraft record = draftBll.GetCurrentByCourse(cid, tcook.Hid);
        LearnSite.Common.ActivityPlanSavedDraftPayload payload = LearnSite.Common.AIActivityPlanSavedDraftHelper.ParseFullLessonRecord(record, cid, tcook.Hid);
        if (payload == null || payload.FullLessonDraft == null)
        {
            context.Response.Write(JsonConvert.SerializeObject(new { success = false, msg = "Saved full lesson draft is invalid." }));
            return;
        }

        context.Response.Write(JsonConvert.SerializeObject(new
        {
            success = true,
            data = new
            {
                topic = payload.Topic,
                grade = payload.Grade,
                duration = payload.Duration,
                teachingGoals = payload.TeachingGoals,
                existingCourseContent = payload.ExistingCourseContent,
                updatedAt = payload.UpdatedAt.ToString("s"),
                draft = new
                {
                    schemaVersion = payload.FullLessonDraft.SchemaVersion,
                    topic = payload.FullLessonDraft.Topic,
                    lessonSummary = payload.FullLessonDraft.LessonSummary,
                    totalMinutes = payload.FullLessonDraft.TotalMinutes,
                    blocks = payload.FullLessonDraft.Blocks.Select(block => new
                    {
                        blockKey = block.BlockKey,
                        sort = block.Sort,
                        blockType = block.BlockType,
                        title = block.Title,
                        minutes = block.Minutes,
                        teachingPurpose = block.TeachingPurpose,
                        lessonPosition = block.LessonPosition,
                        teacherAction = block.TeacherAction,
                        studentAction = block.StudentAction,
                        materials = block.Materials,
                        assessmentFocus = block.AssessmentFocus,
                        status = block.Status
                    }).ToList()
                }
            }
        }));
    }

    private void FullLessonDeleteDraft(HttpContext context)
    {
        int cid;
        if (!TryGetAuthorizedCourse(context, out cid, out _))
        {
            return;
        }

        LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();
        LearnSite.BLL.CourseActivityPlanDraft draftBll = new LearnSite.BLL.CourseActivityPlanDraft();
        LearnSite.Model.CourseActivityPlanDraft record = draftBll.GetCurrentByCourse(cid, tcook.Hid);
        LearnSite.Common.ActivityPlanSavedDraftPayload payload = LearnSite.Common.AIActivityPlanSavedDraftHelper.ParseFullLessonRecord(record, cid, tcook.Hid);
        if (payload == null)
        {
            context.Response.Write(JsonConvert.SerializeObject(new
            {
                success = false,
                msg = "Saved full lesson draft not found."
            }));
            return;
        }

        bool deleted = draftBll.DeleteCurrent(cid, tcook.Hid);
        context.Response.Write(JsonConvert.SerializeObject(new
        {
            success = deleted,
            msg = deleted ? "Saved full lesson draft deleted." : "Saved full lesson draft not found."
        }));
    }

    private List<string> ParseSelectedSections(string selectedSectionsJson)
    {
        if (string.IsNullOrEmpty(selectedSectionsJson))
        {
            return new List<string>();
        }

        try
        {
            JArray sections = JArray.Parse(selectedSectionsJson);
            List<string> result = new List<string>();
            foreach (JToken section in sections)
            {
                string key = section == null ? string.Empty : section.ToString();
                if (LearnSite.Common.AIActivityPlanDraftHelper.IsSupportedSectionTarget(key))
                {
                    result.Add(key);
                }
            }

            return result;
        }
        catch
        {
            return new List<string>();
        }
    }

    private bool TryGetAuthorizedCourse(HttpContext context, out int cid, out LearnSite.Model.Courses course)
    {
        cid = 0;
        course = null;

        if (!int.TryParse(context.Request["cid"], out cid) || cid <= 0)
        {
            context.Response.Write(JsonConvert.SerializeObject(new { success = false, msg = "Course id is invalid." }));
            return false;
        }

        LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();
        if (tcook == null || tcook.Hid <= 0)
        {
            context.Response.Write(JsonConvert.SerializeObject(new { success = false, msg = "Unauthorized" }));
            return false;
        }

        LearnSite.BLL.Courses coursesBll = new LearnSite.BLL.Courses();
        course = coursesBll.GetModel(cid);
        if (course == null || course.Chid.GetValueOrDefault() != tcook.Hid)
        {
            context.Response.Write(JsonConvert.SerializeObject(new { success = false, msg = "Course access denied." }));
            return false;
        }

        return true;
    }

    private object SerializeFullLessonDraft(LearnSite.Common.FullLessonDraft draft)
    {
        if (draft == null)
        {
            return null;
        }

        return new
        {
            schemaVersion = draft.SchemaVersion,
            topic = draft.Topic,
            lessonSummary = draft.LessonSummary,
            totalMinutes = draft.TotalMinutes,
            blocks = draft.Blocks.Select(block => new
            {
                blockKey = block.BlockKey,
                sort = block.Sort,
                blockType = block.BlockType,
                title = block.Title,
                minutes = block.Minutes,
                teachingPurpose = block.TeachingPurpose,
                lessonPosition = block.LessonPosition,
                teacherAction = block.TeacherAction,
                studentAction = block.StudentAction,
                materials = block.Materials,
                assessmentFocus = block.AssessmentFocus,
                status = block.Status
            }).ToList()
        };
    }

    private LearnSite.Common.FullLessonDraft BuildFullLessonDraftFromActivityPlan(string topic, string duration, LearnSite.Common.ActivityPlanDraft planDraft)
    {
        if (!LearnSite.Common.AIActivityPlanDraftHelper.IsValidDraft(planDraft))
        {
            return null;
        }

        LearnSite.Common.FullLessonDraft fullLessonDraft = new LearnSite.Common.FullLessonDraft();
        fullLessonDraft.Topic = LearnSite.Common.AIActivityPlanPromptBuilder.BoundText(topic, LearnSite.Common.AIActivityPlanPromptBuilder.MaxTopicLength);
        fullLessonDraft.LessonSummary = BuildFullLessonSummary(planDraft);
        fullLessonDraft.Blocks = new List<LearnSite.Common.FullLessonDraftBlock>();

        if (planDraft.TeachingGoals != null && planDraft.TeachingGoals.Count > 0)
        {
            fullLessonDraft.Blocks.Add(new LearnSite.Common.FullLessonDraftBlock
            {
                BlockKey = "teaching-goals-1",
                Sort = 1,
                BlockType = "mission",
                Title = "教学目标对齐",
                Minutes = "5分钟",
                TeachingPurpose = "明确本课学习目标",
                LessonPosition = "导入",
                TeacherAction = string.Join("；", planDraft.TeachingGoals.ToArray()),
                StudentAction = "阅读并确认本课目标，带着问题进入学习。",
                Materials = new List<string> { "目标提示", "导学问题" },
                AssessmentFocus = "学生能说出本课核心学习目标",
                Status = "draft"
            });
        }

        if (planDraft.ActivitySteps != null)
        {
            for (int i = 0; i < planDraft.ActivitySteps.Count; i++)
            {
                LearnSite.Common.ActivityPlanDraftStep step = planDraft.ActivitySteps[i];
                if (step == null)
                {
                    continue;
                }

                fullLessonDraft.Blocks.Add(new LearnSite.Common.FullLessonDraftBlock
                {
                    BlockKey = "activity-step-" + (i + 1).ToString(),
                    Sort = fullLessonDraft.Blocks.Count + 1,
                    BlockType = i == planDraft.ActivitySteps.Count - 1 ? "quiz" : "mission",
                    Title = string.IsNullOrEmpty(step.Title) ? "课堂活动" : step.Title,
                    Minutes = step.Minutes,
                    TeachingPurpose = GetTeachingPurposeForStep(step, i),
                    LessonPosition = GetLessonPositionForStep(i, planDraft.ActivitySteps.Count),
                    TeacherAction = step.TeacherAction,
                    StudentAction = step.StudentAction,
                    Materials = BuildBlockMaterials(step.ResourceSuggestion),
                    AssessmentFocus = step.AssessmentCheck,
                    Status = "draft"
                });
            }
        }

        if (planDraft.Resources != null && planDraft.Resources.Count > 0)
        {
            fullLessonDraft.Blocks.Add(new LearnSite.Common.FullLessonDraftBlock
            {
                BlockKey = "resource-study-1",
                Sort = fullLessonDraft.Blocks.Count + 1,
                BlockType = "ware",
                Title = "资源学习支持",
                Minutes = "5分钟",
                TeachingPurpose = "补充关键资源支持课堂推进",
                LessonPosition = "拓展",
                TeacherAction = "引导学生结合资源完成巩固或拓展。",
                StudentAction = string.Join("；", planDraft.Resources.ToArray()),
                Materials = new List<string>(planDraft.Resources),
                AssessmentFocus = planDraft.Assessment != null && planDraft.Assessment.Count > 0 ? string.Join("；", planDraft.Assessment.ToArray()) : "关注学生对资源的理解与应用",
                Status = "draft"
            });
        }

        if (fullLessonDraft.Blocks.Count == 0)
        {
            return null;
        }

        for (int index = 0; index < fullLessonDraft.Blocks.Count; index++)
        {
            fullLessonDraft.Blocks[index].Sort = index + 1;
        }

        fullLessonDraft.TotalMinutes = GetFullLessonTotalMinutes(duration, fullLessonDraft.Blocks);

        return fullLessonDraft;
    }

    private LearnSite.Common.FullLessonDraft ReplaceFullLessonBlock(LearnSite.Common.FullLessonDraft currentDraft, LearnSite.Common.FullLessonDraft generatedDraft, string blockKey)
    {
        if (!LearnSite.Common.AIActivityPlanDraftHelper.IsValidFullLessonDraft(currentDraft)
            || !LearnSite.Common.AIActivityPlanDraftHelper.IsValidFullLessonDraft(generatedDraft)
            || string.IsNullOrEmpty(blockKey))
        {
            return null;
        }

        LearnSite.Common.FullLessonDraft merged = new LearnSite.Common.FullLessonDraft();
        merged.SchemaVersion = currentDraft.SchemaVersion;
        merged.Topic = currentDraft.Topic;
        merged.LessonSummary = generatedDraft.LessonSummary;
        merged.TotalMinutes = currentDraft.TotalMinutes;
        merged.Blocks = new List<LearnSite.Common.FullLessonDraftBlock>();

        LearnSite.Common.FullLessonDraftBlock currentBlock = currentDraft.Blocks.FirstOrDefault(block =>
            block != null && string.Equals(block.BlockKey, blockKey, StringComparison.OrdinalIgnoreCase));
        if (currentBlock == null)
        {
            return null;
        }

        LearnSite.Common.FullLessonDraftBlock replacement = generatedDraft.Blocks.FirstOrDefault(block =>
            block != null && string.Equals(block.BlockKey, blockKey, StringComparison.OrdinalIgnoreCase));
        if (replacement == null)
        {
            replacement = generatedDraft.Blocks.FirstOrDefault(block =>
                block != null
                && string.Equals(block.BlockType, currentBlock.BlockType, StringComparison.OrdinalIgnoreCase)
                && string.Equals(block.LessonPosition, currentBlock.LessonPosition, StringComparison.OrdinalIgnoreCase));
        }
        if (replacement == null)
        {
            replacement = generatedDraft.Blocks.FirstOrDefault(block =>
                block != null && string.Equals(block.BlockType, currentBlock.BlockType, StringComparison.OrdinalIgnoreCase));
        }
        if (replacement == null)
        {
            return null;
        }

        bool replaced = false;
        foreach (LearnSite.Common.FullLessonDraftBlock block in currentDraft.Blocks)
        {
            if (block == null)
            {
                continue;
            }

            if (string.Equals(block.BlockKey, blockKey, StringComparison.OrdinalIgnoreCase))
            {
                LearnSite.Common.FullLessonDraftBlock nextBlock = new LearnSite.Common.FullLessonDraftBlock
                {
                    BlockKey = block.BlockKey,
                    Sort = merged.Blocks.Count + 1,
                    BlockType = replacement.BlockType,
                    Title = replacement.Title,
                    Minutes = replacement.Minutes,
                    TeachingPurpose = replacement.TeachingPurpose,
                    LessonPosition = block.LessonPosition,
                    TeacherAction = replacement.TeacherAction,
                    StudentAction = replacement.StudentAction,
                    Materials = replacement.Materials == null ? new List<string>() : new List<string>(replacement.Materials),
                    AssessmentFocus = replacement.AssessmentFocus,
                    Status = "draft"
                };
                merged.Blocks.Add(nextBlock);
                replaced = true;
            }
            else
            {
                merged.Blocks.Add(new LearnSite.Common.FullLessonDraftBlock
                {
                    BlockKey = block.BlockKey,
                    Sort = merged.Blocks.Count + 1,
                    BlockType = block.BlockType,
                    Title = block.Title,
                    Minutes = block.Minutes,
                    TeachingPurpose = block.TeachingPurpose,
                    LessonPosition = block.LessonPosition,
                    TeacherAction = block.TeacherAction,
                    StudentAction = block.StudentAction,
                    Materials = block.Materials == null ? new List<string>() : new List<string>(block.Materials),
                    AssessmentFocus = block.AssessmentFocus,
                    Status = block.Status
                });
            }
        }

        return replaced ? merged : null;
    }

    private string BuildFullLessonSummary(LearnSite.Common.ActivityPlanDraft draft)
    {
        List<string> parts = new List<string>();
        if (draft.TeachingGoals != null && draft.TeachingGoals.Count > 0)
        {
            parts.Add("围绕“" + draft.TeachingGoals[0] + "”组织整课活动。"
                + (draft.TeachingGoals.Count > 1 ? "兼顾“" + draft.TeachingGoals[draft.TeachingGoals.Count - 1] + "”。" : string.Empty));
        }

        if (draft.ActivitySteps != null && draft.ActivitySteps.Count > 0)
        {
            parts.Add("课堂按 " + draft.ActivitySteps.Count.ToString() + " 个核心环节推进。\n");
        }

        return string.Join(string.Empty, parts.ToArray()).Replace("\n", string.Empty).Trim();
    }

    private string GetFullLessonTotalMinutes(string duration, List<LearnSite.Common.FullLessonDraftBlock> blocks)
    {
        string normalized = LearnSite.Common.AIActivityPlanPromptBuilder.BoundText(duration, LearnSite.Common.AIActivityPlanPromptBuilder.MaxDurationLength);
        if (!string.IsNullOrEmpty(normalized))
        {
            return normalized;
        }

        int total = 0;
        if (blocks != null)
        {
            foreach (LearnSite.Common.FullLessonDraftBlock block in blocks)
            {
                if (block == null || string.IsNullOrEmpty(block.Minutes))
                {
                    continue;
                }

                string digits = new string(block.Minutes.Where(char.IsDigit).ToArray());
                int minutes;
                if (int.TryParse(digits, out minutes))
                {
                    total += minutes;
                }
            }
        }

        return total > 0 ? total.ToString() + "分钟" : "40分钟";
    }

    private string GetTeachingPurposeForStep(LearnSite.Common.ActivityPlanDraftStep step, int index)
    {
        if (step == null)
        {
            return "推进课堂学习";
        }

        if (!string.IsNullOrEmpty(step.AssessmentCheck))
        {
            return step.AssessmentCheck;
        }

        if (index == 0)
        {
            return "激活旧知并建立学习情境";
        }

        return "推进核心学习任务";
    }

    private string GetLessonPositionForStep(int index, int totalCount)
    {
        if (index == 0)
        {
            return "导入";
        }

        if (index == totalCount - 1)
        {
            return "总结";
        }

        return "展开";
    }

    private List<string> BuildBlockMaterials(string resourceSuggestion)
    {
        List<string> items = new List<string>();
        if (!string.IsNullOrEmpty(resourceSuggestion))
        {
            string[] segments = resourceSuggestion.Split(new[] { '；', ';', '、' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string segment in segments)
            {
                string value = segment == null ? string.Empty : segment.Trim();
                if (!string.IsNullOrEmpty(value) && !items.Contains(value))
                {
                    items.Add(value);
                }
            }
        }

        if (items.Count == 0)
        {
            items.Add("课堂活动单");
        }

        return items;
    }

    private void WriteAiChatResponse(HttpContext context, string prompt, double temperature, int maxTokens, string errorPrefix)
    {
        LearnSite.Model.AIProvider defaultProvider = GetDefaultProvider();

        if (defaultProvider == null)
        {
            string respStr = JsonConvert.SerializeObject(new { success = false, msg = "No AI provider configured. Please configure an AI provider first." });
            context.Response.Write(respStr);
            return;
        }

        try
        {
            string chatUrl = defaultProvider.BaseUrl.TrimEnd('/') + "/chat/completions";
            
            var requestBody = new
            {
                model = defaultProvider.ModelName,
                temperature = temperature,
                max_tokens = maxTokens,
                messages = new[]
                {
                    new { role = "user", content = prompt }
                }
            };
            
            string payload = JsonConvert.SerializeObject(requestBody);
            
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(chatUrl);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Timeout = 120000; // 120 seconds timeout for generation
            request.ReadWriteTimeout = 120000;
            
            if (!string.IsNullOrEmpty(defaultProvider.ApiKey))
            {
                request.Headers.Add("Authorization", "Bearer " + defaultProvider.ApiKey);
            }

            byte[] byteArray = Encoding.UTF8.GetBytes(payload);
            request.ContentLength = byteArray.Length;

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            string responseFromServer = reader.ReadToEnd();
                            JObject jsonResp = JsonConvert.DeserializeObject<JObject>(responseFromServer);
                            if (jsonResp != null && jsonResp["choices"] != null && ((JArray)jsonResp["choices"]).Count > 0)
                            {
                                string contentResult = jsonResp["choices"][0]["message"]["content"].ToString();
                                string safeContent = JsonConvert.SerializeObject(new { success = true, data = contentResult });
                                context.Response.Write(safeContent);
                            }
                            else
                            {
                                string respStr = JsonConvert.SerializeObject(new { success = false, msg = "Invalid response format from AI Provider." });
                                context.Response.Write(respStr);
                            }
                        }
                    }
                }
                else
                {
                    string respStr = JsonConvert.SerializeObject(new { success = false, msg = "HTTP Error: " + response.StatusCode });
                    context.Response.Write(respStr);
                }
            }
        }
        catch (WebException wex)
        {
            string errorMsg = wex.Message;
            if (wex.Response != null)
            {
                using (HttpWebResponse errorResponse = (HttpWebResponse)wex.Response)
                {
                    errorMsg += " Status code: " + (int)errorResponse.StatusCode;
                    using (Stream responseStream = errorResponse.GetResponseStream())
                    {
                        if (responseStream != null)
                        {
                            using (StreamReader reader = new StreamReader(responseStream))
                            {
                                errorMsg += " Details: " + reader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            string respStr = JsonConvert.SerializeObject(new { success = false, msg = "Connection failed: " + errorMsg });
            context.Response.Write(respStr);
        }
        catch (Exception ex)
        {
            string respStr = JsonConvert.SerializeObject(new { success = false, msg = errorPrefix + ex.Message });
            context.Response.Write(respStr);
        }
    }

    private LearnSite.Model.AIProvider GetDefaultProvider()
    {
        LearnSite.BLL.AIProvider bll = new LearnSite.BLL.AIProvider();
        List<LearnSite.Model.AIProvider> providers = bll.GetModelList("");
        LearnSite.Model.AIProvider defaultProvider = providers.FirstOrDefault(p => p.IsDefault);

        if (defaultProvider == null && providers.Count > 0)
        {
            defaultProvider = providers[0];
        }

        return defaultProvider;
    }

    private void GetSkillList(HttpContext context)
    {
        LearnSite.BLL.AISkill bll = new LearnSite.BLL.AISkill();
        List<LearnSite.Model.AISkill> list = bll.GetModelList("");

        string json = JsonConvert.SerializeObject(new { success = true, data = list });
        context.Response.Write(json);
    }

    private void SaveSkill(HttpContext context)
    {
        string idStr = context.Request["id"];
        string skillName = context.Request["skillName"];
        string promptContent = context.Request["promptContent"];
        string isActiveStr = context.Request["isActive"];

        if (string.IsNullOrEmpty(skillName) || string.IsNullOrEmpty(promptContent))
        {
            context.Response.Write("{\"success\":false,\"msg\":\"Skill Name and Prompt Content are required.\"}");
            return;
        }

        LearnSite.Model.AISkill model = new LearnSite.Model.AISkill();
        model.SkillName = skillName;
        model.PromptContent = promptContent;
        model.IsActive = string.IsNullOrEmpty(isActiveStr) ? true : (isActiveStr.ToLower() == "true" || isActiveStr == "1");

        LearnSite.BLL.AISkill bll = new LearnSite.BLL.AISkill();

        if (string.IsNullOrEmpty(idStr) || idStr == "0")
        {
            int id = bll.Add(model);
            if (id > 0)
            {
                context.Response.Write("{\"success\":true,\"msg\":\"Added successfully.\"}");
            }
            else
            {
                context.Response.Write("{\"success\":false,\"msg\":\"Failed to add.\"}");
            }
        }
        else
        {
            int id = int.Parse(idStr);
            model.Id = id;
            if (bll.Update(model))
            {
                context.Response.Write("{\"success\":true,\"msg\":\"Updated successfully.\"}");
            }
            else
            {
                context.Response.Write("{\"success\":false,\"msg\":\"Failed to update.\"}");
            }
        }
    }

    private void DeleteSkill(HttpContext context)
    {
        string idStr = context.Request["id"];
        if (!string.IsNullOrEmpty(idStr))
        {
            int id = int.Parse(idStr);
            LearnSite.BLL.AISkill bll = new LearnSite.BLL.AISkill();
            if (bll.Delete(id))
            {
                context.Response.Write("{\"success\":true,\"msg\":\"Deleted successfully.\"}");
            }
            else
            {
                context.Response.Write("{\"success\":false,\"msg\":\"Failed to delete.\"}");
            }
        }
        else
        {
            context.Response.Write("{\"success\":false,\"msg\":\"Invalid ID.\"}");
        }
    }

    private void GetCustomSkillList(HttpContext context)
    {
        LearnSite.BLL.AIGaugeGenerator.EnsureDefaultGaugeSkill();
        LearnSite.BLL.AIStudentExamGenerator.EnsureDefaultStudentExamSkill();
        LearnSite.BLL.AICustomSkill bll = new LearnSite.BLL.AICustomSkill();
        List<LearnSite.Model.AICustomSkill> list = bll.GetModelList("");
        string json = JsonConvert.SerializeObject(new { success = true, data = list });
        context.Response.Write(json);
    }

    private void SaveCustomSkill(HttpContext context)
    {
        string idStr = context.Request["id"];
        string skillName = context.Request["skillName"];
        string promptContent = context.Request["promptContent"];
        string skillScope = context.Request["skillScope"] ?? "";
        string isActiveStr = context.Request["isActive"];

        if (string.IsNullOrEmpty(skillName) || string.IsNullOrEmpty(promptContent))
        {
            context.Response.Write("{\"success\":false,\"msg\":\"技能名称和提示词内容不能为空。\"}");
            return;
        }

        LearnSite.Model.AICustomSkill model = new LearnSite.Model.AICustomSkill();
        model.SkillName = skillName;
        model.PromptContent = promptContent;
        model.SkillScope = skillScope;
        model.IsActive = string.IsNullOrEmpty(isActiveStr) ? true : (isActiveStr.ToLower() == "true" || isActiveStr == "1");

        LearnSite.BLL.AICustomSkill bll = new LearnSite.BLL.AICustomSkill();

        if (string.IsNullOrEmpty(idStr) || idStr == "0")
        {
            int id = bll.Add(model);
            if (id > 0)
                context.Response.Write("{\"success\":true,\"msg\":\"添加成功。\"}");
            else
                context.Response.Write("{\"success\":false,\"msg\":\"添加失败。\"}");
        }
        else
        {
            int id = int.Parse(idStr);
            model.Id = id;
            if (bll.Update(model))
                context.Response.Write("{\"success\":true,\"msg\":\"更新成功。\"}");
            else
                context.Response.Write("{\"success\":false,\"msg\":\"更新失败。\"}");
        }
    }

    private void DeleteCustomSkill(HttpContext context)
    {
        string idStr = context.Request["id"];
        if (!string.IsNullOrEmpty(idStr))
        {
            int id = int.Parse(idStr);
            LearnSite.BLL.AICustomSkill bll = new LearnSite.BLL.AICustomSkill();
            if (bll.Delete(id))
                context.Response.Write("{\"success\":true,\"msg\":\"删除成功。\"}");
            else
                context.Response.Write("{\"success\":false,\"msg\":\"删除失败。\"}");
        }
        else
        {
            context.Response.Write("{\"success\":false,\"msg\":\"无效 ID。\"}");
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }
}
