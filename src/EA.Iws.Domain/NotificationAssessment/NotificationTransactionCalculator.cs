namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using Core.NotificationAssessment;
    using NotificationApplication;

    [AutoRegister]
    public class NotificationTransactionCalculator : INotificationTransactionCalculator
    {
        private readonly INotificationTransactionRepository transactionRepository;

        private readonly INotificationChargeCalculator chargeCalculator;

        public NotificationTransactionCalculator(INotificationTransactionRepository transactionRepository,
            INotificationChargeCalculator chargeCalculator)
        {
            this.transactionRepository = transactionRepository;
            this.chargeCalculator = chargeCalculator;
        }

        public decimal TotalCredits(IList<NotificationTransaction> transactions)
        {
            return transactions.Sum(t => t.Credit).GetValueOrDefault();
        }

        public decimal TotalDebits(IList<NotificationTransaction> transactions)
        {
            return transactions.Sum(t => t.Debit).GetValueOrDefault();
        }

        public async Task<decimal> Balance(Guid notificationId)
        {
            var transactions = await transactionRepository.GetTransactions(notificationId);
            var totalBillable = await chargeCalculator.GetValue(notificationId);

            var totalPaid = TotalCredits(transactions) - TotalDebits(transactions);

            return totalBillable - totalPaid;
        }

        public async Task<bool> IsPaymentComplete(Guid notificationId)
        {
            return await Balance(notificationId) <= 0;
        }

        public async Task<NotificationTransaction> LatestPayment(Guid notificationId)
        {
            var transactions = await transactionRepository.GetTransactions(notificationId);
            return transactions.Where(t => t.Credit > 0).OrderByDescending(t => t.Date).FirstOrDefault();
        }

        public async Task<bool> PaymentIsNowFullyReceived(NotificationTransactionData data)
        {
            var newBalance = await Balance(data.NotificationId)
                - data.Credit.GetValueOrDefault()
                + data.Debit.GetValueOrDefault();

            return newBalance <= 0;
        }

        public async Task<decimal> RefundLimit(Guid notificationId)
        {
            var transactions = await transactionRepository.GetTransactions(notificationId);

            return TotalCredits(transactions) - TotalDebits(transactions);
        }

        public async Task<decimal> TotalPaid(Guid notificationId)
        {
            var transactions = await transactionRepository.GetTransactions(notificationId);
            return TotalCredits(transactions) - TotalDebits(transactions);
        }
    }
}
