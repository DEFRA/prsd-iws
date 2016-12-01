namespace EA.Iws.RequestHandlers.Admin.FinancialGuarantee
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.FinancialGuarantee;
    using Prsd.Core.Mediator;
    using Requests.Admin.FinancialGuarantee;

    internal class ApproveFinancialGuaranteeHandler : IRequestHandler<ApproveFinancialGuarantee, Unit>
    {
        private readonly IFinancialGuaranteeRepository repository;
        private readonly IwsContext context;

        public ApproveFinancialGuaranteeHandler(IFinancialGuaranteeRepository repository, IwsContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        public async Task<Unit> HandleAsync(ApproveFinancialGuarantee message)
        {
            var financialGuaranteeCollection = await repository.GetByNotificationId(message.NotificationId);
            var financialGuarantee = financialGuaranteeCollection.GetFinancialGuarantee(message.FinancialGuaranteeId);

            financialGuarantee.Approve(new ApprovalData(message.DecisionDate, 
                message.ReferenceNumber, message.ActiveLoadsPermitted, message.IsBlanketbond));

            await context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
