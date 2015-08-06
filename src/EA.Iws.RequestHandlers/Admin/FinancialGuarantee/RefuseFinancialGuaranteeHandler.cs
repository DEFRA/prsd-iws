namespace EA.Iws.RequestHandlers.Admin.FinancialGuarantee
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Admin.FinancialGuarantee;

    internal class RefuseFinancialGuaranteeHandler : IRequestHandler<RefuseFinancialGuarantee, bool>
    {
        private readonly IwsContext context;

        public RefuseFinancialGuaranteeHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<bool> HandleAsync(RefuseFinancialGuarantee message)
        {
            var financialGuarantee = await context.FinancialGuarantees.SingleAsync(fg => fg.NotificationApplicationId == message.NotificationId);

            financialGuarantee.Refuse(message.DecisionDate, message.ReasonForRefusal);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
