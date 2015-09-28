namespace EA.Iws.RequestHandlers.RecoveryInfo
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication.Recovery;

    public static class IwsContextExtensions
    {
        public static async Task<RecoveryInfo> GetRecoveryInfoAsync(this IwsContext context, Guid notificationId)
        {
            return await context
                .RecoveryInfos
                .SingleOrDefaultAsync(ri =>
                    ri.NotificationId == notificationId);
        }
    }
}
