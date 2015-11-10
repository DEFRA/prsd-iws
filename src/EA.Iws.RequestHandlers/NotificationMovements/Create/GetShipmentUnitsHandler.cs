namespace EA.Iws.RequestHandlers.NotificationMovements.Create
{
    using System.Threading.Tasks;
    using Core.Shared;
    using Domain.NotificationApplication.Shipment;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.Create;

    internal class GetShipmentUnitsHandler : IRequestHandler<GetShipmentUnits, ShipmentQuantityUnits>
    {
        private readonly IShipmentInfoRepository repository;

        public GetShipmentUnitsHandler(IShipmentInfoRepository repository)
        {
            this.repository = repository;
        }

        public async Task<ShipmentQuantityUnits> HandleAsync(GetShipmentUnits message)
        {
            var shipment = await repository.GetByNotificationId(message.NotificationId);

            return shipment.Units;
        }
    }
}