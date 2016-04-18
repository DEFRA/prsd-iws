namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.NotificationApplication.Annexes;

    internal class AnnexCollectionRepository : IAnnexCollectionRepository
    {
        private readonly IwsContext context;

        public AnnexCollectionRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<AnnexCollection> GetByNotificationId(Guid notificationId)
        {
            return await context.AnnexCollections.SingleAsync(a => a.NotificationId == notificationId);
        }

        public void Add(AnnexCollection annexCollection)
        {
            context.AnnexCollections.Add(annexCollection);
        }
    }
}
