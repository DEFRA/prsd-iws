namespace EA.Iws.Domain.NotificationAssessment
{
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
