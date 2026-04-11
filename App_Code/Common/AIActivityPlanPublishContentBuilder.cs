using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSite.Common
{
    public class AIActivityPlanPublishContentBuilder
    {
        private static readonly string[] SupportedSectionKeys = new string[]
        {
            "teachingGoals",
            "activitySteps",
            "resources",
            "assessment",
            "teacherReminder"
        };

        public string BuildLessonContent(IEnumerable<string> selectedSectionKeys, ActivityPlanDraft draft)
        {
            if (!AIActivityPlanDraftHelper.IsValidDraft(draft) || selectedSectionKeys == null)
            {
                return string.Empty;
            }

            List<string> orderedKeys = NormalizeSelectedSectionKeys(selectedSectionKeys);
            if (orderedKeys.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < orderedKeys.Count; i++)
            {
                string block = BuildSectionBlock(orderedKeys[i], draft);
                if (string.IsNullOrEmpty(block))
                {
                    continue;
                }

                if (builder.Length > 0)
                {
                    builder.Append(Environment.NewLine);
                }

                builder.Append(block);
            }

            return builder.ToString();
        }

        public string BuildMissionContent(string topic, ActivityPlanDraft draft)
        {
            if (!AIActivityPlanDraftHelper.IsValidDraft(draft))
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder();
            string boundedTopic = AIActivityPlanPromptBuilder.BoundText(topic, AIActivityPlanPromptBuilder.MaxTopicLength);
            if (!string.IsNullOrEmpty(boundedTopic))
            {
                builder.AppendFormat("<h2>{0}</h2>", Encode(boundedTopic));
            }

            builder.Append(BuildSectionBlock("teachingGoals", draft));
            builder.Append(BuildSectionBlock("activitySteps", draft));
            builder.Append(BuildSectionBlock("resources", draft));
            builder.Append(BuildSectionBlock("assessment", draft));
            builder.Append(BuildSectionBlock("teacherReminder", draft));
            return builder.ToString();
        }

        private static List<string> NormalizeSelectedSectionKeys(IEnumerable<string> selectedSectionKeys)
        {
            List<string> normalized = new List<string>();
            foreach (string supportedKey in SupportedSectionKeys)
            {
                foreach (string current in selectedSectionKeys)
                {
                    if (string.Equals(current, supportedKey, StringComparison.Ordinal))
                    {
                        normalized.Add(supportedKey);
                        break;
                    }
                }
            }

            return normalized;
        }

        private static string BuildSectionBlock(string sectionKey, ActivityPlanDraft draft)
        {
            switch (sectionKey)
            {
                case "teachingGoals":
                    return BuildListSection("教学目标", draft.TeachingGoals);
                case "activitySteps":
                    return BuildStepsSection(draft.ActivitySteps);
                case "resources":
                    return BuildListSection("教学资源", draft.Resources);
                case "assessment":
                    return BuildListSection("评价设计", draft.Assessment);
                case "teacherReminder":
                    return BuildParagraphSection("教师提醒", draft.TeacherReminder);
                default:
                    return string.Empty;
            }
        }

        private static string BuildListSection(string title, IList<string> items)
        {
            if (items == null || items.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("<p><strong>【活动计划-{0}】</strong></p>", title);
            builder.Append("<ul>");
            for (int i = 0; i < items.Count; i++)
            {
                builder.AppendFormat("<li>{0}</li>", Encode(items[i]));
            }
            builder.Append("</ul>");
            return builder.ToString();
        }

        private static string BuildStepsSection(IList<ActivityPlanDraftStep> steps)
        {
            if (steps == null || steps.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder();
            builder.Append("<p><strong>【活动计划-活动步骤】</strong></p>");
            builder.Append("<ol>");
            for (int i = 0; i < steps.Count; i++)
            {
                ActivityPlanDraftStep step = steps[i];
                builder.Append("<li>");
                builder.AppendFormat("<strong>{0}</strong>", Encode(step.Title));
                builder.AppendFormat("<br />时长：{0}", Encode(step.Minutes));
                builder.AppendFormat("<br />教师活动：{0}", Encode(step.TeacherAction));
                builder.AppendFormat("<br />学生活动：{0}", Encode(step.StudentAction));
                builder.AppendFormat("<br />互动方式：{0}", Encode(step.InteractionMethod));
                builder.AppendFormat("<br />资源建议：{0}", Encode(step.ResourceSuggestion));
                builder.AppendFormat("<br />评价检查：{0}", Encode(step.AssessmentCheck));
                builder.Append("</li>");
            }
            builder.Append("</ol>");
            return builder.ToString();
        }

        private static string BuildParagraphSection(string title, string content)
        {
            string normalized = Encode(content);
            if (string.IsNullOrEmpty(normalized))
            {
                return string.Empty;
            }

            return string.Format("<p><strong>【活动计划-{0}】</strong></p><p>{1}</p>", title, normalized);
        }

        private static string Encode(string value)
        {
            return System.Web.HttpUtility.HtmlEncode(value ?? string.Empty);
        }
    }
}
