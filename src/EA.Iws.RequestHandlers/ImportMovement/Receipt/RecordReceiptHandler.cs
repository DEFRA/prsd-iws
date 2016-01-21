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
        private readonly IReceiveImportMovement receiveImportMovement;
        private readonly ImportNotificationContext context;

        public RecordReceiptHandler(IReceiveImportMovement receiveImportMovement,
            ImportNotificationContext context)
        {
            this.receiveImportMovement = receiveImportMovement;
            this.context = context;
        }

        public async Task<bool> HandleAsync(RecordReceipt message)
        {
            await receiveImportMovement.Receive(message.ImportMovementId, new ShipmentQuantity(message.Quantity, message.Unit), message.Date);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
