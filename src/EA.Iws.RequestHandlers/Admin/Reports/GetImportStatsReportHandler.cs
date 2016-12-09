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

    internal class GetImportStatsReportHandler : IRequestHandler<GetImportStatsReport, ImportStatsData[]>
    {
        private readonly IImportStatsRepository importStatsRepository;
        private readonly IMapper mapper;
        private readonly IUserContext userContext;
        private readonly IInternalUserRepository internalUserRepository;

        public GetImportStatsReportHandler(IImportStatsRepository importStatsRepository, IMapper mapper, IUserContext userContext, IInternalUserRepository internalUserRepository)
        {
            this.importStatsRepository = importStatsRepository;
            this.mapper = mapper;
            this.userContext = userContext;
            this.internalUserRepository = internalUserRepository;
        }

        public async Task<ImportStatsData[]> HandleAsync(GetImportStatsReport message)
        {
            var user = await internalUserRepository.GetByUserId(userContext.UserId);
            var report = await importStatsRepository.GetImportStats(message.From, message.To, user.CompetentAuthority);

            return report.Select(p => mapper.Map<ImportStatsData>(p)).ToArray();
        }
    }
}