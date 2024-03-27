namespace EA.Iws.Domain.ImportNotificationAssessment
{
    using Core.ComponentRegistration;
    using ImportNotification;
    using NotificationApplication;
    using System;
    using System.Threading.Tasks;

    [AutoRegister]
    public class ImportNotificationChargeCalculator : IImportNotificationChargeCalculator
    {
        private readonly IImportNotificationRepository notificationRepository;
        private readonly IShipmentRepository shipmentRepository;
        private readonly IPriceRepository priceRepository;
        private readonly IInterimStatusRepository interimStatusRepository;
        private readonly INumberOfShipmentsHistotyRepository numberOfShipmentsHistotyRepository;

        public ImportNotificationChargeCalculator(IImportNotificationRepository notificationRepository,
            IShipmentRepository shipmentRepository,
            IPriceRepository priceRepository,
            IInterimStatusRepository interimStatusRepository,
            INumberOfShipmentsHistotyRepository numberOfShipmentsHistotyRepository)
        {
            this.notificationRepository = notificationRepository;
            this.shipmentRepository = shipmentRepository;
            this.priceRepository = priceRepository;
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

            var numberOfShipments = shipment.NumberOfShipments;

            var largestNumberOfShipments = await numberOfShipmentsHistotyRepository.GetLargestNumberOfShipments(importNotificationId);

            if (largestNumberOfShipments > shipment.NumberOfShipments)
            {
                numberOfShipments = largestNumberOfShipments;
            }

            var notification = await notificationRepository.Get(importNotificationId);

            return await GetPrice(notification, numberOfShipments, await GetInterimStatus(importNotificationId));
        }

        public async Task<decimal> GetValueForNumberOfShipments(Guid importNotificationId, int numberOfShipments)
        {
            var notification = await notificationRepository.Get(importNotificationId);

            return await GetPrice(notification, numberOfShipments, await GetInterimStatus(importNotificationId));
        }

        private async Task<bool> GetInterimStatus(Guid notificationId)
        {
            var interimStatus = await interimStatusRepository.GetByNotificationId(notificationId);

            return interimStatus.IsInterim;
        }

        private async Task<decimal> GetPrice(ImportNotification notification, int numberOfShipments, bool isInterim)
        {
            var res = await priceRepository.GetPriceAndRefundByNotificationId(notification.Id);
            if (res == null)
            {
                throw new Exception("GetPriceAndRefundByNotificationId(" + notification.Id + ") failed, please investigate");
            }

            return res.Price;
        }
    }
}