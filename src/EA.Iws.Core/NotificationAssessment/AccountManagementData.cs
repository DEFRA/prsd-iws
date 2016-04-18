namespace EA.Iws.Core.NotificationAssessment
{
    using System.Collections.Generic;
    using Shared;

    public class AccountManagementData
    {
        public decimal TotalBillable { get; set; }

        public decimal Balance { get; set; }

        public IList<TransactionRecordData> PaymentHistory { get; set; } 
    }
}
