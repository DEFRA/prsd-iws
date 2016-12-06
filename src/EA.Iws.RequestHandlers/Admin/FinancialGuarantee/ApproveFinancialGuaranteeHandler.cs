namespace EA.Iws.RequestHandlers.Admin.FinancialGuarantee
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.FinancialGuarantee;
    using Prsd.Core.Mediator;
    using Requests.Admin.FinancialGuarantee;

    internal class ApproveFinancialGuaranteeHandler : IRequestHandler<ApproveFinancialGuarantee, Unit>
    {
        private readonly FinancialGuaranteeApproval financialGuaranteeApproval;
        private readonly IwsContext context;

        public ApproveFinancialGuaranteeHandler(FinancialGuaranteeApproval financialGuaranteeApproval, IwsContext context)
        {
            this.financialGuaranteeApproval = financialGuaranteeApproval;
            this.context = context;
        }

        public async Task<Unit> HandleAsync(ApproveFinancialGuarantee message)
        {
            await financialGuaranteeApproval.Approve(message.NotificationId, message.FinancialGuaranteeId,
                new ApprovalData(message.DecisionDate, 
                    message.ReferenceNumber, message.ActiveLoadsPermitted, message.IsBlanketbond));

            await context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
