namespace EA.Iws.Requests.Notification
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetNotificationIdByNumber : IRequest<Guid?>
    {
        public string NotificationNumber { get; private set; }

        public bool IsDeleteNotification { get; set; }

        public GetNotificationIdByNumber(string notificationNumber, bool isDeleteNotification = false)
        {
            NotificationNumber = notificationNumber;
            IsDeleteNotification = isDeleteNotification;
        }
    }
}