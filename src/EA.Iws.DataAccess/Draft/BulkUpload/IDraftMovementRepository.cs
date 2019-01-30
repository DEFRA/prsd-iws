namespace EA.Iws.DataAccess.Draft.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;

    public interface IDraftMovementRepository
    {
        Task<Guid> Add(Guid notificationId, List<PrenotificationMovement> movements);
    }
}
