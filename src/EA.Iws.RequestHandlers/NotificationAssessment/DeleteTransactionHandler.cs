namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class DeleteTransactionHandler : IRequestHandler<DeleteTransaction, bool>
    {
        private readonly INotificationTransactionRepository repository;
        private readonly IwsContext context;

        public DeleteTransactionHandler(INotificationTransactionRepository repository, IwsContext context)
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
