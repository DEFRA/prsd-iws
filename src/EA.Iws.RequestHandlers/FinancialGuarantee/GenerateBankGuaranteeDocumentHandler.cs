namespace EA.Iws.RequestHandlers.FinancialGuarantee
{
    using System.Threading.Tasks;
    using Domain;
    using Prsd.Core.Mediator;
    using Requests.FinancialGuarantee;

    public class GenerateBankGuaranteeDocumentHandler : IRequestHandler<GenerateBankGuaranteeDocument, byte[]>
    {
        private readonly IFinancialGuaranteeDocumentGenerator financialGuaranteeDocumentGenerator;

        public GenerateBankGuaranteeDocumentHandler(IFinancialGuaranteeDocumentGenerator financialGuaranteeDocumentGenerator)
        {
            this.financialGuaranteeDocumentGenerator = financialGuaranteeDocumentGenerator;
        }

        public Task<byte[]> HandleAsync(GenerateBankGuaranteeDocument message)
        {
            return Task.FromResult(financialGuaranteeDocumentGenerator.GenerateBankGuaranteeDocument());
        }
    }
}
