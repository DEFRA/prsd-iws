namespace EA.Iws.RequestHandlers.Admin.Reports
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Admin.Reports;
    using Domain.Reports;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Admin.Reports;

    internal class GetExportStatsReportHandler : IRequestHandler<GetExportStatsReport, ExportStatsData[]>
    {
        private readonly IExportStatsRepository repository;
        private readonly IMapper mapper;

        public GetExportStatsReportHandler(IExportStatsRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<ExportStatsData[]> HandleAsync(GetExportStatsReport message)
        {
            var report = await repository.GetExportStats(message.Year);

            return report.Select(p => mapper.Map<ExportStatsData>(p)).ToArray();
        }
    }
}