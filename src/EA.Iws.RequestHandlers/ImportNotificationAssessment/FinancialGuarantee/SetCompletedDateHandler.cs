namespace EA.Iws.RequestHandlers.ImportNotificationAssessment.FinancialGuarantee
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportNotificationAssessment.FinancialGuarantee;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment.FinancialGuarantee;

    internal class SetCompletedDateHandler : IRequestHandler<SetCompletedDate, bool>
    {
        private readonly IImportFinancialGuaranteeRepository financialGuaranteeRepository;
        private readonly ImportNotificationContext context;

        public SetCompletedDateHandler(IImportFinancialGuaranteeRepository financialGuaranteeRepository, 
            ImportNotificationContext context)
        {
            this.financialGuaranteeRepository = financialGuaranteeRepository;
            this.context = context;
        }

        public async Task<bool> HandleAsync(SetCompletedDate message)
        {
            var guarantee = await financialGuaranteeRepository.GetByNotificationId(message.ImportNotificationId);

            guarantee.Complete(message.Date);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
