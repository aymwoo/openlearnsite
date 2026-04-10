using System;
using System.Collections.Generic;
using System.Text;

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
            sections.Add("你是一名面向一线教师的活动计划助手，请根据教师当前输入生成可直接用于课堂实施的活动计划。");
            sections.Add("当前教师意图：" + topic);

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

            sections.Add("请输出结构清晰、可执行的课堂活动计划，重点体现教学流程、师生活动与实施建议。");
            return string.Join("\n\n", sections.ToArray());
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
