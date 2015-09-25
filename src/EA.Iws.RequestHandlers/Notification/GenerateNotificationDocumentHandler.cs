namespace EA.Iws.RequestHandlers.Notification
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Prsd.Core.Mediator;
    using RequestHandlers.RecoveryInfo;
    using Requests.Notification;

    internal class GenerateNotificationDocumentHandler : IRequestHandler<GenerateNotificationDocument, byte[]>
    {
        private readonly IwsContext context;
        private readonly INotificationDocumentGenerator notificationDocumentGenerator;

        public GenerateNotificationDocumentHandler(IwsContext context,
            INotificationDocumentGenerator notificationDocumentGenerator)
        {
            this.context = context;
            this.notificationDocumentGenerator = notificationDocumentGenerator;
        }

        public async Task<byte[]> HandleAsync(GenerateNotificationDocument query)
        {
            var notification = await context.GetNotificationApplication(query.NotificationId);
            var recoveryInfo = await context.GetRecoveryInfoAsync(query.NotificationId);
            var shipmentInfo = await context.GetShipmentInfoAsync(query.NotificationId);
            var transportRoute =
                await context.TransportRoutes.SingleAsync(p => p.NotificationId == query.NotificationId);

            return notificationDocumentGenerator.GenerateNotificationDocument(notification, shipmentInfo, transportRoute, recoveryInfo);
        }
    }
}