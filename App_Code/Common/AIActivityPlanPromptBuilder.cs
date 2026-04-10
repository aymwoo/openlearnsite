using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LearnSite.Common
{
    public class AIActivityPlanPromptRequest
    {
        public string Topic { get; set; }
        public string Grade { get; set; }
        public string Duration { get; set; }
        public string TeachingGoals { get; set; }
        public string ExistingCourseContent { get; set; }
    }

    public class AIActivityPlanSectionRegenerationRequest : AIActivityPlanPromptRequest
    {
        public string SectionTarget { get; set; }
        public ActivityPlanDraft CurrentDraft { get; set; }
    }

    public static class AIActivityPlanPromptBuilder
    {
        public const int MaxTopicLength = 200;
        public const int MaxGradeLength = 50;
        public const int MaxDurationLength = 50;
        public const int MaxTeachingGoalsLength = 500;
        private const int MaxExistingCourseContentLength = 4000;

        public static string Build(AIActivityPlanPromptRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            string topic = SafeTrim(request.Topic);
            if (string.IsNullOrEmpty(topic))
            {
                throw new ArgumentException("Topic is required.", "request");
            }
            topic = BoundText(topic, MaxTopicLength);

            string grade = BoundText(request.Grade, MaxGradeLength);
            string duration = BoundText(request.Duration, MaxDurationLength);
            string teachingGoals = BoundText(request.TeachingGoals, MaxTeachingGoalsLength);

            List<string> sections = new List<string>();
            sections.Add("请根据以下教师输入生成课堂活动计划草案。只返回 JSON 对象，不要解释，不要 Markdown 代码块。");
            sections.Add("主题/知识点：" + topic);

            AppendStructuredField(sections, "授课年级", grade);
            AppendStructuredField(sections, "课时/时长", duration);
            AppendStructuredField(sections, "教学目标", teachingGoals);

            string existingCourseContent = BoundText(request.ExistingCourseContent, MaxExistingCourseContentLength);
            if (!string.IsNullOrEmpty(existingCourseContent))
            {
                StringBuilder backgroundBuilder = new StringBuilder();
                backgroundBuilder.AppendLine("支持背景：以下是教师当前编辑器中的已有内容，仅作为补充参考。");
                backgroundBuilder.AppendLine("如果与当前输入的主题冲突，以教师刚输入的主题为准。已有内容不要覆盖当前教师意图。");
                backgroundBuilder.Append(existingCourseContent);
                sections.Add(backgroundBuilder.ToString().Trim());
            }

            sections.Add("返回 JSON 对象时，顶层字段固定为 teachingGoals、activitySteps、resources、assessment、teacherReminder。");
            sections.Add("teachingGoals、resources、assessment 必须为字符串数组；teacherReminder 必须为简短字符串。");
            sections.Add("activitySteps 必须按课堂顺序给出，并且每个步骤都必须包含 title、minutes、teacherAction、studentAction、interactionMethod、resourceSuggestion、assessmentCheck。");
            sections.Add("minutes 需要使用明确时长表达，例如“5分钟”。请保持结果适合教师审核，不要自动应用到已有学案内容。");
            return string.Join("\n\n", sections.ToArray());
        }

        public static string BuildSectionRegeneration(AIActivityPlanSectionRegenerationRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            string topic = SafeTrim(request.Topic);
            if (string.IsNullOrEmpty(topic))
            {
                throw new ArgumentException("Topic is required.", "request");
            }

            string sectionTarget = SafeTrim(request.SectionTarget);
            if (!AIActivityPlanDraftHelper.IsSupportedSectionTarget(sectionTarget))
            {
                throw new ArgumentException("Section target is invalid.", "request");
            }

            if (!AIActivityPlanDraftHelper.IsValidDraft(request.CurrentDraft))
            {
                throw new ArgumentException("Current draft is invalid.", "request");
            }

            topic = BoundText(topic, MaxTopicLength);
            string grade = BoundText(request.Grade, MaxGradeLength);
            string duration = BoundText(request.Duration, MaxDurationLength);
            string teachingGoals = BoundText(request.TeachingGoals, MaxTeachingGoalsLength);
            string existingCourseContent = BoundText(request.ExistingCourseContent, MaxExistingCourseContentLength);
            string normalizedTarget = GetNormalizedSectionTarget(sectionTarget);

            List<string> sections = new List<string>();
            sections.Add("请根据以下教师输入，仅重写指定的一个课堂活动计划部分。只返回 JSON 对象，不要解释，不要 Markdown 代码块。");
            sections.Add("主题/知识点：" + topic);
            AppendStructuredField(sections, "授课年级", grade);
            AppendStructuredField(sections, "课时/时长", duration);
            AppendStructuredField(sections, "教学目标", teachingGoals);

            if (!string.IsNullOrEmpty(existingCourseContent))
            {
                StringBuilder backgroundBuilder = new StringBuilder();
                backgroundBuilder.AppendLine("支持背景：以下是教师当前编辑器中的已有内容，仅作为补充参考。");
                backgroundBuilder.AppendLine("如果与当前输入的主题冲突，以教师刚输入的主题为准。已有内容不要覆盖当前教师意图。");
                backgroundBuilder.Append(existingCourseContent);
                sections.Add(backgroundBuilder.ToString().Trim());
            }

            sections.Add("当前完整草案（仅供参考，请保留未指定部分的整体风格和难度）：");
            sections.Add(JsonConvert.SerializeObject(new
            {
                teachingGoals = request.CurrentDraft.TeachingGoals,
                activitySteps = request.CurrentDraft.ActivitySteps,
                resources = request.CurrentDraft.Resources,
                assessment = request.CurrentDraft.Assessment,
                teacherReminder = request.CurrentDraft.TeacherReminder
            }));

            sections.Add("本次只允许重写的顶层字段：" + normalizedTarget);
            sections.Add("返回 JSON 时只能包含一个顶层字段，且该字段名必须是 " + normalizedTarget + "。不要返回完整草案，不要附带其他字段。");

            if (string.Equals(normalizedTarget, "activitySteps", StringComparison.Ordinal))
            {
                sections.Add("activitySteps 必须为数组，并且每个步骤都必须包含 title、minutes、teacherAction、studentAction、interactionMethod、resourceSuggestion、assessmentCheck。");
                sections.Add("minutes 需要使用明确时长表达，例如“5分钟”。");
            }
            else if (string.Equals(normalizedTarget, "teacherReminder", StringComparison.Ordinal))
            {
                sections.Add("teacherReminder 必须为简短字符串，便于教师审核。");
            }
            else
            {
                sections.Add(normalizedTarget + " 必须返回非空内容，并保持适合教师审核。teachingGoals、resources、assessment 必须为字符串数组。");
            }

            return string.Join("\n\n", sections.ToArray());
        }

        private static string GetNormalizedSectionTarget(string sectionTarget)
        {
            string[] allowedTargets = AIActivityPlanDraftHelper.GetAllowedSectionTargets();
            foreach (string allowedTarget in allowedTargets)
            {
                if (string.Equals(allowedTarget, SafeTrim(sectionTarget), StringComparison.OrdinalIgnoreCase))
                {
                    return allowedTarget;
                }
            }

            return string.Empty;
        }

        private static void AppendStructuredField(List<string> sections, string label, string value)
        {
            string trimmedValue = SafeTrim(value);
            if (!string.IsNullOrEmpty(trimmedValue))
            {
                sections.Add(label + "：" + trimmedValue);
            }
        }

        private static string SafeTrim(string value)
        {
            return value == null ? string.Empty : value.Trim();
        }

        public static string BoundText(string value, int maxLength)
        {
            string trimmed = SafeTrim(value);
            if (string.IsNullOrEmpty(trimmed) || maxLength <= 0)
            {
                return trimmed;
            }

            if (trimmed.Length <= maxLength)
            {
                return trimmed;
            }

            return trimmed.Substring(0, maxLength);
        }
    }
}
