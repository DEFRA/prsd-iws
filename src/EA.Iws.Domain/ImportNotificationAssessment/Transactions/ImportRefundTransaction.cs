namespace EA.Iws.Domain.ImportNotificationAssessment.Transactions
{
    using System;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using ImportNotification;
    using Prsd.Core;

    [AutoRegister]
    public class ImportRefundTransaction
    {
        private readonly IImportNotificationTransactionRepository transactionRepository;
        private readonly IImportNotificationTransactionCalculator transactionCalculator;
        private readonly IImportNotificationAssessmentRepository assessmentRepository;

        public ImportRefundTransaction(IImportNotificationTransactionRepository transactionRepository,
            IImportNotificationTransactionCalculator transactionCalculator,
            IImportNotificationAssessmentRepository assessmentRepository)
        {
            this.transactionRepository = transactionRepository;
            this.transactionCalculator = transactionCalculator;
            this.assessmentRepository = assessmentRepository;
        }

        public async Task Save(Guid notificationId, DateTime date, decimal amount, string comments)
        {
            if (date > SystemTime.Now)
            {
                throw new InvalidOperationException(string.Format("Refund date cannot be in the future for notification {0}", notificationId));
            }

            var totalPaid = await transactionCalculator.TotalPaid(notificationId);

            if (amount > totalPaid)
            {
                throw new InvalidOperationException(string.Format("Refund amount cannot exceed total amount paid for notification {0}", notificationId));
            }

            var assessment = await assessmentRepository.GetByNotification(notificationId);

            if (!assessment.Dates.PaymentReceivedDate.HasValue)
            {
                throw new InvalidOperationException(string.Format("A refund cannot be made until a payment has been made for notification {0}", notificationId));
            }

            if (date < assessment.Dates.PaymentReceivedDate.Value)
            {
                throw new InvalidOperationException(string.Format("Refund date cannot be before the payment received date for notification {0}", notificationId));
            }

            transactionRepository.Add(ImportNotificationTransaction.RefundRecord(notificationId, date, amount, comments));
        }
    }
}