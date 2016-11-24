namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using Core.Shipment;
    using Domain.NotificationApplication.Shipment;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class GetOriginalNumberOfShipmentsHandler : IRequestHandler<GetOriginalNumberOfShipments, ShipmentNumberHistoryData>
    {
        private readonly IShipmentNumberHistotyRepository repository;
        private readonly IMap<NumberOfShipmentsHistory, ShipmentNumberHistoryData> map; 

        public GetOriginalNumberOfShipmentsHandler(IShipmentNumberHistotyRepository repository, IMap<NumberOfShipmentsHistory, ShipmentNumberHistoryData> map)
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
