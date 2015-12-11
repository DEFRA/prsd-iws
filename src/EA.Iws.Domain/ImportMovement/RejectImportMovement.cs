namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using System.Threading.Tasks;

    public class RejectImportMovement : IRejectImportMovement
    {
        private readonly IImportMovementRepository movementRepository;
        private readonly IImportMovementRejectionRepository rejectionRepository;

        public RejectImportMovement(IImportMovementRepository movementRepository,
            IImportMovementRejectionRepository rejectionRepository)
        {
            this.movementRepository = movementRepository;
            this.rejectionRepository = rejectionRepository;
        }

        public async Task<ImportMovementRejection> Reject(Guid importMovementId, DateTimeOffset date, string reason, string furtherDetails)
        {
            var movement = await movementRepository.Get(importMovementId);

            var rejection = movement.Reject(date, reason, furtherDetails);

            rejectionRepository.Add(rejection);

            return rejection;
        }
    }
}