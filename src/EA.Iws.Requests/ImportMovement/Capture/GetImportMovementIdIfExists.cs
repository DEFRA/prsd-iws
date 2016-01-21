namespace EA.Iws.Requests.ImportMovement.Capture
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportMovementPermissions.CanReadImportMovements)]
    public class GetImportMovementIdIfExists : IRequest<Guid?>
    {
        public Guid NotificationId { get; private set; }

        public int Number { get; private set; }

        public GetImportMovementIdIfExists(Guid notificationId, int number)
        {
            NotificationId = notificationId;
            Number = number;
        }
    }
}
