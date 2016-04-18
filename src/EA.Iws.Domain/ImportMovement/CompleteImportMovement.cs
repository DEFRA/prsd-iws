namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;

    [AutoRegister]
    public class CompleteImportMovement : ICompleteImportMovement
    {
        private readonly IImportMovementRepository movementRepository;
        private readonly IImportMovementCompletedReceiptRepository completedReceiptRepository;

        public CompleteImportMovement(IImportMovementRepository movementRepository, 
            IImportMovementCompletedReceiptRepository completedReceiptRepository)
        {
            this.movementRepository = movementRepository;
            this.completedReceiptRepository = completedReceiptRepository;
        }

        public async Task<ImportMovementCompletedReceipt> Complete(Guid movementId, DateTime date)
        {
            var movement = await movementRepository.Get(movementId);

            var completedReceipt = movement.Complete(date);

            completedReceiptRepository.Add(completedReceipt);

            return completedReceipt;
        }
    }
}