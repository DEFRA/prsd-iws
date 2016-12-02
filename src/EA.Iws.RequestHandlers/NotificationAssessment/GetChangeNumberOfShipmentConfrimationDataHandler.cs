namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Shipment;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class GetChangeNumberOfShipmentConfrimationDataHandler : IRequestHandler<GetChangeNumberOfShipmentConfrimationData, ConfirmNumberOfShipmentsChangeData>
    {
        private readonly INumberOfShipmentsHistotyRepository shipmentHistotyRepository;
        private readonly INotificationChargeCalculator notificationChargeCalculator;

        public GetChangeNumberOfShipmentConfrimationDataHandler(INumberOfShipmentsHistotyRepository shipmentHistotyRepository, INotificationChargeCalculator notificationChargeCalculator)
        {
            this.shipmentHistotyRepository = shipmentHistotyRepository;
            this.notificationChargeCalculator = notificationChargeCalculator;
        }

        public async Task<ConfirmNumberOfShipmentsChangeData> HandleAsync(GetChangeNumberOfShipmentConfrimationData message)
        {
            return new ConfirmNumberOfShipmentsChangeData
            {
                NotificationId = message.NotificationId,
                CurrentNumberOfShipments = await shipmentHistotyRepository.GetCurrentNumberOfShipments(message.NotificationId),
                CurrentCharge = await notificationChargeCalculator.GetValue(message.NotificationId),
                NewCharge = await notificationChargeCalculator.GetValueForNumberOfShipments(message.NotificationId, message.NumberOfShipments)
            };
        }
    }
}
