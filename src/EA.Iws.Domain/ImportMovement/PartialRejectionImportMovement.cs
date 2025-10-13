namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using EA.Iws.Core.Shared;
    using Prsd.Core;

    [AutoRegister]
    public class PartialRejectionImportMovement : IPartialRejectionImportMovement
    {
        private readonly IImportMovementRepository movementRepository;
        private readonly IImportMovementPartailRejectionRepository importMovementPartailRejectionRepository;

        public PartialRejectionImportMovement(IImportMovementRepository movementRepository,
            IImportMovementPartailRejectionRepository importMovementPartailRejectionRepository)
        {
            this.movementRepository = movementRepository;
            this.importMovementPartailRejectionRepository = importMovementPartailRejectionRepository;
        }

        public async Task<ImportMovementPartialRejection> PartailReject(Guid movementId,
                                                     DateTime date,
                                                     string reason,
                                                     decimal actualQuantity,
                                                     ShipmentQuantityUnits actualUnit,
                                                     decimal rejectedQuantity,
                                                     ShipmentQuantityUnits rejectedUnit,
                                                     DateTime? wasteDisposedDate)
        {
            var movement = await movementRepository.Get(movementId);

            if (date > SystemTime.UtcNow.Date)
            {
                throw new InvalidOperationException("The when the waste was received date cannot be in the future.");
            }

            var partialRejection = movement.PartialReject(movementId, date, reason, actualQuantity, actualUnit, rejectedQuantity, rejectedUnit, wasteDisposedDate);

            importMovementPartailRejectionRepository.Add(partialRejection);

            return partialRejection;
        }
    }
}
