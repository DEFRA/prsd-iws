namespace EA.Iws.DataAccess.Repositories
{
    using Domain.NotificationApplication;

    public class SharedUserHistoryRepository : ISharedUserHistoryRepository
    {
        private readonly IwsContext context;

        public SharedUserHistoryRepository(IwsContext context)
        {
            this.context = context;
        }

        public void AddSharedUserHistory(SharedUserHistory sharedUserHistory)
        {
            this.context.SharedUserHistory.Add(sharedUserHistory);
        }
    }
}
