namespace EA.Iws.Domain.Movement.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.BulkPrenotification;
    using Core.Movement.BulkReceiptRecovery;

    public interface IDraftMovementRepository
    {
        Task<Guid> AddPrenotifications(Guid notificationId, List<PrenotificationMovement> movements, string fileName);

        Task<Guid> AddReceiptRecovery(Guid notificationId, List<ReceiptRecoveryMovement> movements, string fileName);

        Task<IEnumerable<DraftMovement>> GetDraftMovementById(Guid draftBulkUploadId);

        Task<bool> DeleteDraftMovementByNotificationId(Guid notificationId);
    }
}
