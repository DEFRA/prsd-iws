namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using Shipment;

    [AutoRegister]
    public class NotificationChargeCalculator : INotificationChargeCalculator
    {
        private readonly IShipmentInfoRepository shipmentInfoRepository;
        private readonly INotificationApplicationRepository notificationApplicationRepository;
        private readonly IPricingStructureRepository pricingStructureRepository;
        private readonly IFacilityRepository facilityRepository;

        public NotificationChargeCalculator(IShipmentInfoRepository shipmentInfoRepository, 
            INotificationApplicationRepository notificationApplicationRepository,
            IPricingStructureRepository pricingStructureRepository,
            IFacilityRepository facilityRepository)
        {
            this.shipmentInfoRepository = shipmentInfoRepository;
            this.notificationApplicationRepository = notificationApplicationRepository;
            this.pricingStructureRepository = pricingStructureRepository;
            this.facilityRepository = facilityRepository;
        }
        
        public async Task<decimal> GetValue(Guid notificationId)
        {
            var shipmentInfo = await shipmentInfoRepository.GetByNotificationId(notificationId);

            if (shipmentInfo == null)
            {
                return 0;
            }

            var notification = await notificationApplicationRepository.GetById(notificationId);

            return await GetPrice(shipmentInfo, notification);
        }

        private async Task<decimal> GetPrice(ShipmentInfo shipmentInfo, NotificationApplication notification)
        {
            var facilityCollection = await facilityRepository.GetByNotificationId(notification.Id);
            bool isInterim = facilityCollection.IsInterim.HasValue ? facilityCollection.IsInterim.Value : facilityCollection.HasMultipleFacilities;

            var result = await pricingStructureRepository.GetExport(notification.CompetentAuthority, 
                notification.NotificationType, 
                shipmentInfo.NumberOfShipments, 
                isInterim);

            return result.Price;
        }
    }
}
