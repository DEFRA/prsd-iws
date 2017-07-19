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

    internal class GetFinancialGuaranteesReportHandler : IRequestHandler<GetFinancialGuaranteesReport, FinancialGuaranteesData[]>
    {
        private readonly IFinancialGuaranteesRepository financialGuaranteesRepository;
        private readonly IInternalUserRepository internalUserRepository;
        private readonly IUserContext userContext;

        public GetFinancialGuaranteesReportHandler(IFinancialGuaranteesRepository financialGuaranteesRepository, 
            IUserContext userContext,
            IInternalUserRepository internalUserRepository)
        {
            this.financialGuaranteesRepository = financialGuaranteesRepository;
            this.userContext = userContext;
            this.internalUserRepository = internalUserRepository;
        }

        public async Task<FinancialGuaranteesData[]> HandleAsync(GetFinancialGuaranteesReport message)
        {
            var user = await internalUserRepository.GetByUserId(userContext.UserId);

            return (await financialGuaranteesRepository.GetBlanketBonds(
                user.CompetentAuthority,
                message.FinancialGuaranteeReferenceNumber,
                message.ExporterName,
                message.ImporterName,
                message.ProducerName)).ToArray();
        }
    }
}