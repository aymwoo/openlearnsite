namespace LearnSite.BLL
{
    public class AIActivityPlanPublisher
    {
        private readonly LearnSite.DAL.AIActivityPlanPublisher dal = new LearnSite.DAL.AIActivityPlanPublisher();

        public LearnSite.Model.AIActivityPlanPublishResult Publish(LearnSite.Model.AIActivityPlanPublishRequest request)
        {
            if (request == null || request.Cid <= 0 || request.Hid <= 0)
            {
                return null;
            }

            return dal.Publish(request);
        }
    }
}
