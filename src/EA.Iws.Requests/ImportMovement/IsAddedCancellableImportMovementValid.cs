namespace EA.Iws.Requests.ImportMovement
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.ImportMovement;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportMovementPermissions.CanCreateImportMovements)]
    public class IsAddedCancellableImportMovementValid : IRequest<AddedCancellableImportMovementValidation>
    {
        public Guid ImportNotificationId { get; private set; }

        public int ShipmentNumber { get; private set; }

        public IsAddedCancellableImportMovementValid(Guid importNotificationId, int shipmentNumber)
        {
            ImportNotificationId = importNotificationId;
            ShipmentNumber = shipmentNumber;
        }
    }
}
