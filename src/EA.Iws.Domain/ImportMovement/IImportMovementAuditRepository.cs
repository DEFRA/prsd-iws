namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IImportMovementAuditRepository
    {
        Task Add(ImportMovementAudit audit);

        Task<IEnumerable<ImportMovementAudit>> GetPagedShipmentAuditsById(Guid notificationId, int pageNumber,
            int pageSize, int? shipmentNumber);

        Task<int> GetTotalNumberOfShipmentAudits(Guid notificationId, int? shipmentNumber);
    }
}
