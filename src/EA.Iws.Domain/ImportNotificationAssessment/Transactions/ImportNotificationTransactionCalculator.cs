namespace EA.Iws.Domain.ImportNotificationAssessment.Transactions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ImportNotification;

    public class ImportNotificationTransactionCalculator : IImportNotificationTransactionCalculator
    {
        private readonly IImportNotificationAssessmentRepository notificationAssessmentRepository;
        private readonly IImportNotificationTransactionRepository transactionRepository;

        public ImportNotificationTransactionCalculator(IImportNotificationAssessmentRepository notificationAssessmentRepository,
            IImportNotificationTransactionRepository transactionRepository)
        {
            this.notificationAssessmentRepository = notificationAssessmentRepository;
            this.transactionRepository = transactionRepository;
        }

        public async Task<decimal> Balance(Guid importNotificationId)
        {
            var transactions = (await transactionRepository.GetTransactions(importNotificationId)).ToArray();

            return TotalCredits(transactions) - TotalDebits(transactions);
        }

        public decimal TotalCredits(IEnumerable<ImportNotificationTransaction> transactions)
        {
            return transactions.Sum(t => t.Credit.GetValueOrDefault(0));
        }

        public decimal TotalDebits(IEnumerable<ImportNotificationTransaction> transactions)
        {
            return transactions.Sum(t => t.Debit.GetValueOrDefault(0));
        }

        public async Task<bool> PaymentIsNowFullyReceived(Guid importNotificationId, decimal credit)
        {
            var balance = await Balance(importNotificationId);

            return false;
        }
    }
}