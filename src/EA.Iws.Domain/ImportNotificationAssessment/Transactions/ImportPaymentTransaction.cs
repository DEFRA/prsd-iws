namespace EA.Iws.Domain.ImportNotificationAssessment.Transactions
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using Core.ImportNotificationAssessment;
    using Core.Shared;
    using ImportNotification;

    [AutoRegister]
    public class ImportPaymentTransaction
    {
        private readonly IImportNotificationTransactionRepository transactionRepository;
        private readonly IImportNotificationTransactionCalculator transactionCalculator;
        private readonly IImportNotificationAssessmentRepository assessmentRepository;

        public ImportPaymentTransaction(IImportNotificationTransactionRepository transactionRepository, 
            IImportNotificationTransactionCalculator transactionCalculator,
            IImportNotificationAssessmentRepository assessmentRepository)
        {
            this.transactionRepository = transactionRepository;
            this.transactionCalculator = transactionCalculator;
            this.assessmentRepository = assessmentRepository;
        }

        public async Task Save(Guid notificationId, DateTime date, decimal amount, PaymentMethod paymentMethod,
            string receiptNumber, string comments)
        {
            var transaction = ImportNotificationTransaction.PaymentRecord(notificationId, date, amount,
                paymentMethod, receiptNumber, comments);

            transactionRepository.Add(transaction);

            await UpdatePaymentReceivedDate(notificationId);
        }

        public async Task Delete(Guid notificationId, Guid transactionId)
        {
            await transactionRepository.DeleteById(transactionId);

            await UpdatePaymentReceivedDate(notificationId);
        }

        private async Task UpdatePaymentReceivedDate(Guid notificationId)
        {
            var fullPaymentDate = await transactionCalculator.PaymentReceivedDate(notificationId);

            var assessment = await assessmentRepository.GetByNotification(notificationId);

            if (fullPaymentDate != null)
            {
                if (assessment.Status == ImportNotificationStatus.AwaitingPayment)
                {
                    assessment.PaymentComplete(fullPaymentDate.GetValueOrDefault());
                }
                else
                {
                    assessment.Dates.PaymentReceivedDate = fullPaymentDate;
                }
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
