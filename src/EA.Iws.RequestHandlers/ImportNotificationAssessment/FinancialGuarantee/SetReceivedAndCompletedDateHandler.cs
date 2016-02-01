namespace EA.Iws.RequestHandlers.ImportNotificationAssessment.FinancialGuarantee
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportNotificationAssessment.FinancialGuarantee;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment.FinancialGuarantee;

    internal class SetReceivedAndCompletedDateHandler : IRequestHandler<SetReceivedAndCompletedDate, bool>
    {
        private readonly IImportFinancialGuaranteeRepository financialGuaranteeRepository;
        private readonly ImportNotificationContext context;

        public SetReceivedAndCompletedDateHandler(IImportFinancialGuaranteeRepository financialGuaranteeRepository, 
            ImportNotificationContext context)
        {
            this.financialGuaranteeRepository = financialGuaranteeRepository;
            this.context = context;
        }

        public async Task<bool> HandleAsync(SetReceivedAndCompletedDate message)
        {
            var financialGuarantee =
                await financialGuaranteeRepository.GetByNotificationId(message.ImportNotificationId);

            financialGuarantee.UpdateReceivedDate(message.ReceivedDate);

            financialGuarantee.Complete(message.CompletedDate);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
