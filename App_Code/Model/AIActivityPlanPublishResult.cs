using System.Collections.Generic;

namespace LearnSite.Model
{
    public class AIActivityPlanPublishedBlockResult
    {
        public string BlockKey { get; set; }

        public string BlockType { get; set; }

        public string Title { get; set; }

        public int? MissionId { get; set; }

        public int? ExamId { get; set; }

        public int? PaperId { get; set; }

        public int ListMenuId { get; set; }

        public int? ListMenuType { get; set; }

        public string RuntimeRouteType { get; set; }
    }

    public class AIActivityPlanPublishResult
    {
        public int MissionId { get; set; }

        public int ListMenuId { get; set; }

        public string MissionTitle { get; set; }

        public bool PublishedToStudents { get; set; }

        public string UpdatedCourseContent { get; set; }

        public bool IsFullLessonPublish { get; set; }

        public List<AIActivityPlanPublishedBlockResult> PublishedBlocks { get; set; }
    }
}
