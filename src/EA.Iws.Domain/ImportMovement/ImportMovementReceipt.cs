﻿namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using Core.Shared;
    using Prsd.Core.Domain;

    public class ImportMovementReceipt : Entity
    {
        public Guid MovementId { get; private set; }

        public DateTime Date { get; private set; }

        public decimal Quantity { get; private set; }

        public ShipmentQuantityUnits Unit { get; private set; }

        protected ImportMovementReceipt()
        {
        }

        public ImportMovementReceipt(Guid movementId, ShipmentQuantity quantity, DateTime date)
        {
            MovementId = movementId;
            Quantity = quantity.Quantity;
            Unit = quantity.Units;
            Date = date;
        }
    }
}
