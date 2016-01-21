namespace EA.Iws.Requests.Producers
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Security;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class UpdateProducerForNotification : AddProducerToNotification
    {
        public Guid ProducerId { get; set; }
    }
}