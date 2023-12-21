namespace EA.Iws.Requests.NotificationAssessment
{
    using EA.Iws.Core.Authorization;
    using EA.Iws.Core.Authorization.Permissions;
    using EA.Iws.Core.Facilities;
    using EA.Prsd.Core.Mediator;
    using System;

    [RequestAuthorization(ExportNotificationPermissions.CanEditContactDetails)]
    public class SetFacilityDetails : IRequest<Unit>
    {
        public SetFacilityDetails(Guid notificationId, FacilityData facilityData)
        {
            NotificationId = notificationId;
            FacilityData = facilityData;
        }

        public Guid NotificationId { get; private set; }

        public FacilityData FacilityData { get; private set; }
    }
}
