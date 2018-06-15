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

            transactionRepository.Add(transaction);

            await UpdatePaymentReceivedDate(transaction.NotificationId);
        }

        public async Task Delete(Guid notificationId, Guid transactionId)
        {
            await transactionRepository.DeleteById(transactionId);

            await UpdatePaymentReceivedDate(notificationId);
        }

        private async Task UpdatePaymentReceivedDate(Guid notificationId)
        {
            var fullPaymentDate = await transactionCalculator.PaymentReceivedDate(notificationId);

            var assessment = await notificationAssessmentRepository.GetByNotificationId(notificationId);

            if (fullPaymentDate != null)
            {
                assessment.Dates.PaymentReceivedDate = fullPaymentDate;
            }
            else
            {
                if (assessment.Dates.PaymentReceivedDate.HasValue)
                {
                    assessment.Dates.PaymentReceivedDate = null;
                }
            }
        }
    }
}
