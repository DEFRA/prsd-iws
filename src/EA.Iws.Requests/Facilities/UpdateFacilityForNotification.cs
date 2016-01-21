namespace EA.Iws.Requests.Facilities
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Security;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class UpdateFacilityForNotification : AddFacilityToNotification
    {
        public Guid FacilityId { get; set; }
    }
}