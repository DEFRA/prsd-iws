namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.ImportMovement;

    public interface IImportMovementRepository
    {
        Task<ImportMovement> Get(Guid id);

        Task<ImportMovement> GetByNumberOrDefault(Guid importNotificationId, int number);

        Task<ImportMovement> GetPrenotifiedForNotificationByNumber(Guid importNotificationId, int number);

        Task<AddedCancellableImportMovementValidation> IsShipmentExistingInNonCancellableStatus(Guid importNotificationId, int number);

        Task<IEnumerable<ImportMovement>> GetForNotification(Guid importNotificationId);

        Task<IEnumerable<ImportMovement>> GetPrenotifiedForNotification(Guid importNotificationId);

        void Add(ImportMovement movement);

        Task<int> GetLatestMovementNumber(Guid notificationId);

        Task<bool> DeleteById(Guid movementId);

        Task<IEnumerable<ImportMovement>> GetRejectedMovements(Guid notificationId);

        Task SetImportMovementReceiptAndRecoveryData(ImportMovementSummaryData data, Guid createdBy);

        Task<IEnumerable<ImportMovement>> GetImportMovementsByIds(Guid notificationId, IEnumerable<Guid> movementIds);
    }
}
