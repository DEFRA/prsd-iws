namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IImportMovementRepository
    {
        Task<ImportMovement> Get(Guid id);

        Task<ImportMovement> GetByNumberOrDefault(Guid importNotificationId, int number);

        Task<IEnumerable<ImportMovement>> GetForNotification(Guid importNotificationId);

        void Add(ImportMovement movement);
    }
}
