namespace EA.Iws.Domain.ImportNotificationAssessment.Transactions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using Core.ImportNotificationAssessment;
    using ImportNotification;
    using Prsd.Core;

    [AutoRegister]
    public class ImportRefundTransaction
    {
        private readonly IImportNotificationTransactionCalculator transactionCalculator;
        private readonly IImportNotificationTransactionRepository transactionRepository;
        private readonly IImportNotificationAssessmentRepository assessmentRepository;

        public ImportRefundTransaction(
            IImportNotificationTransactionRepository transactionRepository,
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

            var transaction = ImportNotificationTransaction.RefundRecord(notificationId, date, amount, comments);

            var balance = await transactionCalculator.Balance(transaction.NotificationId)
                - transaction.Credit.GetValueOrDefault()
                + transaction.Debit.GetValueOrDefault();

            var transactions = (await transactionRepository.GetTransactions(transaction.NotificationId)).ToList();
            transactions.Add(transaction);

            var paymentDate = CalculatePaymentReceivedDate(transactions, balance);

            await UpdatePaymentReceivedDate(paymentDate, notificationId);

            transactionRepository.Add(transaction);
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