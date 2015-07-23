namespace EA.Iws.Requests.Facilities
{
    using System;
    using Prsd.Core.Mediator;

    public class CopyFacilityFromImporter : IRequest<Guid>
    {
        public CopyFacilityFromImporter(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}