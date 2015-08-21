namespace EA.Iws.RequestHandlers.IntendedShipments
{
    using System;
    using System.Threading.Tasks;
    using Core.Shared;
    using DataAccess;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.IntendedShipments;

    internal class SetIntendedShipmentInfoForNotificationHandler : IRequestHandler<SetIntendedShipmentInfoForNotification, Guid>
    {
        private readonly IwsContext context;
        private readonly IMap<ShipmentQuantityUnits, Domain.ShipmentQuantityUnits> shipmentQuantityUnitsMapper;

        public SetIntendedShipmentInfoForNotificationHandler(IwsContext context,
            IMap<ShipmentQuantityUnits, Domain.ShipmentQuantityUnits> shipmentQuantityUnitsMapper)
        {
            this.context = context;
            this.shipmentQuantityUnitsMapper = shipmentQuantityUnitsMapper;
        }

        public async Task<Guid> HandleAsync(SetIntendedShipmentInfoForNotification command)
        {
            var notification = await context.GetNotificationApplication(command.NotificationId);

            notification.SetShipmentInfo(command.StartDate, command.EndDate, command.NumberOfShipments,
                command.Quantity, shipmentQuantityUnitsMapper.Map(command.Units));

            await context.SaveChangesAsync();

            return notification.Id;
        }
    }
}