namespace EA.Iws.RequestHandlers.Admin.FinancialGuarantee
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.FinancialGuarantee;
    using Prsd.Core.Mediator;
    using Requests.Admin.FinancialGuarantee;

    internal class RefuseFinancialGuaranteeHandler : IRequestHandler<RefuseFinancialGuarantee, Unit>
    {
        private readonly IFinancialGuaranteeRepository repository;
        private readonly IwsContext context;

        public RefuseFinancialGuaranteeHandler(IFinancialGuaranteeRepository repository, IwsContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        public async Task<Unit> HandleAsync(RefuseFinancialGuarantee message)
        {
            var financialGuaranteeCollection = await repository.GetByNotificationId(message.NotificationId);
            var financialGuarantee = financialGuaranteeCollection.GetFinancialGuarantee(message.FinancialGuaranteeId);

            financialGuarantee.Refuse(message.DecisionDate, message.ReasonForRefusal);

            await context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
