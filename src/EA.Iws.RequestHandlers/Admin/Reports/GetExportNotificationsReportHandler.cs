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

    internal class GetExportNotificationsReportHandler : IRequestHandler<GetExportNotificationsReport, DataExportNotificationData[]>
    {
        private readonly IExportNotificationsRepository missingShipmentsRepository;
        private readonly IMapWithParameter<DataExportNotification, UKCompetentAuthority, DataExportNotificationData> mapper;
        private readonly IUserContext userContext;
        private readonly IInternalUserRepository internalUserRepository;

        public GetExportNotificationsReportHandler(IExportNotificationsRepository missingShipmentsRepository,
            IMapWithParameter<DataExportNotification, UKCompetentAuthority, DataExportNotificationData> mapper,
            IUserContext userContext,
            IInternalUserRepository internalUserRepository)
        {
            this.missingShipmentsRepository = missingShipmentsRepository;
            this.mapper = mapper;
            this.userContext = userContext;
            this.internalUserRepository = internalUserRepository;
        }
        
        public async Task<DataExportNotificationData[]> HandleAsync(GetExportNotificationsReport message)
        {
            var user = await internalUserRepository.GetByUserId(userContext.UserId);
            var data = await missingShipmentsRepository.Get(message.From, message.To, user.CompetentAuthority);

            return data.Select(x => mapper.Map(x, user.CompetentAuthority)).ToArray();
        }
    }
}
