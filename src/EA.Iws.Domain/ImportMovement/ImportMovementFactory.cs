namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using Movement;

    [AutoRegister]
    public class ImportMovementFactory : IImportMovementFactory
    {
        private readonly IImportMovementNumberValidator numberValidator;

        public ImportMovementFactory(IImportMovementNumberValidator numberValidator)
        {
            this.numberValidator = numberValidator;
        }

        public async Task<ImportMovement> Create(Guid notificationId, int number, DateTime actualShipmentDate)
        {
            if (!await numberValidator.Validate(notificationId, number))
            {
                throw new MovementNumberException("Cannot create an import movement with a conflicting movement number (" + number + ") for import notification: " + notificationId);
            }

            return new ImportMovement(notificationId, number, actualShipmentDate);
        }
    }
}