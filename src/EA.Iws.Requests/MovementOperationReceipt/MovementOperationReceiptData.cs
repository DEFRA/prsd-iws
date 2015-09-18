namespace EA.Iws.Requests.MovementOperationReceipt
{
    using System;
    using Core.Shared;

    public class MovementOperationReceiptData
    {
        public NotificationType NotificationType { get; set; }
        public DateTime? DateCompleted { get; set; }
    }
}
