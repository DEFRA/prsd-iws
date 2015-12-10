namespace EA.Iws.RequestHandlers.ImportMovement.Receipt
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.ImportMovement;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement.Receipt;

    internal class RecordReceiptHandler : IRequestHandler<RecordReceipt, bool>
    {
        private readonly IImportMovementRepository movementRepository;
        private readonly IImportMovementReceiptRepository receiptRepository;
        private readonly ImportNotificationContext context;

        public RecordReceiptHandler(IImportMovementRepository movementRepository, 
            IImportMovementReceiptRepository receiptRepository,
            ImportNotificationContext context)
        {
            this.movementRepository = movementRepository;
            this.receiptRepository = receiptRepository;
            this.context = context;
        }

        public async Task<bool> HandleAsync(RecordReceipt message)
        {
            var movement = await movementRepository.Get(message.ImportMovementId);

            var receipt = movement.Receive(new ShipmentQuantity(message.Quantity, message.Unit), new DateTimeOffset(message.Date, TimeSpan.Zero));

            receiptRepository.Add(receipt);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
