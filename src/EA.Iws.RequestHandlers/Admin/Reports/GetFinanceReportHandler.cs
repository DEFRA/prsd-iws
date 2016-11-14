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

    internal class GetFinanceReportHandler : IRequestHandler<GetFinanceReport, FinanceReportData[]>
    {
        private readonly IFinanceReportRepository financeReportRepository;
        private readonly IMapper mapper;
        private readonly IInternalUserRepository internalUserRepository;
        private readonly IUserContext userContext;

        public GetFinanceReportHandler(IFinanceReportRepository financeReportRepository, IMapper mapper,
            IInternalUserRepository internalUserRepository, IUserContext userContext)
        {
            this.financeReportRepository = financeReportRepository;
            this.mapper = mapper;
            this.internalUserRepository = internalUserRepository;
            this.userContext = userContext;
        }

        public async Task<FinanceReportData[]> HandleAsync(GetFinanceReport message)
        {
            var user = await internalUserRepository.GetByUserId(userContext.UserId);
            var report = await financeReportRepository.GetFinanceReport(message.From, message.To, user.CompetentAuthority);

            return report.Select(f => mapper.Map<FinanceReportData>(f)).ToArray();
        }
    }
}