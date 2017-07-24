namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using Prsd.Core;

    [AutoRegister]
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

        public async Task<ImportMovementRejection> Reject(Guid importMovementId, DateTime date, string reason)
        {
            var movement = await movementRepository.Get(importMovementId);

            if (date < movement.ActualShipmentDate)
            {
                throw new InvalidOperationException("The when the waste was received date cannot be before the actual date of shipment.");
            }
            if (date > SystemTime.UtcNow.Date)
            {
                throw new InvalidOperationException("The when the waste was received date cannot be in the future.");
            }

            var rejection = movement.Reject(date, reason);

            rejectionRepository.Add(rejection);

            return rejection;
        }
    }
}