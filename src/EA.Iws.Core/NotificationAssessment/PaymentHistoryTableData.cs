namespace EA.Iws.Core.NotificationAssessment
{
    using System;

    public class PaymentHistoryTableData
    {
        public int Transaction { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public int Type { get; set; }

        public string Receipt { get; set; }

        public string Comments { get; set; }
    }
}