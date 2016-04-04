namespace EA.Iws.RequestHandlers.Admin.Reports
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Admin.Reports;
    using Domain.Reports;
    using Prsd.Core.Mediator;
    using Requests.Admin.Reports;

    internal class GetRdfSrfWoodReportHandler : IRequestHandler<GetRdfSrfWoodReport, RdfSrfWoodData[]>
    {
        private readonly IRdfSrfWoodRepository repository;

        public GetRdfSrfWoodReportHandler(IRdfSrfWoodRepository repository)
        {
            this.repository = repository;
        }

        public async Task<RdfSrfWoodData[]> HandleAsync(GetRdfSrfWoodReport message)
        {
            return (await repository.Get(message.From, message.To, message.ChemicalComposition)).ToArray();
        }
    }
}