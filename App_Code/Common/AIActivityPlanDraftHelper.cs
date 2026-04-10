using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace LearnSite.Common
{
    public class ActivityPlanDraft
    {
        public ActivityPlanDraft()
        {
            TeachingGoals = new List<string>();
            ActivitySteps = new List<ActivityPlanDraftStep>();
            Resources = new List<string>();
            Assessment = new List<string>();
            TeacherReminder = string.Empty;
        }

        public List<string> TeachingGoals { get; set; }
        public List<ActivityPlanDraftStep> ActivitySteps { get; set; }
        public List<string> Resources { get; set; }
        public List<string> Assessment { get; set; }
        public string TeacherReminder { get; set; }
    }

    public class ActivityPlanDraftStep
    {
        public int Sort { get; set; }
        public string Title { get; set; }
        public string Minutes { get; set; }
        public string TeacherAction { get; set; }
        public string StudentAction { get; set; }
        public string InteractionMethod { get; set; }
        public string ResourceSuggestion { get; set; }
        public string AssessmentCheck { get; set; }
    }

    public static class AIActivityPlanDraftHelper
    {
        public static ActivityPlanDraft ParseDraft(string responseText)
        {
            JToken token = ParseRootToken(responseText);
            if (token == null)
            {
                return null;
            }

            JObject draftObject = ExtractDraftObject(token);
            if (draftObject == null)
            {
                return null;
            }

            ActivityPlanDraft draft = new ActivityPlanDraft();
            draft.TeachingGoals = NormalizeStringList(GetFirstToken(draftObject, "teachingGoals", "goals", "goalList", "教学目标", "目标"));
            draft.Resources = NormalizeStringList(GetFirstToken(draftObject, "resources", "resourceSuggestions", "materials", "资源建议", "教学资源"));
            draft.Assessment = NormalizeStringList(GetFirstToken(draftObject, "assessment", "assessmentDesign", "checks", "评价设计", "评价"));
            draft.TeacherReminder = NormalizeString(GetFirstToken(draftObject, "teacherReminder", "reminder", "teacherTip", "教师提醒", "温馨提示"));
            draft.ActivitySteps = NormalizeSteps(GetFirstToken(draftObject, "activitySteps", "steps", "stepFlow", "flow", "活动步骤", "教学步骤"));
            if (draft.ActivitySteps == null)
            {
                return null;
            }

            return IsValidDraft(draft) ? draft : null;
        }

        public static bool IsValidDraft(ActivityPlanDraft draft)
        {
            if (draft == null)
            {
                return false;
            }

            if (draft.TeachingGoals == null || draft.TeachingGoals.Count == 0)
            {
                return false;
            }

            if (draft.ActivitySteps == null || draft.ActivitySteps.Count == 0)
            {
                return false;
            }

            if (draft.Resources == null || draft.Resources.Count == 0)
            {
                return false;
            }

            if (draft.Assessment == null || draft.Assessment.Count == 0)
            {
                return false;
            }

            if (string.IsNullOrEmpty(SafeTrim(draft.TeacherReminder)))
            {
                return false;
            }

            foreach (ActivityPlanDraftStep step in draft.ActivitySteps)
            {
                if (!IsValidStep(step))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsValidStep(ActivityPlanDraftStep step)
        {
            if (step == null)
            {
                return false;
            }

            return !string.IsNullOrEmpty(SafeTrim(step.Title))
                && !string.IsNullOrEmpty(SafeTrim(step.Minutes))
                && !string.IsNullOrEmpty(SafeTrim(step.TeacherAction))
                && !string.IsNullOrEmpty(SafeTrim(step.StudentAction))
                && !string.IsNullOrEmpty(SafeTrim(step.InteractionMethod))
                && !string.IsNullOrEmpty(SafeTrim(step.ResourceSuggestion))
                && !string.IsNullOrEmpty(SafeTrim(step.AssessmentCheck));
        }

        private static JToken ParseRootToken(string responseText)
        {
            string trimmed = SafeTrim(responseText);
            if (string.IsNullOrEmpty(trimmed))
            {
                return null;
            }

            trimmed = StripCodeFence(trimmed);

            JToken token = TryParseToken(trimmed);
            if (token != null)
            {
                return token;
            }

            string jsonObjectText = ExtractEnclosedSegment(trimmed, '{', '}');
            if (!string.IsNullOrEmpty(jsonObjectText))
            {
                token = TryParseToken(jsonObjectText);
                if (token != null)
                {
                    return token;
                }
            }

            return null;
        }

        private static JObject ExtractDraftObject(JToken token)
        {
            JObject obj = token as JObject;
            if (obj == null)
            {
                return null;
            }

            JToken nested = GetFirstToken(obj, "draft", "data", "result");
            JObject nestedObject = nested as JObject;
            if (nestedObject != null)
            {
                obj = nestedObject;
            }

            if (GetFirstToken(obj, "teachingGoals", "goals", "goalList", "教学目标", "目标") != null
                || GetFirstToken(obj, "activitySteps", "steps", "stepFlow", "flow", "活动步骤", "教学步骤") != null)
            {
                return obj;
            }

            return null;
        }

        private static JToken GetFirstToken(JObject obj, params string[] names)
        {
            if (obj == null || names == null)
            {
                return null;
            }

            foreach (string name in names)
            {
                foreach (JProperty property in obj.Properties())
                {
                    if (string.Equals(property.Name, name, StringComparison.OrdinalIgnoreCase))
                    {
                        return property.Value;
                    }
                }
            }

            return null;
        }

        private static List<ActivityPlanDraftStep> NormalizeSteps(JToken token)
        {
            List<ActivityPlanDraftStep> steps = new List<ActivityPlanDraftStep>();
            if (token == null)
            {
                return steps;
            }

            JArray array = token as JArray;
            if (array == null)
            {
                JObject obj = token as JObject;
                if (obj != null)
                {
                    JToken nested = GetFirstToken(obj, "items", "steps", "activitySteps", "list", "活动步骤");
                    array = nested as JArray;
                }
            }

            if (array == null)
            {
                return steps;
            }

            int index = 1;
            foreach (JToken item in array)
            {
                ActivityPlanDraftStep step = NormalizeStep(item as JObject, index);
                if (step == null)
                {
                    return null;
                }
                steps.Add(step);
                index++;
            }

            return steps;
        }

        private static ActivityPlanDraftStep NormalizeStep(JObject stepObject, int sort)
        {
            if (stepObject == null)
            {
                return null;
            }

            ActivityPlanDraftStep step = new ActivityPlanDraftStep();
            step.Sort = sort;
            step.Title = NormalizeString(GetFirstToken(stepObject, "title", "stepTitle", "name", "label", "标题", "步骤标题"));
            step.Minutes = NormalizeMinutes(GetFirstToken(stepObject, "minutes", "duration", "time", "时长", "分钟", "用时"));
            step.TeacherAction = NormalizeString(GetFirstToken(stepObject, "teacherAction", "teacher", "teacherTask", "teacherActivity", "教师活动", "教师行为"));
            step.StudentAction = NormalizeString(GetFirstToken(stepObject, "studentAction", "student", "studentTask", "studentActivity", "学生活动", "学生行为"));
            step.InteractionMethod = NormalizeString(GetFirstToken(stepObject, "interactionMethod", "interaction", "interactionType", "互动方式", "互动方法"));
            step.ResourceSuggestion = NormalizeString(GetFirstToken(stepObject, "resourceSuggestion", "resource", "resourceTip", "stepResource", "资源建议", "资源"));
            step.AssessmentCheck = NormalizeString(GetFirstToken(stepObject, "assessmentCheck", "assessment", "check", "checkPoint", "评价检查", "评价点", "检测点"));

            return IsValidStep(step) ? step : null;
        }

        private static List<string> NormalizeStringList(JToken token)
        {
            List<string> items = new List<string>();
            if (token == null)
            {
                return items;
            }

            JArray array = token as JArray;
            if (array != null)
            {
                foreach (JToken item in array)
                {
                    string value = NormalizeString(item);
                    if (!string.IsNullOrEmpty(value))
                    {
                        items.Add(value);
                    }
                }
                return items.Distinct().ToList();
            }

            JObject obj = token as JObject;
            if (obj != null)
            {
                foreach (JProperty property in obj.Properties())
                {
                    string value = NormalizeString(property.Value);
                    if (!string.IsNullOrEmpty(value))
                    {
                        items.Add(value);
                    }
                }
                return items.Distinct().ToList();
            }

            string text = token.Type == JTokenType.String ? token.ToString() : NormalizeString(token);
            if (string.IsNullOrEmpty(text))
            {
                return items;
            }

            foreach (string line in SplitListText(text))
            {
                if (!string.IsNullOrEmpty(line))
                {
                    items.Add(line);
                }
            }

            return items.Distinct().ToList();
        }

        private static string NormalizeMinutes(JToken token)
        {
            string value = NormalizeString(token);
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            if (value.IndexOf("分钟", StringComparison.Ordinal) >= 0 || value.IndexOf("课时", StringComparison.Ordinal) >= 0)
            {
                return value;
            }

            int number;
            if (int.TryParse(value, out number))
            {
                return number.ToString() + "分钟";
            }

            return value;
        }

        private static string NormalizeString(JToken token)
        {
            if (token == null)
            {
                return string.Empty;
            }

            JValue value = token as JValue;
            if (value != null)
            {
                return CollapseText(value.ToString());
            }

            JArray array = token as JArray;
            if (array != null)
            {
                List<string> items = new List<string>();
                foreach (JToken item in array)
                {
                    string text = NormalizeString(item);
                    if (!string.IsNullOrEmpty(text))
                    {
                        items.Add(text);
                    }
                }
                return string.Join("；", items.ToArray());
            }

            JObject obj = token as JObject;
            if (obj != null)
            {
                List<string> items = new List<string>();
                foreach (JProperty property in obj.Properties())
                {
                    string text = NormalizeString(property.Value);
                    if (!string.IsNullOrEmpty(text))
                    {
                        items.Add(text);
                    }
                }
                return string.Join("；", items.ToArray());
            }

            return CollapseText(token.ToString());
        }

        private static IEnumerable<string> SplitListText(string text)
        {
            List<string> result = new List<string>();
            string[] lines = text.Replace("\r", "\n").Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                string cleaned = CleanListMarker(line);
                if (!string.IsNullOrEmpty(cleaned))
                {
                    result.Add(cleaned);
                }
            }

            if (result.Count > 1)
            {
                return result;
            }

            string[] segments = text.Split(new[] { '；', ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (segments.Length > 1)
            {
                List<string> segmented = new List<string>();
                foreach (string segment in segments)
                {
                    string cleaned = CleanListMarker(segment);
                    if (!string.IsNullOrEmpty(cleaned))
                    {
                        segmented.Add(cleaned);
                    }
                }
                if (segmented.Count > 0)
                {
                    return segmented;
                }
            }

            string single = CleanListMarker(text);
            if (!string.IsNullOrEmpty(single))
            {
                result.Clear();
                result.Add(single);
            }

            return result;
        }

        private static string CollapseText(string text)
        {
            string value = SafeTrim(text);
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            value = value.Replace("\r\n", "\n").Replace("\r", "\n");
            string[] parts = value.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> cleaned = new List<string>();
            foreach (string part in parts)
            {
                string item = SafeTrim(part);
                if (!string.IsNullOrEmpty(item))
                {
                    cleaned.Add(item);
                }
            }

            return string.Join(" ", cleaned.ToArray());
        }

        private static string CleanListMarker(string text)
        {
            string value = SafeTrim(text);
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            while (value.Length > 0)
            {
                char first = value[0];
                if (first == '-' || first == '*' || first == '•' || first == '·')
                {
                    value = SafeTrim(value.Substring(1));
                    continue;
                }

                if (char.IsDigit(first))
                {
                    int i = 0;
                    while (i < value.Length && (char.IsDigit(value[i]) || value[i] == '.' || value[i] == '、' || value[i] == ')' || value[i] == '）'))
                    {
                        i++;
                    }
                    if (i > 0)
                    {
                        value = SafeTrim(value.Substring(i));
                        continue;
                    }
                }

                break;
            }

            return value;
        }

        private static string StripCodeFence(string text)
        {
            if (!text.StartsWith("```", StringComparison.Ordinal))
            {
                return text;
            }

            int firstLineBreak = text.IndexOf('\n');
            if (firstLineBreak < 0)
            {
                return text.Replace("```", string.Empty).Trim();
            }

            string body = text.Substring(firstLineBreak + 1);
            int fenceIndex = body.LastIndexOf("```", StringComparison.Ordinal);
            if (fenceIndex >= 0)
            {
                body = body.Substring(0, fenceIndex);
            }

            return body.Trim();
        }

        private static string ExtractEnclosedSegment(string text, char openChar, char closeChar)
        {
            int start = text.IndexOf(openChar);
            int end = text.LastIndexOf(closeChar);
            if (start < 0 || end <= start)
            {
                return string.Empty;
            }

            return text.Substring(start, end - start + 1);
        }

        private static JToken TryParseToken(string text)
        {
            try
            {
                return JToken.Parse(text);
            }
            catch
            {
                return null;
            }
        }

        private static string SafeTrim(string value)
        {
            return value == null ? string.Empty : value.Trim();
        }
    }
}
