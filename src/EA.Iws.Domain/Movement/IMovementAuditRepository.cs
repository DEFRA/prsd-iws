namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMovementAuditRepository
    {
        Task Add(MovementAudit audit);

        Task<IEnumerable<MovementAudit>> GetPagedShipmentAuditsById(Guid notificationId, int pageNumber, int pageSize,
            int? shipmentNumber);

        Task<int> GetTotalNumberOfShipmentAudits(Guid notificationId, int? shipmentNumber);
    }
}
