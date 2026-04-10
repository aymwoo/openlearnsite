using System;

namespace LearnSite.Model
{
    /// <summary>
    /// 课程活动计划草案暂存实体。
    /// </summary>
    [Serializable]
    public class CourseActivityPlanDraft
    {
        public CourseActivityPlanDraft()
        {
            Topic = string.Empty;
            Grade = string.Empty;
            Duration = string.Empty;
            TeachingGoalsInput = string.Empty;
            ExistingCourseContentSnapshot = string.Empty;
            DraftJson = string.Empty;
        }

        public int Id { get; set; }

        public int Cid { get; set; }

        public int Hid { get; set; }

        public string Topic { get; set; }

        public string Grade { get; set; }

        public string Duration { get; set; }

        public string TeachingGoalsInput { get; set; }

        public string ExistingCourseContentSnapshot { get; set; }

        public string DraftJson { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
