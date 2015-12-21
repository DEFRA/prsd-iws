namespace EA.Iws.Requests.Notification
{
    using System;
    using Core.Notification;
    using Prsd.Core.Mediator;

    public class GetAnnexesToBeProvided : IRequest<ProvidedAnnexesData>
    {
        public Guid NotificationId { get; private set; }

        public GetAnnexesToBeProvided(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
