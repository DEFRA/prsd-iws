namespace EA.Iws.RequestHandlers.Shipment
{
    using System;
    using System.Threading.Tasks;
    using Core.Shipment;
    using DataAccess;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Shipment;

    internal class SetShipmentInfoForNotificationHandler : IRequestHandler<SetShipmentInfoForNotification, Guid>
    {
        private readonly IwsContext context;
        private readonly IMap<ShipmentQuantityUnits, Domain.ShipmentQuantityUnits> shipmentQuantityUnitsMapper;

        public SetShipmentInfoForNotificationHandler(IwsContext context,
            IMap<ShipmentQuantityUnits, Domain.ShipmentQuantityUnits> shipmentQuantityUnitsMapper)
        {
            this.context = context;
            this.shipmentQuantityUnitsMapper = shipmentQuantityUnitsMapper;
        }

        public async Task<Guid> HandleAsync(SetShipmentInfoForNotification command)
        {
            var notification = await context.GetNotificationApplication(command.NotificationId);

            notification.SetShipmentInfo(command.StartDate, command.EndDate, command.NumberOfShipments,
                command.Quantity, shipmentQuantityUnitsMapper.Map(command.Units));

            await context.SaveChangesAsync();

            return notification.Id;
        }
    }
}