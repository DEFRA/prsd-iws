namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class AddNotificationTransactionHandler : IRequestHandler<AddNotificationTransaction, bool>
    {
        private readonly IwsContext context;
        private readonly INotificationTransactionRepository repository;

        public AddNotificationTransactionHandler(IwsContext context, INotificationTransactionRepository repository)
        {
            this.context = context;
            this.repository = repository;
        }

        public async Task<bool> HandleAsync(AddNotificationTransaction message)
        {
            repository.Add(message.Data);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
