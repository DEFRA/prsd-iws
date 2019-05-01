namespace EA.Iws.RequestHandlers.Admin.Reports
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Admin.Reports;
    using Domain;
    using Domain.Reports;
    using EA.Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.Admin.Reports;

    internal class GetComplianceReportHandler : IRequestHandler<GetComplianceReport, ComplianceData[]>
    {
        private readonly IComplianceRepository repository;
        private readonly IUserContext userContext;
        private readonly IInternalUserRepository internalUserRepository;

        public GetComplianceReportHandler(IComplianceRepository repository,
            IUserContext userContext,
            IInternalUserRepository internalUserRepository)
        {
            this.repository = repository;
            this.userContext = userContext;
            this.internalUserRepository = internalUserRepository;
        }

        public async Task<ComplianceData[]> HandleAsync(GetComplianceReport message)
        {
            var user = await internalUserRepository.GetByUserId(userContext.UserId);

            return
            (await
                repository.GetComplianceReport(message.DateType, message.From, message.To, message.TextFieldType,
                    message.OperatorType, message.TextSearch, user.CompetentAuthority)).ToArray();
        }
    }
}
