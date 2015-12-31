namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.ImportNotification;

    internal class ShipmentRepository : IShipmentRepository
    {
        private readonly ImportNotificationContext context;

        public ShipmentRepository(ImportNotificationContext context)
        {
            this.context = context;
        }

        public async Task<Shipment> GetByNotificationIdOrDefault(Guid notificationId)
        {
            return await context.Shipments.SingleOrDefaultAsync(s => s.ImportNotificationId == notificationId);
        }

        public void Add(Shipment shipment)
        {
            context.Shipments.Add(shipment);
        }

        public async Task<Shipment> GetByNotificationId(Guid notificationId)
        {
            return await context.Shipments.SingleAsync(s => s.ImportNotificationId == notificationId);
        }
    }
}