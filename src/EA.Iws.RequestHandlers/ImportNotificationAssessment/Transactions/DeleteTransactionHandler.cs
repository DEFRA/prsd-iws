namespace EA.Iws.RequestHandlers.ImportNotificationAssessment.Transactions
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportNotificationAssessment.Transactions;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment.Transactions;

    internal class DeleteTransactionHandler : IRequestHandler<DeleteTransaction, bool>
    {
        private readonly ImportPaymentTransaction transaction;
        private readonly ImportNotificationContext context;

        public DeleteTransactionHandler(ImportPaymentTransaction transaction, ImportNotificationContext context)
        {
            this.transaction = transaction;
            this.context = context;
        }
        public async Task<bool> HandleAsync(DeleteTransaction message)
        {
            await transaction.Delete(message.NotificationId, message.TransactionId);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
