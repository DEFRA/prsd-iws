namespace EA.Iws.Requests.Producers
{
    using System;
    using Authorization;
    using Core.Authorization;
    using Core.Authorization.Permissions;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class UpdateProducerForNotification : AddProducerToNotification
    {
        public Guid ProducerId { get; set; }
    }
}