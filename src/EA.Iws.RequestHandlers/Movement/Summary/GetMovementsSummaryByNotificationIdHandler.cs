namespace EA.Iws.RequestHandlers.Movement.Summary
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.Movement;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Movement.Summary;

    internal class GetMovementsSummaryByNotificationIdHandler : IRequestHandler<GetMovementsSummaryByNotificationId, MovementSummaryData>
    {
        private readonly INotificationMovementsSummaryRepository summaryRepository;
        private readonly IMovementRepository movementRepository;
        private readonly IMapper mapper;

        public GetMovementsSummaryByNotificationIdHandler(
            INotificationMovementsSummaryRepository summaryRepository,
            IMovementRepository movementRepository,
            IMapper mapper)
        {
            this.movementRepository = movementRepository;
            this.mapper = mapper;
            this.summaryRepository = summaryRepository;
        }

        public async Task<MovementSummaryData> HandleAsync(GetMovementsSummaryByNotificationId message)
        {
            var summaryData = await summaryRepository.GetById(message.Id);
            IEnumerable<Movement> notificationMovements;

            if (message.Status.HasValue)
            {
                notificationMovements = await movementRepository.GetMovementsByStatus(message.Id, message.Status.Value);
            }
            else
            {
                notificationMovements = await movementRepository.GetAllMovements(message.Id);
            }

            return mapper.Map<NotificationMovementsSummary, Movement[], MovementSummaryData>(summaryData, notificationMovements.ToArray());
        }
    }
}