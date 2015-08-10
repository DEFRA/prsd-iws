namespace EA.Iws.RequestHandlers.FinancialGuarantee
{
    using System.Threading.Tasks;
    using Domain;
    using Prsd.Core.Mediator;
    using Requests.FinancialGuarantee;

    public class GenerateParentCompanyDocumentHandler : IRequestHandler<GenerateParentCompanyDocument, byte[]>
    {
        private readonly IFinancialGuaranteeDocumentGenerator financialGuaranteeDocumentGenerator;

        public GenerateParentCompanyDocumentHandler(IFinancialGuaranteeDocumentGenerator financialGuaranteeDocumentGenerator)
        {
            this.financialGuaranteeDocumentGenerator = financialGuaranteeDocumentGenerator;
        }

        public Task<byte[]> HandleAsync(GenerateParentCompanyDocument message)
        {
            return Task.FromResult(financialGuaranteeDocumentGenerator.GenerateParentCompanyTemplate());
        }
    }
}
