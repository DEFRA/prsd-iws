namespace EA.Iws.RequestHandlers.ImportNotification.Producers
{
    using Domain.ImportNotification;
    using EA.Prsd.Core.Mediator;
    using Prsd.Core.Mapper;
    using Requests.ImportNotification.Producers;
    using System.Threading.Tasks;

    internal class GetProducerByImportNotificationIdHandler : IRequestHandler<GetProducerByImportNotificationId, Core.ImportNotification.Summary.Producer>
    {
        private readonly IProducerRepository producerRepository;
        private readonly IMapper mapper;

        public GetProducerByImportNotificationIdHandler(IProducerRepository producerRepository, IMapper mapper)
        {
            this.producerRepository = producerRepository;
            this.mapper = mapper;
        }

        public async Task<Core.ImportNotification.Summary.Producer> HandleAsync(GetProducerByImportNotificationId message)
        {
            var producer = await producerRepository.GetByNotificationId(message.ImportNotificationId);

            return mapper.Map<Core.ImportNotification.Summary.Producer>(producer);
        }
    }
}
