namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.AccountManagement
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.NotificationAssessment;
    using Core.Shared;
    using PaymentDetails;
    using RefundDetails;

    public class AccountManagementViewModel
    {
        public AccountManagementViewModel()
        {
        }

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

        public RefundDetailsViewModel RefundViewModel { get; set; }

        public bool ShowRefundDetails { get; set; }

        public bool CanDeleteTransaction { get; set; }
        public string CommentError { get; set; }

        public int ErrorCommentId { get; set; }

        public bool HasPayments
        {
            get
            {
                return TableData.Count != 0 && TableData.Any(t => t.Transaction == TransactionType.Payment);
            }
        }
    }
}