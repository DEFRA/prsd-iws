namespace EA.Iws.Requests.Movement
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Movement;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanCreateExportMovements)]
    public class IsAddedCancellableMovementValid : IRequest<AddedCancellableMovementValidation>
    {
        public Guid NotificationId { get; private set; }

        public int ShipmentNumber { get; private set; }

        public IsAddedCancellableMovementValid(Guid notificationId, int shipmentNumber)
        {
            NotificationId = notificationId;
            ShipmentNumber = shipmentNumber;
        }
    }
}
