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
    public class GaugeGenerationResult
    {
        public GaugeGenerationResult()
        {
            Items = new List<LearnSite.Model.GaugeItem>();
        }

        public bool Success { get; set; }
        public bool UsedFallback { get; set; }
        public string Message { get; set; }
        public string ProviderDisplayName { get; set; }
        public List<LearnSite.Model.GaugeItem> Items { get; set; }
    }

    public class AIGaugeGenerator
    {
        public delegate void GaugeProgressReporter(string stage, string message);

        public static void EnsureDefaultGaugeSkill()
        {
            try
            {
                LearnSite.BLL.AICustomSkill skillBll = new LearnSite.BLL.AICustomSkill();
                List<LearnSite.Model.AICustomSkill> skills = skillBll.GetModelList("SkillScope like '%gauge%'");
                if (skills != null && skills.Count > 0)
                {
                    return;
                }

                LearnSite.Model.AICustomSkill model = new LearnSite.Model.AICustomSkill();
                model.SkillName = AIGaugeSkillHelper.GetDefaultGaugeSkillName();
                model.PromptContent = AIGaugeSkillHelper.GetDefaultGaugeSkillPrompt();
                model.SkillScope = "gauge";
                model.IsActive = true;
                skillBll.Add(model);
            }
            catch
            {
            }
        }

        public static string GetDefaultProviderDisplayName()
        {
            return BuildProviderDisplayName(GetDefaultProvider());
        }

        public GaugeGenerationResult Generate(string gaugeType, string gaugeTitle)
        {
            return Generate(gaugeType, gaugeTitle, null);
        }

        public GaugeGenerationResult Generate(string gaugeType, string gaugeTitle, GaugeProgressReporter progressReporter)
        {
            EnsureDefaultGaugeSkill();
            ReportProgress(progressReporter, "prepare", "正在检查 AI Provider 与默认技能配置...");

            LearnSite.Model.AIProvider provider = GetDefaultProvider();
            string providerName = BuildProviderDisplayName(provider);
            if (provider == null || string.IsNullOrEmpty(provider.BaseUrl) || string.IsNullOrEmpty(provider.ModelName))
            {
                ReportProgress(progressReporter, "fallback", "未配置默认 AI Provider，改为使用默认推荐模板。");
                return BuildFallbackResult(gaugeType, gaugeTitle, "未配置默认 AI Provider，已使用默认量规项。", providerName);
            }

            LearnSite.Model.AICustomSkill skill = GetGaugeSkill();
            string systemPrompt = skill == null ? AIGaugeSkillHelper.GetDefaultGaugeSkillPrompt() : skill.PromptContent;
            systemPrompt = AIGaugeSkillHelper.ReplaceSkillTokens(systemPrompt, gaugeType, gaugeTitle);

            string userPrompt = BuildUserPrompt(gaugeType, gaugeTitle);
            try
            {
                ReportProgress(progressReporter, "ai", "正在调用 AI 生成评价项...");
                string responseText = CallProvider(provider, systemPrompt, userPrompt);
                ReportProgress(progressReporter, "parse", "AI 已返回结果，正在整理量规项...");
                List<LearnSite.Model.GaugeItem> items = AIGaugeSkillHelper.ParseGaugeItems(responseText);
                if (items.Count == 0)
                {
                    ReportProgress(progressReporter, "fallback", "AI 返回内容无法解析，改为使用默认推荐模板。");
                    return BuildFallbackResult(gaugeType, gaugeTitle, "AI 返回内容无法解析，已使用默认量规项。", providerName);
                }

                ReportProgress(progressReporter, "ai_done", "AI 已生成 " + items.Count + " 条量规项。");
                return new GaugeGenerationResult
                {
                    Success = true,
                    UsedFallback = false,
                    Message = "已自动生成 " + items.Count + " 条量规项。",
                    ProviderDisplayName = providerName,
                    Items = items
                };
            }
            catch (Exception ex)
            {
                ReportProgress(progressReporter, "fallback", "AI 生成失败，改为使用默认推荐模板。");
                return BuildFallbackResult(gaugeType, gaugeTitle, "AI 生成失败，已使用默认量规项。" + CleanErrorMessage(ex.Message), providerName);
            }
        }

        public int SaveItems(int gaugeId, List<LearnSite.Model.GaugeItem> items)
        {
            return SaveItems(gaugeId, items, null);
        }

        public int SaveItems(int gaugeId, List<LearnSite.Model.GaugeItem> items, GaugeProgressReporter progressReporter)
        {
            if (gaugeId <= 0 || items == null || items.Count == 0)
            {
                return 0;
            }

            LearnSite.BLL.GaugeItem gaugeItemBll = new LearnSite.BLL.GaugeItem();
            int count = 0;
            int total = items.Count;
            foreach (LearnSite.Model.GaugeItem item in items)
            {
                ReportProgress(progressReporter, "save", "正在写入第 " + (count + 1) + " 条量规项，共 " + total + " 条...");
                item.Mgid = gaugeId;
                if (gaugeItemBll.Add(item) > 0)
                {
                    count++;
                }
            }
            ReportProgress(progressReporter, "save_done", "量规项写入完成，已写入 " + count + " / " + total + " 条。");
            return count;
        }

        private LearnSite.Model.AICustomSkill GetGaugeSkill()
        {
            LearnSite.BLL.AICustomSkill skillBll = new LearnSite.BLL.AICustomSkill();
            List<LearnSite.Model.AICustomSkill> skills = skillBll.GetModelList("IsActive=1 and SkillScope like '%gauge%'");
            if (skills == null || skills.Count == 0)
            {
                return null;
            }

            LearnSite.Model.AICustomSkill defaultSkill = skills.FirstOrDefault(s => s.SkillName == AIGaugeSkillHelper.GetDefaultGaugeSkillName());
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
                return "未配置 AI Provider";
            if (!string.IsNullOrEmpty(provider.DisplayName))
                return provider.DisplayName;
            if (!string.IsNullOrEmpty(provider.ProviderName))
                return provider.ProviderName;
            return provider.ModelName ?? "默认模型";
        }

        private string BuildUserPrompt(string gaugeType, string gaugeTitle)
        {
            return "请为作品类型“" + (gaugeType ?? string.Empty) + "”和量规标题“" + (gaugeTitle ?? string.Empty) + "”生成 5 条可直接用于作品互评的量规项。只输出 JSON 数组，不要解释，不要 Markdown 代码块。格式示例：[{'item':'创意表达','score':20}]。每项字段名固定为 item 和 score。";
        }

        private string CallProvider(LearnSite.Model.AIProvider provider, string systemPrompt, string userPrompt)
        {
            string chatUrl = provider.BaseUrl.TrimEnd('/') + "/chat/completions";
            var requestBody = new
            {
                model = provider.ModelName,
                temperature = 0.4,
                max_tokens = 800,
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
            request.Timeout = 60000;

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
            {
                using (Stream responseStream = response.GetResponseStream())
                {
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
            }
        }

        private GaugeGenerationResult BuildFallbackResult(string gaugeType, string gaugeTitle, string message, string providerName)
        {
            return new GaugeGenerationResult
            {
                Success = false,
                UsedFallback = true,
                Message = message,
                ProviderDisplayName = providerName,
                Items = AIGaugeSkillHelper.BuildFallbackItems(gaugeType, gaugeTitle)
            };
        }

        private string CleanErrorMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return string.Empty;
            }
            return message.Replace("\r", " ").Replace("\n", " ").Replace("\"", "'");
        }

        private void ReportProgress(GaugeProgressReporter progressReporter, string stage, string message)
        {
            if (progressReporter != null)
            {
                progressReporter(stage, message);
            }
        }
    }
}
