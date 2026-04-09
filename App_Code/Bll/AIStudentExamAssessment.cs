namespace LearnSite.BLL
{
    public class AIStudentExamAssessment
    {
        private readonly LearnSite.DAL.AIStudentExamAssessment dal = new LearnSite.DAL.AIStudentExamAssessment();

        public int Add(LearnSite.Model.AIStudentExamAssessment model)
        {
            return dal.Add(model);
        }

        public LearnSite.Model.AIStudentExamAssessment GetLatestByStudentCourse(int sid, int cid)
        {
            return dal.GetLatestByStudentCourse(sid, cid);
        }

        public LearnSite.Model.AIStudentExamAssessment GetLatestByStudentCourseLesson(int sid, int cid, int lid)
        {
            return dal.GetLatestByStudentCourseLesson(sid, cid, lid);
        }
    }
}
