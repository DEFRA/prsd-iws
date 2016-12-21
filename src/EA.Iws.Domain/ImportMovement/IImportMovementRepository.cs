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

        Task<IEnumerable<ImportMovement>> GetPrenotifiedForNotification(Guid importNotificationId);

        void Add(ImportMovement movement);

        Task<int> GetLatestMovementNumber(Guid notificationId);

        Task<bool> DeleteById(Guid movementId);

        Task<IEnumerable<ImportMovement>> GetRejectedMovements(Guid notificationId);
    }
}
