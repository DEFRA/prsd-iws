namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class AddNotificationTransactionHandler : IRequestHandler<AddNotificationTransaction, bool>
    {
        private readonly IwsContext context;
        private readonly INotificationTransactionRepository repository;
        private readonly INotificationTransactionCalculator transactionCalculator;

        public AddNotificationTransactionHandler(IwsContext context, 
            INotificationTransactionRepository repository,  
            INotificationTransactionCalculator transactionCalculator)
        {
            this.context = context;
            this.repository = repository;
            this.transactionCalculator = transactionCalculator;
        }

        public async Task<bool> HandleAsync(AddNotificationTransaction message)
        {
            repository.Add(message.Data);
            
            if (message.Data.Credit > 0 && await transactionCalculator.PaymentIsNowFullyReceived(message.Data))
            {
                var assessment = await context.NotificationAssessments.SingleAsync(p => p.NotificationApplicationId == message.Data.NotificationId);

                if (assessment.Dates.PaymentReceivedDate == null)
                {
                    assessment.PaymentReceived(message.Data.Date); 
                }
            }

            await context.SaveChangesAsync();

            return true;
        }
    }
}
