using System;
using System.Collections.Generic;
using System.Linq;
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

        public string DraftType { get; set; }

        public FullLessonDraft FullLessonDraft { get; set; }

        public int? LinkedMissionId { get; set; }

        public int? LinkedListMenuId { get; set; }

        public Dictionary<string, ActivityPlanPublishLinkPayload> PublishLinks { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class ActivityPlanPublishLinkPayload
    {
        public string BlockKey { get; set; }

        public string BlockType { get; set; }

        public int? MissionId { get; set; }

        public int? ExamId { get; set; }

        public int? PaperId { get; set; }

        public int? ListMenuId { get; set; }
    }

    internal class ActivityPlanSavedDraftJsonModel
    {
        public string DraftType { get; set; }

        public string Topic { get; set; }

        public string Grade { get; set; }

        public string Duration { get; set; }

        public string TeachingGoals { get; set; }

        public string ExistingCourseContent { get; set; }

        public object Draft { get; set; }

        public object FullLessonDraft { get; set; }

        public object PublishLinks { get; set; }
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

        public static LearnSite.Model.CourseActivityPlanDraft BuildFullLessonRecord(int cid, int hid, string topic, string grade, string duration, string teachingGoals, string existingCourseContent, FullLessonDraft draft, int? linkedMissionId = null, int? linkedListMenuId = null, IDictionary<string, ActivityPlanPublishLinkPayload> publishLinks = null)
        {
            string normalizedTopic = AIActivityPlanPromptBuilder.BoundText(topic, AIActivityPlanPromptBuilder.MaxTopicLength);
            if (cid <= 0 || hid <= 0 || string.IsNullOrEmpty(normalizedTopic) || !AIActivityPlanDraftHelper.IsValidFullLessonDraft(draft))
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
                DraftJson = SerializeFullLessonDraftJson(normalizedTopic, normalizedGrade, normalizedDuration, normalizedGoals, normalizedExisting, draft, publishLinks),
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
                DraftType = "activityPlan",
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

        public static ActivityPlanSavedDraftPayload ParseFullLessonRecord(LearnSite.Model.CourseActivityPlanDraft record, int expectedCid, int expectedHid)
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

            if (jsonModel == null || jsonModel.FullLessonDraft == null)
            {
                return null;
            }

            string reparsedJson = JsonConvert.SerializeObject(jsonModel.FullLessonDraft);
            FullLessonDraft draft = AIActivityPlanDraftHelper.ParseFullLessonDraft(reparsedJson);
            if (!AIActivityPlanDraftHelper.IsValidFullLessonDraft(draft))
            {
                return null;
            }

            return new ActivityPlanSavedDraftPayload
            {
                Cid = record.Cid,
                Hid = record.Hid,
                Topic = topic,
                DraftType = "fullLesson",
                Grade = AIActivityPlanPromptBuilder.BoundText(FirstNonEmpty(jsonModel.Grade, record.Grade), AIActivityPlanPromptBuilder.MaxGradeLength),
                Duration = AIActivityPlanPromptBuilder.BoundText(FirstNonEmpty(jsonModel.Duration, record.Duration), AIActivityPlanPromptBuilder.MaxDurationLength),
                TeachingGoals = AIActivityPlanPromptBuilder.BoundText(FirstNonEmpty(jsonModel.TeachingGoals, record.TeachingGoalsInput), AIActivityPlanPromptBuilder.MaxTeachingGoalsLength),
                ExistingCourseContent = AIActivityPlanPromptBuilder.BoundText(FirstNonEmpty(jsonModel.ExistingCourseContent, record.ExistingCourseContentSnapshot), 4000),
                Draft = null,
                FullLessonDraft = draft,
                LinkedMissionId = record.LinkedMissionId,
                LinkedListMenuId = record.LinkedListMenuId,
                PublishLinks = ParsePublishLinks(jsonModel.PublishLinks),
                UpdatedAt = record.UpdatedAt
            };
        }

        public static ActivityPlanPublishLinkPayload GetPublishLink(IDictionary<string, ActivityPlanPublishLinkPayload> publishLinks, string blockKey)
        {
            if (publishLinks == null)
            {
                return null;
            }

            string normalizedBlockKey = AIActivityPlanPromptBuilder.BoundText(blockKey, 100);
            if (string.IsNullOrEmpty(normalizedBlockKey))
            {
                return null;
            }

            ActivityPlanPublishLinkPayload link;
            return publishLinks.TryGetValue(normalizedBlockKey, out link) ? link : null;
        }

        private static string SerializeDraftJson(string topic, string grade, string duration, string teachingGoals, string existingCourseContent, ActivityPlanDraft draft)
        {
            return JsonConvert.SerializeObject(new
            {
                draftType = "activityPlan",
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

        private static string SerializeFullLessonDraftJson(string topic, string grade, string duration, string teachingGoals, string existingCourseContent, FullLessonDraft draft, IDictionary<string, ActivityPlanPublishLinkPayload> publishLinks)
        {
            return JsonConvert.SerializeObject(new
            {
                draftType = "fullLesson",
                topic = topic,
                grade = grade,
                duration = duration,
                teachingGoals = teachingGoals,
                existingCourseContent = existingCourseContent,
                fullLessonDraft = new
                {
                    schemaVersion = draft.SchemaVersion,
                    topic = draft.Topic,
                    lessonSummary = draft.LessonSummary,
                    totalMinutes = draft.TotalMinutes,
                    blocks = draft.Blocks
                },
                publishLinks = SerializePublishLinks(publishLinks)
            });
        }

        private static object SerializePublishLinks(IDictionary<string, ActivityPlanPublishLinkPayload> publishLinks)
        {
            if (publishLinks == null || publishLinks.Count == 0)
            {
                return null;
            }

            return publishLinks
                .Where(entry => entry.Value != null && !string.IsNullOrEmpty((entry.Key ?? string.Empty).Trim()))
                .OrderBy(entry => entry.Key, StringComparer.Ordinal)
                .Select(entry => new
                {
                    blockKey = entry.Key,
                    blockType = entry.Value.BlockType ?? string.Empty,
                    missionId = entry.Value.MissionId,
                    examId = entry.Value.ExamId,
                    paperId = entry.Value.PaperId,
                    listMenuId = entry.Value.ListMenuId
                })
                .ToList();
        }

        private static Dictionary<string, ActivityPlanPublishLinkPayload> ParsePublishLinks(object publishLinksToken)
        {
            Dictionary<string, ActivityPlanPublishLinkPayload> result = new Dictionary<string, ActivityPlanPublishLinkPayload>(StringComparer.OrdinalIgnoreCase);
            if (publishLinksToken == null)
            {
                return result;
            }

            try
            {
                List<ActivityPlanPublishLinkPayload> links = JsonConvert.DeserializeObject<List<ActivityPlanPublishLinkPayload>>(JsonConvert.SerializeObject(publishLinksToken));
                if (links == null)
                {
                    return result;
                }

                for (int i = 0; i < links.Count; i++)
                {
                    ActivityPlanPublishLinkPayload link = links[i];
                    string blockKey = AIActivityPlanPromptBuilder.BoundText(link == null ? string.Empty : link.BlockKey, 100);
                    if (string.IsNullOrEmpty(blockKey))
                    {
                        continue;
                    }

                    result[blockKey] = new ActivityPlanPublishLinkPayload
                    {
                        BlockKey = blockKey,
                        BlockType = AIActivityPlanPromptBuilder.BoundText(link.BlockType, 50),
                        MissionId = link.MissionId,
                        ExamId = link.ExamId,
                        PaperId = link.PaperId,
                        ListMenuId = link.ListMenuId
                    };
                }
            }
            catch
            {
                return new Dictionary<string, ActivityPlanPublishLinkPayload>(StringComparer.OrdinalIgnoreCase);
            }

            return result;
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
