namespace EA.Iws.RequestHandlers.ImportNotification
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportNotification;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;

    internal class SetNewNumberOfShipmentsHandler : IRequestHandler<SetNewNumberOfShipments, bool>
    {
        private readonly ImportNotificationContext context;
        private readonly INumberOfShipmentsHistotyRepository shipmentHistotyRepository;
        private readonly IShipmentRepository shipmentRepository;
        
        public SetNewNumberOfShipmentsHandler(ImportNotificationContext context, INumberOfShipmentsHistotyRepository shipmentHistotyRepository, IShipmentRepository shipmentRepository)
        {
            this.context = context;
            this.shipmentHistotyRepository = shipmentHistotyRepository;
            this.shipmentRepository = shipmentRepository;
        }

        public async Task<bool> HandleAsync(SetNewNumberOfShipments message)
        {
            var shipment = await shipmentRepository.GetByNotificationId(message.NotificationId);
            shipment.UpdateNumberOfShipments(message.NewNumberOfShipments);

            shipmentHistotyRepository.Add(new NumberOfShipmentsHistory(message.NotificationId, message.OldNumberOfShipments, DateTime.UtcNow));

            await context.SaveChangesAsync();

            return true;
        }
    }
}
