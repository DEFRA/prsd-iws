namespace EA.Iws.Requests.ImportNotification
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Shared;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanCreateImportNotification)]
    public class CreateImportNotification : IRequest<Guid>
    {
        public string Number { get; private set; }

        public NotificationType NotificationType { get; private set; }

        public CreateImportNotification(string number, NotificationType notificationType)
        {
            Number = number;
            NotificationType = notificationType;
        }
    }
}
