namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using System.Threading.Tasks;

    public interface IImportMovementRepository
    {
        Task<ImportMovement> Get(Guid id);

        Task<ImportMovement> GetByNumberOrDefault(Guid importNotificationId, int number);
    }
}
