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

    internal class GetSharedUsersByNotificationIdHandler :
        IRequestHandler<GetSharedUsersByNotificationId, IList<NotificationSharedUser>>
    {
        private readonly ISharedUserRepository repository;
        private readonly IMapper mapper;

        public GetSharedUsersByNotificationIdHandler(ISharedUserRepository repository,
            IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<IList<NotificationSharedUser>> HandleAsync(GetSharedUsersByNotificationId message)
        {
            var sharedUsers = await repository.GetAllSharedUsers(message.NotificationId);
             
            return sharedUsers
               .Select(sharedUser => mapper.Map<NotificationSharedUser>(sharedUser))
               .ToList();           
        }
    }
}
