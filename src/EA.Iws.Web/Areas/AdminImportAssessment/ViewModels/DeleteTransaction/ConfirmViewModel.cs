namespace EA.Iws.Web.Areas.AdminImportAssessment.ViewModels.DeleteTransaction
{
    using System;
    using Core.NotificationAssessment;
    using Core.Shared;

    public class ConfirmViewModel
    {
        public Guid NotificationId { get; set; }

        public Guid TransactionId { get; set; }

        public TransactionType Transaction { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public PaymentMethod? Type { get; set; }

        public string ReceiptNumber { get; set; }

        public string Comments { get; set; }

        public ConfirmViewModel(Guid notificationId, TransactionRecordData transaction)
        {
            NotificationId = notificationId;
            TransactionId = transaction.TransactionId;
            Transaction = transaction.Transaction;
            Date = transaction.Date;
            Amount = transaction.Amount;
            Type = transaction.Type;
            ReceiptNumber = transaction.ReceiptNumber;
            Comments = transaction.Comments;
        }
    }
}