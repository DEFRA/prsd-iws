namespace EA.Iws.Domain.ImportNotificationAssessment
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Shared;
    using ImportNotification;
    using NotificationApplication;

    public class ImportNotificationChargeCalculator : IImportNotificationChargeCalculator
    {
        private readonly IImportNotificationRepository notificationRepository;
        private readonly IShipmentRepository shipmentRepository;
        private readonly IPricingStructureRepository pricingStructureRepository;
        private readonly IFacilityRepository facilityRepository;

        public ImportNotificationChargeCalculator(IImportNotificationRepository notificationRepository, 
            IShipmentRepository shipmentRepository,
            IPricingStructureRepository pricingStructureRepository,
            IFacilityRepository facilityRepository)
        {
            this.notificationRepository = notificationRepository;
            this.shipmentRepository = shipmentRepository;
            this.pricingStructureRepository = pricingStructureRepository;
            this.facilityRepository = facilityRepository;
        }

        public async Task<decimal> GetValue(Guid importNotificationId)
        {
            var notification = await notificationRepository.Get(importNotificationId);
            var shipment = await shipmentRepository.GetByNotificationId(importNotificationId);

            return await GetPrice(notification, shipment, await GetInterimStatus(importNotificationId));
        }

        private async Task<bool> GetInterimStatus(Guid notificationId)
        {
            var facilityCollection = await facilityRepository.GetByNotificationId(notificationId);

            return facilityCollection != null && facilityCollection.Facilities != null &&
                            facilityCollection.Facilities.Skip(1).Any();
        }

        private async Task<decimal> GetPrice(ImportNotification notification, Shipment shipment, bool isInterim)
        {
             var pricingStructures = await pricingStructureRepository.Get();

            var correspondingPricingStructure =
                pricingStructures.Single(p => p.CompetentAuthority.Value == notification.CompetentAuthority.Value
                                              && p.Activity.TradeDirection == TradeDirection.Import
                                              && p.Activity.NotificationType == notification.NotificationType
                                              && p.Activity.IsInterim == isInterim
                                              && p.ShipmentQuantityRange.RangeFrom <= shipment.NumberOfShipments
                                              && (p.ShipmentQuantityRange.RangeTo == null
                                                  || p.ShipmentQuantityRange.RangeTo >= shipment.NumberOfShipments));

            return correspondingPricingStructure.Price;
        } 
    }
}