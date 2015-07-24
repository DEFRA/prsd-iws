namespace EA.Iws.Domain.FinancialGuarantee
{
    using System;
    using Core.FinancialGuarantee;
    using Prsd.Core.Domain;

    public class FinancialGuarantee : Entity
    {
        public Guid NotificationId { get; set; }

        public DateTime? ReceivedDate { get; private set; }

        public DateTime? CompletedDate { get; private set; }

        public DateTime? DecisionRequiredDate
        {
            get
            {
                if (CompletedDate.HasValue)
                {
                    return CompletedDate.Value.AddDays(20);
                }
                return null;
            }
        }

        public FinancialGuaranteeStatus Status
        {
            get
            {
                if (ReceivedDate.HasValue)
                {
                    return (CompletedDate.HasValue)
                        ? FinancialGuaranteeStatus.Completed
                        : FinancialGuaranteeStatus.Received;
                }

                return FinancialGuaranteeStatus.PendingReceiptOfApplication;
            }
        }

        protected FinancialGuarantee()
        {
        }

        public FinancialGuarantee(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public void SetReceivedDate(DateTime date)
        {
            ReceivedDate = date;
        }

        public void SetCompletedDate(DateTime date)
        {
            if (!ReceivedDate.HasValue)
            {
                throw new InvalidOperationException(string.Format("Cannot set FG completed date while received date is blank for notification {0}.", NotificationId));
            }

            if (date < ReceivedDate.Value)
            {
                throw new InvalidOperationException(string.Format("Cannot set FG completed date before received date for notification {0}.", NotificationId));
            }

            CompletedDate = date;
        }
    }
}
