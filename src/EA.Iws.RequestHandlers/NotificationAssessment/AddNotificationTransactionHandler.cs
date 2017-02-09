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
        private readonly Transaction transaction;

        public AddNotificationTransactionHandler(IwsContext context, 
            Transaction transaction)
        {
            this.context = context;
            this.transaction = transaction;
        }

        public async Task<bool> HandleAsync(AddNotificationTransaction message)
        {
            await transaction.Save(new NotificationTransaction(message.Data));

            await context.SaveChangesAsync();

            return true;
        }
    }
}
