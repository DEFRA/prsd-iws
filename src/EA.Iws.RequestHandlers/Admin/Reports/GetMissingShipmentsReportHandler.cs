namespace EA.Iws.RequestHandlers.Admin.Reports
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Admin.Reports;
    using Core.Notification;
    using Domain.Reports;
    using Prsd.Core.Domain;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Admin.Reports;

    internal class GetMissingShipmentsReportHandler : IRequestHandler<GetMissingShipmentsReport, MissingShipmentData[]>
    {
        private readonly IMissingShipmentsRepository missingShipmentsRepository;
        private readonly IMapWithParameter<MissingShipment, UKCompetentAuthority, MissingShipmentData> mapper;
        private readonly IUserContext userContext;
        private readonly Domain.IInternalUserRepository internalUserRepository;

        public GetMissingShipmentsReportHandler(IMissingShipmentsRepository missingShipmentsRepository,
            IMapWithParameter<MissingShipment, UKCompetentAuthority, MissingShipmentData> mapper,
            IUserContext userContext,
            Domain.IInternalUserRepository internalUserRepository)
        {
            this.missingShipmentsRepository = missingShipmentsRepository;
            this.mapper = mapper;
            this.userContext = userContext;
            this.internalUserRepository = internalUserRepository;
        }

        public async Task<MissingShipmentData[]> HandleAsync(GetMissingShipmentsReport message)
        {
            var user = await internalUserRepository.GetByUserId(userContext.UserId);
            var data = await missingShipmentsRepository.Get(message.From, message.To, user.CompetentAuthority);

            return data.Select(x => mapper.Map(x, user.CompetentAuthority)).ToArray();
        }
    }
}
