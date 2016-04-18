namespace EA.Iws.RequestHandlers.Admin.UserAdministration
{
    using System.Threading.Tasks;
    using Core.Admin;
    using Domain;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Admin.UserAdministration;

    internal class GetUserByIdHandler : IRequestHandler<GetUserById, ChangeUserData>
    {
        private readonly INotificationUserRepository notificationUserRepository;
        private readonly IMap<User, ChangeUserData> mapper;

        public GetUserByIdHandler(INotificationUserRepository notificationUserRepository, IMap<User, ChangeUserData> mapper)
        {
            this.notificationUserRepository = notificationUserRepository;
            this.mapper = mapper;
        }

        public async Task<ChangeUserData> HandleAsync(GetUserById message)
        {
            var user = await notificationUserRepository.GetUserByUserId(message.UserId);

            return mapper.Map(user);
        }
    }
}
