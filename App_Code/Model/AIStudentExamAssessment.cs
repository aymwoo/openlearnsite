using System;

namespace LearnSite.Model
{
    [Serializable]
    public class AIStudentExamAssessment
    {
        public int Id { get; set; }
        public int? Fid { get; set; }
        public int? Sid { get; set; }
        public string Snum { get; set; }
        public string Sname { get; set; }
        public int? Cid { get; set; }
        public int? Lid { get; set; }
        public int? Vid { get; set; }
        public string ProviderName { get; set; }
        public string SkillName { get; set; }
        public string Summary { get; set; }
        public string AssessmentContent { get; set; }
        public string LearningLog { get; set; }
        public string AnswerLog { get; set; }
        public int? Score { get; set; }
        public int? QuestionCount { get; set; }
        public bool IsFallback { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
