namespace EA.Iws.RequestHandlers.IntendedShipments
{
    using System.Threading.Tasks;
    using Core.IntendedShipments;
    using Domain.NotificationApplication.Shipment;
    using Prsd.Core.Mediator;
    using Requests.IntendedShipments;

    internal class GetIntendedShipmentInfoForNotificationHandler :
        IRequestHandler<GetIntendedShipmentInfoForNotification, IntendedShipmentData>
    {
        private readonly IShipmentInfoRepository shipmentInfoRepository;

        public GetIntendedShipmentInfoForNotificationHandler(IShipmentInfoRepository shipmentInfoRepository)
        {
            this.shipmentInfoRepository = shipmentInfoRepository;
        }

        public async Task<IntendedShipmentData> HandleAsync(GetIntendedShipmentInfoForNotification message)
        {
            return await shipmentInfoRepository.GetIntendedShipmentDataByNotificationId(message.NotificationId);
        }
    }
}