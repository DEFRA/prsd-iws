namespace EA.Iws.Domain.ImportNotificationAssessment.Transactions
{
    using System;
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
            if (await transactionCalculator.PaymentIsNowFullyReceived(notificationId, amount))
            {
                var assessment = await assessmentRepository.GetByNotification(notificationId);

                if (assessment.Status == ImportNotificationStatus.AwaitingPayment)
                {
                    assessment.PaymentComplete(date);
                }
                else if (!assessment.Dates.PaymentReceivedDate.HasValue)
                {
                    assessment.Dates.PaymentReceivedDate = date;
                }
            }

            transactionRepository.Add(ImportNotificationTransaction.PaymentRecord(notificationId, date, amount, paymentMethod, receiptNumber, comments));
        }

        public async Task Delete(Guid notificationId, Guid transactionId)
        {
            var transaction = await transactionRepository.GetById(transactionId);
            var balance = await transactionCalculator.Balance(notificationId)
                + transaction.Credit.GetValueOrDefault()
                - transaction.Debit.GetValueOrDefault();

            if (balance > 0)
            {
                var assessment = await assessmentRepository.GetByNotification(notificationId);

                if (assessment.Dates.PaymentReceivedDate.HasValue)
                {
                    assessment.Dates.PaymentReceivedDate = null;
                }
            }

            await transactionRepository.DeleteById(transactionId);
        }
    }
}
