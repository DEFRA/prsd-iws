namespace EA.Iws.RequestHandlers.Admin.Reports
{
    using System.Threading.Tasks;
    using Core.Admin.Reports;
    using Domain;
    using Domain.Reports;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.Admin.Reports;

    internal class GetExportMovementsReportHandler : IRequestHandler<GetExportMovementsReport, ExportMovementsData>
    {
        private readonly IInternalUserRepository internalUserRepository;
        private readonly IExportMovementsRepository repository;
        private readonly IUserContext userContext;

        public GetExportMovementsReportHandler(IExportMovementsRepository repository,
            IUserContext userContext,
            IInternalUserRepository internalUserRepository)
        {
            this.repository = repository;
            this.userContext = userContext;
            this.internalUserRepository = internalUserRepository;
        }

        public async Task<ExportMovementsData> HandleAsync(GetExportMovementsReport message)
        {
            var user = await internalUserRepository.GetByUserId(userContext.UserId);

            return await repository.Get(message.From, message.To, user.CompetentAuthority);
        }
    }
}