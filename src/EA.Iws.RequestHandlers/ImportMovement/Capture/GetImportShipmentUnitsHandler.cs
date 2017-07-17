namespace EA.Iws.RequestHandlers.ImportMovement.Capture
{
    using System.Threading.Tasks;
    using Core.Shared;
    using Domain.ImportNotification;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement.Capture;

    internal class GetImportShipmentUnitsHandler : IRequestHandler<GetImportShipmentUnits, ShipmentQuantityUnits>
    {
        private readonly IShipmentRepository repository;
        public GetImportShipmentUnitsHandler(IShipmentRepository repository)
        {
            this.repository = repository;
        }

        public async Task<ShipmentQuantityUnits> HandleAsync(GetImportShipmentUnits message)
        {
            var shipment = await repository.GetByNotificationId(message.NotificationId);

            return shipment == null ? default(ShipmentQuantityUnits) : shipment.Quantity.Units;
        }
    }
}
