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
        private readonly INumberOfShipmentsHistotyRepository numberOfShipmentsHistotyRepository;

        public ImportNotificationChargeCalculator(IImportNotificationRepository notificationRepository, 
            IShipmentRepository shipmentRepository,
            IPricingStructureRepository pricingStructureRepository,
            IInterimStatusRepository interimStatusRepository,
            INumberOfShipmentsHistotyRepository numberOfShipmentsHistotyRepository)
        {
            this.notificationRepository = notificationRepository;
            this.shipmentRepository = shipmentRepository;
            this.pricingStructureRepository = pricingStructureRepository;
            this.interimStatusRepository = interimStatusRepository;
            this.numberOfShipmentsHistotyRepository = numberOfShipmentsHistotyRepository;
        }

        public async Task<decimal> GetValue(Guid importNotificationId)
        {
            var shipment = await shipmentRepository.GetByNotificationIdOrDefault(importNotificationId);

            if (shipment == null)
            {
                return 0;
            }

            var largestNumberOfShipments = await numberOfShipmentsHistotyRepository.GetLargestNumberOfShipments(importNotificationId);

            if (largestNumberOfShipments > shipment.NumberOfShipments)
            {
                shipment.UpdateNumberOfShipments(largestNumberOfShipments);
            }

            var notification = await notificationRepository.Get(importNotificationId);

            return await GetPrice(notification, shipment, await GetInterimStatus(importNotificationId));
        }

        public async Task<decimal> GetValueForNumberOfShipments(Guid importNotificationId, int numberOfShipments)
        {
            var notification = await notificationRepository.Get(importNotificationId);
            var shipment = await shipmentRepository.GetByNotificationIdOrDefault(importNotificationId);

            if (shipment == null)
            {
                return 0;
            }

            shipment.UpdateNumberOfShipments(numberOfShipments);

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