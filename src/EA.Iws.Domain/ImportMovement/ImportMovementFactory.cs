namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using System.Threading.Tasks;

    internal class ImportMovementFactory : IImportMovementFactory
    {
        private readonly IImportMovementNumberValidator numberValidator;

        public ImportMovementFactory(IImportMovementNumberValidator numberValidator)
        {
            this.numberValidator = numberValidator;
        }

        public Task<ImportMovement> Create(Guid notificationId, int number, DateTimeOffset actualShipmentDate, DateTimeOffset? prenotificationDate)
        {
            throw new NotImplementedException();
        }
    }
}