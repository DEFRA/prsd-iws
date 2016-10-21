namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;

    [AutoRegister]
    public class CancelImportMovement
    {
        private readonly IImportMovementCompletedReceiptRepository completedReceiptRepository;
        private readonly IImportMovementRepository movementRepository;
        private readonly IImportMovementReceiptRepository receiptRepository;

        public CancelImportMovement(IImportMovementRepository movementRepository,
            IImportMovementReceiptRepository receiptRepository,
            IImportMovementCompletedReceiptRepository completedReceiptRepository)
        {
            this.movementRepository = movementRepository;
            this.receiptRepository = receiptRepository;
            this.completedReceiptRepository = completedReceiptRepository;
        }

        public async Task Cancel(Guid importMovementId)
        {
            var receipt = await receiptRepository.GetByMovementIdOrDefault(importMovementId);
            var completedReceipt = await completedReceiptRepository.GetByMovementIdOrDefault(importMovementId);

            if (receipt != null)
            {
                throw new InvalidOperationException(string.Format("Can't cancel movement {0} as it has been received", importMovementId));
            }

            if (completedReceipt != null)
            {
                throw new InvalidOperationException(string.Format("Can't cancel movement {0} as it has been recovered / disposed of", importMovementId));
            }

            var movement = await movementRepository.Get(importMovementId);

            movement.Cancel();
        } 
    }
}