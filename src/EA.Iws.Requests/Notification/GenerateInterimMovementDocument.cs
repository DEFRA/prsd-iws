namespace EA.Iws.Requests.Notification
{
    using System;
    using Core.Documents;
    using Prsd.Core.Mediator;

    public class GenerateInterimMovementDocument : IRequest<FileData>
    {
        public GenerateInterimMovementDocument(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}
