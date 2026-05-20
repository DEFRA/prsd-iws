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

    internal class GetImportNotificationsReportHandler : IRequestHandler<GetImportNotificationsReport, DataImportNotificationData[]>
    {
        private readonly IImportNotificationsRepository importNotificationsRepository;
        private readonly IMapWithParameter<DataImportNotification, UKCompetentAuthority, DataImportNotificationData> mapper;
        private readonly IUserContext userContext;
        private readonly Domain.IInternalUserRepository internalUserRepository;

        public GetImportNotificationsReportHandler(IImportNotificationsRepository importNotificationsRepository,
            IMapWithParameter<DataImportNotification, UKCompetentAuthority, DataImportNotificationData> mapper,
            IUserContext userContext,
            Domain.IInternalUserRepository internalUserRepository)
        {
            this.importNotificationsRepository = importNotificationsRepository;
            this.mapper = mapper;
            this.userContext = userContext;
            this.internalUserRepository = internalUserRepository;
        }
        
        public async Task<DataImportNotificationData[]> HandleAsync(GetImportNotificationsReport message)
        {
            var user = await internalUserRepository.GetByUserId(userContext.UserId);
            var data = await importNotificationsRepository.Get(message.From, message.To, user.CompetentAuthority);

            return data.Select(x => mapper.Map(x, user.CompetentAuthority)).ToArray();
        }
    }
}
