namespace EA.Iws.RequestHandlers.ImportNotificationAssessment.FinancialGuarantee
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportNotificationAssessment.FinancialGuarantee;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment.FinancialGuarantee;

    internal class CreateFinancialGuaranteeHandler : IRequestHandler<CreateFinancialGuarantee, bool>
    {
        private readonly IImportFinancialGuaranteeRepository financialGuaranteeRepository;
        private readonly ImportNotificationContext context;

        public CreateFinancialGuaranteeHandler(IImportFinancialGuaranteeRepository financialGuaranteeRepository, 
            ImportNotificationContext context)
        {
            this.financialGuaranteeRepository = financialGuaranteeRepository;
            this.context = context;
        }

        public async Task<bool> HandleAsync(CreateFinancialGuarantee message)
        {
            financialGuaranteeRepository.Add(new ImportFinancialGuarantee(message.ImportNotificationId, 
                message.ReceivedDate));

            await context.SaveChangesAsync();

            return true;
        }
    }
}
