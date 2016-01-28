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
        public DateTime ReceivedDate { get; private set; }

        public bool IsInterim { get; private set; }

        public string Number { get; private set; }

        public NotificationType NotificationType { get; private set; }

        public CreateImportNotification(string number, NotificationType notificationType, DateTime receivedDate, bool isInterim)
        {
            ReceivedDate = receivedDate;
            IsInterim = isInterim;
            Number = number;
            NotificationType = notificationType;
        }
    }
}
