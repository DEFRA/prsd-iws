namespace EA.Iws.Requests.WasteRecovery
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetDisposalMethod : IRequest<string>
    {
        public Guid NotificationId { get; private set; }

        public GetDisposalMethod(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
