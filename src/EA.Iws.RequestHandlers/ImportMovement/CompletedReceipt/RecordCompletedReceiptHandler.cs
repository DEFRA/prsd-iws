namespace EA.Iws.RequestHandlers.ImportMovement.CompletedReceipt
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportMovement;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement.CompletedReceipt;

    public class RecordCompletedReceiptHandler : IRequestHandler<RecordCompletedReceipt, bool>
    {
        private readonly ICompleteImportMovement completeImportMovement;
        private readonly ImportNotificationContext context;
        private readonly IImportMovementCompletedReceiptRepository completedReceiptRepository;

        public RecordCompletedReceiptHandler(ICompleteImportMovement completeImportMovement,
            ImportNotificationContext context,
            IImportMovementCompletedReceiptRepository completedReceiptRepository)
        {
            this.completeImportMovement = completeImportMovement;
            this.context = context;
            this.completedReceiptRepository = completedReceiptRepository;
        }

        public async Task<bool> HandleAsync(RecordCompletedReceipt message)
        {
            var completedReceipt = await completedReceiptRepository.GetByMovementIdOrDefault(message.ImportMovementId);
            if (completedReceipt == null)
            {
                await completeImportMovement.Complete(message.ImportMovementId, message.Date);
                await context.SaveChangesAsync();
            }

            return true;
        }
    }
}
