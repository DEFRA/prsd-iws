namespace EA.Iws.RequestHandlers.NotificationMovements
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements;

    internal class GetSummaryAndTableHandler : IRequestHandler<GetSummaryAndTable, NotificationMovementsSummaryAndTable>
    {
        private readonly INotificationApplicationRepository notificationApplicationRepository;
        private readonly INotificationMovementsSummaryRepository summaryRepository;
        private readonly IMovementRepository movementRepository;
        private readonly IMapper mapper;

        public GetSummaryAndTableHandler(
            INotificationApplicationRepository notificationApplicationRepository,
            INotificationMovementsSummaryRepository summaryRepository,
            IMovementRepository movementRepository,
            IMapper mapper)
        {
            this.notificationApplicationRepository = notificationApplicationRepository;
            this.movementRepository = movementRepository;
            this.mapper = mapper;
            this.summaryRepository = summaryRepository;
        }

        public async Task<NotificationMovementsSummaryAndTable> HandleAsync(GetSummaryAndTable message)
        {
            var isInterimNotification = (await notificationApplicationRepository.GetById(message.Id)).IsInterim;
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

            var data = mapper.Map<NotificationMovementsSummary, Movement[], NotificationMovementsSummaryAndTable>(summaryData, notificationMovements.ToArray());
            data.IsInterimNotification = isInterimNotification;

            return data;
        }
    }
}