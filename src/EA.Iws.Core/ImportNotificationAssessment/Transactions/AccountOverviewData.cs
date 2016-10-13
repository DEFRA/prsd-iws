namespace EA.Iws.Core.ImportNotificationAssessment.Transactions
{
    using System;
    using System.Collections.Generic;
    using Shared;

    public class AccountOverviewData
    {
        public decimal TotalCharge { get; set; }

        public decimal TotalPaid { get; set; }

        public DateTime? PaymentReceived { get; set; }

        public IList<TransactionRecordData> Transactions { get; set; }
    }
}
