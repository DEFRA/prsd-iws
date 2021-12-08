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
        private readonly INotificationMovementsSummaryRepository summaryRepository;
        private readonly IFacilityRepository facilityRepository;
        private readonly IMovementRepository movementRepository;
        private readonly IMapper mapper;
        private const int PageSize = 30;

        public GetSummaryAndTableHandler(
            IFacilityRepository facilityRepository,
            INotificationMovementsSummaryRepository summaryRepository,
            IMovementRepository movementRepository,
            IMapper mapper)
        {
            this.facilityRepository = facilityRepository;
            this.movementRepository = movementRepository;
            this.mapper = mapper;
            this.summaryRepository = summaryRepository;
        }

        public async Task<NotificationMovementsSummaryAndTable> HandleAsync(GetSummaryAndTable message)
        {
            var isInterimNotification = (await facilityRepository.GetByNotificationId((message.Id))).IsInterim.GetValueOrDefault();
            var summaryData = await summaryRepository.GetById(message.Id);
            IEnumerable<Movement> notificationMovements;

            if (message.Status.HasValue)
            {
                notificationMovements = await movementRepository.GetPagedMovementsByStatus(message.Id, message.Status.Value, message.PageNumber, PageSize);
            }
            else
            {
                notificationMovements = await movementRepository.GetPagedMovements(message.Id, message.PageNumber, PageSize);
            }

            var data = mapper.Map<NotificationMovementsSummary, Movement[], NotificationMovementsSummaryAndTable>(summaryData, notificationMovements.ToArray());

            foreach (var movement in data.ShipmentTableData)
            {
                if (movement.Status == MovementStatus.Completed)
                {
                    if (!movement.CompletedDate.HasValue)
                    {
                        movement.CompletedDate = notificationMovements.Where(x => x.Id == movement.Id).ToList()[0].PartialRejection.ToList()[0].WasteDisposedDate;
                    }

                    if (!movement.Quantity.HasValue)
                    {
                        movement.Quantity = notificationMovements.Where(x => x.Id == movement.Id).ToList()[0].PartialRejection.ToList()[0].ActualQuantity;
                    }

                    if (!movement.QuantityUnits.HasValue)
                    {
                        movement.QuantityUnits = notificationMovements.Where(x => x.Id == movement.Id).ToList()[0].PartialRejection.ToList()[0].ActualUnit;
                    }

                    if (!movement.ReceivedDate.HasValue)
                    {
                        movement.ReceivedDate = notificationMovements.Where(x => x.Id == movement.Id).ToList()[0].PartialRejection.ToList()[0].WasteReceivedDate;
                    }
                }
                if (movement.Status == MovementStatus.PartiallyRejected)
                {
                    if (movement.CompletedDate == null)
                    {
                        movement.Quantity = notificationMovements.Where(x => x.Id == movement.Id).ToList()[0].PartialRejection.ToList()[0].ActualQuantity;
                        movement.QuantityUnits = notificationMovements.Where(x => x.Id == movement.Id).ToList()[0].PartialRejection.ToList()[0].ActualUnit;
                        movement.ReceivedDate = notificationMovements.Where(x => x.Id == movement.Id).ToList()[0].PartialRejection.ToList()[0].WasteReceivedDate;
                    }
                }
            }

            data.IsInterimNotification = isInterimNotification;

            data.PageSize = PageSize;
            data.PageNumber = message.PageNumber;
            data.NumberOfShipments = await movementRepository.GetTotalNumberOfMovements(message.Id, message.Status);

            return data;
        }
    }
}