namespace EA.Iws.RequestHandlers.Notification
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Shipment;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GetNotificationChargeHandler : IRequestHandler<GetNotificationCharge, decimal>
    {
        private readonly IwsContext context;
        private readonly NotificationChargeCalculator chargeCalculator;
        private readonly IShipmentInfoRepository shipmentInfoRepository;

        public GetNotificationChargeHandler(IwsContext context,
            NotificationChargeCalculator chargeCalculator,
            IShipmentInfoRepository shipmentInfoRepository)
        {
            this.context = context;
            this.chargeCalculator = chargeCalculator;
            this.shipmentInfoRepository = shipmentInfoRepository;
        }

        public async Task<decimal> HandleAsync(GetNotificationCharge message)
        {
            var pricingStructures = await context.PricingStructures.ToArrayAsync();
            var notification = await context.GetNotificationApplication(message.NotificationId);
            var shipmentInfo = await shipmentInfoRepository.GetByNotificationId(message.NotificationId);

            return chargeCalculator.GetValue(pricingStructures, notification, shipmentInfo);
        }
    }
}