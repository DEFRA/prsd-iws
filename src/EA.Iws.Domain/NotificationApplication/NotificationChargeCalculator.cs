namespace EA.Iws.Domain.NotificationApplication
{
    using Core.ComponentRegistration;
    using Shipment;
    using System;
    using System.Threading.Tasks;

    [AutoRegister]
    public class NotificationChargeCalculator : INotificationChargeCalculator
    {
        private readonly IShipmentInfoRepository shipmentInfoRepository;
        private readonly INotificationApplicationRepository notificationApplicationRepository;
        private readonly IPriceRepository priceRepository;
        private readonly INumberOfShipmentsHistotyRepository numberOfShipmentsHistotyRepository;

        public NotificationChargeCalculator(IShipmentInfoRepository shipmentInfoRepository,
            INotificationApplicationRepository notificationApplicationRepository,
            IPriceRepository priceRepository,
            INumberOfShipmentsHistotyRepository numberOfShipmentsHistotyRepository)
        {
            this.shipmentInfoRepository = shipmentInfoRepository;
            this.notificationApplicationRepository = notificationApplicationRepository;
            this.priceRepository = priceRepository;
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
            var res = await priceRepository.GetPriceAndRefundByNotificationId(notification.Id);
            if (res == null)
            {
                throw new Exception("GetPriceAndRefundByNotificationId(" + notification.Id + ") failed, please investigate");
            }

            return res.Price;
        }
    }
}