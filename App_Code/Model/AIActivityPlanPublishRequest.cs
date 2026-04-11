using System.Collections.Generic;

namespace LearnSite.Model
{
    public class AIActivityPlanPublishRequest
    {
        public int Cid { get; set; }

        public int Hid { get; set; }

        public string Topic { get; set; }

        public bool PublishToStudents { get; set; }

        public List<string> SelectedSectionKeys { get; set; }

        public LearnSite.Common.ActivityPlanDraft Draft { get; set; }

        public LearnSite.Common.FullLessonDraft FullLessonDraft { get; set; }

        public string ExistingCourseContent { get; set; }
    }
}
