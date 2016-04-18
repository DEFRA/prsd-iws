namespace EA.Iws.RequestHandlers.FinancialGuarantee
{
    using System.Threading.Tasks;
    using Domain;
    using Prsd.Core.Mediator;
    using Requests.FinancialGuarantee;

    internal class GenerateFinancialGuaranteeDocumentHandler : IRequestHandler<GenerateFinancialGuaranteeDocument, byte[]>
    {
        private readonly IFinancialGuaranteeDocumentGenerator financialGuaranteeDocumentGenerator;

        public GenerateFinancialGuaranteeDocumentHandler(IFinancialGuaranteeDocumentGenerator financialGuaranteeDocumentGenerator)
        {
            this.financialGuaranteeDocumentGenerator = financialGuaranteeDocumentGenerator;
        }

        public Task<byte[]> HandleAsync(GenerateFinancialGuaranteeDocument query)
        {
            return Task.FromResult(financialGuaranteeDocumentGenerator.GenerateFinancialGuaranteeDocument(query.CompetentAuthority));
        }
    }
}