namespace EA.Iws.Requests.Movement.PartialReject
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Shared;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanEditExportMovementsInternal)]
    public class RecordPartialRejectionInternal : IRequest<bool>
    {
        public Guid MovementId { get; private set; }

        public DateTime Date { get; private set; }

        public string Reason { get; private set; }

       public decimal ActualQuantity { get; private set; }

        public ShipmentQuantityUnits ActualUnits { get; private set; }

        public decimal RejectedQuantity { get; private set; }

        public ShipmentQuantityUnits RejectedUnits { get; private set; }

        public RecordPartialRejectionInternal(Guid movementId, 
                                              DateTime receivedDate,
                                              string reason,
                                              decimal actualQuantity, 
                                              ShipmentQuantityUnits actualUnits,
                                              decimal rejectedQuantity,
                                              ShipmentQuantityUnits rejectedUnits)
        {
            MovementId = movementId;
            Date = receivedDate;
            Reason = reason;
            ActualQuantity = actualQuantity;
            ActualUnits = actualUnits;
            RejectedQuantity = rejectedQuantity;
            RejectedUnits = rejectedUnits;
        }
    }
}
