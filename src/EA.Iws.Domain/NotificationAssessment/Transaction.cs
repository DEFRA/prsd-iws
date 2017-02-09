namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;

    [AutoRegister]
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

        public async Task Save(NotificationTransaction transaction)
        {
            if (transaction.Debit > await transactionCalculator.RefundLimit(transaction.NotificationId))
            {
                throw new InvalidOperationException("Transaction cannot refund more than has already been paid");
            }

            var balance = await transactionCalculator.Balance(transaction.NotificationId)
                - transaction.Credit.GetValueOrDefault()
                + transaction.Debit.GetValueOrDefault();

            if (transaction.Credit > 0 && balance <= 0)
            {
                var assessment = await notificationAssessmentRepository.GetByNotificationId(transaction.NotificationId);

                if (assessment.Dates.PaymentReceivedDate == null)
                {
                    assessment.Dates.PaymentReceivedDate = transaction.Date;
                }
            }

            transactionRepository.Add(transaction);
        }

        public async Task Delete(Guid notificationId, Guid transactionId)
        {
            var transaction = await transactionRepository.GetById(transactionId);
            var balance = await transactionCalculator.Balance(transaction.NotificationId)
                + transaction.Credit.GetValueOrDefault()
                - transaction.Debit.GetValueOrDefault();

            if (balance > 0)
            {
                var assessment = await notificationAssessmentRepository.GetByNotificationId(transaction.NotificationId);

                if (assessment.Dates.PaymentReceivedDate.HasValue)
                {
                    assessment.Dates.PaymentReceivedDate = null;
                }
            }

            await transactionRepository.DeleteById(transactionId);
        }
    }
}
