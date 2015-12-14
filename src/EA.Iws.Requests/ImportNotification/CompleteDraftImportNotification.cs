namespace EA.Iws.Requests.ImportNotification
{
    using System;
    using Prsd.Core.Mediator;

    public class CompleteDraftImportNotification : IRequest<bool>
    {
        public CompleteDraftImportNotification(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }

        public Guid ImportNotificationId { get; private set; }
    }
}