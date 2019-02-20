namespace EA.Iws.Domain.ImportNotificationAssessment.Transactions
{
    using System;
    using Core.Shared;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class ImportNotificationTransaction : Entity
    {
        public DateTime Date { get; private set; }

        public Guid NotificationId { get; private set; }

        public PaymentMethod? PaymentMethod { get; private set; }

        public string ReceiptNumber { get; private set; }

        public string Comments { get; private set; }

        public decimal? Debit { get; private set; }

        public decimal? Credit { get; private set; }

        protected ImportNotificationTransaction()
        {
        }

        public static ImportNotificationTransaction PaymentRecord(Guid notificationId, 
            DateTime date, 
            decimal amount, 
            PaymentMethod paymentMethod, 
            string receiptNumber, 
            string comments)
        {
            if (paymentMethod == Core.Shared.PaymentMethod.Cheque)
            {
                Guard.ArgumentNotNullOrEmpty(() => receiptNumber, receiptNumber);
            }
            else
            {
                receiptNumber = "NA";
            }

            return new ImportNotificationTransaction
            {
                PaymentMethod = paymentMethod,
                NotificationId = notificationId,
                Date = date,
                ReceiptNumber = receiptNumber,
                Comments = comments,
                Credit = amount
            };
        }

        public static ImportNotificationTransaction RefundRecord(Guid notificationId,
            DateTime date, 
            decimal amount,
            string comments)
        {
            return new ImportNotificationTransaction
            {
                NotificationId = notificationId,
                Date = date,
                Debit = amount,
                Comments = comments
            };
        }

        public void UpdateComments(string comment)
        {
            this.Comments = comment;
        }
    }
}