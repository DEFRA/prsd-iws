namespace EA.Iws.RequestHandlers.ImportNotification
{
    using System.Threading.Tasks;
    using Core.Shipment;
    using Domain.ImportNotification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;

    internal class GetOriginalNumberOfShipmentsHandler : IRequestHandler<GetOriginalNumberOfShipments, NumberOfShipmentsHistoryData>
    {
        private readonly INumberOfShipmentsHistotyRepository repository;
        private readonly IMap<NumberOfShipmentsHistory, NumberOfShipmentsHistoryData> map;

        public GetOriginalNumberOfShipmentsHandler(INumberOfShipmentsHistotyRepository repository, IMap<NumberOfShipmentsHistory, NumberOfShipmentsHistoryData> map)
        {
            this.repository = repository;
            this.map = map;
        }

        public async Task<NumberOfShipmentsHistoryData> HandleAsync(GetOriginalNumberOfShipments message)
        {
            var result = await repository.GetOriginalNumberOfShipments(message.NotificationId);

            return map.Map(result);
        }
    }
}
