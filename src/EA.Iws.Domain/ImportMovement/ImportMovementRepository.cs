namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using System.Threading.Tasks;

    internal class ImportMovementRepository : IImportMovementRepository
    {
        public Task<ImportMovement> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ImportMovement> GetByNumberOrDefault(Guid importNotificationId, int number)
        {
            throw new NotImplementedException();
        }
    }
}