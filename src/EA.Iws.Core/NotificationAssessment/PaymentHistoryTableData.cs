namespace EA.Iws.Core.NotificationAssessment
{
    using System;
    using Shared;

    public class PaymentHistoryTableData
    {
        public int Transaction { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public PaymentMethod? Type { get; set; }

        public string Receipt { get; set; }

        public string Comments { get; set; }
    }
}