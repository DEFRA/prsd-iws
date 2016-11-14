namespace EA.Iws.RequestHandlers.NotificationMovements
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.FinancialGuarantee;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements;

    internal class GetSummaryAndTableHandler : IRequestHandler<GetSummaryAndTable, NotificationMovementsSummaryAndTable>
    {
        private readonly INotificationMovementsSummaryRepository summaryRepository;
        private readonly IFacilityRepository facilityRepository;
        private readonly IMovementRepository movementRepository;
        private readonly IMapper mapper;
        private readonly IFinancialGuaranteeRepository financialGuaranteeRepository;

        public GetSummaryAndTableHandler(
            IFacilityRepository facilityRepository,
            INotificationMovementsSummaryRepository summaryRepository,
            IMovementRepository movementRepository,
            IMapper mapper,
            IFinancialGuaranteeRepository financialGuaranteeRepository)
        {
            this.facilityRepository = facilityRepository;
            this.movementRepository = movementRepository;
            this.mapper = mapper;
            this.summaryRepository = summaryRepository;
            this.financialGuaranteeRepository = financialGuaranteeRepository;
        }

        public async Task<NotificationMovementsSummaryAndTable> HandleAsync(GetSummaryAndTable message)
        {
            var finacialGuaranteeStatus = await financialGuaranteeRepository.GetStatusByNotificationId(message.Id);
            var isInterimNotification = (await facilityRepository.GetByNotificationId((message.Id))).IsInterim.GetValueOrDefault();
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
            data.FgStatus = finacialGuaranteeStatus;

            return data;
        }
    }
}