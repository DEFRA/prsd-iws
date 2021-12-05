namespace EA.Iws.Requests.Movement.Reject
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using EA.Iws.Core.Shared;
    using Prsd.Core;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanEditExportMovementsInternal)]
    public class RecordRejectionInternal : IRequest<bool>
    {
        public Guid MovementId { get; private set; }

        public string RejectionReason { get; private set; }

        public DateTime RejectedDate { get; private set; }

        public decimal? RejectedQuantity { get; set; }

        public ShipmentQuantityUnits? RejectedUnits { get; set; }

        public RecordRejectionInternal(Guid movementId, 
            DateTime rejectedDate, 
            string rejectionReason,
            decimal? rejectedQuantity,
            ShipmentQuantityUnits? units)
        {
            Guard.ArgumentNotNullOrEmpty(() => rejectionReason, rejectionReason);

            MovementId = movementId;
            RejectedDate = rejectedDate;
            RejectionReason = rejectionReason;
            RejectedQuantity = rejectedQuantity;
            RejectedUnits = units;
        }
    }
}