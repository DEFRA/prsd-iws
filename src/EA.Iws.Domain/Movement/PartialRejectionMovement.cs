namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using EA.Iws.Core.Shared;
    using Prsd.Core;

    [AutoRegister]
    public class PartialRejectionMovement : IPartialRejectionMovement
    {
        private readonly IMovementRepository movementRepository;
        private readonly IMovementPartialRejectionRepository movementPartialRejectionRepository;

        public PartialRejectionMovement(IMovementRepository movementRepository,
            IMovementPartialRejectionRepository movementPartialRejectionRepository)
        {
            this.movementRepository = movementRepository;
            this.movementPartialRejectionRepository = movementPartialRejectionRepository;
        }

        public async Task<MovementPartialRejection> PartailReject(Guid movementId,
                                                     DateTime date,
                                                     string reason,
                                                     decimal actualQuantity,
                                                     ShipmentQuantityUnits actualUnit,
                                                     decimal rejectedQuantity,
                                                     ShipmentQuantityUnits rejectedUnit,
                                                     DateTime? wasteDisposedDate)
        {
            var movement = await movementRepository.GetById(movementId);

            if (date < movement.Date)
            {
                throw new InvalidOperationException("The when the waste was received date cannot be before the actual date of shipment.");
            }
            if (date > SystemTime.UtcNow.Date)
            {
                throw new InvalidOperationException("The when the waste was received date cannot be in the future.");
            }

            var movementPartialRejection = movement.PartialReject(movementId, date, reason, actualQuantity, actualUnit, rejectedQuantity, rejectedUnit, wasteDisposedDate.Value);

            movementPartialRejectionRepository.Add(movementPartialRejection);

            return movementPartialRejection;
        }
    }
}
