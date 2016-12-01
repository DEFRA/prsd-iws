namespace EA.Iws.RequestHandlers.Admin.FinancialGuarantee
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.FinancialGuarantee;
    using Prsd.Core.Mediator;
    using Requests.Admin.FinancialGuarantee;

    internal class CompleteFinancialGuaranteeHandler : IRequestHandler<CompleteFinancialGuarantee, Unit>
    {
        private readonly IwsContext context;
        private readonly IFinancialGuaranteeRepository repository;

        public CompleteFinancialGuaranteeHandler(IFinancialGuaranteeRepository repository, IwsContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        public async Task<Unit> HandleAsync(CompleteFinancialGuarantee message)
        {
            var financialGuaranteeCollection = await repository.GetByNotificationId(message.NotificationId);

            var financialGuarantee = financialGuaranteeCollection.GetFinancialGuarantee(message.FinancialGuaranteeId);

            financialGuarantee.Complete(message.CompletedDate);

            await context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}