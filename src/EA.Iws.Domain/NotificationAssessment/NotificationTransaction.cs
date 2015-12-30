namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using Core.NotificationAssessment;
    using Core.Shared;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class NotificationTransaction : Entity
    {
        protected NotificationTransaction() 
        {
        }

        public NotificationTransaction(NotificationTransactionData notificationTransactionData)
        {
            if (notificationTransactionData.ReceiptNumber != null && notificationTransactionData.ReceiptNumber.Length > 70)
            {
                throw new InvalidOperationException(string.Format("NotificationId {0} - Receipt number must not be more than 70 characters", Id));
            }

            if (notificationTransactionData.Comments != null && notificationTransactionData.Comments.Length > 500)
            {
                throw new InvalidOperationException(string.Format("NotificationId {0} - Comments must not be more than 500 characters", Id));
            }

            Date = notificationTransactionData.Date;
            NotificationId = notificationTransactionData.NotificationId;
            Debit = notificationTransactionData.Debit;
            Credit = notificationTransactionData.Credit;
            PaymentMethod = notificationTransactionData.PaymentMethod;
            ReceiptNumber = notificationTransactionData.ReceiptNumber;
            Comments = notificationTransactionData.Comments;
        }

        public DateTime Date
        {
            get { return date; }
            private set
            {
                Guard.ArgumentNotDefaultValue(() => value, value);
                date = value;
            }
        }

        public Guid NotificationId
        {
            get { return notificationId; }
            private set
            {
                Guard.ArgumentNotDefaultValue(() => value, value);
                notificationId = value;
            }
        }

        public decimal? Debit { get; private set; }

        public decimal? Credit { get; private set; }

        public PaymentMethod? PaymentMethod { get; private set; }

        public string ReceiptNumber { get; private set; }

        public string Comments { get; private set; }

        private DateTime date;
        private Guid notificationId;
    }
}
