namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using System.Threading.Tasks;

    public class ReceiveImportMovement : IReceiveImportMovement
    {
        private readonly IImportMovementRepository movementRepository;
        private readonly IImportMovementReceiptRepository receiptRepository;

        public ReceiveImportMovement(IImportMovementRepository movementRepository, IImportMovementReceiptRepository receiptRepository)
        {
            this.movementRepository = movementRepository;
            this.receiptRepository = receiptRepository;
        }

        public async Task<ImportMovementReceipt> Receive(Guid movementId, ShipmentQuantity quantity, DateTimeOffset date)
        {
            var movement = await movementRepository.Get(movementId);

            var receipt = movement.Receive(quantity, date);

            receiptRepository.Add(receipt);

            return receipt;
        }
    }
}