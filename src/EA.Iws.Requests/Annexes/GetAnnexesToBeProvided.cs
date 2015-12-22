namespace EA.Iws.Requests.Annexes
{
    using System;
    using Core.Annexes.ExportNotification;
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
