namespace EA.Iws.Requests.Facilities
{
    using System;
    using Core.Facilities;
    using Prsd.Core.Mediator;

    public class GetFacilityForNotification : IRequest<FacilityData>
    {
        public GetFacilityForNotification(Guid notificationId, Guid facilityId)
        {
            FacilityId = facilityId;
            NotificationId = notificationId;
        }

        public Guid FacilityId { get; private set; }

        public Guid NotificationId { get; private set; }
    }
}