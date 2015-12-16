namespace EA.Iws.RequestHandlers.Admin.Reports
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Admin.Reports;
    using Domain.Reports;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Admin.Reports;

    internal class GetFinanceReportHandler : IRequestHandler<GetFinanceReport, FinanceReportData[]>
    {
        private readonly IFinanceReportRepository financeReportRepository;
        private readonly IMapper mapper;

        public GetFinanceReportHandler(IFinanceReportRepository financeReportRepository, IMapper mapper)
        {
            this.financeReportRepository = financeReportRepository;
            this.mapper = mapper;
        }

        public async Task<FinanceReportData[]> HandleAsync(GetFinanceReport message)
        {
            var report = await financeReportRepository.GetFinanceReport(message.EndDate);

            return report.Select(f => mapper.Map<FinanceReportData>(f)).ToArray();
        }
    }
}