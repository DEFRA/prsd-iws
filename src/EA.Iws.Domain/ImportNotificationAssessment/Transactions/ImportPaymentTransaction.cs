namespace EA.Iws.Domain.ImportNotificationAssessment.Transactions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using Core.ImportNotificationAssessment;
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
            var transaction = ImportNotificationTransaction.PaymentRecord(notificationId, date, amount,
                paymentMethod, receiptNumber, comments);

            transactionRepository.Add(transaction);

            var balance = await transactionCalculator.Balance(transaction.NotificationId)
                - transaction.Credit.GetValueOrDefault()
                + transaction.Debit.GetValueOrDefault();
            var transactions = await transactionRepository.GetTransactions(transaction.NotificationId);
            var paymentDate = CalculatePaymentReceivedDate(transactions, balance);

            await UpdatePaymentReceivedDate(paymentDate, notificationId);
        }

        public async Task Delete(Guid notificationId, Guid transactionId)
        {
            var transaction = await transactionRepository.GetById(transactionId);
            
            var balance = await transactionCalculator.Balance(transaction.NotificationId)
                + transaction.Credit.GetValueOrDefault()
                - transaction.Debit.GetValueOrDefault();
            var transactions = await transactionRepository.GetTransactions(transaction.NotificationId);
            transactions = transactions.Where(t => t.Id != transactionId);
            var paymentDate = CalculatePaymentReceivedDate(transactions, balance);

            await UpdatePaymentReceivedDate(paymentDate, transaction.NotificationId);

            await transactionRepository.DeleteById(transactionId);
        }

        private static DateTime? CalculatePaymentReceivedDate(IEnumerable<ImportNotificationTransaction> transactions, decimal balance)
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
            var assessment = await assessmentRepository.GetByNotification(notificationId);

            if (paymentDate != null)
            {
                if (assessment.Status == ImportNotificationStatus.AwaitingPayment)
                {
                    assessment.PaymentComplete(paymentDate.GetValueOrDefault());
                }
                else
                {
                    assessment.Dates.PaymentReceivedDate = paymentDate;
                }
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
