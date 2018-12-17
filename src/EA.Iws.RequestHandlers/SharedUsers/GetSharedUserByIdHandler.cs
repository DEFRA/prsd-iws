namespace EA.Iws.RequestHandlers.SharedUsers
{
    using EA.Iws.Core.Notification;
    using EA.Iws.Domain.NotificationApplication;
    using EA.Iws.Requests.SharedUsers;
    using EA.Prsd.Core.Mediator;
    using Prsd.Core.Mapper;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    internal class GetSharedUserByIdHandler : IRequestHandler<GetSharedUserById, NotificationSharedUser>
    {
        private readonly ISharedUserRepository repository;
        private readonly IMapper mapper;

        public GetSharedUserByIdHandler(ISharedUserRepository repository,
            IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<NotificationSharedUser> HandleAsync(GetSharedUserById message)
        {
            var sharedUser = await repository.GetSharedUserById(message.NotificationId, message.SharedId);

            return mapper.Map<NotificationSharedUser>(sharedUser);
        }
    }
}
