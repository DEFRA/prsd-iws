namespace EA.Iws.RequestHandlers.Admin.FinancialGuarantee
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.FinancialGuarantee;
    using Prsd.Core.Mediator;
    using Requests.Admin.FinancialGuarantee;

    internal class ApproveFinancialGuaranteeHandler : IRequestHandler<ApproveFinancialGuarantee, bool>
    {
        private readonly IFinancialGuaranteeRepository repository;
        private readonly IwsContext context;

        public ApproveFinancialGuaranteeHandler(IFinancialGuaranteeRepository repository, IwsContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        public async Task<bool> HandleAsync(ApproveFinancialGuarantee message)
        {
            var financialGuaranteeCollection = await repository.GetByNotificationId(message.NotificationId);
            var financialGuarantee = financialGuaranteeCollection.GetFinancialGuarantee(message.FinancialGuaranteeId);

            financialGuarantee.Approve(new ApproveDates(message.DecisionDate, 
                message.ReferenceNumber, message.ActiveLoadsPermitted, message.IsBlanketbond));

            await context.SaveChangesAsync();

            return true;
        }
    }
}
