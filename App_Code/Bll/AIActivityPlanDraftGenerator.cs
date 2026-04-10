using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using LearnSite.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LearnSite.BLL
{
    public class ActivityPlanDraftGenerationResult
    {
        public bool Success { get; set; }
        public string ProviderDisplayName { get; set; }
        public string SkillName { get; set; }
        public string Message { get; set; }
        public ActivityPlanDraft Draft { get; set; }
    }

    public class AIActivityPlanDraftGenerator
    {
        public ActivityPlanDraftGenerationResult Generate(AIActivityPlanPromptRequest request)
        {
            return GenerateInternal(AIActivityPlanPromptBuilder.Build(request), false, null, null);
        }

        public ActivityPlanDraftGenerationResult RegenerateSection(AIActivityPlanSectionRegenerationRequest request)
        {
            if (request == null)
            {
                return new ActivityPlanDraftGenerationResult
                {
                    Success = false,
                    ProviderDisplayName = "未配置 AI Provider",
                    SkillName = GetDefaultSkillName(),
                    Message = "活动计划局部重生成失败。请求无效。"
                };
            }

            if (!AIActivityPlanDraftHelper.IsSupportedSectionTarget(request.SectionTarget))
            {
                return new ActivityPlanDraftGenerationResult
                {
                    Success = false,
                    ProviderDisplayName = "未配置 AI Provider",
                    SkillName = GetDefaultSkillName(),
                    Message = "活动计划局部重生成失败。不支持的章节。"
                };
            }

            if (!AIActivityPlanDraftHelper.IsValidDraft(request.CurrentDraft))
            {
                return new ActivityPlanDraftGenerationResult
                {
                    Success = false,
                    ProviderDisplayName = "未配置 AI Provider",
                    SkillName = GetDefaultSkillName(),
                    Message = "活动计划局部重生成失败。当前草案无效。"
                };
            }

            return GenerateInternal(AIActivityPlanPromptBuilder.BuildSectionRegeneration(request), true, request.CurrentDraft, request.SectionTarget);
        }

        private ActivityPlanDraftGenerationResult GenerateInternal(string userPrompt, bool isRegeneration, ActivityPlanDraft currentDraft, string sectionTarget)
        {
            AIActivityPlanSkillBootstrap.EnsureDefaultSkill();

            LearnSite.Model.AIProvider provider = GetDefaultProvider();
            string providerName = BuildProviderDisplayName(provider);
            if (provider == null || string.IsNullOrEmpty(provider.BaseUrl) || string.IsNullOrEmpty(provider.ModelName))
            {
                return new ActivityPlanDraftGenerationResult
                {
                    Success = false,
                    ProviderDisplayName = providerName,
                    SkillName = GetDefaultSkillName(),
                    Message = "未配置默认 AI Provider，请先完成配置。"
                };
            }

            LearnSite.Model.AICustomSkill skill = GetActivityPlanSkill();
            string skillName = skill == null ? GetDefaultSkillName() : skill.SkillName;
            string systemPrompt = skill == null ? AIActivityPlanSkillBootstrap.CreateDefaultSkillModel().PromptContent : skill.PromptContent;

            try
            {
                string responseText = CallProvider(provider, systemPrompt, userPrompt);
                ActivityPlanDraft draft = isRegeneration
                    ? AIActivityPlanDraftHelper.MergeRegeneratedSection(currentDraft, sectionTarget, responseText)
                    : AIActivityPlanDraftHelper.ParseDraft(responseText);

                if (!AIActivityPlanDraftHelper.IsValidDraft(draft))
                {
                    return new ActivityPlanDraftGenerationResult
                    {
                        Success = false,
                        ProviderDisplayName = providerName,
                        SkillName = skillName,
                        Message = isRegeneration
                            ? "AI 返回内容无法整理为有效的章节重生成结果，请稍后重试。"
                            : "AI 返回内容无法整理为完整的结构化活动计划，请稍后重试。"
                    };
                }

                return new ActivityPlanDraftGenerationResult
                {
                    Success = true,
                    ProviderDisplayName = providerName,
                    SkillName = skillName,
                    Message = isRegeneration ? "已更新指定章节的活动计划草案。" : "结构化活动计划草案已生成。",
                    Draft = draft
                };
            }
            catch (Exception ex)
            {
                return new ActivityPlanDraftGenerationResult
                {
                    Success = false,
                    ProviderDisplayName = providerName,
                    SkillName = skillName,
                    Message = (isRegeneration ? "活动计划局部重生成失败。" : "活动计划生成失败。") + CleanErrorMessage(ex.Message)
                };
            }
        }

        private LearnSite.Model.AICustomSkill GetActivityPlanSkill()
        {
            LearnSite.BLL.AICustomSkill skillBll = new LearnSite.BLL.AICustomSkill();
            List<LearnSite.Model.AICustomSkill> skills = skillBll.GetModelList("IsActive=1 and SkillScope like '%" + AIActivityPlanSkillBootstrap.ActivityPlanSkillScope + "%' ");
            if (skills == null || skills.Count == 0)
            {
                return null;
            }

            LearnSite.Model.AICustomSkill defaultSkill = skills.FirstOrDefault(s => s.SkillName == AIActivityPlanSkillBootstrap.CreateDefaultSkillModel().SkillName);
            return defaultSkill ?? skills[0];
        }

        private static LearnSite.Model.AIProvider GetDefaultProvider()
        {
            LearnSite.BLL.AIProvider providerBll = new LearnSite.BLL.AIProvider();
            List<LearnSite.Model.AIProvider> providers = providerBll.GetModelList("");
            if (providers == null || providers.Count == 0)
            {
                return null;
            }

            LearnSite.Model.AIProvider provider = providers.FirstOrDefault(p => p.IsDefault);
            return provider ?? providers[0];
        }

        private static string BuildProviderDisplayName(LearnSite.Model.AIProvider provider)
        {
            if (provider == null)
            {
                return "未配置 AI Provider";
            }
            if (!string.IsNullOrEmpty(provider.DisplayName))
            {
                return provider.DisplayName;
            }
            if (!string.IsNullOrEmpty(provider.ProviderName))
            {
                return provider.ProviderName;
            }
            return provider.ModelName ?? "默认模型";
        }

        private static string GetDefaultSkillName()
        {
            return AIActivityPlanSkillBootstrap.CreateDefaultSkillModel().SkillName;
        }

        private string CallProvider(LearnSite.Model.AIProvider provider, string systemPrompt, string userPrompt)
        {
            string chatUrl = provider.BaseUrl.TrimEnd('/') + "/chat/completions";
            var requestBody = new
            {
                model = provider.ModelName,
                temperature = 0.4,
                max_tokens = 1400,
                messages = new object[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = userPrompt }
                }
            };

            string payload = JsonConvert.SerializeObject(requestBody);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(chatUrl);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Timeout = 120000;
            request.ReadWriteTimeout = 120000;

            if (!string.IsNullOrEmpty(provider.ApiKey))
            {
                request.Headers.Add("Authorization", "Bearer " + provider.ApiKey);
            }

            byte[] body = Encoding.UTF8.GetBytes(payload);
            request.ContentLength = body.Length;
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(body, 0, body.Length);
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(responseStream))
            {
                string responseText = reader.ReadToEnd();
                JObject responseJson = JsonConvert.DeserializeObject<JObject>(responseText);
                if (responseJson == null || responseJson["choices"] == null || ((JArray)responseJson["choices"]).Count == 0)
                {
                    throw new Exception("AI 返回格式无效。");
                }

                JToken content = responseJson["choices"][0]["message"]["content"];
                return content == null ? string.Empty : content.ToString();
            }
        }

        private string CleanErrorMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return string.Empty;
            }

            return message.Replace("\r", " ").Replace("\n", " ").Replace("\"", "'");
        }
    }
}
