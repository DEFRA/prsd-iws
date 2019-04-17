namespace EA.Iws.RequestHandlers.ImportMovement
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Shared;
    using DataAccess;
    using Domain.ImportMovement;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement;
    internal class GetImportMovementAuditByNotificationIdHandler : IRequestHandler<GetImportMovementAuditByNotificationId, ShipmentAuditData>
    {
        private readonly IwsContext context;
        private readonly IMapper mapper;
        private readonly IImportMovementAuditRepository repository;
        private const int PageSize = 15;

        public GetImportMovementAuditByNotificationIdHandler(IwsContext context, IMapper mapper,
          IImportMovementAuditRepository repository)
        {
            this.context = context;
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<ShipmentAuditData> HandleAsync(GetImportMovementAuditByNotificationId message)
        {
            IEnumerable<ImportMovementAudit> notificationAudits = await repository.GetPagedShipmentAuditsById(message.NotificationId, message.PageNumber, PageSize);

            var movementAuditTable = mapper.Map<IEnumerable<ImportMovementAudit>, ShipmentAuditData>(notificationAudits);
            movementAuditTable.PageNumber = message.PageNumber;
            movementAuditTable.PageSize = PageSize;
            movementAuditTable.NumberOfShipmentAudits = await repository.GetTotalNumberOfShipmentAudits(message.NotificationId);

            return movementAuditTable;
        }
    }
}
