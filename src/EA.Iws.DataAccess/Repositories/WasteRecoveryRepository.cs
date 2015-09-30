namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.NotificationApplication.WasteRecovery;
    using Domain.Security;

    internal class WasteRecoveryRepository : IWasteRecoveryRepository
    {
        private readonly IwsContext context;
        private readonly INotificationApplicationAuthorization notificationApplicationAuthorization;

        public WasteRecoveryRepository(IwsContext context, INotificationApplicationAuthorization notificationApplicationAuthorization)
        {
            this.context = context;
            this.notificationApplicationAuthorization = notificationApplicationAuthorization;
        }

        public void Delete(WasteRecovery wasteRecovery)
        {
            context.DeleteOnCommit(wasteRecovery);
        }

        public async Task<WasteRecovery> GetByNotificationId(Guid notificationId)
        {
            await notificationApplicationAuthorization.EnsureAccessAsync(notificationId);
            return await context.WasteRecoveries.SingleOrDefaultAsync(ri => ri.NotificationId == notificationId);
        }
    }
}
