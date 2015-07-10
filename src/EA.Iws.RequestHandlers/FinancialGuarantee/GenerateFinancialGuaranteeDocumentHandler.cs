namespace EA.Iws.RequestHandlers.FinancialGuarantee
{
    using System.Threading.Tasks;
    using Domain;
    using Prsd.Core.Mediator;
    using Requests.FinancialGuarantee;

    internal class GenerateFinancialGuaranteeDocumentHandler : IRequestHandler<GenerateFinancialGuaranteeDocument, byte[]>
    {
        private readonly IDocumentGenerator documentGenerator;

        public GenerateFinancialGuaranteeDocumentHandler(IDocumentGenerator documentGenerator)
        {
            this.documentGenerator = documentGenerator;
        }

        public Task<byte[]> HandleAsync(GenerateFinancialGuaranteeDocument query)
        {
            return Task.FromResult(documentGenerator.GenerateFinancialGuaranteeDocument());
        }
    }
}