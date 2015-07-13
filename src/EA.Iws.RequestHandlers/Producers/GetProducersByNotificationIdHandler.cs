namespace EA.Iws.RequestHandlers.Producers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Producers;
    using DataAccess;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Producers;

    internal class GetProducersByNotificationIdHandler :
        IRequestHandler<GetProducersByNotificationId, IList<ProducerData>>
    {
        private readonly IwsContext context;
        private readonly IMap<NotificationApplication, IList<ProducerData>> mapper;

        public GetProducersByNotificationIdHandler(IwsContext context,
            IMap<NotificationApplication, IList<ProducerData>> mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IList<ProducerData>> HandleAsync(GetProducersByNotificationId message)
        {
            var notification = await context.GetNotificationApplication(message.NotificationId);

            return mapper.Map(notification);
        }
    }
}