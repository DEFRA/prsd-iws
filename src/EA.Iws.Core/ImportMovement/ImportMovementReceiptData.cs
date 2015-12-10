namespace EA.Iws.Core.ImportMovement
{
    using System;
    using System.Collections.Generic;
    using Shared;

    public class ImportMovementReceiptData
    {
        public DateTimeOffset? ReceiptDate { get; set; }

        public decimal? ActualQuantity { get; set; }

        public ShipmentQuantityUnits? ReceiptUnits { get; set; }

        public IList<ShipmentQuantityUnits> PossibleUnits { get; set; }

        public string RejectionReason { get; set; }

        public string RejectionReasonFurtherInformation { get; set; }

        public bool IsReceived { get; set; }
    }
}
