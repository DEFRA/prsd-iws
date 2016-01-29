namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IImportMovementTableDataRepository
    {
        Task<IEnumerable<MovementTableData>> GetById(Guid importNotificationId);
    }
}
