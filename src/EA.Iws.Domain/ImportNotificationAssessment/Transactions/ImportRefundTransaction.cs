namespace EA.Iws.Domain.ImportNotificationAssessment.Transactions
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using Prsd.Core;

    [AutoRegister]
    public class ImportRefundTransaction
    {
        private readonly IImportNotificationTransactionCalculator transactionCalculator;
        private readonly IImportNotificationTransactionRepository transactionRepository;

        public ImportRefundTransaction(IImportNotificationTransactionRepository transactionRepository,
            IImportNotificationTransactionCalculator transactionCalculator)
        {
            this.transactionRepository = transactionRepository;
            this.transactionCalculator = transactionCalculator;
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

            var firstPayment = (await transactionRepository.GetTransactions(notificationId))
                .OrderBy(x => x.Date)
                .FirstOrDefault(x => x.Credit != null);

            if (firstPayment == null)
            {
                throw new InvalidOperationException(
                    string.Format("Can't make a refund for notification {0} when no payments have been made", notificationId));
            }

            if (date < firstPayment.Date)
            {
                throw new InvalidOperationException(
                    string.Format("Refund date cannot be before the first payment date for notification {0}",
                        notificationId));
            }

            transactionRepository.Add(ImportNotificationTransaction.RefundRecord(notificationId, date, amount, comments));
        }
    }
}