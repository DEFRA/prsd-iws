namespace EA.Iws.RequestHandlers.ImportNotificationAssessment.Transactions
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportNotificationAssessment.Transactions;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment.Transactions;

    internal class DeleteTransactionHandler : IRequestHandler<DeleteTransaction, bool>
    {
        private readonly IImportNotificationTransactionRepository repository;
        private readonly ImportNotificationContext context;

        public DeleteTransactionHandler(IImportNotificationTransactionRepository repository, ImportNotificationContext context)
        {
            this.repository = repository;
            this.context = context;
        }
        public async Task<bool> HandleAsync(DeleteTransaction message)
        {
            await repository.DeleteById(message.TransactionId);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
