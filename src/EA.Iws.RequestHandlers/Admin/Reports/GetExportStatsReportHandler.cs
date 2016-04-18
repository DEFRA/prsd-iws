namespace EA.Iws.RequestHandlers.Admin.Reports
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Admin.Reports;
    using Domain;
    using Domain.Reports;
    using Prsd.Core.Domain;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Admin.Reports;

    internal class GetExportStatsReportHandler : IRequestHandler<GetExportStatsReport, ExportStatsData[]>
    {
        private readonly IExportStatsRepository exportStatsRepository;
        private readonly IMapper mapper;
        private readonly IUserContext userContext;
        private readonly IInternalUserRepository internalUserRepository;

        public GetExportStatsReportHandler(IExportStatsRepository exportStatsRepository, IMapper mapper, IUserContext userContext, IInternalUserRepository internalUserRepository)
        {
            this.exportStatsRepository = exportStatsRepository;
            this.mapper = mapper;
            this.userContext = userContext;
            this.internalUserRepository = internalUserRepository;
        }

        public async Task<ExportStatsData[]> HandleAsync(GetExportStatsReport message)
        {
            var user = await internalUserRepository.GetByUserId(userContext.UserId);
            var report = await exportStatsRepository.GetExportStats(message.Year, user.CompetentAuthority);

            return report.Select(p => mapper.Map<ExportStatsData>(p)).ToArray();
        }
    }
}