namespace EA.Iws.Requests.Facilities
{
    using System;
    using Authorization;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
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