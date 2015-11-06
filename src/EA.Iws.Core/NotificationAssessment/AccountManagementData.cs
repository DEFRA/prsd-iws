namespace EA.Iws.Core.NotificationAssessment
{
    using System.Collections.Generic;

    public class AccountManagementData
    {
        public decimal TotalBillable { get; set; }

        public decimal Balance { get; set; }

        public IList<PaymentHistoryTableData> PaymentHistory { get; set; } 
    }
}
