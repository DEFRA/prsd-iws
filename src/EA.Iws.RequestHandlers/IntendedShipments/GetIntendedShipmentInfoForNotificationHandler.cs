namespace EA.Iws.RequestHandlers.IntendedShipments
{
    using System.Threading.Tasks;
    using Core.IntendedShipments;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Shipment;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.IntendedShipments;

    internal class GetIntendedShipmentInfoForNotificationHandler :
        IRequestHandler<GetIntendedShipmentInfoForNotification, IntendedShipmentData>
    {
        private readonly IFacilityRepository facilityRepository;
        private readonly IShipmentInfoRepository shipmentInfoRepository;
        private readonly IMap<ShipmentInfo, IntendedShipmentData> shipmentMapper;

        public GetIntendedShipmentInfoForNotificationHandler(IFacilityRepository facilityRepository,
            IShipmentInfoRepository shipmentInfoRepository,
            IMap<ShipmentInfo, IntendedShipmentData> shipmentMapper)
        {
            this.facilityRepository = facilityRepository;
            this.shipmentMapper = shipmentMapper;
            this.shipmentInfoRepository = shipmentInfoRepository;
        }

        public async Task<IntendedShipmentData> HandleAsync(GetIntendedShipmentInfoForNotification message)
        {
            var facilityCollection = await facilityRepository.GetByNotificationId(message.NotificationId);
            var shipmentInfo = await shipmentInfoRepository.GetByNotificationId(message.NotificationId);

            var data = shipmentMapper.Map(shipmentInfo);
            data.NotificationId = message.NotificationId;
            data.IsPreconsentedRecoveryFacility = facilityCollection.AllFacilitiesPreconsented.GetValueOrDefault();

            return data;
        }
    }
}