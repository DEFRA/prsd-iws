namespace EA.Iws.Domain.Movement.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;

    public interface IDraftMovementRepository
    {
        Task<Guid> Add(Guid notificationId, List<PrenotificationMovement> movements, string fileName);

        Task<IEnumerable<DraftMovement>> GetDraftMovementById(Guid draftBulkUploadId);

        Task<bool> DeleteDraftMovementByNotificationId(Guid notificationId);
    }
}
