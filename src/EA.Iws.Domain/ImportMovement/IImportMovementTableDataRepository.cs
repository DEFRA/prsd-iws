namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IImportMovementTableDataRepository
    {
        Task<IEnumerable<MovementTableData>> GetById(Guid importNotificationId, int pageNumber, int pageSize);

        Task<IEnumerable<MovementTableData>> GetById(Guid importNotificationId);

        Task<int> GetTotalNumberOfShipments(Guid importNotificationId);
    }
}
