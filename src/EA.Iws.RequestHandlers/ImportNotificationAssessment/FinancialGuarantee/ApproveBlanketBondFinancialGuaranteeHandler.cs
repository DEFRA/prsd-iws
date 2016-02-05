namespace EA.Iws.RequestHandlers.ImportNotificationAssessment.FinancialGuarantee
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportNotificationAssessment.FinancialGuarantee;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment.FinancialGuarantee;

    internal class ApproveBlanketBondFinancialGuaranteeHandler : IRequestHandler<ApproveBlanketBondFinancialGuarantee, bool>
    {
        private readonly ApproveImportFinancialGuarantee approveFinancialGuarantee;
        private readonly ImportNotificationContext context;

        public ApproveBlanketBondFinancialGuaranteeHandler(ApproveImportFinancialGuarantee approveFinancialGuarantee, ImportNotificationContext context)
        {
            this.approveFinancialGuarantee = approveFinancialGuarantee;
            this.context = context;
        }

        public async Task<bool> HandleAsync(ApproveBlanketBondFinancialGuarantee message)
        {
            await
                approveFinancialGuarantee.ApproveBlanketBond(
                    new DecisionData(message.ImportNotificationId, message.DecisionDate),
                    message.ValidFrom,
                    message.ActiveLoadsPermitted,
                    message.BlanketBondReference);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
