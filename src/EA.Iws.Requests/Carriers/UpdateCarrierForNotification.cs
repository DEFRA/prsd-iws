namespace EA.Iws.Requests.Carriers
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Security;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class UpdateCarrierForNotification : AddCarrierToNotification
    {
        public Guid CarrierId { get; set; }
    }
}