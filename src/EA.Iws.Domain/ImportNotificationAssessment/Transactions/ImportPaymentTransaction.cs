namespace EA.Iws.Domain.ImportNotificationAssessment.Transactions
{
    using System;
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
            var balance = await transactionCalculator.Balance(transaction.NotificationId)
               - transaction.Credit.GetValueOrDefault()
               + transaction.Debit.GetValueOrDefault();

            transactionRepository.Add(transaction);

            if (balance <= 0)
            {
                var assessment = await assessmentRepository.GetByNotification(notificationId);
                var transactions = await transactionRepository.GetTransactions(transaction.NotificationId);
                transactions = transactions.Where(t => t.Credit > 0).OrderByDescending(t => t.Date).ToList();

                foreach (var tran in transactions)
                {
                    if (balance == 0)
                    {
                        if (assessment.Status == ImportNotificationStatus.AwaitingPayment)
                        {
                            assessment.PaymentComplete(tran.Date);
                        }
                        else
                        {
                            assessment.Dates.PaymentReceivedDate = tran.Date;
                        }

                        break;
                    }

                    balance += tran.Credit.GetValueOrDefault() - tran.Debit.GetValueOrDefault();
                }
            }
        }

        public async Task Delete(Guid notificationId, Guid transactionId)
        {
            var transaction = await transactionRepository.GetById(transactionId);
            var balance = await transactionCalculator.Balance(notificationId)
                + transaction.Credit.GetValueOrDefault()
                - transaction.Debit.GetValueOrDefault();
            var assessment = await assessmentRepository.GetByNotification(notificationId);

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
