namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class DeleteTransactionHandler : IRequestHandler<DeleteTransaction, bool>
    {
        private readonly Transaction transaction;
        private readonly IwsContext context;

        public DeleteTransactionHandler(Transaction transaction, IwsContext context)
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
