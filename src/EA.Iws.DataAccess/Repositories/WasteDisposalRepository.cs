namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.NotificationApplication.WasteRecovery;
    using Domain.Security;

    internal class WasteDisposalRepository : IWasteDisposalRepository
    {
        private readonly IwsContext context;
        private readonly INotificationApplicationAuthorization notificationApplicationAuthorization;

        public WasteDisposalRepository(IwsContext context, INotificationApplicationAuthorization notificationApplicationAuthorization)
        {
            this.context = context;
            this.notificationApplicationAuthorization = notificationApplicationAuthorization;
        }

        public async Task<WasteDisposal> GetByNotificationId(Guid notificationId)
        {
            await notificationApplicationAuthorization.EnsureAccessAsync(notificationId);
            return await context.WasteDisposals.SingleOrDefaultAsync(i => i.NotificationId == notificationId);
        }

        public void Delete(WasteDisposal wasteDisposal)
        {
            context.DeleteOnCommit(wasteDisposal);
        }
    }
}
