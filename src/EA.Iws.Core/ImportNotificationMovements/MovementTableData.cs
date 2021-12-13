﻿namespace EA.Iws.Core.ImportNotificationMovements
{
    using System;
    using Shared;

    public class MovementTableData
    {
        public Guid Id { get; set; }

        public int Number { get; set; }

        public DateTime? PreNotification { get; set; }

        public DateTime? ShipmentDate { get; set; }

        public DateTime? Received { get; set; }

        public decimal? Quantity { get; set; }

        public ShipmentQuantityUnits? Unit { get; set; }

        public DateTime? Rejected { get; set; }

        public DateTime? RecoveredOrDisposedOf { get; set; }

        public bool IsCancelled { get; set; }

        public bool IsPartialRejected { get; set; }
    }
}
