namespace EA.Iws.RequestHandlers.Notification
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.NotificationApplication.Shipment;
    using Domain.TransportRoute;
    using Prsd.Core.Mediator;
    using RequestHandlers.WasteRecovery;
    using Requests.Notification;

    internal class GenerateNotificationDocumentHandler : IRequestHandler<GenerateNotificationDocument, byte[]>
    {
        private readonly INotificationDocumentGenerator notificationDocumentGenerator;
        
        public GenerateNotificationDocumentHandler(INotificationDocumentGenerator notificationDocumentGenerator)
        {
            this.notificationDocumentGenerator = notificationDocumentGenerator;
        }

        public async Task<byte[]> HandleAsync(GenerateNotificationDocument query)
        {
            return await notificationDocumentGenerator.GenerateNotificationDocument(query.NotificationId);
        }
    }
}