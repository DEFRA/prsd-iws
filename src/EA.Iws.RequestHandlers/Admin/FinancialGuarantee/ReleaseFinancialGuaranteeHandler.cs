namespace EA.Iws.RequestHandlers.Admin.FinancialGuarantee
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.FinancialGuarantee;
    using Prsd.Core.Mediator;
    using Requests.Admin.FinancialGuarantee;

    internal class ReleaseFinancialGuaranteeHandler : IRequestHandler<ReleaseFinancialGuarantee, Unit>
    {
        private readonly IFinancialGuaranteeRepository repository;
        private readonly IwsContext context;

        public ReleaseFinancialGuaranteeHandler(IFinancialGuaranteeRepository repository, IwsContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        public async Task<Unit> HandleAsync(ReleaseFinancialGuarantee message)
        {
            var financialGuaranteeCollection = await repository.GetByNotificationId(message.NotificationId);
            var financialGuarantee = financialGuaranteeCollection.GetFinancialGuarantee(message.FinancialGuaranteeId);

            financialGuarantee.Release(message.DecisionDate);

            await context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
