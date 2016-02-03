namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using System.Threading.Tasks;
    using Core.ImportNotificationMovements;

    public interface IImportMovementsSummaryRepository
    {
        Task<Summary> GetById(Guid importNotificationId);
    }
}
