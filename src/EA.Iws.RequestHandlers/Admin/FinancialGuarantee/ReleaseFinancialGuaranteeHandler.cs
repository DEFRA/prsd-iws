namespace EA.Iws.RequestHandlers.Admin.FinancialGuarantee
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Admin.FinancialGuarantee;

    internal class ReleaseFinancialGuaranteeHandler : IRequestHandler<ReleaseFinancialGuarantee, bool>
    {
        private readonly IwsContext context;

        public ReleaseFinancialGuaranteeHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<bool> HandleAsync(ReleaseFinancialGuarantee message)
        {
            var financialGuarantee = await context.FinancialGuarantees.SingleAsync(fg => fg.NotificationApplicationId == message.NotificationId);

            financialGuarantee.Release(message.DecisionDate);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
