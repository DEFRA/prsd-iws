namespace EA.Iws.Requests.WasteComponentType
{
    using EA.Iws.Core.Authorization;
    using EA.Iws.Core.Authorization.Permissions;
    using EA.Iws.Core.WasteComponentType;
    using EA.Prsd.Core.Mediator;
    using System;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetWasteComponentInfoForNotification : IRequest<WasteComponentData>
    {
        public GetWasteComponentInfoForNotification(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}
