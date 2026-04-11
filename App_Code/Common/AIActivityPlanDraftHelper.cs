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

    public class FullLessonDraft
    {
        public FullLessonDraft()
        {
            SchemaVersion = "v1.2-full-lesson";
            Topic = string.Empty;
            LessonSummary = string.Empty;
            TotalMinutes = string.Empty;
            Blocks = new List<FullLessonDraftBlock>();
        }

        public string SchemaVersion { get; set; }
        public string Topic { get; set; }
        public string LessonSummary { get; set; }
        public string TotalMinutes { get; set; }
        public List<FullLessonDraftBlock> Blocks { get; set; }
    }

    public class FullLessonDraftBlock
    {
        public FullLessonDraftBlock()
        {
            BlockKey = string.Empty;
            BlockType = string.Empty;
            Title = string.Empty;
            Minutes = string.Empty;
            TeachingPurpose = string.Empty;
            LessonPosition = string.Empty;
            TeacherAction = string.Empty;
            StudentAction = string.Empty;
            Materials = new List<string>();
            AssessmentFocus = string.Empty;
            Status = string.Empty;
        }

        public string BlockKey { get; set; }
        public int Sort { get; set; }
        public string BlockType { get; set; }
        public string Title { get; set; }
        public string Minutes { get; set; }
        public string TeachingPurpose { get; set; }
        public string LessonPosition { get; set; }
        public string TeacherAction { get; set; }
        public string StudentAction { get; set; }
        public List<string> Materials { get; set; }
        public string AssessmentFocus { get; set; }
        public string Status { get; set; }
    }

    public static class AIActivityPlanDraftHelper
    {
        private static readonly string[] AllowedSectionTargets = new[]
        {
            "teachingGoals",
            "activitySteps",
            "resources",
            "assessment",
            "teacherReminder"
        };

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

        public static FullLessonDraft ParseFullLessonDraft(string responseText)
        {
            JToken token = ParseRootToken(responseText);
            if (token == null)
            {
                return null;
            }

            JObject draftObject = ExtractFullLessonDraftObject(token);
            if (draftObject == null)
            {
                return null;
            }

            FullLessonDraft draft = new FullLessonDraft();
            draft.SchemaVersion = NormalizeString(GetFirstToken(draftObject, "schemaVersion"));
            if (string.IsNullOrEmpty(draft.SchemaVersion))
            {
                draft.SchemaVersion = "v1.2-full-lesson";
            }

            draft.Topic = NormalizeString(GetFirstToken(draftObject, "topic", "lessonTopic", "主题", "课题"));
            draft.LessonSummary = NormalizeString(GetFirstToken(draftObject, "lessonSummary", "summary", "lessonOverview", "整课概述", "课程概述"));
            draft.TotalMinutes = NormalizeMinutes(GetFirstToken(draftObject, "totalMinutes", "duration", "lessonMinutes", "总时长", "课时"));
            draft.Blocks = NormalizeFullLessonBlocks(GetFirstToken(draftObject, "blocks", "lessonBlocks", "fullLessonBlocks", "环节", "活动块"));
            if (draft.Blocks == null)
            {
                return null;
            }

            return IsValidFullLessonDraft(draft) ? draft : null;
        }

        public static bool IsSupportedSectionTarget(string sectionTarget)
        {
            string normalizedTarget = SafeTrim(sectionTarget);
            return AllowedSectionTargets.Any(target => string.Equals(target, normalizedTarget, StringComparison.OrdinalIgnoreCase));
        }

        public static bool IsValidFullLessonDraft(FullLessonDraft draft)
        {
            if (draft == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(SafeTrim(draft.Topic))
                || string.IsNullOrEmpty(SafeTrim(draft.LessonSummary))
                || string.IsNullOrEmpty(SafeTrim(draft.TotalMinutes))
                || draft.Blocks == null
                || draft.Blocks.Count == 0)
            {
                return false;
            }

            HashSet<string> keys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < draft.Blocks.Count; i++)
            {
                FullLessonDraftBlock block = draft.Blocks[i];
                if (!IsValidFullLessonBlock(block))
                {
                    return false;
                }

                if (!keys.Add(block.BlockKey))
                {
                    return false;
                }

                if (block.Sort != i + 1)
                {
                    return false;
                }
            }

            return true;
        }

        public static string[] GetAllowedSectionTargets()
        {
            return AllowedSectionTargets.ToArray();
        }

        public static ActivityPlanDraft MergeRegeneratedSection(ActivityPlanDraft currentDraft, string sectionTarget, string responseText)
        {
            if (!IsValidDraft(currentDraft) || !IsSupportedSectionTarget(sectionTarget))
            {
                return null;
            }

            JObject sectionObject = ParseSectionObject(responseText, sectionTarget);
            if (sectionObject == null)
            {
                return null;
            }

            ActivityPlanDraft mergedDraft = CloneDraft(currentDraft);
            string normalizedTarget = NormalizeSectionTarget(sectionTarget);
            if (string.IsNullOrEmpty(normalizedTarget))
            {
                return null;
            }

            if (string.Equals(normalizedTarget, "teachingGoals", StringComparison.Ordinal))
            {
                List<string> goals = NormalizeStringList(sectionObject[normalizedTarget]);
                if (goals == null || goals.Count == 0)
                {
                    return null;
                }
                mergedDraft.TeachingGoals = goals;
            }
            else if (string.Equals(normalizedTarget, "activitySteps", StringComparison.Ordinal))
            {
                List<ActivityPlanDraftStep> steps = NormalizeSteps(sectionObject[normalizedTarget]);
                if (steps == null || steps.Count == 0)
                {
                    return null;
                }
                mergedDraft.ActivitySteps = steps;
            }
            else if (string.Equals(normalizedTarget, "resources", StringComparison.Ordinal))
            {
                List<string> resources = NormalizeStringList(sectionObject[normalizedTarget]);
                if (resources == null || resources.Count == 0)
                {
                    return null;
                }
                mergedDraft.Resources = resources;
            }
            else if (string.Equals(normalizedTarget, "assessment", StringComparison.Ordinal))
            {
                List<string> assessment = NormalizeStringList(sectionObject[normalizedTarget]);
                if (assessment == null || assessment.Count == 0)
                {
                    return null;
                }
                mergedDraft.Assessment = assessment;
            }
            else if (string.Equals(normalizedTarget, "teacherReminder", StringComparison.Ordinal))
            {
                string reminder = NormalizeString(sectionObject[normalizedTarget]);
                if (string.IsNullOrEmpty(reminder))
                {
                    return null;
                }
                mergedDraft.TeacherReminder = reminder;
            }

            return IsValidDraft(mergedDraft) ? mergedDraft : null;
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

        private static bool IsValidFullLessonBlock(FullLessonDraftBlock block)
        {
            if (block == null)
            {
                return false;
            }

            return !string.IsNullOrEmpty(SafeTrim(block.BlockKey))
                && block.Sort > 0
                && !string.IsNullOrEmpty(SafeTrim(block.BlockType))
                && !string.IsNullOrEmpty(SafeTrim(block.Title))
                && !string.IsNullOrEmpty(SafeTrim(block.Minutes))
                && !string.IsNullOrEmpty(SafeTrim(block.TeachingPurpose))
                && !string.IsNullOrEmpty(SafeTrim(block.LessonPosition))
                && !string.IsNullOrEmpty(SafeTrim(block.TeacherAction))
                && !string.IsNullOrEmpty(SafeTrim(block.StudentAction))
                && block.Materials != null
                && block.Materials.Count > 0
                && !string.IsNullOrEmpty(SafeTrim(block.AssessmentFocus));
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

        private static JObject ExtractFullLessonDraftObject(JToken token)
        {
            JObject obj = token as JObject;
            if (obj == null)
            {
                return null;
            }

            JToken nested = GetFirstToken(obj, "draft", "data", "result", "fullLessonDraft");
            JObject nestedObject = nested as JObject;
            if (nestedObject != null)
            {
                obj = nestedObject;
            }

            if (GetFirstToken(obj, "blocks", "lessonBlocks", "fullLessonBlocks", "环节", "活动块") != null)
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

        private static List<FullLessonDraftBlock> NormalizeFullLessonBlocks(JToken token)
        {
            List<FullLessonDraftBlock> blocks = new List<FullLessonDraftBlock>();
            if (token == null)
            {
                return blocks;
            }

            JArray array = token as JArray;
            if (array == null)
            {
                JObject obj = token as JObject;
                if (obj != null)
                {
                    JToken nested = GetFirstToken(obj, "items", "blocks", "lessonBlocks", "list", "环节", "活动块");
                    array = nested as JArray;
                }
            }

            if (array == null)
            {
                return blocks;
            }

            foreach (JToken item in array)
            {
                FullLessonDraftBlock block = NormalizeFullLessonBlock(item as JObject);
                if (block == null)
                {
                    return null;
                }

                blocks.Add(block);
            }

            return blocks;
        }

        private static FullLessonDraftBlock NormalizeFullLessonBlock(JObject blockObject)
        {
            if (blockObject == null)
            {
                return null;
            }

            int sort;
            string sortText = NormalizeString(GetFirstToken(blockObject, "sort", "order", "index", "序号"));
            if (!int.TryParse(sortText, out sort) || sort <= 0)
            {
                return null;
            }

            FullLessonDraftBlock block = new FullLessonDraftBlock();
            block.BlockKey = NormalizeString(GetFirstToken(blockObject, "blockKey", "key", "id", "块标识", "环节标识"));
            block.Sort = sort;
            block.BlockType = NormalizeString(GetFirstToken(blockObject, "blockType", "type", "activityType", "类型", "环节类型"));
            block.Title = NormalizeString(GetFirstToken(blockObject, "title", "name", "label", "标题", "环节标题"));
            block.Minutes = NormalizeMinutes(GetFirstToken(blockObject, "minutes", "duration", "time", "时长", "分钟", "用时"));
            block.TeachingPurpose = NormalizeString(GetFirstToken(blockObject, "teachingPurpose", "purpose", "goal", "教学目的", "教学意图"));
            block.LessonPosition = NormalizeString(GetFirstToken(blockObject, "lessonPosition", "position", "stage", "lessonStage", "教学位置", "课堂位置"));
            block.TeacherAction = NormalizeString(GetFirstToken(blockObject, "teacherAction", "teacher", "teacherTask", "教师活动", "教师行为"));
            block.StudentAction = NormalizeString(GetFirstToken(blockObject, "studentAction", "student", "studentTask", "学生活动", "学生行为"));
            block.Materials = NormalizeStringList(GetFirstToken(blockObject, "materials", "resources", "materialList", "材料", "资源"));
            block.AssessmentFocus = NormalizeString(GetFirstToken(blockObject, "assessmentFocus", "assessment", "check", "评价重点", "评价关注点"));
            block.Status = NormalizeString(GetFirstToken(blockObject, "status", "blockStatus", "状态"));
            if (string.IsNullOrEmpty(block.Status))
            {
                block.Status = "draft";
            }

            return IsValidFullLessonBlock(block) ? block : null;
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

        private static ActivityPlanDraft CloneDraft(ActivityPlanDraft draft)
        {
            ActivityPlanDraft copy = new ActivityPlanDraft();
            copy.TeachingGoals = draft.TeachingGoals == null ? new List<string>() : new List<string>(draft.TeachingGoals);
            copy.Resources = draft.Resources == null ? new List<string>() : new List<string>(draft.Resources);
            copy.Assessment = draft.Assessment == null ? new List<string>() : new List<string>(draft.Assessment);
            copy.TeacherReminder = draft.TeacherReminder ?? string.Empty;
            copy.ActivitySteps = new List<ActivityPlanDraftStep>();

            if (draft.ActivitySteps != null)
            {
                foreach (ActivityPlanDraftStep step in draft.ActivitySteps)
                {
                    copy.ActivitySteps.Add(new ActivityPlanDraftStep
                    {
                        Sort = step == null ? 0 : step.Sort,
                        Title = step == null ? string.Empty : step.Title,
                        Minutes = step == null ? string.Empty : step.Minutes,
                        TeacherAction = step == null ? string.Empty : step.TeacherAction,
                        StudentAction = step == null ? string.Empty : step.StudentAction,
                        InteractionMethod = step == null ? string.Empty : step.InteractionMethod,
                        ResourceSuggestion = step == null ? string.Empty : step.ResourceSuggestion,
                        AssessmentCheck = step == null ? string.Empty : step.AssessmentCheck
                    });
                }
            }

            return copy;
        }

        private static JObject ParseSectionObject(string responseText, string sectionTarget)
        {
            JToken token = ParseRootToken(responseText);
            if (token == null)
            {
                return null;
            }

            JObject sectionObject = ExtractSectionObject(token);
            if (sectionObject == null)
            {
                return null;
            }

            string normalizedTarget = NormalizeSectionTarget(sectionTarget);
            if (string.IsNullOrEmpty(normalizedTarget))
            {
                return null;
            }

            if (sectionObject.Properties().Count() != 1)
            {
                return null;
            }

            JProperty property = sectionObject.Properties().FirstOrDefault();
            if (property == null || !string.Equals(property.Name, normalizedTarget, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            JObject normalizedObject = new JObject();
            normalizedObject[normalizedTarget] = property.Value;
            return normalizedObject;
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

        private static JObject ExtractSectionObject(JToken token)
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

            return obj;
        }

        private static string NormalizeSectionTarget(string sectionTarget)
        {
            string normalizedTarget = SafeTrim(sectionTarget);
            foreach (string allowedTarget in AllowedSectionTargets)
            {
                if (string.Equals(allowedTarget, normalizedTarget, StringComparison.OrdinalIgnoreCase))
                {
                    return allowedTarget;
                }
            }

            return string.Empty;
        }

        private static string SafeTrim(string value)
        {
            return value == null ? string.Empty : value.Trim();
        }
    }
}
