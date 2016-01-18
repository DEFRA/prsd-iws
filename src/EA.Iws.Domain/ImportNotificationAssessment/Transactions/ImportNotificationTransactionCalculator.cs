namespace EA.Iws.Domain.ImportNotificationAssessment.Transactions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using Prsd.Core;

    [AutoRegister]
    public class ImportNotificationTransactionCalculator : IImportNotificationTransactionCalculator
    {
        private readonly IImportNotificationTransactionRepository transactionRepository;
        private readonly IImportNotificationChargeCalculator chargeCalculator;

        public ImportNotificationTransactionCalculator(IImportNotificationTransactionRepository transactionRepository,
            IImportNotificationChargeCalculator chargeCalculator)
        {
            this.transactionRepository = transactionRepository;
            this.chargeCalculator = chargeCalculator;
        }

        public async Task<decimal> Balance(Guid importNotificationId)
        {
            var transactions = (await transactionRepository.GetTransactions(importNotificationId)).ToArray();

            return TotalCredits(transactions) - TotalDebits(transactions);
        }

        public decimal TotalCredits(IEnumerable<ImportNotificationTransaction> transactions)
        {
            Guard.ArgumentNotNull(() => transactions, transactions);

            return transactions.Sum(t => t.Credit.GetValueOrDefault(0));
        }

        public decimal TotalDebits(IEnumerable<ImportNotificationTransaction> transactions)
        {
            Guard.ArgumentNotNull(() => transactions, transactions);

            return transactions.Sum(t => t.Debit.GetValueOrDefault(0));
        }

        public async Task<bool> PaymentIsNowFullyReceived(Guid importNotificationId, decimal credit)
        {
            var price = await chargeCalculator.GetValue(importNotificationId);

            var balance = await Balance(importNotificationId) + credit;

            return price - balance <= 0;
        }
    }
}