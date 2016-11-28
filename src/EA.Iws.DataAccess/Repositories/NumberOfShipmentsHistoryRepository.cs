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

            return await context.NumberOfShipmentsHistories.Where(x => x.NotificationId == notificationId)
                        .OrderBy(x => x.DateChanged)
                        .FirstOrDefaultAsync();
        }

        public async Task<int> GetCurrentNumberOfShipments(Guid notificationId)
        {
            await notificationApplicationAuthorization.EnsureAccessAsync(notificationId);

            var shipmentInfo = await context.ShipmentInfos.Where(x => x.NotificationId == notificationId).SingleOrDefaultAsync();

            return shipmentInfo.NumberOfShipments;
        }

        public async Task<int> GetLargestNumberOfShipments(Guid notificationId)
        {
            await notificationApplicationAuthorization.EnsureAccessAsync(notificationId);

            var currentNumber = await GetCurrentNumberOfShipments(notificationId);

            var largestInHistory = await context.NumberOfShipmentsHistories.Where(x => x.NotificationId == notificationId)
                                        .OrderByDescending(x => x.NumberOfShipments)
                                        .FirstOrDefaultAsync();

            if (largestInHistory != null && largestInHistory.NumberOfShipments > currentNumber)
            {
                return largestInHistory.NumberOfShipments;
            }

            return currentNumber;
        }
    }
}
