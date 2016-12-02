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
        private readonly INumberOfShipmentsHistotyRepository numberOfShipmentsHistotyRepository;

        public NotificationChargeCalculator(IShipmentInfoRepository shipmentInfoRepository, 
            INotificationApplicationRepository notificationApplicationRepository,
            IPricingStructureRepository pricingStructureRepository,
            IFacilityRepository facilityRepository,
            INumberOfShipmentsHistotyRepository numberOfShipmentsHistotyRepository)
        {
            this.shipmentInfoRepository = shipmentInfoRepository;
            this.notificationApplicationRepository = notificationApplicationRepository;
            this.pricingStructureRepository = pricingStructureRepository;
            this.facilityRepository = facilityRepository;
            this.numberOfShipmentsHistotyRepository = numberOfShipmentsHistotyRepository;
        }
        
        public async Task<decimal> GetValue(Guid notificationId)
        {
            var shipmentInfo = await shipmentInfoRepository.GetByNotificationId(notificationId);

            if (shipmentInfo == null)
            {
                return 0;
            }

            var numberOfShipments = shipmentInfo.NumberOfShipments;

            var largestNumberOfShipments = await numberOfShipmentsHistotyRepository.GetLargestNumberOfShipments(notificationId);

            if (largestNumberOfShipments > shipmentInfo.NumberOfShipments)
            {
                numberOfShipments = largestNumberOfShipments;
            }

            var notification = await notificationApplicationRepository.GetById(notificationId);

            return await GetPrice(numberOfShipments, notification);
        }

        public async Task<decimal> GetValueForNumberOfShipments(Guid notificationId, int numberOfShipments)
        {
            var notification = await notificationApplicationRepository.GetById(notificationId);

            return await GetPrice(numberOfShipments, notification);
        }

        private async Task<decimal> GetPrice(int numberOfShipments, NotificationApplication notification)
        {
            var facilityCollection = await facilityRepository.GetByNotificationId(notification.Id);
            bool isInterim = facilityCollection.IsInterim.HasValue ? facilityCollection.IsInterim.Value : facilityCollection.HasMultipleFacilities;

            var result = await pricingStructureRepository.GetExport(notification.CompetentAuthority, 
                notification.NotificationType,
                numberOfShipments, 
                isInterim);

            return result.Price;
        }
    }
}
