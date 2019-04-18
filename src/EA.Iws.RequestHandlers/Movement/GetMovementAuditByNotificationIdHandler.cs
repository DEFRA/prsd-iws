namespace EA.Iws.RequestHandlers.Movement
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Shared;
    using DataAccess;
    using Domain.Movement;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class GetMovementAuditByNotificationIdHandler : IRequestHandler<GetMovementAuditByNotificationId, ShipmentAuditData>
    {
        private readonly IMapper mapper;
        private readonly IMovementAuditRepository repository;
        private const int PageSize = 15;

        public GetMovementAuditByNotificationIdHandler(IMapper mapper, IMovementAuditRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<ShipmentAuditData> HandleAsync(GetMovementAuditByNotificationId message)
        {
            var notificationAudits =
                await
                    repository.GetPagedShipmentAuditsById(message.NotificationId, message.PageNumber, PageSize,
                        message.ShipmentNumber);

            var movementAuditTable = mapper.Map<IEnumerable<MovementAudit>, ShipmentAuditData>(notificationAudits);
            movementAuditTable.PageNumber = message.PageNumber;
            movementAuditTable.PageSize = PageSize;
            movementAuditTable.NumberOfShipmentAudits = await repository.GetTotalNumberOfShipmentAudits(message.NotificationId);

            return movementAuditTable;
        }
    }
}
