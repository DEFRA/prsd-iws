namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;

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
            string reason,
            string furtherDetails = null)
        {
            var movement = await movementRepository.GetById(movementId);
            
            var movementRejection = movement.Reject(rejectionDate,
                reason,
                furtherDetails);

            movementRejectionRepository.Add(movementRejection);

            return movementRejection;
        } 
    }
}
