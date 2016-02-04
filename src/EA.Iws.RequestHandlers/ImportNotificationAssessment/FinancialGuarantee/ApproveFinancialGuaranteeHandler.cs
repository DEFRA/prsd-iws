namespace EA.Iws.RequestHandlers.ImportNotificationAssessment.FinancialGuarantee
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.ImportNotificationAssessment.FinancialGuarantee;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment.FinancialGuarantee;

    internal class ApproveFinancialGuaranteeHandler : IRequestHandler<ApproveFinancialGuarantee, bool>
    {
        private readonly ApproveImportFinancialGuarantee approveFinancialGuarantee;
        private readonly ImportNotificationContext context;

        public ApproveFinancialGuaranteeHandler(ApproveImportFinancialGuarantee approveFinancialGuarantee, ImportNotificationContext context)
        {
            this.approveFinancialGuarantee = approveFinancialGuarantee;
            this.context = context;
        }

        public async Task<bool> HandleAsync(ApproveFinancialGuarantee message)
        {
            await
                approveFinancialGuarantee.Approve(new DecisionData(message.ImportNotificationId, message.DecisionDate),
                    new DateRange(message.ValidFrom, message.ValidTo),
                    message.ActiveLoadsPermitted,
                    message.Reference);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
