namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.ImportNotification;
    using Domain.Security;

    internal class ShipmentNumberHistoryRepository : IShipmentNumberHistotyRepository
    {
        private readonly ImportNotificationContext context;
        private readonly IImportNotificationApplicationAuthorization authorization;

        public ShipmentNumberHistoryRepository(ImportNotificationContext context, IImportNotificationApplicationAuthorization authorization)
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
    }
}
