namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using Prsd.Core;

    [AutoRegister]
    public class CompleteImportMovement : ICompleteImportMovement
    {
        private readonly IImportMovementRepository movementRepository;
        private readonly IImportMovementCompletedReceiptRepository completedReceiptRepository;
        private readonly IImportMovementReceiptRepository movementReceiptRepository;

        public CompleteImportMovement(IImportMovementRepository movementRepository, 
            IImportMovementCompletedReceiptRepository completedReceiptRepository,
            IImportMovementReceiptRepository movementReceiptRepository)
        {
            this.movementRepository = movementRepository;
            this.completedReceiptRepository = completedReceiptRepository;
            this.movementReceiptRepository = movementReceiptRepository;
        }

        public async Task<ImportMovementCompletedReceipt> Complete(Guid movementId, DateTime date)
        {
            var movement = await movementRepository.Get(movementId);

            var movementReceipt = await movementReceiptRepository.GetByMovementIdOrDefault(movementId);

            if (date < movementReceipt.Date)
            {
                throw new InvalidOperationException("The when was the waste recovered date cannot be before the when was the waste received. ");
            }
            if (date > SystemTime.UtcNow.Date)
            {
                throw new InvalidOperationException("The when the waste was recovered date cannot be in the future.");
            }

            var completedReceipt = movement.Complete(date);

            completedReceiptRepository.Add(completedReceipt);

            return completedReceipt;
        }
    }
}