namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.NotificationApplication.Shipment;
    using Domain.Security;

    internal class ShipmentNumberHistoryRepository : IShipmentNumberHistotyRepository
    {
        private readonly IwsContext context;
        private readonly INotificationApplicationAuthorization notificationApplicationAuthorization;

        public ShipmentNumberHistoryRepository(IwsContext context, INotificationApplicationAuthorization notificationApplicationAuthorization)
        {
            this.context = context;
            this.notificationApplicationAuthorization = notificationApplicationAuthorization;
        }

        public async Task<ShipmentNumberHistory> GetOriginalNumberOfShipments(Guid notificationId)
        {
            await notificationApplicationAuthorization.EnsureAccessAsync(notificationId);

            return await context.ShipmentNumberHistories.Where(x => x.NotificationId == notificationId).OrderBy(x => x.DateChanged).FirstAsync();
        }
    }
}
