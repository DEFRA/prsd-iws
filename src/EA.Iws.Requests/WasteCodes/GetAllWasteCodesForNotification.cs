namespace EA.Iws.Requests.WasteCodes
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.WasteCodes;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetAllWasteCodesForNotification : IRequest<WasteCodeData[]>
    {
        public GetAllWasteCodesForNotification(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}