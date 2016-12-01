namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication.Shipment;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class SetNewNumberOfShipmentsHandler : IRequestHandler<SetNewNumberOfShipments, bool>
    {
        private readonly IwsContext context;
        private readonly INumberOfShipmentsHistotyRepository shipmentHistotyRepository;
        private readonly IShipmentInfoRepository shipmentInfoRepository;

        public SetNewNumberOfShipmentsHandler(IwsContext context, INumberOfShipmentsHistotyRepository shipmentHistotyRepository, IShipmentInfoRepository shipmentInfoRepository)
        {
            this.context = context;
            this.shipmentHistotyRepository = shipmentHistotyRepository;
            this.shipmentInfoRepository = shipmentInfoRepository;
        }

        public async Task<bool> HandleAsync(SetNewNumberOfShipments message)
        {
            var shipmentInfo = await shipmentInfoRepository.GetByNotificationId(message.NotificationId);
            shipmentInfo.UpdateNumberOfShipments(message.NewNumberOfShipments);
            
            shipmentHistotyRepository.Add(new NumberOfShipmentsHistory(message.NotificationId, message.OldNumberOfShipments, DateTime.UtcNow));

            await context.SaveChangesAsync();
            
            return true;
        }
    }
}
