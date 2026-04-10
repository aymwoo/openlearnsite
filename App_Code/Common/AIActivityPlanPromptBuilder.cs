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
