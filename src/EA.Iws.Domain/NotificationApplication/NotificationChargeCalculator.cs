namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Shared;
    using Shipment;

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

            if (notification.Charge != null)
            {
                return notification.Charge.GetValueOrDefault();
            }

            return await GetPrice(shipmentInfo, notification);
        }

        public async Task<decimal> GetCalculatedValue(Guid notificationId)
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
            var pricingStructures = await pricingStructureRepository.Get();

            bool isInterim = (await facilityRepository.GetByNotificationId(notification.Id)).IsInterim;

            var pricingStructure = pricingStructures.Single(p =>
                p.CompetentAuthority.Value == notification.CompetentAuthority.Value
                && p.Activity.TradeDirection == TradeDirection.Export
                && p.Activity.NotificationType == notification.NotificationType
                && p.Activity.IsInterim == isInterim
                && (p.ShipmentQuantityRange.RangeFrom <= shipmentInfo.NumberOfShipments
                    && (p.ShipmentQuantityRange.RangeTo == null
                    || p.ShipmentQuantityRange.RangeTo >= shipmentInfo.NumberOfShipments)));

            return pricingStructure.Price;
        }
    }
}
