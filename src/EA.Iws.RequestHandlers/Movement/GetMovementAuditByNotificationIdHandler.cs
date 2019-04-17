namespace EA.Iws.RequestHandlers.Movement
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Notification.Audit;
    using Core.Shared;
    using DataAccess;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using Requests.Notification;

    internal class GetMovementAuditByNotificationIdHandler : IRequestHandler<GetMovementAuditByNotificationId, ShipmentAuditData>
    {
        private readonly IwsContext context;
        private readonly IMapper mapper;
        private readonly IMovementAuditRepository repository;
        private const int PageSize = 15;

        public GetMovementAuditByNotificationIdHandler(IwsContext context, IMapper mapper,
          IMovementAuditRepository repository)
        {
            this.context = context;
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<ShipmentAuditData> HandleAsync(GetMovementAuditByNotificationId message)
        {
            IEnumerable<MovementAudit> notificationAudits = await repository.GetPagedShipmentAuditsById(message.NotificationId, message.PageNumber, PageSize);

            var movementAuditTable = mapper.Map<IEnumerable<MovementAudit>, ShipmentAuditData>(notificationAudits);
            movementAuditTable.PageNumber = message.PageNumber;
            movementAuditTable.PageSize = PageSize;
            movementAuditTable.NumberOfShipmentAudits = await repository.GetTotalNumberOfShipmentAudits(message.NotificationId);

            return movementAuditTable;
        }
    }
}
