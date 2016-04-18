namespace EA.Iws.Requests.ImportNotification
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanReadImportNotification)]
    public class CheckImportNumberUnique : IRequest<bool>
    {
        public string NotificationNumber { get; private set; }

        public CheckImportNumberUnique(string notificationNumber)
        {
            NotificationNumber = notificationNumber;
        }
    }
}
