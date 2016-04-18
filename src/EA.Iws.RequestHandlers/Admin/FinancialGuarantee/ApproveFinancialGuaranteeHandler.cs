namespace EA.Iws.RequestHandlers.Admin.FinancialGuarantee
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.FinancialGuarantee;
    using Prsd.Core.Mediator;
    using Requests.Admin.FinancialGuarantee;

    internal class ApproveFinancialGuaranteeHandler : IRequestHandler<ApproveFinancialGuarantee, bool>
    {
        private readonly IwsContext context;

        public ApproveFinancialGuaranteeHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<bool> HandleAsync(ApproveFinancialGuarantee message)
        {
            var financialGuarantee = await context.FinancialGuarantees.SingleAsync(fg => fg.NotificationApplicationId == message.NotificationId);

            financialGuarantee.Approve(new ApproveDates(message.DecisionDate, message.ValidFrom, message.ValidTo, 
                message.ReferenceNumber, message.ActiveLoadsPermitted, message.IsBlanketbond));

            await context.SaveChangesAsync();

            return true;
        }
    }
}
