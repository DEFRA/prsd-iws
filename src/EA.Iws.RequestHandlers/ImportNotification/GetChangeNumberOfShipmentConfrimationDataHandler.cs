namespace EA.Iws.RequestHandlers.ImportNotification
{
    using System.Threading.Tasks;
    using Core.ImportNotification;
    using Domain.ImportNotification;
    using Domain.ImportNotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;

    internal class GetChangeNumberOfShipmentConfrimationDataHandler : IRequestHandler<GetChangeNumberOfShipmentConfrimationData, ConfirmNumberOfShipmentsChangeData>
    {
        private readonly INumberOfShipmentsHistotyRepository shipmentsHistotyRepository;
        private readonly IImportNotificationChargeCalculator notificationChargeCalculator;

        public GetChangeNumberOfShipmentConfrimationDataHandler(INumberOfShipmentsHistotyRepository shipmentsHistotyRepository, IImportNotificationChargeCalculator notificationChargeCalculator)
        {
            this.shipmentsHistotyRepository = shipmentsHistotyRepository;
            this.notificationChargeCalculator = notificationChargeCalculator;
        }

        public async Task<ConfirmNumberOfShipmentsChangeData> HandleAsync(GetChangeNumberOfShipmentConfrimationData message)
        {
            return new ConfirmNumberOfShipmentsChangeData
            {
                NotificationId = message.NotificationId,
                CurrentNumberOfShipments = await shipmentsHistotyRepository.GetCurrentNumberOfShipments(message.NotificationId),
                CurrentCharge = await notificationChargeCalculator.GetValue(message.NotificationId),
                NewCharge = await notificationChargeCalculator.GetValueForNumberOfShipments(message.NotificationId, message.NumberOfShipments)
            };
        }
    }
}
