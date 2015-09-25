namespace EA.Iws.RequestHandlers.Notification
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.TransportRoute;
    using Prsd.Core.Mediator;
    using RequestHandlers.RecoveryInfo;
    using Requests.Notification;

    internal class GenerateNotificationDocumentHandler : IRequestHandler<GenerateNotificationDocument, byte[]>
    {
        private readonly IwsContext context;
        private readonly INotificationDocumentGenerator notificationDocumentGenerator;
        private readonly ITransportRouteRepository transportRouteRepository;

        public GenerateNotificationDocumentHandler(IwsContext context,
            INotificationDocumentGenerator notificationDocumentGenerator, 
            ITransportRouteRepository transportRouteRepository)
        {
            this.context = context;
            this.notificationDocumentGenerator = notificationDocumentGenerator;
            this.transportRouteRepository = transportRouteRepository;
        }

        public async Task<byte[]> HandleAsync(GenerateNotificationDocument query)
        {
            var notification = await context.GetNotificationApplication(query.NotificationId);
            var recoveryInfo = await context.GetRecoveryInfoAsync(query.NotificationId);
            var shipmentInfo = await context.GetShipmentInfoAsync(query.NotificationId);
            var transportRoute = await transportRouteRepository.GetByNotificationId(query.NotificationId);

            return notificationDocumentGenerator.GenerateNotificationDocument(notification, shipmentInfo, transportRoute, recoveryInfo);
        }
    }
}