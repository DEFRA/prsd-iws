namespace EA.Iws.RequestHandlers.ImportNotificationAssessment.FinancialGuarantee
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportNotificationAssessment.FinancialGuarantee;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment.FinancialGuarantee;

    internal class RefuseFinancialGuaranteeHandler : IRequestHandler<RefuseFinancialGuarantee, bool>
    {
        private readonly RefuseImportFinancialGuarantee refuseGuarantee;
        private readonly ImportNotificationContext context;

        public RefuseFinancialGuaranteeHandler(RefuseImportFinancialGuarantee refuseGuarantee, ImportNotificationContext context)
        {
            this.refuseGuarantee = refuseGuarantee;
            this.context = context;
        }

        public async Task<bool> HandleAsync(RefuseFinancialGuarantee message)
        {
            await
                refuseGuarantee.Refuse(new DecisionData(message.ImportNotificationId, message.DecisionDate),
                    message.Reason);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
