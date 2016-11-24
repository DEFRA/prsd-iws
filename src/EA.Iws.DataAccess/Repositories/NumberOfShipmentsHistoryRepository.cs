namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.NotificationApplication.Shipment;
    using Domain.Security;

    internal class NumberOfShipmentsHistoryRepository : INumberOfShipmentsHistotyRepository
    {
        private readonly IwsContext context;
        private readonly INotificationApplicationAuthorization notificationApplicationAuthorization;

        public NumberOfShipmentsHistoryRepository(IwsContext context, INotificationApplicationAuthorization notificationApplicationAuthorization)
        {
            this.context = context;
            this.notificationApplicationAuthorization = notificationApplicationAuthorization;
        }

        public async Task<NumberOfShipmentsHistory> GetOriginalNumberOfShipments(Guid notificationId)
        {
            await notificationApplicationAuthorization.EnsureAccessAsync(notificationId);

            return await context.NumberOfShipmentsHistories.Where(x => x.NotificationId == notificationId).OrderBy(x => x.DateChanged).FirstOrDefaultAsync();
        }
    }
}
