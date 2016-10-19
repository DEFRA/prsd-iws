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
        private readonly IImportNotificationAssessmentRepository assessmentRepository;
        private readonly IImportNotificationTransactionCalculator transactionCalculator;
        private readonly IImportNotificationTransactionRepository transactionRepository;

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
            if (date > SystemTime.UtcNow.Date)
            {
                throw new InvalidOperationException(
                    string.Format("Refund date cannot be in the future for notification {0}", notificationId));
            }

            var totalPaid = await transactionCalculator.TotalPaid(notificationId);

            if (amount > totalPaid)
            {
                throw new InvalidOperationException(
                    string.Format("Refund amount cannot exceed total amount paid for notification {0}", notificationId));
            }

            var assessment = await assessmentRepository.GetByNotification(notificationId);

            if (assessment.Dates != null
                && assessment.Dates.PaymentReceivedDate.HasValue
                && date < assessment.Dates.PaymentReceivedDate.Value)
            {
                throw new InvalidOperationException(
                    string.Format("Refund date cannot be before the payment received date for notification {0}",
                        notificationId));
            }

            transactionRepository.Add(ImportNotificationTransaction.RefundRecord(notificationId, date, amount, comments));
        }
    }
}