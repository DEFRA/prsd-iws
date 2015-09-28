namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.NotificationApplication.Recovery;
    using Domain.Security;

    internal class RecoveryInfoRepository : IRecoveryInfoRepository
    {
        private readonly IwsContext context;
        private readonly INotificationApplicationAuthorization notificationApplicationAuthorization;

        public RecoveryInfoRepository(IwsContext context, INotificationApplicationAuthorization notificationApplicationAuthorization)
        {
            this.context = context;
            this.notificationApplicationAuthorization = notificationApplicationAuthorization;
        }

        public async Task<RecoveryInfo> GetByNotificationId(Guid notificationId)
        {
            await notificationApplicationAuthorization.EnsureAccessAsync(notificationId);
            return await context.RecoveryInfos.SingleOrDefaultAsync(ri => ri.NotificationId == notificationId);
        }
    }
}
