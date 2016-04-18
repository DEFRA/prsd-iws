namespace EA.Iws.Requests.WasteRecovery
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Shared;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class SetWasteRecoveryProvider : IRequest<bool>
    {
        public ProvidedBy ProvidedBy { get; private set; }
        public Guid NotificationId { get; private set; }

        public SetWasteRecoveryProvider(ProvidedBy providedBy, Guid notificationId)
        {
            ProvidedBy = providedBy;
            NotificationId = notificationId;
        }
    }
}
