namespace EA.Iws.Core.Shared
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using NotificationAssessment;

    public class TransactionRecordData
    {
        public TransactionType Transaction { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public PaymentMethod? Type { get; set; }

        public string ReceiptNumber { get; set; }

        public string Comments { get; set; }

        public Guid TransactionId { get; set; }
    }
}