namespace EA.Iws.RequestHandlers.ImportMovement
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Shared;
    using Domain.ImportMovement;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement;

    internal class GetImportMovementAuditByNotificationIdHandler : IRequestHandler<GetImportMovementAuditByNotificationId, ShipmentAuditData>
    {
        private readonly IMapper mapper;
        private readonly IImportMovementAuditRepository repository;
        private const int PageSize = 15;

        public GetImportMovementAuditByNotificationIdHandler(IMapper mapper,
            IImportMovementAuditRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<ShipmentAuditData> HandleAsync(GetImportMovementAuditByNotificationId message)
        {
            var notificationAudits =
                (await
                        repository.GetPagedShipmentAuditsById(message.NotificationId, message.PageNumber, PageSize,
                            message.ShipmentNumber))
                    .ToList();

            var movementAuditTable = mapper.Map<IEnumerable<ImportMovementAudit>, ShipmentAuditData>(notificationAudits);
            movementAuditTable.PageNumber = message.PageNumber;
            movementAuditTable.PageSize = PageSize;
            movementAuditTable.NumberOfShipmentAudits = notificationAudits.Count;

            return movementAuditTable;
        }
    }
}
