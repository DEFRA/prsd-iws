namespace EA.Iws.Domain.ImportNotificationAssessment
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using Core.Shared;
    using ImportNotification;
    using NotificationApplication;

    [AutoRegister]
    public class ImportNotificationChargeCalculator : IImportNotificationChargeCalculator
    {
        private readonly IImportNotificationRepository notificationRepository;
        private readonly IShipmentRepository shipmentRepository;
        private readonly IPricingStructureRepository pricingStructureRepository;
        private readonly IInterimStatusRepository interimStatusRepository;

        public ImportNotificationChargeCalculator(IImportNotificationRepository notificationRepository, 
            IShipmentRepository shipmentRepository,
            IPricingStructureRepository pricingStructureRepository,
            IInterimStatusRepository interimStatusRepository)
        {
            this.notificationRepository = notificationRepository;
            this.shipmentRepository = shipmentRepository;
            this.pricingStructureRepository = pricingStructureRepository;
            this.interimStatusRepository = interimStatusRepository;
        }

        public async Task<decimal> GetValue(Guid importNotificationId)
        {
            var notification = await notificationRepository.Get(importNotificationId);
            var shipment = await shipmentRepository.GetByNotificationIdOrDefault(importNotificationId);

            if (shipment == null)
            {
                return 0;
            }

            return await GetPrice(notification, shipment, await GetInterimStatus(importNotificationId));
        }

        private async Task<bool> GetInterimStatus(Guid notificationId)
        {
            var interimStatus = await interimStatusRepository.GetByNotificationId(notificationId);

            return interimStatus.IsInterim;
        }

        private async Task<decimal> GetPrice(ImportNotification notification, Shipment shipment, bool isInterim)
        {
             var pricingStructures = await pricingStructureRepository.Get();

            var correspondingPricingStructure =
                pricingStructures.Single(p => p.CompetentAuthority == notification.CompetentAuthority
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