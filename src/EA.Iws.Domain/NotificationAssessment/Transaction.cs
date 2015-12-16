namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;

    public class Transaction
    {
        private readonly INotificationAssessmentRepository notificationAssessmentRepository;
        private readonly INotificationTransactionRepository transactionRepository;
        private readonly INotificationTransactionCalculator transactionCalculator;

        public Transaction(INotificationAssessmentRepository notificationAssessmentRepository,
            INotificationTransactionRepository transactionRepository,
            INotificationTransactionCalculator transactionCalculator)
        {
            this.notificationAssessmentRepository = notificationAssessmentRepository;
            this.transactionRepository = transactionRepository;
            this.transactionCalculator = transactionCalculator;
        }

        public async Task Save(NotificationTransactionData data)
        {
            if (data.Debit > await transactionCalculator.RefundLimit(data.NotificationId))
            {
                throw new InvalidOperationException("Transaction cannot refund more than has already been paid");
            }

            transactionRepository.Add(data);

            if (data.Credit > 0 && await transactionCalculator.PaymentIsNowFullyReceived(data))
            {
                var assessment = await notificationAssessmentRepository.GetByNotificationId(data.NotificationId);

                if (assessment.Dates.PaymentReceivedDate == null)
                {
                    assessment.Dates.PaymentReceivedDate = data.Date;
                }
            }
        }
    }
}
