﻿namespace EA.Iws.Requests.ImportMovement.PartialReject
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Shared;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportMovementPermissions.CanEditImportMovements)]
    public class RecordPartialRejection : IRequest<bool>
    {
        public Guid MovementId { get; private set; }

        public DateTime Date { get; private set; }

        public string Reason { get; private set; }

        public decimal ActualQuantity { get; private set; }

        public ShipmentQuantityUnits ActualUnits { get; private set; }

        public decimal RejectedQuantity { get; private set; }

        public ShipmentQuantityUnits RejectedUnits { get; private set; }

        public DateTime? WasteDisposedDate { get; private set; }

        public RecordPartialRejection(Guid movementId,
                                              DateTime date,
                                              string reason,
                                              decimal actualQuantity,
                                              ShipmentQuantityUnits actualUnits,
                                              decimal rejectedQuantity,
                                              ShipmentQuantityUnits rejectedUnits,
                                              DateTime? wasteDisposedDate)
        {
            MovementId = movementId;
            Date = date;
            Reason = reason;
            ActualQuantity = actualQuantity;
            ActualUnits = actualUnits;
            RejectedQuantity = rejectedQuantity;
            RejectedUnits = rejectedUnits;
            WasteDisposedDate = wasteDisposedDate;
        }
    }
}
