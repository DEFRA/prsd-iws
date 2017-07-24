namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using Prsd.Core;

    [AutoRegister]
    public class RejectMovement : IRejectMovement
    {
        private readonly IMovementRepository movementRepository;
        private readonly IMovementRejectionRepository movementRejectionRepository;

        public RejectMovement(IMovementRepository movementRepository, 
            IMovementRejectionRepository movementRejectionRepository)
        {
            this.movementRepository = movementRepository;
            this.movementRejectionRepository = movementRejectionRepository;
        }

        public async Task<MovementRejection> Reject(Guid movementId,
            DateTime rejectionDate,
            string reason)
        {
            var movement = await movementRepository.GetById(movementId);

            if (rejectionDate < movement.Date)
            {
                throw new InvalidOperationException("The when the waste was received date cannot be before the actual date of shipment.");
            }
            if (rejectionDate > SystemTime.UtcNow.Date)
            {
                throw new InvalidOperationException("The when the waste was received date cannot be in the future.");
            }

            var movementRejection = movement.Reject(rejectionDate,
                reason);

            movementRejectionRepository.Add(movementRejection);

            return movementRejection;
        } 
    }
}
