namespace EA.Iws.Requests.MovementReceipt
{
    using System;
    using Core.MovementReceipt;

    public class MovementAcceptanceData
    {
        public Guid MovementId { get; set; }
        public Decision? Decision { get; set; }
        public string RejectionReason { get; set; }
    }
}
