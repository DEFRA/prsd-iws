namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.ImportNotification;
    using Domain.Security;

    internal class ShipmentRepository : IShipmentRepository
    {
        private readonly ImportNotificationContext context;
        private readonly IImportNotificationApplicationAuthorization authorization;

        public ShipmentRepository(ImportNotificationContext context, IImportNotificationApplicationAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public async Task<Shipment> GetByNotificationIdOrDefault(Guid notificationId)
        {
            var shipment = await context.Shipments.SingleOrDefaultAsync(s => s.ImportNotificationId == notificationId);
            if (shipment != null)
            {
                await authorization.EnsureAccessAsync(notificationId);
            }
            return shipment;
        }

        public void Add(Shipment shipment)
        {
            context.Shipments.Add(shipment);
        }

        public async Task<Shipment> GetByNotificationId(Guid notificationId)
        {
            await authorization.EnsureAccessAsync(notificationId);
            return await context.Shipments.SingleAsync(s => s.ImportNotificationId == notificationId);
        }
    }
}