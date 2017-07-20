namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using Prsd.Core;

    [AutoRegister]
    public class ReceiveImportMovement : IReceiveImportMovement
    {
        private readonly IImportMovementRepository movementRepository;
        private readonly IImportMovementReceiptRepository receiptRepository;

        public ReceiveImportMovement(IImportMovementRepository movementRepository, IImportMovementReceiptRepository receiptRepository)
        {
            this.movementRepository = movementRepository;
            this.receiptRepository = receiptRepository;
        }

        public async Task<ImportMovementReceipt> Receive(Guid movementId, ShipmentQuantity quantity, DateTime date)
        {
            var movement = await movementRepository.Get(movementId);

            if (date < movement.ActualShipmentDate)
            {
                throw new InvalidOperationException("The when the waste was received date cannot be before the actual date of shipment.");
            }
            if (date > SystemTime.UtcNow.Date)
            {
                throw new InvalidOperationException("The when the waste was received date cannot be in the future.");
            }

            var receipt = movement.Receive(quantity, date);

            receiptRepository.Add(receipt);

            return receipt;
        }
    }
}