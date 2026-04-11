using System;
using Newtonsoft.Json;

namespace LearnSite.Common
{
    public class ActivityPlanSavedDraftPayload
    {
        public int Cid { get; set; }

        public int Hid { get; set; }

        public string Topic { get; set; }

        public string Grade { get; set; }

        public string Duration { get; set; }

        public string TeachingGoals { get; set; }

        public string ExistingCourseContent { get; set; }

        public ActivityPlanDraft Draft { get; set; }

        public int? LinkedMissionId { get; set; }

        public int? LinkedListMenuId { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    internal class ActivityPlanSavedDraftJsonModel
    {
        public string Topic { get; set; }

        public string Grade { get; set; }

        public string Duration { get; set; }

        public string TeachingGoals { get; set; }

        public string ExistingCourseContent { get; set; }

        public object Draft { get; set; }
    }

    public static class AIActivityPlanSavedDraftHelper
    {
        public static LearnSite.Model.CourseActivityPlanDraft BuildRecord(int cid, int hid, string topic, string grade, string duration, string teachingGoals, string existingCourseContent, ActivityPlanDraft draft, int? linkedMissionId = null, int? linkedListMenuId = null)
        {
            string normalizedTopic = AIActivityPlanPromptBuilder.BoundText(topic, AIActivityPlanPromptBuilder.MaxTopicLength);
            if (cid <= 0 || hid <= 0 || string.IsNullOrEmpty(normalizedTopic) || !AIActivityPlanDraftHelper.IsValidDraft(draft))
            {
                return null;
            }

            string normalizedGrade = AIActivityPlanPromptBuilder.BoundText(grade, AIActivityPlanPromptBuilder.MaxGradeLength);
            string normalizedDuration = AIActivityPlanPromptBuilder.BoundText(duration, AIActivityPlanPromptBuilder.MaxDurationLength);
            string normalizedGoals = AIActivityPlanPromptBuilder.BoundText(teachingGoals, AIActivityPlanPromptBuilder.MaxTeachingGoalsLength);
            string normalizedExisting = AIActivityPlanPromptBuilder.BoundText(existingCourseContent, 4000);
            DateTime now = DateTime.Now;

            return new LearnSite.Model.CourseActivityPlanDraft
            {
                Cid = cid,
                Hid = hid,
                Topic = normalizedTopic,
                Grade = normalizedGrade,
                Duration = normalizedDuration,
                TeachingGoalsInput = normalizedGoals,
                ExistingCourseContentSnapshot = normalizedExisting,
                DraftJson = SerializeDraftJson(normalizedTopic, normalizedGrade, normalizedDuration, normalizedGoals, normalizedExisting, draft),
                LinkedMissionId = linkedMissionId,
                LinkedListMenuId = linkedListMenuId,
                CreatedAt = now,
                UpdatedAt = now
            };
        }

        public static ActivityPlanSavedDraftPayload ParseRecord(LearnSite.Model.CourseActivityPlanDraft record, int expectedCid, int expectedHid)
        {
            if (record == null || expectedCid <= 0 || expectedHid <= 0)
            {
                return null;
            }

            if (record.Cid != expectedCid || record.Hid != expectedHid)
            {
                return null;
            }

            string topic = AIActivityPlanPromptBuilder.BoundText(record.Topic, AIActivityPlanPromptBuilder.MaxTopicLength);
            if (string.IsNullOrEmpty(topic))
            {
                return null;
            }

            ActivityPlanSavedDraftJsonModel jsonModel;
            try
            {
                jsonModel = JsonConvert.DeserializeObject<ActivityPlanSavedDraftJsonModel>(record.DraftJson ?? string.Empty);
            }
            catch
            {
                return null;
            }

            if (jsonModel == null || jsonModel.Draft == null)
            {
                return null;
            }

            string reparsedJson = JsonConvert.SerializeObject(jsonModel.Draft);
            ActivityPlanDraft draft = AIActivityPlanDraftHelper.ParseDraft(reparsedJson);
            if (!AIActivityPlanDraftHelper.IsValidDraft(draft))
            {
                return null;
            }

            return new ActivityPlanSavedDraftPayload
            {
                Cid = record.Cid,
                Hid = record.Hid,
                Topic = topic,
                Grade = AIActivityPlanPromptBuilder.BoundText(FirstNonEmpty(jsonModel.Grade, record.Grade), AIActivityPlanPromptBuilder.MaxGradeLength),
                Duration = AIActivityPlanPromptBuilder.BoundText(FirstNonEmpty(jsonModel.Duration, record.Duration), AIActivityPlanPromptBuilder.MaxDurationLength),
                TeachingGoals = AIActivityPlanPromptBuilder.BoundText(FirstNonEmpty(jsonModel.TeachingGoals, record.TeachingGoalsInput), AIActivityPlanPromptBuilder.MaxTeachingGoalsLength),
                ExistingCourseContent = AIActivityPlanPromptBuilder.BoundText(FirstNonEmpty(jsonModel.ExistingCourseContent, record.ExistingCourseContentSnapshot), 4000),
                Draft = draft,
                LinkedMissionId = record.LinkedMissionId,
                LinkedListMenuId = record.LinkedListMenuId,
                UpdatedAt = record.UpdatedAt
            };
        }

        private static string SerializeDraftJson(string topic, string grade, string duration, string teachingGoals, string existingCourseContent, ActivityPlanDraft draft)
        {
            return JsonConvert.SerializeObject(new
            {
                topic = topic,
                grade = grade,
                duration = duration,
                teachingGoals = teachingGoals,
                existingCourseContent = existingCourseContent,
                draft = new
                {
                    teachingGoals = draft.TeachingGoals,
                    activitySteps = draft.ActivitySteps,
                    resources = draft.Resources,
                    assessment = draft.Assessment,
                    teacherReminder = draft.TeacherReminder
                }
            });
        }

        private static string FirstNonEmpty(string first, string second)
        {
            string primary = first == null ? string.Empty : first.Trim();
            if (!string.IsNullOrEmpty(primary))
            {
                return primary;
            }

            return second == null ? string.Empty : second.Trim();
        }
    }
}
