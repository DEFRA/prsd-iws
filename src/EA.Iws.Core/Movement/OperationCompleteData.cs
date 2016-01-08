namespace EA.Iws.Core.Movement
{
    using System;
    using Shared;

    public class OperationCompleteData
    {
        public NotificationType NotificationType { get; set; }

        public DateTime MovementDate { get; set; }

        public DateTime ReceiptDate { get; set; }
    }
}
