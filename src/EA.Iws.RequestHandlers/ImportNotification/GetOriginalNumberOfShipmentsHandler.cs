namespace EA.Iws.RequestHandlers.ImportNotification
{
    using System.Threading.Tasks;
    using Core.Shipment;
    using Domain.ImportNotification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;

    internal class GetOriginalNumberOfShipmentsHandler : IRequestHandler<GetOriginalNumberOfShipments, ShipmentNumberHistoryData>
    {
        private readonly IShipmentNumberHistotyRepository repository;
        private readonly IMap<ShipmentNumberHistory, ShipmentNumberHistoryData> map;

        public GetOriginalNumberOfShipmentsHandler(IShipmentNumberHistotyRepository repository, IMap<ShipmentNumberHistory, ShipmentNumberHistoryData> map)
        {
            this.repository = repository;
            this.map = map;
        }

        public async Task<ShipmentNumberHistoryData> HandleAsync(GetOriginalNumberOfShipments message)
        {
            var result = await repository.GetOriginalNumberOfShipments(message.NotificationId);

            return map.Map(result);
        }
    }
}
