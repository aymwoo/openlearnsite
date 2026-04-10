namespace LearnSite.BLL
{
    public class CourseActivityPlanDraft
    {
        private readonly LearnSite.DAL.CourseActivityPlanDraft dal = new LearnSite.DAL.CourseActivityPlanDraft();

        public LearnSite.Model.CourseActivityPlanDraft GetCurrentByCourse(int cid, int hid)
        {
            if (cid <= 0 || hid <= 0)
            {
                return null;
            }

            return dal.GetCurrentByCourse(cid, hid);
        }

        public bool UpsertCurrent(LearnSite.Model.CourseActivityPlanDraft model)
        {
            if (model == null || model.Cid <= 0 || model.Hid <= 0)
            {
                return false;
            }

            return dal.UpsertCurrent(model);
        }

        public bool DeleteCurrent(int cid, int hid)
        {
            if (cid <= 0 || hid <= 0)
            {
                return false;
            }

            return dal.DeleteCurrent(cid, hid);
        }
    }
}
