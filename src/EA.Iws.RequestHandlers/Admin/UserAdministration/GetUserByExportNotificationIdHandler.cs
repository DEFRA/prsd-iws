namespace EA.Iws.RequestHandlers.Admin.UserAdministration
{
    using System.Threading.Tasks;
    using Core.Admin;
    using Domain;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Admin.UserAdministration;

    internal class GetUserByExportNotificationIdHandler : IRequestHandler<GetUserByExportNotificationId, ChangeUserData>
    {
        private readonly INotificationUserRepository notificationUserRepository;
        private readonly IMap<User, ChangeUserData> mapper;

        public GetUserByExportNotificationIdHandler(INotificationUserRepository notificationUserRepository, IMap<User, ChangeUserData> mapper)
        {
            this.notificationUserRepository = notificationUserRepository;
            this.mapper = mapper;
        }

        public async Task<ChangeUserData> HandleAsync(GetUserByExportNotificationId message)
        {
            var user = await notificationUserRepository.GetUserByExportNotificationId(message.NotificationId);

            return mapper.Map(user);
        }
    }
}
