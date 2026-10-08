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

            // Build the complete transaction list including the new (unsaved) transaction
            // so that balance and CalculatePaymentReceivedDate use the same consistent data.
            var transactions = (await transactionRepository.GetTransactions(notificationId)).ToList();
            transactions.Add(transaction);

            var balance = await CalculateBalance(notificationId, transactions);

            var paymentDate = CalculatePaymentReceivedDate(transactions, balance);

            await UpdatePaymentReceivedDate(paymentDate, notificationId);

            transactionRepository.Add(transaction);
        }

        public async Task Delete(Guid notificationId, Guid transactionId)
        {
            var transaction = await transactionRepository.GetById(transactionId);

            // Build the transaction list with the target transaction removed
            // so that balance and CalculatePaymentReceivedDate use the same consistent data.
            var transactions = (await transactionRepository.GetTransactions(notificationId)).ToList();
            transactions.RemoveAll(t => t.Id == transactionId);

            var balance = await CalculateBalance(notificationId, transactions);

            var paymentDate = CalculatePaymentReceivedDate(transactions, balance);

            await UpdatePaymentReceivedDate(paymentDate, notificationId);

            await transactionRepository.DeleteById(transactionId);
        }

        private async Task<decimal> CalculateBalance(Guid notificationId, IEnumerable<ImportNotificationTransaction> transactions)
        {
            var totalBillable = await transactionCalculator.TotalBillable(notificationId);
            var netPaid = transactions.Sum(t => t.Credit.GetValueOrDefault() - t.Debit.GetValueOrDefault());

            return totalBillable - netPaid;
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
