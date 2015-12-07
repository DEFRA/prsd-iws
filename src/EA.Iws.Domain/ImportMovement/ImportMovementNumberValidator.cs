namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class ImportMovementNumberValidator : IImportMovementNumberValidator
    {
        private readonly IImportMovementRepository movementRepository;

        public ImportMovementNumberValidator(IImportMovementRepository movementRepository)
        {
            this.movementRepository = movementRepository;
        }

        public async Task<bool> Validate(Guid notificationId, int number)
        {
            var movements = await movementRepository.GetForNotification(notificationId);

            return movements.All(m => m.Number != number);
        }
    }
}