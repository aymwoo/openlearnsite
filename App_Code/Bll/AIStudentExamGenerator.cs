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
    public class StudentExamAssessmentResult
    {
        public bool Success { get; set; }
        public bool UsedFallback { get; set; }
        public string ProviderDisplayName { get; set; }
        public string SkillName { get; set; }
        public string Summary { get; set; }
        public string AssessmentContent { get; set; }
        public string LearningLog { get; set; }
        public string Message { get; set; }
    }

    public class StudentExamAssessmentContext
    {
        public int? Fid { get; set; }
        public int Sid { get; set; }
        public string Snum { get; set; }
        public string Sname { get; set; }
        public int Cid { get; set; }
        public int Lid { get; set; }
        public int Vid { get; set; }
        public string ExamTitle { get; set; }
        public int Score { get; set; }
        public int QuestionCount { get; set; }
        public string AnswerLogJson { get; set; }
        public string LearningLog { get; set; }
    }

    public class AIStudentExamGenerator
    {
        public delegate void ExamProgressReporter(string stage, string message);

        public static void EnsureDefaultStudentExamSkill()
        {
            try
            {
                LearnSite.BLL.AICustomSkill skillBll = new LearnSite.BLL.AICustomSkill();
                List<LearnSite.Model.AICustomSkill> skills = skillBll.GetModelList("SkillScope like '%student_exam%'");
                if (skills != null && skills.Count > 0)
                    return;

                LearnSite.Model.AICustomSkill model = new LearnSite.Model.AICustomSkill();
                model.SkillName = AIStudentExamSkillHelper.GetDefaultSkillName();
                model.PromptContent = AIStudentExamSkillHelper.GetDefaultSkillPrompt();
                model.SkillScope = "student_exam";
                model.IsActive = true;
                skillBll.Add(model);
            }
            catch
            {
            }
        }

        public StudentExamAssessmentResult Generate(StudentExamAssessmentContext context, ExamProgressReporter reporter)
        {
            EnsureDefaultStudentExamSkill();
            LearnSite.Model.AIProvider provider = GetDefaultProvider();
            string providerName = BuildProviderDisplayName(provider);

            if (provider == null || string.IsNullOrEmpty(provider.BaseUrl) || string.IsNullOrEmpty(provider.ModelName))
            {
                Report(reporter, "fallback", "未配置默认 AI Provider，改为使用规则化评估摘要。", providerName);
                return BuildFallback(context, providerName, "未配置默认 AI Provider，已使用默认评估摘要。");
            }

            LearnSite.Model.AICustomSkill skill = GetStudentExamSkill();
            string skillName = skill == null ? AIStudentExamSkillHelper.GetDefaultSkillName() : skill.SkillName;
            string systemPrompt = skill == null ? AIStudentExamSkillHelper.GetDefaultSkillPrompt() : skill.PromptContent;
            systemPrompt = AIStudentExamSkillHelper.ReplaceTokens(systemPrompt, context.Sname, context.ExamTitle, context.Score, context.QuestionCount, context.AnswerLogJson);
            string userPrompt = BuildUserPrompt(context);

            try
            {
                Report(reporter, "ai", "正在调用 AI Provider “" + providerName + "” 进行测验评估...", providerName);
                string responseText = CallProvider(provider, systemPrompt, userPrompt);
                JObject resultObj = AIStudentExamSkillHelper.ParseResponseObject(responseText);
                if (resultObj == null)
                {
                    Report(reporter, "fallback", "AI 返回内容无法解析，改为使用规则化评估摘要。", providerName);
                    return BuildFallback(context, providerName, "AI 返回内容无法解析，已使用默认评估摘要。");
                }

                string summary = GetValue(resultObj, "summary");
                string analysis = GetValue(resultObj, "analysis");
                string suggestions = GetValue(resultObj, "suggestions");
                string learningLog = GetValue(resultObj, "learningLog");

                if (string.IsNullOrEmpty(summary))
                    summary = AIStudentExamSkillHelper.BuildFallbackSummary(context.ExamTitle, context.Score, context.QuestionCount);
                if (string.IsNullOrEmpty(analysis))
                    analysis = AIStudentExamSkillHelper.BuildFallbackAnalysis(context.AnswerLogJson);
                if (string.IsNullOrEmpty(suggestions))
                    suggestions = AIStudentExamSkillHelper.BuildFallbackSuggestions();
                if (string.IsNullOrEmpty(learningLog))
                    learningLog = context.LearningLog;

                return new StudentExamAssessmentResult
                {
                    Success = true,
                    UsedFallback = false,
                    ProviderDisplayName = providerName,
                    SkillName = skillName,
                    Summary = summary,
                    AssessmentContent = "【分析】\n" + analysis + "\n\n【建议】\n" + suggestions,
                    LearningLog = learningLog,
                    Message = "AI 测验评估已生成。"
                };
            }
            catch (Exception ex)
            {
                Report(reporter, "fallback", "AI 评估失败，改为使用规则化评估摘要。", providerName);
                return BuildFallback(context, providerName, "AI 评估失败，已使用默认评估摘要。" + ex.Message.Replace("\r", " ").Replace("\n", " "));
            }
        }

        public int SaveAssessment(StudentExamAssessmentContext context, StudentExamAssessmentResult result)
        {
            LearnSite.BLL.AIStudentExamAssessment bll = new LearnSite.BLL.AIStudentExamAssessment();
            LearnSite.Model.AIStudentExamAssessment model = new LearnSite.Model.AIStudentExamAssessment();
            model.Fid = context.Fid;
            model.Sid = context.Sid;
            model.Snum = context.Snum;
            model.Sname = context.Sname;
            model.Cid = context.Cid;
            model.Lid = context.Lid;
            model.Vid = context.Vid;
            model.ProviderName = result.ProviderDisplayName;
            model.SkillName = result.SkillName;
            model.Summary = result.Summary;
            model.AssessmentContent = result.AssessmentContent;
            model.LearningLog = result.LearningLog;
            model.AnswerLog = context.AnswerLogJson;
            model.Score = context.Score;
            model.QuestionCount = context.QuestionCount;
            model.IsFallback = result.UsedFallback;
            model.CreatedAt = DateTime.Now;
            return bll.Add(model);
        }

        public static string GetDefaultProviderDisplayName()
        {
            LearnSite.Model.AIProvider provider = GetDefaultProvider();
            return BuildProviderDisplayName(provider);
        }

        public StudentExamAssessmentResult GenerateRuleBasedAssessment(StudentExamAssessmentContext context)
        {
            return BuildFallback(context, "规则评估", "未启用 AI 评估，已使用规则评估摘要。");
        }

        private StudentExamAssessmentResult BuildFallback(StudentExamAssessmentContext context, string providerName, string message)
        {
            return new StudentExamAssessmentResult
            {
                Success = false,
                UsedFallback = true,
                ProviderDisplayName = providerName,
                SkillName = AIStudentExamSkillHelper.GetDefaultSkillName(),
                Summary = AIStudentExamSkillHelper.BuildFallbackSummary(context.ExamTitle, context.Score, context.QuestionCount),
                AssessmentContent = "【分析】\n" + AIStudentExamSkillHelper.BuildFallbackAnalysis(context.AnswerLogJson) + "\n\n【建议】\n" + AIStudentExamSkillHelper.BuildFallbackSuggestions(),
                LearningLog = AIStudentExamSkillHelper.BuildFallbackLearningLog(context.Sname, context.ExamTitle, context.Score, context.QuestionCount, context.LearningLog),
                Message = message
            };
        }

        private static LearnSite.Model.AIProvider GetDefaultProvider()
        {
            LearnSite.BLL.AIProvider providerBll = new LearnSite.BLL.AIProvider();
            List<LearnSite.Model.AIProvider> providers = providerBll.GetModelList("");
            if (providers == null || providers.Count == 0)
                return null;
            LearnSite.Model.AIProvider provider = providers.FirstOrDefault(p => p.IsDefault);
            return provider ?? providers[0];
        }

        private LearnSite.Model.AICustomSkill GetStudentExamSkill()
        {
            LearnSite.BLL.AICustomSkill skillBll = new LearnSite.BLL.AICustomSkill();
            List<LearnSite.Model.AICustomSkill> skills = skillBll.GetModelList("IsActive=1 and SkillScope like '%student_exam%'");
            if (skills == null || skills.Count == 0)
                return null;
            LearnSite.Model.AICustomSkill defaultSkill = skills.FirstOrDefault(s => s.SkillName == AIStudentExamSkillHelper.GetDefaultSkillName());
            return defaultSkill ?? skills[0];
        }

        private string BuildUserPrompt(StudentExamAssessmentContext context)
        {
            return "请根据以下学生测验数据生成教师可读的分析结果，只返回 JSON 对象。测验标题：" + context.ExamTitle + "；学生：" + context.Sname + "；得分：" + context.Score + " / " + context.QuestionCount + "；作答日志：" + context.AnswerLogJson;
        }

        private string CallProvider(LearnSite.Model.AIProvider provider, string systemPrompt, string userPrompt)
        {
            string chatUrl = provider.BaseUrl.TrimEnd('/') + "/chat/completions";
            var requestBody = new
            {
                model = provider.ModelName,
                temperature = 0.4,
                max_tokens = 1000,
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
                request.Headers.Add("Authorization", "Bearer " + provider.ApiKey);

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
                    throw new Exception("AI 返回格式无效。");
                JToken content = responseJson["choices"][0]["message"]["content"];
                return content == null ? string.Empty : content.ToString();
            }
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

        private void Report(ExamProgressReporter reporter, string stage, string message, string providerName)
        {
            if (reporter != null)
                reporter(stage, message + (string.IsNullOrEmpty(providerName) ? string.Empty : " 当前 Provider：" + providerName));
        }

        private string GetValue(JObject obj, string name)
        {
            JToken token = obj[name];
            return token == null ? string.Empty : token.ToString();
        }
    }
}
