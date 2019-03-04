namespace EA.Iws.RequestHandlers.Admin.Reports
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Admin.Reports;
    using Domain;
    using Domain.NotificationApplication;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.Admin.Reports;

    internal class GetProducerReportHandler : IRequestHandler<GetProducerReport, ProducerData[]>
    {
        private readonly IProducerRepository repository;
        private readonly IUserContext userContext;
        private readonly IInternalUserRepository internalUserRepository;

        public GetProducerReportHandler(IProducerRepository repository,
            IUserContext userContext,
            IInternalUserRepository internalUserRepository)
        {
            this.repository = repository;
            this.userContext = userContext;
            this.internalUserRepository = internalUserRepository;
        }

        public async Task<ProducerData[]> HandleAsync(GetProducerReport message)
        {
            var user = await internalUserRepository.GetByUserId(userContext.UserId);

            return
            (await
                repository.GetProducerReport(message.DateType, message.From, message.To, message.TextFieldType,
                    message.OperatorType, message.TextSearch, user.CompetentAuthority)).ToArray();
        }
    }
}
