namespace EA.Iws.RequestHandlers.NotificationMovements
{
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.Movement;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements;

    internal class GetBasicMovementSummaryHandler : IRequestHandler<GetBasicMovementSummary, BasicMovementSummary>
    {
        private readonly INotificationMovementsSummaryRepository repository;
        private readonly IMapper mapper;

        public GetBasicMovementSummaryHandler(IMapper mapper,
            INotificationMovementsSummaryRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<BasicMovementSummary> HandleAsync(GetBasicMovementSummary message)
        {
            var summary = await repository.GetById(message.NotificationId);

            return mapper.Map<BasicMovementSummary>(summary);
        }
    }
}