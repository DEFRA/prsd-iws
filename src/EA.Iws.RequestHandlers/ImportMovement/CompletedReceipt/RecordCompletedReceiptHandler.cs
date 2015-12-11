namespace EA.Iws.RequestHandlers.ImportMovement.CompletedReceipt
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportMovement;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement.CompletedReceipt;

    public class RecordCompletedReceiptHandler : IRequestHandler<RecordCompletedReceipt, bool>
    {
        private readonly ICompleteImportMovement completeImportMovement;
        private readonly ImportNotificationContext context;

        public RecordCompletedReceiptHandler(ICompleteImportMovement completeImportMovement,
            ImportNotificationContext context)
        {
            this.completeImportMovement = completeImportMovement;
            this.context = context;
        }

        public async Task<bool> HandleAsync(RecordCompletedReceipt message)
        {
            await
                completeImportMovement.Complete(message.ImportMovementId,
                    new DateTimeOffset(message.Date, TimeSpan.Zero));

            await context.SaveChangesAsync();

            return true;
        }
    }
}
