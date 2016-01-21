namespace EA.Iws.Requests.NotificationMovements.Capture
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanCreateExportMovementsInternal)]
    public class GetMovementIdIfExists : IRequest<Guid?>
    {
        public Guid NotificationId { get; private set; }

        public int Number { get; private set; }

        public GetMovementIdIfExists(Guid notificationId, int number)
        {
            NotificationId = notificationId;
            Number = number;
        }
    }
}
