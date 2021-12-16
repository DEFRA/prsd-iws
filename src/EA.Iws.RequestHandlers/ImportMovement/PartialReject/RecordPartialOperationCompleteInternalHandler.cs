namespace EA.Iws.RequestHandlers.ImportMovement.PartialReject
{
    using System.Threading.Tasks;
    using DataAccess;
    using EA.Iws.Domain.ImportMovement;
    using EA.Iws.Requests.ImportMovement.PartialReject;
    using EA.Prsd.Core.Domain;
    using Prsd.Core.Mediator;

    internal class RecordPartialOperationCompleteInternalHandler : IRequestHandler<RecordPartialOperationCompleteInternal, bool>
    {
        private readonly IImportMovementPartailRejectionRepository movementPartialRepository;
        private readonly ImportNotificationContext context;
        private readonly IUserContext userContext;
        private readonly IImportMovementRepository importMovementRepository;

        public RecordPartialOperationCompleteInternalHandler(IImportMovementPartailRejectionRepository movementPartialRepository, ImportNotificationContext context, IUserContext userContext,
            IImportMovementRepository importMovementRepository)
        {
            this.movementPartialRepository = movementPartialRepository;
            this.context = context;
            this.userContext = userContext;
            this.importMovementRepository = importMovementRepository;
        }

        public async Task<bool> HandleAsync(RecordPartialOperationCompleteInternal message)
        {
            var movementPartial = await movementPartialRepository.GetByMovementId(message.Id);
            movementPartial.WasteDisposedDate = message.CompleteDate;

            await context.SaveChangesAsync();

            return true;
        }
    }
}
