namespace EA.Iws.Requests.WasteRecovery
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Shared;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetWasteRecoveryProvider : IRequest<ProvidedBy?>
    {
        public Guid NotificationId { get; private set; }

        public GetWasteRecoveryProvider(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
