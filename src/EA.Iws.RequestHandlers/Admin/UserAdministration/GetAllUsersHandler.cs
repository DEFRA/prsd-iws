namespace EA.Iws.RequestHandlers.Admin.UserAdministration
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Admin;
    using Domain;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Admin.UserAdministration;

    internal class GetAllUsersHandler : IRequestHandler<GetAllUsers, IEnumerable<ChangeUserData>>
    {
        private readonly INotificationUserRepository notificationUserRepository;
        private readonly IMap<IEnumerable<User>, IEnumerable<ChangeUserData>> mapper;

        public GetAllUsersHandler(INotificationUserRepository notificationUserRepository,
            IMap<IEnumerable<User>, IEnumerable<ChangeUserData>> mapper)
        {
            this.notificationUserRepository = notificationUserRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ChangeUserData>> HandleAsync(GetAllUsers message)
        {
            var users = await notificationUserRepository.GetAllUsers();

            return mapper.Map(users);
        }
    }
}
