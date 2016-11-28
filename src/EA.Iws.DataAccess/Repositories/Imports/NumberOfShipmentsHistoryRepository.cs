namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.ImportNotification;
    using Domain.Security;

    internal class NumberOfShipmentsHistoryRepository : INumberOfShipmentsHistotyRepository
    {
        private readonly ImportNotificationContext context;
        private readonly IImportNotificationApplicationAuthorization authorization;

        public NumberOfShipmentsHistoryRepository(ImportNotificationContext context, IImportNotificationApplicationAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public async Task<NumberOfShipmentsHistory> GetOriginalNumberOfShipments(Guid notificationId)
        {
            await authorization.EnsureAccessAsync(notificationId);

            return await context.NumberOfShipmentsHistories.Where(x => x.ImportNotificationId == notificationId)
                        .OrderBy(x => x.DateChanged)
                        .FirstOrDefaultAsync();
        }

        public async Task<int> GetCurrentNumberOfShipments(Guid notificationId)
        {
            await authorization.EnsureAccessAsync(notificationId);

            var shipmentInfo = await context.Shipments.Where(n => n.ImportNotificationId == notificationId).SingleOrDefaultAsync();

            return shipmentInfo.NumberOfShipments;
        }

        public async Task<int> GetLargestNumberOfShipments(Guid notificationId)
        {
            await authorization.EnsureAccessAsync(notificationId);

            var currentNumber = await GetCurrentNumberOfShipments(notificationId);

            var largestInHistory = await context.NumberOfShipmentsHistories.Where(n => n.ImportNotificationId == notificationId)
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
