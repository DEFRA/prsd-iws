namespace EA.Iws.Requests.Movement.Reject
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using EA.Iws.Core.Shared;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanEditExportMovementsExternal)]
    public class SetMovementRejected : IRequest<Guid>
    {
        public SetMovementRejected(Guid movementId, DateTime dateReceived, string reason, byte[] fileBytes,
            string fileType, decimal? quantity, ShipmentQuantityUnits? unit)
        {
            FileBytes = fileBytes;
            FileType = fileType;
            MovementId = movementId;
            DateReceived = dateReceived;
            Reason = reason;
            Quantity = quantity;
            Unit = unit;
        }

        public byte[] FileBytes { get; private set; }

        public string FileType { get; private set; }

        public Guid MovementId { get; private set; }

        public DateTime DateReceived { get; private set; }

        public string Reason { get; private set; }

        public decimal? Quantity { get; set; }

        public ShipmentQuantityUnits? Unit { get; set; }
    }
}