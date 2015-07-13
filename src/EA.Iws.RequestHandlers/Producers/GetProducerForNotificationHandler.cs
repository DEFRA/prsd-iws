namespace EA.Iws.RequestHandlers.Producers
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.Producers;
    using DataAccess;
    using Domain.Notification;
    using Mappings;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Producers;

    internal class GetProducerForNotificationHandler : IRequestHandler<GetProducerForNotification, ProducerData>
    {
        private readonly IwsContext context;
        private readonly IMapWithParentObjectId<Producer, ProducerData> mapper;

        public GetProducerForNotificationHandler(IwsContext context, IMapWithParentObjectId<Producer, ProducerData> mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<ProducerData> HandleAsync(GetProducerForNotification message)
        {
            var notification = await context.GetNotificationApplication(message.NotificationId);
            var producer = notification.GetProducer(message.ProducerId);

            return mapper.Map(producer, message.NotificationId);
        }
    }
}