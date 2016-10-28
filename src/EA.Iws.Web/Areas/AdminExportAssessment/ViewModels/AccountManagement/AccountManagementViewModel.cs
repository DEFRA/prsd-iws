namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.AccountManagement
{
    using System.Collections.Generic;
    using Core.NotificationAssessment;
    using Core.Shared;
    using PaymentDetails;

    public class AccountManagementViewModel
    {
        public AccountManagementViewModel(AccountManagementData data)
        {
            TotalBillable = data.TotalBillable;
            Balance = data.Balance;
            TableData = data.PaymentHistory;
        }

        public decimal TotalBillable { get; set; }

        public decimal Balance { get; set; }

        public decimal AmountRemaining
        {
            get { return this.TotalBillable - this.Balance; }  
        }

        public IList<TransactionRecordData> TableData { get; set; }

        public PaymentDetailsViewModel PaymentViewModel { get; set; }

        public bool ShowPaymentDetails { get; set; }
    }
}