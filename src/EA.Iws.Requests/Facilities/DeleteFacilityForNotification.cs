namespace EA.Iws.Requests.Facilities
{
    using System;
    using Prsd.Core.Mediator;
    using Security;

    [NotificationReadOnlyAuthorize]
    public class DeleteFacilityForNotification : IRequest<bool>
    {
        public DeleteFacilityForNotification(Guid notificationId, Guid facilityId)
        {
            NotificationId = notificationId;
            FacilityId = facilityId;
        }

        public Guid NotificationId { get; private set; }

        public Guid FacilityId { get; private set; }
    }
}