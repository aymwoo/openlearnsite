using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using LearnSite.Model;
using Newtonsoft.Json.Linq;

namespace LearnSite.Common
{
    public static class AIGaugeSkillHelper
    {
        private const string DefaultGaugeSkillName = "AI量规生成助手";
        private const int DefaultTotalScore = 100;

        public static string GetDefaultGaugeSkillName()
        {
            return DefaultGaugeSkillName;
        }

        public static string GetDefaultGaugeSkillPrompt()
        {
            return "你是一名小学信息技术课程评价量规设计助手。请围绕作品类型 {{gtype}} 和量规标题 {{gtitle}} 设计适合学生互评的评价维度。输出时只返回 JSON 数组，每一项包含 item 和 score 两个字段。评价描述要具体、简洁、易观察，分值为正整数，总分控制在 100 分左右。";
        }

        public static string ReplaceSkillTokens(string template, string gaugeType, string gaugeTitle)
        {
            string content = template ?? string.Empty;
            return content
                .Replace("{{gtype}}", gaugeType ?? string.Empty)
                .Replace("{{gaugeType}}", gaugeType ?? string.Empty)
                .Replace("{{title}}", gaugeTitle ?? string.Empty)
                .Replace("{{gtitle}}", gaugeTitle ?? string.Empty)
                .Replace("{{gaugeTitle}}", gaugeTitle ?? string.Empty);
        }

        public static List<GaugeItem> ParseGaugeItems(string responseContent)
        {
            List<GaugeItem> items = ParseJsonItems(responseContent);
            if (items.Count == 0)
            {
                items = ParseTextItems(responseContent);
            }
            return NormalizeItems(items);
        }

        public static List<GaugeItem> BuildFallbackItems(string gaugeType, string gaugeTitle)
        {
            List<GaugeItem> items = new List<GaugeItem>();
            string prefix = string.IsNullOrEmpty(gaugeType) ? "作品" : gaugeType + "作品";
            string titlePart = string.IsNullOrEmpty(gaugeTitle) ? string.Empty : "“" + gaugeTitle + "”";

            items.Add(new GaugeItem { Mitem = prefix + titlePart + "主题契合与创意表达", Mscore = 20, Msort = 1 });
            items.Add(new GaugeItem { Mitem = prefix + "功能实现与技术完成度", Mscore = 20, Msort = 2 });
            items.Add(new GaugeItem { Mitem = prefix + "结构设计与操作体验", Mscore = 20, Msort = 3 });
            items.Add(new GaugeItem { Mitem = prefix + "界面美观与细节处理", Mscore = 20, Msort = 4 });
            items.Add(new GaugeItem { Mitem = prefix + "规范表达与学习反思", Mscore = 20, Msort = 5 });

            return NormalizeItems(items);
        }

        private static List<GaugeItem> ParseJsonItems(string responseContent)
        {
            List<GaugeItem> items = new List<GaugeItem>();
            string jsonText = ExtractJsonPayload(responseContent);
            if (string.IsNullOrEmpty(jsonText))
            {
                return items;
            }

            try
            {
                JToken token = JToken.Parse(jsonText);
                JArray array = token as JArray;
                if (array == null && token is JObject)
                {
                    JObject obj = (JObject)token;
                    array = obj["items"] as JArray ?? obj["criteria"] as JArray ?? obj["data"] as JArray;
                }
                if (array == null)
                {
                    return items;
                }

                foreach (JToken child in array)
                {
                    string itemText = GetTextValue(child, "item", "name", "title", "desc", "description");
                    int score = GetIntValue(child, "score", "points", "value", "weight");
                    if (!string.IsNullOrEmpty(itemText))
                    {
                        items.Add(new GaugeItem { Mitem = itemText.Trim(), Mscore = score });
                    }
                }
            }
            catch
            {
            }

            return items;
        }

        private static List<GaugeItem> ParseTextItems(string responseContent)
        {
            List<GaugeItem> items = new List<GaugeItem>();
            if (string.IsNullOrEmpty(responseContent))
            {
                return items;
            }

            string cleaned = CleanResponseText(responseContent);
            string[] lines = cleaned.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Regex lineRegex = new Regex(@"^(?:\d+[\.|、]\s*)?(?<item>.+?)(?:[（(\[]?(?<score>\d{1,3})\s*分[）)\]]?)?$", RegexOptions.Compiled);
            foreach (string raw in lines)
            {
                string line = raw.Trim().Trim('-', '*', ' ');
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }
                Match match = lineRegex.Match(line);
                if (!match.Success)
                {
                    continue;
                }

                string itemText = match.Groups["item"].Value.Trim('：', ':', ' ');
                int score = 0;
                int.TryParse(match.Groups["score"].Value, out score);
                if (!string.IsNullOrEmpty(itemText))
                {
                    items.Add(new GaugeItem { Mitem = itemText, Mscore = score });
                }
            }

            return items;
        }

        private static string ExtractJsonPayload(string responseContent)
        {
            string cleaned = CleanResponseText(responseContent);
            if (string.IsNullOrEmpty(cleaned))
            {
                return string.Empty;
            }

            int arrayStart = cleaned.IndexOf('[');
            int arrayEnd = cleaned.LastIndexOf(']');
            if (arrayStart >= 0 && arrayEnd > arrayStart)
            {
                return cleaned.Substring(arrayStart, arrayEnd - arrayStart + 1);
            }

            int objStart = cleaned.IndexOf('{');
            int objEnd = cleaned.LastIndexOf('}');
            if (objStart >= 0 && objEnd > objStart)
            {
                return cleaned.Substring(objStart, objEnd - objStart + 1);
            }

            return cleaned;
        }

        private static string CleanResponseText(string responseContent)
        {
            string cleaned = responseContent ?? string.Empty;
            cleaned = cleaned.Replace("```json", string.Empty).Replace("```", string.Empty).Trim();
            return cleaned;
        }

        private static string GetTextValue(JToken token, params string[] names)
        {
            if (!(token is JObject))
            {
                return token == null ? string.Empty : token.ToString();
            }

            JObject obj = (JObject)token;
            foreach (string name in names)
            {
                JToken value = obj[name];
                if (value != null && !string.IsNullOrEmpty(value.ToString()))
                {
                    return value.ToString();
                }
            }
            return string.Empty;
        }

        private static int GetIntValue(JToken token, params string[] names)
        {
            if (!(token is JObject))
            {
                return 0;
            }

            JObject obj = (JObject)token;
            foreach (string name in names)
            {
                JToken value = obj[name];
                if (value == null)
                {
                    continue;
                }

                int number;
                if (int.TryParse(value.ToString(), out number))
                {
                    return number;
                }
            }
            return 0;
        }

        private static List<GaugeItem> NormalizeItems(List<GaugeItem> items)
        {
            List<GaugeItem> normalized = new List<GaugeItem>();
            if (items == null)
            {
                return normalized;
            }

            foreach (GaugeItem item in items)
            {
                if (item == null || string.IsNullOrEmpty(item.Mitem))
                {
                    continue;
                }

                string text = item.Mitem.Trim();
                if (text.Length > 50)
                {
                    text = text.Substring(0, 50);
                }

                bool duplicated = false;
                foreach (GaugeItem existing in normalized)
                {
                    if (string.Equals(existing.Mitem, text, StringComparison.OrdinalIgnoreCase))
                    {
                        duplicated = true;
                        break;
                    }
                }
                if (duplicated)
                {
                    continue;
                }

                normalized.Add(new GaugeItem { Mitem = text, Mscore = item.Mscore });
                if (normalized.Count >= 8)
                {
                    break;
                }
            }

            if (normalized.Count == 0)
            {
                return normalized;
            }

            bool needResetScore = false;
            int total = 0;
            foreach (GaugeItem item in normalized)
            {
                int score = item.Mscore.HasValue ? item.Mscore.Value : 0;
                if (score <= 0)
                {
                    needResetScore = true;
                    break;
                }
                total += score;
            }

            if (needResetScore || total != DefaultTotalScore)
            {
                int baseScore = DefaultTotalScore / normalized.Count;
                int remainder = DefaultTotalScore % normalized.Count;
                for (int i = 0; i < normalized.Count; i++)
                {
                    normalized[i].Mscore = baseScore + (i < remainder ? 1 : 0);
                }
            }

            for (int i = 0; i < normalized.Count; i++)
            {
                normalized[i].Msort = i + 1;
            }

            return normalized;
        }
    }
}
