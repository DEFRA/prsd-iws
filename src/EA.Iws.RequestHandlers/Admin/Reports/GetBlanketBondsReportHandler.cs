namespace EA.Iws.RequestHandlers.Admin.Reports
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Admin.Reports;
    using Domain;
    using Domain.Reports;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.Admin.Reports;

    internal class GetBlanketBondsReportHandler : IRequestHandler<GetBlanketBondsReport, BlanketBondsData[]>
    {
        private readonly IBlanketBondsRepository blanketBondsRepository;
        private readonly IInternalUserRepository internalUserRepository;
        private readonly IUserContext userContext;

        public GetBlanketBondsReportHandler(IBlanketBondsRepository blanketBondsRepository, 
            IUserContext userContext,
            IInternalUserRepository internalUserRepository)
        {
            this.blanketBondsRepository = blanketBondsRepository;
            this.userContext = userContext;
            this.internalUserRepository = internalUserRepository;
        }

        public async Task<BlanketBondsData[]> HandleAsync(GetBlanketBondsReport message)
        {
            var user = await internalUserRepository.GetByUserId(userContext.UserId);

            return (await blanketBondsRepository.GetBlanketBonds(
                user.CompetentAuthority,
                message.FinancialGuaranteeReferenceNumber,
                message.ExporterName,
                message.ImporterName,
                message.ProducerName)).ToArray();
        }
    }
}