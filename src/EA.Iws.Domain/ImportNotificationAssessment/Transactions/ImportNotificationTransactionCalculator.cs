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

        public async Task<decimal> TotalPaid(Guid importNotificationId)
        {
            var transactions = (await transactionRepository.GetTransactions(importNotificationId)).ToArray();

            return TotalCredits(transactions) - TotalDebits(transactions);
        }

        private static decimal TotalCredits(IEnumerable<ImportNotificationTransaction> transactions)
        {
            Guard.ArgumentNotNull(() => transactions, transactions);

            return transactions.Sum(t => t.Credit.GetValueOrDefault(0));
        }

        private static decimal TotalDebits(IEnumerable<ImportNotificationTransaction> transactions)
        {
            Guard.ArgumentNotNull(() => transactions, transactions);

            return transactions.Sum(t => t.Debit.GetValueOrDefault(0));
        }

        public async Task<bool> PaymentIsNowFullyReceived(Guid importNotificationId, decimal credit)
        {
            var price = await chargeCalculator.GetValue(importNotificationId);

            var balance = await TotalPaid(importNotificationId) + credit;

            return price - balance <= 0;
        }

        public async Task<decimal> Balance(Guid importNotificationId)
        {
            var totalBillable = await chargeCalculator.GetValue(importNotificationId);
            var totalPaid = await TotalPaid(importNotificationId);

            return totalBillable - totalPaid;
        }

        public async Task<DateTime?> PaymentReceivedDate(Guid notificationId)
        {
            var balance = await Balance(notificationId);

            if (balance <= 0)
            {
                var transactions = await transactionRepository.GetTransactions(notificationId);
                transactions = transactions.Where(t => t.Credit > 0).OrderByDescending(t => t.Date).ToList();

                foreach (var tran in transactions)
                {
                    if (balance == 0)
                    {
                        return tran.Date;
                    }
                    balance += tran.Credit.GetValueOrDefault() - tran.Debit.GetValueOrDefault();
                }
            }
            return null;
        }
    }
}