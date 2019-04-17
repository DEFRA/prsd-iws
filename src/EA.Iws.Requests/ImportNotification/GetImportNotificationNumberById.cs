namespace EA.Iws.Requests.ImportNotification
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanReadImportNotification)]
    public class GetImportNotificationNumberById : IRequest<string>
    {
        public Guid NotificationId { get; private set; }

        public GetImportNotificationNumberById(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
