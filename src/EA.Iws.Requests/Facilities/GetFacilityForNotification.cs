namespace EA.Iws.Requests.Facilities
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Facilities;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
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