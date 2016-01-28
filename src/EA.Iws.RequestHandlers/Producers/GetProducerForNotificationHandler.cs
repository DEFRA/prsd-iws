namespace EA.Iws.RequestHandlers.Producers
{
    using System.Threading.Tasks;
    using Core.Producers;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Producers;

    internal class GetProducerForNotificationHandler : IRequestHandler<GetProducerForNotification, ProducerData>
    {
        private readonly IProducerRepository repository;
        private readonly IwsContext context;
        private readonly IMapWithParentObjectId<Producer, ProducerData> mapper;

        public GetProducerForNotificationHandler(IwsContext context,
            IMapWithParentObjectId<Producer, ProducerData> mapper,
            IProducerRepository repository)
        {
            this.context = context;
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<ProducerData> HandleAsync(GetProducerForNotification message)
        {
            var producers = await repository.GetByNotificationId(message.NotificationId);

            var producer = producers.GetProducer(message.ProducerId);

            return mapper.Map(producer, message.NotificationId);
        }
    }
}