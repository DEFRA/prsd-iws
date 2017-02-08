namespace EA.Iws.Core.Movement
{
    using System;

    public class MovementFileData
    {
        public int ShipmentNumber { get; set; }

        public Guid? PrenotificationFileId { get; set; }

        public Guid? ReceiptFileId { get; set; }

        public Guid? OperationReceiptFileId { get; set; }
    }
}