namespace EA.Iws.RequestHandlers.Notification
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Prsd.Core.Mediator;
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

            return notificationDocumentGenerator.GenerateNotificationDocument(notification);
        }
    }
}