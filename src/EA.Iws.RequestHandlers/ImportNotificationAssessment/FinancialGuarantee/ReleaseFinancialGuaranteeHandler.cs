namespace EA.Iws.RequestHandlers.ImportNotificationAssessment.FinancialGuarantee
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportNotificationAssessment.FinancialGuarantee;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment.FinancialGuarantee;

    internal class ReleaseFinancialGuaranteeHandler : IRequestHandler<ReleaseFinancialGuarantee, bool>
    {
        private readonly ReleaseImportFinancialGuarantee releaseGuarantee;
        private readonly ImportNotificationContext context;

        public ReleaseFinancialGuaranteeHandler(ReleaseImportFinancialGuarantee releaseGuarantee, ImportNotificationContext context)
        {
            this.releaseGuarantee = releaseGuarantee;
            this.context = context;
        }

        public async Task<bool> HandleAsync(ReleaseFinancialGuarantee message)
        {
            await releaseGuarantee.Release(new DecisionData(message.ImportNotificationId, message.DecisionDate));

            await context.SaveChangesAsync();

            return true;
        }
    }
}
