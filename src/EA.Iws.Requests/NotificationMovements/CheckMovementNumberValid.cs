namespace EA.Iws.Requests.NotificationMovements
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanReadExportMovements)]
    public class CheckMovementNumberValid : IRequest<MovementNumberStatus>
    {
        public Guid NotificationId { get; private set; }

        public int Number { get; private set; }

        public CheckMovementNumberValid(Guid notificationId, int number)
        {
            NotificationId = notificationId;
            Number = number;
        }
    }
}
