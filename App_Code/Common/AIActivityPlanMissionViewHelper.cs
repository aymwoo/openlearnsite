using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace LearnSite.Common
{
    public class ActivityPlanMissionGuideView
    {
        public string GoalHtml { get; set; }

        public string InstructionsHtml { get; set; }

        public string StepsHtml { get; set; }
    }

    public static class AIActivityPlanMissionViewHelper
    {
        private static readonly Regex HeadingRegex = new Regex("<p><strong>【活动计划-(?<title>[^】]+)】</strong></p>(?<content>.*?)(?=<p><strong>【活动计划-|$)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
        private static readonly Regex ListItemRegex = new Regex("<li>(.*?)</li>", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        public static ActivityPlanMissionGuideView BuildActivityGuideView(string missionContent)
        {
            string normalized = NormalizeMissionContent(missionContent);
            if (string.IsNullOrEmpty(normalized))
            {
                return null;
            }

            IDictionary<string, string> sections = ExtractSections(normalized);
            if (!sections.ContainsKey("教学目标") || !sections.ContainsKey("活动步骤"))
            {
                return null;
            }

            string topicInstruction = BuildTopicInstructionHtml(normalized);
            string goalHtml = BuildGoalHtml(sections["教学目标"]);
            string instructionsHtml = BuildInstructionsHtml(sections, topicInstruction);
            string stepsHtml = BuildStepsHtml(sections["活动步骤"]);
            if (string.IsNullOrEmpty(goalHtml) || string.IsNullOrEmpty(instructionsHtml) || string.IsNullOrEmpty(stepsHtml))
            {
                return null;
            }

            return new ActivityPlanMissionGuideView
            {
                GoalHtml = goalHtml,
                InstructionsHtml = instructionsHtml,
                StepsHtml = stepsHtml
            };
        }

        private static string NormalizeMissionContent(string missionContent)
        {
            return System.Web.HttpUtility.HtmlDecode(missionContent ?? string.Empty).Trim();
        }

        private static IDictionary<string, string> ExtractSections(string missionContent)
        {
            Dictionary<string, string> sections = new Dictionary<string, string>();
            MatchCollection matches = HeadingRegex.Matches(missionContent);
            foreach (Match match in matches)
            {
                string title = match.Groups["title"].Value.Trim();
                string content = match.Groups["content"].Value.Trim();
                if (!string.IsNullOrEmpty(title) && !sections.ContainsKey(title))
                {
                    sections.Add(title, content);
                }
            }

            return sections;
        }

        private static string BuildGoalHtml(string sectionHtml)
        {
            List<string> goals = ExtractListItems(sectionHtml);
            if (goals.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder();
            builder.Append("<ul>");
            for (int i = 0; i < goals.Count; i++)
            {
                builder.AppendFormat("<li>{0}</li>", goals[i]);
            }

            builder.Append("</ul>");
            return builder.ToString();
        }

        private static string BuildInstructionsHtml(IDictionary<string, string> sections, string topicInstruction)
        {
            List<string> instructions = new List<string>();
            if (!string.IsNullOrEmpty(topicInstruction))
            {
                instructions.Add(topicInstruction);
            }

            AppendInstructionItems(instructions, sections, "教学资源", "准备资源：");
            AppendInstructionItems(instructions, sections, "评价设计", "完成后检查：");
            if (sections.ContainsKey("教师提醒"))
            {
                string teacherReminder = StripTags(sections["教师提醒"]);
                if (!string.IsNullOrEmpty(teacherReminder))
                {
                    instructions.Add("学习提示：" + Encode(teacherReminder));
                }
            }

            if (instructions.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder();
            builder.Append("<ul>");
            for (int i = 0; i < instructions.Count; i++)
            {
                builder.AppendFormat("<li>{0}</li>", instructions[i]);
            }

            builder.Append("</ul>");
            return builder.ToString();
        }

        private static string BuildTopicInstructionHtml(string missionContent)
        {
            Match match = Regex.Match(missionContent ?? string.Empty, "<h2>(.*?)</h2>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                return string.Empty;
            }

            string topic = StripTags(match.Groups[1].Value);
            if (string.IsNullOrEmpty(topic))
            {
                return string.Empty;
            }

            return "活动主题：" + Encode(topic);
        }

        private static void AppendInstructionItems(List<string> instructions, IDictionary<string, string> sections, string sectionName, string prefix)
        {
            if (!sections.ContainsKey(sectionName))
            {
                return;
            }

            List<string> items = ExtractListItems(sections[sectionName]);
            for (int i = 0; i < items.Count; i++)
            {
                instructions.Add(prefix + items[i]);
            }
        }

        private static string BuildStepsHtml(string sectionHtml)
        {
            List<string> steps = ExtractListItems(sectionHtml);
            if (steps.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder();
            builder.Append("<ol>");
            for (int i = 0; i < steps.Count; i++)
            {
                string step = steps[i].Trim();
                if (string.IsNullOrEmpty(step))
                {
                    continue;
                }

                builder.AppendFormat("<li>{0}</li>", step);
            }

            builder.Append("</ol>");
            return builder.ToString();
        }

        private static List<string> ExtractListItems(string html)
        {
            List<string> items = new List<string>();
            MatchCollection matches = ListItemRegex.Matches(html ?? string.Empty);
            foreach (Match match in matches)
            {
                string value = StripTags(match.Groups[1].Value);
                if (!string.IsNullOrEmpty(value))
                {
                    items.Add(Encode(value));
                }
            }

            return items;
        }

        private static string StripTags(string value)
        {
            string withoutBreaks = (value ?? string.Empty).Replace("<br />", "；").Replace("<br/>", "；").Replace("<br>", "；");
            string text = Regex.Replace(withoutBreaks, "<.*?>", string.Empty, RegexOptions.Singleline);
            return System.Web.HttpUtility.HtmlDecode(text).Trim();
        }

        private static string Encode(string value)
        {
            return System.Web.HttpUtility.HtmlEncode(value ?? string.Empty);
        }
    }
}
