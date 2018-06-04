namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
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

            var balance = await transactionCalculator.Balance(transaction.NotificationId)
                - transaction.Credit.GetValueOrDefault()
                + transaction.Debit.GetValueOrDefault();

            transactionRepository.Add(transaction);

            if (balance <= 0)
            {
                var assessment = await notificationAssessmentRepository.GetByNotificationId(transaction.NotificationId);
                var transactions = await transactionRepository.GetTransactions(transaction.NotificationId);
                transactions = transactions.Where(t => t.Credit > 0).OrderByDescending(t => t.Date).ToList();

                foreach (var tran in transactions)
                {
                    if (balance == 0)
                    {
                        assessment.Dates.PaymentReceivedDate = tran.Date;
                        break;
                    }
                    balance += tran.Credit.GetValueOrDefault() - tran.Debit.GetValueOrDefault();
                }
            }
        }

        public async Task Delete(Guid notificationId, Guid transactionId)
        {
            var transaction = await transactionRepository.GetById(transactionId);
            var balance = await transactionCalculator.Balance(transaction.NotificationId)
                + transaction.Credit.GetValueOrDefault()
                - transaction.Debit.GetValueOrDefault();
            var assessment = await notificationAssessmentRepository.GetByNotificationId(transaction.NotificationId);

            await transactionRepository.DeleteById(transactionId);

            if (balance > 0)
            {
                if (assessment.Dates.PaymentReceivedDate.HasValue)
                {
                    assessment.Dates.PaymentReceivedDate = null;
                }
            }
            else
            {
                var transactions = await transactionRepository.GetTransactions(transaction.NotificationId);
                transactions = transactions.Where(t => t.Credit > 0).OrderByDescending(t => t.Date).ToList();

                foreach (var tran in transactions)
                {
                    if (balance == 0)
                    {
                        assessment.Dates.PaymentReceivedDate = tran.Date;
                        break;
                    }
                    balance += tran.Credit.GetValueOrDefault() - tran.Debit.GetValueOrDefault();
                }
            }
        }
    }
}
