namespace EA.Iws.Requests.ImportNotification
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanReadImportNotification)]
    public class GetImportNotificationIdByNumber : IRequest<Guid?>
    {
        public string NotificationNumber { get; private set; }

        public GetImportNotificationIdByNumber(string notificationNumber)
        {
            NotificationNumber = notificationNumber;
        }
    }
}
