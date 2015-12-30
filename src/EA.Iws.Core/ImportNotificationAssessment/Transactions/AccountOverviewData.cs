namespace EA.Iws.Core.ImportNotificationAssessment.Transactions
{
    using System.Collections.Generic;
    using Shared;

    public class AccountOverviewData
    {
        public decimal TotalCharge { get; set; }

        public decimal TotalPaid { get; set; }

        public IList<TransactionRecordData> Transactions { get; set; }
    }
}
