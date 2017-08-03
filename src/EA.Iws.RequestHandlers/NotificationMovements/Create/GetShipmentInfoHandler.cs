namespace EA.Iws.RequestHandlers.NotificationMovements.Create
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using Core.PackagingType;
    using Core.Shared;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Shipment;
    using Domain.NotificationConsent;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.Create;
    using ShipmentInfo = Core.Movement.ShipmentInfo;

    internal class GetShipmentInfoHandler : IRequestHandler<GetShipmentInfo, ShipmentInfo>
    {
        private readonly IMapper mapper;
        private readonly INotificationConsentRepository consentRepository;
        private readonly IShipmentInfoRepository shipmentInfoRepository;
        private readonly INotificationApplicationRepository notificationRepository;

        public GetShipmentInfoHandler(INotificationConsentRepository consentRepository,
            IShipmentInfoRepository shipmentInfoRepository,
            INotificationApplicationRepository notificationRepository,
            IMapper mapper)
        {
            this.consentRepository = consentRepository;
            this.shipmentInfoRepository = shipmentInfoRepository;
            this.notificationRepository = notificationRepository;
            this.mapper = mapper;
        }

        public async Task<ShipmentInfo> HandleAsync(GetShipmentInfo message)
        {
            var consent = await consentRepository.GetByNotificationId(message.NotificationId);
            var notification = await notificationRepository.GetById(message.NotificationId);
            var shipment = await shipmentInfoRepository.GetByNotificationId(message.NotificationId);

            var units = shipment == null ? default(ShipmentQuantityUnits) : shipment.Units;

            var packagingData = mapper.Map<PackagingData>(notification.PackagingInfos
                .OrderBy(pi => pi.PackagingType)
                .ToList());

            var dates = mapper.Map<ShipmentDates>(consent.ConsentRange);

            return new ShipmentInfo
            {
                PackagingData = packagingData,
                ShipmentDates = dates,
                ShipmentQuantityUnits = units
            };
        }
    }
}