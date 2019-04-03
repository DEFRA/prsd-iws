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

    internal class GetShipmentsReportHandler : IRequestHandler<GetShipmentsReport, ShipmentData[]>
    {
        private readonly IShipmentsRepository shipmentsRepository;
        private readonly IMapWithParameter<Shipment, UKCompetentAuthority, ShipmentData> mapper;
        private readonly IUserContext userContext;
        private readonly Domain.IInternalUserRepository internalUserRepository;

        public GetShipmentsReportHandler(IShipmentsRepository shipmentsRepository,
            IMapWithParameter<Shipment, UKCompetentAuthority, ShipmentData> mapper,
            IUserContext userContext,
            Domain.IInternalUserRepository internalUserRepository)
        {
            this.shipmentsRepository = shipmentsRepository;
            this.mapper = mapper;
            this.userContext = userContext;
            this.internalUserRepository = internalUserRepository;
        }

        public async Task<ShipmentData[]> HandleAsync(GetShipmentsReport message)
        {
            var user = await internalUserRepository.GetByUserId(userContext.UserId);
            var data =
                await
                    shipmentsRepository.Get(message.From, message.To, user.CompetentAuthority, message.DateType,
                        message.TextFieldType, message.TextFieldOperatorType, message.TextSearch);

            return data.Select(x => mapper.Map(x, user.CompetentAuthority)).ToArray();
        }
    }
}
