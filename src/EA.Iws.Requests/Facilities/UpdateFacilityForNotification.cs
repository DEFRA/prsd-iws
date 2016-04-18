namespace EA.Iws.Requests.Facilities
{
    using System;
    using Authorization;
    using Core.Authorization;
    using Core.Authorization.Permissions;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class UpdateFacilityForNotification : AddFacilityToNotification
    {
        public Guid FacilityId { get; set; }
    }
}