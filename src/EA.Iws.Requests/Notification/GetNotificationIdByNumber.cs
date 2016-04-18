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

        public GetNotificationIdByNumber(string notificationNumber)
        {
            NotificationNumber = notificationNumber;
        }
    }
}