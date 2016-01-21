namespace EA.Iws.Requests.WasteRecovery
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetRecoverablePercentage : IRequest<decimal?>
    {
        public Guid NotificationId { get; private set; }

        public GetRecoverablePercentage(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
