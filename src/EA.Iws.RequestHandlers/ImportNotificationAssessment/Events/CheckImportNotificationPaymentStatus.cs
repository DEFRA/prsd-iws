namespace EA.Iws.RequestHandlers.ImportNotificationAssessment.Events
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ImportNotificationAssessment;
    using DataAccess;
    using Domain.ImportNotificationAssessment;
    using Domain.ImportNotificationAssessment.Transactions;
    using Prsd.Core.Domain;

    public class CheckImportNotificationPaymentStatus : IEventHandler<ImportNotificationSubmittedEvent>
    {
        private readonly IImportNotificationTransactionCalculator transactionCalculator;
        private readonly ImportNotificationContext context;
        private readonly IImportNotificationTransactionRepository transactionRepository;

        public CheckImportNotificationPaymentStatus(IImportNotificationTransactionCalculator transactionCalculator, 
            ImportNotificationContext context,
            IImportNotificationTransactionRepository transactionRepository)
        {
            this.transactionCalculator = transactionCalculator;
            this.context = context;
            this.transactionRepository = transactionRepository;
        }

        public async Task HandleAsync(ImportNotificationSubmittedEvent @event)
        {
            var isFullyPaid = await transactionCalculator.PaymentIsNowFullyReceived(@event.Assessment.NotificationApplicationId, 0);

            if (isFullyPaid && @event.Assessment.Status == ImportNotificationStatus.AwaitingPayment)
            {
                var transactions = await transactionRepository.GetTransactions(@event.Assessment.NotificationApplicationId);

                @event.Assessment.PaymentComplete(transactions.OrderBy(t => t.Date).Select(t => t.Date).Last());
            }

            await context.SaveChangesAsync();
        }
    }
}
