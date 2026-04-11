using System;

namespace LearnSite.Common
{
    public class WebCoursewareRuntimePayloadResult
    {
        public string Topic { get; set; }

        public string BlockKey { get; set; }

        public int ListMenuId { get; set; }

        public int MissionId { get; set; }

        public WebCoursewareBlockPayload WebCourseware { get; set; }
    }

    public static class WebCoursewareRuntimePayloadResolver
    {
        public static WebCoursewareRuntimePayloadResult Resolve(ActivityPlanSavedDraftPayload savedDraft, int listMenuId, int missionId)
        {
            if (savedDraft == null
                || !string.Equals(savedDraft.DraftType, "fullLesson", StringComparison.OrdinalIgnoreCase)
                || !AIActivityPlanDraftHelper.IsValidFullLessonDraft(savedDraft.FullLessonDraft)
                || savedDraft.PublishLinks == null)
            {
                return null;
            }

            for (int i = 0; i < savedDraft.FullLessonDraft.Blocks.Count; i++)
            {
                FullLessonDraftBlock block = savedDraft.FullLessonDraft.Blocks[i];
                if (block == null
                    || !string.Equals(AIActivityPlanDraftHelper.NormalizeFullLessonBlockType(block.BlockType), "webcourseware", StringComparison.Ordinal)
                    || !AIActivityPlanDraftHelper.IsValidWebCoursewarePayload(block.WebCourseware))
                {
                    continue;
                }

                ActivityPlanPublishLinkPayload link = AIActivityPlanSavedDraftHelper.GetPublishLink(savedDraft.PublishLinks, block.BlockKey);
                if (!IsMatchingLink(link, listMenuId, missionId))
                {
                    continue;
                }

                return new WebCoursewareRuntimePayloadResult
                {
                    Topic = savedDraft.Topic,
                    BlockKey = block.BlockKey,
                    ListMenuId = link.ListMenuId.GetValueOrDefault(),
                    MissionId = link.MissionId.GetValueOrDefault(),
                    WebCourseware = block.WebCourseware
                };
            }

            return null;
        }

        private static bool IsMatchingLink(ActivityPlanPublishLinkPayload link, int listMenuId, int missionId)
        {
            if (link == null
                || !string.Equals(AIActivityPlanDraftHelper.NormalizeFullLessonBlockType(link.BlockType), "webcourseware", StringComparison.Ordinal))
            {
                return false;
            }

            if (listMenuId > 0 && link.ListMenuId.GetValueOrDefault() == listMenuId)
            {
                return true;
            }

            if (missionId > 0 && link.MissionId.GetValueOrDefault() == missionId)
            {
                return true;
            }

            return false;
        }
    }
}
