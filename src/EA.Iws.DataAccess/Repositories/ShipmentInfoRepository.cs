namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.NotificationApplication.Shipment;
    using Domain.Security;

    internal class ShipmentInfoRepository : IShipmentInfoRepository
    {
        private readonly IwsContext context;
        private readonly INotificationApplicationAuthorization notificationApplicationAuthorization;

        public ShipmentInfoRepository(IwsContext context, INotificationApplicationAuthorization notificationApplicationAuthorization)
        {
            this.context = context;
            this.notificationApplicationAuthorization = notificationApplicationAuthorization;
        }

        public async Task<ShipmentInfo> GetByNotificationId(Guid notificationId)
        {
            await notificationApplicationAuthorization.EnsureAccessAsync(notificationId);
            return await context.ShipmentInfos.SingleOrDefaultAsync(si => si.NotificationId == notificationId);
        }
    }
}
