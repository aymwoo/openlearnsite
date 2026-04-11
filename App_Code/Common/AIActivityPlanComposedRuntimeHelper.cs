using System;
using System.Collections.Generic;

namespace LearnSite.Common
{
    public class AIActivityPlanComposedRuntimeBlockSummary
    {
        public int Sort { get; set; }

        public string BlockKey { get; set; }

        public string BlockType { get; set; }

        public string Title { get; set; }

        public string Minutes { get; set; }

        public int ListMenuId { get; set; }

        public int ListMenuType { get; set; }

        public string RuntimeRouteType { get; set; }

        public string RuntimeUrl { get; set; }

        public int? MissionId { get; set; }

        public int? ExamId { get; set; }

        public int? PaperId { get; set; }

        public bool IsRuntimeReady { get; set; }

        public string CompletionState { get; set; }

        public string CompletionEvidence { get; set; }
    }

    public static class AIActivityPlanComposedRuntimeHelper
    {
        public static List<AIActivityPlanComposedRuntimeBlockSummary> LoadPublishedCourseSummaries(
            int cid,
            int hid,
            Func<int, LearnSite.Model.ListMenu> listMenuResolver,
            Func<int, LearnSite.Model.MenuWorks> menuWorksResolver,
            Func<int, bool> workPassResolver)
        {
            if (cid <= 0 || hid <= 0)
            {
                return null;
            }

            LearnSite.BLL.CourseActivityPlanDraft draftBll = new LearnSite.BLL.CourseActivityPlanDraft();
            LearnSite.Model.CourseActivityPlanDraft record = draftBll.GetCurrentByCourse(cid, hid);
            if (record == null)
            {
                return null;
            }

            ActivityPlanSavedDraftPayload savedDraft = AIActivityPlanSavedDraftHelper.ParseFullLessonRecord(record, cid, hid);
            return BuildComposedRuntimeSummaries(savedDraft, listMenuResolver, menuWorksResolver, workPassResolver);
        }

        public static AIActivityPlanComposedRuntimeBlockSummary FindSummaryByListMenuId(
            IList<AIActivityPlanComposedRuntimeBlockSummary> summaries,
            int listMenuId)
        {
            if (summaries == null || listMenuId <= 0)
            {
                return null;
            }

            for (int i = 0; i < summaries.Count; i++)
            {
                AIActivityPlanComposedRuntimeBlockSummary summary = summaries[i];
                if (summary != null && summary.ListMenuId == listMenuId)
                {
                    return summary;
                }
            }

            return null;
        }

        public static List<AIActivityPlanComposedRuntimeBlockSummary> BuildComposedRuntimeSummaries(
            ActivityPlanSavedDraftPayload savedDraft,
            Func<int, LearnSite.Model.ListMenu> listMenuResolver,
            Func<int, LearnSite.Model.MenuWorks> menuWorksResolver,
            Func<int, bool> workPassResolver)
        {
            if (savedDraft == null
                || !string.Equals(savedDraft.DraftType, "fullLesson", StringComparison.OrdinalIgnoreCase)
                || !AIActivityPlanDraftHelper.IsValidFullLessonDraft(savedDraft.FullLessonDraft)
                || savedDraft.PublishLinks == null)
            {
                return null;
            }

            List<AIActivityPlanComposedRuntimeBlockSummary> summaries = new List<AIActivityPlanComposedRuntimeBlockSummary>();
            for (int i = 0; i < savedDraft.FullLessonDraft.Blocks.Count; i++)
            {
                FullLessonDraftBlock block = savedDraft.FullLessonDraft.Blocks[i];
                AIActivityPlanComposedRuntimeBlockSummary summary;
                if (!TryBuildBlockSummary(savedDraft.PublishLinks, block, listMenuResolver, menuWorksResolver, workPassResolver, out summary))
                {
                    return null;
                }

                summaries.Add(summary);
            }

            return summaries;
        }

        private static bool TryBuildBlockSummary(
            IDictionary<string, ActivityPlanPublishLinkPayload> publishLinks,
            FullLessonDraftBlock block,
            Func<int, LearnSite.Model.ListMenu> listMenuResolver,
            Func<int, LearnSite.Model.MenuWorks> menuWorksResolver,
            Func<int, bool> workPassResolver,
            out AIActivityPlanComposedRuntimeBlockSummary summary)
        {
            summary = null;
            RuntimeDefinition runtime;
            if (!TryResolveRuntime(block, out runtime))
            {
                return false;
            }

            ActivityPlanPublishLinkPayload link = AIActivityPlanSavedDraftHelper.GetPublishLink(publishLinks, block.BlockKey);
            if (link != null && !IsConsistentPublishLink(block, link))
            {
                return false;
            }

            summary = new AIActivityPlanComposedRuntimeBlockSummary
            {
                Sort = block.Sort,
                BlockKey = block.BlockKey,
                BlockType = block.BlockType,
                Title = block.Title,
                Minutes = block.Minutes,
                ListMenuType = runtime.ListMenuType,
                RuntimeRouteType = runtime.RuntimeRouteType,
                RuntimeUrl = string.Empty,
                CompletionState = "unknown",
                CompletionEvidence = "missingPublishLink"
            };

            if (link == null)
            {
                return true;
            }

            summary.ListMenuId = link.ListMenuId.GetValueOrDefault();
            summary.MissionId = link.MissionId;
            summary.ExamId = link.ExamId;
            summary.PaperId = link.PaperId;

            int runtimeTargetId = GetRuntimeTargetId(runtime, link);
            if (summary.ListMenuId <= 0 || runtimeTargetId <= 0)
            {
                summary.CompletionEvidence = "missingLinkedIds";
                return true;
            }

            if (listMenuResolver != null)
            {
                LearnSite.Model.ListMenu menu = listMenuResolver(summary.ListMenuId);
                if (!IsConsistentListMenu(menu, runtime, runtimeTargetId))
                {
                    return false;
                }
            }

            summary.IsRuntimeReady = true;
            summary.RuntimeUrl = runtime.RuntimePath + "?lid=" + summary.ListMenuId;

            bool hasMenuCompletion = false;
            if (menuWorksResolver != null)
            {
                LearnSite.Model.MenuWorks menuWork = menuWorksResolver(summary.ListMenuId);
                hasMenuCompletion = menuWork != null && menuWork.Klid.GetValueOrDefault() == summary.ListMenuId;
            }

            bool hasWorkPass = false;
            if (runtime.UsesMissionCompletionEvidence && summary.MissionId.HasValue && workPassResolver != null)
            {
                hasWorkPass = workPassResolver(summary.MissionId.Value);
            }

            if (hasMenuCompletion)
            {
                summary.CompletionState = "completed";
                summary.CompletionEvidence = "menuWorks";
            }
            else if (hasWorkPass)
            {
                summary.CompletionState = "completed";
                summary.CompletionEvidence = "workPass";
            }
            else
            {
                summary.CompletionState = "incomplete";
                summary.CompletionEvidence = "none";
            }

            return true;
        }

        private static bool TryResolveRuntime(FullLessonDraftBlock block, out RuntimeDefinition runtime)
        {
            runtime = null;
            if (block == null || !AIActivityPlanDraftHelper.IsSupportedPublishedBlockType(block.BlockType))
            {
                return false;
            }

            string normalizedBlockType = AIActivityPlanDraftHelper.NormalizeFullLessonBlockType(block.BlockType);
            switch (normalizedBlockType)
            {
                case "mission":
                    runtime = new RuntimeDefinition(1, "showmission", "~/student/showmission.aspx", true, true);
                    return true;
                case "guidedinquiry":
                    if (!AIActivityPlanDraftHelper.IsValidGuidedInquiryPayload(block.GuidedInquiry))
                    {
                        return false;
                    }
                    runtime = new RuntimeDefinition(1, "showmission", "~/student/showmission.aspx", true, true);
                    return true;
                case "resource-study":
                    if (!AIActivityPlanDraftHelper.IsValidResourceStudyPayload(block.ResourceStudy))
                    {
                        return false;
                    }
                    runtime = new RuntimeDefinition(6, "description", "~/student/description.aspx", true, false);
                    return true;
                case "webcourseware":
                    if (!AIActivityPlanDraftHelper.IsValidWebCoursewarePayload(block.WebCourseware))
                    {
                        return false;
                    }
                    runtime = new RuntimeDefinition(38, "ware", "~/student/ware.aspx", true, false);
                    return true;
                case "quiz":
                    if (!AIActivityPlanDraftHelper.IsValidQuizPayload(block.Quiz))
                    {
                        return false;
                    }
                    runtime = new RuntimeDefinition(39, "preview", "~/webform/preview.aspx", false, false);
                    return true;
                default:
                    return false;
            }
        }

        private static bool IsConsistentPublishLink(FullLessonDraftBlock block, ActivityPlanPublishLinkPayload link)
        {
            if (block == null || link == null)
            {
                return false;
            }

            return string.Equals(block.BlockKey, link.BlockKey, StringComparison.OrdinalIgnoreCase)
                && string.Equals(
                    AIActivityPlanDraftHelper.NormalizeFullLessonBlockType(block.BlockType),
                    AIActivityPlanDraftHelper.NormalizeFullLessonBlockType(link.BlockType),
                    StringComparison.Ordinal);
        }

        private static bool IsConsistentListMenu(LearnSite.Model.ListMenu menu, RuntimeDefinition runtime, int runtimeTargetId)
        {
            return menu != null
                && menu.Lid > 0
                && menu.Ltype.GetValueOrDefault() == runtime.ListMenuType
                && menu.Lxid.GetValueOrDefault() == runtimeTargetId;
        }

        private static int GetRuntimeTargetId(RuntimeDefinition runtime, ActivityPlanPublishLinkPayload link)
        {
            if (runtime == null || link == null)
            {
                return 0;
            }

            if (runtime.UsesMissionTarget)
            {
                return link.MissionId.GetValueOrDefault();
            }

            if (runtime.ListMenuType == 39)
            {
                return link.ExamId.GetValueOrDefault();
            }

            return 0;
        }

        private sealed class RuntimeDefinition
        {
            public RuntimeDefinition(int listMenuType, string runtimeRouteType, string runtimePath, bool usesMissionTarget, bool usesMissionCompletionEvidence)
            {
                ListMenuType = listMenuType;
                RuntimeRouteType = runtimeRouteType;
                RuntimePath = runtimePath;
                UsesMissionTarget = usesMissionTarget;
                UsesMissionCompletionEvidence = usesMissionCompletionEvidence;
            }

            public int ListMenuType { get; private set; }

            public string RuntimeRouteType { get; private set; }

            public string RuntimePath { get; private set; }

            public bool UsesMissionTarget { get; private set; }

            public bool UsesMissionCompletionEvidence { get; private set; }
        }
    }
}
