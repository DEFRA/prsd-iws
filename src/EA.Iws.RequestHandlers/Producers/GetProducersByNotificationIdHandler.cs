namespace EA.Iws.RequestHandlers.Producers
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Notification;
    using Mappings;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Producers;

    internal class GetProducersByNotificationIdHandler :
        IRequestHandler<GetProducersByNotificationId, IList<ProducerData>>
    {
        private readonly IwsContext context;
        private readonly IMap<NotificationApplication, IList<ProducerData>> mapper;

        public GetProducersByNotificationIdHandler(IwsContext context, IMap<NotificationApplication, IList<ProducerData>> mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IList<ProducerData>> HandleAsync(GetProducersByNotificationId message)
        {
            var notification = await context.NotificationApplications.Where(n => n.Id == message.NotificationId).SingleAsync();

            return mapper.Map(notification);
        }
    }
}