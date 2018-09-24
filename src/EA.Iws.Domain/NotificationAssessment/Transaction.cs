namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;

    [AutoRegister]
    public class Transaction
    {
        private readonly INotificationAssessmentRepository notificationAssessmentRepository;
        private readonly INotificationTransactionRepository transactionRepository;
        private readonly INotificationTransactionCalculator transactionCalculator;
        
        public Transaction(INotificationAssessmentRepository notificationAssessmentRepository,
            INotificationTransactionRepository transactionRepository, 
            INotificationTransactionCalculator transactionCalculator)
        {
            this.notificationAssessmentRepository = notificationAssessmentRepository;
            this.transactionRepository = transactionRepository;
            this.transactionCalculator = transactionCalculator;
        }

        public async Task Save(NotificationTransaction transaction)
        {
            if (transaction.Debit > await transactionCalculator.RefundLimit(transaction.NotificationId))
            {
                throw new InvalidOperationException("Transaction cannot refund more than has already been paid");
            }

            transactionRepository.Add(transaction);

            var balance = await transactionCalculator.Balance(transaction.NotificationId)
                - transaction.Credit.GetValueOrDefault()
                + transaction.Debit.GetValueOrDefault();
            var transactions = await transactionRepository.GetTransactions(transaction.NotificationId);
            var paymentDate = CalculatePaymentReceivedDate(transactions, balance);

            await UpdatePaymentReceivedDate(paymentDate, transaction.NotificationId);
        }

        public async Task Delete(Guid notificationId, Guid transactionId)
        {
            var transaction = await transactionRepository.GetById(transactionId);
            
            var balance = await transactionCalculator.Balance(transaction.NotificationId)
                + transaction.Credit.GetValueOrDefault()
                - transaction.Debit.GetValueOrDefault();
            var transactions = await transactionRepository.GetTransactions(notificationId);
            transactions.Remove(transactions.FirstOrDefault(t => t.Id == transactionId));
            var paymentDate = CalculatePaymentReceivedDate(transactions, balance);

            await UpdatePaymentReceivedDate(paymentDate, notificationId);

            await transactionRepository.DeleteById(transactionId);
        }

        private static DateTime? CalculatePaymentReceivedDate(IList<NotificationTransaction> transactions, decimal balance)
        {
            if (balance <= 0)
            {
                transactions = transactions.Where(t => t.Credit > 0).OrderByDescending(t => t.Date).ToList();

                foreach (var tran in transactions)
                {
                    balance += tran.Credit.GetValueOrDefault() - tran.Debit.GetValueOrDefault();

                    if (balance > 0)
                    {
                        return tran.Date;
                    }
                }
            }
            return null;
        }

        private async Task UpdatePaymentReceivedDate(DateTime? paymentDate, Guid notificationId)
        {
            var assessment = await notificationAssessmentRepository.GetByNotificationId(notificationId);

            if (paymentDate != null)
            {
                assessment.Dates.PaymentReceivedDate = paymentDate;
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
