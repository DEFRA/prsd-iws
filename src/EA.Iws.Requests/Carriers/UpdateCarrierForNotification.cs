namespace EA.Iws.Requests.Carriers
{
    using System;
    using Authorization;
    using Core.Authorization;
    using Core.Authorization.Permissions;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class UpdateCarrierForNotification : AddCarrierToNotification
    {
        public Guid CarrierId { get; set; }
    }
}