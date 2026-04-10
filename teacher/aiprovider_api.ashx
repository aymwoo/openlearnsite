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
