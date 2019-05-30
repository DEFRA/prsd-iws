namespace EA.Iws.Requests.ImportMovement
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Movement;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportMovementPermissions.CanEditImportMovements)]
    public class AuditImportMovement : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }

        public int ShipmentNumber { get; private set; }

        public string UserId { get; private set; }

        public MovementAuditType Type { get; private set; }

        public DateTimeOffset DateAdded { get; private set; }

        public AuditImportMovement(Guid notificationId, int shipmentNumber, string userId, MovementAuditType type,
            DateTimeOffset dateAdded)
        {
            NotificationId = notificationId;
            ShipmentNumber = shipmentNumber;
            UserId = userId;
            Type = type;
            DateAdded = dateAdded;
        }
    }
}
