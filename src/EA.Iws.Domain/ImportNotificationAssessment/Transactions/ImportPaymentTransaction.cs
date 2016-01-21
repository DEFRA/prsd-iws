namespace EA.Iws.Domain.ImportNotificationAssessment.Transactions
{
    using System;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
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
            transactionRepository.Add(ImportNotificationTransaction.PaymentRecord(notificationId, date, amount, paymentMethod, receiptNumber, comments));

            if (await transactionCalculator.PaymentIsNowFullyReceived(notificationId, amount))
            {
                var assessment = await assessmentRepository.GetByNotification(notificationId);

                assessment.PaymentComplete(date);
            }
        }
    }
}
