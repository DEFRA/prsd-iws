namespace EA.Iws.Requests.Annexes
{
    using System;
    using Core.Annexes.ExportNotification;
    using Prsd.Core.Mediator;

    public class GetAnnexes : IRequest<ProvidedAnnexesData>
    {
        public Guid NotificationId { get; private set; }

        public GetAnnexes(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
