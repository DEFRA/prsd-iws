namespace EA.Iws.Requests.ImportNotification
{
    using System;
    using Core.ImportNotification;
    using Prsd.Core.Mediator;

    public class GetNotificationDetails : IRequest<NotificationDetails>
    {
        public GetNotificationDetails(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }

        public Guid ImportNotificationId { get; private set; }
    }
}