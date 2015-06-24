namespace EA.Iws.RequestHandlers.Shipment
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Shipment;

    internal class SetShipmentInfoForNotificationHandler : IRequestHandler<SetShipmentInfoForNotification, Guid>
    {
        private readonly IwsContext db;
        private readonly IMap<ShipmentQuantityUnits, Domain.ShipmentQuantityUnits> shipmentQuantityUnitsMapper;

        public SetShipmentInfoForNotificationHandler(IwsContext db,
            IMap<ShipmentQuantityUnits, Domain.ShipmentQuantityUnits> shipmentQuantityUnitsMapper)
        {
            this.db = db;
            this.shipmentQuantityUnitsMapper = shipmentQuantityUnitsMapper;
        }

        public async Task<Guid> HandleAsync(SetShipmentInfoForNotification command)
        {
            var notification =
                await
                    db.NotificationApplications.Include(n => n.ShipmentInfo)
                        .SingleAsync(n => n.Id == command.NotificationId);

            notification.SetShipmentInfo(command.StartDate, command.EndDate, command.NumberOfShipments,
                command.Quantity, shipmentQuantityUnitsMapper.Map(command.Units));

            await db.SaveChangesAsync();

            return notification.Id;
        }
    }
}