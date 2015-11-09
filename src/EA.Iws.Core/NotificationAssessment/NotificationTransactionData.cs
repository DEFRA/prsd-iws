namespace EA.Iws.Core.NotificationAssessment
{
    using System;

    public class NotificationTransactionData
    {
        public DateTime Date { get; set; }

        public Guid NotificationId { get; set; }

        public decimal? Debit { get; set; }

        public decimal? Credit { get; set; }

        public int? PaymentMethod { get; set; }

        public string ReceiptNumber { get; set; }

        public string Comments { get; set; }
    }
}
